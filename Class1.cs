/******************************************************************
 * File: Class1.cs
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

using System;
using System.Threading.Tasks;
using System.Linq;
async Task ConvObject()
{

    var client = new HttpClient();
    var itog = await url_crowler.execGet(client
                         , "User ID=dm;Password=rav1234;Host=master.pgsqlanomaly01.service.consul;Port=5432;Database=fpdb;", "User ID=fp;Password=rav1234;Host=master.pgsqlanomaly01.service.consul;Port=5432;Database=fpdb;SearchPath=md;"
                         , "dictcurrency", "https://pkgstore.datahub.io/core/currency-codes/codes-all_json/data/029be9faf6547aba93d64384f7444774/codes-all_json.json"
                         , @"select DISTINCT *
from json_to_recordset(@body::json)
as x(""AlphabeticCode"" text,""Currency"" text, ""NumericCode"" float,""MinorUnit"" text)
WHERE x."AlphabeticCode" is not null and x."NumericCode" is not null"
                         , 200);


}

