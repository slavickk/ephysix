using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace UniElLib
{
    public static class IConf
    {
        public static string ConfigValue(this string key)
        {
            return Configuration[key];
        }

        public static IConfiguration Configuration;

    }
}
