Snowflake JSON Query Generator
==============================
A small tool in WinForms C#/.NET that helps you learn how to write and process SQL queries with semi-structured data in Snowflake Data Cloud.

Example Usage
-------------

To connect at Snowflake, save a **SNOWFLAKE_CONNSTR** environment variable with the following format:

**<code>account=your-snowflake-account;user=your-username;password=your-password;</code>**  

Load the solution in Visual Studio (the free Community Edition), update the NuGet packages, compile and run.

![Screen](/images/Snowflake-JSON-Query-Generator.png)

* Switch between different JSON data, with the combobox in the top left corner. Your JSON loaded data comes from the files/ folder. To add more JSON data, add more .json files to this folder, through Visual Studio. Make sure you specify the **Copy always** option, with build action Content!  
* Switch between multiple generated query types, with another top combobox. This will generate standard SQL JSON queries based on your loaded data. The queries have comments with details, and are automatically executed in Snowflake, if there is no error.  
* The Results will show one or more rows. First cell found with a possible JSON object or JSON array is automatically displayed in the bottom-right JSON viewer. Click on another cell to change the selection.  

All data is read-only, and each selection change triggers an automatic query generation and execution.

Generated Query Types
---------------------

* **Single PARSE_JSON** - Generates a simple "SELECT parse_json(...)" query with the JSON loaded content. This is typically used to prepare input JSON data on-the-fly, not stored in the database.  
* **Single OBJECT_CONSTRUCT with PARSE_JSON** - Generates a "SELECT object_construct(...)" query for the top JSON object. All other child objects are created with PARSE_JSON(...) calls. This is to briefly demo the OBJECT_CONSTRUCT and avoid too much clutter.  
* **Multiple OBJECT_CONSTRUCT with ARRAY_CONSTRUCT_COMPACT** - Generates a "SELECT object_construct(...)" query for all JSON objects. Arrays are created with ARRAY_CONSTRUCT_COMPACT calls (no NULLs). This is to demonstrate how to fully create any JSON object from individual elements (properties with values).  
* **Multiple OBJECT_CONSTRUCT_KEEP_NULL with ARRAY_CONSTRUCT** - Generates a "SELECT object_construct_keep_null(...)" query for all JSON objects. Arrays are created with ARRAY_CONSTRUCT calls (keep NULLs). This is to demonstrate how to fully create any JSON object from individual elements (properties with values).  
* **First array** - Returns only the first JSON array from the input data. This is to demo how to address a nested JSON element.  
* **First value array** - Returns only the first JSON array with individual values (not objects) from the input data. This is to demo how to address a nested JSON element.  
* **First array object** - Returns the first JSON array object (if any) from the input data. This is to demo the : notation for nested objects.  
* **First array object property** - Returns the first JSON array object property (if any) from the input data. This is to demo using the dot notation for properties.  
* **LATERAL FLATTEN first array** - Generates a query that will return one row for each individual object of the first JSON array found. This is to demo the LATERAL and FLATTEN constructs.  
* **LATERAL FLATTEN first array + ARRAY_AGG/LISTAGG to compact** - Generates a query that will return one row for each individual object of the first JSON array found, like before. But then uses ARRAY_AGG and LISTAGG to aggregate all these individual rows into a single array. This will demo both expanding (into rows) and collapsing back (rows to content).  
* **LATERAL FLATTEN first two arrays** - Flattens elements of the first two nested arrays. This shows how to call LATERAL FLATTEN twice in the same query.  
* **TABLE FLATTEN first array** - Like LATERAL FLATTEN, but use a TABLE call instead of LATERAL.  
* **TABLE FLATTEN first array nested** - Like LATERAL FLATTEN, but use TABLE when you provide a nested PARSE_JSON call.  
* **TABLE FLATTEN first array RECURSIVE** - Like TABLE FLATTEN for the first array, but with recursive parameter TRUE. This will eventually return more rows, going down into each property value.  
