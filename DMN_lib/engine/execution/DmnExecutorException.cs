﻿using System;

namespace net.adamec.lib.common.dmn.engine.engine.execution
{
    /// <summary>
    /// Exception thrown while executing (evaluating) the DMN Model
    /// </summary>
    public class DmnExecutorException : Exception
    {
        public string ownerName;
        public string expression;
        /// <summary>
        /// Creates <see cref="DmnExecutorException" /> with given <paramref name="message" /> and optional <paramref name="innerException" />
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Optional inner exception</param>
        public DmnExecutorException(string owner,string expression1 ,string message, Exception innerException = null) : base(message, innerException)
        {
            ownerName = owner;
            expression = expression1;

        }
    }
}
