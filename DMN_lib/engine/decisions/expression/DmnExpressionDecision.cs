/******************************************************************
 * File: DmnExpressionDecision.cs
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
using net.adamec.lib.common.core.logging;

using net.adamec.lib.common.dmn.engine.engine.definition;
using net.adamec.lib.common.dmn.engine.engine.execution.context;
using net.adamec.lib.common.dmn.engine.engine.execution.result;
using NLog;

namespace net.adamec.lib.common.dmn.engine.engine.decisions.expression
{
    /// <summary>
    /// Expression decision definition
    /// </summary>
    public class DmnExpressionDecision : DmnDecision
    {
        /// <summary>
        /// Decision expression
        /// </summary>
        public string Expression { get; }

        /// <summary>
        /// Decision output variable
        /// </summary>
        public IDmnVariable Output { get; }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="name">Unique name of the decision</param>
        /// <param name="expression">Decision expression</param>
        /// <param name="output">Decision output variable</param>
        /// <param name="requiredInputs">Decision required inputs (input variables)</param>
        /// <param name="requiredDecisions">List of decisions, the decision depends on</param>
        public DmnExpressionDecision(
            string name,
            string expression,
            IDmnVariable output,
            IReadOnlyCollection<IDmnVariable> requiredInputs,
            IReadOnlyCollection<IDmnDecision> requiredDecisions)
        : base(name, requiredInputs, requiredDecisions)
        {
            Expression = expression;
            Output = output;
        }

        /// <summary>
        /// Evaluates the decision.
        /// </summary>
        /// <param name="context">DMN Engine execution context</param>
        /// <param name="executionId">Identifier of the execution run</param>
        /// <returns>Decision result</returns>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is nul</exception>
        protected override DmnDecisionResult Evaluate(DmnExecutionContext context, string executionId)
        {
            if (context == null) throw Logger.FatalCorr<ArgumentNullException>(executionId,$"{nameof(context)} is null");

            Logger.InfoCorr(executionId, $"Evaluating expressiong decision {Name} with expression {Expression}...");
            var result = context.EvalExpression(Expression, Output.Type,executionId,this.Name);
            Logger.InfoCorr(executionId, $"Evaluated expressiong decision {Name} with expression {Expression}");
            var outVariable = context.GetVariable(Output);
            outVariable.Value = result;
            return new DmnDecisionResult(new DmnDecisionSingleResult(context.GetVariable(Output)));
        }
    }
}
