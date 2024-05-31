/******************************************************************
 * File: Resolver.cs
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

using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CamundaInterface
{
    public class ConsulKV
    {
//        http://10.74.30.21/
        public static string CONSUL_ADDR = "http://10.74.30.21:8500";
//        public static  string CONSUL_ADDR= "http://192.168.75.204:8500";
    }
    public class Resolver
    {
        public static IPAddress consulIP = null;

        public static List<string> ResolveConsulAddrs(string nameService, string tag = "")
        {
            if (consulIP == null)
            {
                var uri = new Uri(ConsulKV.CONSUL_ADDR);
                consulIP = IPAddress.Parse(uri.Host);
            }
            var client = new LookupClient(consulIP, 8600);
            if (nameService != "" && tag != "")
            {
                var entry = client.ResolveService($"{nameService}.service{DataCenterResolver.DataCenter}consul", tag);

                return entry.Select(ii => ii.AddressList[0].ToString() + ":" + ii.Port).ToList();
            }
            if (tag == "")
            {
                var entry = client.ResolveService($"service{DataCenterResolver.DataCenter}consul", nameService);
                return entry.Select(ii => ii.AddressList[0].ToString() + ":" + ii.Port).ToList();

            }
            return new List<string>();
        }

        async public static Task<List<string>> ResolveConsulAddrsAsync(string nameService, string tag = "")
        {
            if (consulIP == null)
            {
                var uri = new Uri(ConsulKV.CONSUL_ADDR);
                consulIP = IPAddress.Parse(uri.Host);
            }
            var client = new LookupClient(consulIP, 8600);
            if (nameService != "" && tag != "")
            {
                var entry = await client.ResolveServiceAsync($"{nameService}.service{DataCenterResolver.DataCenter}consul", tag);

                return entry.Select(ii => ii.AddressList[0].ToString() + ":" + ii.Port).ToList();
            }
            if (tag == "")
            {
                var entry = await client.ResolveServiceAsync($"service{DataCenterResolver.DataCenter}consul", nameService);
                return entry.Select(ii => ii.AddressList[0].ToString() + ":" + ii.Port).ToList();

            }
            return new List<string>();
        }


        public static string ResolveConsulAddr(string nameService, string tag = "")
        {
            if (nameService == "chronicler0--")
                return "192.168.75.160:48080";
            if (consulIP == null)
            {
                var uri = new Uri(ConsulKV.CONSUL_ADDR);
                consulIP = IPAddress.Parse(uri.Host);
            }
            var client = new LookupClient(consulIP, 8600);
            if (nameService != "" && tag != "")
            {
                var entry = client.ResolveService($"{nameService}.service{DataCenterResolver.DataCenter}consul", tag);
                if (entry.Length == 0)
                    return "";
                return entry[0].AddressList[0].ToString() + ":" + entry[0].Port;
            }
            if (tag == "")
            {
                var entry = client.ResolveService($"service{DataCenterResolver.DataCenter}consul", nameService);
                if (entry.Length == 0)
                    return "";
                return entry[0].AddressList[0].ToString() + ":" + entry[0].Port;

            }
            return "";
        }

        async public static Task<string> ResolveConsulAddrAsync(string nameService, string tag = "")
        {
            if (nameService == "chronicler0--")
                return "192.168.75.160:48080";
            if (consulIP == null)
            {
                var uri = new Uri(ConsulKV.CONSUL_ADDR);
                consulIP = IPAddress.Parse(uri.Host);
            }
            var client = new LookupClient(consulIP, 8600);
            if (nameService != "" && tag != "")
            {
                var entry = await client.ResolveServiceAsync($"{nameService}.service{DataCenterResolver.DataCenter}consul", tag);
                return entry[0].AddressList[0].ToString() + ":" + entry[0].Port;
            }
            if (tag == "")
            {
                var entry = await client.ResolveServiceAsync($"service{DataCenterResolver.DataCenter}consul", nameService);
                return entry[0].AddressList[0].ToString() + ":" + entry[0].Port;

            }
            return "";
        }


    }
}
