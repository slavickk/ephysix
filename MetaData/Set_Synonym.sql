/******************************************************************
 * File: Set_Synonym.sql
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
Установка синонимов между одноименными таблицами/полями в двух источниках
*/

select s.srcid,s.nodeid as snodeid,s.name as sname, slf.arcid, f.nodeid as fnodeid, f.name as fname, f.synonym as fsynonym,
       d.srcid,d.nodeid as dnodeid,d.name as dname, dlf.arcid, df.nodeid as dfnodeid, df.name as dfname, df.synonym as dfsynonym,
       md_set_synonyms(f.nodeid, df.nodeid)
    from md.md_node s, md.md_type st, md.md_arc slf, md.md_type slft, md.md_node f, md.md_type ft,
         md.md_node d, md.md_type dt, md.md_arc dlf, md.md_type dlft, md.md_node df, md.md_type dft
    where s.typeid=st.typeid and st.key='Table' and not s.isdeleted
      and slf.typeid=slft.typeid and slft.key='Column2Table'  and not slf.isdeleted
      and f.typeid=ft.typeid and ft.key='Column' and not f.isdeleted
      and s.nodeid=slf.toid and f.nodeid=slf.fromid
      and s.srcid=1 and slf.srcid=1 and f.srcid=1
      and s.name='terminal' and d.name=s.name and f.name=df.name
      and d.typeid=dt.typeid and dt.key='Table' and not d.isdeleted
      and dlf.typeid=dlft.typeid and dlft.key='Column2Table'  and not dlf.isdeleted
      and df.typeid=dft.typeid and dft.key='Column' and not df.isdeleted
      and d.nodeid=dlf.toid and df.nodeid=dlf.fromid
      and d.srcid=5 and dlf.srcid=5 and df.srcid=5
    order by f.nodeid desc,sname, fname;
