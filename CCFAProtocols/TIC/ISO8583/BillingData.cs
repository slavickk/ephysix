/******************************************************************
 * File: BillingData.cs
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

using System.Linq;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583
{
    public struct BillingData
    {
        public long? BillID;
        public string? VendorTitle;
        public string? BillingAcct;
        public string? StartPeriodDate;
        public string? EndPeriodDate;
        public string? EffectiveData;
        public double? Amount;
        public ushort? Currency;
        public string? ShortDescription;
        public string? Description;
        public string? BillAlias;
        public string? ParentBillID;
        public string? SubBillID;
        public string? VendorExtPaymentParam;
        public string? CustomerId;
        public string? CustomerDDAgentID;
        public string? ExtDescription;
        public short? VendorExactPayment;
        public short? AmountType;

        public BillingData(string record) : this()
        {
            string?[] strings = record.Split((char) UAMPSymbols.FS)
                .Select(s => s.Length == 1 && s[0] == (char) UAMPSymbols.NI ? null : s).ToArray();
            BillID = strings[0] is null ? null : long.Parse(strings[0]!);
            VendorTitle = strings[1];
            BillingAcct = strings[2];
            StartPeriodDate = strings[3];
            EndPeriodDate = strings[4];
            EffectiveData = strings[5];
            Amount = strings[6] is null ? null : double.Parse(strings[6]!);
            Currency = strings[7] is null ? null : ushort.Parse(strings[7]!);
            ShortDescription = strings[8];
            Description = strings[9];
            BillAlias = strings[10];
            ParentBillID = strings[11];
            SubBillID = strings[12];
            VendorExtPaymentParam = strings[13];
            CustomerId = strings[14];
            CustomerDDAgentID = strings[15];
            ExtDescription = strings[16];
            VendorExactPayment = strings[17] is null ? null : short.Parse(strings[17]!);
            AmountType = strings[18] is null ? null : short.Parse(strings[18]!);
        }


        public string GetString()
        {
            return string.Join((char) UAMPSymbols.FS,
                BillID ?? (char) UAMPSymbols.NI,
                VendorTitle ?? "" + (char) UAMPSymbols.NI,
                BillingAcct ?? "" + (char) UAMPSymbols.NI,
                StartPeriodDate ?? "" + (char) UAMPSymbols.NI,
                EndPeriodDate ?? "" + (char) UAMPSymbols.NI,
                EffectiveData ?? "" + (char) UAMPSymbols.NI,
                (object?) Amount ?? "" + (char) UAMPSymbols.NI,
                (object?) Currency ?? "" + (char) UAMPSymbols.NI,
                ShortDescription ?? "" + (char) UAMPSymbols.NI,
                Description ?? "" + (char) UAMPSymbols.NI,
                BillAlias ?? "" + (char) UAMPSymbols.NI,
                (object?) ParentBillID ?? "" + (char) UAMPSymbols.NI,
                SubBillID ?? "" + (char) UAMPSymbols.NI,
                VendorExtPaymentParam ?? "" + (char) UAMPSymbols.NI,
                CustomerId ?? "" + (char) UAMPSymbols.NI,
                CustomerDDAgentID ?? "" + (char) UAMPSymbols.NI,
                ExtDescription ?? "" + (char) UAMPSymbols.NI,
                (object?) VendorExactPayment ?? "" + (char) UAMPSymbols.NI,
                (object?) AmountType ?? "" + (char) UAMPSymbols.NI
            );
        }
    }
}