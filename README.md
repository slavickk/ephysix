# Integration utility for CCFA
# ENVIRONMENTS
|ENV_NAME|DEFAULT_VALUE|POSSIBLE VALUES|IS NECESSARY|DEPENDENT VARIABLES|DESCRIPTION|
| ------ | ------ | ------ | ------ | ------ | ------ |
|YAML_PATH|/app/Data/model.yml|None||None|YAML configuration file path|
|LOG_LEVEL|Debug|Verbose, Debug, Information, Warning, Error, Fatal||None|Logging level|
|LOG_PATH||||None|FilePath for logging.If variable absent-logging into default input/output |
# PORTS
|PORT_NAME|DEFAULT_VALUE|PROTOCOL|HEALTHCHECK|HEALTHCHEK ROUTE|DESCRIPTION|
| ------ | ------ | ------ | ------ | ------ | ------ |
|PACKET_BEAT_PORT|15001|tcp|false||PACKETBEAT port ( if listening PAKKETBEAT)|
|http port|80|http|true|see adress/swagger|http port|
|https port|443|https|true|see adress/swagger|https port|
