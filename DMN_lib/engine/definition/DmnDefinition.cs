/******************************************************************
 * File: DmnDefinition.cs
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

using System;
using System.Collections.Generic;
using System.Linq;
using net.adamec.lib.common.dmn.engine.engine.decisions;

namespace net.adamec.lib.common.dmn.engine.engine.definition
{
    /// <summary>
    /// DMN model definition for execution engine - encapsulates Decisions, Variables (incl. Input data)
    /// </summary>
    public class DmnDefinition
    {
        /// <summary>
        /// Unique identifier of the definition (set at CTOR)
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Variables used while executing the DMN model - can be used within the Decision Tables and/or Expressions
        /// In general, it holds the Input Data of DMN model and outputs from Decision Tables and/or Expressions
        /// </summary>
        public IReadOnlyDictionary<string, IDmnVariable> Variables { get; }

        /// <summary>
        /// Input data interface. Input data are stored as Variables with <see cref="IDmnVariable.IsInputParameter"/> flag,
        /// complex objects are supported
        /// </summary>
        public IReadOnlyDictionary<string, IDmnVariable> InputData =>
            Variables
                .Where(v => v.Value.IsInputParameter)
                .ToDictionary(i => i.Key, i => i.Value);

        /// <summary>
        /// Dictionary of available decisions by name
        /// </summary>
        public IReadOnlyDictionary<string, IDmnDecision> Decisions { get; }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="variables">Variables used while executing the DMN model</param>
        /// <param name="decisions">Dictionary of available decisions by name</param>
        public DmnDefinition(
            IReadOnlyDictionary<string, IDmnVariable> variables,
            IReadOnlyDictionary<string, IDmnDecision> decisions)
        {
            Id = Guid.NewGuid().ToString();
            Variables = variables;
            Decisions = decisions;
        }
    }
}
