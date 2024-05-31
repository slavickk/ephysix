/******************************************************************
 * File: IDmnDecision.cs
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
using net.adamec.lib.common.dmn.engine.engine.definition;
using net.adamec.lib.common.dmn.engine.engine.execution;
using net.adamec.lib.common.dmn.engine.engine.execution.context;
using net.adamec.lib.common.dmn.engine.engine.execution.result;

namespace net.adamec.lib.common.dmn.engine.engine.decisions
{
    /// <summary>
    /// Decision interface
    /// </summary>
    public interface IDmnDecision
    {
        /// <summary>
        /// Unique name of the decision
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Decision required inputs (input variables)
        /// </summary>
        IReadOnlyCollection<IDmnVariable> RequiredInputs { get; }
        /// <summary>
        /// List of decisions, the decision depends on
        /// </summary>
        IReadOnlyCollection<IDmnDecision> RequiredDecisions { get; }

        /// <summary>
        /// Executes the decision.
        /// </summary>
        /// <param name="context">DMN Engine execution context</param>
        /// <param name="executionId">Identifier of the execution run</param>
        /// <returns>Decision result</returns>
        DmnDecisionResult Execute(DmnExecutionContext context, string executionId);


    }
}
