## `IEncryptionService`

Provides encryption and decryption services for IContainer and SecuredContainer.
```csharp
public interface Absio.Sdk.Container.Encryption.IEncryptionService

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task<IContainer>` | DecryptContainer(`SecuredContainer` securedContainer) | Decrypts the SecuredContainer content and header. | 
| `Task<IContainer>` | DecryptContainerContent(`SecuredContainer` securedContainer) | Decrypts the SecuredContainer content. | 
| `Task<IContainer>` | DecryptContainerHeader(`SecuredContainer` securedContainer) | Decrypts the SecuredContainer header object. | 
| `Task<SecuredContainer>` | EncryptContainer(`IContainer` container) | Encrypts and serializes a container. | 
| `Task` | GenerateKeyBlobForEachUserAccess(`List<ContainerAccessLevel>` containerAccessLevels, `Guid` containerId, `ContainerMetadata` containerMetadata) | Generates an encrypted copy of the SecuredContainer encryption key for each user listed in the container access  levels.  The encrypted key is set on each ContainerAccessLevel.KeyBlob property. | 


