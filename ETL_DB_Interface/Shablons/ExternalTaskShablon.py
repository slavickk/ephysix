import requests
import json
import urllib3
import sys
                                      
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
    # Send a POST request with the JSON payload in the request body
    response = requests.post(url, json=request_data, verify=False)
    
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
{% endfor %}