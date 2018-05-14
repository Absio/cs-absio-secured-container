## `AccessChange`

Contains changes made to a particular ContainerAccessLevel stored on Absio Broker™ application.
```csharp
public class Absio.Sdk.Events.EventChanges.AccessChange
    : IEquatable<AccessChange>

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Nullable<DateTime>` | ExpiresAt | Read only.  When this user's access will expire.  Null if never. | 
| `Byte[]` | KeyBlob | Read only.  A new public key blob for access to the container. | 
| `Nullable<DateTime>` | KeyBlobCreatedAt | Read only.  When the above key blob was created. | 
| `Nullable<Guid>` | KeyBlobCreatedBy | Read only.  Who created the above key blob. | 
| `Nullable<DateTime>` | KeyBlobModifiedAt | Read only.  When the above key blob was modified. | 
| `Nullable<Guid>` | KeyBlobModifiedBy | Read only.  Who modified the above key blob. | 
| `Nullable<Permission>` | Permissions | Read only.  The permission set granted to this user for this container. | 
| `Guid` | UserId | Read only.  The unique id of the user whose access is changing. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`AccessChange` other) | Compares this instance with the other instance for equality based on the properties. | 
| `Boolean` | Equals(`Object` obj) | Compares this instance with the other instance for equality based on the properties. | 
| `Int32` | GetHashCode() |  | 


## `ContainerEventChanges`

Contains a list of changes made to an IContainer and its ContainerAccessLevel stored on the Absio Broker™ application.
```csharp
public class Absio.Sdk.Events.EventChanges.ContainerEventChanges
    : IEquatable<ContainerEventChanges>

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `List<AccessChange>` | AccessAdded | List of AccessChange information for access additions. | 
| `List<AccessChange>` | AccessChanged | List of AccessChange information for modifications to existing access. | 
| `List<AccessChange>` | AccessRemoved | List of AccessChange information for access removals. | 
| `SecuredContainerChanges` | Changes | Changes made to the SecuredContainer stored on the server as part of this event. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`ContainerEventChanges` other) |  | 
| `Boolean` | Equals(`Object` obj) |  | 
| `Int32` | GetHashCode() |  | 


## `SecuredContainerChanges`

Contains changes made to a particular IContainer stored on Absio Broker™ application.
```csharp
public class Absio.Sdk.Events.EventChanges.SecuredContainerChanges
    : IEquatable<SecuredContainerChanges>

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Nullable<Int64>` | Length | Read only.  The new length of the secured container. | 
| `Nullable<DateTime>` | ModifiedAt | Read only.  When this secured container changed. | 
| `Nullable<Guid>` | ModifiedBy | Read only.  The unique id of the user who changed the secured container. | 
| `String` | Type | Read only.  The type of secured container. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`SecuredContainerChanges` other) | Compares this instance with the other instance for equality based on the properties. | 
| `Boolean` | Equals(`Object` obj) | Compares this instance with the other instance for equality based on the properties. | 
| `Int32` | GetHashCode() |  | 


