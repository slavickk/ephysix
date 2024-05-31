﻿/******************************************************************
 * File: DmnExecutionVariable.cs
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
using System.Diagnostics.CodeAnalysis;
using net.adamec.lib.common.core.logging;
using net.adamec.lib.common.dmn.engine.engine.definition;
using NLog;

namespace net.adamec.lib.common.dmn.engine.engine.execution.context
{
    /// <summary>
    /// Runtime (execution) variable
    /// </summary>
    public class DmnExecutionVariable
    {
        /// <summary>
        /// Logger
        /// </summary>
        protected static ILogger Logger = CommonLogging.CreateLogger<DmnExecutionVariable>();

        /// <summary>
        /// Variable definition
        /// </summary>
        protected IDmnVariable Definition { get; }

        /// <summary>
        /// Unique variable name
        /// </summary>
        public string Name => Definition.Name;
        /// <summary>
        /// Variable type or null if not defined
        /// </summary>
        public Type Type => Definition.Type;

        /// <summary>
        /// Flag whether the variable is input parameter
        /// </summary>
        public bool IsInputParameter => Definition.IsInputParameter;

        /// <summary>
        /// Backing field for <see cref="Value"/> property
        /// </summary>
        // ReSharper disable once InconsistentNaming
        protected object _value;

        /// <summary>
        /// Variable value
        /// </summary>
        /// <exception cref="DmnExecutorException">Setter: Can't override input parameter</exception>
        /// <exception cref="DmnExecutorException">Setter: Can't cast value to target type</exception>
        public virtual object Value
        {
            get => _value;
            set
            {
                if (IsInputParameter) throw Logger.Error<DmnExecutorException>("Can't override input parameter");
                SetValueInternal(value);
            }
        }


        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="definition">Variable definition</param>
        /// <exception cref="ArgumentNullException"><paramref name="definition"/> is null</exception>
        /// <exception cref="DmnExecutorException">Missing variable name</exception>
        public DmnExecutionVariable(IDmnVariable definition)
        {
            if (definition == null) throw Logger.Fatal<ArgumentNullException>($"{nameof(definition)} is null");

            if (string.IsNullOrWhiteSpace(definition.Name?.Trim()))
                throw Logger.Fatal<DmnExecutorException>($"Missing variable name");

            Definition = definition;
        }

        /// <summary>
        /// Sets the internal value without checking whether it's input param
        /// </summary>
        /// <param name="value">New variable value</param>
        /// <exception cref="DmnExecutorException">Can't cast value to target type</exception>
        protected void SetValueInternal(object value)
        {
            try
            {
                _value = Type != null && value != null
                    ? Convert.ChangeType(value, Type)
                    : value;
            }
            catch (Exception ex)
                when (ex is InvalidCastException ||
                      ex is FormatException ||
                      ex is OverflowException ||
                      ex is ArgumentNullException)
            {
                //most probably from Convert.ChangeType
                throw Logger.Error<DmnExecutorException>(
                    $"Can't cast value to target type: {Type?.Name ?? "[null]"}. Value is: {value} of type {value?.GetType().Name??"null"}");
            }
            catch (Exception ex) when (Logger.FatalFltr(ex))
            {
                //just log it and as Logger.FatalFltr returns false by default, the exception will not be catch (will be thrown)
            }
        }

        /// <summary>
        /// CTOR for <see cref="Clone"/>
        /// </summary>
        /// <param name="cloneFrom">Variable to clone</param>
        protected DmnExecutionVariable(DmnExecutionVariable cloneFrom)
        {
            Definition = cloneFrom.Definition;

            if (cloneFrom.Value is ICloneable cloneableValue)
            {
                _value = cloneableValue.Clone();
            }
            else
            {
                _value = cloneFrom._value;
            }
        }

        /// <summary>
        /// Clones the variable
        /// </summary>
        /// <returns>Cloned variable</returns>
        public virtual DmnExecutionVariable Clone()
        {
            var retVal = new DmnExecutionVariable(this);
            return retVal;
        }

        /// <summary>
        /// Sets the input parameter variable value
        /// </summary>
        /// <remarks>It's not possible to set the input parameter variable value using the <see cref="Value"/> property setter by design.
        /// This dedicated method it to be used to ensure that the input parameter value is set intentionally (from <see cref="DmnExecutionContext"/>)</remarks>
        /// <param name="value">Value to be set</param>
        /// <exception cref="DmnExecutorException">Variable is not an input parameter</exception>
        public virtual void SetInputParameterValue(object value)
        {
            if (!IsInputParameter) throw Logger.Error<DmnExecutorException>($"{Name} is not an input parameter");
           // _value = value;
           SetValueInternal(value);
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return $"{Name}{(Type == null ? "" : ":" + Type)}={Value ?? "[null]"}";
        }

        /// <summary>
        /// Gets the variable hashcode from name and value
        /// </summary>
        /// <returns>Variable hashcode</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + Name.GetHashCode();
                hash = hash * 23 + (Value?.GetHashCode() ?? 0);
                return hash;
            }
        }
    }
}
