/******************************************************************
 * File: POSConditionCodes.cs
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
    /// <summary>
    ///     <remarks>3D=D3 for compability</remarks>
    /// </summary>
    public enum POSConditionCodes : ushort
    {
        Normal = 0,
        CustomerNotPresent = 1,
        UnattendedCustomerOperatedTerminal = 2,
        MerchantSuspicious = 3,
        CustomerPresentCardNotPresent = 5,
        MailPhoneOrder = 8,
        CustomerIdentityVerified = 10,
        NormalWithBonuses = 11,
        VerificationOnly = 51,
        RecurringPayment = 52,
        InstallmentPayment = 53,
        VSECRequest = 59,
        RecurringPaymentSecureAttempt = 60,
        RecurringPaymentSecureVSEC = 61,
        RecurringPaymentNonSecureVSEC = 62,
        RecurringPaymentEMVChipTransaction = 63,
        InstallmentPaymentSecureAttempt = 64,
        InstallmentPaymentSecureVSEC = 65,
        InstallmentPaymentNonSecureVSEC = 66,
        InstallmentPaymentEMVChipTransaction = 67,
        Referral = 71,
        HardwareCryptographicCustomerAuthentication = 72,
        SoftwareCryptographicCustomerAuthentication = 73,
        MerchantRiskBasedDecisioning = 78,
        AuthenticationByIssuerRiskBasedDecisioning = 79,
        D3SecureSupportedOnlyByAcquirer = 81,
        D3Secure = 82,
        NonauthenticatedSETwithoutCardholderCertificateChipCryprogramUsed = 83,
        SecureSETWithCardholderCertificateChipCryptogramUsed = 84,
        SecureSETWithCardholderCertificate = 85,
        NonauthenticatedSETwithoutCardholderCertificate = 86,
        ChannelEncryptedVSEC = 87,
        NonSecureVSEC = 88,
        ChannelEncryptedVSECChipCriptogramUsed = 89,
        EMVChipTransaction = 91,
        EMVChipTransactionWithBonuses = 92,
        RepresentmentOfItem = 13,
        Chargeback = 17,
        ChargebackReversal = 54
    }
}