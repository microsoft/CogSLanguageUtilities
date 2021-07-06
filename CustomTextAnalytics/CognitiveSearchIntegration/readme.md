# Cognitive Search Integration

This project aims to integrate your Custom Text app with Cognitive Search. 
Our goal is to make your app searchable.

### Scenario
If you have a Custom Text app which creates custom entities for legal documents, and assuming you have tons of documents in your data storage, you can integrate this app with Cognitive Search to easily search through your documents.
For examples, search for documents that mentioned some company or person, etc.

## How to Index Your App

![cognitive search integration pipeline](./Docs/Assets/indexing-pipeline.png)

## First: Prepare Your Resources

You need to provision the following resources:
 - Blob Container Data Store
	 - which contains your Custom Text documents (read more [here](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-blobs-introduction))
 - Cognitive Search Service
	 - which we use to index and search your application (read more [here](https://docs.microsoft.com/en-us/azure/search/search-what-is-azure-search))
 - Azure Function
	 - In order to enable Custom Text in the indexing pipeline, Cognitive Search needs an Azure function which calls the prediction endpoint for your Custom Text app.
	 - This is basically an azure function that when provided with some document, runs the Custom Text prediction endpoint and waits till it gets the entities result.


## Second: Deploy the Azure Function
We created the Azure function for you. You can find the required project [here](.\CustomTextAzureFunction\CustomTextAnalyticsFunction.sln).

Replace the secrets in the [Program.cs](.\CustomTextAzureFunction\CustomTextAnalytics.Function\Program.cs) file with yours
(CustomText app endpoint, key, modelId)

After that, simply deploy the function. See article [here](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs#publish-to-azure).


## Third: Run the 'Index' command

 - Configs file
	 - Fill the [configs.json](./Docs/Assets/configs.json) file with your service secrets (that you already provisioned)
	 - This configs file needs to be placed next to the cli tool to read it
 - Application Schema
	 - Provide your Custom Text application [schema.json](./Docs/Assets/app-schema.json) in the same provided format
 - Run the index command
	 - Run the index command, and wait till process finishes
```console
clutils index --schema <path/to/your/schema> --index-name <name-your-index-here>
```
- Check process success
	- check your cognitive search resource for the created index, and make sure the indexer runs without errors
    - please file any issues [here](https://github.com/microsoft/CogSLanguageUtilities/issues) if you have any troubles

## Search your app
Use the SearchClient sdk (see docs [here](https://docs.microsoft.com/en-us/azure/search/search-howto-dotnet-sdk#run-queries))

