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