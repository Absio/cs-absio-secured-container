## `AbstractEvent`

Represents an event triggered by the Absio server or the CoreSdkSession.
```csharp
public abstract class Absio.Sdk.Abstract.AbstractEvent
    : IEquatable<AbstractEvent>

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `EventActionType` | ActionType | The event action type. | 
| `DateTime` | Date | The event date. | 
| `Int64` | EventId | The event ID. | 
| `EventType` | EventType | The event type. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`AbstractEvent` other) | Compares all properties for equality | 
| `Boolean` | Equals(`Object` obj) | Compares all properties for equality | 
| `Int32` | GetHashCode() |  | 


## `AbstractOfsMapper`

The base class for all OFS Mappers (for mapping data to the OFS).
```csharp
public abstract class Absio.Sdk.Abstract.AbstractOfsMapper

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `IDataAccess` | DataAccess | The data access used for persisting and reading all data (except for content/header) from the OFS. | 
| `Ofs` | Ofs | The instance of the OFS itself.  This class is used for determining storage locations. | 


## `AbstractServerMapper`

The base class for all Server Mappers (for mapping data to the Absio Broker™ application).
```csharp
public abstract class Absio.Sdk.Abstract.AbstractServerMapper

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Authenticated | True if the Credentials property is not null. | 
| `IRestClient` | Client | Gets the rest client from the rest client session. | 
| `AuthenticatedServerCredentials` | Credentials | Gets the credentials from the rest client session. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Dispose() | Dispose of the mapper. | 


