/******************************************************************
 * File: md_attr_INSERT.sql
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

INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (0, 0, 'Root', 'Root', '2022-07-13 06:06:50.638581 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (1, 0, 'DataType', '��� ������', '2022-07-13 06:06:50.660154 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (2, 0, 'DataTypeDef', '��� ������ (�����������)', '2022-07-13 06:06:50.680254 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (3, 0, 'Constraint', '�����������', '2022-07-13 06:06:50.700324 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (4, 0, 'Addon', '�������������� ����', '2022-07-13 06:06:50.733532 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (5, 1, 'NUMBER', '��� ������ NUMBER', '2022-07-13 06:06:50.755100 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (6, 1, 'VARCHAR', '��� ������ VARCHAR', '2022-07-13 06:06:50.775235 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (7, 1, 'CHAR', '��� ������ CHAR', '2022-07-13 06:06:50.795001 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (8, 1, 'DATE', '��� ������ DATE', '2022-07-13 06:06:50.815876 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (9, 1, 'BLOB', '��� ������ BLOB', '2022-07-13 06:06:50.836792 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (10, 1, 'CLOB', '��� ������ CLOB', '2022-07-13 06:06:50.879718 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (11, 1, 'RAW', '��� ������ RAW', '2022-07-13 06:06:50.901217 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (12, 1, 'FLOAT', '��� ������ FLOAT', '2022-07-13 06:06:50.938084 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (13, 1, 'BINARY_DOUBLE', '��� ������ BINARY DOUBLE', '2022-07-13 06:06:50.959012 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (14, 1, 'TIMESTAMP', '��� ������ TIMESTAMP', '2022-07-13 06:06:50.979277 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (15, 1, 'NullType', '��� ������ NullType', '2022-07-13 06:06:51.006673 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (16, 1, 'INTEGER', '��� ������ INTEGER', '2022-07-13 06:06:51.037550 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (17, 1, 'LONG', '��� ������ LONG', '2022-07-13 06:06:51.068677 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (18, 2, 'Precision', '��� ������ NUMBER, Precision', '2022-07-13 06:06:51.088867 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (19, 2, 'Scale', '��� ������ NUMBER, Scale', '2022-07-13 06:06:51.141798 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (20, 2, 'Length', '��� ������ VARCHAR, CHAR, Length', '2022-07-13 06:06:51.162563 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (21, 3, 'isNull', '����������� isNull', '2022-07-13 06:06:51.184259 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (22, 4, 'Description', '��������', '2022-07-13 06:06:51.218590 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (23, 2, 'DefaultValue', 'Default Value ��� ���� ����� ������', '2022-07-13 06:06:51.238818 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (24, 3, 'SQLText', '����������� SQL ��� �����������', '2022-07-13 06:06:51.260203 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (25, 4, 'Comment', '�����������', '2022-07-13 06:06:51.281645 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (26, 4, 'SequenceNumber', '���������������� �����', '2022-07-13 06:06:51.320251 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (27, 4, 'ForeignKeyOptions', '����� �������� FK', '2022-07-13 06:06:51.338797 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (28, 4, 'Definition', '�����������', '2022-07-13 06:06:51.361092 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (32, 1, 'JSONB', '��� ������ JSONB', '2022-07-13 06:45:23.207447 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (33, 1, 'BIGINT', '��� ������ BIGINT', '2022-07-13 06:45:27.253052 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (34, 1, 'BOOLEAN', '��� ������ BOOLEAN', '2022-07-13 06:45:27.355116 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (35, 1, 'TEXT', '��� ������ TEXT', '2022-07-13 06:45:28.238713 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (36, 1, 'NUMERIC', '��� ������ NUMERIC', '2022-07-13 06:45:29.935574 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (37, 1, 'BYTEA', '��� ������ BYTEA', '2022-07-13 06:45:37.174994 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (38, 1, 'ENUM', '��� ������ ENUM', '2022-07-13 06:45:37.418091 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (39, 4, 'Alias', '����� ����� ������� � ETL', '2022-07-16 10:48:36.421076 +00:00', true);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (40, 4, 'Condition', 'ETL-������� �� �������� ', '2022-07-16 11:58:37.455494 +00:00', true);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (41, 4, 'SelectList', 'ETL - Select List of tables', '2022-07-17 08:49:12.130178 +00:00', true);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (42, 4, 'OutputName', 'ETL ��� ��������� �������', '2022-07-17 12:39:36.356501 +00:00', true);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (43, 4, 'Decription', '��������� ��������� �������� ������� ����������', '2022-07-27 05:57:02.862628 +00:00', true);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (44, 4, 'DestID', '��� ��������� ETL ������ (id �� md_src)', '2022-07-27 06:15:42.493032 +00:00', true);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (45, 0, 'RU', '������������� ��������', '2022-07-29 09:12:51.549748 +00:00', true);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (46, 0, 'EN', 'English Attributes', '2022-07-29 09:12:51.608808 +00:00', true);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (49, 4, 'ETLVarType', 'Type of ETL package variable(String,Integer,Float,DateTime)', '2022-08-06 07:08:26.758861 +00:00', true);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (50, 4, 'ETLVarDefaultValue', 'ETL variable default value( (for test progon only)', '2022-08-06 07:08:26.758861 +00:00', true);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (51, 4, 'SensData', '������� ������������� ������', '2022-10-05 04:16:17.247435 +00:00', true);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (52, 4, 'TextIndexed', '������� �������������� ���� ��� ��������������� ������', '2022-10-09 05:08:31.459184 +00:00', true);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (53, 4, 'SelectViewed', '���� ������������ � ��������� ����������', '2022-10-09 05:08:33.203414 +00:00', true);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (54, 4, 'Description_DB', '�������� ������� � ���� Description_DB', '2022-10-11 10:03:20.316479 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (55, 1, 'DOUBLE_PRECISION', '��� ������ DOUBLE_PRECISION', '2022-10-12 07:20:28.534344 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (56, 1, '�LOB', '��� ������ �LOB', '2022-10-14 08:15:03.930008 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (57, 4, 'AddParamETL', '�������������� �������� ETL', '2022-10-23 09:22:53.490633 +00:00', true);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (66, 3, 'SequenceNumber', '�����������. ���������������� �����', '2022-10-23 11:14:28.411340 +00:00', false);
INSERT INTO md.md_attr (attrid, attrpid, key, name, tscreated, iscustom) VALUES (68, 3, 'ForeignKeyOptions', '�����������. ����� FK', '2022-10-23 11:17:42.340886 +00:00', false);
