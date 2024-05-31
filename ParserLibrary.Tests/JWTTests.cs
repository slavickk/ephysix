/******************************************************************
 * File: JWTTests.cs
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
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;

namespace ParserLibrary.Tests;

[TestFixture]
public class JWTTests
{
    /// <summary>
    /// Sample code to generate a JWT token that can be used to test the API.
    /// The token is valid for 30 minutes.
    /// </summary>
    [Test]
    public void GenerateToken()
    {
        // The sample self-signed-jwt-cert.p12 is generated below using GenerateSelfSignedCertificate()
        var key = new X509SecurityKey(new X509Certificate2("self-signed-jwt-cert.p12", "qwerty"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("sub", "slavickk") }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = credentials
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);

        Console.WriteLine("Generated token: " + handler.WriteToken(token));
    }
    
    /// <summary>
    /// Sample code to generate a self-signed certificate that can be used to sign JWT tokens.
    /// </summary>
    [Test]
    public void GenerateSelfSignedCertificate()
    {
        var rsa = RSA.Create();
        var req = new CertificateRequest("CN=jwtSigner", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(1));
        var certBytes = cert.Export(X509ContentType.Pfx, "qwerty");
        
        // Export the certificate as PKCS#12 (includes the private key)
        File.WriteAllBytes("self-signed-jwt-cert.p12", certBytes);
        
        // Export the certificate as PEM without the private key
        var pem = ExportCertificateAsPemWithoutPrivateKey(cert);
        File.WriteAllText("self-signed-jwt-cert.pem", pem);
    }

    /// <summary>
    /// Export a certificate as PEM without the private key.
    /// </summary>
    /// <param name="certificate"></param>
    /// <returns></returns>
    private static string ExportCertificateAsPemWithoutPrivateKey(X509Certificate2 certificate)
    {
        var builder = new StringBuilder();
        builder.AppendLine("-----BEGIN CERTIFICATE-----");
        builder.AppendLine(
            Convert.ToBase64String(
                certificate.Export(X509ContentType.Cert),
                Base64FormattingOptions.InsertLineBreaks));
        builder.AppendLine("-----END CERTIFICATE-----");
        return builder.ToString();
    }
}