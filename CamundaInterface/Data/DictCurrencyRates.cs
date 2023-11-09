using System;
using System.Threading.Tasks;
using System.Net.Http;
using CamundaInterface;
using ParserLibrary;
using System.Linq;
using static CamundaInterface.SendToRefDataLoader;
async Task<int> ConvObject(string ConnSelect1, string ConnAdm1)
{
    var client = new HttpClient();
    int all = 0, errors = 0;
    ExportItem itog1=null, itog2=null;
    if(Environment.GetEnvironmentVariable("RDL_URL")==null)
    {
        Logger.log($"RDL_URL not define ,exec load rates impossible!!!!!!!!!!!!!");
        return 1;
    }
    //http://CSExternalTask.service.dc1.consul:24169/api/Api/url-crowler 
    {

        var ConnSelect = @"User ID=dm;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;";
        var ConnAdm = @"User ID=fp;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;SearchPath=md;";
        var URL = @"https://pkgstore.datahub.io/core/currency-codes/codes-all_json/data/029be9faf6547aba93d64384f7444774/codes-all_json.json";
        var SQL = @"select DISTINCT *
from json_to_recordset(@body::json)
as x(""AlphabeticCode"" text,""Currency"" text, ""NumericCode"" float,""MinorUnit"" text)
union
select 'RUB','Russian rouble',810,'2'
;
";
        var Table = @"dm.dictcurrency";
        var UpdateTimeout = @"86400";


        itog1 = await url_crowler.execGet(client
                                     , ConnSelect1, ConnAdm1
                                     , Table, URL
                                     , SQL
                                     , Convert.ToInt32(UpdateTimeout));

    }

    //http://CSExternalTask.service.dc1.consul:24169/api/Api/url-crowler 
    {

        var ConnSelect = @"User ID=dm;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;";
        var ConnAdm = @"User ID=fp;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;SearchPath=md;";
        var URL = @"https://openexchangerates.org/api/latest.json?app_id=bc0094ab2e33483fb7a58c1d317c8c70";
        var SQL = @"SELECT key, value::float
  FROM jsonb_each(@body::jsonb  -> 'rates')  as users;";
        var Table = @"dm.dictcurrencyrate";
        var UpdateTimeout = @"86400";


        itog2 = await url_crowler.execGet(client
                                     , ConnSelect1, ConnAdm1
                                     , Table, URL
                                     , SQL
                                     , Convert.ToInt32(UpdateTimeout));

    }

    //http://CSExternalTask.service.dc1.consul:24169/api/Api/to-dict-sender Transfer data from one sources(SQL) and  to NoSQL(key/value ) destination
    if((itog1?.all>0 || itog2?.all > 0) && ((itog1 != null) && itog2 != null && itog1.errors==0 && itog2.errors==0))
    {

        var DictName = @"currencyrates";
        var ConnSelect = @"User ID=dm;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;";

        var ConnAdm = @"User ID=fp;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;SearchPath=md;";
        var DictAddr = Environment.GetEnvironmentVariable("RDL_URL");
        var MaxRecords = @"1000";
        var SQLText = @"select dictcurrency.numericcode numericcode,dictcurrencyrate.rate rate from dm.dictcurrency   
 inner join   dm.dictcurrencyrate  on (dictcurrency.alphabeticcode=dictcurrencyrate.scode)  
";
        var SensitiveData = @", ";
        var CountInKey = @"1";
        var Fields = @"numericcode,rate";
        var Variables = @"";
        var Signature = @"569074234566666";


        var itog = await SendToRefDataLoader.putRequestToRefDataLoader(client, "XXXXXXX:to-dict-sender"
      , ConnSelect1, ConnAdm1, DictName, "TEST", SQLText
      , Convert.ToInt32(MaxRecords), DictAddr, SensitiveData, Convert.ToInt32(CountInKey), Fields);
        all = itog.all;
        errors = itog.errors;


    }
    Logger.log($"exec load rates all:{all} errors:{errors} all1:{itog1.all} all2:{itog2.all}");

    return errors;//, errors);
}