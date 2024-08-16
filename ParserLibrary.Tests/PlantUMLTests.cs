using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Npgsql;
using NUnit.Framework;

namespace ParserLibrary.Tests
{
    public class PlantUMLTests
    {
        [Test]
        public void testUml()
        {
            NpgsqlConnectionStringBuilder conn = new NpgsqlConnectionStringBuilder();
//            conn.ConnectionString = "User ID=fp;Password=rav12\"34;Host=192.168.75.220;Port=5432;Database=fpdb;SearchPath=md;";
            conn.ConnectionString = "Host=192.168.75.220;Port=5432;Database=fpdb;SearchPath=md;";
            conn.Username = "User ID";
            conn.Password = "1234;1234";
            
            var dict = new List<PlantUMLItem>() {
                new PlantUMLItem()
                {
                    Name="Step1",
                     links=new List<PlantUMLItem.Link>()
                     {
                         new PlantUMLItem.Link()
                         {
                             NameRq="Link1", children=new PlantUMLItem()
                             {
                                 Name="Step2", color="#00FF00",
                                 links= new List<PlantUMLItem.Link>()
                                 {
                                     new PlantUMLItem.Link()
                                     {
                                         isError=true,NameRp="Return error",
                                          NameRq="Link155"
                                          , children=new PlantUMLItem() {
                                              Name="Item999"
                                          }
                                     }
                                 }
                             }
                         }
                         ,
                                                 new PlantUMLItem.Link()
                         {
                              NameRq="Link2", children=new PlantUMLItem()
                             {
                                 Name="Step3"
                             }
                         }

                     }
                }
            };
            var jsonBody=JsonSerializer.Serialize(dict);
            var st=PlantUMLItem.getUML("Tect" ,dict);

        }

    }
}
