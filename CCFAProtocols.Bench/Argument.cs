/******************************************************************
 * File: Argument.cs
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

namespace CCFAProtocols.Bench;

public class Argument<T>
{
    private readonly string _name;
    private T obj;

    public Argument(T val, string? name = null)
    {
        _name = name ?? val.ToString();
        obj = val;
    }

    public T Get() => obj;

    public static implicit operator T(Argument<T> arg) => arg.obj;

    public override string ToString()
    {
        return _name;
    }
}