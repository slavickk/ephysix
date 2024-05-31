﻿/******************************************************************
 * File: DmnDecisionTableInput.cs
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
    /// Definition of decision table input - contains index (order), mapping to the source variable or source expression, and optional list of allowed values
    /// </summary>
    /// <remarks>Source variable and source expression are mutually exclusive and one of them has to be provided.
    /// When the definition contains both variable and expression, the expression is used.
    /// See<see cref="DmnDefinitionFactory.CreateDecisionTable"/> for details.</remarks>
    public class DmnDecisionTableInput
    {
        /// <summary>
        /// Index of the input (order)
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// Input source variable (the variable value is compared to the rules)
        /// </summary>
        /// <remarks>Source variable and source expression are mutually exclusive.</remarks>
        public IDmnVariable Variable { get; }
        /// <summary>
        /// Input source expression (the evaluated expression is compared to the rules)
        /// </summary>
        /// <remarks>Source variable and source expression are mutually exclusive.</remarks>
        public string Expression { get; }
        /// <summary>
        /// Optional array of allowed values
        /// </summary>
        public string[] AllowedValues { get; }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="index">Index of the input (order)</param>
        /// <param name="variable">Input source variable (the variable value is compared to the rules)</param>
        /// <param name="expression">Input source expression (the evaluated expression is compared to the rules)</param>
        /// <param name="allowedValues">Optional array of allowed values</param>
        public DmnDecisionTableInput(
            int index, 
            IDmnVariable variable,
            string expression = null,
            string[] allowedValues = null)
        {
            Index = index;
            Variable = variable;
            Expression = expression;
            AllowedValues = allowedValues;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return $"#{Index}:{(!string.IsNullOrWhiteSpace(Expression)?Expression:Variable.ToString())}";
        }
    }
}
