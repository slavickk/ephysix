/******************************************************************
 * File: CollectHitPolicyAggregationEnum.cs
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

namespace net.adamec.lib.common.dmn.engine.parser.dto
{
    /// <summary>
    /// Collect hit policy aggregation type
    /// </summary>
    public enum CollectHitPolicyAggregationEnum
    {
        /// <summary>
        /// None, return list of values
        /// </summary>
        List,
        /// <summary>
        /// The sum of all output values
        /// </summary>
        Sum,
        /// <summary>
        /// The smallest value of all output values
        /// </summary>
        Min,
        /// <summary>
        /// The largest value of all output values
        /// </summary>
        Max,
        /// <summary>
        /// The number of output values
        /// </summary>
        Count
    }
}
