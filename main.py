# -*- coding: utf-8 -*-
from camunda.external_task.external_task import ExternalTask, TaskResult
from camunda.external_task.external_task_worker import ExternalTaskWorker
from sqlalchemy import *
from sqlalchemy.schema import DropTable, CreateTable, CreateIndex
from sqlalchemy.exc import IntegrityError
from sqlalchemy.sql.expression import func
import json
import hashlib
import re
from app_logger import logger
import logging
import multiprocessing as mp
import time
import sys
from flask import Flask, Response
import prometheus_client
from prometheus_client import Gauge, Histogram, Counter, Summary
import os
import dns.name
import dns.message
import dns.query
import dns.flags

logger.info(f'Start service version=0.0.8 ({__name__})')

def getenv(cfg):
    cfg['maxTasks']             = 1 if 'MAX_TASKS' not in os.environ else int(os.environ['MAX_TASKS'])
    cfg['lockDuration']         = 10000 if 'LOCK_DURATION' not in os.environ else int(os.environ['LOCK_DURATION'])
    cfg['asyncResponseTimeout'] = 5000 if 'ASYNC_RESPONCE_TIMEOUT' not in os.environ else int(os.environ['ASYNC_RESPONCE_TIMEOUT'])
    cfg['retries']              = 3 if 'RETRIES' not in os.environ else int(os.environ['RETRIES'])
    cfg['retryTimeout']         = 5000 if 'RETRY_TIMEOUT' not in os.environ else int(os.environ['RETRY_TIMEOUT'])
    cfg['sleepSeconds']         = 30 if 'SLEEP_SECONDS' not in os.environ else int(os.environ['SLEEP_SECONDS'])
    cfg['DBDriver']             = 'postgresql+psycopg2' if 'DBDRIVER' not in os.environ else  os.environ['DBDRIVER']
    cfg['DBUser']               = 'md' if 'DBUSER' not in os.environ else os.environ['DBUSER']
    cfg['DBPassword']           = 'rav1234' if 'DBPASSWORD' not in os.environ else os.environ['DBPASSWORD']
    cfg['DSN']                  = 'master.pgsqlanomaly01.service.consul:5432/fpdb' if 'DSN' not in os.environ else os.environ['DSN']
    cfg['CONSUL_ADDR']          = '10.74.30.22' if 'CONSUL_ADDR' not in os.environ else os.environ['CONSUL_ADDR']
    cfg['CAMUNDA_NAME']         = 'camunda.service.dc1.consul' if 'CAMUNDA_NAME' not in os.environ else os.environ['CAMUNDA_NAME']
    cfg['TOPIC']                = 'TEST' if 'TOPIC' not in os.environ else os.environ['TOPIC'] #'LoginDB'
#    cfg[''] =  if '' not in os.environ else  os.environ['']
    return cfg

def get_camunda_address(cfg):
    """
    Выдает строку коннекта к camunda сервису на основании его DNS имени
    """
    ADDITIONAL_RDCLASS = 65535

    domain = dns.name.from_text(cfg['CAMUNDA_NAME'])
    if not domain.is_absolute():
        domain = domain.concatenate(dns.name.root)

    request = dns.message.make_query(domain, dns.rdatatype.SRV)
    request.flags |= dns.flags.AD
    request.find_rrset(request.additional, dns.name.root, ADDITIONAL_RDCLASS, dns.rdatatype.OPT, create=True, force_unique=True)
    response = dns.query.udp(q=request, where=cfg['CONSUL_ADDR'], port=8600)
    val = response.answer[0].to_text().strip().rstrip('.').split(' ')
    port = val[6]
    val = response.additional[0].to_text().strip().rstrip('.').split(' ')
    ip = val[4]
    logger.info(f'Camunda has address = {ip}:{port}')
#    return 'http://' + ip + ':' + port + '/engine-rest'
    return 'http://' + cfg['CAMUNDA_NAME'] + ':' + port + '/engine-rest'


def web(h, st):
    """
    Отвечает за выдачу Метрик и HealthCheck
    """
    logger.info(f'Start WEB component (Health & Metrics)')
    log = logging.getLogger('werkzeug')
    log.disabled = True
    app = Flask(__name__)

    @app.route("/health")
    def health():
        if h.value == 1:
            return f"Error ({h.value})!!!", 500
        else:
            return f"OK ({h.value})"

    @app.route("/metrics")
    def metrics():
        delta = time.time() - st.value
#        print(f'{delta} = {time.time()} - {st.value}')
        uptime['service_uptime'].set(delta)  # Increment by given value
        os.environ['PATH_INFO'] = '/metrics'
        ccc = metrics_app(os.environ, start_fn)
        return Response((ccc[0]).decode('utf-8'), status=200, mimetype='text/plain')

    def start_fn(a, b):
        return

    uptime = {}
    metrics_app = prometheus_client.make_wsgi_app()
    uptime['service_uptime'] = Gauge('service_uptime', 'External Task Service Uptime')
    app.run(host='0.0.0.0', port=5000)


def db2db(task: ExternalTask) -> TaskResult:
    """
    Основной процесс, который вызывается по каждому заданию с определенным топиком, подписка идет в main

     Параметры Camunda task:
     Oper      : Refill - сделать Truncate Target tables и только затем залить в неё данные
                 Recreate - пересоздать Target tables, если hash в MD_Camunda_Hash отличается
                 ExecSQL - Выполнить SQL на SName и вернуть результаты в виде JSON
                 CloseAction - закрыть задачу на SName в app_actions. Если есть непустой параметр ErrorMessage,
                               то задача помечается как error. Поиск задачи в app_actions происходит по
                               app_actions.camunda_task_id=task.get_process_instance_id()
     SName     : md_src.name источника данных (из него возьмем SDriver, SDSN, SLogin, SPassword).
                 Например: 'DummySystem3'
     TName     : md_src.name целевой БД (из него возьмем TDriver, TDSN, TLogin, TPassword)
                 Например: 'DATAMART'
     SQLTable  : Target таблицы массив JSON. Порядок важен, относительно него заполняются таблицы, связанные ключами.
                 Например: '[{"Table":"branch1","Columns":[{"ind":18,"Name":"phone3","Type":"VARCHAR"},{"ind":19,"Name":"phone2","Type":"VARCHAR"},{"ind":20,"Name":"phone1","Type":"VARCHAR"},{"ind":21,"Name":"address3","Type":"VARCHAR"},{"ind":22,"Name":"contactname2","Type":"VARCHAR"},{"ind":23,"Name":"contactname1","Type":"VARCHAR"},{"ind":24,"Name":"address2","Type":"VARCHAR"},{"ind":25,"Name":"address1","Type":"VARCHAR"},{"ind":26,"Name":"city","Type":"VARCHAR"},{"ind":27,"Name":"region","Type":"VARCHAR"},{"ind":28,"Name":"country","Type":"NUMBER"},{"ind":29,"Name":"title","Type":"VARCHAR"},{"ind":30,"Name":"institutionid","Type":"NUMBER"},{"ind":31,"Name":"externalid","Type":"VARCHAR"},{"ind":32,"Name":"contactname3","Type":"VARCHAR"}],"ExtIDs":[],"Indexes":[[22,23],[18,19]]},{"Table":"customer1","Columns":[{"ind":33,"Name":"status","Type":"NUMBER"},{"ind":34,"Name":"othernames","Type":"VARCHAR"},{"ind":35,"Name":"startdate","Type":"DATE"},{"ind":36,"Name":"institutionid","Type":"NUMBER"},{"ind":37,"Name":"branchid","Type":"NUMBER"},{"ind":38,"Name":"externalid","Type":"VARCHAR"},{"ind":39,"Name":"phone1","Type":"VARCHAR"},{"ind":40,"Name":"address1","Type":"VARCHAR"},{"ind":41,"Name":"city","Type":"VARCHAR"},{"ind":42,"Name":"country","Type":"NUMBER"},{"ind":43,"Name":"inn","Type":"VARCHAR"},{"ind":44,"Name":"gender","Type":"VARCHAR"},{"ind":45,"Name":"lastname","Type":"VARCHAR"},{"ind":46,"Name":"firstname","Type":"VARCHAR"},{"ind":47,"Name":"birthdate","Type":"DATE"},{"ind":48,"Name":"statustime","Type":"DATE"},{"ind":49,"Name":"email","Type":"VARCHAR"}],"ExtIDs":[{"Column":"branchid","Table":"branch1"}],"Indexes":[]},{"Table":"account1","Columns":[{"ind":1,"Name":"statustime","Type":"DATE"},{"ind":2,"Name":"customerid","Type":"NUMBER"},{"ind":3,"Name":"orignumber","Type":"VARCHAR"},{"ind":4,"Name":"branchid","Type":"NUMBER"},{"ind":5,"Name":"externalid","Type":"VARCHAR"},{"ind":6,"Name":"currency","Type":"NUMBER"},{"ind":7,"Name":"closedate","Type":"DATE"},{"ind":8,"Name":"opendate","Type":"DATE"},{"ind":9,"Name":"status","Type":"NUMBER"}],"ExtIDs":[{"Column":"branchid","Table":"branch1"},{"Column":"customerid","Table":"customer1"}]},{"Table":"card1","Columns":[{"ind":10,"Name":"closedate","Type":"DATE"},{"ind":11,"Name":"firstusedate","Type":"DATE"},{"ind":12,"Name":"statustime","Type":"DATE"},{"ind":13,"Name":"expirationdate","Type":"DATE"},{"ind":14,"Name":"mbr","Type":"NUMBER"},{"ind":15,"Name":"pan","Type":"VARCHAR"},{"ind":16,"Name":"status","Type":"NUMBER"},{"ind":17,"Name":"branchid","Type":"NUMBER"}],"ExtIDs":[{"Column":"accountid","Table":"account1"},{"Column":"branchid","Table":"branch1"}]}]'
     SQLText   : SQL текст-выражение для выборки из источника.
                 Например 'select card.pan pan,card.mbr mbr from card'
     Переданные значения параметров: '{"VAR_pan":{"value":"4550000000000006","type":"String"},"VAR_ost":{"value":"03.06.2022 15:06:49","type":"String"},"VAR_num":{"value":546,"type":"Integer"},"VAR_db":{"value":"@1@","type":"String"}}'
     return task.bpmn_error(
            error_code='656590',
            error_message='Error in ExternalTask',
            variables={"var1": "value1", "success": False}
            ) - Вызывает Business ошибку (т.н. Error Boundary Event - Молния) в которой нужно определить переменные в блоке
                Error: "Code variable", куда попадет "error_code" и "Message variable", куда попадет "error_message".
                Переменные из variables просто появятся в процессе
     return task.failure(
            error_message="db2db task failed",
            error_details=f'gggrrr',
            max_retries=2,
            retry_timeout=50
            ) - будет происходить бесконечная попытка выполнить блок.
    """
    global engine_src, engine_ser, engine_dst, columns, table, indexes, insert, Oper
    # Не нашел как передать параметры в процедуру, заполняю cfg еще раз.
    cfg = {}
    cfg = getenv(cfg)
    dtypes = {"NUMBER": Numeric, "NUMERIC": Numeric, "DECIMAL": Numeric, "SMALLINT": SmallInteger,
              "INT": Integer, "INTEGER": Integer, "BIGINT": BigInteger, "BOOLEAN": Boolean,
              "DATETIME": DateTime, "ENUM": Enum, "FLOAT": Float,
              "REAL": Float, "INTERVAL": Interval, "UNICODE": Unicode, "CLOB": Text,
              "TEXT": Text, "CHAR": String, "JSON": String, "NCHAR": String,
              "NVARCHAR": String, "VARCHAR": String, "DATE": DateTime, "TIMESTAMP": DateTime,
              "BLOB": LargeBinary, "BYTEA": LargeBinary
              }

    logger.debug(f'Process Instance ID = {task.get_process_instance_id()}')
    logger.debug(f'Activity ID = {task.get_activity_id()}')

    try:
        vars = task.get_variables()
        try:
            Oper = vars['Oper']
        except KeyError:
            Oper = ''

        try:
            SQLTable = vars['SQLTable']
        except KeyError:
            SQLTable = '{}'

        try:
            SQLText = vars['SQLText']
        except KeyError:
            SQLText = ''

        # Connect к базе МетаДанных, собираем и забираем строки коннекта к источнику и к цели
        engine_ser = create_engine(cfg['DBDriver'] + '://'+cfg['DBUser']+':'+cfg['DBPassword']+'@'+cfg['DSN'])
        engine_ser.execution_options(stream_results=True)

        (dsn_src) = (engine_ser.execute(text("""
            select d.driver||'://'||s.login||':'||s.pass||'@'||s.dsn from md_src s, md_src_driver d where s.driverid=d.driverid and s.name=:name
            """),{"name": vars['SName']}).fetchone())[0]
        engine_src = create_engine(dsn_src)
        engine_src.execution_options(stream_results=True, isolation_level='AUTOCOMMIT')
        if Oper == 'CloseAction':
            logger.debug(f'Start CloseAction')
            with engine_src.begin() as conn:
                result = conn.execute(text("select app_change_status_actions(pcamundaid=>:camundaid, pstatus=>'completed') as actionid"),
                                      {'camundaid':task.get_process_instance_id()})
            execresult = json.dumps([dict(r) for r in result])
            logger.debug(f'CloseAction Result: {execresult}')
            return task.complete({"Result": execresult})
        # Для ExecSQL переменная TName не определена
        if Oper != 'ExecSQL' and Oper != 'ExecSQLBool' and Oper != 'CloseAction':
            (dsn_dst) = (engine_ser.execute(text("""
                select d.driver||'://'||s.login||':'||s.pass||'@'||s.dsn from md_src s, md_src_driver d where s.driverid=d.driverid and s.name=:name
                """),{"name": vars['TName']}).fetchone())[0]
            engine_dst = create_engine(dsn_dst)
            engine_dst.execution_options(stream_results=True, isolation_level='AUTOCOMMIT')
        # Обходим все входные переменные процесса и при обнаружении значения переменной '^@\d+@$' заменяем его значением из базы
        for i in vars:
            if not ((vars[i]) is None):
                if re.search(r'^@\d+@$',str((vars[i]))):
                    id = int(re.sub(r'@', '', str((vars[i]))))
                    logger.debug(f'Search variable "{i}" in DB {id = }')
                    (vars[i]) = (engine_ser.execute(text("select val from MD_Camunda_CLOB where id=:id"), {"id": id}).fetchone())[0]
            logger.debug(f'Variable {i} = {(vars[i])}')



# Если содержимое Camunda переменной соответствует шаблону '^\@\d+\@$', то его надо искать в базе src, в таблице MD_Camunda_CLOB, по идентификатору \d+
# create table MD_Camunda_CLOB (
#       id  serial,
#       val text not null,
#       constraint md_camunda_CLOB_pk primary key (id)
# );
# Каждый выполнявшийся запрос укладывается в базу под определенным HASH=hashlib.md5((SQLTable + SQLText ).encode()).hexdigest()
# Требуется для определения "новых/измененных" запросов и перестройки таблиц в соответствии с этим
#  create table MD_Camunda_Hash(
#     key  varchar not null,
#     hash varchar not null,
#     constraint md_camunda_hash_pk primary key(key)
# );


        #vars['SQLTable']=[{"Table":"branch1","Columns":[{"ind":18,"Name":"phone3","Type":"VARCHAR"},{"ind":19,"Name":"phone2","Type":"VARCHAR"},{"ind":20,"Name":"phone1","Type":"VARCHAR"},{"ind":21,"Name":"address3","Type":"VARCHAR"},{"ind":22,"Name":"contactname2","Type":"VARCHAR"},{"ind":23,"Name":"contactname1","Type":"VARCHAR"},{"ind":24,"Name":"address2","Type":"VARCHAR"},{"ind":25,"Name":"address1","Type":"VARCHAR"},{"ind":26,"Name":"city","Type":"VARCHAR"},{"ind":27,"Name":"region","Type":"VARCHAR"},{"ind":28,"Name":"country","Type":"NUMBER"},{"ind":29,"Name":"title","Type":"VARCHAR"},{"ind":30,"Name":"institutionid","Type":"NUMBER"},{"ind":31,"Name":"externalid","Type":"VARCHAR"},{"ind":32,"Name":"contactname3","Type":"VARCHAR"}],"ExtIDs":[],"Indexes":[[22,23],[18,19]]},{"Table":"customer1","Columns":[{"ind":33,"Name":"status","Type":"NUMBER"},{"ind":34,"Name":"othernames","Type":"VARCHAR"},{"ind":35,"Name":"startdate","Type":"DATE"},{"ind":36,"Name":"institutionid","Type":"NUMBER"},{"ind":37,"Name":"branchid","Type":"NUMBER"},{"ind":38,"Name":"externalid","Type":"VARCHAR"},{"ind":39,"Name":"phone1","Type":"VARCHAR"},{"ind":40,"Name":"address1","Type":"VARCHAR"},{"ind":41,"Name":"city","Type":"VARCHAR"},{"ind":42,"Name":"country","Type":"NUMBER"},{"ind":43,"Name":"inn","Type":"VARCHAR"},{"ind":44,"Name":"gender","Type":"VARCHAR"},{"ind":45,"Name":"lastname","Type":"VARCHAR"},{"ind":46,"Name":"firstname","Type":"VARCHAR"},{"ind":47,"Name":"birthdate","Type":"DATE"},{"ind":48,"Name":"statustime","Type":"DATE"},{"ind":49,"Name":"email","Type":"VARCHAR"}],"ExtIDs":[{"Column":"branchid","Table":"branch1"}],"Indexes":[]},{"Table":"account1","Columns":[{"ind":1,"Name":"statustime","Type":"DATE"},{"ind":2,"Name":"customerid","Type":"NUMBER"},{"ind":3,"Name":"orignumber","Type":"VARCHAR"},{"ind":4,"Name":"branchid","Type":"NUMBER"},{"ind":5,"Name":"externalid","Type":"VARCHAR"},{"ind":6,"Name":"currency","Type":"NUMBER"},{"ind":7,"Name":"closedate","Type":"DATE"},{"ind":8,"Name":"opendate","Type":"DATE"},{"ind":9,"Name":"status","Type":"NUMBER"}],"ExtIDs":[{"Column":"branchid","Table":"branch1"},{"Column":"customerid","Table":"customer1"}]},{"Table":"card1","Columns":[{"ind":10,"Name":"closedate","Type":"DATE"},{"ind":11,"Name":"firstusedate","Type":"DATE"},{"ind":12,"Name":"statustime","Type":"DATE"},{"ind":13,"Name":"expirationdate","Type":"DATE"},{"ind":14,"Name":"mbr","Type":"NUMBER"},{"ind":15,"Name":"pan","Type":"VARCHAR"},{"ind":16,"Name":"status","Type":"NUMBER"},{"ind":17,"Name":"branchid","Type":"NUMBER"}],"ExtIDs":[{"Column":"accountid","Table":"account1"},{"Column":"branchid","Table":"branch1"}]}]
        COLUMNS = {} #{'branch1': {18: {'phone3': 'VARCHAR'}, 19: {'phone2': 'VARCHAR'}, 20: {'phone1': 'VARCHAR'}, 21: {'address3': 'VARCHAR'}, 22: {'contactname2': 'VARCHAR'}, 23: {'contactname1': 'VARCHAR'}, 24: {'address2': 'VARCHAR'}, 25: {'address1': 'VARCHAR'}, 26: {'city': 'VARCHAR'}, 27: {'region': 'VARCHAR'}, 28: {'country': 'NUMBER'}, 29: {'title': 'VARCHAR'}, 30: {'institutionid': 'NUMBER'}, 31: {'externalid': 'VARCHAR'}, 32: {'contactname3': 'VARCHAR'}}, 'customer1': {33: {'status': 'NUMBER'}, 34: {'othernames': 'VARCHAR'}, 35: {'startdate': 'DATE'}, 36: {'institutionid': 'NUMBER'}, 37: {'branchid': 'NUMBER'}, 38: {'externalid': 'VARCHAR'}, 39: {'phone1': 'VARCHAR'}, 40: {'address1': 'VARCHAR'}, 41: {'city': 'VARCHAR'}, 42: {'country': 'NUMBER'}, 43: {'inn': 'VARCHAR'}, 44: {'gender': 'VARCHAR'}, 45: {'lastname': 'VARCHAR'}, 46: {'firstname': 'VARCHAR'}, 47: {'birthdate': 'DATE'}, 48: {'statustime': 'DATE'}, 49: {'email': 'VARCHAR'}}, 'account1': {1: {'statustime': 'DATE'}, 2: {'customerid': 'NUMBER'}, 3: {'orignumber': 'VARCHAR'}, 4: {'branchid': 'NUMBER'}, 5: {'externalid': 'VARCHAR'}, 6: {'currency': 'NUMBER'}, 7: {'closedate': 'DATE'}, 8: {'opendate': 'DATE'}, 9: {'status': 'NUMBER'}}, 'card1': {10: {'closedate': 'DATE'}, 11: {'firstusedate': 'DATE'}, 12: {'statustime': 'DATE'}, 13: {'expirationdate': 'DATE'}, 14: {'mbr': 'NUMBER'}, 15: {'pan': 'VARCHAR'}, 16: {'status': 'NUMBER'}, 17: {'branchid': 'NUMBER'}}}
        EXTIDS = {}  #{'branch1': {}, 'customer1': {'branchid': 'branch1'}, 'account1': {'branchid': 'branch1', 'customerid': 'customer1'}, 'card1': {'accountid': 'account1', 'branchid': 'branch1'}}
        INDEXES = {} #{'branch1': [[22, 23], [18, 19]], 'customer1': [], 'account1': [], 'card1': []}
        TABKEY = {}
        for row in json.loads(SQLTable):
            COLUMNS[row['Table']] = {}
            EXTIDS[row['Table']] = {}
            INDEXES[row['Table']] = {}
            TABKEY[row['Table']] = {}
            for col in row['Columns']:
                # Вырезаем поля с идентификатором как PK для данной таблицы
                if col['Name']==row['Table']+'id':
                    continue
                COLUMNS[row['Table']][col['ind']] = {'Name': col['Name'], 'Type': col['Type']}
                if col['Name']=='externalid':
                    TABKEY[row['Table']]['externalid'] = ' on conflict (externalid) do update set externalid=excluded.externalid '

            try:
                for ext in row['ExtIDs']:
                    EXTIDS[row['Table']][ext['Column']] = ext['Table']
            except KeyError:
                EXTIDS[row['Table']] = []

            try:
                INDEXES[row['Table']] = row['Indexes']
            except KeyError:
                INDEXES[row['Table']] = []

        # Получаем список параметров (:bla_bla_123) из SQLText и сопоставляем со значениями параметрами, которые были переданы.
        # Если в SQLText есть параметр, а значения для него нет - ошибка.
        # Если в SQLText нет параметров, а значение есть для него нет - тогда ничего
        from dateutil import parser

        SQLParams      = {}
        param_pattern  = re.compile(r':\w+')
        SQLText_params = param_pattern.findall(SQLText)
        if SQLText_params:
            for j in SQLText_params:
                try:
                    if j[1:]=='expDate':
                        SQLParams[j[1:]] = parser.parse(vars[j[1:]])
                    else:
                        SQLParams[j[1:]] = vars[j[1:]]
                except KeyError as ex:
                    logger.error(f'SQLText params parse and matching error: {ex}')
        logger.debug(f'All SQLParams = {SQLParams}')


        hash_ = hashlib.md5((SQLTable + SQLText).encode()).hexdigest()
        # Если HASH такого запроса в базе не обнаружен - выставляем флаг, что запрос изменился
        logger.debug(f"Select Camunda_Hash: key={(SQLTable + cfg['TOPIC'])}, hash={hash_}")
        norecreate_flag = engine_ser.execute(text(
                "select 1 from MD_Camunda_Hash where key=:key and hash=:hash"
                ), {"key": SQLTable + cfg['TOPIC'], "hash": hash_}).fetchone()

        if Oper == 'Refill':
            try:
                logger.debug(f'Oper==Refill => truncate table {SQLTable}')
                engine_dst.execute(text("truncate table "+SQLTable))
            except BaseException as ex:
                logger.error(f'Exception truncate: {ex}')
                norecreate_flag = None

        if norecreate_flag is None and Oper == 'Recreate':
            # Если запрос изменился и Выставлена операция на разрешение пересоздания таблицы - пересоздаем целевые таблицы
            logger.debug(f'Recreate tables: {SQLTable}')
            for table in COLUMNS:
                logger.debug(f'For table: {table}')
                # cols = []
                # Первая колонка автоматически создаваемой таблицы должна иметь имя {table}+id и тип BIGINT
                cols = [Column(table + 'id', dtypes["BIGINT"], primary_key=True, server_default=text("nextval('dm.dm_seq')"))]
                for colind in COLUMNS[table]:
                    logger.debug(f"Add column (colind): {COLUMNS[table][colind]['Name']}  {dtypes[COLUMNS[table][colind]['Type']]}")
                    if COLUMNS[table][colind]['Name'] == 'externalid':
                        cols.append(Column(COLUMNS[table][colind]['Name'], dtypes[COLUMNS[table][colind]['Type']], unique=True))
                    else:
                        cols.append(Column(COLUMNS[table][colind]['Name'], dtypes[COLUMNS[table][colind]['Type']]))
                # Если есть индексы - создаем их
                if INDEXES[table]:
                    idx_count = 0
                    for idxind in INDEXES[table]:  # [[14, 16],[18, 19]]
                        idx_count += 1
                        idx = []
                        idx.append(f'{table}_{idx_count}')
                        for colind in idxind:
                            idx.append(COLUMNS[table][colind]['Name'])
                        logger.debug(f'Add Index: {idx}')
                        cols.append(Index(*idx))

                sql_meta = MetaData(engine_dst)
                tab = Table(table, sql_meta, *cols)
                engine_dst.execute('drop table if exists {}'.format(table))
                sql_meta.create_all()
                table_creation_sql = CreateTable(tab)
                logger.debug(f'Create table SQL: {table_creation_sql}')
                #            engine_dst.execute(table_creation_sql)
                logger.debug(f"Insert Camunda_Hash: key={(SQLTable + cfg['TOPIC'])}, hash={hash_}")
                engine_ser.execute(text(
                    "insert into MD_Camunda_Hash (key, hash) values(:key, :hash) on conflict (key) do update set hash=:hash"
                ), {"key": SQLTable + cfg['TOPIC'], "hash": hash_})

# Сформировать следующий SQL
# insert into {table}({ins}) values({val}) on conflict do nothing return {table}+id
#   INSERT INTO dm.account1 (statustime, customerid, orignumber, branchid, externalid, currency, closedate, opendate, status)
#       VALUES (1, 1, '111', 1, 12, 123, '2023-03-12 16:01:04.000000', '2023-03-12 16:01:09.000000', 1)
#       on conflict (externalid) do update set externalid=excluded.externalid returning account1id into id_;

        if Oper == 'ExecSQL' or Oper == 'ExecSQLBool':
            logger.debug(f'Start {Oper}')
            try:
                with engine_src.begin() as conn:
                    result = conn.execute(text(SQLText), SQLParams)
                if result.rowcount > 1 or Oper == 'ExecSQL':
                    execresult = json.dumps([dict(r) for r in result])
                    logger.debug(f'ExecSQL Result(Arr): {execresult}')
                    return task.complete({"Result": execresult})
                else:
                    for i in result:
                        logger.debug(f'ExecSQLBool Result: {i[0]}')
                        return task.complete(i[0])
            except Exception as ex:
                logger.debug(f'ExecSQL Except: {ex}')
                return task.bpmn_error(error_code='CAMUNDA-00001', error_message=f'{ex}')
#                return task.failure(error_message="db2db task failed", error_details=f'Exec Fail', max_retries=1,retry_timeout=1)
        else:
            logger.debug(f'Start Inserts')
#expDate
            result = engine_src.execute(text(SQLText), SQLParams)

        count = 0
        errors = 0
        while True:
            chunk = result.fetchmany(10000)
            if not chunk:
                break
            for src in chunk:
                # LegacyRow: (datetime.datetime(2021, 12, 2, 17, 34, 39), 61, None, 1, 'MEGA~410810002', 840, None, None, 1, datetime.datetime(2021, 7, 27, 0, 0), datetime.datetime(2020, 6, 1, 0, 0), 2, '676280519001970406', None, None, None, None, None, None, None, None, 'sdf', None, 50, 'MEGA_Branch', 1, None, 'Igorevich', None, None, None, 'Mironec', 'Igor', None, 'twa@mailtest, igor@mail.hru')
                logger.debug(f'Source: {src}')
                count+=1
                TABID = {}
                for table in COLUMNS:
                    ins = ''
                    val = ''
                    params = {}
                    for colind in COLUMNS[table]:
                        ins += COLUMNS[table][colind]['Name'] + ','
                        val += ':' + COLUMNS[table][colind]['Name'] + ','
                        try:
                            # Если нужна подстановка значения ColumntID из ранее вставленной записи, берем это значение из TABID[table]
                            params[COLUMNS[table][colind]['Name']] = TABID[EXTIDS[table][COLUMNS[table][colind]['Name']]]
                        except KeyError:
                            params[COLUMNS[table][colind]['Name']] = src[colind-1]
                    ins = ins[:-1]
                    val = val[:-1]
                    # insert = f"insert into {table} ({ins}) values({val}) on conflict (externalid) do update set externalid=excluded.externalid returning {table}id"
                    try:
                        insert = f"insert into {table} ({ins}) values({val}) {TABKEY[table]['externalid']} returning {table}id"
                    except:
                        insert = f"insert into {table} ({ins}) values({val}) returning {table}id"
                    logger.debug(f'Insert: {insert}')
                    try:
                        result_dst = engine_dst.execute(text(insert), params)
                        result_id = (result_dst.fetchone())[0]
                    except IntegrityError as ie:
                        logger.debug(f'Insert Error: {ie}')
                        err = ie.args[0]
                        # Если не заполнен справочник верхнего уровня - берем следующую строку
                        if re.search(r'violates foreign key constraint', err):
                            logger.error(f'Foreign Key for table {table} is not found: {err}')
                            errors += 1
                            break
                        # Если наткнулись на уникальность по какому-то другому ключу, делаем фиктивный инсерт для получения ID этой записи
                        pattern = r'DETAIL:  Key ([^\)]*\))'
                        constraint = (re.findall(pattern, err, re.MULTILINE))[0]
                        def rep(obj):
                            return f'excluded.{obj.group(0)}'
                        excluded = re.sub(r'(\w+)', rep, constraint)
                        insert = f"insert into {table} ({ins}) values({val}) on conflict {constraint} do update set {constraint}={excluded} returning {table}id"
                        logger.debug(f'Insert NEW: {insert}')
                        try:
                            result_dst = engine_dst.execute(text(insert), params)
                            result_id = (result_dst.fetchone())[0]
                        except:
                            errors += 1
                            result_id = None
                    TABID[table] = result_id
                    logger.debug(f"Returning {table}ID={TABID[table]}")

    except BaseException as ex:
        logger.error(f'Exception: {ex}')
#        return task.bpmn_error(error_code="BPMN_ERROR_CODE", error_message="BPMN Vsio Herovo",variables={"var1": "value1", "success": False})
        return task.failure(error_message="db2db task failed", error_details=f'{ex}', max_retries=3, retry_timeout=5000)
    finally:
        if engine_src:
            engine_src.dispose()
        if Oper != 'ExecSQL' and Oper != 'ExecSQLBool'  and Oper != 'CloseAction':
            if engine_dst:
                engine_dst.dispose()
        if engine_ser:
            engine_ser.dispose()
    logger.info(f'Task completed: Count={count}, Errors={errors}')
    return task.complete({"All": count, "Errors": errors})

def main(health):
    """
    Подписка процесса на топик. Установка Health=OK. Как выставить Health!=OK я пока не знаю
    """
    cfg = {}
    cfg = getenv(cfg)
    logger.info(f'Start MAIN component')
    with health.get_lock():
        health.value = 0  # OK

    ExternalTaskWorker(worker_id="1", base_url=get_camunda_address(cfg), config=cfg).subscribe(cfg['TOPIC'], db2db)

    # try:
    #     while True:
    #         mdcyrcle(cfg)
    #         time.sleep(60)
    # except BaseException as err:
    #     logger.error(f'MAIN error: {err=}, {type(err)=}', extra={"Code": 1})
    #     with health.get_lock():
    #         health.value = 1 # Error!
    #     raise

if __name__ == '__main__':
    """
    Основной процесс-форкатель
    """
    if sys.platform.lower().startswith("win"):
        method = 'spawn'
    else:
        method = 'fork'
    logger.debug(f"set_start_method('{method}')")
    mp.set_start_method(method)
    workers = []
    health = mp.Value('i', 1)
    stime = mp.Value('f', time.time())
    p = mp.Process(target=web, args=(health, stime))
    p.start()
    workers.append(p)
    c = mp.Process(target=main, args=(health,))
    c.start()
    workers.append(c)
    for worker in workers:
        worker.join()