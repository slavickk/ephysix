/******************************************************************
 * File: Decision.cs
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

using System.Collections.Generic;
using System.Xml.Serialization;

namespace net.adamec.lib.common.dmn.engine.parser.dto
{
    /// <inheritdoc />
    /// <summary>
    /// Single decision within the DMN file
    /// It can be expression decision of decision table.
    /// Decision table is recognized by having the <see cref="DecisionTable"/> element, 
    /// otherwise, the expression decision is expected with elements <see cref="Expression"/> and <see cref="OutputVariable"/>
    /// </summary>
    public class Decision : NamedElement
    {
        /// <summary>
        /// List of inputs of decisions the decision depends on
        /// </summary>
        [XmlElement("informationRequirement")]
        public List<InformationRequirement> InformationRequirements { get; set; }

        //------------------------------------
        //Expression
        //------------------------------------

        /// <summary>
        /// Output variable for the expression decision
        /// </summary>
        [XmlElement("variable")]
        public Variable OutputVariable { get; set; }
        /// <summary>
        /// Expression of the expression decision
        /// </summary>
        [XmlElement("literalExpression")]
        public Expression Expression { get; set; }

        //------------------------------------
        //Decision table
        //------------------------------------

        /// <summary>
        /// Decision table encapsulating element
        /// </summary>
        [XmlElement("decisionTable")]
        public DecisionTable DecisionTable { get; set; }
        
    }
}
