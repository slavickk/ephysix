/******************************************************************
 * File: TICSender.cs
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

using System.Net.Sockets;
using System.Threading.Tasks;
using ParserLibrary.TIC.TICFrames;
using PluginBase;
using UniElLib;


namespace Plugins
{
    public class TICSender : ISender
    {
        private TICFrame Frame;
        private TcpClient? twfaclient;
        public string twfaHost= "192.168.75.148";
        public int twfaPort=5553;
        public void setTemplate(string key, string body)
        {
            throw new NotImplementedException();
        }

        public TypeContent typeContent => TypeContent.json;
        public void Init()
        {
            throw new NotImplementedException();
        }


        public int ticFrame = 6;
/*        {
            get => Frame.FrameNum;
            set => Frame = TICFrame.GetFrame(value);
        }*/

        public ISenderHost host { get; set; }

        public Task<string> send(UniElLib.AbstrParser.UniEl root, ContextItem context)
        {
            return Task.FromResult(string.Empty);
        }

        public async Task<string> send(string JsonBody, ContextItem context)
        {
            if (twfaclient is null || !twfaclient.Connected)
            {
                Frame = TICFrame.GetFrame(ticFrame);
                twfaclient = new TcpClient();
                await twfaclient.ConnectAsync(twfaHost, twfaPort);
            }

            NetworkStream stream = twfaclient.GetStream();
            await Frame.SerializeFromJson(stream, JsonBody);
            return await Frame.DeserializeToJson(stream);
        }

        public string getTemplate(string key)
        {
            return string.Empty;
        }
    }
}