# ParserLibrary documentation

## HTTPReceiverSwagger

A Receiver implementation that uses Swagger/OpenAPI to parse requests
and validate requests and responses against the Swagger specification.

The Swagger specification is loaded from `swaggerSpecPath` in the pipeline configuration.

`HTTPReceiverSwagger` compiles the server side (the Controller and data classes) dynamically from this specification,
using [NSwag](https://github.com/RicoSuter/NSwag).
This dynamically compiled server code performs request validation.

The intermediate dynamically compiled server code can be saved to a file, e.g. for debugging/diagnostic,
which is enabled by setting `serverCodePath` to the file name in the pipeline configuration.

HTTPS is supported and can be enabled by setting `certSubject` to certificate subject to use in the pipeline configuration.
The certificate is loaded from the local machine store.

### Testing the HTTPReceiverSwagger locally

You can use `curl` to test the `HTTPReceiverSwagger` locally.
E.g. assuming the `HTTPReceiverSwagger` is running on port 8080,
and the pet `DummySender` has been configured in the pipeline definition
to return pets,
the following command will retrieve the pet with ID 123:

```bash
curl -v -X 'GET' 'https://localhost:8080/v2/pet/123' -H 'accept: text/plain'
```
TODO: once the JWT pull request is merged, update the above command to include the JWT token.

## JWT verification

The signer's certificate used for JWT verification is loaded
from the **local machine store** by its subject name
specified in `jwtIssueSigningCertSubject` in the pipeline configuration.

For development purposes, you can generate a self-signed certificate and a JWT token
by executing the `JWTTests.GenerateSelfSignedCertificate()` and `JWTTests.GenerateToken()` tests.
