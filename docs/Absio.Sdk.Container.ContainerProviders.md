## `BaseContainerProvider`

Provides base functionality for container provider instances.
```csharp
public class Absio.Sdk.Container.ContainerProviders.BaseContainerProvider

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `IContainer` | BuildNewContainer(`Guid` userId, `Byte[]` content, `Object` customHeaderObject = null, `List<ContainerAccessLevel>` accessLevels = null, `String` type = null) | Builds an IContainer instance from the provided parameters. | 


## `IContainerProvider`

Defines a data provider that performs IContainer create, read, update, and delete operations.
```csharp
public interface Absio.Sdk.Container.ContainerProviders.IContainerProvider

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task<Guid>` | CreateAsync(`Byte[]` content, `Object` customHeaderObject = null, `List<ContainerAccessLevel>` accessLevels = null, `String` type = null) | Creates a new container and returns its ID. | 
| `Task` | DeleteAsync(`Guid` containerId) | Deletes the container with the given ID. | 
| `Task<IContainer>` | GetAsync(`Guid` containerId) | Fetches an IContainer with the given ID. | 
| `Task<Byte[]>` | GetContentAsync(`Guid` containerId) | Fetches the data from a container with the given ID. | 
| `Task<ContainerHeader>` | GetHeaderAsync(`Guid` containerId) | Fetches the header from a container with the given ID. | 
| `Task<ContainerMetadata>` | GetMetadataAsync(`Guid` containerId) | Fetches the metadata from a container with the given ID. | 
| `void` | Initialize(`ISession` session) | Initialize the Container Provider with a user session. | 
| `Task` | UpdateAsync(`Guid` containerId, `Byte[]` content, `Object` customHeaderObject = null, `List<ContainerAccessLevel>` accessLevels = null, `String` type = null) | Updates portions of a container with the given ID. | 


## `OfsContainerProvider`

Container provider that performs IContainer create, read, update, and delete operations in a local Obfuscating File  System."/>.
```csharp
public class Absio.Sdk.Container.ContainerProviders.OfsContainerProvider
    : BaseContainerProvider, IContainerProvider

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task<Guid>` | CreateAsync(`Byte[]` content, `Object` customHeaderObject = null, `List<ContainerAccessLevel>` accessLevels = null, `String` type = null) | Creates a new container and returns its ID. | 
| `Task` | DeleteAsync(`Guid` containerId) | Deletes the container with the given ID. | 
| `Task<IContainer>` | GetAsync(`Guid` containerId) | Fetches an IContainer with the given ID. | 
| `Task<Byte[]>` | GetContentAsync(`Guid` containerId) | Fetches the data from a container with the given ID. | 
| `Task<ContainerHeader>` | GetHeaderAsync(`Guid` containerId) | Fetches the header from a container with the given ID. | 
| `Task<ContainerMetadata>` | GetMetadataAsync(`Guid` containerId) | Fetches the metadata from a container with the given ID. | 
| `void` | Initialize(`ISession` session) | Initialize the Container Provider with a user session. | 
| `Task` | UpdateAsync(`Guid` containerId, `Byte[]` content, `Object` customHeaderObject = null, `List<ContainerAccessLevel>` accessLevels = null, `String` type = null) | Updates portions of a container with the given ID. | 


## `ServerCacheOfsContainerProvider`

Container provider that performs IContainer create, read, update, and delete operations on the Absio API Server.  Containers are cached locally in the Obfuscating File System for offline access and performance gains."/>.
```csharp
public class Absio.Sdk.Container.ContainerProviders.ServerCacheOfsContainerProvider
    : BaseContainerProvider, IContainerProvider

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task<Guid>` | CreateAsync(`Byte[]` content, `Object` customHeaderObject = null, `List<ContainerAccessLevel>` accessLevels = null, `String` type = null) | Creates a new container and returns its ID. | 
| `Task` | DeleteAsync(`Guid` containerId) | Deletes the container with the given ID. | 
| `Task<IContainer>` | GetAsync(`Guid` containerId) | Fetches an IContainer with the given ID. | 
| `Task<Byte[]>` | GetContentAsync(`Guid` containerId) | Fetches the data from a container with the given ID. | 
| `Task<ContainerHeader>` | GetHeaderAsync(`Guid` containerId) | Fetches the header from a container with the given ID. | 
| `Task<ContainerMetadata>` | GetMetadataAsync(`Guid` containerId) | Fetches the metadata from a container with the given ID. | 
| `void` | Initialize(`ISession` session) | Initialize the Container Provider with a user session. | 
| `Task` | UpdateAsync(`Guid` containerId, `Byte[]` content, `Object` customHeaderObject = null, `List<ContainerAccessLevel>` accessLevels = null, `String` type = null) | Updates portions of a container with the given ID. | 


## `ServerContainerProvider`

Container provider that performs IContainer create, read, update, and delete operations on the Absio API  Server."/>.
```csharp
public class Absio.Sdk.Container.ContainerProviders.ServerContainerProvider
    : BaseContainerProvider, IContainerProvider

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task<Guid>` | CreateAsync(`Byte[]` content, `Object` customHeaderObject = null, `List<ContainerAccessLevel>` accessLevels = null, `String` type = null) | Creates a new container and returns its ID. | 
| `Task` | DeleteAsync(`Guid` containerId) | Deletes the container with the given ID. | 
| `Task<IContainer>` | GetAsync(`Guid` containerId) | Fetches an IContainer with the given ID. | 
| `Task<Byte[]>` | GetContentAsync(`Guid` containerId) | Fetches the data from a container with the given ID. | 
| `Task<ContainerHeader>` | GetHeaderAsync(`Guid` containerId) | Fetches the header from a container with the given ID. | 
| `Task<ContainerMetadata>` | GetMetadataAsync(`Guid` containerId) | Fetches the metadata from a container with the given ID. | 
| `void` | Initialize(`ISession` session) | Initialize the Container Provider with a user session. | 
| `Task` | UpdateAsync(`Guid` containerId, `Byte[]` content = null, `Object` customHeaderObject = null, `List<ContainerAccessLevel>` accessLevels = null, `String` type = null) | Updates portions of a container with the given ID. | 


