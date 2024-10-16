﻿/******************************************************************
 * File: DetectSensitive.cs
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
using System.Linq;
using System.Text.RegularExpressions;

public static class DetectSensitive
{
    public static string MaskSensitive(this string input)
    {
        return SensitiveInfoScanner.MaskSensitiveInfo(input, 5, 5, 3);
    }
    public static string MaskSensitiveInfo(string input, int n, int k, int m)
    {
        // Create a regular expression pattern to match the sensitive information
        string pattern = @"\b\d{16,24}\b";
        Regex regex = new Regex(pattern);

        // Replace the sensitive information with the modified string
        string maskedInput = regex.Replace(input, match =>
        {
            string originalMatch = match.Value;
            int matchLength = originalMatch.Length;

            if (matchLength <= n + m)
            {
                return originalMatch;
            }
            else
            {
                string maskedValue = originalMatch.Substring(0, n) + new string('*', k) + originalMatch.Substring(matchLength - m);
                return maskedValue;
            }
        });

        return maskedInput;
    }

    public static bool ContainsNameParts(string input)
    {
        // Split the input into individual words
        string[] words = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        // Count the number of words in the input
        int wordCount = words.Length;

        // Check if the input contains at least two words
        if (wordCount >= 2)
        {
            // Check if the first and last words start with an uppercase letter
            bool startsWithUpperCase = char.IsUpper(words[0][0]) && char.IsUpper(words[wordCount - 1][0]);

            // Check if the first and last words have more than one character
            bool hasMultipleCharacters = words[0].Length > 1 && words[wordCount - 1].Length > 1;

            // Check if the words between the first and last words start with a lowercase letter
            bool inBetweenWordsStartWithLowerCase = true;
            for (int i = 1; i < wordCount - 1; i++)
            {
                if (char.IsUpper(words[i][0]))
                {
                    inBetweenWordsStartWithLowerCase = false;
                    break;
                }
            }

            // Return true if the conditions for name parts are satisfied
            if (startsWithUpperCase && hasMultipleCharacters && inBetweenWordsStartWithLowerCase)
            {
                return true;
            }
        }

        return false;
    }
    public class SensitiveInfoScanner
    {
        public static string MaskSensitiveInfo(string input, int n, int k, int m)
        {
            // Create a regular expression pattern to match the sensitive information
            string pattern = @"\b\d{16,24}\b";
            Regex regex = new Regex(pattern);

            // Replace the sensitive information with the modified string
            string maskedInput = regex.Replace(input, match =>
            {
                string originalMatch = match.Value;
                int matchLength = originalMatch.Length;

                if (matchLength <= n + m)
                {
                    return originalMatch;
                }
                else
                {
                    string maskedValue = originalMatch.Substring(0, n) + new string('*', k) + originalMatch.Substring(matchLength - m);
                    return maskedValue;
                }
            });

            return maskedInput;
        }
    }


}
