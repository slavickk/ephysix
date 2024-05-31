/******************************************************************
 * File: DmnDecisionTableOutput.cs
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

using System.Diagnostics.CodeAnalysis;
using net.adamec.lib.common.dmn.engine.engine.definition;

namespace net.adamec.lib.common.dmn.engine.engine.decisions.table.definition
{
    /// <summary>
    /// Definition of decision table output - contains index (order), mapping to the variable and optional list of allowed values
    /// </summary>
    public class DmnDecisionTableOutput
    {
        /// <summary>
        /// Index of the output (order)
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// Variable to store the output to
        /// </summary>
        public IDmnVariable Variable { get; }
        /// <summary>
        /// Optional array of allowed values
        /// </summary>
        public string[] AllowedValues { get; }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="index">Index of the output (order)</param>
        /// <param name="variable">Variable to store the output to</param>
        /// <param name="allowedValues">Optional array of allowed values</param>
        public DmnDecisionTableOutput(int index, IDmnVariable variable, string[] allowedValues = null)
        {
            Index = index;
            Variable = variable;
            AllowedValues = allowedValues;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return $"#{Index}:{Variable}";
        }
    }
}
