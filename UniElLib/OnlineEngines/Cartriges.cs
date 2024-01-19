using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniElLib.OnlineEngines
{
    public abstract class Cartrige
    {
        public bool isReady = false;
        public abstract class Link
        {
            public Cartrige sourceCartridge;
            public Cartrige destCartridge;
            public abstract bool check();
        }

        public abstract IEnumerable<(Link link, bool isLastLink)> getLinks();
/*        public IEnumerable<Cartrige> getAllDestinationLinks()
        {
            return null;
        }
*/

        public abstract Task<bool> Exec();

        public async Task startExec()
        {
            foreach(var item in getLinks())
            {
                if(item.link.check())
                    item.link.destCartridge.isReady = true;
                if (item.link.destCartridge.isReady && item.isLastLink)
                {
                    await item.link.destCartridge.Exec();
                    await item.link.destCartridge.startExec();
                }


            }
        }
    }
}
