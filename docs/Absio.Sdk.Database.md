## `ContainerDbInfo`

Represents the IContainer information stored in the OFS's encrypted database.
```csharp
public class Absio.Sdk.Database.ContainerDbInfo
    : IEquatable<ContainerDbInfo>

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `ContainerMetadata` | Metadata | The metadata for the container. | 
| `String` | OfsLocation | The location within the OFS where the encryted data is stored. | 
| `Nullable<DateTime>` | SyncedAt | The time the data was last synced.  Used by ServerCacheOfsProvider to track when a contanier was updated in cache | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`ContainerDbInfo` other) | Compares this instance with the other instance for equality based on the properties. | 
| `Boolean` | Equals(`Object` obj) | Compares this instance with the other instance for equality based on the properties. | 
| `Int32` | GetHashCode() |  | 


## `DatabaseState`

This is returned from the OpenOrCreateDatabaseAsync call on IDataAccess.  The enumeration represents what was done by the data access in that call (create a database,  New or open a database, Existing).
```csharp
public enum Absio.Sdk.Database.DatabaseState
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | New | Represents the creation of a new database. | 
| `1` | Existing | Represent the opening of an existing database. | 


