/******************************************************************
 * File: DmnExecutionSnapshot.cs
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
using System.Linq;
using net.adamec.lib.common.dmn.engine.engine.decisions;
using net.adamec.lib.common.dmn.engine.engine.execution.result;

namespace net.adamec.lib.common.dmn.engine.engine.execution.context
{
    /// <summary>
    /// Snapshot of execution context generated at the beginning of <see cref="DmnExecutionContext.ExecuteDecision(string)">execution</see> and
    /// after execution of decision. The snapshot (step) is generated for each decision in dependency tree
    /// </summary>
    public sealed class DmnExecutionSnapshot
    {
        /// <summary>
        /// Zero based sequence number of the execution step
        /// </summary>
        public int Step { get; }
        /// <summary>
        /// Name of the decision executed just before snapshot (null for the snapshot #0 created at the beginning of execution)
        /// </summary>
        public string DecisionName => Decision?.Name;
        /// <summary>
        /// Decision executed just before snapshot (null for the snapshot #0 created at the beginning of execution)
        /// </summary>
        public IDmnDecision Decision { get; }
        /// <summary>
        /// Result of the decision executed just before snapshot (null for the snapshot #0 created at the beginning of execution)
        /// </summary>
        public DmnDecisionResult DecisionResult { get; }
        /// <summary>
        /// Snapshot (clone) of all execution variables in execution context
        /// </summary>
        public IReadOnlyList<DmnExecutionVariable> Variables { get; }

        /// <summary>
        /// Gets the variable by <paramref name="variableName"/>
        /// </summary>
        /// <param name="variableName">Name of the variable to retrieve</param>
        /// <returns>Variable with given <paramref name="variableName"/> or null when not found</returns>
        public DmnExecutionVariable this[string variableName] => Variables.FirstOrDefault(v => v.Name == variableName);

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="step">Sequence number of the execution step</param>
        /// <param name="ctx">Execution context to create snapshot for</param>
        internal DmnExecutionSnapshot(int step, DmnExecutionContext ctx)
        {
            Step = step;
            Variables = ctx.Variables.Values.Select(v => v.Clone()).ToList();
        }
        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="step">Sequence number of the execution step</param>
        /// <param name="ctx">Execution context to create snapshot for</param>
        /// <param name="decision">Decision executed just before snapshot</param>
        /// <param name="result">Result of the decision executed just before snapshot</param>
        internal DmnExecutionSnapshot(int step, DmnExecutionContext ctx, IDmnDecision decision, DmnDecisionResult result)
        : this(step, ctx)
        {
            Decision = decision;
            DecisionResult = result?.Clone();
        }

    }
}
