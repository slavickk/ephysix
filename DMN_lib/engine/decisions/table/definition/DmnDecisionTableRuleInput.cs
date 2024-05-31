﻿/******************************************************************
 * File: DmnDecisionTableRuleInput.cs
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

using System.Diagnostics.CodeAnalysis;
using net.adamec.lib.common.dmn.engine.parser;

namespace net.adamec.lib.common.dmn.engine.engine.decisions.table.definition
{
    /// <summary>
    /// Definition of decision table rule input - contains the input match evaluation expression and mapping to table input
    /// </summary>
    public class DmnDecisionTableRuleInput
    {
        /// <summary>
        /// Corresponding table input
        /// </summary>
        public DmnDecisionTableInput Input { get; }
        /// <summary>
        /// Expression used to evaluate the rule input match ("both sides")
        /// </summary>
        public string Expression { get; }

        /// <summary>
        /// Unparsed expression used to evaluate the rule input match ("right side")
        /// </summary>
        public string UnparsedExpression { get; }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="input">Corresponding table input</param>
        /// <param name="unparsedExpression">Expression used to evaluate the rule input match ("right side")</param>
        public DmnDecisionTableRuleInput(DmnDecisionTableInput input, string unparsedExpression)
        {
            Input = input;
            UnparsedExpression = unparsedExpression;
            Expression = SfeelParser.ParseInput(unparsedExpression, string.IsNullOrWhiteSpace(input.Expression) ? input.Variable.Name : input.Expression);
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return $"#{Input.Index} {Input} <-- {Expression}";
        }
    }
}
