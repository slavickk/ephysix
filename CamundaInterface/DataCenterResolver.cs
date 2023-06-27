using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamundaInterface
{
    public static class DataCenterResolver
    {
        static string dataCenter = ".";

        public static string DataCenter
        {
            set
            {
                if (string.IsNullOrEmpty(dataCenter))
                    dataCenter = ".";
                else
                    dataCenter = $".{value}.";

            }
            get
            {
                return dataCenter;
            }
        }
    }

}
