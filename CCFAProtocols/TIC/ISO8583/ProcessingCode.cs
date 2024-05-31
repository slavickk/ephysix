/******************************************************************
 * File: ProcessingCode.cs
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

namespace CCFAProtocols.TIC.ISO8583
{
    public class ProcessingCode
    {
        public AccountType FromAccountType;
        public AccountType ToAccountType;
        public ushort TransactionCode;

        public override string ToString()
        {
            return
                $"FromAccountType: {Enum.GetName<AccountType>(FromAccountType)}({(byte)FromAccountType}),ToAccountType: {Enum.GetName<AccountType>(ToAccountType)}({(byte)ToAccountType}),TransactionCode:{TransactionCode}";
        }
    }
}