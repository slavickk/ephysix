######################################################################
# File: ExternalTaskShablon.py
# Copyright (c) 2024 Vyacheslav Kotrachev
#
# This library is free software; you can redistribute it and/or
# modify it under the terms of the GNU Lesser General Public
# License as published by the Free Software Foundation; either
# version 2.1 of the License, or (at your option) any later version.
#
# This library is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
# Lesser General Public License for more details.
#
######################################################################
import requests
import json
import urllib3
import sys
from datetime import datetime
import datetime

                                      
# Suppress the InsecureRequestWarning
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

#print ('Number of arguments:', len(sys.argv), 'arguments.')
#print ('Argument List:', str(sys.argv))
OperUUID=None
if(len(sys.argv) <= {{package.CountVar}}):
    print("Usage:scriptname {% for var in package.Parameters) %}{{var.Name}} {% endfor %}")
    sys.exit()
{% for task in package.UsedExternalTasks %}
#{{task.topic}}:{{task.description}}
baseAddress = '{{task.url}}'
print("execute external task ",'{{task.topic}}','{{task.description}}')
password = "your_password"
timeout = 30  # Adjust this to your desired timeout value

# Construct the URL by concatenating the variables
url = f"{baseAddress}"

# Define the request payload as a Python dictionary
{% assign index=1 %}
request_data = {
  "OperUUID":OperUUID,
{% for var in package.Parameters) %}
  "{{var.Name}}":str(sys.argv[{{index}}]),{% assign index = index | plus: 1 %}{% endfor %}
{% for par in task.parameters %}
  "{{par.Name}}":'''{{par.Value}}''',{% endfor %}
}

try:
    time1 = datetime.datetime.now()

    print(time1.strftime("%Y-%m-%d %H:%M:%S"),"Send post request to url",url,"....",end='')
    # Send a POST request with the JSON payload in the request body
    response = requests.post(url, json=request_data, verify=False)

    delta = datetime.datetime.now() - time1

# time difference in seconds
    if response.status_code == 200:
        print("[OK]")
    else:
        print("[FAILED]")

    print("Request finished with code ",response.status_code, f" exec {delta.total_seconds()*1000} ms")
    
    # Check the HTTP status code
    if response.status_code == 200:
        # If the response status code is 200 (OK), parse the JSON response
        data = response.json()
        
        # Access the JSON data
        all_value = data.get("all")
        error_value = data.get("errors")
        if data.get("OperUUID") is not None:
            OperUUID=data.get("OperUUID")
        # Print the values
        print(f"all: {all_value}, error: {error_value}")
    else:
        # If the response status code is not 200, handle the error
        print(f"HTTP Error {response.status_code}: {response.text}")
        
except requests.exceptions.RequestException as e:
    # Handle network-related errors (e.g., connection error)
    print(f"Network Error: {e}")
    exit(1)
{% endfor %}
exit(0)