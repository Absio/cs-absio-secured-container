## `IContainerDatabaseStorage`

Provides database storage for ContainerDbInfo and other IContainer metadata.
```csharp
public interface Absio.Sdk.Container.Storages.IContainerDatabaseStorage

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task` | DeleteContainerFromDbAsync(`Guid` containerId) | Deletes container information from the database. | 
| `Task<ContainerDbInfo>` | ReadContainerFromDbAsync(`Guid` containerId) | Reads container information from the database. | 
| `Task` | UpdateAccessLevelInDbAsync(`List<ContainerAccessLevel>` accessLevels) | Writes ContainerAccessLevel information to the database. | 
| `Task` | UpdateContainerInDbAsync(`ContainerDbInfo` info) | Updates container information in the database. | 
| `Task` | WriteContainerToDbAsync(`ContainerDbInfo` info) | Writes container information to the database. | 


## `IContainerStorage<TResult, TParam>`

Interface for generic container storage operations.
```csharp
public interface Absio.Sdk.Container.Storages.IContainerStorage<TResult, TParam>

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task` | CreateAsync(`TParam` data) |  | 
| `Task` | DeleteAsync(`TParam` data) |  | 
| `Task<TResult>` | ReadAsync(`TParam` data) |  | 
| `Task` | UpdateAsync(`TParam` data) |  | 


## `IOfsStorage`

Provides operations for storing encrypted SecuredContainers in the Obfuscating File System.
```csharp
public interface Absio.Sdk.Container.Storages.IOfsStorage
    : IContainerStorage<SecuredContainer, ContainerParameters>

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task<String>` | GetNewContainerOfsLocationAsync() | Retrieves a random unused file location in the Obfuscating File System. | 


## `IServerStorage`

Provides operations for storing encrypted SecuredContainers on the Absio API Server.
```csharp
public interface Absio.Sdk.Container.Storages.IServerStorage
    : IContainerStorage<ContainerDownloadInfo, ContainerUploadInfo>

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task<ContainerDownloadInfo>` | ReadContainerInfoAsync(`Guid` containerId) | Reads the container info (metadata and url info) without downloading content bytes. | 
| `Task` | UpdateContainerAccessLevelsAsync(`Guid` containerId, `List<ContainerAccessLevel>` accessLevels) | Updates the given container access levels on the Absio Server. | 
| `Task` | UpdateContainerTypeAsync(`Guid` containerId, `String` containerType) | Updates the given container type on the Absio Server | 


