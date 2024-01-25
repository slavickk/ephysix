# Integration tools
About WebCamundaExecutors see [CamundaExecutors](WebApiCamundaExecutors/readme.md)
## Functionality
 The main function of integration tool - easy and fast transformation of real-time streams between different sources and destinations without programming(Low-Code system).
 
 The processing flow consists of sequentially executed steps, each of which consists of a receiver (optionally) , filter transform block and sender.
 
 The receiver transforms the input stream into an internal hierarchical structure. After filtering and converting  hierarchical structure convert to externals format on sender.
 
 Different kind of receiver support different types of protocols (http, tcp).
 
 Receivers can respose input format of stream( supports XML,JSON,BASE64...).
 
## OpenTelemetry & Jaeger  support
 2 environment variable needed:
 * JAEGER_AGENT_HOST e.g. localhost
 * JAEGER_AGENT_PORT e.g. 6831 (if not set ,jaeger support is switch off)
 * JAEGER_SAVE_CONTEXT - if variable set swith on save jaeger context file in  CONTEXT directory . Strongly
 it is not recommended to turn on in working mode

## Pipeline debug control 
 Switch on set  DEBUG_MODE environment variable, if DEBUG_MODE is absent , debugging info is switch off. 

## Pipeline save history control 
 Switch on set  LOG_HISTORY_MODE environment variable, if LOG_HISTORY_MODE is absent , logging input/output streams are switch off. 

## About OpenApi support
 HTTPReceiver also support Swagger emulation (based on json definition) . Each call transform to internal structure and after transfomation convert internal structure to return method of call. 
 This makes it possible to use the product as a proxy for a systems with existing API endpoint.



## Nearest aims
* OpenAPI Support
* Bult-in documentation(DocFX) ```https://dotnet.github.io/docfx/tutorial/docfx_getting_started.html```
* Kafka sender and receivier, CDC(Change Data Capture) receiver
* GUI Web-based tools for build integration pipelines
 
 
## ENVIRONMENTS
|ENV_NAME|DEFAULT_VALUE|POSSIBLE VALUES|IS NECESSARY|DEPENDENT VARIABLES|DESCRIPTION|
| ------ | ------ | ------ | ------ | ------ | ------ |
|YAML_PATH|/app/Data/model.yml||true|None|YAML configuration file path|
|LOG_LEVEL|Debug|Verbose, Debug, Information, Warning, Error, Fatal|true|None|Logging level|
|LOG_PATH|||false|None|FilePath for logging.If variable absent-logging into default input/output |
|LOG_HISTORY_MODE|||false|None|if variable presented ,swith on save Request &Answers from extended sources |
|LOG_EXT_STAT|||false|None|if variable presented ,swith on pipeline execution statistic |
|DEBUG_MODE|||false|None|if variable presented ,swith on extended pipeline log |
## PORTS
|PORT_NAME|DEFAULT_VALUE|PROTOCOL|HEALTHCHECK|HEALTHCHEK ROUTE|METRICS|METRICS ROUTE|DESCRIPTION|
| ------ | ------ | ------ | ------ | ------ | ------ | ------ | ------ |
|http port|80|http|true|/api/Monitoring/ConsulHealthCheck|true|/api/Monitoring/getMetrics|http port|
|https port|443|https|true|/api/Monitoring/ConsulHealthCheck|true|/api/Monitoring/getMetrics|https port|


See ENVIRONMETS and PORTS  specific  to the selected pipeline (YAML configuration file path with .md extension).Default directory ./DataProject/Data/

