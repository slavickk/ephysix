# <ServiceName> CamundaExternalTask (v. 0.0.12)

---
# <Short Description> Sceleton for Camunda External Task by Python 

[TOC]
## Description
Service has some calls:
 - /health
 - /metrics

Скрипт регистрируется в текущей Camunda. Ждет сообщения с тегом TOPIC="LoginDB". Выполняет SQLText по копированию данных из базы SRC в DST.
На стороне DST возможно использование нескольких таблиц. В случае операции ExecSQL, используется идентификатор SRC базы.  
## Переменные в процессе Camunda
| NAME               | EXAMPLE   | POSSIBLE VALUES | DESCRIPTION                                                                                                                                                                                                                                                                                            |
|--------------------|-----------|-----------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Oper               | None      | Refill          | сделать Truncate Target tables и только затем залить в неё данные                                                                                                                                                                                                                                      |
|                    |           | Recreate        | пересоздать Target tables, если hash в MD_Camunda_Hash отличается                                                                                                                                                                                                                                      |
|                    |           | ExecSQL         | Выполнить SQL на SName и вернуть результаты в виде JSON. Insert'ы делаются как обычно, вызов процедур делается в стиле ```select app_add_alert(jsonb_build_object('Stream',:expdate), jsonb_build_object('RexResult',:pan))```. Результат придет в Camunda Activities как ```{"Result": execresult}``` |
| SName              | DummySystem3      | md.md_src.name  | md_src.name источника данных (из него возьмем SDriver, SDSN, SLogin, SPassword)                                                                                                                                                                                                                        |
| TName              | DATAMART  | md.md_src.name  | md_src.name целевой БД (из него возьмем TDriver, TDSN, TLogin, TPassword)                                                                                                                                                                                                                              |
| SQLTable           | см. далее | см. далее       | Target таблицы массив JSON. Порядок важен, относительно него заполняются таблицы, связанные ключами.                                                                                                                                                                                                   |
| SQLText            | см. далее | см. далее       | SQL текст-выражение для выборки из источника или вызов функции через select                                                                                                                                                                                                                            |
| Передача в процесс | см. далее | см. далее       | Динамическая передача параметров в процесс                                                                                                                                                                                                                                                             |

### SQLTable
```JSON
[
	{
		"Table": "branch",
		"Columns": [
			{"ind": 20, "Name": "phone3", 		"Type": "VARCHAR"},
			{"ind": 21, "Name": "phone2", 		"Type": "VARCHAR"},
			{"ind": 22, "Name": "phone1", 		"Type": "VARCHAR"},
			{"ind": 23, "Name": "address3",		"Type": "VARCHAR"},
			{"ind": 24, "Name": "contactname2",	"Type": "VARCHAR"},
			{"ind": 25, "Name": "contactname1",	"Type": "VARCHAR"},
			{"ind": 26, "Name": "address2",		"Type": "VARCHAR"},
			{"ind": 27, "Name": "address1",		"Type": "VARCHAR"},
			{"ind": 28, "Name": "city",		"Type": "VARCHAR"},
			{"ind": 29, "Name": "region",		"Type": "VARCHAR"},
			{"ind": 30, "Name": "country",		"Type": "NUMBER"},
			{"ind": 31, "Name": "title",		"Type": "VARCHAR"},
			{"ind": 32, "Name": "institutionid",    "Type": "NUMBER"},
			{"ind": 33, "Name": "externalid",	"Type": "VARCHAR"},
			{"ind": 34, "Name": "contactname3",	"Type": "VARCHAR"}
		],
		"ExtIDs": [],
		"Indexes": [
			[22,23],
			[26,27]
		]
	},
	{
		"Table": "customer",
		"Columns": [
			{"ind": 35, "Name": "status",		"Type": "NUMBER"},
			{"ind": 36, "Name": "othernames",	"Type": "VARCHAR"},
			{"ind": 37, "Name": "startdate",	"Type": "DATE"},
			{"ind": 38, "Name": "institutionid",    "Type": "NUMBER"},
			{"ind": 39, "Name": "branchid",		"Type": "NUMBER"},
			{"ind": 40, "Name": "externalid",	"Type": "VARCHAR"},
			{"ind": 41, "Name": "phone1",		"Type": "VARCHAR"},
			{"ind": 42, "Name": "address1",		"Type": "VARCHAR"},
			{"ind": 43, "Name": "city",		"Type": "VARCHAR"},
			{"ind": 44, "Name": "country",		"Type": "NUMBER"},
			{"ind": 45, "Name": "inn",		"Type": "VARCHAR"},
			{"ind": 46, "Name": "gender",		"Type": "VARCHAR"},
			{"ind": 47, "Name": "lastname",		"Type": "VARCHAR"},
			{"ind": 48, "Name": "firstname",	"Type": "VARCHAR"},
			{"ind": 49, "Name": "birthdate",	"Type": "DATE"},
			{"ind": 50, "Name": "statustime",	"Type": "DATE"},
			{"ind": 51, "Name": "email",		"Type": "VARCHAR"}
		],
		"ExtIDs": [
			{"Column": "branchid",	"Table": "branch"}
		],
		"Indexes": []
	},
	{
		"Table": "account",
		"Columns": [
			{"ind":  1, "Name": "statustime",	"Type": "DATE"},
			{"ind":  2, "Name": "customerid",	"Type": "NUMBER"},
			{"ind":  3, "Name": "orignumber",	"Type": "VARCHAR"},
			{"ind":  4, "Name": "branchid",		"Type": "NUMBER"},
			{"ind":  5, "Name": "externalid",	"Type": "VARCHAR"},
			{"ind":  6, "Name": "currency",		"Type": "NUMBER"},
			{"ind":  7, "Name": "closedate",	"Type": "DATE"},
			{"ind":  8, "Name": "opendate",		"Type": "DATE"},
			{"ind":  9, "Name": "status",		"Type": "NUMBER"},
			{"ind": 11, "Name": "accountid",	"Type": "NUMBER"}
		],
		"ExtIDs": [
			{"Column": "branchid",	"Table": "branch"},
			{"Column": "customerid","Table": "customer"}
		]
	},
	{
		"Table": "card",
		"Columns": [
			{"ind": 10, "Name": "accountid",	"Type": "NUMBER"},
			{"ind": 12, "Name": "closedate",	"Type": "DATE"},
			{"ind": 13, "Name": "firstusedate",	"Type": "DATE"},
			{"ind": 14, "Name": "statustime",	"Type": "DATE"},
			{"ind": 15, "Name": "expirationdate",   "Type": "DATE"},
			{"ind": 16, "Name": "mbr",		"Type": "NUMBER"},
			{"ind": 17, "Name": "pan",		"Type": "VARCHAR"},
			{"ind": 18, "Name": "status",		"Type": "NUMBER"},
			{"ind": 19, "Name": "branchid",		"Type": "NUMBER"}
		],
		"ExtIDs": [
			{"Column": "accountid",	"Table": "account"},
			{"Column": "branchid",	"Table": "branch"}
		]
	}
]
```
### SQLText
```SQL
select account.statustime statustime,account.customerid customerid,account.orignumber orignumber,account.branchid branchid,account.externalid externalid,account.currency currency,account.closedate closedate,account.opendate opendate,account.status status
      ,account2card.accountid accountid,account2card.accountid accountid
	  ,card.closedate closedate,card.firstusedate firstusedate,card.statustime statustime,card.expirationdate expirationdate,card.mbr mbr,card.pan pan,card.status status,card.branchid branchid
	  ,branch.phone3 phone3,branch.phone2 phone2,branch.phone1 phone1,branch.address3 address3,branch.contactname2 contactname2,branch.contactname1 contactname1,branch.address2 address2,branch.address1 address1,branch.city city,branch.region region,branch.country country,branch.title title,branch.institutionid institutionid,branch.externalid externalid,branch.contactname3 contactname3
	  ,customer.status status,customer.othernames othernames,customer.startdate startdate,customer.institutionid institutionid,customer.branchid branchid,customer.externalid externalid,customer.phone1 phone1,customer.address1 address1,customer.city city,customer.country country,customer.inn inn,customer.gender gender,customer.lastname lastname,customer.firstname firstname,customer.birthdate birthdate,customer.statustime statustime,customer.email email 
 from account   
 inner join   account2card  on (account.id=account2card.accountid)  
 inner join   card  on (account2card.pan=card.pan AND account2card.mbr=card.mbr)  
 inner join  branch  on (branch.id=card.branchid)  
 inner join   customer  on (card.customerid=customer.id)  
 where card.pan = :pan
  AND card.expirationdate = TO_DATE (:expdate, 'DD.MM.YYYY')
```
### Передача в процесс
```JSON
{
	"pan": {
		"value": "676280519001970406",
		"type": "String"
	},
	"expdate": {
		"value": "01.06.2020",
		"type": "String"
	}
}
```
### Примеры вызова для разных Activities Camunda (Topic=LoginDB):
| Camunda Activities Name | Inputs Variables   | Type    | Value                         | DESCRIPTION                                                                                                                                        |
|-------------------------|--------------------|---------|-------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------|
| app_parse_alert         | Oper               | String  | ExecSQL                       | Запуск SQL функции и получение ответа в виде {"Result": True} или {"Result": {"Count": 5, "Errors":3}} или {"Result": ["Rule1", "Rule2", "Rule3"]} |
|                         | SName              | String  | FP                            | Источник данных в MetaData md.md_src.Name                                                                                                          |
|                         | SQLText            | String  | select alert_enrich(:alertid) | Вызов функции в БД. Функция запустит несколько отдельных Camunda процессов с одним GroupID==AlertID. У данной функции нет return параметров        |
|                         | alertid            | String  | \<AlertID>                    | Значение AlertID, которое проходит через весь процесс                                                                                              |

| Camunda Activities Name | Inputs Variables | Type   | Value                                       | DESCRIPTION                                                                                                                           |
|-------------------------|------------------|--------|---------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------|
| app_check_task          | Oper             | String | ExecSQL                                     | Запуск SQL функции и получение ответа в виде {"Result": "Wait"} или {"Result": "OK"} или {"Result": "Error"}                          |
|                         | SName            | String | FP                                          | Источник данных в MetaData md.md_src.Name                                                                                             |
|                         | SQLText          | String | select app_check_actions_by_group(:alertid) | Вызов функции в БД. Функция проверяет есть ли еще работающие app_actions с групповым ID=AlertID. Данная функция возвращает true/false |
|                         | alertid          | String | \<AlertID>                                  | Значение AlertID, которое проходит через весь процесс                                                                                 |

| Camunda Activities Name | Inputs Variables | Type   | Value                       | DESCRIPTION                                                                                                                              |
|-------------------------|------------------|--------|-----------------------------|------------------------------------------------------------------------------------------------------------------------------------------|
| app_alert_link          | Oper             | String | ExecSQL                     | Запуск SQL функции и получение ответа в виде {"Result": "Wait"} или {"Result": "OK"} или {"Result": "Error"}                             |
|                         | SName            | String | FP                          | Источник данных в MetaData md.md_src.Name                                                                                                |
|                         | SQLText          | String | select alert_link(:alertid) | Вызов функции в БД. Функция линкует текущий алерт с объектами, полученными в процессе обогащения. У данной функции нет return параметров |
|                         | alertid          | String | \<AlertID>                  | Значение AlertID, которое проходит через весь процесс                                                                                    |

| Camunda Activities Name | Inputs Variables | Type   | Value                                               | DESCRIPTION                                                                                                       |
|-------------------------|------------------|--------|-----------------------------------------------------|-------------------------------------------------------------------------------------------------------------------|
| app_check_alert         | Oper             | String | ExecSQL                                             | Запуск SQL функции и получение ответа в виде {"Result": "Wait"} или {"Result": "OK"} или {"Result": "Error"}      |
|                         | SName            | String | FP                                                  | Источник данных в MetaData md.md_src.Name                                                                         |
|                         | SQLText          | String | select app_check_alert_status(:alertid, <StatusID>) | Вызов функции в БД. Функция проверяет есть ли такой статус у алерта AlertID. Данная функция возвращает true/false |
|                         | alertid          | String | \<AlertID>                                          | Значение AlertID, которое проходит через весь процесс                                                             |


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
| CONSUL_ADDR            | 172.17.0.1                                     |                 | *            |                     | Внутренний адрес Consul (нужен только для DNS)                                                                                                                                                                                                                                                                                                                            |
| TOPIC                  | LoginDB                                        |                 | *            |                     | Имя топика в Camunda                                                                                                                                                                                                                                                                                                                                                      |
| CAMUNDA_NAME           | camunda.service.dc1.consul                     |                 | *            |                     | DNS адрес Camunda                                                                                                                                                                                                                                                                                                                                                         |
| DATAMARTDB             | DATAMART                                       |                 | *            |                     | Имя БД DataMart для фиксации ошибок                                                                                                                                                                                                                                                                                                                                       |

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
