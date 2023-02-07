# Integration tools
## Functionality
 The main function of integration tool - easy and fast transformation of real-time streams between different sources and destinations without programming(Low-Code system).
 
 The processing flow consists of sequentially executed steps, each of which consists of a receiver (optionally) , filter transform block and sender.
 
 The receiver transforms the input stream into an internal hierarchical structure. After filtering and converting  hierarchical structure convert to externals format on sender.
 
 Different kind of receiver support different types of protocols (http, tcp).
 
 Receivers can respose input format of stream( supports XML,JSON,BASE64...).
 
## OpenTelemetry & Jaeger  support
 2 environment variable needed:
 * JAEGER_AGENT_HOST e.g. localhost
 * JAEGER_AGENT_PORT e.g. 6831
 
## About OpenApi support
 HTTPReceiver also support Swagger emulation (based on json definition) . Each call transform to internal structure and after transfomation convert internal structure to return method of call. 
 This makes it possible to use the product as a proxy for a systems with existing API endpoint.



## Nearest aims
* OpenAPI Support
* Bult-in documentation(DocFX) https://dotnet.github.io/docfx/tutorial/docfx_getting_started.html
* Kafka sender and receivier, CDC(Change Data Capture) receiver
* GUI Web-based tools for build integration pipelines
 
 
## ENVIRONMENTS(obsolete)
|ENV_NAME|DEFAULT_VALUE|POSSIBLE VALUES|IS NECESSARY|DEPENDENT VARIABLES|DESCRIPTION|
| ------ | ------ | ------ | ------ | ------ | ------ |
|YAML_PATH|/app/Data/model.yml||true|None|YAML configuration file path|
|LOG_LEVEL|Debug|Verbose, Debug, Information, Warning, Error, Fatal|true|None|Logging level|
|LOG_PATH|||false|None|FilePath for logging.If variable absent-logging into default input/output |
## PORTS(obsolete)
|PORT_NAME|DEFAULT_VALUE|PROTOCOL|HEALTHCHECK|HEALTHCHEK ROUTE|METRICS|METRICS ROUTE|DESCRIPTION|
| ------ | ------ | ------ | ------ | ------ | ------ | ------ | ------ |
|PACKET_BEAT_PORT|15001|tcp|false|None|false||PACKETBEAT port (if listening PACKETBEAT)|
|http port|80|http|true|/api/Monitoring/ConsulHealthCheck|true|/api/Monitoring/getMetrics|http port|
|https port|443|https|true|/api/Monitoring/ConsulHealthCheck|true|/api/Monitoring/getMetrics|https port|

# Development environment

Don't forget to configure the user name and email to be used in commits. Do this on every machine you use to work on the project.

Here is how to do it for the project (not globally):
```
git config --local user.name "Your Name"
git config --local user.email "Your Email"
```
