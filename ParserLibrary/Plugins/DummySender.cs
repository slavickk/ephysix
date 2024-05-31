/******************************************************************
 * File: DummySender.cs
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

using System.Collections.Generic;
using System.Threading.Tasks;
using PluginBase;
using UniElLib;


namespace Plugins;

/// <summary>
/// Dummy sender that doesn't send anything anywhere, and returns a predefined response.
/// This is an ISender-based counterpart to the legacy DummyReceiver. 
/// </summary>
public class DummySender : ISender
{
    public TypeContent typeContent => TypeContent.json;
    public void Init()
    {
        ParserLibrary.Logger.log("Plugins.DummySender.Init() called.");
    }

    /// <summary>
    /// Dictionary of dummy responses to return, indexed by string keys.
    /// </summary>
    public Dictionary<string, string> DummyResponses;

    /// <summary>
    /// The key of the dummy response to return.
    /// </summary>
    public string ResponseToReturn;
    
    public ISenderHost host { get; set; }

    public Task<string> send(UniElLib.AbstrParser.UniEl root, ContextItem context)
    {
        return Task.FromResult(this.DummyResponses[this.ResponseToReturn]);
    }

    public Task<string> send(string JsonBody, ContextItem context)
    {
        return Task.FromResult(this.DummyResponses[this.ResponseToReturn]);
    }

    public string getTemplate(string key)
    {
        return string.Empty;
    }

    public void setTemplate(string key, string body)
    {
    }
}