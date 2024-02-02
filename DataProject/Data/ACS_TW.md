# Pipeline ACS-CCFA-TWO
## ENVIRONMENT VARIABLES
 * SECRET_WORD :Phrase which used for hash sensivity information  
 * BUFF_DIR :Directory for saved messages (if ccfa not available) e.g. /CACHE1
 * FID :Financial identifier of bank e.g. CUB_BANK 
 * DB_USER_FPDB :If fpdb not available , set empty (not skipped) 
 * DB_PASSWORD_FPDB :If fpdb not available , set empty (not skipped) 
 * DB_URL_FPDB :If fpdb not available , set empty (not skipped) 
 * CCFA_SEND_TIMEOUT :CCFA send timeout in milliseconds 
 * REX_URL :CCFA EP entry point url e.g. http://192.168.75.214
 * TWO_SEND_TIMEOUT :TWO send timeout in milliseconds 
 * AUTH_HOST_URL :auth host(TX or TW) entry point url e.g. http://192.168.75.173:30212
## PORTS(input)
|PORT|PROTOCOL|
| ------ | ------ |
| 8080 | http |

To check the correctness of the pipeline settings, run ```curl http://<entry_point_url>:<entry_point_port>/SelfTest``` 

OR


 run ```curl -X 'GET' 'https://<entry_point_url>:<monitoring_port>/api/Monitoring/SelfTest'  -H 'accept: */*'```



Other ports( output ) see in ENVIRONMENT VARIABLES section
