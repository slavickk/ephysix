######################################################################
# File: main.py
# Copyright (c) 2024 Vyacheslav Kotrachev
#
# This library is free software; you can redistribute it and/or
# modify it under the terms of the GNU Lesser General Public
# License as published by the Free Software Foundation; either
# version 2.1 of the License, or (at your option) any later version.
#
# This library is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARDummyProtocol1ULAR PURPOSE.  See the GNU
# Lesser General Public License for more details.
#
######################################################################
# -*- coding: utf-8 -*-

from sqlalchemy import *
import json
import base64
from app_logger import logger
import multiprocessing as mp
import time
import sys
from flask import Flask, Response
import prometheus_client
from prometheus_client import Gauge, Histogram, Counter, Summary
import os
import logging
#from sensitivedatalib import sensitivedatafinder


logger.info(f'Start service version=0.0.23 ({__name__})')

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
        uptime['metadata_uptime'].set(delta)  # Increment by given value
        os.environ['PATH_INFO'] = '/metrics'
        ccc = metrics_app(os.environ, start_fn)
        return Response((ccc[0]).decode('utf-8'), status=200, mimetype='text/plain')

    def start_fn(a, b):
        return

    uptime = {}
    metrics_app = prometheus_client.make_wsgi_app()
    uptime['metadata_uptime'] = Gauge('metadata_uptime', 'MetaData Service Uptime')
    app.run(host='0.0.0.0', port=5000)


def load_dbp(file_):
    f = open(file_, encoding="utf8")
    data = json.load(f)
    srcid = 1
    for i in data:
        table = i['title']
        tdescr_en = (i['description'])['en']
        tdescr_ru = (i['description'])['ru']
        print(f'{table = } : {tdescr_ru} / {tdescr_en}')
        engine_md.execute(text("insert into MD_Node_Attr_Val(NodeID, AttrID, Val, srcid) " +
                               " select nodeid, md_get_attr('Description_RU'), :val, :srcid from md_node n, md_type t where n.typeid=t.typeid and lower(n.name)=lower(:node) and t.key='Table'"),
                          {"node": table, "val": tdescr_ru, "srcid": srcid})
        engine_md.execute(text("insert into MD_Node_Attr_Val(NodeID, AttrID, Val, srcid) " +
                               " select nodeid, md_get_attr('Description_EN'), :val, :srcid from md_node n, md_type t where n.typeid=t.typeid and lower(n.name)=lower(:node) and t.key='Table'"),
                          {"node": table, "val": tdescr_en, "srcid": srcid})
        # for d in i['description']:
        #     print(f"{d} = {(i['description'])[d]}")
        for c in i['columns']:
            column = c['name']
            cdescr_en = (c['description'])['en']
            cdescr_ru = (c['description'])['ru']
            print(f"column={c['name']} : {cdescr_ru} / {cdescr_en}")
            engine_md.execute(text("insert into MD_Node_Attr_Val(NodeID, AttrID, Val, srcid) " +
                                   "select nc.NodeID,md_get_attr('Description_RU'), :val, :srcid from md_node nt, md_type tt, md_arc na, md_type ta, md_node nc, md_type tc" +
                                   "  where nt.typeid=tt.typeid and lower(nt.name)=lower(:table) and tt.key='Table'" +
                                   "    and na.FromID=nc.NodeID and na.ToID=nt.NodeID and na.typeid=ta.TypeID and ta.key='Column2Table'" +
                                   "    and nc.typeid=tc.typeid and lower(nc.name)=lower(:column) and tc.key='Column'"),
                              {"table": table, "column": column, "val": cdescr_ru, "srcid": srcid})
            engine_md.execute(text("insert into MD_Node_Attr_Val(NodeID, AttrID, Val, srcid) " +
                                   "select nc.NodeID,md_get_attr('Description_EN'), :val, :srcid from md_node nt, md_type tt, md_arc na, md_type ta, md_node nc, md_type tc" +
                                   "  where nt.typeid=tt.typeid and lower(nt.name)=lower(:table) and tt.key='Table'" +
                                   "    and na.FromID=nc.NodeID and na.ToID=nt.NodeID and na.typeid=ta.TypeID and ta.key='Column2Table'" +
                                   "    and nc.typeid=tc.typeid and lower(nc.name)=lower(:column) and tc.key='Column'"),
                              {"table": table, "column": column, "val": cdescr_en, "srcid": srcid})
            # for d in c['description']:
            #     print(f"{d} = {(c['description'])[d]}")
    f.close()
    # engine_md.execute(text(
    #     "insert into MD_Node_Attr_Val(NodeID, AttrID, Val, srcid) values (:nodeid, md_get_attr('Description_RU'),:val,:srcid)"),
    #                   {"nodeid": nodeid_table, "val": tc["text"], "srcid": srcid})


# load_dbp('./twfa_tables.json')

def merge_node2node(engine, srcid, name, tablenodeid):
    """ Проверяем есть ли уже такая связка NodeColumn-ArcToTable, если есть - берем её NodeColumnID,
        также помечаем, что мы её 'потрогали'
        Если нет - вставляем её в темповую табличку, помечаем запись как 'Измененная' """
    arctype='Column2Table'
    nodetype='Column'
    with engine.connect().execution_options(autocommit=True) as conn:
        nodeid = (conn.execute(text(
            "select md_merge_tmp_node2node(psrcid=>:srcid, pmasternodeid=>:tablenodeid, parctype=>(:arctype)::text, pslavename=>(:name)::text, pslavetype=>(:nodetype)::text)"),
            {"name": name, "nodetype": nodetype, "arctype": arctype, "srcid": srcid, "tablenodeid": tablenodeid}).fetchone())[0]
    return nodeid

def merge_node(engine, srcid, name, type):
    """ Проверяем есть ли уже Node с таким Name в данном DataSource, если есть - берем её NodeID,
        также помечаем, что мы её 'потрогали'
        Если нет - вставляем её в темповую табличку, помечаем запись как 'Измененная' """
    with engine.connect().execution_options(autocommit=True) as conn:
        nodeid = (conn.execute(text(
            "select md_merge_tmp_node(psrcid=>:srcid, pname=>:name, ptype=>:type)"),
            {"name": name, "type": type, "srcid": srcid}).fetchone())[0]
    return nodeid

def merge_nav(engine, srcid, nodeid, pattr, attr, val):
    """ Проверяем есть ли запись с атрибутом attr у узла с nodeid, если есть, мы его обновляем и помечаем, что его 'потрогали'
        Если нет - вставляем запись """
    with engine.connect().execution_options(autocommit=True) as conn:
        navid = (conn.execute(text(
            "select md_merge_tmp_nav(psrcid=>:srcid, pnodeid=>:nodeid, ppattr=>:pattr, pattr=>:attr, pval=>(:val)::text)"),
            {"nodeid": nodeid, "pattr": pattr, "attr": attr, "srcid": srcid, "val": val}).fetchone())[0]
    return navid

def merge_arc(engine, srcid, fromid, toid, type):
    """ Проверяем есть ли уже такая связь для FromID и ToID, если есть - берем её ArcID,
        также помечаем, что мы её 'потрогали'
        Если нет - вставляем её в темповую табличку, помечаем запись как 'Измененная' """
    with engine.connect().execution_options(autocommit=True) as conn:
        arcid = (conn.execute(text(
            "select md_merge_tmp_arc(psrcid=>:srcid, pfromid=>:fromid, ptoid=>:toid, ptype=>:type)"),
            {"fromid": fromid, "toid": toid, "type": type, "srcid": srcid}).fetchone())[0]
    return arcid


def merge_aav(engine, srcid, arcid, pattr, attr, val):
    """ Проверяем есть ли запись с номером счетчика, если есть - мы её обновляем и помечаем, что её 'потрогали'
        Если нет - вставляем запись """
    with engine.connect().execution_options(autocommit=True) as conn:
        aavid = (conn.execute(text(
            "select md_merge_tmp_aav(psrcid=>:srcid, parcid=>:arcid, ppattr=>:pattr, pattr=>:attr, pval=>(:val)::text)"),
            {"arcid": arcid, "pattr": pattr, "attr": attr, "srcid": srcid, "val": val}).fetchone())[0]
    return aavid

def mdcyrcle(cfg):
    # engine_src = create_engine('oracle+cx_oracle://TWA:TWA@(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.75.173)(PORT=1521))(CONNECT_DATA=(SID=DEV3)))')
    # engine_md = create_engine('postgresql+psycopg2://md:rav1234@192.168.75.220/fpdb')
    engine_md = create_engine(f"{cfg['mddriver']}://{cfg['mdlogin']}:{cfg['mdpasswd']}@{cfg['mddsn']}")
    with engine_md.connect().execution_options(autocommit=True) as conn:
        conn.execute(f'alter role md set search_path = md, public')
        """ Устанавливает пароль шифрования для записи Sensitive данных в таблицу md_example """
        conn.execute(text("select set_config('DM.Password', :pass, false)"),{"pass": cfg['cryptopass']})
    src_row = engine_md.execute(text(
        "select s.srcid ,d.driver||'://'||s.Login||':'||s.Pass||'@'||s.DSN, s.login, d.driver, s.DSN, s.descr, s.schema from md_src s, md_src_driver d where s.DriverID=d.DriverID and s.status=1"
    )).fetchall()

    for src in src_row:
        try:
            logger.debug(f"Source Database '{src[5]}' with DSN={src[4]}")
            # Соединяемся с базой-источником
            engine_src = create_engine(src[1])

            # Переключаемся на схему пользователя + public (если у нас PostgreSQL)
            if src[3] == 'postgresql+psycopg2':
                logger.debug(f"This is PostgreSQL search path={src[6]}, public")
                engine_src.execute(f"alter role {src[2]} set search_path = {src[6]}, public")
            #    alter role user1 set search_path = "$user", public
            elif src[3] == 'oracle+cx_oracle':
                logger.debug(f"This is Oracle ALTER SESSION SET CURRENT_SCHEMA = {src[6]}")
                engine_src.execute(f"ALTER SESSION SET CURRENT_SCHEMA = {src[6]}")
            # фиксируем текущий srcid, он участвует в каждом объекте метаданных и srcschema, она участвует в FK
            srcid = src[0]
            srcschema=src[6]
            # Создаем копии основных объектов каждого источника в виде временных таблиц + IsChanged (boolean NOT NULL default false) + Schema
            temp = ''
            temp = 'temporary'
            for t in ['node', 'arc', 'node_attr_val', 'arc_attr_val']:
                logger.debug(f'Drop table IF EXISTS: TMP_{t}')
                engine_md.execute(f'drop table IF EXISTS TMP_{t}')
                logger.debug(f"Create {temp} table: TMP_{t}")
                engine_md.execute(text(
                    f"create {temp} table TMP_{t} as select n.*, 0 as IsChanged, s.schema as schema from md_{t} n, md_src s "
                    f"where not n.isdeleted and n.srcid=s.srcid "
                    f"  and (s.driverid, s.dsn)=(select driverid, dsn from md_src where srcid=:srcid)"),
                    {"srcid": srcid})

            inspector_src = inspect(engine_src)
            #    os._exit(0)

            # Цикл по ВСЕМ таблицам. Они являются вершиной иерархии всех объектов базы
            for t in inspector_src.get_table_names():
                logger.debug(f"Table={t}")
                nodeid_table = merge_node(engine_md, srcid, t, 'Table')
            #        print(f'{nodeid_table=}')
                tc = inspector_src.get_table_comment(t)
                if tc["text"] != None:
                    merge_nav(engine_md, srcid, nodeid_table, 'Addon', 'Description_DB', tc["text"])
                col2id = {}
                # Делаем обход по всем колонкам данной таблицы
                for c in inspector_src.get_columns(t):
                    tname = type(c["type"]).__name__
                    logger.debug(f'Column={c["name"]} / {tname}')
                    nodeid_column = merge_node2node(engine_md, srcid, c["name"], nodeid_table)
                    # try:
                    #     # Ищем связку КолонкаТаблицы+СвязьСТаблицей, если есть, помечаем и колонку и связь, что мы их "потрогали"
                    #     nodeid_column, arcid = (engine_md.execute(text('''
                    #         select nc.nodeid, ct.arcid from TMP_arc ct, TMP_node nc
                    #             where ct.fromid=nc.nodeid and ct.toid=:nodeid and ct.typeid=md_get_type('Column2Table') and ct.srcid=:srcid
                    #               and nc.typeid=md_get_type('Column') and nc.name=:name and nc.srcid=:srcid
                    #         '''),
                    #         {"nodeid": nodeid_table, "srcid": srcid, "name": c["name"]}).fetchone())[0:2]
                    #     engine_md.execute(text("update TMP_Node set IsChanged=1 where NodeID=:nodeid"),{"nodeid": nodeid_column})
                    #     engine_md.execute(text("update TMP_Arc set IsChanged=1 where ArcID=:arcid"),{"arcid": arcid})
                    #     logger.debug("Do Nothing")
                    # except TypeError as ex:
                    #     nodeid_column = (engine_md.execute(text(
                    #         "insert into TMP_Node(NodeId, name, typeid, srcid, IsChanged) values (nextval('md_seq'), :name, md_get_type('Column'), :srcid, 2) returning NodeID"),
                    #                                        {"name": c["name"], "srcid": srcid}).fetchone())[0]
                    #     engine_md.execute(text(
                    #         "insert into TMP_Arc(ArcId, FromID, ToID, typeid, srcid, IsChanged) values (nextval('md_seq'), :columnid,:tableid, md_get_type('Column2Table'),:srcid, 2)"),
                    #                       {"columnid": nodeid_column, "tableid": nodeid_table, "srcid": srcid})
                    #     # Если добавили колонку - таблица стала "как новая"
                    #     engine_md.execute(text("update TMP_Node set IsChanged=2 where NodeID=:nodeid"), {"nodeid": nodeid_table})
                    #     logger.debug("Changed")

                    col2id[c["name"]] = nodeid_column
            #            print(f'Column (ID={nodeid_column} / {tname}/{c["type"]}): {c}')

                    if tname == "NUMBER":
                        merge_nav(engine_md, srcid, nodeid_column, 'DataTypeDef', 'Precision', (c["type"]).precision)
                        merge_nav(engine_md, srcid, nodeid_column, 'DataTypeDef', 'Scale', (c["type"]).scale)
            #                print(f'{tname}: ({(c["type"]).precision}/{(c["type"]).scale} )')
                    elif tname == "VARCHAR" or tname == "CHAR":
                        merge_nav(engine_md, srcid, nodeid_column, 'DataTypeDef', 'Length', (c["type"]).length)
            #                print(f'{tname}: ({(c["type"]).length})')
                    # tname == "DATE" or tname == "BLOB" or tname == "CLOB" or tname == "RAW" or tname == "FLOAT" or \
                    # tname == 'BINARY_DOUBLE' or tname == 'TIMESTAMP' or tname == 'NullType' or tname == 'INTEGER' or \
                    # tname == 'LONG' :
                    merge_nav(engine_md, srcid, nodeid_column, 'DataType', tname, '')
                    merge_nav(engine_md, srcid, nodeid_column, 'Constraint', 'isNull', c["nullable"])

                    if c["default"] != None:
                        merge_nav(engine_md, srcid, nodeid_column, 'DataTypeDef', 'DefaultValue', c["default"])

                    if c["comment"] != None:
                        merge_nav(engine_md, srcid, nodeid_column, 'Addon', 'Comment', c["comment"])

                for un in inspector_src.get_unique_constraints(t):
                    nodeid_unique = merge_node(engine_md, srcid, un["name"], 'Unique')
                    count = 0
                    for c in un['column_names']:
                        count += 1
                        arcid = merge_arc(engine_md, srcid, nodeid_unique, col2id[c], 'Unique')
                        merge_aav(engine_md, srcid, arcid, 'Constraint', 'SequenceNumber', count)

                for ch in inspector_src.get_check_constraints(t):
                    nodeid_check = merge_node(engine_md, srcid, ch["name"], 'Check')
                    arcid = merge_arc(engine_md, srcid, nodeid_check, nodeid_table, 'Check')
                    merge_nav(engine_md, srcid, nodeid_check, 'Constraint', 'SQLText', ch["sqltext"])

                pk = inspector_src.get_pk_constraint(t)
                if pk['name'] != None:
                    logger.debug(f"PK={pk}")
                    nodeid_pk = merge_node(engine_md, srcid, pk["name"], 'PrimaryKey')
                    count = 0
                    for c in pk['constrained_columns']:
                        count += 1
                        arcid = merge_arc(engine_md, srcid, nodeid_pk, col2id[c], 'PrimaryKey')
                        merge_aav(engine_md, srcid, arcid, 'Constraint', 'SequenceNumber', count)

                for ind in inspector_src.get_indexes(t):
                    logger.debug(f"IDX={ind}")
                    if ind['unique']:
                        indtype = 'Unique'
                    else:
                        indtype = 'Index'
                    nodeid_ind = merge_node(engine_md, srcid, ind["name"], indtype)
                    count = 0
                    for c in ind['column_names']:
                        count += 1
                        arcid = merge_arc(engine_md, srcid, nodeid_ind, col2id[c], indtype)
                        merge_aav(engine_md, srcid, arcid, 'Addon', 'SequenceNumber', count)

            #### Прошли все таблицы и колонки, теперь создаем Foreign Keys ####
            for t in inspector_src.get_table_names():
                # Еще раз пройти по ВСЕМ таблицам и сделать FK. 'options'=>descr
                for fk in inspector_src.get_foreign_keys(t):
                    logger.debug(f"FK={fk}")
                    nodeid_fk = merge_node(engine_md, srcid, fk["name"], 'ForeignKey')
                    merge_nav(engine_md, srcid, nodeid_fk, 'Constraint', 'ForeignKeyOptions', json.dumps(fk["options"]))
                    count = 0
                    if fk["referred_schema"]:
                        referred_schema = fk["referred_schema"]
                    else:
                        referred_schema = srcschema
                    for c in fk["constrained_columns"]:
                        count += 1
                        constr_colid = (engine_md.execute(text(
                            "select n3.NodeID from tmp_node n2, tmp_arc a2, tmp_node n3" +
                            "  where n2.typeid=md_get_type('Table') /* Table */  and a2.toid=n2.nodeid and a2.fromid=n3.nodeid and a2.typeid=md_get_type('Column2Table') /* Column2Table */" +
                            "    and n3.typeid=md_get_type('Column') /* Column */ and n3.name = :column /* Column */ and n2.name=:table /* Table*/ " +
                            "    and n2.srcid= :srcid /* SRCID */ and n3.srcid=n2.srcid and a2.srcid=n3.srcid"
                        ), {"column": c, "table": t, "srcid": srcid}).fetchone())[0]
                        arcid = merge_arc(engine_md, srcid, constr_colid, nodeid_fk, 'ForeignKey')
                        merge_aav(engine_md, srcid, arcid, 'Constraint', 'SequenceNumber', count)

                        try:
                            ref_colid = (engine_md.execute(text(
                                "select n3.NodeID from tmp_node n2, tmp_arc a2, tmp_node n3" +
                                "  where n2.typeid=md_get_type('Table') /* Table */  and a2.toid=n2.nodeid and a2.fromid=n3.nodeid and a2.typeid=md_get_type('Column2Table') /* Column2Table */" +
                                "    and n3.typeid=md_get_type('Column') /* Column */ and n3.name = :column and n2.name=:table" +
                                "    and n2.schema= :schema and n3.schema=n2.schema and a2.schema=n3.schema"
#                                "    and n2.srcid= :srcid /* SRCID */ and n3.srcid=n2.srcid and a2.srcid=n3.srcid"
                            ), {"column": (fk["referred_columns"])[fk["constrained_columns"].index(c)],
                                "table": fk["referred_table"], "schema": referred_schema}).fetchone())[0]
#                                "table": fk["referred_table"], "srcid": srcid}).fetchone())[0]
                            arcid = merge_arc(engine_md, srcid, nodeid_fk, ref_colid, 'ForeignKey')
                            merge_aav(engine_md, srcid, arcid, 'Constraint', 'SequenceNumber', count)
                #                print(f'fkname={fk["name"]}, fkoption={json.dumps(fk["options"])}, constr_colid={constr_colid}, ref_colid={ref_colid}')
                        except TypeError as terr:
                            logger.info(f'Foreign key to another scheme: {terr=}')
                            merge_nav(engine_md, srcid, nodeid_fk, 'Constraint', 'ForeignKey2AnotherScheme', json.dumps(fk))

            for v in inspector_src.get_view_names():
                logger.debug(f"View={v}")
                vd = inspector_src.get_view_definition(v)
                nodeid_view = merge_node(engine_md, srcid, v, 'View')

                if vd != None:
            #            print(f'Definition: {vd}')
                    merge_nav(engine_md, srcid, nodeid_view, 'Addon', 'Definition', vd)

            count_node = (engine_md.execute(text('select count(*) from TMP_Node')).fetchone())[0]
            count_nav  = (engine_md.execute(text('select count(*) from TMP_Node_Attr_Val')).fetchone())[0]
            count_arc  = (engine_md.execute(text('select count(*) from TMP_Arc')).fetchone())[0]
            count_aav  = (engine_md.execute(text('select count(*) from TMP_Arc_Attr_Val')).fetchone())[0]
            logger.debug(f'{count_node = }, {count_nav = }, {count_arc = }, {count_aav = }')
            # MERGE
            logger.debug(f"Merge into MD DB")
            with engine_md.connect().execution_options(autocommit=True) as conn:
                conn.execute(text('select md_merge_tmp_into_mddb(:srcid)'), {"srcid": srcid})

            # logger.debug(f"Check Sensitive")
            # for ins in engine_md.execute(text("select nodeid,srcid,name from tmp_node where ischanged=2 and typeid=md_get_type('Table')"), {}).fetchall():
            #     for row in engine_src.execute(text(f"select * from " + ins[2])).fetchmany(100):
            #         for i in row._mapping:
            #             sens = sensitivedatafinder.find_sensitive_at_db_column([str(row._mapping[i])])
            #             if len(sens) != 0:
            #                 sens_keys = ", ".join(list(sens.keys()))
            #                 logger.debug(f"Sensitive: tab='{ins[2]}', Col={i}, sens={sens_keys}, data={str(row._mapping[i])}")
            #                 with engine_md.connect().execution_options(autocommit=True) as conn:
            #                     conn.execute(text('select md_set_sens_attr(psrcid=>:srcid, ptable=>:table, pcolumn=>:column, pval=>:val)'),
            #                                  {"srcid":srcid,"table":ins[2],"column":i,"val":sens_keys})

            logger.debug(f"Insert Examples")
            for ins in engine_md.execute(text("select nodeid,srcid,name from tmp_node where ischanged=2 and typeid=md_get_type('Table')"), {}).fetchall():
                engine_md.execute(text("delete from md_example where nodeid=:nodeid"), {"nodeid": ins[0]})
                ctype = {}
                for c in inspector_src.get_columns(ins[2]):
                    ctype[c["name"]] = type(c["type"]).__name__
                logger.debug(f"Table: '{ins[2]}', Columns:{ctype}")
                for row in engine_src.execute(text(f"select * from " + ins[2])).fetchmany(10):
                    data = {}
                    for i in row._mapping:
#                        logger.debug(f"Table: '{ins[2]}', I:{i}")
                        if ctype[i] == 'BLOB' and row._mapping[i] != None:
                            data[i] = base64.b64encode(row._mapping[i])
                        else:
                            data[i] = row._mapping[i]
            #                print(f'{ctype[i] = } / {data[i] = }')
                    jdata = json.dumps(data, default=str)
            #            print(f'{jdata = }')
                    with engine_md.connect().execution_options(autocommit=True) as conn:
                        conn.execute(text("select md_save_example(psrcid=>:srcid, ptable=>:table, pexample=>:json)"),
                                      {"srcid":srcid,"table": ins[2], "json": jdata})

            engine_src.dispose()
            engine_md.execute(text("update md_src s set status=0, errmessage='', tsupdated=current_timestamp where s.srcid=:srcid"), {"srcid": src[0]})
            logger.debug(f"Good finish SRC '{src[5]}'")
        except BaseException as err:
            logger.error(f'MDCyrcle error: {err=}, {type(err)=}, set 2', extra={"Code": 1})
            engine_md.execute(text("update md_src s set status=2, errmessage=:err, tsupdated=current_timestamp where s.srcid=:srcid"),
                              {"srcid": src[0], "err": err.args[0] + ', Type:' + type(err).__name__})
    engine_md.dispose()

def getenv(cfg):
    cfg['mddsn']      = 'master.pgsqlanomaly01.service.consul/fpdb' if 'DB_URL_FPDB' not in os.environ else os.environ['DB_URL_FPDB']
    cfg['mddriver']   = 'postgresql+psycopg2' if 'MDDRIVER' not in os.environ else os.environ['MDDRIVER']
    cfg['mdlogin']    = 'md' if 'DB_USER_FPDB' not in os.environ else os.environ['DB_USER_FPDB']
    cfg['mdpasswd']   = 'rav1234' if 'DB_PASSWORD_FPDB' not in os.environ else os.environ['DB_PASSWORD_FPDB']
    cfg['cryptopass'] = 'TestovyPass' if 'CRYPTOPASS' not in os.environ else os.environ['CRYPTOPASS']
    return cfg

def main(health):
    cfg = {}
    cfg = getenv(cfg)
    logger.info(f'Start MAIN component')
    with health.get_lock():
        health.value = 0  # OK
    while True:
        try:
            mdcyrcle(cfg)
            with health.get_lock():
                health.value = 0  # OK
            time.sleep(60)
            logger.info(f'Hr-r-r')
        except BaseException as err:
            logger.error(f'MAIN error: {err=}, {type(err)=}', extra={"Code": 1})
            with health.get_lock():
                health.value = 1 # Error!
            time.sleep(300)


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
