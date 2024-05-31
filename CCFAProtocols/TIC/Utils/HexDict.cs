/******************************************************************
 * File: HexDict.cs
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace CCFAProtocols.TIC.Instruments
{
    public static class HexUtils
    {
        //TODO: перевести на побитовые операции
        public static Dictionary<char, bool[]> HexToBoolMap = new()
        {
            {'0', new[] {false, false, false, false}},
            {'1', new[] {false, false, false, true}},
            {'2', new[] {false, false, true, false}},
            {'3', new[] {false, false, true, true}},
            {'4', new[] {false, true, false, false}},
            {'5', new[] {false, true, false, true}},
            {'6', new[] {false, true, true, false}},
            {'7', new[] {false, true, true, true}},
            {'8', new[] {true, false, false, false}},
            {'9', new[] {true, false, false, true}},
            {'A', new[] {true, false, true, false}},
            {'B', new[] {true, false, true, true}},
            {'C', new[] {true, true, false, false}},
            {'D', new[] {true, true, false, true}},
            {'E', new[] {true, true, true, false}},
            {'F', new[] {true, true, true, true}}
        };

        public static Dictionary<bool[], char> BoolsToHexMap =
            new(HexToBoolMap.Select(pair =>
                new KeyValuePair<bool[], char>(pair.Value, pair.Key)), new BoolArrayEquilityComparer());
    }

    public class BoolArrayEquilityComparer : IEqualityComparer<bool[]>
    {
        public bool Equals(bool[]? x, bool[]? y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;
            return x.SequenceEqual(y);
        }

        public int GetHashCode(bool[] obj)
        {
            return obj.Aggregate(0, (total, next) => HashCode.Combine(total, next));
        }
    }
}