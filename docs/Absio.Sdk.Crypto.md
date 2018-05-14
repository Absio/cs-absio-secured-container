## `DecryptedEccData`

This is used to return the ECC decryption results from EccUtils.DecryptAsync().  There are multiple pieces of data  wrapped up in the ECC data.
```csharp
public class Absio.Sdk.Crypto.DecryptedEccData

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Byte[]` | Data | The decrypted data. | 
| `Guid` | Id | The unique id fo the data. | 
| `Guid` | Sender | The id of the sender. | 


## `EccUtils`

This is an Absio Utility class for doing Elliptic Curve Crypto operations.  It can be used to encrypt and  decrypt data into a Absio specific format.  In order to perform the encryption all that is needed is the  data, senders signing key and the recipients derivation key.
```csharp
public class Absio.Sdk.Crypto.EccUtils

```

Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int64` | CurrentEccVersion |  | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task<DecryptedEccData>` | DecryptAsync(`Byte[]` encryptedEccData, `SigningKey` senderKey, `DerivationKey` recipientKey) | This will decrypt the encrypted data using ECDH (public key is included in the encrypted data with the  recipient's private derivation key).  The encrypted data must be in the Absio specific ECC format.  The  various portions of the data are returned in the data object.  The signature of the sender will be verified. | 
| `Task<Byte[]>` | EncryptAsync(`Byte[]` data, `Guid` senderUserId, `SigningKey` senderKey, `DerivationKey` recipientKey, `Nullable<Guid>` id = null, `Int64` eccVersion = 1) | This will encrypt the data using ECDH (random private key with the recipients public derivation key).  The encrypted results will be packaged along with all needed information to decrypt the data using  EncryptAsync().  In addition the encrypted data is signed by the sender.  The ECC format of the data is  specific to Absio. | 
| `void` | ReadKeyIndexes(`Byte[]` encryptedEccData, `Int32&` recipientDerivationKeyIndex, `Int32&` senderSigningKeyIndex) | This utility method will read out the derivation and signing indexes from the ECC data.  This  can be used to ensure that the correct keys (based on index) are sent to the the DecryptAsync() method. | 


## `KeyFileSettings`

```csharp
public class Absio.Sdk.Crypto.KeyFileSettings

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | AlgorithmName |  | 
| `Int32` | Iterations |  | 
| `Int32` | IvLength |  | 
| `Int32` | KeyLength |  | 
| `Int32` | SaltLength |  | 
| `ShaType` | ShaType |  | 
| `Int64` | Version |  | 


Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `List<KeyFileSettings>` | AllSettings |  | 
| `KeyFileSettings` | DefaultSettings |  | 
| `KeyFileSettings` | Version1Settings |  | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | GetIterations(`Int64` version) |  | 
| `KeyFileSettings` | GetSettings(`Int64` version) |  | 


## `KeyRing`

Represents an Absio user's credentials, including any IKey instances and the Absio User ID.
```csharp
public class Absio.Sdk.Crypto.KeyRing
    : IEquatable<KeyRing>

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Dictionary<Int32, DerivationKey>` | DerivationKeys | The dictionary of DerivationKey arranged by key index. | 
| `DerivationKey` | LatestDerivationKey | The latest DerivationKey by key index. | 
| `SigningKey` | LatestSigningKey | The latest SigningKey by key index. | 
| `Dictionary<Int32, SigningKey>` | SigningKeys | The dictionary of SigningKey arranged by key index. | 
| `Nullable<Guid>` | UserId | The user's unique Id. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`KeyRing` other) |  | 
| `Boolean` | Equals(`Object` obj) |  | 
| `Int32` | GetHashCode() |  | 


## `KeyType`

Enumeration of IKey usage types.
```csharp
public enum Absio.Sdk.Crypto.KeyType
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | Auth | IKey instances used for signing and authentication. | 
| `1` | Derivation | IKey instances used for key derivation. | 


## `PublicKeyMetadata`

```csharp
public class Absio.Sdk.Crypto.PublicKeyMetadata
    : IEquatable<PublicKeyMetadata>

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | Index | Read only. The KeyRing index of the key. | 
| `IKey` | Key | Read only.  The actual key. | 
| `KeyType` | KeyType | Read only.  The type of key. | 
| `Guid` | UserId | The user ID of the owner of this key. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`PublicKeyMetadata` other) |  | 
| `Boolean` | Equals(`Object` obj) |  | 
| `Int32` | GetHashCode() |  | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `PublicKeyMetadata` | Create(`Guid` userId, `Int32` index, `String` active, `String` keyType, `Byte[]` publicKeyBytes) | Create a PublicKeyMetadata out of the provided parameters.  The string values will be processed appropriately. | 


