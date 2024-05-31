/******************************************************************
 * File: DmnExecutionSnapshots.cs
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
using net.adamec.lib.common.dmn.engine.engine.decisions;
using net.adamec.lib.common.dmn.engine.engine.execution.result;

namespace net.adamec.lib.common.dmn.engine.engine.execution.context
{
    /// <summary>
    /// Set of available snapshots
    /// </summary>
    public sealed class DmnExecutionSnapshots
    {
        /// <summary>
        /// Set of available snapshots
        /// </summary>
        public IReadOnlyList<DmnExecutionSnapshot> Snapshots => snapshots;
        /// <summary>
        /// Internal set of available snapshots
        /// </summary>
        private readonly List<DmnExecutionSnapshot> snapshots = new List<DmnExecutionSnapshot>();

        /// <summary>
        /// Last snapshot available or null when there are no snapshot
        /// </summary>
        public DmnExecutionSnapshot Last => snapshots.Count > 0 ? snapshots[snapshots.Count - 1] : null;
        
        /// <summary>
        /// Internal CTOR
        /// </summary>
        internal DmnExecutionSnapshots()
        {

        }

        /// <summary>
        /// Creates the snapshot without the decision information
        /// </summary>
        /// <param name="ctx">Execution context to create snapshot for</param>
        internal void CreateSnapshot(DmnExecutionContext ctx)
        {
            CreateSnapshot(ctx, null, null);
        }

        /// <summary>
        /// Creates the snapshot with the decision information
        /// </summary>
        /// <param name="ctx">Execution context to create snapshot for</param>
        /// <param name="decision">Decision executed just before snapshot</param>
        /// <param name="result">Result of the decision executed just before snapshot</param>
        internal void CreateSnapshot(DmnExecutionContext ctx, IDmnDecision decision, DmnDecisionResult result)
        {
            snapshots.Add(new DmnExecutionSnapshot(snapshots.Count, ctx, decision, result));
        }

        /// <summary>
        /// Clears the set of snapshots
        /// </summary>
        internal void Reset()
        {
            snapshots.Clear();
        }
    }
}