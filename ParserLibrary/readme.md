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
