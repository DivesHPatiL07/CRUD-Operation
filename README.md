CRUD Operation
To run the project, please follow the steps outlined below:

Step 1: Download the zip file (CRUD-Operation-master.zip).

Step 2: Extract the downloaded file to any location on your system.

Step 3: Locate the database file (CRUDDB.bacpac) within the extracted folder.

Step 4: Import the database file into MSSQL Server Management Studio:
        => Open MSSQL Server Management Studio and right-click on "Databases".
        => Select "Import data-tier application".
        => Click "Next".
        => Choose "Import from local disk" and browse for the CRUDDB.bacpac database file you downloaded.
        => Enter a new database name of your choice.
        => Click "Next" and then "Finish".
If you've followed the above steps correctly, your database will be successfully created.

Step 5: Connect this database to our project:
        => Open our project in Visual Studio IDE.
        => Open the appsettings.json file.
        => Find "DB_Conn_String" and replace it with your own connection string properly.
Once you've completed these steps, the application should run smoothly.

Thank You!






