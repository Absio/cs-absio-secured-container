## `AbsioCodedException`

Base class from which all Absio exceptions should inherit.
```csharp
public class Absio.Sdk.Exceptions.AbsioCodedException
    : Exception, ISerializable, _Exception

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | ErrorGuid | The guid of the error returned by the server. | 


## `AllSigningKeysInactiveException`

Specialized exception class to be used when all the Signing Keys in a users key file are inactive
```csharp
public class Absio.Sdk.Exceptions.AllSigningKeysInactiveException
    : AbsioCodedException, ISerializable, _Exception

```

## `AlreadyExistsException`

Specialized exception class to be used when the user attempts to register a username that is already in use
```csharp
public class Absio.Sdk.Exceptions.AlreadyExistsException
    : AbsioCodedException, ISerializable, _Exception

```

## `ApiKeyDisabledException`

Specialized exception class to be used when the container was not found on the server.
```csharp
public class Absio.Sdk.Exceptions.ApiKeyDisabledException
    : AbsioCodedException, ISerializable, _Exception

```

## `AuthenticationException`

Thrown when attempting to call a method call without local authentication.
```csharp
public class Absio.Sdk.Exceptions.AuthenticationException
    : AbsioCodedException, ISerializable, _Exception

```

## `ContainerDigestValidationException`

Thrown when the container digest validation fails
```csharp
public class Absio.Sdk.Exceptions.ContainerDigestValidationException
    : AbsioCodedException, ISerializable, _Exception

```

## `ContainerRecalledException`

Thrown when attempting to access the ContainerHeader or content of an recalled Container.
```csharp
public class Absio.Sdk.Exceptions.ContainerRecalledException
    : AbsioCodedException, ISerializable, _Exception

```

## `DecryptPermissionException`

Thrown when Decrypt Container permission isn't set but Key Blob is requested.
```csharp
public class Absio.Sdk.Exceptions.DecryptPermissionException
    : Exception, ISerializable, _Exception

```

## `DownloadPermissionException`

Thrown when Download Container permission isn't set but content is requested.
```csharp
public class Absio.Sdk.Exceptions.DownloadPermissionException
    : Exception, ISerializable, _Exception

```

## `ExpiredException`

Thrown when attempting to access the ContainerHeader or content of an expired Container.
```csharp
public class Absio.Sdk.Exceptions.ExpiredException
    : AbsioCodedException, ISerializable, _Exception

```

## `IncorrectArgumentException`

```csharp
public class Absio.Sdk.Exceptions.IncorrectArgumentException
    : AbsioCodedException, ISerializable, _Exception

```

## `InsufficientPermissionsException`

Specialized exception class to be used when the user doesn't have sufficient permission to perform the attempted  request.
```csharp
public class Absio.Sdk.Exceptions.InsufficientPermissionsException
    : AbsioCodedException, ISerializable, _Exception

```

## `InvalidException`

Specialized exception class to be used when the user attempts to register a username that is already in use
```csharp
public class Absio.Sdk.Exceptions.InvalidException
    : AbsioCodedException, ISerializable, _Exception

```

## `KeyException`

This thrown when a there is an issue with a Key.  For instance, if the Key type index is not supported this exception will be thrown.
```csharp
public class Absio.Sdk.Exceptions.KeyException
    : Exception, ISerializable, _Exception

```

## `KeyFileDecryptionException`

```csharp
public class Absio.Sdk.Exceptions.KeyFileDecryptionException
    : AbsioCodedException, ISerializable, _Exception

```

## `KeyFileDoesNotExistLocallyException`

```csharp
public class Absio.Sdk.Exceptions.KeyFileDoesNotExistLocallyException
    : AbsioCodedException, ISerializable, _Exception

```

## `KeyFileRescueException`

```csharp
public class Absio.Sdk.Exceptions.KeyFileRescueException
    : Exception, ISerializable, _Exception

```

## `KeyFileSettingsException`

```csharp
public class Absio.Sdk.Exceptions.KeyFileSettingsException
    : Exception, ISerializable, _Exception

```

## `KeyFileUsedException`

Specialized exception class to be used when the number of uses of a key file would be exceeded
```csharp
public class Absio.Sdk.Exceptions.KeyFileUsedException
    : AbsioCodedException, ISerializable, _Exception

```

## `LicenseKeyExceededException`

Specialized exception class to be used when the license count would have been exceeded
```csharp
public class Absio.Sdk.Exceptions.LicenseKeyExceededException
    : AbsioCodedException, ISerializable, _Exception

```

## `NotFoundException`

Specialized exception class to be used when the container was not found on the Absio Broker® application.
```csharp
public class Absio.Sdk.Exceptions.NotFoundException
    : AbsioCodedException, ISerializable, _Exception

```

## `NotFoundLocallyException`

Specialized exception class to be used when the container was not found in the OFS.
```csharp
public class Absio.Sdk.Exceptions.NotFoundLocallyException
    : AbsioCodedException, ISerializable, _Exception

```

## `OfsFileCreationException`

This exception is thrown if Ofs fails to find a clean file location on the GetNewHashedFileAsync call.
```csharp
public class Absio.Sdk.Exceptions.OfsFileCreationException
    : AbsioCodedException, ISerializable, _Exception

```

## `RecipientKeyBlobVerificationException`

Thrown when the RecipientKeyBlob signing public key validation fails
```csharp
public class Absio.Sdk.Exceptions.RecipientKeyBlobVerificationException
    : AbsioCodedException, ISerializable, _Exception

```

## `SdkExceptionCodes`

```csharp
public static class Absio.Sdk.Exceptions.SdkExceptionCodes

```

Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | ContainerContentDigestValidationCode |  | 
| `String` | ContainerHeaderDigestValidationCode |  | 
| `String` | ContainerNotFoundCode |  | 
| `String` | ContainerNotFoundLocallyCode |  | 
| `String` | KeyFilePassphraseWrongCode |  | 
| `String` | LoginAllKeysInactiveCode |  | 
| `String` | LoginKeyFileDecryptionCode |  | 
| `String` | LoginKeyFileDecryptionWithNewerOnServerCode |  | 
| `String` | LoginKeyFileDoesNotExistLocallyCode |  | 
| `String` | LoginKeyFileUsedCode |  | 
| `String` | LoginUserNotFoundCode |  | 
| `String` | NotAbsioCoreAuthenticatedCode |  | 
| `String` | NotAbsioServerAuthenticatedCode |  | 
| `String` | OfsFileCreationCode |  | 
| `String` | Pbkdf2DecryptionCode |  | 
| `String` | RecipientKeyBlobVerificationCode |  | 
| `String` | RescueKeyFileDecryptionCode |  | 
| `String` | ServerAuthKeyFileInvalidCode |  | 
| `String` | ServerConnectionCode |  | 
| `String` | UnknownErrorCode |  | 
| `String` | ValidatePasswordKeyFileDecryptionCode |  | 


## `SdkExceptionInfo`

```csharp
public static class Absio.Sdk.Exceptions.SdkExceptionInfo

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Exception` | FromErrorCode(`String` errorCode) |  | 
| `void` | ThrowFromErrorCode(`String` errorCode) |  | 


## `SdkExceptionMessages`

```csharp
public static class Absio.Sdk.Exceptions.SdkExceptionMessages

```

Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | ContainerContentDigestValidationMessage |  | 
| `String` | ContainerHeaderDigestValidationMessage |  | 
| `String` | ContainerNotFoundLocallyMessage |  | 
| `String` | ContainerNotFoundMessage |  | 
| `String` | LoginAllKeysInactiveMessage |  | 
| `String` | LoginKeyFileDecryptionMessage |  | 
| `String` | LoginKeyFileDecryptionWithNewerOnServerMessage |  | 
| `String` | LoginKeyFileDoesNotExistLocallyMessage |  | 
| `String` | LoginKeyFileUsedMessage |  | 
| `String` | LoginUserNotFoundMessage |  | 
| `String` | NotAbsioCoreAuthenticationMessage |  | 
| `String` | NotAbsioServerAuthenticationMessage |  | 
| `String` | OfsFileCreationMessage |  | 
| `String` | Pbkdf2DecryptionMessage |  | 
| `String` | RescueKeyFileDecryptionMessage |  | 
| `String` | ServerAuthKeyFileInvalidMessage |  | 
| `String` | ServerConnectionMessage |  | 
| `String` | UnknownErrorMessage |  | 
| `String` | ValidatePasswordKeyFileDecryptionMessage |  | 


## `ServerConnectionException`

Specialized exception class to be used when there is a problem connecting to the Absio Broker® application
```csharp
public class Absio.Sdk.Exceptions.ServerConnectionException
    : AbsioCodedException, ISerializable, _Exception

```

## `ServerErrorException`

Thrown when the Absio Broker® application rejects an API call.
```csharp
public class Absio.Sdk.Exceptions.ServerErrorException
    : Exception, ISerializable, _Exception

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `HttpStatusCode` | Code | The HttpStatusCode returned by the server. | 
| `String` | CodeInfo | Additional information for the error. | 
| `String` | ServerErrorCode | The server error code. | 


## `ServerExceptionCodes`

Unique error codes for the API endpoints.  If something returns a non-200 HTTP status code, it should also return an error id value indicating exactly what  the problem is.
```csharp
public static class Absio.Sdk.Exceptions.ServerExceptionCodes

```

Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | ApiKeyDisabled |  | 
| `String` | ApiKeyInvalidType |  | 
| `String` | ApiKeyMissing |  | 
| `String` | ApiKeyNotFound |  | 
| `String` | ContainerCreateAlreadyExists |  | 
| `String` | ContainerDeleteNotFound |  | 
| `String` | ContainerGetNotFound |  | 
| `String` | ContainerPublishContainerNotFound |  | 
| `String` | ContainerPublishContainerRecipNotFound |  | 
| `String` | ContainerPublishInsufficientPermissions |  | 
| `String` | ContainerRecallInsufficientPermissions |  | 
| `String` | ContainerRecallNotFound |  | 
| `String` | ContainerRecipsContainerNotFound |  | 
| `String` | ContainerRecipsContainerRecipNotFound |  | 
| `String` | ContainerRecipsInsufficientPermissions |  | 
| `String` | ContainerUpdateAccessContainerNotFound |  | 
| `String` | ContainerUpdateAccessInsufficientPermissions |  | 
| `String` | ContainerUpdateContainerNotFound |  | 
| `String` | ContainerUpdateContainerRecipNotFound |  | 
| `String` | ContainerUpdateHasBeenRecalled |  | 
| `String` | ContainerUpdateInsufficientPermissions |  | 
| `String` | ContainerUpdateMetadataRecipNotFound |  | 
| `String` | ContainerUpdateReadRecipNotFound |  | 
| `String` | ContainerUpdateRecipientsRequired |  | 
| `String` | ContainerUpdateTypeContainerNotFound |  | 
| `String` | ContainerUpdateTypeInsufficientPermissions |  | 
| `String` | KeysCreateAlreadyExists |  | 
| `String` | KeysCreateInvalidKey |  | 
| `String` | KeysFileChecksumGetKeysFileNotFound |  | 
| `String` | KeysFileChecksumGetUserIdInvalid |  | 
| `String` | KeysFileChecksumGetUserNotFound |  | 
| `String` | KeysFileCreateKeysFileExists |  | 
| `String` | KeysFileGetInvalidPassphrase |  | 
| `String` | KeysFileGetInvalidUserId |  | 
| `String` | KeysFileGetKeysFileNotFound |  | 
| `String` | KeysFileGetPassphraseRequired |  | 
| `String` | KeysFileGetUsagesExceeded |  | 
| `String` | KeysFileGetUserNotFound |  | 
| `String` | KeysFileReminderGetKeysFileNotFound |  | 
| `String` | KeysFileReminderGetUserIdInvalid |  | 
| `String` | KeysFileReminderGetUserNotFound |  | 
| `String` | KeysFileUpdateKeysFileNotFound |  | 
| `String` | KeysGetUserNotFound |  | 
| `String` | KeysUpdateInvalidKey |  | 
| `String` | KeysUpdateKeyNotFound |  | 
| `String` | LoginAuthKeyNotFound |  | 
| `String` | LoginUserNotActive |  | 
| `String` | LoginUserNotFound |  | 
| `String` | LoginVerificationFailure |  | 
| `String` | UserCreateLicenseKeyExceeded |  | 


## `ServerExceptionInfo`

```csharp
public static class Absio.Sdk.Exceptions.ServerExceptionInfo

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Exception` | FromErrorCode(`ServerErrorException` exception) |  | 
| `void` | ThrowFromErrorCode(`ServerErrorException` exception) |  | 


## `ServerExceptionMessages`

Unique error messages for the API endpoints.
```csharp
public static class Absio.Sdk.Exceptions.ServerExceptionMessages

```

Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | ApiKeyDisabled |  | 
| `String` | ApiKeyInvalidType |  | 
| `String` | ApiKeyMissing |  | 
| `String` | ApiKeyNotFound |  | 
| `String` | ContainerCreateAlreadyExists |  | 
| `String` | ContainerDeleteNotFound |  | 
| `String` | ContainerGetNotFound |  | 
| `String` | ContainerPublishContainerNotFound |  | 
| `String` | ContainerPublishContainerRecipNotFound |  | 
| `String` | ContainerPublishInsufficientPermissions |  | 
| `String` | ContainerRecallInsufficientPermissions |  | 
| `String` | ContainerRecallNotFound |  | 
| `String` | ContainerRecipsContainerNotFound |  | 
| `String` | ContainerRecipsContainerRecipNotFound |  | 
| `String` | ContainerRecipsInsufficientPermissions |  | 
| `String` | ContainerUpdateAccessContainerNotFound |  | 
| `String` | ContainerUpdateAccessInsufficientPermissions |  | 
| `String` | ContainerUpdateContainerNotFound |  | 
| `String` | ContainerUpdateContainerRecipNotFound |  | 
| `String` | ContainerUpdateHasBeenRecalled |  | 
| `String` | ContainerUpdateInsufficientPermissions |  | 
| `String` | ContainerUpdateMetadataRecipNotFound |  | 
| `String` | ContainerUpdateReadRecipNotFound |  | 
| `String` | ContainerUpdateRecipientsRequired |  | 
| `String` | ContainerUpdateTypeContainerNotFound |  | 
| `String` | ContainerUpdateTypeInsufficientPermissions |  | 
| `String` | KeysCreateAlreadyExists |  | 
| `String` | KeysCreateInvalidKey |  | 
| `String` | KeysFileChecksumGetKeysFileNotFound |  | 
| `String` | KeysFileChecksumGetUserIdInvalid |  | 
| `String` | KeysFileChecksumGetUserNotFound |  | 
| `String` | KeysFileCreateKeysFileExists |  | 
| `String` | KeysFileGetInvalidPassphrase |  | 
| `String` | KeysFileGetInvalidUserId |  | 
| `String` | KeysFileGetKeysFileNotFound |  | 
| `String` | KeysFileGetPassphraseRequired |  | 
| `String` | KeysFileGetUsagesExceeded |  | 
| `String` | KeysFileGetUserNotFound |  | 
| `String` | KeysFileReminderGetKeysFileNotFound |  | 
| `String` | KeysFileReminderGetUserIdInvalid |  | 
| `String` | KeysFileReminderGetUserNotFound |  | 
| `String` | KeysFileUpdateKeysFileNotFound |  | 
| `String` | KeysGetUserNotFound |  | 
| `String` | KeysUpdateInvalidKey |  | 
| `String` | KeysUpdateKeyNotFound |  | 
| `String` | LoginAuthKeyNotFound |  | 
| `String` | LoginUserNotActive |  | 
| `String` | LoginUserNotFound |  | 
| `String` | LoginVerificationFailure |  | 
| `String` | UserCreateLicenseKeyExceeded |  | 


## `UserSuspendedViaWebClientException`

Specialized exception to be used when the user has been suspended
```csharp
public class Absio.Sdk.Exceptions.UserSuspendedViaWebClientException
    : AbsioCodedException, ISerializable, _Exception

```

## `UserTerminatedException`

Specialized exception to be used when a user has been terminated
```csharp
public class Absio.Sdk.Exceptions.UserTerminatedException
    : AbsioCodedException, ISerializable, _Exception

```

## `VersionException`

This thrown when a particular version of a portion of the SDK is not supported.  For instance, if the ECC version is not supported this exception will be thrown.
```csharp
public class Absio.Sdk.Exceptions.VersionException
    : Exception, ISerializable, _Exception

```

