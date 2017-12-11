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
| `Int32` | AlgorithmIndex | Read only.  The index of the algorithm used to generate this key. | 
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
| `PublicKeyMetadata` | Create(`Guid` userId, `Int32` algorithmIndex, `Int32` index, `String` active, `String` keyType, `Byte[]` publicKeyBytes) | Create a PublicKeyMetadata out of the provided parameters.  The string values will be processed appropriately. | 


