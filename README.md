# Integration tools
# ENVIRONMENTS
|ENV_NAME|DEFAULT_VALUE|POSSIBLE VALUES|IS NECESSARY|DEPENDENT VARIABLES|DESCRIPTION|
| ------ | ------ | ------ | ------ | ------ | ------ |
|YAML_PATH|/app/Data/model.yml||true|None|YAML configuration file path|
|LOG_LEVEL|Debug|Verbose, Debug, Information, Warning, Error, Fatal|true|None|Logging level|
|LOG_PATH|||false|None|FilePath for logging.If variable absent-logging into default input/output |
# PORTS
|PORT_NAME|DEFAULT_VALUE|PROTOCOL|HEALTHCHECK|HEALTHCHEK ROUTE|METRICS|METRICS ROUTE|DESCRIPTION|
| ------ | ------ | ------ | ------ | ------ | ------ | ------ | ------ |
|PACKET_BEAT_PORT|15001|tcp|false|None|false||PACKETBEAT port (if listening PACKETBEAT)|
|http port|80|http|true|/api/Monitoring/ConsulHealthCheck|true|/api/Monitoring/getMetrics|http port|
|https port|443|https|true|/api/Monitoring/ConsulHealthCheck|true|/api/Monitoring/getMetrics|https port|
