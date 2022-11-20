# <ServiceName> CamundaExternalTask (v. 0.0.1)

---
# <Short Description> Sceleton for Camunda External Task by Python 

[TOC]
## Description
Service has some calls:
 - /health
 - /metrics

Скрипт регистрируется в текущей Camunda. Ждет сообщения с тегом "LoginDB". Выполняет SQL по копированию данных из базы SRC в DST 

## Deployment
### Environments
| ENV_NAME               | DEFAULT_VALUE                                  | POSSIBLE VALUES | IS NECCESARY | DEPENDENT VARIABLES | DESCRIPTION                                                                                                                                                                                                                                                                                                                                                               |
|------------------------|------------------------------------------------|-----------------|--------------|---------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| MAX_TASKS              | 1                                              | 1-100           | *            |                     | Specifies the maximum amount of tasks that can be fetched within one request. This information is optional. Default is 10.                                                                                                                                                                                                                                                |
| LOCK_DURATION          | 10000                                          |                 | *            |                     | 	in milliseconds to lock the external tasks, must be greater than zero, the default lock duration is 20 seconds (20,000 milliseconds), is overridden by the lock duration configured on a topic subscription                                                                                                                                                              |
| ASYNC_RESPONCE_TIMEOUT | 5000                                           |                 | *            |                     | 	Asynchronous response (long polling) is enabled if a timeout is given. Specifies the maximum waiting time for the response of fetched and locked external tasks. The response is performed immediately, if external tasks are available in the moment of the request. This information is optional. Unless a timeout is given, fetch and lock responses are synchronous. |
| RETRIES                | 3                                              |                 | *            |                     | Количество попыток                                                                                                                                                                                                                                                                                                                                                        |
| RETRY_TIMEOUT          | 5000                                           |                 | *            |                     | Timeout одной попытки                                                                                                                                                                                                                                                                                                                                                     |
| SLEEP_SECONDS          | 30                                             |                 | *            |                     | Частота попыток                                                                                                                                                                                                                                                                                                                                                           |
| DBDRIVER               | postgresql+psycopg2                            |                 | *            |                     | Драйвер сервисного подключения к базе                                                                                                                                                                                                                                                                                                                                     |
| DBUSER                 | md                                             |                 | *            |                     | Логин сервисного подключения                                                                                                                                                                                                                                                                                                                                              |
| DBPASSWORD             | xxx                                            |                 | *            |                     | Пароль сервисного подключения                                                                                                                                                                                                                                                                                                                                             |
| DSN                    | master.pgsqlanomaly01.service.consul:5432/fpdb |                 | *            |                     | DSN сервисного подключения                                                                                                                                                                                                                                                                                                                                                |
| CONSUL_ADDR            | 172.17.0.1                                     |                 | *            |                     | Внутренний адрес Consul                                                                                                                                                                                                                                                                                                                                                   |
| TOPIC                  | LoginDB                                        |                 | *            |                     | Имя топика в Camunda                                                                                                                                                                                                                                                                                                                                                      |

### Ports
| PORT_NAME | DEFAULT_VALUE | PROTOCOL | HEALTHCHECK | HEALTHCHECK ROUTE | METRICS | METRICS ROUTE | DESCRIPTION |
| --------- | ------------- | -------- | ----------- |-------------------| ------- |---------------| ----------- |
|SERVER_PORT| 5000          |tcp/http2 | True        | /health     | True | /metrics  | |

### Dockerfile
```Dockerfile
FROM python:3.10-slim
WORKDIR /app
ENV PYTHONDONTWRITEBYTECODE 1
ENV PYTHONUNBUFFERED 1
COPY requirements.txt /app
RUN apt-get update && \
    apt-get -y install libpq-dev gcc procps libaio1 wget unzip && \
    wget -q https://download.oracle.com/otn_software/linux/instantclient/instantclient-basiclite-linuxx64.zip && \
    unzip instantclient-basiclite-linuxx64.zip && \
    rm -f instantclient-basiclite-linuxx64.zip && \
    cd instantclient* && \
    rm -f *jdbc* *occi* *mysql* *jar uidrvci genezi adrci && \
    echo /app/instantclient* > /etc/ld.so.conf.d/oracle-instantclient.conf && \
    ldconfig && \
    cd .. && \
    python -m pip install --upgrade pip && \
    pip install --no-cache-dir -r requirements.txt
COPY . /app
CMD ["python", "/app/main.py"]
```
### Signals
Сервис не поддерживает сигналы
### Metrics
#### Измерения:
 - service_uptime - время работы сервиса

### Logs format
```JSON
{
"Time":"2021-01-04 17:20:40,199",
"Level":"WARNING",
"Text":"This is a warning",
"Addr": "192.168.1.1:12345",
"Code":0,
"Class":"test1",
"ThreadId":19432,
"ExtUID":"",
"Container":"ExternalTask",
"Host":"",
"Service":"MAIN"
}
```
Level=[DEBUG, INFO, WARNING, ERROR, CRIDummyProtocol1AL]\
Code > 0 у сообщений с ошибкой\
Addr - Адрес:Порт контейнера сервиса\
Class - Имя файла-пакета\
Service - Наименование модуля\
ExtUID - Должны заполнятся системой оркестрации\
Container - MetaData\
Host - Должны заполнятся системой оркестрации 
