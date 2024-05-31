/******************************************************************
 * File: IReceiver.cs
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

namespace PluginBase;

/// <summary>
/// A receiver is a component that receives messages from an external source and sends them
/// to the owning step through the receiver host.
/// </summary>
public interface IReceiver
{
    Task start();
    IReceiverHost host { get; set; }
    Task sendResponse(string response, object context);
    bool cantTryParse { get; }
    bool debugMode { get; set; }
}

/// <summary>
/// A receiver host is part of a step and is responsible for handling the input messages from the receiver.
/// </summary>
public interface IReceiverHost
{
    /// <summary>
    /// Handle an input message from receiver.
    /// </summary>
    /// <param name="input">Input message</param>
    /// <param name="context">Arbitrary context object</param>
    /// <returns></returns>
    Task signal(string input, object context);
    string IDStep { get; }
    int MaxConcurrentConnections { get; }
    int ConnectionTimeoutInMilliseconds { get; }


}
