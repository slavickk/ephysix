/******************************************************************
 * File: NullSender.cs
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

using PluginBase;
using UniElLib;

namespace Plugins;

/// <summary>
/// A sender that does absolutely nothing, even less than DummySender.
/// This implementation is here for completeness only, as it was present in the original codebase.
/// </summary>
[Annotation("Пустой Sender")]
public class NullSender : ISender
{
    public ISenderHost host { get; set; }
    public async Task<string> send(AbstrParser.UniEl root, ContextItem context)
    {
        return string.Empty;
    }

    public async Task<string> send(string JsonBody, ContextItem context)
    {
        return string.Empty;
    }

    public string getTemplate(string key)
    {
        return string.Empty;
    }

    public void setTemplate(string key, string body)
    {
    }

    public TypeContent typeContent => TypeContent.internal_list;
    public void Init()
    {
        ParserLibrary.Logger.log("Plugins.NullSender.Init() called.");
    }
}