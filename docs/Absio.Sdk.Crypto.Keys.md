## `DerivationKey`

Represents an asymmetric IKey used for cryptographic key derivation.
```csharp
public class Absio.Sdk.Crypto.Keys.DerivationKey
    : EllipticCurveKey, IKey

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `DerivationKey` | NewKey(`Int32` index) |  | 


## `EllipticCurveKey`

Represents an elliptical curve asymmetric IKey used for cryptographic key operations.
```csharp
public class Absio.Sdk.Crypto.Keys.EllipticCurveKey
    : IKey

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Active | Whether the DerivationKey is active and should be used in cryptographic processes. | 
| `Byte[]` | Bytes | The bytes of the DerivationKey. | 
| `Int32` | Index | The index of the DerivationKey in the user's keychain. | 
| `Boolean` | IsPublic | Whether the DerivationKey is the public component. | 
| `KeyType` | UsageType | The usage type of this key (signing or derivation). | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`Object` o) |  | 
| `Int32` | GetHashCode() |  | 
| `EllipticCurveKey` | GetPublicKey() | Gets a public component instance of the DerivationKey. | 
| `Byte[]` | PublicBytes() | Gets the Public component of the DerivationKey. | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `EllipticCurveKey` | NewKey(`Int32` index, `KeyType` usageType) | Generates a new DerivationKey instance with the specified index. | 


## `IKey`

Represents an asymmetric IKey used for cryptographic key derivation.
```csharp
public interface Absio.Sdk.Crypto.Keys.IKey

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Active | Whether the IKey is active and should be used in cryptographic processes. | 
| `Byte[]` | Bytes | The bytes of the IKey. | 
| `Int32` | Index | The index of the IKey in the user's keychain. | 
| `Boolean` | IsPublic | Whether the IKey is the public component. | 
| `KeyType` | UsageType | The usage type of this key (signing or derivation). | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Byte[]` | PublicBytes() | Gets the Public component of the IKey. | 


## `SigningKey`

Represents an asymmetric IKey used for cryptographic signing and authentication.
```csharp
public class Absio.Sdk.Crypto.Keys.SigningKey
    : EllipticCurveKey, IKey

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `SigningKey` | NewKey(`Int32` index) |  | 


