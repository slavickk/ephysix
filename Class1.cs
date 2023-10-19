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

