## `ICursor`

This interface represents the cursor of a SQL query.  It is used to process the associated result set.
```csharp
public interface Absio.Sdk.Interfaces.ICursor
    : IDisposable

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | IsEmpty | Used to check if the cursor is empty.  It is true when there are no records to process (empty query/result set). | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Close() | This is used to close the cursor.  This should be called when the cursor is no longer needed.  This will  free up any tied up resources associated with the query. | 
| `Byte[]` | GetBlob(`Int32` columnIndex) | Used to get a byte array representation of a specific column index. | 
| `MemoryStream` | GetBlobMemoryStream(`Int32` columnIndex) | Used to get a MemoryStream representation of a specific column index. | 
| `Boolean` | GetBoolean(`Int32` columnIndex) | Used to get a bool representation of a specific column index. | 
| `Int32` | GetByteBufferLength() | Used to get the byte buffer length. | 
| `Int32` | GetColumnCount() | Used to get the number of columns included in the query result set. | 
| `Int32` | GetColumnIndex(`String` columnName) | Gets the column index for the specified column name | 
| `String` | GetColumnName(`Int32` columnIndex) | Gets the database defined name of the specified column. | 
| `List<String>` | GetColumnNames() | Gets the database defined names of all the columns. | 
| `DateTime` | GetDateTimeFromUtcTicksToLocalTime(`Int32` columnIndex) | Used to get a DateTime representation of a specific column index.  The time is stored in UTC as ticks but returned in local format. | 
| `Double` | GetDouble(`Int32` columnIndex) | Used to get a double representation of a specific column index. | 
| `Int32` | GetFirstFieldIndex() | Used to identify what base indexing the cursor is operating with. | 
| `Single` | GetFloat(`Int32` columnIndex) | Used to get a float representation of a specific column index. | 
| `Guid` | GetGuid(`Int32` columnIndex) | Used to get a Guid representation of a specific column index. | 
| `Int32` | GetInt(`Int32` columnIndex) | Used to get an int representation of a specific column index. | 
| `Int64` | GetLong(`Int32` columnIndex) | Used to get a long representation of a specific column index. | 
| `Byte[]` | GetNullableBlob(`Int32` columnIndex) | Used to get a byte array representation of a specific column index.  If it is not available/defined  null is returned. | 
| `MemoryStream` | GetNullableBlobMemoryStream(`Int32` columnIndex) | Used to get a MemoryStream representation of a specific column index.  If it is not available/defined  null is returned. | 
| `Nullable<Boolean>` | GetNullableBoolean(`Int32` columnIndex) | Used to get a bool representation of a specific column index.  If it is not available/defined  null is returned. | 
| `Nullable<DateTime>` | GetNullableDateTimeFromUtcTicksToLocalTime(`Int32` columnIndex) | Used to get a DateTime representation of a specific column index.  The time is stored in UTC as ticks but returned in local format.  If it is not available/defined  null is returned. | 
| `Nullable<Double>` | GetNullableDouble(`Int32` columnIndex) | Used to get a double representation of a specific column index.  If it is not available/defined  null is returned. | 
| `Nullable<Single>` | GetNullableFloat(`Int32` columnIndex) | Used to get a float representation of a specific column index.  If it is not available/defined  null is returned. | 
| `Nullable<Int32>` | GetNullableInt(`Int32` columnIndex) | Used to get an int representation of a specific column index.  If it is not available/defined  null is returned. | 
| `Nullable<Int64>` | GetNullableLong(`Int32` columnIndex) | Used to get a long representation of a specific column index.  If it is not available/defined  null is returned. | 
| `Nullable<Int16>` | GetNullableShort(`Int32` columnIndex) | Used to get a short representation of a specific column index.  If it is not available/defined  null is returned. | 
| `String` | GetNullableString(`Int32` columnIndex) | Used to get a string representation of a specific column index.  If it is not available/defined  null is returned. | 
| `Int32` | GetRowPosition() | Used to get the row position of the cursor. | 
| `Int16` | GetShort(`Int32` columnIndex) | Used to get a short representation of a specific column index. | 
| `String` | GetString(`Int32` columnIndex) | Used to get a string representation of a specific column index. | 
| `Task<T>` | InvokeSafeAsync(`Func<T>` func) | Used to safely process the cursor asynchronously. | 
| `Task` | InvokeSafeAsync(`Action` action) | Used to safely process the cursor asynchronously. | 
| `Boolean` | IsClosed() | Used to find out if the cursor has been closed or not. | 
| `Boolean` | IsFirstRow() | Used to find out if the cursor is on the first row of the result set. | 
| `Boolean` | IsNull(`Int32` columnIndex) | Used to find out if the value for the specified column for the current row of the cursor is null or not. | 
| `Boolean` | MoveToNextRow() | Used to move to the next row while processing the cursor.  If the call will return a boolean representing  if there are more records to process or not. | 


## `IDataAccess`

This is the interface of the data access used for the encrypted database in the obfuscated file system (OFS).
```csharp
public interface Absio.Sdk.Interfaces.IDataAccess
    : IDisposable

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | CloseDatabase() | This will close the database. | 
| `void` | DestroyDatabase() | This will destroy the database (including all associated database files). | 
| `Task<Int64>` | ExecuteCountQueryWithParametersAsync(`String` sql, `List<SQLiteParameter>` parameters) | This will execute a query for a row count with the list of parameters. | 
| `Task<ICursor>` | ExecuteQueryWithParametersAsync(`String` sql, `List<SQLiteParameter>` parameters) | This will execute a query with the list of parameters, returning a cursor for processing records. | 
| `Task<Int32>` | ExecuteSqlWithParametersAsync(`String` sql, `List<SQLiteParameter>` parameters) | This will execute some SQL with the list of parameters, returning the number of records affected by the execution. | 
| `Task<DatabaseState>` | OpenOrCreateDatabaseAsync() | This will open database connection.  If the database does not exist, it will make it and then open the connection. | 


## `IRestClient`

```csharp
public interface Absio.Sdk.Interfaces.IRestClient
    : IDisposable

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | ApiKey | The API key assigned to this usage of the AbsioSDK. | 
| `String` | ApplicationName | The name of the application using the AbsioSDK (optional).  This is included in the headers on each request. | 
| `String` | ServerUrl | The host address of the server, eg: http://www.absio.com. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task` | DeleteAsync(`String` endPoint, `Func<HttpHeaders, Task>` customHeaderEntries) | Makes a Delete request to the server for the specified endpoint. | 
| `Task<T>` | DeleteJsonAsync(`String` endPoint, `Func<HttpHeaders, Task>` customHeaderEntries) |  | 
| `Task<Stream>` | GetDataStreamAsync(`String` url) | Makes a Get request to the URL and opens a readable Stream. | 
| `Task<TResult>` | GetJsonAsync(`String` parameters, `Func<HttpHeaders, Task>` customHeaderEntries) |  | 
| `Task<TResult>` | PostJsonAsync(`String` endPoint, `Func<HttpHeaders, Task>` customHeaderEntries, `T` data) |  | 
| `Task` | PutDataAsync(`UrlInfoJson` urlInfo, `Byte[]` data) | Makes a Put request to the server for the specified URL to upload the data. | 
| `Task` | PutJsonAsync(`String` endPoint, `Func<HttpHeaders, Task>` customHeaderEntries, `T` data) |  | 
| `Task<TResult>` | PutJsonAsync(`String` endPoint, `Func<HttpHeaders, Task>` customHeaderEntries, `T` data) |  | 


