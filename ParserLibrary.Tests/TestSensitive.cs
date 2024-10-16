﻿/******************************************************************
 * File: TestSensitive.cs
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

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniElLib;

namespace ParserLibrary.Tests
{
    public class TestSensitive
    {

        [Test]
        public void Test()
        {
            int n = 4; // Number of symbols from the beginning
            int k = 5; // Number of wildcard characters
            int m = 3; // Number of symbols from the end
            string input = "This is a sample string containing sensitive information like 1234567890123456 and 12345678901234567890.";
            string maskedInput = DetectSensitive.MaskSensitiveInfo(input, n, k, m);

            Console.WriteLine("Original input: " + input);
            Console.WriteLine("Masked input: " + maskedInput);

            string input1 = "John Doe";
            bool containsNameParts1 = DetectSensitive.ContainsNameParts(input1);
            Console.WriteLine($"Input: {input1}");
            Console.WriteLine($"Contains Name Parts: {containsNameParts1}");

            string input2 = "Mary A. Smith";
            bool containsNameParts2 = DetectSensitive.ContainsNameParts(input2);
            Console.WriteLine($"Input: {input2}");
            Console.WriteLine($"Contains Name Parts: {containsNameParts2}");

            string input3 = "Mr. James";
            bool containsNameParts3 = DetectSensitive.ContainsNameParts(input3);
            Console.WriteLine($"Input: {input3}");
            Console.WriteLine($"Contains Name Parts: {containsNameParts3}");

            string input4 = "Mr. E678a";
            bool containsNameParts4 = DetectSensitive.ContainsNameParts(input4);
            Console.WriteLine($"Input: {input4}");
            Console.WriteLine($"Contains Name Parts: {containsNameParts4}");

        }

    }
}
