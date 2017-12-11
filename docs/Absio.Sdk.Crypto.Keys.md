## `AbstractKey<T>`

Represents an asymmetric IKey used for cryptographic key operations.
```csharp
public abstract class Absio.Sdk.Crypto.Keys.AbstractKey<T>
    : IKey

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Active | Whether the DerivationKey is active and should be used in cryptographic processes. | 
| `IAlgorithmType` | AlgorithmType | The type of algorithm the key is for. | 
| `Byte[]` | Bytes | The bytes of the DerivationKey. | 
| `Int32` | Index | The index of the DerivationKey in the user's keychain. | 
| `Boolean` | IsPublic | Whether the DerivationKey is the public component. | 
| `T` | Type | The cryptographic type of the DerivationKey. | 
| `KeyType` | UsageType | The usage type of this key (signing or derivation). | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`Object` o) |  | 
| `Int32` | GetHashCode() |  | 
| `Byte[]` | PublicBytes() | Gets the Public component of the DerivationKey. | 


## `DerivationKey`

Represents an asymmetric IKey used for cryptographic key derivation.
```csharp
public class Absio.Sdk.Crypto.Keys.DerivationKey
    : AbstractKey<EllipticCurveDerivationAlgorithmType>, IKey

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `EllipticCurveDerivationAlgorithmType` | Type | The cryptographic type of the DerivationKey. | 
| `KeyType` | UsageType | The usage type of this key (derivation). | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `DerivationKey` | GetPublicDerivationKey() | Gets a public component instance of the DerivationKey. | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `DerivationKey` | NewKey(`Int32` index) | Generates a new DerivationKey instance with the specified index. | 


## `IKey`

Represents an asymmetric IKey used for cryptographic key derivation.
```csharp
public interface Absio.Sdk.Crypto.Keys.IKey

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Active | Whether the IKey is active and should be used in cryptographic processes. | 
| `IAlgorithmType` | AlgorithmType | The type of algorithm the key is for. | 
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
    : AbstractKey<EllipticCurveSigningAlgorithmType>, IKey

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `EllipticCurveSigningAlgorithmType` | Type | The cryptographic type of the SigningKey. | 
| `KeyType` | UsageType | The usage type of this key (signing). | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `SigningKey` | GetPublicSigningKey() | Gets a public component instance of the SigningKey. | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `SigningKey` | NewKey(`Int32` index) | Generates a new SigningKey instance with the specified index. | 


