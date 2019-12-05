![](https://github.com/Microsoft/MCW-Template-Cloud-Workshop/raw/master/Media/ms-cloud-workshop.png 'Microsoft Cloud Workshops')

<div class="MCWHeader1">
Big data and visualization
</div>

<div class="MCWHeader2">
Hands-on lab unguided
</div>

<div class="MCWHeader3">
August 2018
</div>

Information in this document, including URL and other Internet Web site references, is subject to change without notice. Unless otherwise noted, the example companies, organizations, products, domain names, e-mail addresses, logos, people, places, and events depicted herein are fictitious, and no association with any real company, organization, product, domain name, e-mail address, logo, person, place or event is intended or should be inferred. Complying with all applicable copyright laws is the responsibility of the user. Without limiting the rights under copyright, no part of this document may be reproduced, stored in or introduced into a retrieval system, or transmitted in any form or by any means (electronic, mechanical, photocopying, recording, or otherwise), or for any purpose, without the express written permission of Microsoft Corporation.

Microsoft may have patents, patent applications, trademarks, copyrights, or other intellectual property rights covering subject matter in this document. Except as expressly provided in any written license agreement from Microsoft, the furnishing of this document does not give you any license to these patents, trademarks, copyrights, or other intellectual property.

The names of manufacturers, products, or URLs are provided for informational purposes only and Microsoft makes no representations and warranties, either expressed, implied, or statutory, regarding these manufacturers or the use of the products with any Microsoft technologies. The inclusion of a manufacturer or product does not imply endorsement of Microsoft of the manufacturer or product. Links may be provided to third party sites. Such sites are not under the control of Microsoft and Microsoft is not responsible for the contents of any linked site or any link contained in a linked site, or any changes or updates to such sites. Microsoft is not responsible for webcasting or any other form of transmission received from any linked site. Microsoft is providing these links to you only as a convenience, and the inclusion of any link does not imply endorsement of Microsoft of the site or the products contained therein.

© 2018 Microsoft Corporation. All rights reserved.

Microsoft and the trademarks listed at https://www.microsoft.com/en-us/legal/intellectualproperty/Trademarks/Usage/General.aspx are trademarks of the Microsoft group of companies. All other trademarks are property of their respective owners.

**Contents**

<!-- TOC -->

- [Big data and visualization hands-on lab unguided](#big-data-and-visualization-hands-on-lab-unguided)
  - [Abstract and learning objectives](#abstract-and-learning-objectives)
  - [Overview](#overview)
  - [Solution architecture](#solution-architecture)
  - [Requirements](#requirements)
  - [Exercise 1: Build a Machine Learning Model](#exercise-1-build-a-machine-learning-model)
    - [Task 1: Create your Azure Machine Learning project](#task-1-create-your-azure-machine-learning-project)
    - [Task 2: Upload the sample datasets](#task-2-upload-the-sample-datasets)
    - [Task 3: Open Azure Databricks and complete lab notebooks](#task-3-open-azure-databricks-and-complete-lab-notebooks)
    - [Task 4: Configure your Machine Learning environment](#task-4-configure-your-machine-learning-environment)
  - [Exercise 2: Setup Azure Data Factory](#exercise-2-setup-azure-data-factory)
    - [Task 1: Download and stage data to be processed](#task-1-download-and-stage-data-to-be-processed)
    - [Task 2: Install and configure Azure Data Factory Integration Runtime on the lab VM](#task-2-install-and-configure-azure-data-factory-integration-runtime-on-the-lab-vm)
    - [Task 3: Configure Azure Data Dactory](#task-3-configure-azure-data-dactory)
  - [Exercise 3: Deploy your machine learning model with Azure ML](#exercise-3-deploy-your-machine-learning-model-with-azure-ml)
    - [Task 1: Edit the scoring and configuration files](#task-1-edit-the-scoring-and-configuration-files)
    - [Task 2: Deploy the model](#task-2-deploy-the-model)
  - [Exercise 4: Develop a data factory pipeline for data movement](#exercise-4-develop-a-data-factory-pipeline-for-data-movement)
    - [Task 1: Create copy pipeline using the Copy Data Wizard](#task-1-create-copy-pipeline-using-the-copy-data-wizard)
  - [Exercise 5: Operationalize ML scoring with Azure Databricks and Data Factory](#exercise-5-operationalize-ml-scoring-with-azure-databricks-and-data-factory)
    - [Task 1: Create Azure Databricks Linked Service](#task-1-create-azure-databricks-linked-service)
    - [Task 2: Complete the BatchScore Azure Databricks notebook code](#task-2-complete-the-batchscore-azure-databricks-notebook-code)
    - [Task 3: Trigger workflow](#task-3-trigger-workflow)
  - [Exercise 6: Summarize data using Azure Databricks](#exercise-6-summarize-data-using-azure-databricks)
    - [Task 1: Summarize delays by airport](#task-1-summarize-delays-by-airport)
  - [Exercise 7: Visualizing in Power BI Desktop](#exercise-7-visualizing-in-power-bi-desktop)
    - [Task 1: Obtain the JDBC connection string to your Azure Databricks cluster](#task-1-obtain-the-jdbc-connection-string-to-your-azure-databricks-cluster)
    - [Task 2: Connect to Azure Databricks using Power BI Desktop](#task-2-connect-to-azure-databricks-using-power-bi-desktop)
    - [Task 3: Create Power BI report](#task-3-create-power-bi-report)
  - [Exercise 8: Deploy intelligent web app](#exercise-8-deploy-intelligent-web-app)
    - [Task 1: Deploy web app from GitHub](#task-1-deploy-web-app-from-github)
  - [After the hands-on lab](#after-the-hands-on-lab)
    - [Task 1: Delete resource group](#task-1-delete-resource-group)

<!-- /TOC -->

# Big data and visualization hands-on lab unguided

## Abstract and learning objectives

This hands-on lab is designed to provide exposure to many of Microsoft's transformative line of business applications built using Microsoft big data and advanced analytics.

By the end of the lab, you will be able to show an end-to-end solution, leveraging many of these technologies, but not necessarily doing work in every component possible.

## Overview

Margie's Travel (MT) provides concierge services for business travelers. In an increasingly crowded market, they are always looking for ways to differentiate themselves, and provide added value to their corporate customers.

They are looking to pilot a web app that their internal customer service agents can use to provide additional information useful to the traveler during the flight booking process. They want to enable their agents to enter in the flight information and produce a prediction as to whether the departing flight will encounter a 15-minute or longer delay, considering the weather forecasted for the departure hour.

## Solution architecture

Below is a diagram of the solution architecture you will build in this lab. Please study this carefully so you understand the whole of the solution as you are working on the various components.

![This is the high-level overview diagram of the end-to-end solution.](../Whiteboard%20design%20session/media/high-level-overview.png 'High-level overview diagram')

## Requirements

1. Microsoft Azure subscription must be pay-as-you-go or MSDN.

    a. Trial subscriptions will not work.

2. Follow all the steps provided in [Before the Hands-on Lab](Before%20the%20HOL%20-%20Big%20data%20and%20visualization.md).

## Exercise 1: Build a Machine Learning Model

Duration: 60 minutes

In this exercise, you will implement a classification experiment. You will load the training data from your local machine into a dataset. Then, you will explore the data to identify the primary components you should use for prediction, and use two different algorithms for predicting the classification. You will then evaluate the performance of both and algorithms choose the algorithm that performs best. The model selected will be exposed as a web service that is integrated with the sample web app.

### Task 1: Create your Azure Machine Learning project

_Tasks to complete_:

- Launch Azure Machine Learning Workbench from within your DSVM.
- Sign in with your Azure account when prompted.
- Create a new project named "FlightDelays", setting the project directory to "C:\HOL", and using the Blank Project template.

_Exit criteria_:

- You have the following new file path with a default project structure: C:\HOL\FlightDelays.

  ![Project structure generated after creating new Workbench project](media/new-project-structure.png)

### Task 2: Upload the sample datasets

_Tasks to complete_:

- Download three CSV sample datasets from <http://bit.ly/2wGAqrl>.

- Extract the ZIP, and verify you have the following files:

  - FlightDelaysWithAirportCodes.csv
  - FlightWeatherWithAirportCodes.csv
  - AirportCodeLocationClean.csv

- Upload the sample CSVs as new tables within your Azure Databricks workspace.

_Exit criteria_:

- There should be a total of three files that are uploaded. Each table should be named as follows:

  - flightweatherwithairportcode_csv to **flight_weather_with_airport_code**
  - flightdelayswithairportcodes_csv to **flight_delays_with_airport_codes**
  - airportcodelocationlookupclean_csv to **airport_code_location_lookup_clean**

  ![Azure Databricks tables shown after all three files uploaded](media/uploaded-data-files.png 'Uploaded data files')

### Task 3: Open Azure Databricks and complete lab notebooks

_Tasks to complete_:

- Download the following files:

  - [01 Prepare Flight Data.dbc](lab-files/01%20Prepare%20Flight%20Data.dbc)
  - [02 Predict Flight Delays.dbc](lab-files/02%20Predict%20Flight%20Delays.dbc)

- Import those notebooks into your Azure Databricks workspace.

- Attach your cluster to the notebooks when after opening, and complete them in order. Make sure to complete all of the **#TODO:** lines.

_Exit criteria_:

- You have successfully run each cell of both notebooks without errors. If you get stuck at any point, you can download and compare against the "complete" versions of those notebooks.

### Task 4: Configure your Machine Learning environment

_Tasks to complete_:

- The first thing you need to do before deploying your model is to enable the container-based Resource Providers on your Azure subscription.

- Navigate to the Azure Portal.

- Select All Services, Subscriptions and then select your subscription from the list.

- Under the Settings grouping, select Resource Providers.

  ![Select resource providers in the menu](media/resource-providers-menu.png 'Resource providers in the menu')

- Search for "container" and in the list that appears verify that all resource providers related to containers are registered. If not, select the Register link next to the items that are not registered.

  ![Search for containers and enable all resource providers](media/enable-container-resource-providers.png 'Enable container resource providers')

- Search for and enable the **Microsoft.MachineLearningServices** service provider.

- From your Lab VM (DSVM), browse to the download folder in File Explorer, and extract **flight_delays.zip** in place. This file was downloaded at the end of the **02 Predict Flight Delays complete** notebook.

- Within the extracted folder, navigate to dbfs\tmp\models and copy the **pipelineModel** subfolder, then paste it into C:\HOL\FlightDelays.

- Open the Command Prompt using the File menu within the Azure Machine Learning Workbench (making sure your project is first selected).

- Execute the following to update/install the Azure CLI packages:

  ```bash
  pip install azure-cli azure-cli-ml azure-ml-api-sdk
  ```

- Execute the following to upgrade the `pyspark` version:

  ```bash
  pip install pyspark --upgrade
  ```

- Set up your machine learning environment with the following command:

  ```bash
  az ml env setup -c -n <environment name> --location <location> --resource-group <yourresourcegroupname>
  ```

      Replace the tokens above with appropriate values:

      - For <environment name> enter flightdelays, or something similar. This value can only contain lowercase alphanumeric characters.
      - For <location>, use eastus2, westcentralus, australiaeast, westeurope, or southeastasia, as those are the only acceptable values at this time.
      - For <yourresourcegroupname>, enter the resource group name you've been using for this lab.

- Sign into Azure using your web browser if prompted.

- Select your subscription name back in the command prompt.

_Exit criteria_:

- The ACS cluster is successfully created. It will take **10-20 minutes** to complete. Periodically check the status with `az ml env show -g <resourceGroupName> -n <clusterName>` and move on to the next exercise while waiting.

## Exercise 2: Setup Azure Data Factory

Duration: 20 minutes

In this exercise, you will create a baseline environment for Azure Data Factory development for further operationalization of data movement and processing. You will create a Data Factory service, and then install the Data Management Gateway which is the agent that facilitates data movement from on-premises to Microsoft Azure.

### Task 1: Download and stage data to be processed

_Tasks to complete_:

- Initiate an RDP connection to the Lab VM (DSVM) you created in the Before the Lab section.

- Download and extract the ZIP containing sample data to a folder named C:\\Data on your Lab VM. The data can be downloaded from <http://bit.ly/2zi4Sqa>.

_Exit criteria_:

- You have a folder containing sample data files, partitioned by year and month on the C:\\ drive of your Lab VM.

### Task 2: Install and configure Azure Data Factory Integration Runtime on the lab VM

_Tasks to complete_:

- Download and install the latest version of Azure Data Factory Integration Runtime from <https://www.microsoft.com/en-us/download/details.aspx?id=39717>.

_Exit criteria_:

- The Azure Data Factory Integration Runtime is installed and running on your system. Keep in open for now. We will come back to this screen once we have provisioned the Data Factory in Azure, and obtain the gateway key so we can connect Data Factory to this "on-premises" server.

  ![The Microsoft Integration Runtime Configuration Manager, Register Integration Runtime page displays.](media/image119.png 'Register Integration Runtime page')

### Task 3: Configure Azure Data Dactory

_Tasks to complete_:

- Create a new Integration Runtime (gateway), and connect it to the Azure Data Factory Integration Runtime running on your Lab VM.

_Exit criteria_:

- You have authored an Integration Runtime in ADF, and successfully connected it to the ADF Integration Runtime on your Lab VM. You should see a screen like the following:

  ![The Microsoft Integration Runtime Configuration Manager details](media/adf-ir-config-manager.png 'Microsoft Integration Runtime Configuration Manager')

## Exercise 3: Deploy your machine learning model with Azure ML

Duration: 20 minutes

In this exercise, you will deploy your trained machine learning model to Azure Container Services with the help of Azure Machine Learning Model Management.

### Task 1: Edit the scoring and configuration files

_Tasks to complete_:

- Open the Azure Machine Learning Workbench and then open the Command Prompt.

- Execute the following to update/install the Azure CLI packages:

  ```bash
  pip install azure-cli azure-cli-ml azure-ml-api-sdk
  ```

- Execute the following to upgrade the `pyspark` version:

  ```bash
  pip install pyspark --upgrade
  ```

- After the package installation is complete, execute the following command from the command prompt to launch a local instance of Jupyter notebooks:

  ```bash
  jupyter notebook
  ```

- This command will launch Jupyter in a new web browser. If prompted, select **Firefox** as your default web browser.

  ![Local instance of Jupyter running within the web browser](media/jupyter-in-browser.png)

- Create a new Jupyter notebook with the **Python 3 Spark - local** kernel.

- Create the same methods we will use in our score.py file to test them out locally. Paste the following code into the first cell and execute:

  ```python
  def init():
  # read in the model file
  from pyspark.ml import Pipeline, PipelineModel
  from pyspark.ml.feature import OneHotEncoder, StringIndexer, VectorAssembler, Bucketizer
  global pipeline

  pipeline = PipelineModel.load('pipelineModel')

  def generate_api_schema():
      from azureml.api.schema.dataTypes import DataTypes
      from azureml.api.schema.sampleDefinition import SampleDefinition
      from azureml.api.realtime.services import generate_schema
      import os
      print("create schema")
      sample_input = "{\"OriginAirportCode\":\"SAT\",\"Month\":5,\"DayofMonth\":5,\"CRSDepHour\":13,\"DayOfWeek\":7,\"Carrier\":\"MQ\",\"DestAirportCode\":\"ORD\",\"WindSpeed\":9,\"SeaLevelPressure\":30.03,\"HourlyPrecip\":0}"
      inputs = {"input_df": SampleDefinition(DataTypes.STANDARD, sample_input)}
      os.makedirs('outputs', exist_ok=True)
      print(generate_schema(inputs=inputs, filepath="outputs/schema.json", run_func=run))

  def run(input_df):
      from pyspark.context import SparkContext
      from pyspark.sql.session import SparkSession
      sc = SparkContext.getOrCreate()
      spark = SparkSession(sc)
      response = ''

      try:
          import json
          # Set inferSchema=true to prevent the float values from being seen as strings
          # which can later cause the VectorAssembler to throw an error: 'Data type StringType is not supported.'
          df = spark.read.option("inferSchema", "true").json(sc.parallelize([input_df]))

          #Get prediction results for the dataframe
          score = pipeline.transform(df)
          predictions = score.collect()
          #response = df
          #Get each scored result
          for pred in predictions:
              confidence = str(pred['probability'][0]) if pred['prediction'] == 0 else str(pred['probability'][1])
              response += "{\"prediction\":" + str(pred['prediction']) + ",\"probability\":" +  confidence + "},"
          # Remove the last comma
          response = response[:-1]
      except Exception as e:
          return (str(e))

      # Return results
      return response
  ```

- Paste the following into the second cell:

  ```python
  testInput = "{\"OriginAirportCode\":\"SAT\",\"Month\":5,\"DayofMonth\":5,\"CRSDepHour\":13,\"DayOfWeek\":7,\"Carrier\":\"MQ\",\"DestAirportCode\":\"ORD\",\"WindSpeed\":9,\"SeaLevelPressure\":30.03,\"HourlyPrecip\":0}"
  testInput2 = "{\"OriginAirportCode\":\"ATL\",\"Month\":2,\"DayofMonth\":5,\"CRSDepHour\":8,\"DayOfWeek\":4,\"Carrier\":\"MQ\",\"DestAirportCode\":\"MCO\",\"WindSpeed\":3,\"SeaLevelPressure\":31.03,\"HourlyPrecip\":0}"
  # test init() in local notebook# test  
  init()

  # test run() in local notebook
  run("[" + testInput + "," + testInput2 + "]")
  ```

- Run the cell. This tests the `run()` method, passing in two test parameters. One should return a prediction of 1, and the other 0, though your results may vary. Your output should look similar to the following: `'{"prediction":1.0,"probability":0.560342524075},{"prediction":0.0,"probability":0.69909752}'`. If everything works as expected, then we are ready to modify the score.py file. Save and close this notebook to return to the Jupyter home page.

- Open **score.py**. Replace the file contents with the following:

  ```python
  # This script generates the scoring file
  # necessary to operationalize your model
  from azureml.api.schema.dataTypes import DataTypes
  from pyspark.ml import Pipeline, PipelineModel
  from pyspark.ml.feature import OneHotEncoder, StringIndexer, VectorAssembler
  from pyspark.context import SparkContext
  from pyspark.sql.session import SparkSession
  sc = SparkContext.getOrCreate()
  spark = SparkSession(sc)

  # Prepare the web service definition by authoring
  # init() and run() functions. Test the functions
  # before deploying the web service.

  model = None

  def init():
      # read in the model file
      global pipeline
      pipeline = PipelineModel.load("pipelineModel")

  def run(input_df):
      response = ''

      try:
          import json
          # Set inferSchema=true to prevent the float values from being seen as strings
          # which can later cause the VectorAssembler to throw an error: 'Data type StringType is not supported.'
          df = spark.read.option("inferSchema", "true").json(sc.parallelize([input_df]))

          #Get prediction results for the dataframe
          score = pipeline.transform(df)
          predictions = score.collect()
          #response = df
          #Get each scored result
          for pred in predictions:
              confidence = str(pred['probability'][0]) if pred['prediction'] == 0 else str(pred['probability'][1])
              response += "{\"prediction\":" + str(pred['prediction']) + ",\"probability\":" +  confidence + "},"
          # Remove the last comma
          response = response[:-1]
      except Exception as e:
          return (str(e))

      # Return results
      return response
  ```

- **Save** your changes.

- From the Jupyter home page, open the **aml_config** folder, then open **conda_dependencies.yml**.

  ![Open the aml_config/conda_dependencies.yml file](media/jupyter-conda-dependencies.png 'Jupyter UI with file list')

- Replace the file contents with the following:

  ```yml
  # Conda environment specification. The dependencies defined in this file will
  # be automatically provisioned for managed runs. These include runs against
  # the localdocker, remotedocker, and cluster compute targets.

  # Note that this file is NOT used to automatically manage dependencies for the
  # local compute target. To provision these dependencies locally, run:
  # conda env update --file conda_dependencies.yml

  # Details about the Conda environment file format:
  # https://conda.io/docs/using/envs.html#create-environment-file-by-hand

  # For managing Spark packages and configuration, see spark_dependencies.yml.

  # Version of this configuration file's structure and semantics in AzureML.
  # This directive is stored in a comment to preserve the Conda file structure.
  # [AzureMlVersion] = 2

  name: project_environment
  dependencies:
    # The python interpreter version.
    # Currently Azure ML Workbench only supports 3.5.2.
    - python=3.5.2

    # Required for Jupyter Notebooks.
    - ipykernel=4.6.1

    - pip:
      # Required packages for AzureML execution, history, and data preparation.
      - --index-url https://azuremldownloads.azureedge.net/python-repository/preview
      - --extra-index-url https://pypi.python.org/simple
      - azureml-requirements

      # The API for Azure Machine Learning Model Management Service.
      # Details: https://github.com/Azure/Machine-Learning-Operationalization
      - azure-ml-api-sdk==0.1.0a11
      - azureml.datacollector==0.1.0a13
      - pyspark
  ```

- **Save** your changes. Close conda_dependencies.yml.

_Exit criteria_:

- You are able to successfully test the ML scoring in the new notebook you created.

- The score.py and conda_dependencies.yml files have been updated.

### Task 2: Deploy the model

_Tasks to complete_:

- Open a new Command Prompt from the Azure Machine Learning Workbench and execute the following:

```bash
az ml account modelmanagement set -n <youracctname> -g <yourresourcegroupname>
```

```bash
az ml env set -n <yourclustername> -g <yourresourcegroupname>
```

```bash
az ml service create realtime --model-file pipelineModel -f score.py -n pred -c aml_config\conda_dependencies.yml -r spark-py
```

- **Copy the Scoring URL** to Notepad or similar for later reference.

- Generate your service keys. **Copy the PrimaryKey value** to Notepad or similar for later reference.

_Exit criteria_:

- You are able to successfully test your scoring service with: `az ml service run realtime -i pred.[flightdelays-xyz.location] -d "{\"OriginAirportCode\":\"SAT\",\"Month\":5,\"DayofMonth\":5,\"CRSDepHour\":13,\"DayOfWeek\":7,\"Carrier\":\"MQ\",\"DestAirportCode\":\"ORD\",\"WindSpeed\":9,\"SeaLevelPressure\":30.03,\"HourlyPrecip\":0}"`

- You have recorded your Scoring URL and service key values for later reference.

## Exercise 4: Develop a data factory pipeline for data movement

Duration: 20 minutes

In this exercise, you will create an Azure Data Factory pipeline to copy data (.CSV files) from an on-premises server (Lab VM) to Azure Blob Storage. The goal of the exercise is to demonstrate data movement from an on-premises location to Azure Storage (via the Integration Runtime).

### Task 1: Create copy pipeline using the Copy Data Wizard

_Tasks to complete_:

- Use the Copy Data tool in ADF to generate a Copy Pipeline, moving data from your "on-premises" Lab VM, to the Azure Storage account that was provisioned in the lab setup.

  - The pipeline should run regularly, once per month.

  - Start date is 03/01/2017 12:00 am.

  - For the source:

    - Use a File System source.

    - Choose the path C:\\Data\\FlightsAndWeather.

    - Ensure files copied recursively.

  - For the destination:

    - Use Azure Blob Storage.

    - Make sure you select the storage account you created during lab setup.

    - The folder path should be something like: `sparkcontainer/FlightsAndWeather/{yyyy}/{MM}/`.

    - Add a header to the file.

    - Set the Copy performance settings to have a degree of copy parallelism of 10 and to skip incompatible rows.

    - Update the Copy settings to set the number of retries to 3.

  - Deploy the pipeline.

_Exit criteria_:

- On the **Deployment** screen you will see a message that the deployment in is progress, and after a minute or two that the deployment completed.

  ![Select Edit Pipeline on the bottom of the page](media/adf-copy-data-deployment.png 'Deployment page')

## Exercise 5: Operationalize ML scoring with Azure Databricks and Data Factory

Duration: 20 minutes

In this exercise, you will extend the Data Factory to operationalize the scoring of data using the previously created machine learning model within an Azure Databricks notebook.

### Task 1: Create Azure Databricks Linked Service

_Tasks to complete_:

- Create a new folder named "adf" within the Azure Databricks Workspace folder. Add a new python notebook to that folder named "BatchScore".

- Create a new Azure Databricks Linked Service in ADF by adding a new Notebook activity to the design surface, named BatchScore. Choose existing for the job cluster, selecting the same region as your Azure Databricks workspace.

  - Generate an Azure Databricks user token and use that as your access token for the linked Azure Databricks service.

  - Input your cluster Id, which is found within the Tags tab of your cluster within the Azure Databricks UI.

  - Set the Notebook path to **/adf/BatchScore**.

- Publish your pipeline.

_Exit criteria_:

- You have an Azure Databricks Notebook Activity attached to your Copy Activity, and your pipeline has been published.

  ![Attach the copy activity to the notebook and then publish](media/adf-ml-connect-copy-to-notebook.png 'Attach the copy activity to the notebook')

### Task 2: Complete the BatchScore Azure Databricks notebook code

We need to complete the notebook code for the batch scoring. For simplicity, we will persist the values in a new global persistent Databricks table. In production data workloads, you may save the scored data to Blob Storage, Azure Cosmos DB, or other serving layer. Another implementation detail we are skipping for the lab is processing only new files. This can be accomplished by creating a widget in the notebook that accepts a path parameter that is passed in from Azure Data Factory.

_Tasks to complete_:

- Go back to your Azure Databricks workspace and update your new BatchScore notebook by adding the following cells:

  ```python
  from pyspark.ml import Pipeline, PipelineModel
  from pyspark.ml.feature import OneHotEncoder, StringIndexer, VectorAssembler, Bucketizer
  from pyspark.sql.functions import array, col, lit
  from pyspark.sql.types import *
  ```

- Add a new cell below and paste the following. Replace STORAGE-ACCOUNT-NAME with the name of your storage account. You can find this in the Azure portal by locating the storage account that you created in the lab setup, within your resource group. The container name is set to the default used for this lab. If yours is different, update the containerName variable accordingly.

  ```python
  accountName = "STORAGE-ACCOUNT-NAME"
  containerName = "sparkcontainer"
  ```

- Paste the following into a new cell to define the schema for the CSV files:

  ```python
  data_schema = StructType([
          StructField('OriginAirportCode',StringType()),
          StructField('Month', IntegerType()),
          StructField('DayofMonth', IntegerType()),
          StructField('CRSDepHour', IntegerType()),
          StructField('DayOfWeek', IntegerType()),
          StructField('Carrier', StringType()),
          StructField('DestAirportCode', StringType()),
          StructField('DepDel15', IntegerType()),
          StructField('WindSpeed', DoubleType()),
          StructField('SeaLevelPressure', DoubleType()),  
          StructField('HourlyPrecip', DoubleType())])
  ```

- Paste the following into a new cell to create a new DataFrame from the CSV files, applying the schema:

  ```python
  dfDelays = spark.read.csv("wasbs://" + containerName + "@" + accountName + ".blob.core.windows.net/FlightsAndWeather/*/*/FlightsAndWeather.csv",
                      schema=data_schema,
                      sep=",",
                      header=True)
  ```

- Paste the following into a new cell to load the trained machine learning model you created earlier in the lab, **finishing the TODO item**:

  ```python
  # Load the saved pipeline model
  model = #TODO: Complete this line using PipelineModel.load and pointing it to the DBFS file location where you saved your pipeline model
  ```

- Paste the following into a new cell to make a prediction against the loaded data set, **finishing the TODO item**:

  ```python
  # Make a prediction against the dataset
  prediction = #TODO: Execute a transform from your model, passing in dfDelays
  ```

- Paste the following into a new cell to save the scored data into a new global table, **finishing the TODO item**:

    ```python
    prediction.write.mode("overwrite"). #TODO: Complete this line to save as a global table named "scoredflights"
    ```

_Exit criteria_:

- Your BatchScore notebook is completed with no errors.

### Task 3: Trigger workflow

_Tasks to complete_:

- Within Azure Data Factory, manually trigger your pipeline. Enter **3/1/2017** into the windowStart parameter before you run.

_Exit criteria_:

- When you monitor your pipeline activity, you see a successful run of your pipeline.

![View your pipeline activity](media/adf-ml-monitor.png 'Monitor')

## Exercise 6: Summarize data using Azure Databricks

Duration: 20 minutes

In this exercise, you will prepare a summary of flight delay data using Spark SQL.

### Task 1: Summarize delays by airport

_Tasks to complete_:

- Navigate to your Azure Databricks workspace in the Azure portal.

- Create a new python notebook.

- Save a new global table named flight_delays_summary that represents the following query:

  ```sql
  SELECT  OriginAirportCode, Month, DayofMonth, CRSDepHour, Sum(prediction) NumDelays,
    CONCAT(Latitude, ',', Longitude) OriginLatLong
    FROM scoredflights s
    INNER JOIN airport_code_location_lookup_clean a
    ON s.OriginAirportCode = a.Airport
    WHERE Month = 4
    GROUP BY OriginAirportCode, OriginLatLong, Month, DayofMonth, CRSDepHour
    Having Sum(prediction) > 1
    ORDER BY NumDelays DESC
  ```

_Exit criteria_:

- You have created flight delays summary global table, which can be queried from Power BI Desktop.

## Exercise 7: Visualizing in Power BI Desktop

Duration: 20 minutes

In this exercise, you will create visualizations in Power BI Desktop.

### Task 1: Obtain the JDBC connection string to your Azure Databricks cluster

Before you begin, you must first obtain the JDBC connection string to your Azure Databricks cluster.

_Tasks to complete_:

- In Azure Databricks, go to Clusters and select your cluster.

- On the cluster edit page, scroll down and select the JDBC/ODBC tab.

  ![Select the JDBC/ODBC tab](media/databricks-power-bi-jdbc.png 'JDBC strings')

- On the JDBC/ODBC tab, copy and save the JDBC URL.

  - Construct the JDBC server address that you will use when you set up your Spark cluster connection in Power BI Desktop.

  - Take the JDBC URL that you copied and saved in step 3 and do the following:

  - Replace jdbc:hive2 with https.

  - Remove everything in the path between the port number and sql, retaining the components indicated by the boxes in the image below.

  ![Select the parts to create the Power BI connection string](media/databricks-power-bi-spark-address-construct.png 'Construct Power BI connection string')

_Exit criteria_:

- You have created a JDBC connection string similar to <https://eastus.azuredatabricks.net:443/sql/protocolv1/o/1707858429329790/0614-124738-doubt405> or <https://eastus.azuredatabricks.net:443/sql/protocolv1/o/1707858429329790/lab> (if you choose the aliased version).

### Task 2: Connect to Azure Databricks using Power BI Desktop

_Tasks to complete_:

- Launch Power BI Desktop.

- Connect to your Azure Databricks Spark instance, and query the global tables you created in the previous exercise.

_Exit criteria_:

- You have successfully connected to your Azure Databricks cluster, and have the fields from the flight_delay_summary global table loaded in the report design surface.

### Task 3: Create Power BI report

_Tasks to complete_:

- Generate a Power BI report containing Map, Stacked Column Chart, and Treemap visualizations of the flight delay summary data.

- The Map visualization should represent the number of delays, based on the location of the airport.

- The Stacked Column Chart should provide information about the probability of a delay, based on the day.

- The Treemap visual display details about the number of delays associated with a particular airport.

_Exit criteria_:

- You should have a Power BI report generated, contain three interlinked tiles, displaying flight delay details.

  ![The Report design surface now displays the map of the United States with dots, a stacked bar chart, and a Treeview.](media/pbi-desktop-full-report.png 'Report design surface')

## Exercise 8: Deploy intelligent web app

Duration: 20 minutes

In this exercise, you will deploy an intelligent web application to Azure from GitHub. This application leverages the operationalized machine learning model that was deployed in Exercise 1 to bring action-oriented insight to an already existing business process.

### Task 1: Deploy web app from GitHub

_Tasks to complete_:

- Navigate to the AdventureWorks README page (<https://github.com/Microsoft/MCW-Big-data-and-visualization/blob/master/Hands-on%20lab/lab-files/BigDataTravel/README.md>), and deploy a web app to Azure using an ARM template.

- Provide your ML API details, which was captured during the Machine Learning model deployment exercise.

- Enter your DarkSky API key as part of the deployment process.

_Exit criteria_:

- You are able to successfully navigate to the deployed web app, and test various airport connections to view weather and delay prediction details.

## After the hands-on lab

Duration: 10 minutes

In this exercise, attendees will deprovision any Azure resources that were created in support of the lab.

### Task 1: Delete resource group

1. Using the Azure portal, navigate to the Resource group you used throughout this hands-on lab by selecting **Resource groups** in the left menu.

2. Search for the name of your research group and select it from the list.

3. Select **Delete** in the command bar and confirm the deletion by re-typing the Resource group name and selecting **Delete**.

You should follow all steps provided _after_ attending the Hands-on lab.
