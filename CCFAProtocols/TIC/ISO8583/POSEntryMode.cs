/******************************************************************
 * File: POSEntryMode.cs
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
    public class POSEntryMode
    {
        public EntryMethod EntryMethod;
        public PinMethod PinMethod;
    }

    public enum PinMethod : byte
    {
        Unknown = 0,
        CanAcceptPin = 1,
        CannotAcceptPIN = 2,
        TerminalPINpadDown = 8
    }

    public enum EntryMethod : byte
    {
        Unknown = 0,
        ManyalKey = 1,
        MagneticStripeReadCVVnotReliable = 2,
        ConsumerPresentedQR = 3,
        OpticalChacterReader = 4,
        ICCCVVRelivle = 5,
        ContactlessEMV = 7,
        eCommerceChip = 9,
        MagneticStripeReadCVVReliable = 90,
        ContactlessMagneticStripeData = 91,
        ICCCVVUnreliable = 95
    }
}