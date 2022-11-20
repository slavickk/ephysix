# -*- coding: utf-8 -*-

from camunda.external_task.external_task import ExternalTask, TaskResult
from camunda.external_task.external_task_worker import ExternalTaskWorker
from sqlalchemy import *
#from sqlalchemy.schema import DropTable, CreateTable, CreateIndex
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



logger.info(f'Start service version=0.0.1 ({__name__})')

def getenv(cfg):
    cfg['maxTasks']             = 1 if 'MAX_TASKS' not in os.environ else os.environ['MAX_TASKS']
    cfg['lockDuration']         = 10000 if 'LOCK_DURATION' not in os.environ else os.environ['LOCK_DURATION']
    cfg['asyncResponseTimeout'] = 5000 if 'ASYNC_RESPONCE_TIMEOUT' not in os.environ else os.environ['ASYNC_RESPONCE_TIMEOUT']
    cfg['retries']              = 3 if 'RETRIES' not in os.environ else  os.environ['RETRIES']
    cfg['retryTimeout']         = 5000 if 'RETRY_TIMEOUT' not in os.environ else  os.environ['RETRY_TIMEOUT']
    cfg['sleepSeconds']         = 30 if 'SLEEP_SECONDS' not in os.environ else  os.environ['SLEEP_SECONDS']
    cfg['DBDriver']             = 'postgresql+psycopg2' if 'DBDRIVER' not in os.environ else  os.environ['DBDRIVER']
    cfg['DBUser']               = 'md' if 'DBUSER' not in os.environ else  os.environ['DBUSER']
    cfg['DBPassword']           = 'rav1234' if 'DBPASSWORD' not in os.environ else  os.environ['DBPASSWORD']
    cfg['DSN']                  = 'master.pgsqlanomaly01.service.consul:5432/fpdb' if 'DSN' not in os.environ else  os.environ['DSN']
    cfg['CONSUL_ADDR']          = '192.168.75.205' if 'CONSUL_ADDR' not in os.environ else  os.environ['CONSUL_ADDR']
    cfg['CAMUNDA_NAME']         = 'camunda.service.consul' if 'CAMUNDA_NAME' not in os.environ else  os.environ['CAMUNDA_NAME']
    cfg['TOPIC']                = 'LoginDB' if 'TOPIC' not in os.environ else  os.environ['TOPIC']
#    cfg[''] =  if '' not in os.environ else  os.environ['']
    return cfg

def get_camunda_address(cfg):
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
    return 'http://' + ip + ':' + port + '/engine-rest'


def web(h, st):
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
    global engine_src, engine_ser, engine_dst
    # Не нашел как передать параметры в процедуру, заполняю cfg еще раз.
    cfg = {}
    cfg = getenv(cfg)
    try:
        engine_ser = create_engine(cfg['DBDriver'] + '://'+cfg['DBUser']+':'+cfg['DBPassword']+'@'+cfg['DSN'])
        engine_ser.execution_options(stream_results=True)
        vars = task.get_variables()
        for i in vars:
            if not ((vars[i]) is None):
                if re.search(r'^@\d+@$',str((vars[i]))):
                    id = int(re.sub(r'@', '', str((vars[i]))))
                    logger.debug(f'Search variable "{i}" in DB {id = }')
                    (vars[i]) = (engine_ser.execute(text("select val from MD_Camunda_CLOB where id=:id"), {"id": id}).fetchone())[0]
            logger.debug(f'Variable {i} = {(vars[i])}')
        engine_src = create_engine(vars['SDriver'] + '://' + vars['SLogin'] + ':' + vars['SPassword'] + '@' + vars['SDSN'])
        engine_dst = create_engine(vars['TDriver'] + '://' + vars['TLogin'] + ':' + vars['TPassword'] + '@' + vars['TDSN'])
        engine_src.execution_options(stream_results=True)
        engine_dst.execution_options(stream_results=True)

# Если содержимое Camunda переменной соответствует шаблону '^\@\d+\@$', то его надо искать в базе src, в таблице MD_Camunda_CLOB, по идентификатору \d+
# create table MD_Camunda_CLOB (
#       id  serial,
#       val text not null,
#       constraint md_camunda_CLOB_pk primary key (id)
# );
# Каждый выполнявшийся запрос укладывается в базу под определенным HASH=hashlib.md5((SQLIndexes + SQLTable + SQLText + SQLColumns).encode()).hexdigest()
# Требуется для определения "новых/измененных" запросов и перестройки таблиц в соответствии с этим
#  create table MD_Camunda_Hash(
#     key  varchar not null,
#     hash varchar not null,
#     constraint md_camunda_hash_pk primary key(key)
# );

        dtypes = {"NUMBER"   : Numeric    , "NUMERIC" : Numeric    , "DECIMAL" : Numeric   , "SMALLINT" : SmallInteger,
                  "INT"      : Integer    , "INTEGER" : Integer    , "BIGINT"  : BigInteger, "BOOLEAN"  : Boolean,
                  "DATETIME" : DateTime   , "ENUM"    : Enum      , "FLOAT"    : Float   ,
                  "REAL"     : Float      , "INTERVAL": Interval   , "UNICODE" : Unicode   , "CLOB"     : Text      ,
                  "TEXT"     : Text       , "CHAR"    : String     , "JSON"    : String    , "NCHAR"    : String     ,
                  "NVARCHAR" : String     , "VARCHAR" : String     , "DATE"    : DateTime  , "TIMESTAMP": DateTime,
                  "BLOB"     : LargeBinary, "BYTEA"   : LargeBinary
                 }

        if vars['SQLIndexes']==None:
            SQLIndexes = '{}'
        else:
            SQLIndexes = vars['SQLIndexes']

        SQLParams = {}
        if vars['SQLParams']!=None:
            for j in (vars['SQLParams']).split(','):
                SQLParams[j.strip()] = vars[j.strip()]
        logger.debug(f'All SQLParams = {SQLParams}')

        SQLTable   = vars['SQLTable']
        SQLText    = vars['SQLText']
        SQLColumns = vars['SQLColumns']

        hash_ = hashlib.md5((SQLIndexes + SQLTable + SQLText + SQLColumns).encode()).hexdigest()
        recreate_flag = engine_ser.execute(text(
                "select 1 from MD_Camunda_Hash where key=:key and hash=:hash"
                ), {"key": SQLTable + cfg['TOPIC'], "hash": hash_}).fetchone()

        if recreate_flag is None:
            # Если HASH такого запроса в базе не обнаружен - пересоздаем целевые таблицы
            logger.debug(f'Recreate table {SQLTable}')
            cols=[]
            jcols=json.loads(SQLColumns)
            for i in jcols:
                cols.append(Column(i, dtypes[jcols[i]]))
            idx_count=0
            for i in json.loads(SQLIndexes):
                idx_count += 1
                idx = []
                idx.append(f'{SQLTable}_{idx_count}')
                for j in i.split(','):
                    idx.append(j)
                cols.append(Index(*idx))
            sql_meta = MetaData(engine_dst)
            table = Table(SQLTable, sql_meta, *cols)
            engine_dst.execute('drop table if exists {}'.format(SQLTable))
            sql_meta.create_all()
#            table_creation_sql = CreateTable(table)
#            engine_dst.execute(table_creation_sql)
            engine_ser.execute(text(
                "insert into MD_Camunda_Hash (key, hash) values(:key, :hash) on conflict (key) do update set hash=:hash"
                 ), {"key": SQLTable + cfg['TOPIC'], "hash": hash_})
        if vars['Oper'] == 'Refill':
            logger.debug(f'Oper==Refill => truncate table {SQLTable}')
            engine_dst.execute(text("truncate table "+SQLTable))

        ins=''
        val=''
        for c in json.loads(SQLColumns):
            ins += c + ','
            val += ':' + c + ','
        ins = ins[:-1]
        val = val[:-1]
        insert = f'insert into {SQLTable} ({ins}) values({val})'
        logger.debug(f'Insert: {insert}')
        result = engine_src.execute(text(SQLText), SQLParams)
        count = 0
        errors = 0
        while True:
            chunk = result.fetchmany(10000)
            if not chunk:
                break
            for src in chunk:
                count+=1
                params = {}
                for i in range(len(src)):
                    params[src._fields[i]] = src[i]
                example=json.dumps(params, ensure_ascii=False, default=str)
                logger.debug(f'insert value: {example}')
                try:
                    engine_dst.execute(text(insert), params)
                except:
                    errors+=1
    except BaseException as ex:
        logger.error(f'Exception: {ex}')
#        return task.bpmn_error(error_code="BPMN_ERROR_CODE", error_message="BPMN Vsio Herovo",variables={"var1": "value1", "success": False})
        return task.failure(error_message="db2db task failed", error_details=f'{ex}', max_retries=3, retry_timeout=5000)
    finally:
        if engine_src:
            engine_src.dispose()
        if engine_dst:
            engine_dst.dispose()
        if engine_ser:
            engine_ser.dispose()
    logger.info(f'Task completed: Count={count}, Errors={errors}')
    return task.complete({"All": count, "Errors": errors})

def main(health):
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