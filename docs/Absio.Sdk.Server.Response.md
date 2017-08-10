## `ContainerDownloadInfo`

Contains information for downloading an IContainer from the Absio API server.
```csharp
public class Absio.Sdk.Server.Response.ContainerDownloadInfo
    : IEquatable<ContainerDownloadInfo>

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Byte[]` | Content | The decrypted content of this container. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`ContainerDownloadInfo` other) |  | 
| `Boolean` | Equals(`Object` obj) |  | 
| `Int32` | GetHashCode() |  | 


## `ContainerUploadInfo`

Contains information for uploading an IContainer to the Absio API server.
```csharp
public class Absio.Sdk.Server.Response.ContainerUploadInfo
    : IEquatable<ContainerUploadInfo>

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int64` | Length | The length of the encrypted SecuredContainer | 
| `String` | Type | The categorical type of the container. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`ContainerUploadInfo` other) | Compares this instance with the other instance for equality based on the properties. | 
| `Boolean` | Equals(`Object` obj) | Compares this instance with the other instance for equality based on the properties. | 
| `Int32` | GetHashCode() |  | 


