/******************************************************************
 * File: ParsedExpressionCacheScopeEnum.cs
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
    /// Scope of the parsed expression cache
    /// </summary>
    public enum ParsedExpressionCacheScopeEnum
    {
        /// <summary>
        /// Don't cache parsed expressions
        /// </summary>
        None,
        /// <summary>
        /// Cache parsed expressions within the single execution run only
        /// </summary>
        Execution,
        /// <summary>
        /// Cache parsed expressions within execution context only
        /// </summary>
        Context,
        /// <summary>
        /// Cache parsed expressions for definition (cross execution contexts)
        /// </summary>
        Definition,
        /// <summary>
        /// Cache parsed expressions globally (cross definitions and execution contexts)
        /// </summary>
        Global
    }
}