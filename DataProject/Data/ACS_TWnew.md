# Pipeline ACS-CCFA-TWO
## ENVIRONMENT VARIABLES
 * SECRET_WORD 
 * DB_USER_FPDB 
 * REX_URL 
 * AUTH_HOST_URL 
## PORTS(input)
|PORT|PROTOCOL|
| ------ | ------ |
| 8080 | http |

To check the correctness of the pipeline settings, run ```curl http://<entry_point_url>:<entry_point_port>/SelfTest``` 

OR


 run ```curl -X 'GET' 'https://<entry_point_url>:<monitoring_port>/api/Monitoring/SelfTest'  -H 'accept: */*'```



Other ports( output ) see in ENVIRONMENT VARIABLES section
