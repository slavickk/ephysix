/******************************************************************
 * File: Cartriges.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

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
