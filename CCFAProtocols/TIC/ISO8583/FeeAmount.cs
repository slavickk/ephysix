/******************************************************************
 * File: FeeAmount.cs
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

namespace CCFAProtocols.TIC.ISO8583
{
    public class AcquirerFeeAmount
    {
        /// <summary>
        ///     <value>
        ///         <para>'D'-withdraw, true</para>
        ///         <para>'C'-append, false</para>
        ///     </value>
        /// </summary>
        public char _isWithdraw;

        public uint Amount;

        /// '
        /// <inheritdoc cref="_isWithdraw" />
        public bool IsWithdraw
        {
            get => _isWithdraw == 'D';
            set => _isWithdraw = value ? 'D' : 'C';
        }

        public override string ToString()
        {
            var str = "FeeAmount:";
            foreach (var field in typeof(AcquirerFeeAmount).GetFields())
                str += $"\n\t\t{field.Name}: {field.GetValue(this)}";

            return str;
        }
    }
}