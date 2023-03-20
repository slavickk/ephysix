using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234123412341234"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
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
}