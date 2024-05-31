/******************************************************************
 * File: AliasProducers.cs
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

namespace UniElLib;

public abstract class AliasProducer
{
    public abstract string getAlias(string originalValue);
}

[Sensitive("FIO")]
public class AliasFIO : AliasProducer
{

    public override string getAlias(string originalValue)
    {
        var tt =originalValue.Split(' ');
        return tt.Select(query => query.Substring(0,1)).Aggregate((a, b) => a + " " + b+"."); ;
    }
}

[Sensitive("PAN")]
public class AliasPan : AliasProducer
{
    public override string getAlias(string originalValue)
    {
        if (originalValue.Length >= 16)
            return $"{originalValue.Substring(0, 4)}***{originalValue.Substring(originalValue.Length - 3)}";
        return "*";
    }
}

[Sensitive("PHONE")]
public class AliasPhone : AliasProducer
{
    public override string getAlias(string originalValue)
    {
        if (originalValue.Length > 3)
            return $"{originalValue.Substring(0, 2)}*{originalValue.Substring(originalValue.Length - 3)}";
        return "*";
    }
}