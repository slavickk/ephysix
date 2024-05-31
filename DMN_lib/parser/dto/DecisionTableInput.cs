/******************************************************************
 * File: DecisionTableInput.cs
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
using System.Xml.Serialization;

namespace net.adamec.lib.common.dmn.engine.parser.dto
{
    /// <inheritdoc />
    /// <summary>
    /// Decision table input definition
    /// </summary>
    public class DecisionTableInput : IdedElement
    {
        /// <summary>
        /// Label used to map to variable when the expression is not used
        /// </summary>
        [XmlAttribute("label")]
        public string Label { get; set; }

        /// <summary>
        /// Input expression without the <see cref="Expression.Text"/> subelement just defines the data type of the input.
        /// When  the <see cref="Expression.Text"/> subelement is provided the expression is used as the input
        /// </summary>
        [XmlElement("inputExpression")]
        public Expression InputExpression { get; set; }

        /// <summary>
        /// Optional list of allowed input values
        /// If the expression input is used, the allowed input values are checked after the expression evaluation
        /// </summary>
        [XmlElement("inputValues")]
        public AllowedValues AllowedInputValues { get; set; }

        /// <inheritdoc />
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return $"{Label}: {InputExpression}";
        }
    }
}
