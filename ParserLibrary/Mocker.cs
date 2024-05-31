/******************************************************************
 * File: Mocker.cs
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary
{
    public  class Mocker
    {
        
        public string MocBody="";
        public string MocFile;
        string MocContent;
        object syncro = new object();
        public int MocTimeoutInMilliseconds=0;
        public async Task<string> getMock ()
        {
            if(MocTimeoutInMilliseconds >0)
                await Task.Delay(MocTimeoutInMilliseconds);
            string input;
            if ((MocBody ?? "") != "")
            {
                return MocBody;
            }
            else
            if ((MocFile ?? "") != "")
            {
                if (string.IsNullOrEmpty(MocContent))
                {
                    lock (syncro)
                    {
                        if (string.IsNullOrEmpty(MocContent))
                        {

                            // string ans;
                            using (StreamReader sr = new StreamReader(MocFile))
                            {
                                MocContent = sr.ReadToEnd();
                                return MocContent;
                            }
                        }
                        else
                            return MocContent;

                    }
                    
                } else
                    return MocContent;
/*                using (StreamReader sr = new StreamReader(MocFile))
                {
                    return sr.ReadToEnd();
                }*/
            }
            else
                return "";
        }

    }
}
