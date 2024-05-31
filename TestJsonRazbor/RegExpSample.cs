/******************************************************************
 * File: RegExpSample.cs
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
using System.Text;
using System.Text.RegularExpressions;

namespace TestJsonRazbor
{

    public class TestT
    {
        static void Main1()
        {
            string input = "Hello, World!";
            int n = 5; // Number of characters to extract

            string pattern = $"^.{n}";
            Regex regex = new Regex(pattern);

            Match match = regex.Match(input);

            if (match.Success)
            {
                string result = match.Value;
                Console.WriteLine($"First {n} characters: {result}");
            }
            else
            {
                Console.WriteLine("No match found.");
            }
        }



        static void Main2()
        {
            string input1 = "Hello"; // String with length < 8
            string input2 = "Howdydoo"; // String with length >= 8

            string pattern = @"^(.{3}|.{7})"; // Regular expression pattern
            Regex regex = new Regex(pattern);

            ExtractAndDisplayFirstSymbols(input1, regex);
            ExtractAndDisplayFirstSymbols(input2, regex);
        }

        static void ExtractAndDisplayFirstSymbols(string input, Regex regex)
        {
            Match match = regex.Match(input);

            if (match.Success)
            {
                string result = match.Groups[1].Value;
                Console.WriteLine($"Input: \"{input}\" - Extracted symbols: \"{result}\"");
            }
            else
            {
                Console.WriteLine($"Input: \"{input}\" - No match found.");
            }
        }
    }
}
