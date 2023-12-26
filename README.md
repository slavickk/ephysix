# <ServiceName> MetaData (v. 0.0.20)

---
# <Short Description> Sync MetaData from all Sources

[TOC]
## Description
Service has some calls:
 - /health
 - /metrics

Скрипт раз в 60 секунд опрашивает талицу md.md_src, найдя строку с status=1 берет её в работу - полностью синхронизирует данную схему из удаленного источника с сохраненной. Если всё удачно, то статус меняется на 0, если ошибка - меняется на 2 (в этом случае надо искать ошибки в логе сервиса). 

## Deployment
### Environments
| ENV_NAME         | DEFAULT_VALUE       | POSSIBLE VALUES                       | IS NECCESARY | DEPENDENT VARIABLES | DESCRIPTION                                        |
|------------------|---------------------|---------------------------------------|--------------|---------------------|----------------------------------------------------|
| DB_URL_FPDB      | 192.168.75.220/fpdb |                                       | *            |                     | DSN базы данных для хранения                       |
| MDDRIVER         | postgresql+psycopg2 |                                       | *            |                     | Драйвер БД для хранения в стиле SQLAlchemy         |
| DB_USER_FPDB     | md                  |                                       | *            |                     | Login name в базу MD                               |
| DB_PASSWORD_FPDB | xxx                 |                                       | *            |                     | Password в базу DM                                 |
| CRYPTOPASS       | TestovyPass         |                                       | *            |                     | Пароль для шифрования чувствительных данных в базе |
| LOG_LEVEL        | INFO                | DEBUG, INFO, WARNING, ERROR, CRIDummyProtocol1AL | *            |                     | Уровень логирования                                |

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
 - metadata_uptime - время работы сервиса

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
"Container":"MetaData",
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
