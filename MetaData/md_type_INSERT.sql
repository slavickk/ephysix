/******************************************************************
 * File: md_type_INSERT.sql
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

INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (-6, 'VForeignKey', '����������� ������� ����������', '2022-07-16 04:53:17.090908 +00:00', true);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (1, 'Table', '�������', '2022-07-13 06:06:44.915538 +00:00', false);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (2, 'Column', '������� �������', '2022-07-13 06:06:44.937522 +00:00', false);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (3, 'Index', '������', '2022-07-13 06:06:44.957412 +00:00', false);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (4, 'PrimaryKey', '��������� ����', '2022-07-13 06:06:44.978165 +00:00', false);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (5, 'Unique', '����������� ������������', '2022-07-13 06:06:44.999379 +00:00', false);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (6, 'ForeignKey', '������� ����������', '2022-07-13 06:06:45.020433 +00:00', false);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (7, 'Check', '����������� Check', '2022-07-13 06:06:45.122918 +00:00', false);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (8, 'Table2Scheme', '����� �������-�����', '2022-07-13 06:06:45.147094 +00:00', false);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (9, 'Column2Table', '����� �������-�������', '2022-07-13 06:06:45.198718 +00:00', false);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (10, 'View', '�����', '2022-07-13 06:06:45.223426 +00:00', false);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (11, 'View2SRC', '����� �����-��������', '2022-07-13 06:06:45.264398 +00:00', false);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (12, 'Memo', '����� �����������', '2022-07-13 06:06:45.290351 +00:00', true);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (13, 'Stream', '����� DummySystem1', '2022-08-23 05:18:05.427064 +00:00', true);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (14, 'StreamField', '���� ������', '2022-08-23 05:18:05.427064 +00:00', true);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (15, 'Field2Stream', '����� ����� - ���� ������', '2022-08-23 13:52:42.498779 +00:00', true);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (16, 'selector', '��������', '2022-10-20 09:24:26.978876 +00:00', true);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (18, 'Selector2Table', '����� �������� - �������', '2022-10-22 05:31:42.703299 +00:00', true);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (101, 'ETLPackage', '����� ETL', '2022-07-16 09:23:26.435728 +00:00', true);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (102, 'ETLLink', 'ETL: ����� ������ � ForeignKey', '2022-07-16 09:58:39.927821 +00:00', true);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (103, 'ETLTable', 'ETL-�������-��������', '2022-07-16 11:55:51.159726 +00:00', true);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (104, 'ETLRelation', 'ETL-����� ����� �����������', '2022-07-16 11:55:51.159726 +00:00', true);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (105, 'ETLCondition', 'ETL-�������������� �������', '2022-07-16 11:55:51.159726 +00:00', true);
INSERT INTO md.md_type (typeid, key, name, tscreated, iscustom) VALUES (106, 'ETLVariable', '���������� ETL ������', '2022-07-30 12:17:15.940524 +00:00', true);
