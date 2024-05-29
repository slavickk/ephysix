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
