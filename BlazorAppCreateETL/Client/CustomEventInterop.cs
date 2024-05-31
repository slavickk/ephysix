/******************************************************************
 * File: CustomEventInterop.cs
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

using Microsoft.JSInterop;

namespace BlazorAppCreateETL.Client
{
    public class CustomEventHelper
    {
        private readonly Func<EventArgs, Task> _callback;
        private readonly Func<string,double,double, Task> _callback1;

        public CustomEventHelper(Func<EventArgs, Task> callback)
        {
            _callback = callback;
        }
        public CustomEventHelper(Func<string,double,double, Task> callback)
        {
            _callback1 = callback;
        }
        [JSInvokable]
        public Task GetD3Enter(string passedName,double x,double y) => _callback1(passedName,x,y);
       


        [JSInvokable]
        public Task OnCustomEvent(EventArgs args) => _callback(args);
    }

    public class CustomEventInterop : IDisposable
    {
        private readonly IJSObjectReference _jsRuntime;
        private DotNetObjectReference<CustomEventHelper> Reference;

        public CustomEventInterop(IJSObjectReference jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public ValueTask<string> SetupCustomEventCallback(Func<EventArgs, Task> callback)
        {
            Reference = DotNetObjectReference.Create(new CustomEventHelper(callback));
            // addCustomEventListener will be a js function we create later
            return _jsRuntime.InvokeAsync<string>("addCustomEventListener", Reference);
        }
        public ValueTask<string> SetupCustomEventCallback(Func<string,double,double, Task> callback)
        {
            Reference = DotNetObjectReference.Create(new CustomEventHelper(callback));
            // addCustomEventListener will be a js function we create later
            return _jsRuntime.InvokeAsync<string>("addCustomEventListener", Reference);
        }


        public void Dispose()
        {
            Reference?.Dispose();
        }
    }
}
