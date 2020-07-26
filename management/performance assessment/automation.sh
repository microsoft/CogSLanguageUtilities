#!/bin/sh
# scp -i "C:\Users\a-moshab\Downloads\LuisDocCracking_key.pem" "C:\Users\a-moshab\Desktop\LUIS-D Tool\management\performance assessment\automation.sh" "azureuser@104.45.234.238:/home/azureuser/script.sh"
# scp -i "C:\Users\a-moshab\Downloads\LuisDocCracking_key.pem" "azureuser@104.45.234.238:/home/azureuser/New_DATA" "C:\Users\a-moshab\Desktop\LUIS-D Tool\management\performance assessment\result.text"
for var in 0 1 2 3 4 5 6 7 8 9
do
    curl -H "Connection-String:DefaultEndpointsProtocol=https;AccountName=nourdocuments;AccountKey=jAAYWwUed9ZubOY0jnwus2FjibBflOIKpAg8voQFxjE7ZSpFErdUg4H801wY2/1PeUuOXEhZ5MxuyevXCuDEQQ==;EndpointSuffix=core.windows.net" "http://localhost:8007/api/performance/parser/parseAll?sourceContainerName=container1&destinationContainerName=container2" >> New_DATA
done