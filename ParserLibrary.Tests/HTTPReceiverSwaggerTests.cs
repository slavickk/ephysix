using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Serilog;
using Serilog.Exceptions;

namespace ParserLibrary.Tests;

[TestFixture]
public class HTTPReceiverSwaggerTests
{
    [Test]
    public async Task HTTPReceiverSwaggerEnabledPipelineStarts_JWT_Disabled()
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.WithExceptionDetails()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
        
        var pip = Pipeline.loadFromString(@"pipelineDescription: Dummy pipeline
steps:
- isBridge: true
  iDStep: Step_0
  description: Dummy step
  ireceiver: !Plugins.HTTPReceiverSwagger
    address: localhost
    port: 8080
    swaggerSpecPath: TestData/swagger-dummy.json  # standard Swagger file format
    certSubject: localhost
#    jwtIssueSigningCertSubject: jwtSigner
  sender: !DummySender
    responseToReturn: method2Response  # key to get the response from the below dictionary of dummy responses
    dummyResponses:
      method2Response: ""
        {
            'AWAnswer': {
                'Vendor': 'Vendor 1',
                'REF': 'Ref 1',
                'Status': 'Accepted'
            }
        }""
", typeof(global::Plugins.HTTPReceiverSwagger).Assembly);
        
        var task = pip.run();
        
        // Send a GET to Method2
        using var client = new HttpClient();
        using var response = await client.GetAsync("https://localhost:8080/TestMethod");
   //     using var response = await client.GetAsync("https://localhost:8080/swagger");

   //     using var response = await client.GetAsync("https://localhost:8080/api/v1/export");
        
        Assert.IsTrue(response.IsSuccessStatusCode);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        using (var responseJson = System.Text.Json.JsonDocument.Parse(responseContent))
        {
            Assert.NotNull(responseJson);
            // TODO: server is converting property names to snake case; make them exactly match the specification
            if (responseJson.RootElement.TryGetProperty("awAnswer", out var awAnswer))
            {
                Assert.NotNull(awAnswer);
                Assert.AreEqual("Vendor 1", awAnswer.GetProperty("vendor").GetString());
                Assert.AreEqual("Ref 1", awAnswer.GetProperty("ref").GetString());
                Assert.AreEqual("Accepted", awAnswer.GetProperty("status").GetString());
            }
            else
            {
                Assert.Fail("Response does not contain AWAnswer");
            }
        }

        await pip.stop();
        
        Console.WriteLine("Waiting for pipeline to stop");
        await task;
        
        Console.WriteLine("Pipeline stopped");
    }
    
    [Test]
    [Ignore("TODO: finish the JWT-enabled pipeline test")]
    public async Task HTTPReceiverSwaggerEnabledPipelineStarts_JWT_Enabled()
    {
        // TODO: add a "jwtSigner" certificate to the machine store
        // TODO: sign the request with the above certificate
        
        Log.Logger = new LoggerConfiguration()
            .Enrich.WithExceptionDetails()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
        
        var pip = Pipeline.loadFromString(@"pipelineDescription: Dummy pipeline
steps:
- isBridge: true
  iDStep: Step_0
  description: Dummy step
  ireceiver: !HTTPReceiverSwagger
    address: localhost
    port: 8080
    swaggerSpecPath: TestData/swagger-dummy.json  # standard Swagger file format
    certSubject: localhost
    jwtIssueSigningCertSubject: jwtSigner
  sender: !DummySender
    responseToReturn: method2Response  # key to get the response from the below dictionary of dummy responses
    dummyResponses:
      method2Response: ""
        {
            'AWAnswer': {
                'Vendor': 'Vendor 1',
                'REF': 'Ref 1',
                'Status': 'Accepted'
            }
        }""
", null);
        
        var task = pip.run();
        
        // Send a GET to Method2
        using var client = new HttpClient();

        using var response = await client.GetAsync("https://localhost:8080/api/Method2");
        
        Assert.IsTrue(response.IsSuccessStatusCode);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        using (var responseJson = System.Text.Json.JsonDocument.Parse(responseContent))
        {
            Assert.NotNull(responseJson);
            // TODO: server is converting property names to snake case; make them exactly match the specification
            if (responseJson.RootElement.TryGetProperty("awAnswer", out var awAnswer))
            {
                Assert.NotNull(awAnswer);
                Assert.AreEqual("Vendor 1", awAnswer.GetProperty("vendor").GetString());
                Assert.AreEqual("Ref 1", awAnswer.GetProperty("ref").GetString());
                Assert.AreEqual("Accepted", awAnswer.GetProperty("status").GetString());
            }
            else
            {
                Assert.Fail("Response does not contain AWAnswer");
            }
        }

        await pip.stop();
        
        Console.WriteLine("Waiting for pipeline to stop");
        await task;
        
        Console.WriteLine("Pipeline stopped");
    }
}
