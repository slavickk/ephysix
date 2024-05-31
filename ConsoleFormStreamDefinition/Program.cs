/******************************************************************
 * File: Program.cs
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

using ClassApiExecutor;

namespace ConsoleFormStreamDefinition
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] fields =new string[] { "Kind", "LifePhase", "IsReversal", "UndoState", "CustCard", "Result" };

            TranParserHelper.getDefine(fields);
            Console.WriteLine("Hello, World!");
        }
    }
}