using ParserLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
