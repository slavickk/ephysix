/******************************************************************
 * File: MultiCurrencyData.cs
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
    public class MultiCurrencyData
    {
        public uint FromAccountExhangeRate; //106.5
        public ulong OriginalAmount; //106.4
        public ushort OriginalCurrency; //106.2
        public ulong TOAccountAmount; //106.3
        public ushort TOAccountCurrency; //106.1
        public uint ToAccountExchangeRate; //106.6
    }
}