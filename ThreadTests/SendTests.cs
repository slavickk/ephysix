/******************************************************************
 * File: SendTests.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARDummyProtocol1ULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

// A prototype of an async Send() method that returns only when
// the response has been received on another thread.

TaskCompletionSource<string> _tcs;

async Task Main(string[] args)
{
    _tcs = new TaskCompletionSource<string>();
    var endResult = await Send("send payload");
    Console.WriteLine("Back from Send: " + endResult);
}

async Task<string> Send(string payload)
{
    Console.WriteLine("Sending: " + payload);
    // Send data to the network
    // ...
    
    // Run a background task that sets the result of _tcs after sleeping for 3 seconds.
    Task.Run(async () =>
    {
        Console.WriteLine("Waiting for incoming notification from the external service");
        await Task.Delay(3000);
        OnReceived("received payload");
    });

    // Wait for OnReceived to be set the result
    return await _tcs.Task;
}

void OnReceived(string payload)
{
    // Process received data
    // ...
    Console.WriteLine("Received: " + payload);

    // Signal that OnReceived has been called
    _tcs.SetResult(payload);
}

_tcs = new TaskCompletionSource<string>();
var endResult = await Send("send payload");
Console.WriteLine("Back from Send: " + endResult);
