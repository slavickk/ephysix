/******************************************************************
 * File: IGraphGeneration.cs
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

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGraphGeneration.cs" company="Jamie Dixon Ltd">
//   Jamie Dixon
// </copyright>
// <summary>
//   Defines the IGraphGeneration interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GraphVizWrapper
{
    public interface IGraphGeneration
    {
        /// <summary>
        /// Gets or sets the Rendering Engine to be used.
        /// </summary>
        Enums.RenderingEngine RenderingEngine { get; set; }

        /// <summary>
        /// Generates a graph based on the dot file passed in.
        /// </summary>
        /// <param name="dotFile">
        /// A string representation of a dot file.
        /// </param>
        /// <param name="returnType">
        /// The type of file to be returned.
        /// </param>
        /// <returns>
        /// a byte array.
        /// </returns>
        byte[] GenerateGraph(string dotFile, Enums.GraphReturnType returnType);
    }
}
