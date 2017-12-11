## `ContainerEvent`

Represents a Container-related event tracked by the Absio Broker application.
```csharp
public class Absio.Sdk.Events.ContainerEvent
    : AbstractEvent, IEquatable<AbstractEvent>

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `ContainerEventChanges` | Changes | Contains changes made to the IContainer and its access as part of an EventActionType.Updated event. | 
| `String` | ClientAppName | The client application which triggered the event. | 
| `Nullable<Guid>` | ContainerId | The container ID. | 
| `Boolean` | Expired | If an EventActionType.Deleted event was caused by expiration. | 
| `Nullable<DateTime>` | ExpiredAt | When access to the container expired. | 
| `Nullable<DateTime>` | ModifiedAt | When the container was EventActionType.Updated. | 
| `String` | Type | The categorical type of the container. | 
| `Guid` | UserId | The ID of the user that triggered the event. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`ContainerEvent` other) | Compares all properties for equality | 
| `Boolean` | Equals(`Object` obj) | Compares all properties for equality | 
| `Int32` | GetHashCode() |  | 


## `EventActionType`

Enumeration for event actions.
```csharp
public enum Absio.Sdk.Events.EventActionType
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | Accessed | IContainer was accessed by a user. | 
| `1` | Added | IContainer was stored on the Absio Broker application. | 
| `2` | Deleted | Access to an IContainer was removed on the Absio Broker application. | 
| `4` | Updated | IContainer was updated on the Absio Broker application. | 


## `EventPackageJson`

This class is used for returning lists of events from the Absio Broker application.  Events are returned as JSON in the REST request.  A block of events is represented as a package.  Events are returned from the server in these packages as ONLY a limited number  of events will be returned at a given time.  Thus, the package has the HasMore flag to denote if there is another package to  process from the Absio Broker application.
```csharp
public class Absio.Sdk.Events.EventPackageJson
    : IEquatable<EventPackageJson>

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `List<AbstractEvent>` | Events | The list of events represented by this package. | 
| `Boolean` | HasMore | True if there are more events to process for a given request. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`EventPackageJson` other) |  | 
| `Boolean` | Equals(`Object` obj) |  | 
| `IEnumerable<T>` | GetEvents(`EventType` type) |  | 
| `Int32` | GetHashCode() |  | 


## `EventType`

This is an enumeration of the different types of events that the Absio Broker application tracks and manages.
```csharp
public enum Absio.Sdk.Events.EventType
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | Container | Events dealing with Absio Secured Containers. | 
| `1` | KeysFile | Events dealing with Key File persistence. | 


