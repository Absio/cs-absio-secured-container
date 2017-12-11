## `ContainerInfoJson`

Contains information for downloading an IContainer from the Absio Broker application.
```csharp
public class Absio.Sdk.Server.Response.ContainerInfoJson
    : IEquatable<ContainerInfoJson>

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `List<ContainerAccessLevel>` | Access | The access list of the container from the Absio Broker application. | 
| `ContainerMetadata` | Metadata | The metadata for the container from the Absio Broker application. | 
| `UrlInfoJson` | UrlInfo | The URL information to download the encrypted container from the Absio Broker application's storage location. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`ContainerInfoJson` other) |  | 
| `Boolean` | Equals(`Object` obj) |  | 
| `Int32` | GetHashCode() |  | 


## `UrlInfoJson`

This is a JSON representation of the URL data used to define where an encrypted container is stored in the Absio Broker application's  storage location (out of the box this is on S3).  This has the URL and the expiration time. This is used for upload and download  of the encrypted container.
```csharp
public class Absio.Sdk.Server.Response.UrlInfoJson
    : IEquatable<UrlInfoJson>

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `DateTime` | ExpiresAt | The expiration time of the URL. | 
| `Boolean` | IsExpired | True if the URL is already expired. | 
| `String` | Url | The storage location as a URL for the container. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`UrlInfoJson` other) |  | 
| `Boolean` | Equals(`Object` obj) |  | 
| `Int32` | GetHashCode() |  | 


