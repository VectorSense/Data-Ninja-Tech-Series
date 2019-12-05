![](https://github.com/Microsoft/MCW-Template-Cloud-Workshop/raw/master/Media/ms-cloud-workshop.png 'Microsoft Cloud Workshops')

<div class="MCWHeader1">
Big data and visualization
</div>

<div class="MCWHeader2">
Hands-on lab step-by-step
</div>

<div class="MCWHeader3">
November 2018
</div>

Information in this document, including URL and other Internet Web site references, is subject to change without notice. Unless otherwise noted, the example companies, organizations, products, domain names, e-mail addresses, logos, people, places, and events depicted herein are fictitious, and no association with any real company, organization, product, domain name, e-mail address, logo, person, place or event is intended or should be inferred. Complying with all applicable copyright laws is the responsibility of the user. Without limiting the rights under copyright, no part of this document may be reproduced, stored in or introduced into a retrieval system, or transmitted in any form or by any means (electronic, mechanical, photocopying, recording, or otherwise), or for any purpose, without the express written permission of Microsoft Corporation.

Microsoft may have patents, patent applications, trademarks, copyrights, or other intellectual property rights covering subject matter in this document. Except as expressly provided in any written license agreement from Microsoft, the furnishing of this document does not give you any license to these patents, trademarks, copyrights, or other intellectual property.

The names of manufacturers, products, or URLs are provided for informational purposes only and Microsoft makes no representations and warranties, either expressed, implied, or statutory, regarding these manufacturers or the use of the products with any Microsoft technologies. The inclusion of a manufacturer or product does not imply endorsement of Microsoft of the manufacturer or product. Links may be provided to third party sites. Such sites are not under the control of Microsoft and Microsoft is not responsible for the contents of any linked site or any link contained in a linked site, or any changes or updates to such sites. Microsoft is not responsible for webcasting or any other form of transmission received from any linked site. Microsoft is providing these links to you only as a convenience, and the inclusion of any link does not imply endorsement of Microsoft of the site or the products contained therein.

© 2018 Microsoft Corporation. All rights reserved.

Microsoft and the trademarks listed at https://www.microsoft.com/en-us/legal/intellectualproperty/Trademarks/Usage/General.aspx are trademarks of the Microsoft group of companies. All other trademarks are property of their respective owners.

**Contents**

<!-- TOC -->

- [Big data and visualization hands-on lab step-by-step](#big-data-and-visualization-hands-on-lab-step-by-step)
  - [Abstract and learning objectives](#abstract-and-learning-objectives)
  - [Overview](#overview)
  - [Solution architecture](#solution-architecture)
  - [Requirements](#requirements)
  - [Exercise 1: Load Sample Data and Databricks Notebooks](#exercise-1-Load-Sample-Data-and-Databricks-Notebooks)
    - [Task 1: Upload the Sample Datasets](#task-2-upload-the-sample-datasets)
    - [Task 2: Open Azure Databricks and complete lab notebooks](#task-3-open-azure-databricks-and-complete-lab-notebooks)
  - [Exercise 2: Setup Azure Data Factory](#exercise-2-setup-azure-data-factory)
    - [Task 1: Download and stage data to be processed](#task-1-download-and-stage-data-to-be-processed)
    - [Task 2: Install and configure Azure Data Factory Integration Runtime](#task-2-install-and-configure-azure-data-factory-integration-runtime)
    - [Task 3: Configure Azure Data Factory](#task-3-configure-azure-data-factory)
  - [Exercise 3: Develop a data factory pipeline for data movement](#exercise-3-develop-a-data-factory-pipeline-for-data-movement)
    - [Task: Create copy pipeline using the Copy Data Wizard](#task-1-create-copy-pipeline-using-the-copy-data-wizard)
  - [Exercise 4: Operationalize ML scoring with Azure Databricks and Data Factory](#exercise-4-operationalize-ml-scoring-with-azure-databricks-and-data-factory)
    - [Task 1: Create Azure Databricks Linked Service](#task-1-create-azure-databricks-linked-service)
    - [Task 2: Trigger workflow](#task-3-trigger-workflow)
  - [Exercise 5: Summarize data using Azure Databricks](#exercise-5-summarize-data-using-azure-databricks)
    - [Task: Summarize delays by airport](#task-1-summarize-delays-by-airport)
  - [Exercise 6: Visualizing in Power BI Desktop](#exercise-6-visualizing-in-power-bi-desktop)
    - [Task 1: Obtain the JDBC connection string to your Azure Databricks cluster](#task-1-obtain-the-jdbc-connection-string-to-your-azure-databricks-cluster)
    - [Task 2: Connect to Azure Databricks using Power BI Desktop](#task-2-connect-to-azure-databricks-using-power-bi-desktop)
    - [Task 3: Create Power BI report](#task-3-create-power-bi-report)
  - [Exercise 7: Deploy intelligent web app](#exercise-7-deploy-intelligent-web-app-optional-lab)
    - [Task 1: Register for a trial API account at darksky.net](#task-1-register-for-a-trial-api-account-at-darkskynet)
    - [Task 2: Deploy web app from GitHub](#task-2-deploy-web-app-from-github)
  - [After the hands-on lab](#after-the-hands-on-lab)
    - [Task 1: Delete resource group](#task-1-delete-resource-group)

<!-- /TOC -->

# Big data and visualization hands-on lab step-by-step

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

## Exercise 1: Load Sample Data and Databricks Notebooks

Duration: 60 minutes

In this exercise, you will implement a classification experiment. You will load the training data from your local machine into a dataset. Then, you will explore the data to identify the primary components you should use for prediction, and use two different algorithms for predicting the classification. You will then evaluate the performance of both algorithms and choose the algorithm that performs best. The model selected will be exposed as a web service that is integrated with the optional sample web app at the end.

### Task 1: Upload the Sample Datasets

1. Before you begin working with machine learning services, there are three datasets you need to load.

2. Download the three CSV sample datasets from here: <http://bit.ly/2wGAqrl> (If you get an error, or the page won't open, try pasting the URL into a new browser window and verify the case sensitive URL is exactly as shown). If still having trouble, a zip file called AdventureWorksTravelDatasets.zip is included in the lab-files folders.

3. Extract the ZIP and verify you have the following files:

    - FlightDelaysWithAirportCodes.csv
    - FlightWeatherWithAirportCodes.csv
    - AirportCodeLocationLookupClean.csv

4. Open a browser and navigate to the Azure portal (<https://portal.azure.com>), and navigate to Azure Databricks service under the Resource Group you created when completing the prerequisites for this hands-on lab.

    ![Select the Azure Databricks service from within your lab resource group](media/select-azure-databricks-service.png 'Select Azure Databricks')

5. In the Overview pane of the Azure Databricks service, select **Launch Workspace**.

    ![Select Launch Workspace within the Azure Databricks service overview pane](media/azure-databricks-launch-workspace.png 'Select Launch Workspace')

    Azure Databricks will automatically log you in using Azure Active Directory Single Sign On.

    ![Azure Databricks Azure Active Directory Single Sign On](media/azure-databricks-aad.png 'AAD Single Sign On')

6. Once signed in, select **Data** from the menu. Next, select **default** under Databases (if this does not appear, start your cluster). Finally, select **Add Data** above the Tables header.

    ![From the Azure Databricks workspace, select Data, default database, then new table](media/azure-databricks-create-tables.png 'Create new table')

7. Select **Upload File** under Create New Table, and then select either select or drag-and-drop the FlightDelaysWithAirportCodes.csv file into the file area. Select **Create Table with UI**.

    ![Create a new table using the FlightDelaysWithAirportCodes.csv file](<media/![](media/create-flight-delays-table-ui.png).png> 'Create new table')

8. Select your cluster to preview the table, then select **Preview Table**.

9. Change the Table Name to \"flight_delays_with_airport_codes\" and select the checkmark for **First row is header**. Select **Create Table**.

    ![Rename table and check the first row is header checkbox](media/flight-delays-attributes.png 'Rename table')

10. Repeat the previous steps for the FlightWeatherWithAirportCode.csv and AirportCodeLocationsClean.csv files, setting the name for each dataset in a similar fashion. There should be a total of three files that are uploaded. Each table should be named as follows:

    - flightweatherwithairportcode_csv to **flight_weather_with_airport_code**
    - flightdelayswithairportcodes_csv to **flight_delays_with_airport_codes**
    - airportcodelocationlookupclean_csv to **airport_code_location_lookup_clean**

    ![Azure Databricks tables shown after all three files uploaded](media/uploaded-data-files.png 'Uploaded data files')

### Task 2: Open Azure Databricks and complete lab notebooks

1. Download the following file:

    - [BigDataVis.dbc](lab-files/BigDataVis.dbc)

2. Within Azure Databricks, select **Workspace** on the menu, then **Users**, select your user, then select the down arrow on the top of your user workspace. Select **Import**.

    ![Screenshot showing selecting import within the user workspace](media/select-import-in-user-workspace.png 'Import')

3. Within the Import Notebooks dialog, select Import from: file, then drag-and-drop the file or browse to upload it.

    ![Select import from file](media/import-notebooks.png 'Import from file')

4. Select **BigDataVis** to open the notebook.

5. Before you begin, make sure you attach your cluster to the notebooks, using the dropdown. You will need to do this for each notebook you open. There are 5 notebooks included in the BigDataVis.dbc

    ![Select your cluster to attach it to the notebook](media/attach-cluster-to-notebook.png 'Attach cluster to notebook')

6. Run each cell of the notebooks 01, 02 and 03 individually by selecting within the cell, then entering **Ctrl+Enter** on your keyboard. Pay close attention to the instructions within the notebook so you understand each step of the data preparation process.

7. Do NOT run the `Clean up` part of Notebook 3 (i.e. this command: `webservice.delete()`). You will need the URL of your Machine Learning Model exposed later in **Exercise 7: Deploy intelligent web app (Optional Lab)**. *Note: you could get this URL by updating your Notebook by adding this line `print(webservice.scoring_uri)` or by going to your Azure Machine Learning service workspace via the Azure portal and then to the "Deployments" blade.*

8. Do NOT run Notebooks 4 and 5 yet, they will be discussed later in the lab.


## Exercise 2: Setup Azure Data Factory

Duration: 20 minutes

In this exercise, you will create a baseline environment for Azure Data Factory development for further operationalization of data movement and processing. You will create a Data Factory service, and then install the Data Management Gateway which is the agent that facilitates data movement from on-premises to Microsoft Azure.

### Task 1: Download and stage data to be processed

1. Open a web browser.

2. Download the AdventureWorks sample data from <http://bit.ly/2zi4Sqa>.

3. Extract it to a new folder called **C:\\Data**.

### Task 2: Install and configure Azure Data Factory Integration Runtime on your machine 

1. To download the latest version of Azure Data Factory Integration Runtime, go to <https://www.microsoft.com/en-us/download/details.aspx?id=39717>.

    ![The Azure Data Factory Integration Runtime Download webpage displays.](media/image112.png 'Azure Data Factory Integration Runtime Download webpage')

2. Select Download, then choose the download you want from the next screen.

    ![Under Choose the download you want, IntegrationRuntime_3.0.6464.2 (64-bit).msi is selected.](media/image113.png 'Choose the download you want section')

3. Run the installer, once downloaded.

4. When you see the following screen, select Next.

    ![The Welcome page in the Microsoft Integration Runtime Setup Wizard displays.](media/image114.png 'Microsoft Integration Runtime Setup Wizard')

5. Check the box to accept the terms and select Next.

    ![On the End-User License Agreement page, the check box to accept the license agreement is selected, as is the Next button.](media/image115.png 'End-User License Agreement page')

6. Accept the default Destination Folder, and select Next.

    ![On the Destination folder page, the destination folder is set to C;\Program Files\Microsoft Integration Runtime\ and the Next button is selected.](media/image116.png 'Destination folder page')

7. Select Install to complete the installation.

    ![On the Ready to install Microsoft Integration Runtime page, the Install button is selected.](media/image117.png 'Ready to install page')

8. Select Finish once the installation has completed.

    ![On the Completed the Microsoft Integration Runtime Setup Wizard page, the Finish button is selected.](media/image118.png 'Completed the Wizard page')

9. After selecting Finish, the following screen will appear. Keep it open for now. You will come back to this screen once the Data Factory in Azure has been provisioned, and obtain the gateway key so we can connect Data Factory to this "on-premises" server.

    ![The Microsoft Integration Runtime Configuration Manager, Register Integration Runtime page displays.](media/image119.png 'Register Integration Runtime page')

### Task 3: Configure Azure Data Factory

1. Launch a new browser window, and navigate to the Azure portal (<https://portal.azure.com>). Once prompted, log in with your Microsoft Azure credentials. If prompted, choose whether your account is an organization account or a Microsoft account. This will be based on which account was used to provision your Azure subscription that is being used for this lab.

2. From the side menu in the Azure portal, choose **Resource groups**, then enter your resource group name into the filter box, and select it from the list.

3. Next, select your Azure Data Factory service from the list.

4. On the Data Factory blade, select **Author & Monitor** under Actions.

    ![In the Azure Data Factory blade, under Actions, the Author & Monitor option is selected.](media/adf-author-monitor.png 'Author & Monitor')

5. A new page will open in another tab or new window. Within the Azure Data Factory site, select **Author** (the pencil icon) on the menu.

    ![Select Author from the menu](media/adf-home-author-link.png 'Author link on ADF home page')

6. Now, select **Connections** at the bottom of Factory Resources (1), then select the **Integration Runtimes** tab (2), and finally select **+ New** (3).

    ![Select Connections at the bottom of the page, then select the Integration Runtimes tab, and select New.](media/adf-new-ir.png 'Steps to create a new Integation Runtime connection')

7. In the Integration Runtime Setup blade that appears, select "Perform data movement and dispatch activities to external computes", then select **Next**.

    ![Select Perform data movement and dispatch activities to external computes](media/adf-ir-setup-1.png 'Integration Runtime Setup step 1')

8. Select **Self-Hosted** then select **Next**.

    ![Select Private Network then Next](media/adf-ir-setup-2.png 'Integration Runtime Setup step 2')

9. Enter a **Name**, such as bigdatagateway-\[initials\], and select **Next**.

    ![Enter a Name and select Next](media/adf-ir-setup-3.png 'Integration Runtime Setup step 3')

10. Under Option 2: Manual setup, copy the Key1 authentication key value by selecting the Copy button, then select **Finish**.


    ![Copy the Key1 value](media/adf-ir-setup-4.png 'Integration Runtime Setup step 4')

11. _Don't close the current screen or browser session_.

12. Paste the **Key1** value into the box in the middle of the Microsoft Integration Runtime Configuration Manager screen.

    ![The Microsoft Integration Runtime Configuration Manager Register Integration Runtime page displays.](media/image127.png 'Microsoft Integration Runtime Configuration Manager')

13. Select **Register**.

14. It will take a minute or two to register. If it takes more than a couple of minutes, and the screen does not respond or returns an error message, close the screen by selecting the **Cancel** button.

15. The next screen will be New Integration Runtime (Self-hosted) Node. Select Finish.

    ![The Microsoft Integration Runtime Configuration Manager New Integration Runtime (Self-hosted) Node page displays.](media/adf-ir-self-hosted-node.png 'Microsoft Integration Runtime Configuration Manager')

16. You will then get a screen with a confirmation message. Select the **Launch Configuration Manager** button to view the connection details.

    ![The Microsoft Integration Runtime Configuration Manager Node is connected to the cloud service page displays with connection details.](media/adf-ir-launch-config-manager.png 'Microsoft Integration Runtime Configuration Manager')

    ![The Microsoft Integration Runtime Configuration Manager details](media/adf-ir-config-manager.png 'Microsoft Integration Runtime Configuration Manager')

17. You can now return to the Azure portal, and view the Integration Runtime you just configured.

    ![You can view your Integration Runtime you just configured](media/adf-ir-running.png 'Integration Runtime in running state')

18. Select the Azure Data Factory Overview button on the menu. Leave this open for the next exercise.

    ![Select the Azure Data Factory Overview button on the menu](media/adf-overview.png 'ADF Overview')

## Exercise 3: Develop a data factory pipeline for data movement

Duration: 20 minutes

In this exercise, you will create an Azure Data Factory pipeline to copy data (.CSV files) from an on-premises server (your machine) to Azure Blob Storage. The goal of the exercise is to demonstrate data movement from an on-premises location to Azure Storage (via the Integration Runtime).

### Task 1: Create copy pipeline using the Copy Data Wizard

1. Within the Azure Data Factory overview page, select **Copy Data**.

    ![Select Copy Data from the overview page](media/adf-copy-data-link.png 'Copy Data')

2. In the Copy Data properties, enter the following:

    - Task name: **CopyOnPrem2AzurePipeline**

    - Task description: (Optional) **"This pipeline copies timesliced CSV files from on-premises C:\\Data to Azure Blob Storage as a continuous job"**.

    - Task cadence or Task schedule: **Select Run regularly on schedule**

    - Trigger type: **Select Schedule**

    - Start date time (UTC): **03/01/2018 12:00 am**

    - Recurrence: **Select Monthly, and every 1 month**
    - Under the Advanced recurrence options, make sure you have a value in the textboxes for **Hours (UTC)** and **Minutes (UTC)** otherwise it will fail later during Publishing.

    - End: **No End**

    ![Set the ADF pipeline copy activity properties by setting the Task Name to CopyOnPrem2AzurePipeline, adding a description, setting the Task cadence to Run regularly on a Monthly schedule, every 1 month.](media/adf-copy-data-properties.png 'Properties dialog box')

3. Select **Next**.

4. On the Source data store screen, select **+ Create new connection**.

5. Scroll through the options and select **File System**, then select **Continue**.

    ![Select File System, then Continue](media/adf-copy-data-new-linked-service.png 'Select File System')

6. In the New Linked Service form, enter the following:

    - Name: **OnPremServer**

    - Connect via integration runtime: **Select the Integration runtime created previously in this exercise**.

    - Host: **C:\\Data**

    - User name: **Use your machine's login username**

    - Password: **Use your machine's login password**

7. Select **Test connection** to verify you correctly entered the values. Finally, select **Finish**.

    ![On the Copy Data activity, specify File server share connection page, fields are set to the previously defined values.](media/adf-copy-data-linked-service-settings.png 'New Linked Service settings')

8. On the Source data store page, select **Next**.

    ![Select Next](media/adf-copy-data-source-next.png 'Select Next')

9. On the **Choose the input file or folder** screen, select **Browse**, then select the **FlightsAndWeather** folder. Next, check **Copy file recursively**, then select **Next**.

    ![In the Choose the input file or folder section, the FlightsandWeather folder is selected.](media/adf-copy-data-source-choose-input.png 'Choose the input file or folder page')

10. On the File format settings page, select the following options:

    - File format: **Text format**

    - Column delimiter: **Comma (,)**

    - Row delimiter: **Carriage Return + Line feed (\r\n)**

    - Skip line count: **0**

    - Source files contain column names in the first row: **Checked**

    - Treat empty column value as null: **Checked**

    ![Enter the form values](media/adf-copy-data-file-format.png 'File format settings')

11. Select **Next**.

12. On the Destination screen, select **+ Create new connection**.

13. Select **Azure Blob Storage** within the New Linked Service blade, then select **Continue**.

    ![Select Azure Blob Storage, then Continue](media/adf-copy-data-blob-storage.png 'Select Blob Storage')

14. On the New Lined Service (Azure Blob Storage) account screen, enter the following and then choose **Finish**.

    - Name: **BlobStorageOutput**

    - Connect via integration runtime: **Select your Integration Runtime**.

    - Azure selection method: **From Azure subscription**

    - Storage account name: **Select the blob storage account you provisioned in the before-the-lab section**.

    ![On the Copy Data New Linked Service Azure Blob storage account page, fields are set to the previously defined settings.](media/adf-copy-data-blob-storage-linked.png 'New Linked Service Blob Storage')

15. On the Destination data store page, select **Next**.

16. From the **Choose the output file or folder** tab, enter the following:

    - Folder path: **sparkcontainer/FlightsAndWeather/{Year}/{Month}/**

    - Filename: **FlightsAndWeather.csv**

    - Year: Select **yyyy** from the drop down.

    - Month: Select **MM** from the drop down.

    - Copy behavior: **Merge files**

    - Select **Next**

      ![On the Copy Data Choose the output file or folder page, fields are set to the previously defined settings.](media/adf-copy-data-output-file-folder.png 'Choose the output file or folder page')

17. On the File format settings screen, select the **Text format** file format, and check the **Add header to file** checkbox, then select **Next**.

    ![On the Copy Data File format settings page, the check box for Add header to file is selected.](media/adf-copy-data-file-format-settings.png 'File format settings page')

18. On the **Settings** screen, select **Skip incompatible rows** under Actions. Expand Advanced settings and set Degree of copy parallelism to **10**, then select **Next**.

    ![Select Skip incompatible rows and set copy parallelism to 10](media/adf-copy-data-settings.png 'Settings page')

19. Review settings on the **Summary** tab, but **DO NOT choose Next**.

    ![Summary page](media/adf-copy-data-summary.png 'Summary page')

20. Scroll down on the summary page until you see the **Copy Settings** section. Select **Edit** next to **Copy Settings**.

    ![Scroll down and select Edit within Copy Settings](media/adf-copy-data-review-page.png 'Summary page')

21. Change the following Copy settings:

    - Retry: Set to **3**.

    - Select **Save**.

      ![Set retry to 3](media/adf-copy-data-copy-settings.png 'Copy settings')

22. After saving the Copy settings, select **Next** on the Summary tab.

23. On the **Deployment** screen you will see a message that the deployment in is progress, and after a minute or two that the deployment completed. Select **Edit Pipeline** to close out of the wizard.

    ![Select Edit Pipeline on the bottom of the page](media/adf-copy-data-deployment.png 'Deployment page')

## Exercise 4: Operationalize ML scoring with Azure Databricks and Data Factory

Duration: 20 minutes

In this exercise, you will extend the Data Factory to operationalize the scoring of data using the previously created machine learning model within an Azure Databricks notebook.

### Task 1: Create Azure Databricks Linked Service

1. Return to, or reopen, the Author & Monitor page for your Azure Data Factory in a web browser, navigate to the Author view, and select the pipeline.

    ![Select the ADF pipeline created in the previous exercise](media/adf-ml-select-pipeline.png 'Select the ADF pipeline')

2. Once there, expand Databricks under Activities.

    ![Expand the Databricks activity after selecting your pipeline](media/adf-ml-expand-databricks-activity.png 'Expand Databricks Activity')

3. Drag the Notebook activity onto the design surface to the side of the Copy activity.

    ![Drag the Notebook onto the design surface](media/adf-ml-drag-notebook-activity.png 'Notebook on design surface')

4. Select the Notebook activity on the design surface to display tabs containing its properties and settings at the bottom of the screen. On the **General** tab, enter "BatchScore" into the Name field.

    ![Type BatchScore as the Name under the General tab](media/adf-ml-notebook-general.png 'Databricks Notebook General Tab')

5. Select the **Azure Databricks** tab, and select **+ New** next to the Databricks Linked service drop down. Here, you will configure a new linked service which will serve as the connection to your Databricks cluster.

    ![Screenshot of the Settings tab](media/adf-ml-settings-new-link.png 'Databricks Notebook Settings Tab')

6. On the New Linked Service dialog, enter the following:

    - Name: enter a name, such as AzureDatabricks.
    - Connect via integration runtime: Leave set to Default.
    - Account selection method: Select From Azure subscription.
    - Choose your Azure Subscription.
    - Pick your Databricks workspace to populate the Domain automatically.
    - Select cluster: choose Existing cluster.

    ![Screenshot showing filled out form with defined parameters](media/adf-ml-databricks-service-settings.png 'Databricks Linked Service settings')

7. Leave the form open and open your Azure Databricks workspace in another browser tab. You will retrieve the Access token and cluster id here.

8. In Azure Databricks, select the Account icon in the top corner of the window, then select **User Settings**.

    ![Select account icon, then user settings](media/databricks-select-user-settings.png 'Azure Databricks user account settings')

9. Select **Generate New Token** under the Access Tokens tab. Enter **ADF access** for the comment and leave the lifetime at 90 days. Select **Generate**.

    ![Generate a new token](media/databricks-generate-new-token.png 'Generate New Token')

10. **Copy** the generated token.

    ![Copy the generated token](media/databricks-copy-token.png 'Copy generated token')

11. Switch back to your Azure Data Factory screen and paste the generated token into the **Access token** field within the form.

    ![Paste the generated access token](media/adf-ml-access-token.png 'Paste access token')

12. Leave the form open and switch back to Azure Databricks. Select **Clusters** on the menu, then select your cluster in the list. Select the **Tags** tab and copy the **ClusterId** value.

    ![Screenshot of the cluster tags tab](media/databricks-cluster-id.png 'Copy the ClusterId value')

13. Switch back to your Azure Data Factory screen and paste the ClusterId value into the **Existing cluster id** field. Select **Finish**.

    ![Paste the cluster id and select finish](media/adf-ml-databricks-clusterid.png 'Paste cluster id')

14. Switch back to Azure Databricks. Select **Workspace** in the menu. Open notebook **04 Deploy for Batch Scoring**. Examine the content but don't run any of the cells yet. You need to replace `STORAGE-ACCOUNT-NAME` with the name of the blob storage account you provisioned in the before-the-lab section.

    ![Right-click within workspace and select 04 Deploy for Batch Score](media/databricks-workspace-create-folder.png 'Create folder')

15. Switch back to your Azure Data Factory screen. Browse to your  **04 Deploy for Batch Score** into the Notebook path field.

    ![browse to 04 Deploy for Batch score into the notebook path](media/adf-ml-notebook-path.png 'Notebook path')

19. The final step is to connect the Copy activities with the Notebook activity. Select the small green box on the side of the copy activity, and drag the arrow onto the Notebook activity on the design surface. What this means is that the copy activity has to complete processing and generate its files in your storage account before the Notebook activity runs, ensuring the files required by the BatchScore notebook are in place at the time of execution. Select **Publish All** after making the connection.

    ![Attach the copy activity to the notebook and then publish](media/adf-ml-connect-copy-to-notebook.png 'Attach the copy activity to the notebook')

### Task 2: Trigger workflow

1. Switch back to Azure Data Factory. Select your pipeline if it is not already opened.

2. Select **Trigger**, then **Trigger Now** located above the pipeline design surface.

    ![Manually trigger the pipeline](media/adf-ml-trigger-now.png 'Trigger Now')

3. Enter **3/1/2017** into the windowStart parameter, then select **Finish**.

    ![Screenshot showing the Pipeline Run form](media/adf-ml-pipeline-run.png 'Pipeline Run')

4. Select **Monitor** in the menu. You will be able to see your pipeline activity in progress as well as the status of past runs.

    ![View your pipeline activity](media/adf-ml-monitor.png 'Monitor')

## Exercise 5: Summarize data using Azure Databricks

Duration: 20 minutes

In this exercise, you will prepare a summary of flight delay data using Spark SQL.

### Task: Summarize delays by airport

1. Open your Azure Databricks workspace and open the final notebook called **05 explore Data**
2. Execute each cell and follow the instructions in the notebook that explains each step.

## Exercise 6: Visualizing in Power BI Desktop

Duration: 20 minutes

In this exercise, you will create visualizations in Power BI Desktop.

### Task 1: Obtain the JDBC connection string to your Azure Databricks cluster

Before you begin, you must first obtain the JDBC connection string to your Azure Databricks cluster.

1. In Azure Databricks, go to Clusters and select your cluster.

2. On the cluster edit page, scroll down and select the JDBC/ODBC tab.

    ![Select the JDBC/ODBC tab](media/databricks-power-bi-jdbc.png 'JDBC strings')

3. On the JDBC/ODBC tab, copy and save the JDBC URL.

    - Construct the JDBC server address that you will use when you set up your Spark cluster connection in Power BI Desktop.

    - Take the JDBC URL that you copied and saved in step 3 and do the following:

    - Replace jdbc:hive2 with https.

    - Remove everything in the path between the port number and sql, retaining the components indicated by the boxes in the image below.

    ![Select the parts to create the Power BI connection string](media/databricks-power-bi-spark-address-construct.png 'Construct Power BI connection string')

    - In our example, the server address would be:

    <https://eastus.azuredatabricks.net:443/sql/protocolv1/o/1707858429329790/0614-124738-doubt405> or <https://eastus.azuredatabricks.net:443/sql/protocolv1/o/1707858429329790/lab> (if you choose the aliased version)

### Task 2: Connect to Azure Databricks using Power BI Desktop

1. Download Power BI Desktop from https://powerbi.microsoft.com/en-us/desktop/

2. When Power BI Desktop starts, you will need to enter your personal information, or Sign in if you already have an account.

    ![The Power BI Desktop Welcome page displays.](media/image177.png 'Power BI Desktop Welcome page')

3. Select Get data on the screen that is displayed next.
    ![On the Power BI Desktop Sign in page, in the pane, Get data is selected.](media/image178.png 'Power BI Desktop Sign in page')

4. Select **Other** from the side, and select **Spark** from the list of available data sources.

    ![In the pane of the Get Data page, Other is selected. In the pane, Spark is selected.](media/pbi-desktop-get-data.png 'Get Data page')

5. Select **Connect**.

6. On the next screen, you will be prompted for your Spark cluster information.

7. Paste the JDBC connection string you constructed a few steps ago into the **Server** field.

8. Select the **HTTP** protocol.

9. Select **DirectQuery** for the Data Connectivity mode, and select **OK**. This option will offload query tasks to the Azure Databricks Spark cluster, providing near-real time querying.

    ![Configure your connection to the Spark cluster](media/pbi-desktop-connect-spark.png 'Spark form')

10. Enter your credentials on the next screen as follows:

    - User name: **token**

    - Password: Remember that ADF Access token we generated and asked you to paste in Notepad, that is the password.

     ![Copy the generated token](media/databricks-copy-token.png 'Copy generated token')

    ![Enter "token" for the user name and paste user token into the password field](media/pbi-desktop-login.png 'Enter credentials')

11. Select **Connect**.

12. In the Navigator dialog, check the box next to **flight_delays_summary**, and select **Load**.

    ![In the Navigator dialog box, in the pane under Display Options, the check box for flight_delays_summary is selected. In the pane, the table of flight delays summary information displays.](media/pbi-desktop-select-table-navigator.png 'Navigator dialog box')

13. It will take several minutes for the data to load into the Power BI Desktop client.

### Task 3: Create Power BI report

1. Once the data finishes loading, you will see the fields appear on the far side of the Power BI Desktop client window.

    ![Power BI Desktop fields](media/pbi-desktop-fields.png 'Power BI Desktop Fields')

2. From the Visualizations area, next to Fields, select the Globe icon to add a Map visualization to the report design surface.

    ![On the Power BI Desktop Visualizations palette, the globe icon is selected.](media/image187.png 'Power BI Desktop Visualizatoins palette')

3. With the Map visualization still selected, drag the **OriginLatLong** field to the **Location** field under Visualizations. Then Next, drag the **NumDelays** field to the **Size** field under Visualizations.

    ![In the Fields column, the check boxes for NumDelays and OriginLatLong are selected. An arrow points from OriginLatLong in the Fields column, to OriginLatLong in the Visualization's Location field. A second arrow points from NumDelays in the Fields column, to NumDelays in the Visualization's Size field.](media/pbi-desktop-configure-map-vis.png 'Visualizations and Fields columns')

4. You should now see a map that looks similar to the following (resize and zoom on your map if necessary):

    ![On the Report design surface, a Map of the United States displays with varying-sized dots over different cities.](media/pbi-desktop-map-vis.png 'Report design surface')

5. Unselect the Map visualization by selecting the white space next to the map in the report area.

6. From the Visualizations area, select the **Stacked Column Chart** icon to add a bar chart visual to the report's design surface.

    ![The stacked column chart icon is selected on the Visualizations palette.](media/image190.png 'Visualizations palette')

7. With the Stacked Column Chart still selected, drag the **DayofMonth** field and drop it into the **Axis** field located under Visualizations.

8. Next, drag the **NumDelays** field over, and drop it into the **Value** field.

    ![In the Fields column, the check boxes for NumDelays and DayofMonth are selected. An arrow points from NumDelays in the Fields column, to NumDelays in the Visualization's Axis field. A second arrow points from DayofMonth in the Fields column, to DayofMonth in the Visualization's Value field.](media/pbi-desktop-configure-stacked-vis.png 'Visualizations and Fields columns')

9. Grab the corner of the new Stacked Column Chart visual on the report design surface, and drag it out to make it as wide as the bottom of your report design surface. It should look something like the following.

    ![On the Report Design Surface, under the map of the United States with dots, a stacked bar chart displays.](media/pbi-desktop-stacked-vis.png 'Report Design Surface')

10. Unselect the Stacked Column Chart visual by selecting on the white space next to the map on the design surface.

11. From the Visualizations area, select the Treemap icon to add this visualization to the report.

    ![On the Visualizations palette, the Treemap icon is selected.](media/image193.png 'Visualizations palette')

12. With the Treemap visualization selected, drag the **OriginAirportCode** field into the **Group** field under Visualizations.

13. Next, drag the **NumDelays** field over, and drop it into the **Values** field.

    ![In the Fields column, the check boxes for NumDelays and OriginAirportcode are selected. An arrow points from NumDelays in the Fields column, to NumDelays in the Visualization's Values field. A second arrow points from OriginAirportcode in the Fields column, to OriginAirportcode in the Visualization's Group field.](media/pbi-desktop-config-treemap-vis.png 'Visualizations and Fields columns')

14. Grab the corner of the Treemap visual on the report design surface, and expand it to fill the area between the map and the side edge of the design surface. The report should now look similar to the following.

    ![The Report design surface now displays the map of the United States with dots, a stacked bar chart, and a Treeview.](media/pbi-desktop-full-report.png 'Report design surface')

15. You can cross filter any of the visualizations on the report by selecting one of the other visuals within the report, as shown below (This may take a few seconds to change, as the data is loaded).

    ![The map on the Report design surface is now zoomed in on the northeast section of the United States, and the only dot on the map is on Chicago. In the Treeview, all cities except ORD are grayed out. In the stacked bar graph, each bar is now divided into a darker and a lighter color, with the darker color representing the airport.](media/pbi-desktop-full-report-filter.png 'Report design surface')

16. You can save the report, by choosing Save from the File menu, and entering a name and location for the file.

    ![The Power BI Save as window displays.](media/image197.png 'Power BI Save as window')

## Exercise 7: Deploy intelligent web app (Optional Lab)

Duration: 20 minutes

In this exercise, you will deploy an intelligent web application to Azure from GitHub. This application leverages the operationalized machine learning model that was deployed in Exercise 1 to bring action-oriented insight to an already existing business process.

### Task 1: Register for a trial API account at darksky.net

To retrieve the 7-day hourly weather forecast, you will use an API from darksky.net. There is a free trial version that provides you access to the API you need for this hands-on lab.

1. Navigate to <https://darksky.net/dev>.

2. Select TRY FOR FREE.

    ![Select the TRY FOR FREE button on the Dark Sky dev page](media/dark-sky-api-try-for-free.png)

3. Complete the Register form by providing your email address and a password. Select REGISTER.

    ![Complete the registration form and select REGISTER](media/dark-sky-register.png)

4. Check your email account you used for registration. You should have a confirmation email from Dark Sky. Open the email and follow the confirmation link within to complete the registration process. When the welcome page loads, log in with your new account.

    ![Dark Sky welcome page. Choose login to continue](media/dark-sky-welcome.png)

5. After logging in, you will be directed to the Your Account page. Take note of your **Secret Key** and copy it to a text editor such as Notepad for later. You will need this key to make API calls later in the lab.

    ![The Dark Sky Your Account page - copy the Secret Key](media/dark-sky-your-account.png)

6. To verify that your API Key is working, follow the link on the bottom of the page located underneath Sample API Call. You should see a JSON result that looks similar to the following:

    ![Sample JSON result from Dark Sky API link](media/dark-sky-sample-json-result.png)

### Task 2: Deploy web app from GitHub

1. Navigate to <https://github.com/Microsoft/MCW-Big-data-and-visualization/blob/master/Hands-on%20lab/lab-files/BigDataTravel/README.md> in your browser of choice, but where you are already authenticated to the Azure portal.

2. Read through the README information on the GitHub page.

3. Select **Deploy to Azure**.

    ![Screenshot of the Deploy to Azure button.](media/deploy-to-azure-button.png 'Deploy to Azure button')

4. On the following page, ensure the fields are populated correctly.

    - Ensure the correct Directory and Subscription are selected.

    - Select the Resource Group that you have been using throughout this lab.

    - Either keep the default Site name, or provide one that is globally unique, and then choose a Site Location.

    - Enter Weather API information
    
    - Finally, enter the ML URL. We got that from Azure databricks Notebook #3, remember? If you cleaned your resources at the end of this Notebook #3, you will need to re-run it and keep that web service running to get its associated URL.

    ![Fields on the Deploy to Azure page are populated with the previously copied information.](media/azure-deployment-form.png 'Deploy to Azure page')

5. Select **Next**, and on the following screen, select **Deploy**.

6. The page should begin deploying your application while showing you a status of what is currently happening.

    > Note: If you run into errors during the deployment that indicate a bad request or unauthorized, verify that the user you are logged into the portal with an account that is either a Service Administrator or a Co-Administrator. You won't have permissions to deploy the website otherwise.

7. After a short time, the deployment will complete, and you will be presented with a link to your newly deployed web application. CTRL+Click to open it in a new tab.

8. Try a few different combinations of origin, destination, date, and time in the application. The information you are shown is the result of both the ML API you published, as well as information retrieved from the DarkSky API.

9. Congratulations! You have built and deployed an intelligent system to Azure.

## After the hands-on lab

Duration: 10 minutes

In this exercise, attendees will deprovision any Azure resources that were created in support of the lab.

### Task 1: Delete resource group

1. Using the Azure portal, navigate to the Resource group you used throughout this hands-on lab by selecting **Resource groups** in the menu.

2. Search for the name of your research group and select it from the list.

3. Select **Delete** in the command bar and confirm the deletion by re-typing the Resource group name and selecting **Delete**.

You should follow all steps provided _after_ attending the Hands-on lab.
