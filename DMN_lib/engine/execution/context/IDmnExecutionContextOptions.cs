/******************************************************************
 * File: IDmnExecutionContextOptions.cs
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

namespace net.adamec.lib.common.dmn.engine.engine.execution.context
{
    /// <summary>
    /// Execution configuration options
    /// </summary>
    public interface IDmnExecutionContextOptions
    {
        /// <summary>
        /// Flag whether to record snapshots of variables and results during the execution (false by default).
        /// This option can be used for execution tracking/audit and/or debugging
        /// </summary>
        bool RecordSnapshots { get; }
        /// <summary>
        /// Flag whether to evaluate the table rules in parallel (true by default).
        /// This option can be used for performance tuning.
        /// </summary>
        bool EvaluateTableRulesInParallel { get; }
        /// <summary>
        /// Flag whether to evaluate the table outputs for positive rules in parallel (false by default).
        /// This option can be used for performance tuning.
        /// </summary>
        bool EvaluateTableOutputsInParallel { get; }
        /// <summary>
        /// Scope of the parsed expression cache (Cache parsed expressions for definition cross contexts by default)
        /// </summary>
        ParsedExpressionCacheScopeEnum ParsedExpressionCacheScope { get; }
    }
}