using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniElLib;

public interface ComparerV
{
    public object Body 
   {
        get
        {
            return this;
        }
    }
    bool Compare(AbstrParser.UniEl el);
}
