/******************************************************************
 * File: MD.v9.sql
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARDummyProtocol1ULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

/*
  v6 - везде добавил схему md и сделал DROP ... IF EXISTS и добавил owner md
  v7 - Добавил функции по работе с TMP_xxx таблицами + перенос из TMP в основные таблицы
       Сделал функции установки и проверки флага 'Sensitive' для колонки таблицы
  v8 - Добавил комментарии к таблицам/колонкам
  v9 - Добавил Constraint на некоторые таблицы. Добавил таблицы MD_Parameter, MD_Camunda_Blob, MD_Camunda_Hash
       Переделал Аудит с поколоночного на пооперационный, все записывается теперь в JSONB
*/

drop table MD_Node_Attr_Val; /*drop, create, fk, ind, trigger */
drop table MD_Arc_Attr_Val;
drop table MD_Audit; /*drop, create, fk, ind, trigger */
drop table MD_Example;
drop table MD_Arc; /*drop, create, fk, ind, trigger */
drop table MD_Attr; /*drop, create, fk, ind, trigger */
drop table MD_Node; /*drop, create, fk, ind, trigger */
drop table MD_Type; /*drop, create, fk, ind, trigger */
drop table MD_SRC;
drop table MD_SRC_DRIVER;
drop table MD_Lang;
drop table MD_Description;
drop table MD_Parameter;
drop table MD_Camunda_Blob; --
drop table MD_Camunda_Hash; --

drop sequence md_seq;
drop sequence md_audit_seq;
drop SEQUENCE md_synonym_seq;

CREATE SEQUENCE md_seq AS bigint;
comment on SEQUENCE md_seq is 'Одна последовательность для ВСЕХ объектов MetaData';

CREATE SEQUENCE md_audit_seq AS bigint;
comment on SEQUENCE md_seq is 'Последовательность для объектов MetaData/Audit';

CREATE SEQUENCE md_synonym_seq AS int;
comment on SEQUENCE md_synonym_seq is 'Последовательность для Синонимов MD_Node.Synonym';


CREATE TABLE MD_SRC_Driver(
    DriverID  serial        NOT NULL,
    driver    varchar(100)  NOT NULL,
    Descr     text          NOT NULL,
    TSCreated  timestamptz   NOT NULL default current_timestamp,
    CONSTRAINT MD_SRC_Driver_PK PRIMARY KEY (DriverID)
);

comment on table  MD_SRC_Driver             is 'Справочник драйверов для коннекта к источникам. По терминологии SQLAlchemy';
comment on column MD_SRC_Driver.driverid    is 'Уникальный идентификатор записи (PK)';
comment on column MD_SRC_Driver.driver      is 'Строка коннекта';
comment on column MD_SRC_Driver.descr       is 'Описание принадлежности драйвера';
comment on column MD_SRC_Driver.tscreated   is 'Когда создана запись';


CREATE TABLE MD_SRC(
    SRCID     serial        NOT NULL,
    DriverID  int,
    DSN       varchar(100),
    Login     varchar(100),
    Pass      varchar(100),
    Name      varchar(100)  NOT NULL,
    Descr     text,
    Status    int default 0, /* Если 1, то обновить данные из этого источника */
    errmessage text,
    TSCreated  timestamptz   NOT NULL default current_timestamp,
    CONSTRAINT MD_SRC_PK PRIMARY KEY (SRCID)
    CONSTRAINT MD_SRC_UN UNIQUE (NAME)
);

comment on table  MD_SRC               is 'Список источников данных';
comment on column MD_SRC.srcid         is 'Уникальный идентификатор записи (PK)';
comment on column MD_SRC.driverid      is 'Ссылка на драйвера';
comment on column MD_SRC.dsn           is 'Строка коннекта/DSN';
comment on column MD_SRC.login         is 'Логин к базе';
comment on column MD_SRC.pass          is 'пароль к базе';
comment on column MD_SRC.name          is 'Наименование источника';
comment on column MD_SRC.descr         is 'Описание источника';
comment on column MD_SRC.status        is '0 - ничего не делать, 1 - на следующем цикле обновить данные, 2 - в результате обновления случилась ошибка, см .errmessage, далее не обновлять';
comment on column MD_SRC.errmessage    is 'Сообщение о последней ошибке';
comment on column MD_SRC.tscreated     is 'Когда создана запись';


CREATE TABLE MD_Arc(
    ArcID     bigint        NOT NULL,
    SRCID     int           NOT NULL,
    FromID    bigint        NOT NULL,
    ToID      bigint        NOT NULL,
    TypeID    int           NOT NULL,
    IsCustom  boolean       NOT NULL default false,
    IsDeleted boolean       NOT NULL default false,
    TSCreated timestamptz   NOT NULL default current_timestamp,
    TSChanged timestamptz   NOT NULL,
    CONSTRAINT MD_Arc_PK PRIMARY KEY (ArcID)
);

comment on table  MD_ARC               is 'Все связи между объектами метаданных';
comment on column MD_ARC.arcid         is 'Уникальный идентификатор записи (PK)';
comment on column MD_ARC.srcid         is 'Ссылка на источник данных';
comment on column MD_ARC.fromid        is 'Соединяет ноду From, ссылка';
comment on column MD_ARC.toid          is 'Соединяет ноду To, ссылка';
comment on column MD_ARC.typeid        is 'Тип связи, ссылка';
comment on column MD_ARC.iscustom      is 'Пользовательская запись, автоматом не изменяется';
comment on column MD_ARC.isdeleted     is 'Удаленная запись';
comment on column MD_ARC.tscreated     is 'Когда создана запись';
comment on column MD_ARC.tschanged     is 'Когда изменена запись';

/*
CREATE TEMPORARY TABLE MD_Arc_tmp(
    ArcID     bigint        NOT NULL,
    FromID    bigint        NOT NULL,
    ToID      bigint        NOT NULL,
    TypeID    bigint        NOT NULL,
    IsDeleted boolean       NOT NULL default false,
    IsProcessed boolean     NOT NULL default false
)  on commit preserve rows;
*/

CREATE TABLE MD_Node(
    NodeID    bigint        NOT NULL,
    SRCID     int           NOT NULL,
    Name      varchar(300)  NOT NULL,
    Synonym   int,
    TypeID    int           NOT NULL,
    IsCustom  boolean       NOT NULL default false,
    IsDeleted boolean       NOT NULL default false,
    TSCreated timestamptz   NOT NULL default current_timestamp,
    TSChanged timestamptz   NOT NULL,
    CONSTRAINT MD_Node_PK PRIMARY KEY (NodeID)
);

comment on table  MD_NODE              is 'Список объектов метаданных';
comment on column MD_NODE.nodeid       is 'Уникальный идентификатор записи (PK)';
comment on column MD_NODE.srcid        is 'Ссылка на источник данных';
comment on column MD_NODE.name         is 'Наименование объекта';
comment on column md_node.Synonym      is 'Идентификатор синонима/группы/кластера';
comment on column MD_NODE.typeid       is 'Тип объекта, ссылка';
comment on column MD_NODE.iscustom     is 'Пользовательская запись, автоматом не изменяется';
comment on column MD_NODE.isdeleted    is 'Удаленная запись';
comment on column MD_NODE.tscreated    is 'Когда создана запись';
comment on column MD_NODE.tschanged    is 'Когда изменена запись';


CREATE TABLE MD_Type(
    TypeID  serial          NOT NULL,
    key     varchar(100)    NOT NULL,
    Name    varchar(300)    NOT NULL,
    IsCustom  boolean       NOT NULL default false,
    TSCreated  timestamptz   NOT NULL default current_timestamp,
    CONSTRAINT MD_Type_PK PRIMARY KEY (TypeID)
);

comment on table  MD_TYPE               is 'Справочник типов для NODE/ARC';
comment on column MD_TYPE.typeid        is 'Уникальный идентификатор записи (PK)';
comment on column MD_TYPE.key           is 'Обозначение типа для автоматической обработки';
comment on column MD_TYPE.name          is 'Описание типа';
comment on column MD_TYPE.tscreated     is 'Когда создана запись';


CREATE TABLE MD_Attr(
    AttrID     serial        NOT NULL,
    AttrPID    int           NOT NULL,
    key        varchar(100)  NOT NULL,
    Name       varchar(300)  NOT NULL,
    IsCustom  boolean       NOT NULL default false,
    TSCreated  timestamptz   NOT NULL default current_timestamp,
    CONSTRAINT MD_Attr_PK PRIMARY KEY (AttrID)
);

comment on table  MD_ATTR               is 'Справочник атрибутов для NODE/ARC';
comment on column MD_ATTR.attrid        is 'Уникальный идентификатор записи (PK)';
comment on column MD_ATTR.attrpid       is 'Организация древовидной структуры (PID)';
comment on column MD_ATTR.key           is 'Обозначение атрибута для автоматической обработки';
comment on column MD_ATTR.name          is 'Описание атрибута';
comment on column MD_ATTR.iscustom      is 'Пользовательская запись, автоматом не изменяется';
comment on column MD_ATTR.tscreated     is 'Когда создана запись';


CREATE TABLE MD_Node_Attr_Val(
    NAVID     bigint        NOT NULL,
    SRCID     int           NOT NULL,
    NodeID    bigint        NOT NULL,
    AttrID    int           NOT NULL,
    Val       text          ,
    IsCustom  boolean       NOT NULL default false,
    IsDeleted boolean       NOT NULL default false,
    TSCreated  timestamptz   NOT NULL default current_timestamp,
    TSChanged  timestamptz   NOT NULL,
    CONSTRAINT MD_Node_Attr_Val_PK PRIMARY KEY (NAVID)
);

comment on table  MD_Node_Attr_Val               is 'Список значений для атрибутов объектов системы';
comment on column MD_Node_Attr_Val.navid         is 'Уникальный идентификатор записи (PK)';
comment on column MD_Node_Attr_Val.srcid         is 'Ссылка на источник данных';
comment on column MD_Node_Attr_Val.nodeid        is 'Ссылка на NODE';
comment on column MD_Node_Attr_Val.attrid        is 'Ссылка на ATTR';
comment on column MD_Node_Attr_Val.val           is 'Значение атрибута для объекта';
comment on column MD_Node_Attr_Val.iscustom      is 'Пользовательская запись, автоматом не изменяется';
comment on column MD_Node_Attr_Val.isdeleted     is 'Удаленная запись';
comment on column MD_Node_Attr_Val.tscreated     is 'Когда создана запись';
comment on column MD_Node_Attr_Val.tschanged     is 'Когда изменена запись';


CREATE TABLE MD_Arc_Attr_Val(
    AAVID     bigint        NOT NULL,
    SRCID     int           NOT NULL,
    ArcID     bigint        NOT NULL,
    AttrID    int           NOT NULL,
    Val       text          ,
    IsCustom  boolean       NOT NULL default false,
    IsDeleted boolean       NOT NULL default false,
    TSCreated  timestamptz   NOT NULL default current_timestamp,
    TSChanged  timestamptz   NOT NULL,
    CONSTRAINT MD_Arc_Attr_Val_PK PRIMARY KEY (AAVID)
);

comment on table  MD_Arc_Attr_Val               is 'Список значений атрибутов для связей системы';
comment on column MD_Arc_Attr_Val.aavid         is 'Уникальный идентификатор записи (PK)';
comment on column MD_Arc_Attr_Val.srcid         is 'Ссылка на источник данных';
comment on column MD_Arc_Attr_Val.arcid         is 'Ссылка на Arc';
comment on column MD_Arc_Attr_Val.attrid        is 'Ссылка на ATTR';
comment on column MD_Arc_Attr_Val.val           is 'Значение атрибута для связи';
comment on column MD_Arc_Attr_Val.iscustom      is 'Пользовательская запись, автоматом не изменяется';
comment on column MD_Arc_Attr_Val.isdeleted     is 'Удаленная запись';
comment on column MD_Arc_Attr_Val.tscreated     is 'Когда создана запись';
comment on column MD_Arc_Attr_Val.tschanged     is 'Когда изменена запись';


CREATE TABLE MD_Audit(
    AudID     bigint        NOT NULL,
    AUser     varchar(100)  NOT NULL default current_user,
    TSAudit   timestamptz   NOT NULL default current_timestamp,
    ObjectID  bigint        NOT NULL,
    ObjectName  text       ,
    OPER      varchar(1)    NOT NULL CHECK (OPER IN ('I', 'U', 'D')),
    OldVal    jsonb         ,
    NewVal    jsonb         ,
    CONSTRAINT MD_Audit_PK PRIMARY KEY (AudID)
);

comment on table  MD_Audit               is 'Аудит изменений данных в системе';
comment on column MD_Audit.audid         is 'Уникальный идентификатор записи (PK)';
comment on column MD_Audit.auser         is 'Идентификатор пользователя, изменившего запись';
comment on column MD_Audit.tsaudit       is 'Дата/Время изменения';
comment on column MD_Audit.objectid      is 'Ссылка на измененный объект';
comment on column MD_Audit.objectname    is 'Наименование объекта/таблицы';
comment on column MD_Audit.oper          is 'Тип операции: I - Insert, D - Delete, U - Update';
comment on column MD_Audit.oldval        is 'Старое значение полей JSON';
comment on column MD_Audit.newval        is 'Новое значение полей JSON';


CREATE TABLE MD_Example(
    ExampleID bigint        NOT NULL,
    NodeID    bigint        NOT NULL,
    Example   jsonb         NOT NULL,
    CONSTRAINT MD_Example_PK PRIMARY KEY (ExampleID)
);

comment on table  MD_Example               is 'Примеры данных';
comment on column MD_Example.exampleid     is 'Уникальный идентификатор записи (PK)';
comment on column MD_Example.nodeid        is 'Ссылка на Node';
comment on column MD_Example.example       is 'Значение для примера из источника';


CREATE TABLE MD_Lang(
    LangID serial,
    Sign   varchar(10)  NOT NULL,
    Descr  varchar(100) NOT NULL,
    CONSTRAINT MD_Lang_PK PRIMARY KEY (LangID)
);

insert into MD_Lang(Sign, Descr) values('Ru', 'Русский');
insert into MD_Lang(Sign, Descr) values('En', 'English');

comment on table  MD_Lang               is 'Справочник языков в системе';
comment on column MD_Lang.langid        is 'Уникальный идентификатор записи (PK)';
comment on column MD_Lang.sign          is 'Обозначение языка для автоматической обработки';
comment on column MD_Lang.descr         is 'Описание языка';


CREATE TABLE md.MD_Description(
    NodeID   bigint        NOT NULL,
    LangID   bigint        NOT NULL,
    Descr    text          NOT NULL,
    CONSTRAINT MD_Description_PK PRIMARY KEY (NodeID, LangID)
);

comment on table  MD_Description               is 'Описание объектов системы';
comment on column MD_Description.nodeid        is 'Ссылка на NODE';
comment on column MD_Description.langid        is 'Ссылка на LANG';
comment on column MD_Description.descr         is 'Описание';


CREATE TABLE MD_Parameter(
    ParamID serial,
    Name   varchar(100)  NOT NULL,
    Val    text NOT NULL, -- value
    DVal   text NOT NULL, --default value
    Descr  varchar(1000) NOT NULL,
    CONSTRAINT MD_Parameter_PK PRIMARY KEY (ParamID)
);

insert into md_parameter(name, val, dval, descr) values('example_qty',10,10,'Quantity of example rows');

comment on table  MD_Parameter               is 'Список параметров системы';
comment on column MD_Parameter.paramid       is 'Уникальный идентификатор записи (PK)';
comment on column MD_Parameter.name          is 'Наименование параметра';
comment on column MD_Parameter.val           is 'Значение параметра';
comment on column MD_Parameter.dval          is 'Значение Default';
comment on column MD_Parameter.descr         is 'Описание параметра';


create table md.md_camunda_clob (
    id  serial constraint md_camunda_clob_pk primary key,
    val text not null
);

comment on table  md_camunda_clob           is 'Хранение больших по размеру переменных для Camunda/ExternalTask';
comment on column md_camunda_clob.id        is 'Уникальный идентификатор записи (PK)';
comment on column md_camunda_clob.val       is 'Значение переменной';

alter table md.md_camunda_clob owner to md;

grant select on sequence md.md_camunda_clob_id_seq to md_r;
grant select on md.md_camunda_clob to md_r;
grant delete, insert, update on md.md_camunda_clob to md_w;


create table md.md_camunda_hash (
    key  varchar not null constraint md_camunda_hash_pk primary key,
    hash varchar not null
);

comment on table  md_camunda_hash           is 'Хранение SQL+Params Camunda/ExternalTask. Если hash изменился - пересоздаем целевые таблицы';
comment on column md_camunda_hash.key       is 'SQLTable+Camunda.Topic';
comment on column md_camunda_hash.hash      is 'hashlib.md5((SQLIndexes + SQLTable + SQLText + SQLColumns).encode()).hexdigest()';

alter table md.md_camunda_hash owner to md;

grant select on md.md_camunda_hash to md_r;
grant delete, insert, update on md.md_camunda_hash to md_w;



/* Constraints */
ALTER TABLE MD_SRC ADD CONSTRAINT MD_SRC_Driver_FK1 FOREIGN KEY (DriverID) REFERENCES MD_SRC_Driver(DriverID);

ALTER TABLE MD_Arc ADD CONSTRAINT MD_Arc_Node_FK1 FOREIGN KEY (FromID) REFERENCES MD_Node(NodeID);
ALTER TABLE MD_Arc ADD CONSTRAINT MD_Arc_Node_FK2 FOREIGN KEY (ToID)   REFERENCES MD_Node(NodeID);
ALTER TABLE MD_Arc ADD CONSTRAINT MD_Arc_Type_FK  FOREIGN KEY (TypeID) REFERENCES MD_Type(TypeID);
ALTER TABLE MD_Arc ADD CONSTRAINT MD_Arc_SRC_FK  FOREIGN KEY (SRCID) REFERENCES MD_SRC(SRCID);

ALTER TABLE MD_Attr ADD CONSTRAINT MD_Attr_FK  FOREIGN KEY (AttrPID)  REFERENCES MD_Attr(AttrID);

ALTER TABLE MD_Node ADD CONSTRAINT MD_Node_Type_FK  FOREIGN KEY (TypeID)  REFERENCES MD_Type(TypeID);
ALTER TABLE MD_Node ADD CONSTRAINT MD_Node_SRC_FK  FOREIGN KEY (SRCID) REFERENCES MD_SRC(SRCID);

ALTER TABLE MD_Type ADD CONSTRAINT MD_Type_Key_UN  UNIQUE (Key);
ALTER TABLE MD_Type ADD CONSTRAINT MD_Type_Name_UN UNIQUE (name);

ALTER TABLE MD_Attr ADD CONSTRAINT MD_Attr_Key_UN UNIQUE (AttrPID,Key);

ALTER TABLE MD_Node_Attr_Val ADD CONSTRAINT MD_Node_Attr_Val_Node_FK FOREIGN KEY (NodeID) REFERENCES MD_Node(NodeID);
ALTER TABLE MD_Node_Attr_Val ADD CONSTRAINT MD_Node_Attr_Val_Attr_FK FOREIGN KEY (AttrID) REFERENCES MD_Attr(AttrID);
ALTER TABLE MD_Node_Attr_Val ADD CONSTRAINT MD_Node_Attr_Val_SRC_FK  FOREIGN KEY (SRCID) REFERENCES MD_SRC(SRCID);

ALTER TABLE MD_Arc_Attr_Val ADD CONSTRAINT MD_Arc_Attr_Val_Arc_FK FOREIGN KEY (ArcID) REFERENCES MD_Arc(ArcID);
ALTER TABLE MD_Arc_Attr_Val ADD CONSTRAINT MD_Arc_Attr_Val_Attr_FK FOREIGN KEY (AttrID) REFERENCES MD_Attr(AttrID);
ALTER TABLE MD_Arc_Attr_Val ADD CONSTRAINT MD_Arc_Attr_Val_SRC_FK  FOREIGN KEY (SRCID) REFERENCES MD_SRC(SRCID);

ALTER TABLE MD_Description ADD CONSTRAINT MD_Description_FK1 FOREIGN KEY (NodeID) REFERENCES md.MD_Node(NodeID);
ALTER TABLE MD_Description ADD CONSTRAINT MD_Description_FK2 FOREIGN KEY (LangID) REFERENCES md.MD_Lang(LangID);

ALTER TABLE MD_Arc  ADD CONSTRAINT MD_Arc_UN   UNIQUE (srcid,fromid,toid,typeid);
ALTER TABLE MD_Node ADD CONSTRAINT MD_Node_UN  UNIQUE (srcid,name,typeid);
ALTER TABLE MD_Arc_Attr_Val  ADD CONSTRAINT MD_Arc_Attr_Val_UN  UNIQUE (srcid,arcid,attrid);
ALTER TABLE MD_Node_Attr_Val ADD CONSTRAINT MD_Arc_Node_Val_UN  UNIQUE (srcid,nodeid,attrid);

/* Indexes */
create index md_src_driverid_i          on md_src (DriverID);

create index md_arc_fromid_i            on md_arc (FromID);
create index md_arc_ToID_i              on md_arc (ToID);
create index md_arc_TypeID_i            on md_arc (TypeID);
create index md_arc_SRCID_i             on md_arc (SRCID);
create index md_arc_IsCustom_i          on md_arc (IsCustom);
create index md_arc_IsDeleted_i         on md_arc (IsDeleted);
create index md_arc_TSCreated_i         on md_arc (TSCreated);
create index md_arc_TSChanged_i         on md_arc (TSChanged);

/*
create index md_arc_tmp_fromid_i        on md_arc_tmp (FromID);
create index md_arc_tmp_ToID_i          on md_arc_tmp (ToID);
create index md_arc_tmp_TypeID_i        on md_arc_tmp (TypeID);
create index md_arc_tmp_IsDeleted_i     on md_arc_tmp (IsDeleted);
create index md_arc_tmp_IsProcessed_i   on md_arc_tmp (IsProcessed);
*/
create index md_node_Name_i             on md_node (Name);
create index MD_Node_Synonym_i          on MD_Node (Synonym);
create index md_node_TypeID_i           on md_node (TypeID);
create index md_node_SRCID_i            on md_node (SRCID);
create index md_node_IsCustom_i         on md_node (IsCustom);
create index md_node_IsDeleted_i        on md_node (IsDeleted);
create index md_node_TSCreated_i        on md_node (TSCreated);
create index md_node_TSChanged_i        on md_node (TSChanged);

create index md_type_key_i              on md_type (key);
create index md_type_Name_i             on md_type (Name);
create index md_type_TSCreated_i        on md_type (TSCreated);

create index md_attr_AttrPID_i          on md_attr (AttrPID);
create index md_attr_key_i              on md_attr (key);
create index md_attr_Name_i             on md_attr (Name);
create index md_attr_TSCreated_i        on md_attr (TSCreated);

create index md_Node_Attr_Val_NodeID_i  on md_Node_Attr_Val (NodeID);
create index md_Node_Attr_Val_AttrID_i  on md_Node_Attr_Val (AttrID);
create index md_Node_Attr_Val_Val_i     on md_Node_Attr_Val (Val);
create index md_Node_Attr_Val_SRCID_i     on md_Node_Attr_Val (SRCID);
create index md_Node_Attr_Val_IsCustom_i  on md_Node_Attr_Val (IsCustom);
create index md_Node_Attr_Val_isDeleted_i on md_Node_Attr_Val (isDeleted);
create index md_Node_Attr_Val_TSCreated_i on md_Node_Attr_Val (TSCreated);
create index md_Node_Attr_Val_TSChanged_i on md_Node_Attr_Val (TSChanged);

create index md_Arc_Attr_Val_ArcID_i on md_Arc_Attr_Val (ArcID);
create index md_Arc_Attr_Val_AttrID_i on md_Arc_Attr_Val (AttrID);
create index md_Arc_Attr_Val_Val_i on md_Arc_Attr_Val (Val);
create index md_Arc_Attr_Val_SRCID_i     on md_Arc_Attr_Val (SRCID);
create index md_Arc_Attr_Val_IsCustom_i  on md_Arc_Attr_Val (IsCustom);
create index md_Arc_Attr_Val_isDeleted_i on md_Arc_Attr_Val (isDeleted);
create index md_Arc_Attr_Val_TSCreated_i on md_Arc_Attr_Val (TSCreated);
create index md_Arc_Attr_Val_TSChanged_i on md_Arc_Attr_Val (TSChanged);

create index md_Audit_AUser_i on md_Audit (AUser);
create index md_Audit_TSAudit_i on md_Audit (TSAudit);
create index md_Audit_ObjectID_i on md_Audit (ObjectID);
create index md_Audit_OPER_i on md_Audit (OPER);
create index md_Audit_Field_i on md_Audit (Field);
create index md_Audit_OldVal_i on md_Audit (OldVal);
create index md_Audit_NewVal_i on md_Audit (NewVal);

create index md_example_NodeID_i        on md_example(NodeID);

create index md_lang_Sign_i             on md_lang(Sign);

create index md_description_NodeID_i    on md_description(NodeID);
create index md_description_LangID_i    on md_description(LangID);

create unique index MD_Parameter_Name_i      on MD_Parameter(Name);


DROP TRIGGER IF EXISTS MD_Arc_TR on md.MD_Arc cascade;
DROP FUNCTION IF EXISTS md.MD_Arc_TR;
CREATE OR REPLACE FUNCTION md.MD_Arc_TR()
  RETURNS TRIGGER
  LANGUAGE PLPGSQL
  AS
$$
DECLARE
    NEWfromNode_ md.md_node%rowtype;
    NEWtoNode_   md.md_node%rowtype;
    NEWtype_     md.md_type%rowtype;
    NEWsrc_      md.md_src%rowtype;
    OLDfromNode_ md.md_node%rowtype;
    OLDtoNode_   md.md_node%rowtype;
    OLDtype_     md.md_type%rowtype;
    OLDsrc_      md.md_src%rowtype;
    NFromIDJB     jsonb:='{}';
    NToIDJB       jsonb:='{}';
    NTypeIDJB     jsonb:='{}';
    NSRCIDJB      jsonb:='{}';
    NisDeletedJB  jsonb:='{}';
    NisCustomJB   jsonb:='{}';
    OFromIDJB     jsonb:='{}';
    OToIDJB       jsonb:='{}';
    OTypeIDJB     jsonb:='{}';
    OSRCIDJB      jsonb:='{}';
    OisDeletedJB  jsonb:='{}';
    OisCustomJB   jsonb:='{}';
BEGIN
    IF NEW.ArcID is null then
        NEW.ArcID:=nextval('md.md_seq');
    end if;
    NEW.TSChanged:=current_timestamp;
    CASE TG_OP
    WHEN 'INSERT' then
        NEW.TSCreated:=current_timestamp;
        select * into NEWfromNode_ from md.md_node where NodeID=NEW.FromID;
        select * into NEWtoNode_   from md.md_node where NodeID=NEW.toID;
        select * into NEWtype_     from md.md_type where TypeID=NEW.TypeID;
        select * into NEWsrc_      from md.md_src  where SRCID=NEW.SRCID;
        insert into md.md_audit(objectid, oper, objectname, oldval, newval)
           VALUES (NEW.ArcID, 'I','md_node',Null
                   ,jsonb_build_object('FromID',NEWfromNode_.Name||' ('||NEW.FromID||')'
                                     ,'ToID',NEWtoNode_.Name||' ('||NEW.ToID||')'
                                     ,'TypeID',NEWtype_.Name||' ('||NEW.TypeID||')'
                                     ,'SRCID',NEWsrc_.Name||' ('||NEW.SRCID||')'
                                     ,'isCustom',NEW.IsCustom
                                     ,'isDeleted',NEW.IsDeleted
                                     )
                 );
--        raise notice 'INSERT';
        RETURN NEW;
    WHEN 'UPDATE' then
        if OLD.FromID<>NEW.FromID then
            select * into NEWfromNode_ from md.md_node where NodeID=NEW.FromID;
            select * into OLDfromNode_ from md.md_node where NodeID=OLD.FromID;
            OFromIDJB:=jsonb_build_object('FromID',OLDfromNode_.Name||' ('||OLD.FromID||')');
            NFromIDJB:=jsonb_build_object('FromID',NEWfromNode_.Name||' ('||NEW.FromID||')');
        end if;
        if OLD.ToID<>NEW.ToID then
            select * into NEWtoNode_ from md.md_node where NodeID=NEW.ToID;
            select * into OLDtoNode_ from md.md_node where NodeID=OLD.ToID;
            OToIDJB:=jsonb_build_object('ToID',OLDtoNode_.Name||' ('||OLD.ToID||')');
            NToIDJB:=jsonb_build_object('ToID',NEWtoNode_.Name||' ('||NEW.ToID||')');
        end if;
        if OLD.TypeID<>NEW.TypeID then
            select * into NEWtype_ from md.md_type where TypeID=NEW.TypeID;
            select * into OLDtype_ from md.md_type where TypeID=OLD.TypeID;
            OTypeIDJB:=jsonb_build_object('TypeID',OLDtype_.Name||' ('||OLD.TypeID||')');
            NTypeIDJB:=jsonb_build_object('TypeID',NEWtype_.Name||' ('||NEW.TypeID||')');
        end if;
        if OLD.SRCID<>NEW.SRCID then
            select * into NEWsrc_ from md.md_src where SRCID=NEW.SRCID;
            select * into OLDsrc_ from md.md_src where SRCID=OLD.SRCID;
            OSRCIDJB:=jsonb_build_object('SRCID',OLDsrc_.Name||' ('||OLD.SRCID||')');
            NSRCIDJB:=jsonb_build_object('SRCID',NEWsrc_.Name||' ('||NEW.SRCID||')');
        end if;
        if OLD.IsCustom<>NEW.IsCustom then
            OIsCustomJB:=jsonb_build_object('IsCustom',OLD.IsCustom);
            NIsCustomJB:=jsonb_build_object('IsCustom',NEW.IsCustom);
        end if;
        if OLD.IsDeleted<>NEW.IsDeleted then
            OIsDeletedJB:=jsonb_build_object('IsDeleted',OLD.IsDeleted);
            NIsDeletedJB:=jsonb_build_object('IsDeleted',NEW.IsDeleted);
        end if;
--        raise notice 'UPDATE';
        insert into md.md_audit(objectid, oper, objectname, oldval, newval)
           VALUES (NEW.ArcID, 'U','md_node',OFromIDJB||OToIDJB||OTypeIDJB||OSRCIDJB||OIsCustomJB||OIsDeletedJB
                                           ,NFromIDJB||NToIDJB||NTypeIDJB||NSRCIDJB||NIsCustomJB||NIsDeletedJB);
        RETURN NEW;
    WHEN 'DELETE' then
        select * into OLDfromNode_ from md.md_node where NodeID=OLD.FromID;
        select * into OLDtoNode_   from md.md_node where NodeID=OLD.toID;
        select * into OLDtype_     from md.md_type where TypeID=OLD.TypeID;
        select * into OLDsrc_      from md.md_src  where SRCID=OLD.SRCID;
        insert into md.md_audit(objectid, oper, objectname, oldval, newval)
           VALUES (OLD.ArcID, 'D','md_node'
                   ,jsonb_build_object('FromID',OLDfromNode_.Name||' ('||OLD.FromID||')'
                                     ,'ToID',OLDtoNode_.Name||' ('||OLD.ToID||')'
                                     ,'TypeID',OLDtype_.Name||' ('||OLD.TypeID||')'
                                     ,'SRCID',OLDsrc_.Name||' ('||OLD.SRCID||')'
                                     ,'isCustom',OLD.IsCustom
                                     ,'isDeleted',OLD.IsDeleted
                                     )
                   ,null
                 );
--        raise notice 'DELETE';
        RETURN OLD;
    ELSE
        raise notice 'OP=%',TG_OP;
    END CASE;
--    select ccfa_get_formkey(NEW.id), ccfa_get_formver(NEW.id), ccfa_get_appver(NEW.id) into formkey_, formver_,appver_;
END;
$$;

alter function md.md_arc_tr owner to md;

CREATE TRIGGER MD_Arc_TR
    BEFORE UPDATE OR INSERT OR DELETE
    ON md.md_Arc
    FOR EACH ROW
    EXECUTE PROCEDURE md.md_Arc_TR();

DROP TRIGGER IF EXISTS MD_Node_TR on md.md_Node cascade;
DROP FUNCTION IF EXISTS md.md_Node_TR;
CREATE OR REPLACE FUNCTION md.md_Node_TR()
  RETURNS TRIGGER
  LANGUAGE PLPGSQL
  AS
$$
DECLARE
    OLDtype_ md.md_type%rowtype;
    NEWtype_ md.md_type%rowtype;
    OLDsrc_  md.md_SRC%rowtype;
    NEWsrc_  md.md_src%rowtype;
    NNameJB       jsonb:='{}';
    NSynonymJB    jsonb:='{}';
    NTypeIDJB     jsonb:='{}';
    NSRCIDJB      jsonb:='{}';
    NisDeletedJB  jsonb:='{}';
    NisCustomJB   jsonb:='{}';
    ONameJB       jsonb:='{}';
    OSynonymJB    jsonb:='{}';
    OTypeIDJB     jsonb:='{}';
    OSRCIDJB      jsonb:='{}';
    OisDeletedJB  jsonb:='{}';
    OisCustomJB   jsonb:='{}';
BEGIN
    IF NEW.NodeID is null then
        NEW.NodeID:=nextval('md.md_seq');
    end if;
    NEW.TSChanged:=current_timestamp;
    CASE TG_OP
    WHEN 'INSERT' then
        NEW.TSCreated:=current_timestamp;
        select * into NEWtype_   from md.md_type where TypeID=NEW.TypeID;
        select * into NEWsrc_    from md.md_src  where SRCID=NEW.SRCID;
        insert into md.md_audit(objectid, oper, objectname, oldval, newval)
           VALUES (NEW.NodeID, 'I','md_arc', Null
                   ,jsonb_build_object('Name',NEW.Name
                                     ,'TypeID',NEWtype_.Name||' ('||NEW.TypeID||')'
                                     ,'SRCID',NEWsrc_.Name||' ('||NEW.SRCID||')'
                                     ,'Synonym',NEW.Synonym
                                     ,'isCustom',NEW.IsCustom
                                     ,'isDeleted',NEW.IsDeleted
                                     )
                 );
--        raise notice 'INSERT';
        RETURN NEW;
    WHEN 'UPDATE' then
        if OLD.TypeID<>NEW.TypeID then
            select * into OLDtype_   from md.md_type where TypeID=OLD.TypeID;
            select * into NEWtype_   from md.md_type where TypeID=NEW.TypeID;
            OTypeIDJB:=jsonb_build_object('TypeID',OLDtype_.Name||' ('||OLD.TypeID||')');
            NTypeIDJB:=jsonb_build_object('TypeID',NEWtype_.Name||' ('||NEW.TypeID||')');
        end if;
        if OLD.SRCID<>NEW.SRCID then
            select * into OLDsrc_   from md.md_src where SRCID=OLD.SRCID;
            select * into NEWsrc_   from md.md_src where SRCID=NEW.SRCID;
            OSRCIDJB:=jsonb_build_object('SRCID',OLDsrc_.Name||' ('||OLD.SRCID||')');
            NSRCIDJB:=jsonb_build_object('SRCID',NEWsrc_.Name||' ('||NEW.SRCID||')');
        end if;
        if OLD.Name<>NEW.Name then
            ONameJB:=jsonb_build_object('Name',OLD.Name);
            NNameJB:=jsonb_build_object('Name',NEW.Name);
        end if;
        if OLD.Synonym<>NEW.Synonym then
            OSynonymJB:=jsonb_build_object('Synonym',OLD.Synonym);
            NSynonymJB:=jsonb_build_object('Synonym',NEW.Synonym);
        end if;
        if OLD.IsCustom<>NEW.IsCustom then
            OIsCustomJB:=jsonb_build_object('IsCustom',OLD.IsCustom);
            NIsCustomJB:=jsonb_build_object('IsCustom',NEW.IsCustom);
        end if;
        if OLD.IsDeleted<>NEW.IsDeleted then
            OIsDeletedJB:=jsonb_build_object('IsDeleted',OLD.IsDeleted);
            NIsDeletedJB:=jsonb_build_object('IsDeleted',NEW.IsDeleted);
        end if;
        insert into md.md_audit(objectid, oper, objectname, oldval, newval)
           VALUES (NEW.NodeID,'U','md_arc',ONameJB||OSynonymJB||OTypeIDJB||OSRCIDJB||OIsCustomJB||OIsDeletedJB
                                          ,NNameJB||NSynonymJB||NTypeIDJB||NSRCIDJB||NIsCustomJB||NIsDeletedJB);
--        raise notice 'UPDATE';
        RETURN NEW;
    WHEN 'DELETE' then
        select * into OLDtype_   from md.md_type where TypeID=OLD.TypeID;
        select * into OLDsrc_    from md.md_src  where SRCID=OLD.SRCID;
        insert into md.md_audit(objectid, oper, objectname, oldval, newval)
           VALUES (OLD.NodeID, 'D','md_arc'
                   ,jsonb_build_object('Name',OLD.Name
                                     ,'TypeID',OLDtype_.Name||' ('||OLD.TypeID||')'
                                     ,'SRCID',OLDsrc_.Name||' ('||OLD.SRCID||')'
                                     ,'Synonym',OLD.Synonym
                                     ,'isCustom',OLD.IsCustom
                                     ,'isDeleted',OLD.IsDeleted
                                     )
                   ,Null
                 );
--        raise notice 'DELETE';
        RETURN OLD;
    ELSE
        raise notice 'OP=%',TG_OP;
        RETURN NEW;
    END CASE;
--    select ccfa_get_formkey(NEW.id), ccfa_get_formver(NEW.id), ccfa_get_appver(NEW.id) into formkey_, formver_,appver_;
END;
$$;

alter function md.md_node_tr owner to md;

CREATE TRIGGER MD_Node_TR
    BEFORE UPDATE OR INSERT OR DELETE
    ON md.md_Node
    FOR EACH ROW
    EXECUTE PROCEDURE md.md_Node_TR();

DROP TRIGGER IF EXISTS MD_Audit_TR on md.md_Audit cascade;
DROP FUNCTION IF EXISTS md.md_Audit_TR;
CREATE OR REPLACE FUNCTION md.md_Audit_TR()
  RETURNS TRIGGER
  LANGUAGE PLPGSQL
  AS
$$
BEGIN
    IF NEW.AudID is null then
        NEW.AudID:=nextval('md.md_audit_seq');
    end if;
    IF NEW.AUser is null then
        NEW.AUser:=current_user;
    end if;
    NEW.TSAudit:=current_timestamp;
    RETURN NEW;
END;
$$;

alter function md.md_audit_tr owner to md;

CREATE TRIGGER MD_Audit_TR
    BEFORE INSERT
    ON md.md_Audit
    FOR EACH ROW
    EXECUTE PROCEDURE md.md_Audit_TR();

DROP TRIGGER IF EXISTS md_Node_Attr_Val_TR on  md.md_Node_Attr_Val cascade;
DROP FUNCTION IF EXISTS  md.md_Node_Attr_Val_TR;
CREATE OR REPLACE FUNCTION  md.md_Node_Attr_Val_TR()
  RETURNS TRIGGER
  LANGUAGE PLPGSQL
  AS
$$
DECLARE
    NEWNode md.md_Node%rowtype;
    OLDNode md.md_Node%rowtype;
    NEWAttr md.md_Attr%rowtype;
    OLDAttr md.md_Attr%rowtype;
    NEWsrc  md.md_SRC%rowtype;
    OLDsrc  md.md_SRC%rowtype;
    NAttrIDJB     jsonb:='{}';
    NValJB        jsonb:='{}';
    NNodeIDJB     jsonb:='{}';
    NSRCIDJB      jsonb:='{}';
    NisDeletedJB  jsonb:='{}';
    NisCustomJB   jsonb:='{}';
    OAttrIDJB     jsonb:='{}';
    OValJB        jsonb:='{}';
    ONodeIDJB     jsonb:='{}';
    OSRCIDJB      jsonb:='{}';
    OisDeletedJB  jsonb:='{}';
    OisCustomJB   jsonb:='{}';
BEGIN
    IF NEW.NAVID is null then
        NEW.NAVID:=nextval('md.md_seq');
    end if;
    NEW.TSChanged:=current_timestamp;
    CASE TG_OP
    WHEN 'INSERT' then
        NEW.TSCreated:=current_timestamp;
        select * into NEWAttr   from md.md_attr where AttrID=NEW.AttrID;
        select * into NEWNode   from md.md_Node where NodeID=NEW.NodeID;
        select * into NEWSRC    from md.md_SRC  where SRCID=NEW.SRCID;
        insert into md.md_audit(objectid, oper, objectname, oldval, newval)
           VALUES (NEW.NAVID, 'I','md_node_attr_val',Null
                   ,jsonb_build_object('AttrID',NEWattr.Name||' (key='||NEWattr.key||')'
                                     ,'NodeID',NEWNode.Name||' (NodeID='||NEW.NodeID||')'
                                     ,'SRCID',NEWSRC.Name||' (SRCID='||NEW.SRCID||')'
                                     ,'Val',NEW.Val
                                     ,'isCustom',NEW.IsCustom
                                     ,'isDeleted',NEW.IsDeleted
                                     )
                 );
--        raise notice 'INSERT';
        RETURN NEW;
    WHEN 'UPDATE' then
        if OLD.AttrID<>NEW.AttrID then
            select * into NEWAttr   from md.md_attr where AttrID=NEW.AttrID;
            select * into OLDAttr   from md.md_attr where AttrID=OLD.AttrID;
            OAttrIDJB:=jsonb_build_object('AttrID',OLDattr.Name||' (key='||OLDattr.key||')');
            NAttrIDJB:=jsonb_build_object('AttrID',NEWattr.Name||' (key='||NEWattr.key||')');
        end if;
        if OLD.NodeID<>NEW.NodeID then
            select * into NEWNode   from md.md_node where NodeID=NEW.NodeID;
            select * into OLDNode   from md.md_node where NodeID=OLD.NodeID;
            ONodeIDJB:=jsonb_build_object('NodeID',OLDNode.Name||' (NodeID='||OLD.NodeID||')');
            NNodeIDJB:=jsonb_build_object('NodeID',NEWNode.Name||' (NodeID='||NEW.NodeID||')');
        end if;
        if OLD.Val<>NEW.Val then
            OValJB:=jsonb_build_object('Val',OLD.Val);
            NValJB:=jsonb_build_object('Val',NEW.Val);
        end if;
        if OLD.SRCID<>NEW.SRCID then
            select * into OLDSRC   from md.md_src where SRCID=OLD.SRCID;
            select * into NEWSRC   from md.md_src where SRCID=NEW.SRCID;
            OSRCIDJB:=jsonb_build_object('SRCID',OLDSRC.Name||' ('||OLD.SRCID||')');
            NSRCIDJB:=jsonb_build_object('SRCID',NEWSRC.Name||' ('||NEW.SRCID||')');
        end if;
        if OLD.IsCustom<>NEW.IsCustom then
            OIsCustomJB:=jsonb_build_object('IsCustom',OLD.IsCustom);
            NIsCustomJB:=jsonb_build_object('IsCustom',NEW.IsCustom);
        end if;
        if OLD.IsDeleted<>NEW.IsDeleted then
            OIsDeletedJB:=jsonb_build_object('IsDeleted',OLD.IsDeleted);
            NIsDeletedJB:=jsonb_build_object('IsDeleted',NEW.IsDeleted);
        end if;
        insert into md.md_audit(objectid, oper, objectname, oldval, newval)
           VALUES (NEW.NAVID,'U','md_node_attr_val',OAttrIDJB||ONodeIDJB||OValJB||OSRCIDJB||OIsCustomJB||OIsDeletedJB
                                                   ,NAttrIDJB||NNodeIDJB||NValJB||NSRCIDJB||NIsCustomJB||NIsDeletedJB);
--        raise notice 'UPDATE';
        RETURN NEW;
    WHEN 'DELETE' then
        select * into OLDAttr   from md.md_attr where AttrID=OLD.AttrID;
        select * into OLDNode   from md.md_Node where NodeID=OLD.NodeID;
        select * into OLDsrc    from md.md_src  where SRCID=OLD.SRCID;
        insert into md.md_audit(objectid, oper, objectname, oldval, newval)
           VALUES (OLD.NAVID, 'D','md_node_attr_val'
                   ,jsonb_build_object('AttrID',OLDattr.Name||' (key='||OLDattr.key||')'
                                     ,'NodeID',OLDNode.Name||' (NodeID='||OLD.NodeID||')'
                                     ,'SRCID',OLDSRC.Name||' (SRCID='||OLD.SRCID||')'
                                     ,'Val',OLD.Val
                                     ,'isCustom',OLD.IsCustom
                                     ,'isDeleted',OLD.IsDeleted
                                     )
                   ,Null
                 );
--        raise notice 'DELETE';
        RETURN OLD;
    ELSE
        raise notice 'OP=%',TG_OP;
        RETURN NEW;
    END CASE;
END;
$$;

alter function md.md_node_attr_val_tr owner to md;

CREATE TRIGGER MD_Node_Attr_Val_TR
    BEFORE UPDATE OR INSERT OR DELETE
    ON  md.md_Node_Attr_Val
    FOR EACH ROW
    EXECUTE PROCEDURE  md.md_Node_Attr_Val_TR();


DROP TRIGGER IF EXISTS MD_Arc_Attr_Val_TR on  md.md_Arc_Attr_Val cascade;
DROP FUNCTION IF EXISTS  md.md_Arc_Attr_Val_TR;
CREATE OR REPLACE FUNCTION  md.md_Arc_Attr_Val_TR()
  RETURNS TRIGGER
  LANGUAGE PLPGSQL
  AS
$$
DECLARE
    NEWArcType md.md_Type%rowtype;
    OLDArcType md.md_Type%rowtype;
    NEWAttr md.md_Attr%rowtype;
    OLDAttr md.md_Attr%rowtype;
    NEWsrc  md.md_SRC%rowtype;
    OLDsrc  md.md_SRC%rowtype;
    NAttrIDJB     jsonb:='{}';
    NValJB        jsonb:='{}';
    NArcIDJB      jsonb:='{}';
    NSRCIDJB      jsonb:='{}';
    NisDeletedJB  jsonb:='{}';
    NisCustomJB   jsonb:='{}';
    OAttrIDJB     jsonb:='{}';
    OValJB        jsonb:='{}';
    OArcIDJB      jsonb:='{}';
    OSRCIDJB      jsonb:='{}';
    OisDeletedJB  jsonb:='{}';
    OisCustomJB   jsonb:='{}';
BEGIN
    IF NEW.AAVID is null then
        NEW.AAVID:=nextval('md.md_seq');
    end if;
    NEW.TSChanged:=current_timestamp;
    CASE TG_OP
    WHEN 'INSERT' then
        NEW.TSCreated:=current_timestamp;
        select * into NEWAttr   from md.md_attr where AttrID=NEW.AttrID;
        select t.* into NEWArcType from md.md_Arc a, md.md_Type t where a.typeid=t.typeid and a.ArcID=NEW.ArcID;
        select * into NEWSRC    from md.md_SRC  where SRCID=NEW.SRCID;
        insert into md.md_audit(objectid, oper, objectname, oldval, newval)
           VALUES (NEW.AAVID, 'I','md_arc_attr_val', Null
                   ,jsonb_build_object('AttrID',NEWattr.Name||' (key='||NEWattr.key||')'
                                     ,'ArcID',NEWArcType.key||' (ArcID='||NEW.ArcID||')'
                                     ,'SRCID',NEWSRC.Name||' (SRCID='||NEW.SRCID||')'
                                     ,'Val',NEW.Val
                                     ,'isCustom',NEW.IsCustom
                                     ,'isDeleted',NEW.IsDeleted
                                     )
                 );
--        raise notice 'INSERT';
        RETURN NEW;
    WHEN 'UPDATE' then
        if OLD.AttrID<>NEW.AttrID then
            select * into NEWAttr   from md.md_attr where AttrID=NEW.AttrID;
            select * into OLDAttr   from md.md_attr where AttrID=OLD.AttrID;
            OAttrIDJB:=jsonb_build_object('AttrID',OLDattr.Name||' (key='||OLDattr.key||')');
            NAttrIDJB:=jsonb_build_object('AttrID',NEWattr.Name||' (key='||NEWattr.key||')');
        end if;
        if OLD.ArcID<>NEW.ArcID then
            select * into NEWArcType   from md.md_Arc where ArcID=NEW.ArcID;
            select * into OLDArcType   from md.md_Arc where ArcID=OLD.ArcID;
            OArcIDJB:=jsonb_build_object('ArcID',OLDArcType.key||' (ArcID='||OLD.ArcID||')');
            NArcIDJB:=jsonb_build_object('ArcID',NEWArcType.key||' (ArcID='||NEW.ArcID||')');
        end if;
        if OLD.Val<>NEW.Val then
            OValJB:=jsonb_build_object('Val',OLD.Val);
            NValJB:=jsonb_build_object('Val',NEW.Val);
        end if;
        if OLD.SRCID<>NEW.SRCID then
            select * into OLDSRC   from md.md_src where SRCID=OLD.SRCID;
            select * into NEWSRC   from md.md_src where SRCID=NEW.SRCID;
            OSRCIDJB:=jsonb_build_object('SRCID',OLDSRC.Name||' ('||OLD.SRCID||')');
            NSRCIDJB:=jsonb_build_object('SRCID',NEWSRC.Name||' ('||NEW.SRCID||')');
        end if;
        if OLD.IsCustom<>NEW.IsCustom then
            OIsCustomJB:=jsonb_build_object('IsCustom',OLD.IsCustom);
            NIsCustomJB:=jsonb_build_object('IsCustom',NEW.IsCustom);
        end if;
        if OLD.IsDeleted<>NEW.IsDeleted then
            OIsDeletedJB:=jsonb_build_object('IsDeleted',OLD.IsDeleted);
            NIsDeletedJB:=jsonb_build_object('IsDeleted',NEW.IsDeleted);
        end if;
        insert into md.md_audit(objectid, oper, objectname, oldval, newval)
           VALUES (NEW.AAVID, 'U','md_arc_attr_val',OAttrIDJB||OArcIDJB||OValJB||OSRCIDJB||OIsCustomJB||OIsDeletedJB
                                                   ,NAttrIDJB||NArcIDJB||NValJB||NSRCIDJB||NIsCustomJB||NIsDeletedJB);
--        raise notice 'UPDATE';
        RETURN NEW;
   WHEN 'DELETE' then
        select * into OLDAttr   from md.md_attr where AttrID=OLD.AttrID;
        select t.* into OLDArcType from md.md_Arc a, md.md_Type t where a.typeid=t.typeid and a.ArcID=OLD.ArcID;
        select * into OLDsrc    from md.md_src  where SRCID=OLD.SRCID;
        insert into md.md_audit(objectid, oper, objectname, oldval, newval)
           VALUES (NEW.AAVID, 'D','md_arc_attr_val'
                   ,jsonb_build_object('AttrID',OLDattr.Name||' (key='||OLDattr.key||')'
                                     ,'ArcID',OLDArcType.key||' (ArcID='||OLD.ArcID||')'
                                     ,'SRCID',OLDSRC.Name||' (SRCID='||OLD.SRCID||')'
                                     ,'Val',OLD.Val
                                     ,'isCustom',OLD.IsCustom
                                     ,'isDeleted',OLD.IsDeleted
                                     )
                   ,Null
                 );

--        raise notice 'DELETE';
        RETURN OLD;
    ELSE
        raise notice 'OP=%',TG_OP;
        RETURN NEW;
    END CASE;
END;
$$;

alter function md.md_arc_attr_val_tr owner to md;


CREATE TRIGGER MD_Arc_Attr_Val_TR
    BEFORE UPDATE OR INSERT OR DELETE
    ON  md.md_Arc_Attr_Val
    FOR EACH ROW
    EXECUTE PROCEDURE  md.md_Arc_Attr_Val_TR();


DROP FUNCTION IF EXISTS md.md_Get_Arc_SRC;
CREATE OR REPLACE FUNCTION md.md_Get_Arc_SRC(parcid md.md_arc.arcid%type)
  RETURNS md.md_node.name%type
  LANGUAGE PLPGSQL
  AS
$$
declare
    src1_ md.md_node.name%type;
    src2_ md.md_node.name%type;
BEGIN
    select md.md_Get_Node_SRC(a.FromID), md.md_Get_Node_SRC(a.ToID) into src1_, src2_ from md.md_arc a where a.ArcID=parcid;
    if src1_=src2_ then
        return src1_;
    else
       return 'Different SRCs!';
    end if;
END;
$$

alter function md.md_Get_Arc_SRC owner to md;


DROP FUNCTION IF EXISTS md.md_Get_Node_SRC;
CREATE OR REPLACE FUNCTION md.md_Get_Node_SRC(pnodeid md.md_node.nodeid%type)
  RETURNS md.md_node.name%type
  LANGUAGE PLPGSQL
  IMMUTABLE
  AS
$$
declare
    key_ md.md_type.key%type;
    name_ md.md_node.name%type;
    nodeid_ md.md_node.nodeid%type;
    oldi_ md.md_node.name%type;
    i record;
BEGIN
    select t.key, n.name into key_, name_ from md.md_node n, md.md_type t where n.typeid=t.typeid and n.nodeid=pnodeid;
    raise notice 'ID=%, KEY=%, name=%', pnodeid, key_, name_;
    if key_ = 'SRC' then
        return name_;
    elsif key_ ='ForeignKey' then
        for i in (select md.md_Get_Node_SRC(n.nodeid) as name from md.md_arc a, md.md_type ta, md.md_node n, md.md_type tn
                      where a.typeid=ta.typeid and ta.key='ForeignKey'
                        and n.typeid=tn.typeid and tn.key='Column'
                        and ((a.fromid=pnodeid and a.toid=n.nodeid) or (a.toid=pnodeid and a.fromid=n.nodeid)) --(Arc to Table)
                    ) loop
            raise notice 'I=%', i.name;
            if oldi_ is null then
                raise notice 'Start OLDI=%', i.name;
                oldi_:=i.name;
            end if;
            if oldi_<>i.name then
                return 'Different SRCs!';
            end if;
        end loop;
        return oldi_;
    elsif key_ ='Index' then
        select n.nodeid into nodeid_ from md.md_arc a, md.md_type ta, md.md_node n, md.md_type tn
          where a.typeid=ta.typeid and ta.key='Index'
            and n.typeid=tn.typeid and tn.key='Column'
            and ((a.fromid=pnodeid and a.toid=n.nodeid) or (a.toid=pnodeid and a.fromid=n.nodeid)) --(Arc to Table)
          limit 1;
        return md.md_Get_Node_SRC(nodeid_);
    elsif key_ ='Check' then
        select n.nodeid into nodeid_ from md.md_arc a, md.md_type ta, md.md_node n, md.md_type tn
          where a.typeid=ta.typeid and ta.key='Check'
            and n.typeid=tn.typeid and tn.key='Column'
            and ((a.fromid=pnodeid and a.toid=n.nodeid) or (a.toid=pnodeid and a.fromid=n.nodeid)) --(Arc to Table)
          limit 1;
        return md.md_Get_Node_SRC(nodeid_);
    elsif key_ ='Unique' then
        select n.nodeid into nodeid_ from md.md_arc a, md.md_type ta, md.md_node n, md.md_type tn
          where a.typeid=ta.typeid and ta.key='Unique'
            and n.typeid=tn.typeid and tn.key='Column'
            and ((a.fromid=pnodeid and a.toid=n.nodeid) or (a.toid=pnodeid and a.fromid=n.nodeid)) --(Arc to Table)
          limit 1;
        return md.md_Get_Node_SRC(nodeid_);
    elsif key_ ='PrimaryKey' then
        select n.nodeid into nodeid_ from md.md_arc a, md.md_type ta, md.md_node n, md.md_type tn
          where a.typeid=ta.typeid and ta.key='PrimaryKey'
            and n.typeid=tn.typeid and tn.key='Column'
            and ((a.fromid=pnodeid and a.toid=n.nodeid) or (a.toid=pnodeid and a.fromid=n.nodeid)) --(Arc to Table)
          limit 1;
        return md.md_Get_Node_SRC(nodeid_);
    elsif key_='Column' then
        select n.* into nodeid_ from md.md_arc a, md.md_type ta, md.md_node n, md.md_type tn
          where a.typeid=ta.typeid and ta.key='Column2Table'
            and n.typeid=tn.typeid and tn.key='Table'
            and a.fromid=pnodeid --(Arc to Table)
            and a.toid=n.nodeid;
        return md.md_Get_Node_SRC(nodeid_);
    elsif key_='Table' then
        select n.* into nodeid_ from md.md_arc a, md.md_type ta, md.md_node n, md.md_type tn
          where a.typeid=ta.typeid and ta.key='Table2SRC'
            and n.typeid=tn.typeid and tn.key='SRC'
            and a.fromid=pnodeid --(Arc to Table)
            and a.toid=n.nodeid;
        return md.md_Get_Node_SRC(nodeid_);
    elsif key_='View' then
        select n.* into nodeid_ from md.md_arc a, md.md_type ta, md.md_node n, md.md_type tn
          where a.typeid=ta.typeid and ta.key='View2SRC'
            and n.typeid=tn.typeid and tn.key='SRC'
            and a.fromid=pnodeid --(Arc to Table)
            and a.toid=n.nodeid;
        return md.md_Get_Node_SRC(nodeid_);
    end if;
    return 'NONE!!!!';
END;
$$

alter function md.md_Get_Node_SRC owner to md;

DROP FUNCTION IF EXISTS md.md_Get_Type;
CREATE OR REPLACE FUNCTION md.md_Get_Type(pkey md.md_type.key%type)
  RETURNS md.md_type.typeid%type
  LANGUAGE PLPGSQL
  AS
$$
BEGIN
    return (select typeid from md.md_type where key=pkey limit 1);
END;
$$;

alter function md.md_Get_Type owner to md;


DROP FUNCTION IF EXISTS md.md_get_attr;
create function md.md_get_attr(pparentkey md.md_attr.key%type,pkey md.md_attr.key%type) returns md.md_attr.attrid%type
	language plpgsql
as $$
Declare
    attrid_        md.md_attr.attrid%type;
    parentattrid_  md.md_attr.attrid%type;
BEGIN
    select attrid into parentattrid_ from md.md_attr where key=pparentkey limit 1;
    if parentattrid_ is null then
        insert into md.md_attr(attrpid, key, name) values (0, pparentkey, pparentkey) returning attrid into parentattrid_;
    end if;
    select attrid into attrid_ from md.md_attr where key=pkey and attrpid=parentattrid_ limit 1;
    if attrid_ is null then
        insert into md.md_attr(attrpid, key, name) values (parentattrid_, pkey, pkey) returning attrid into attrid_;
    end if;
    return attrid_;
END;
$$;

alter function md.md_Get_Attr owner to md;


DROP FUNCTION IF EXISTS md.md_get_parameter;
create or replace function md.md_get_parameter( /* получить значение параметра, если его нет - выдать NULL */
  in pname      md.md_Parameter.name%type        -- Имя существующего параметра
) returns md.md_Parameter.val%type as $$
begin
    return (select val from md.md_parameter where name=pname);
end
$$ LANGUAGE plpgsql;

DROP FUNCTION IF EXISTS md.md_set_parameter;
create or replace function md.md_set_parameter( /* установить значение параметра, если его нет - ошибка */
  in pname      md.md_Parameter.name%type,        -- Имя существующего параметра
  in pval       md.md_Parameter.val%type          -- Новое значение, если null - взять default
) returns void as $$
declare
    updated int;
begin
    update md.md_parameter set val=coalesce(pval, dval) where name=pname;
    GET DIAGNOSDummyProtocol1S updated = ROW_COUNT;
    if updated=0 then
        raise exception 'Parameter with name % is not found!', pname;
    end if;
end
$$ LANGUAGE plpgsql;

alter function md.md_Get_parameter owner to md;


DROP FUNCTION IF EXISTS md.md_merge_tmp_node;
create or replace function md.md_merge_tmp_node( /* Добавить или обновить TMP_Node по Name-SrcID */
  in psrcid      md.md_node.srcid%type,        -- Идентификатор источника данных
  in pname       md.md_node.name%type,         -- Наименование узла
  in ptype       md.md_type.key%type           -- Обозначение типа
) returns integer as $$
declare
    nodeid_ int;
begin
    select NodeID into nodeid_ from TMP_Node where srcid=psrcid and name=pname and typeid=md_get_type(ptype);
    if nodeid_ is null then
        insert into TMP_Node(nodeid, name, typeid, srcid, IsChanged)
               values (nextval('md_seq'), pname, md_get_type(ptype), psrcid, 2) returning NodeID into nodeid_;
    else
        update TMP_Node set  IsChanged=1 where NodeID=nodeid_;
    end if;
    return nodeid_;
end
$$ LANGUAGE plpgsql;

alter function md.md_merge_tmp_node owner to md;

DROP FUNCTION IF EXISTS md.md_merge_tmp_arc;
create or replace function md.md_merge_tmp_arc( /* Добавить или обновить TMP_Arc по FromID-ToID-SrcID */
  in psrcid      md.md_arc.srcid%type,         -- Идентификатор источника данных
  in pfromid     md.md_arc.fromid%type,        -- Идентификатор узла From
  in ptoid       md.md_arc.toid%type,          -- Идентификатор узла To
  in ptype       md.md_type.key%type           -- Обозначение типа
) returns integer as $$
declare
    arcid_ int;
begin
    select ArcID into arcid_ from TMP_Arc where srcid=psrcid and FromID=pfromid and TOID=ptoid and typeid=md_get_type(ptype);
    if arcid_ is null then
        insert into TMP_Arc(ArcID, FromID, ToID, typeid, srcid, IsChanged)
            values (nextval('md_seq'), pfromid, ptoid, md_get_type(ptype), psrcid, 2)  returning ArcID into arcid_;
    else
        update TMP_Arc set  IsChanged=1 where ArcID=arcid_;
    end if;
    return arcid_;
end
$$ LANGUAGE plpgsql;

alter function md.md_merge_tmp_arc owner to md;


DROP FUNCTION IF EXISTS md.md_merge_tmp_nav;
create or replace function md.md_merge_tmp_nav( /* Добавить или обновить TMP_Node_Attr_Val по NodeID-PAttr-Attr-SrcID */
  in psrcid      md.md_node_attr_val.srcid%type,    -- Идентификатор источника данных
  in pnodeid     md.md_node_attr_val.nodeid%type,   -- Идентификатор Node
  in ppattr      md.md_attr.key%type,           -- Parent Идентификатор атрибута
  in pattr       md.md_attr.key%type,            -- Идентификатор атрибута
  in pval        md.md_node_attr_val.val%type       -- Значение атрибута
) returns integer as $$
declare
    navid_ int;
begin
    select NAVID into navid_ from TMP_Node_Attr_Val where srcid=psrcid and NodeId=pnodeid and AttrId=md_get_attr(ppattr, pattr);
    if navid_ is null then
        insert into TMP_Node_Attr_Val(NAVID, NodeID, AttrID, Val, srcid, IsChanged)
            values (nextval('md_seq'), pnodeid, md_get_attr(ppattr, pattr),pval,psrcid, 2) returning NAVID into navid_;
    else
        update TMP_Node_Attr_Val set Val=pval, IsChanged=1 where NAVID=navid_ and srcid=psrcid;
    end if;
    return navid_;
end
$$ LANGUAGE plpgsql;

alter function md.md_merge_tmp_nav owner to md;

DROP FUNCTION IF EXISTS md.md_merge_tmp_aav;
create or replace function md.md_merge_tmp_aav( /* Добавить или обновить TMP_Arc_Attr_Val по ArcID-PAttr-Attr-SrcID */
  in psrcid      md.md_arc_attr_val.srcid%type,   -- Идентификатор источника данных
  in parcid      md.md_arc_attr_val.arcid%type,   -- Идентификатор Arc
  in ppattr      md.md_attr.key%type,             -- Parent Идентификатор атрибута
  in pattr       md.md_attr.key%type,             -- Идентификатор атрибута
  in pval        md.md_node_attr_val.val%type     -- Значение атрибута
) returns integer as $$
declare
    aavid_ int;
begin
    select AAVID into aavid_ from TMP_Arc_Attr_Val where srcid=psrcid and ArcId=parcid and AttrId=md_get_attr(ppattr, pattr);
    if aavid_ is null then
        insert into TMP_Arc_Attr_Val(AAVID, ArcID, AttrID, Val, srcid, IsChanged)
            values (nextval('md_seq'), parcid, md_get_attr(ppattr, pattr),pval,psrcid, 2) returning AAVID into aavid_;
    else
        update TMP_Arc_Attr_Val set Val=pval, IsChanged=1 where AAVID=aavid_ and srcid=psrcid;
    end if;
    return aavid_;
end
$$ LANGUAGE plpgsql;

alter function md.md_merge_tmp_aav owner to md;

DROP FUNCTION IF EXISTS md.md_merge_tmp_into_mddb;
create or replace function md.md_merge_tmp_into_mddb() /* Все данные из TMP таблиц перемещаем в основные */
returns void as $$
begin
    -- Inserted or Updated objects
    insert into md_node(nodeid, srcid, name, typeid)
        select n.nodeid, n.srcid, n.name, n.typeid
            from tmp_node n inner join md_type t on (t.typeid=n.typeid)
            where not (coalesce(t.iscustom,False) or coalesce(n.iscustom,False)) and n.ischanged in (1,2)
        on conflict (nodeid) do update set name=excluded.name;
    insert into md_arc(arcid, srcid, fromid, toid, typeid)
        select a.arcid, a.srcid, a.fromid, a.toid, a.typeid
            from tmp_arc a inner join md_type t on (t.typeid=a.typeid)
            where not (coalesce(t.iscustom, False) or coalesce(a.iscustom,False)) and a.ischanged in (1,2)
        on conflict (arcid) do update set typeid=excluded.typeid;
    insert into md_node_attr_val(navid, srcid, nodeid, attrid, val)
        select n.navid, n.srcid, n.nodeid, n.attrid, n.val
            from tmp_node_attr_val n inner join md_attr t on (t.attrid=n.attrid)
            where not (coalesce(t.iscustom, False) or coalesce(n.iscustom,False)) and n.ischanged in (1,2)
        on conflict (navid) do update set val=excluded.val;
    insert into md_arc_attr_val(aavid, srcid, arcid, attrid, val)
        select a.aavid, a.srcid, a.arcid, a.attrid, a.val
            from tmp_arc_attr_val a inner join md_attr t on (t.attrid=a.attrid)
            where not (coalesce(t.iscustom, False) or coalesce(a.iscustom,False)) and a.ischanged in (1,2)
        on conflict (aavid) do update set val=excluded.val;
    -- DELETED objects
    insert into md_node(nodeid, srcid, name, typeid)
        select n.nodeid, n.srcid, n.name, n.typeid
            from tmp_node n inner join md_type t on (t.typeid=n.typeid)
            where not (coalesce(t.iscustom, False) or coalesce(n.iscustom,False)) and n.ischanged=0
        on conflict (nodeid) do update set isdeleted=true;
    insert into md_arc(arcid, srcid, fromid, toid, typeid)
        select a.arcid, a.srcid, a.fromid, a.toid, a.typeid
            from tmp_arc a inner join md_type t on (t.typeid=a.typeid)
            where not (coalesce(t.iscustom, False) or coalesce(a.iscustom,False)) and a.ischanged=0
        on conflict (arcid) do update set isdeleted=true;
    insert into md_node_attr_val(navid, srcid, nodeid, attrid, val)
        select n.navid, n.srcid, n.nodeid, n.attrid, n.val
            from tmp_node_attr_val n inner join md_attr t on (t.attrid=n.attrid)
            where not (coalesce(t.iscustom, False) or coalesce(n.iscustom,False)) and n.ischanged=0
        on conflict (navid) do update set isdeleted=true;
    insert into md_arc_attr_val(aavid, srcid, arcid, attrid, val)
        select a.aavid, a.srcid, a.arcid, a.attrid, a.val
            from tmp_arc_attr_val a inner join md_attr t on (t.attrid=a.attrid)
            where not (coalesce(t.iscustom, False) or coalesce(a.iscustom,False)) and a.ischanged=0
        on conflict (aavid) do update set isdeleted=true;
end
$$ LANGUAGE plpgsql;

alter function md.md_merge_tmp_into_mddb owner to md;


DROP FUNCTION IF EXISTS md.md_set_sens_attr;
create or replace function md.md_set_sens_attr( /* Установить/снять признак чувствительности данных на колонку */
  in psrcid      md.md_arc_attr_val.srcid%type,   -- Идентификатор источника данных
  in ptable      md.md_node.name%type,            -- Имя таблицы
  in pcolumn     md.md_node.name%type,            -- Имя колонки
  in pval        md.md_arc_attr_val.val%type      -- установить или снять атрибут (False/True)
) returns void as $$
declare
    tableid_  md.md_node.nodeid%type;
    columnid_ md.md_node.nodeid%type;
    navid_    md.md_node_attr_val.navid%type;
begin
    /* находим колонку */
    select n1.nodeid as tableid, n2.nodeid as columnid
        into tableid_, columnid_
        from md_node n1, md_node n2, md_arc a
        where n1.nodeid=a.toid and n2.nodeid=a.fromid and n1.srcid=n2.srcid and n1.srcid=a.srcid
          and n1.typeid=md_get_type('Table') and n2.typeid=md_get_type('Column') and a.typeid=md_get_type('Column2Table')
          and n1.name=ptable /*'progonresults'*/ and n2.name=pcolumn /*'version_id'*/ and n1.srcid=psrcid /* 7 */;
    if columnid_ is not null then
        select NAVID into navid_ from md_Node_Attr_Val where srcid=psrcid and NodeId=columnid_ and AttrId=md_get_attr('Addon', 'SensData');
        if navid_ is null and length(pval)!=0 then
            insert into md_Node_Attr_Val(NAVID, NodeID, AttrID, Val, srcid)
                values (nextval('md_seq'), columnid_, md_get_attr('Addon', 'SensData'),pval,psrcid);
        elsif navid_ is not null and length(pval)=0 then
            delete from md_Node_Attr_Val where navid=navid_ and srcid=psrcid;
        end if;
    else
        raise 'Can"t find %.% at SourceID=%',ptable,pcolumn,psrcid;
    end if;
end
$$ LANGUAGE plpgsql;

alter function md.md_set_sens_attr owner to md;


DROP FUNCTION IF EXISTS md.md_get_sens_attr;
create or replace function md.md_get_sens_attr( /* Выяснить, есть ли признак чувствительности данных на колонку */
  in psrcid      md.md_arc_attr_val.srcid%type,   -- Идентификатор источника данных
  in ptable      md.md_node.name%type,            -- Имя таблицы
  in pcolumn     md.md_node.name%type            -- Имя колонки
) returns boolean as $$
declare
    tableid_  md.md_node.nodeid%type;
    columnid_ md.md_node.nodeid%type;
    navid_    md.md_node_attr_val.navid%type;
begin
    /* находим колонку */
    select n1.nodeid as tableid, n2.nodeid as columnid
        into tableid_, columnid_
        from md_node n1, md_node n2, md_arc a
        where n1.nodeid=a.toid and n2.nodeid=a.fromid and n1.srcid=n2.srcid and n1.srcid=a.srcid
          and n1.typeid=md_get_type('Table') and n2.typeid=md_get_type('Column') and a.typeid=md_get_type('Column2Table')
          and n1.name=ptable /*'progonresults'*/ and n2.name=pcolumn /*'version_id'*/ and n1.srcid=psrcid /* 7 */;
    if columnid_ is not null then
        select NAVID into navid_ from md_Node_Attr_Val where srcid=psrcid and NodeId=columnid_ and AttrId=md_get_attr('Addon', 'SensData');
        if navid_ is null then
            return False;
        else
            return True;
        end if;
    else
        raise 'Can"t find %.% at SourceID=%',ptable,pcolumn,psrcid;
    end if;
end
$$ LANGUAGE plpgsql;

alter function md.md_get_sens_attr owner to md;


DROP FUNCTION IF EXISTS md.md_save_example;
create or replace function md.md_save_example( /* Выяснить, есть ли признак чувствительности данных на колонку */
  in psrcid      md.md_arc_attr_val.srcid%type,  -- Идентификатор источника данных
  in ptable      md.md_node.name%type,           -- Имя таблицы
  in pexample    md.md_example.example%type      -- Значение колонки Example
) returns void as $$
declare
    rec record;
    oval_ text;
    nval_ jsonb;
    nodeid_ md.md_node.nodeid%type;
begin
    /* По всем колонкам */
    for rec in  select n1.nodeid as tableid, n2.name as colname, md_get_sens_attr(psrcid, n1.name, n2.name) as sens
                    from md_node n1, md_node n2, md_arc a
                    where n1.nodeid=a.toid and n2.nodeid=a.fromid and n1.srcid=n2.srcid and n1.srcid=a.srcid
                      and n1.typeid=md_get_type('Table') and n2.typeid=md_get_type('Column') and a.typeid=md_get_type('Column2Table')
                      and n1.name=ptable /*'progonresults'*/ and n1.srcid=psrcid /* 7 */ loop
--        raise notice 'TableID: %, ColName: % (Sens: %), VAL:%', rec.tableid, rec.colname, (rec.sens)::text, pexample;
        if rec.sens then
            oval_:=(pexample->rec.colname)::text;
--            raise notice 'OLD VAL:%', oval_;
            nval_:=json_build_object(rec.colname, encode(pgp_sym_encrypt(oval_, current_setting('DM.Password'), 'cipher-algo=aes256'), 'base64'));
--            raise notice 'NEW VAL:%', nval_;
            pexample:=pexample||nval_;
--            raise notice 'NEW Example:%', pexample;
        end if;
        nodeid_:=rec.tableid;
    end loop;
    insert into md_example(exampleid, nodeid, example) values(nextval('md_seq'),nodeid_, pexample);
end
$$ LANGUAGE plpgsql;

alter function md.md_save_example owner to md;

DROP FUNCTION IF EXISTS md.md_get_synonyms;
create or replace function md.md_get_synonyms( /* выдать все идентификаторы-синонимы заданного объекта + SrcId */
  in pnodeid     md.md_node.nodeid%type         -- Идентификатор объекта
) returns table (
    srcid md.md_node.srcid%type,
    nodeid md.md_node.nodeid%type
    ) as $$
begin
    return query (select n.srcid, n.nodeid from md.md_node n where n.nodeid<>pnodeid and n.synonym=(select synonym from md.md_node n1 where n1.nodeid=pnodeid));
end
$$ LANGUAGE plpgsql;

alter function md.md_get_synonyms owner to md;

DROP FUNCTION IF EXISTS md.md_set_synonyms;
create or replace function md.md_set_synonyms( /* установить синонимическую связь двух объектов, возвращает идентификатор синонима */
  in pnodeid1     md.md_node.nodeid%type,         -- Идентификатор объекта 1
  in pnodeid2     md.md_node.nodeid%type          -- Идентификатор объекта 2
) returns int as $$
declare
    synonym1_ md.md_node.synonym%type;
    synonym2_ md.md_node.synonym%type;
    synonym_  md.md_node.synonym%type;
begin
    select synonym into synonym1_ from md.md_node where nodeid=pnodeid1;
    select synonym into synonym2_ from md.md_node where nodeid=pnodeid2;
    if synonym1_ is null and synonym2_ is null then
        synonym_:=nextval('md.md_synonym_seq');
        update md.md_node set synonym=synonym_ where nodeid in (pnodeid1, pnodeid2);
    elseif synonym1_ is null and synonym2_ is not null then
        update md.md_node set synonym=synonym2_ where nodeid=pnodeid1;
        synonym_:=synonym2_;
    elseif synonym1_ is not null and synonym2_ is null then
        update md.md_node set synonym=synonym1_ where nodeid=pnodeid2;
        synonym_:=synonym1_;
    elseif synonym1_ is not null and synonym2_ is not null then
        if synonym1_<>synonym2_ then
            update md.md_node set synonym=synonym1_ where synonym=synonym2_;
        end if;
        synonym_:=synonym1_;
    end if;
    return synonym_;
end
$$ LANGUAGE plpgsql;

alter function md.md_set_synonyms owner to md;



--insert into MD_Type(TypeID, key, Name) values(nextval('md_seq'), 'SRC',         'Источник данных');
--insert into MD_Type(TypeID, key, Name) values(nextval('md_seq'), 'Scheme',      'Схема БД');
insert into MD_Type(TypeID, key, Name) values(nextval('md_seq'), 'Table',       'Таблица');
insert into MD_Type(TypeID, key, Name) values(nextval('md_seq'), 'Column',      'Колонка Таблицы');
insert into MD_Type(TypeID, key, Name) values(nextval('md_seq'), 'Index' ,      'Индекс');
insert into MD_Type(TypeID, key, Name) values(nextval('md_seq'), 'PrimaryKey',  'Первичный ключ');
insert into MD_Type(TypeID, key, Name) values(nextval('md_seq'), 'Unique',      'Ограничение уникальности');
insert into MD_Type(TypeID, key, Name) values(nextval('md_seq'), 'ForeignKey',  'Внешнее соединение');
insert into MD_Type(TypeID, key, Name) values(nextval('md_seq'), 'Check',       'Ограничение Check');
--insert into MD_Type(TypeID, key, Name) values(nextval('md_seq'), 'Scheme2SRC',  'Связь Схема-Источник');
--insert into MD_Type(TypeID, key, Name) values(nextval('md_seq'), 'Table2SRC','Связь Таблица-Источник');
--insert into MD_Type(TypeID, key, Name) values(nextval('md_seq'), 'Table2Scheme','Связь Таблица-Схема');
insert into MD_Type(TypeID, key, Name) values(nextval('md_seq'), 'Column2Table','Связь Колонка-Таблица');
insert into MD_Type(TypeID, key, Name) values(nextval('md_seq'), 'View',        'Обзор');
insert into MD_Type(TypeID, key, Name) values(nextval('md_seq'), 'View2SRC',    'Связь Обзор-Источник');
insert into MD_Type(TypeID, key, Name) values(nextval('md_seq'), 'Memo',        'Связь напоминание');

insert into MD_Attr(AttrID,AttrPID, key, Name) VALUES (0,0,'Root', 'Root');
insert into MD_Attr(AttrPID, key, Name) VALUES (0,'DataType', 'Тип данных');
insert into MD_Attr(AttrPID, key, Name) VALUES (0,'DataTypeDef', 'Тип данных (определения)');
insert into MD_Attr(AttrPID, key, Name) VALUES (0,'Counstraint', 'Ограничение');
insert into MD_Attr(AttrPID, key, Name) VALUES (0,'Addon', 'Дополнительное поле');
--insert into md_attr(attrpid, key, name) values (0, 'RU', 'Русскоязычные атрибуты');
--insert into md_attr(attrpid, key, name) values (0, 'EN', 'English Attributes');

insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('DataType'),'NUMBER','');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('DataType'),'VARCHAR','');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('DataType'),'CHAR','');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('DataType'),'DATE','');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('DataType'),'BLOB','');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('DataType'),'CLOB','');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('DataType'),'RAW','');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('DataType'),'FLOAT','');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('DataType'),'BINARY_DOUBLE','');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('DataType'),'TIMESTAMP','');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('DataType'),'NullType','');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('DataType'),'INTEGER','');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('DataType'),'LONG','');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('DataTypeDef'),'precision','');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('DataTypeDef'),'scale','');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('DataTypeDef'),'length','');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('Counstraint'),'isNull','');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('Addon'),'Description_DB','');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('Addon'),'DefaultValue','');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('Addon'),'SQLText','');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('Addon'),'Comment','');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('Addon'),'SequenceNumber','Последовательный номер');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('Addon'),'ForeignKeyOptions','Опции создания FK');
insert into MD_Attr(AttrPID, key, Name) VALUES (md_get_attr('Addon'),'Definition','');
--insert into md_attr(attrpid, key, name) values (md_get_attr('RU'), 'Description_RU', 'Русскоязычное описание');
--insert into md_attr(attrpid, key, name) values (md_get_attr('EN'), 'Description_EN', 'English Description');

insert into MD_SRC_Driver(DRIVER, DESCR) values ('oracle+cx_oracle','Oracle');
insert into MD_SRC_Driver(DRIVER, DESCR) values ('postgresql+psycopg2','PostgreSQL');

insert into MD_SRC(DriverID, DSN, Login, Pass, Name, Descr) select DriverID, '(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.75.173)(PORT=1521))(CONNECT_DATA=(SID=DEV3)))', 'TWA', 'TWA','DummySystem3', 'DummySystem3 DB' from MD_SRC_Driver where Descr='Oracle';
insert into MD_SRC(DriverID, DSN, Login, Pass, Name, Descr) select DriverID, '192.168.75.220:5432/fpdb', 'fp', 'rav1234','FP', 'FP DB' from MD_SRC_Driver where Descr='PostgreSQL';
insert into MD_SRC(DriverID, DSN, Login, Pass, Name, Descr) select DriverID, '192.168.75.220:5432/ruledb', 'rules', 'rav1234','RULE', 'RULE DB' from MD_SRC_Driver where Descr='PostgreSQL';
insert into MD_SRC(DriverID, DSN, Login, Pass, Name, Descr) select DriverID, '192.168.75.220:5432/ruledb', 'rules', 'rav1234','RULE1', 'RULE1 DB' from MD_SRC_Driver where Descr='PostgreSQL';
