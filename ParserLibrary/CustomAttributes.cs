/******************************************************************
 * File: CustomAttributes.cs
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

using System;

namespace ParserLibrary;

public class GUIAttribute:Attribute
{
    Type settingsType;
    public GUIAttribute(Type setType)
    {
        settingsType = setType;
    }
}

public class SensitiveAttribute : Attribute
{
    public string NameSensitive;

    public SensitiveAttribute(string name)
    {
        NameSensitive = name;
    }
}

public class AnnotationAttribute : Attribute
{
    public string Description
    {
        get { return description; }
    }

    string description;

    public AnnotationAttribute(string description)
    {
        this.description = description;
    }
}