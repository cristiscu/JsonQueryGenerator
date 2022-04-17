Snowflake JSON Query Generator
==============================
A small tool in WinForms C#/.NET that helps you learn how to write and process SQL queries with semi-structured data in Snowflake Data Cloud.

Example Usage
-------------

To connect at Snowflake, save a **SNOWFLAKE_CONNSTR** environment variable with the following format:

**<code>account=your-snowflake-account;user=your-username;password=your-password;</code>**  

Load the solution in Visual Studio (the free Community Edition), update the NuGet packages, compile and run.

![Screen](/images/Snowflake JSON Query Generator.png)

* Switch between different JSON data, with the combobox in the top left corner. Your JSON loaded data comes from the files/ folder. To add more JSON data, add more .json files to this folder, through Visual Studio. Make sure you specify the **Copy always** option, with build action Content!  
* Switch between multiple generated query types, with another top combobox. This will generate standard SQL JSON queries based on your loaded data. The queries have comments with details, and are automatically executed in Snowflake, if there is no error.  
* The Results will show one or more rows. First cell found with a possible JSON object or JSON array is automatically displayed in the bottom-right JSON viewer. Click on another cell to change the selection.  

All data is read-only, and each selection change triggers an automatic query generation and execution.
