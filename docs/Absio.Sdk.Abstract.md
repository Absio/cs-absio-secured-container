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


