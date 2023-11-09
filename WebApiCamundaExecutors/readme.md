# Camunda executors
 
 
## ENVIRONMENTS
|ENV_NAME|DEFAULT_VALUE|POSSIBLE VALUES|IS NECESSARY|DEPENDENT VARIABLES|DESCRIPTION|
| ------ | ------ | ------ | ------ | ------ | ------ |
|CONSUL_ADDR|||true|None|Addr of consul|
|LOG_LEVEL|Debug|Verbose, Debug, Information, Warning, Error, Fatal|true|None|Logging level|
|DB_URL_FPDB||||None|URL of database|
|DB_USER_FPDB||||None|username of database|
|DB_PASSWORD_FPDB||||None|password of database|
|CRON_STRING||||None|cron string for sheluling of loading rates e.g.1 2 * * *|
|RDL_URL||||None|url of ReferenceDataLoader e.g.1 https://aa.dd.com:8853|
## PORTS
|PORT_NAME|DEFAULT_VALUE|PROTOCOL|HEALTHCHECK|HEALTHCHEK ROUTE|METRICS|METRICS ROUTE|DESCRIPTION|
| ------ | ------ | ------ | ------ | ------ | ------ | ------ | ------ |
|http port|80|http|true|/api/Monitoring/ConsulHealthCheck|true|/api/Monitoring/getMetrics|http port|
|https port|443|https|true|/api/Monitoring/ConsulHealthCheck|true|/api/Monitoring/getMetrics|https port|

# Development environment

Don't forget to configure the user name and email to be used in commits. Do this on every machine you use to work on the project.

Here is how to do it for the project (not globally):
```
git config --local user.name "Your Name"
git config --local user.email "Your Email"
```
