## `AccessNotification`

Represents when a user accessed a Container from the Absio API server.
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


## `ContainerAccessLevel`

Describes a User's access to a Container.  This class is used by the SecureContainerSession.CreateAsync and SecureContainerSession.UpdateAsync methods to  define new user access to a Container. When creating a new ContainerAccessLevel, a user can define an optional  expiration, as well as specific permissions.  The class is returned by SecureContainerSession.GetAsync to describe existing container access, including the  encrypted keyblob, created/modified information, expiration, and specific permissions.
```csharp
public class Absio.Sdk.Container.ContainerAccessLevel
    : IEquatable<ContainerAccessLevel>

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Guid` | ContainerId | The unique id of the container to whom these permissions and expiration apply. | 
| `Nullable<DateTime>` | CreatedAt | The DateTime this ContainerAccessLevel was created. | 
| `Nullable<Guid>` | CreatedBy | The ID of the user who created this ContainerAccessLevel. | 
| `Nullable<DateTime>` | ExpiresAt | The date and time this user's access to the container expires. | 
| `Byte[]` | KeyBlob | The encrypted SecuredContainerKeys used to decrypt the associated container. | 
| `Nullable<DateTime>` | ModifiedAt | The DateTime this ContainerAccessLevel was last modified. | 
| `Nullable<Guid>` | ModifiedBy | The ID of the user who last modified this ContainerAccessLevel. | 
| `Permission` | Permissions | The permissions this user is allowed to perform on this container. | 
| `Guid` | UserId | The unique ID of the user to whom these permissions and expiration apply. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`ContainerAccessLevel` other) |  | 
| `Boolean` | Equals(`Object` obj) |  | 
| `Int32` | GetHashCode() |  | 


Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Permission` | DefaultContainerUserPermissions | Default permissions for users granted access to a container.  Includes permissions to download and decrypt the container, view the access list, and view the container type. | 
| `Permission` | DefaultOwnerPermissions | Default permissions for the creator of a container.  Includes permissions to download and decrypt the container, update the container, view and modify access, and view  and modify the type. | 


## `ContainerHeader`

Custom metadata encrypted and stored with the SecuredContainer.
```csharp
public class Absio.Sdk.Container.ContainerHeader
    : IEquatable<ContainerHeader>

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | CurrentVersion | The schema version of the ContainerHeader. | 
| `Object` | CustomHeader | Custom metadata to store in the SecuredContainer. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`ContainerHeader` other) |  | 
| `Boolean` | Equals(`Object` obj) |  | 
| `T` | GetCustomHeader() |  | 
| `Int32` | GetHashCode() |  | 


## `ContainerMetadata`

Contains information about a SecuredContainer, including timestamps, length, and categorical type.
```csharp
public class Absio.Sdk.Container.ContainerMetadata

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `List<ContainerAccessLevel>` | ContainerAccessLevels | The list of users, their permissions and the per-user expiration dates for this container.The owner has access to  all the ContainerAccessLevels while non-owners can only see their ContainerAccessLevel. | 
| `Nullable<DateTime>` | CreatedAt | The date and time this content was created. | 
| `Nullable<Guid>` | CreatedBy | The unique ID of the user who created this content. | 
| `Guid` | Id | The unique ID for this Container. | 
| `Int64` | Length | The length of the associated SecuredContainer. | 
| `Nullable<DateTime>` | ModifiedAt | The date and time this container was last modified. | 
| `Nullable<Guid>` | ModifiedBy | The unique ID of the user who last modified this content. | 
| `String` | Type | A string used to categorize the Container. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`Object` obj) |  | 
| `Int32` | GetHashCode() |  | 


## `IContainer`

The Container contains decrypted content bytes along with metadata and decrypted custom header.
```csharp
public interface Absio.Sdk.Container.IContainer

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `ContainerHeader` | ContainerHeader | The custom header object of this container | 
| `Byte[]` | Content | The decrypted content of this container. | 
| `Guid` | Id | The unique identifier for this Container. | 
| `Int64` | LatestEventId | The unique id of the most recent event to happen to this Container. | 
| `ContainerMetadata` | Metadata | The metadata of this container. | 
| `Nullable<DateTime>` | SyncedAt | The DateTime this container was last synchronized with the server.  Null if it hasn't been synced with the server. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `T` | GetHeaderAs() |  | 


## `Permission`

Represents various permissions that can be granted to a user with access to a SecuredContainer on the Absio API  server. These permissions will be enforced by the server, and will restrict certain information from being shared  with the user.
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


## `SecuredContainer`

The assembled and encrypted IContainer and corresponding metadata.
```csharp
public class Absio.Sdk.Container.SecuredContainer
    : IEquatable<SecuredContainer>

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `ContainerMetadata` | Metadata | The ContainerMetadata associated with this SecuredContainer. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`SecuredContainer` other) |  | 
| `Boolean` | Equals(`Object` obj) |  | 
| `Int32` | GetHashCode() |  | 


