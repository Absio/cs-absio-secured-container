## `AccessNotification`

Represents when a user accessed a Container from the Absio Broker™ application.
```csharp
public class Absio.Sdk.Container.AccessNotification
    : IEquatable<AccessNotification>

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Guid` | ContainerId | The ID of the container that was accessed. | 
| `Nullable<DateTime>` | FirstAccessed | The date and time the container was first accessed by this user. | 
| `Guid` | UserId | The ID of the user who accessed the container. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`AccessNotification` other) |  | 
| `Boolean` | Equals(`Object` obj) |  | 
| `Int32` | GetHashCode() |  | 


## `Container`

This is the decrypted version of a SecuredContainer.  It has the content, header and metadata.  The content and header are not encrypted.
```csharp
public class Absio.Sdk.Container.Container
    : IEquatable<Container>, IContainer

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `ContainerHeader` | ContainerHeader | The decrypted custom header of the container | 
| `Byte[]` | Content | The decrypted content of the container. | 
| `Guid` | Id | The ID of the Container. | 
| `ContainerMetadata` | Metadata | The metadata of the container. | 
| `Nullable<DateTime>` | SyncedAt | The DateTime the container was last synchronized with the server.  Null if it hasn't been synced with the server. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`Container` other) | Check for equality with the container. | 
| `Boolean` | Equals(`Object` obj) | Check for equality with the container. | 
| `Int32` | GetHashCode() | The unique hash code for this object. | 
| `T` | GetHeaderAs() |  | 
| `void` | SetHeader(`Object` customHeaderObject, `Int32` version = 1) | Sets the custom header to the given serializable object | 


## `ContainerAccessLevel`

Describes a User's access to a Container.  This class is used by the Container create and update methods on Providers.  In addition, the SecuredContainer Mappers  also use it.  It is used to define the access of user to a SecuredContainer.  When creating a new ContainerAccessLevel, a user can define an optional  expiration, as well as specific permissions.  ContainerMetadata that is associated with both SecuredContainers and  IContainers (the decrypted representation of a SecuredContainer) has the associated access for all users lists.  Thus, any GetXXXAsync call that includes metadata will have the access of all users for the container in question.
```csharp
public class Absio.Sdk.Container.ContainerAccessLevel
    : IEquatable<ContainerAccessLevel>

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Guid` | ContainerId | The ID of the container this access corresponds to. | 
| `Nullable<DateTime>` | CreatedAt | The DateTime this access was created. | 
| `Nullable<Guid>` | CreatedBy | The ID of the user who created this access. | 
| `Nullable<DateTime>` | ExpiresAt | The date and time this access to the container expires. | 
| `Byte[]` | KeyBlob | The encrypted SecuredContainerKeys used to decrypt the SecuredContainer.  This can only be decrypted by the UserId user. | 
| `Nullable<DateTime>` | ModifiedAt | The DateTime this access was last modified. | 
| `Nullable<Guid>` | ModifiedBy | The ID of the user who last modified this access. | 
| `Permission` | Permissions | The permissions this user is allowed to perform on this container. | 
| `Guid` | UserId | The unique ID of the user to whom these permissions and expiration apply. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`ContainerAccessLevel` other) | Compares another access to this access for equality.  Returns true if they are equal. | 
| `Boolean` | Equals(`Object` obj) | Compares another access to this access for equality.  Returns true if they are equal. | 
| `Int32` | GetHashCode() | Generates a unique hash code for the data represented in this access. | 


Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Permission` | DefaultContainerUserPermissions | Default permissions for users granted access to a container.  Includes permissions to download and decrypt the container, view the access list, and view the container type. | 
| `Permission` | DefaultOwnerPermissions | Default permissions for the creator of a container.  Includes permissions to download and decrypt the container, update the container, view and modify access, and view  and modify the type. | 


## `ContainerHeader`

Custom data encrypted and stored with the SecuredContainer.  This data can be decrypted separate  from the actual encrypted content.  Thus, identifying information, rules, controls, etc can be  encrypted separate and processed separate from the actual content.
```csharp
public class Absio.Sdk.Container.ContainerHeader
    : IEquatable<ContainerHeader>

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Object` | CustomData | Custom data to store in the SecuredContainer separate from the content. | 
| `Int32` | Version | The schema version of the ContainerHeader. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`ContainerHeader` other) | Used to compare this ContainerHeader to another.  Returns true if they are equal. | 
| `Boolean` | Equals(`Object` obj) | Used to compare this ContainerHeader to another.  Returns true if they are equal. | 
| `T` | GetDataAs() |  | 
| `Int32` | GetHashCode() | The hash code of this object. | 


## `ContainerMetadata`

Descriptive information about a SecuredContainer/IContainer, including access, timestamps, length, categorical type and the ID.  NOTE: while Absio typically thinks of the type field as a categorical descriptor of the container, it can take on any value desired.  It is stored in the encrypted database in the OfsProvider and on the Absio Broker™ application's database for the ServerProvider.  It can  be used to help access data or used for quick access to rules/control/etc associated with the container.  It is simply a string  so you can put anything in there you desire.
```csharp
public class Absio.Sdk.Container.ContainerMetadata

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `List<ContainerAccessLevel>` | ContainerAccessLevels | The list of access for the container.  The access includes the user, their permissions,  optional expiration date and encrypted key.  The access list will be populated with as much  information as the specified user has for container.  If a user does not have access to see  the access list, they will only see their specific access. | 
| `Nullable<DateTime>` | CreatedAt | The date and time the container was created. | 
| `Nullable<Guid>` | CreatedBy | The ID of the user who created the container. | 
| `Guid` | Id | The ID of the container. | 
| `Int64` | Length | The length of the SecuredContainer in bytes (as it would be on disk). | 
| `Nullable<DateTime>` | ModifiedAt | The date and time the container was last modified.  This will be null if the container has never been modified. | 
| `Nullable<Guid>` | ModifiedBy | The ID of the user who last modified the container.  This will be null if the container has never been modified. | 
| `String` | Type | A string used to categorize the container. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `ContainerAccessLevel` | AccessLevelFor(`Guid` userId) | Gets the access for a specific user.  If there are no access levels or the user is not in the list null is returned. | 
| `Boolean` | Equals(`Object` obj) | Compare the passed in object to this. | 
| `Int32` | GetHashCode() | The hash code of this object. | 


## `IContainer`

The interface for the decrypted version of a SecuredContainer.  It has the decrypted content and header.  In addition it has  all associated metadata (ContainerMetadata) and the time it was last synced.
```csharp
public interface Absio.Sdk.Container.IContainer

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `ContainerHeader` | ContainerHeader | The decrypted custom header of the container | 
| `Byte[]` | Content | The decrypted content of the container. | 
| `Guid` | Id | The ID of the Container. | 
| `ContainerMetadata` | Metadata | The metadata of the container. | 
| `Nullable<DateTime>` | SyncedAt | The DateTime the container was last synchronized with the server.  Null if it hasn't been synced with the server. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `T` | GetHeaderAs() |  | 


## `Permission`

Represents various permissions that can be granted to a user with access to a SecuredContainer.  These permissions will be enforced by the Absio Broker™ application when using the ServerProvider, and  will restrict certain information from being shared with the user.  The OfsProvider will not  enforce the adherence to the permissions.  It is up to all providers to determine how, if at  all, they would like to enforce permissions.  The same is true of the Mappers that a Provider  uses.
```csharp
public enum Absio.Sdk.Container.Permission
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `1` | DownloadContainer | Permission to download the SecuredContainer header and content. | 
| `2` | DecryptContainer | Permission to decrypt the SecuredContainer header and content. | 
| `4` | UploadContainer | Permission to modify the SecuredContainer header and content and upload to the server. | 
| `8` | ViewAccess | Permission to view the access list for the SecuredContainer. | 
| `16` | ModifyAccess | Permission to modify the access list for the SecuredContainer. | 
| `32` | ModifyContainerType | Permission to modify the categorical type of the SecuredContainer. | 
| `64` | ViewContainerType | Permission to view the categorical type of the SecuredContainer. | 
| `128` | RxAccessEvents | Permission to receive the access events of the SecuredContainer. | 


## `SecuredContainer`

This contains the Absio format of the encrypted content and header, its length and corresponding metadata.  This is the encrypted version of a Container/IContainer.
```csharp
public class Absio.Sdk.Container.SecuredContainer
    : IEquatable<SecuredContainer>

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int64` | Length | The Length of the encypted data. | 
| `ContainerMetadata` | Metadata | The ContainerMetadata associated with the container. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Byte[]` | Bytes() | Gets the secured bytes of this SecuredContainer. | 
| `Boolean` | Equals(`SecuredContainer` other) |  | 
| `Boolean` | Equals(`Object` obj) |  | 
| `Int32` | GetHashCode() |  | 
| `Stream` | GetStream() | Gets the secured bytes of this SecuredContainer as a Stream. | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task<SecuredContainer>` | CreateAsync(`Byte[]` securedBytes, `ContainerMetadata` metadata) | Asynchronously creates a SecuredContainer from the given secured bytes and metadata. | 


