/******************************************************************
 * File: IDmnVariable.cs
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
using System.Collections.Generic;

namespace net.adamec.lib.common.dmn.engine.engine.definition
{
    /// <summary>
    /// Read only definition DMN model variable
    /// </summary>
    public interface IDmnVariable
    {
        /// <summary>
        /// Name of the variable
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Type of the variable when recognized from the decisions
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Flag whether the variable represents the input parameter of the decision model.
        /// Such variable will be read-only
        /// </summary>
        bool IsInputParameter { get; }

        /// <summary>
        /// Flag whether the variable has any "setter" - is there any output to Variable or is Input parameter
        /// </summary>
        bool HasValueSetter { get; }

        /// <summary>
        /// Information about value "setters" for the variable.
        /// </summary>
        /// <remarks>
        /// Can be "Input: {inputName}", "Table Decision {decisionName}" or "Expression Decision {decisionName}".
        /// </remarks>
        IEnumerable<string> ValueSetters { get; }
    }
}
