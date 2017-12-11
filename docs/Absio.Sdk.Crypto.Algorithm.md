## `CipherAlgorithmType`

Enumeration for symmetric encryption algorithms used by the Absio service.
```csharp
public class Absio.Sdk.Crypto.Algorithm.CipherAlgorithmType
    : IAlgorithmType

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Algorithm | The algorithm name. | 
| `String` | BlockAlgorithm | The block algorithm name. | 
| `Int32` | BlockSize | The algorithm block size | 
| `String` | CipherInstance | Fully-qualified algorithm name including Algorithm, BlockAlgorithm, and  PaddingAlgorithm. | 
| `CipherAlgorithmType` | DefaultType | Default CipherAlgorithmType. | 
| `Int32` | Index | Enumeration index. | 
| `Int32` | KeySize | Expected key size for the Algorithm. | 
| `Int32` | KeySizeInBits | Get the key size in bits for this algorithm. | 
| `String` | PaddingAlgorithm | Padding algorithm name. | 
| `Boolean` | RequiresIv | If the Algorithm requires an initialization vector. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | GetIvSize() | Get the initialization vector size in bytes for this algorithm. | 
| `Int32` | GetNonceSize() | Get the nonce size in bytes for this algorithm. | 


Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `CipherAlgorithmType` | Aes256 | Algorithm specification for AES/CTR/NoPadding. | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `CipherAlgorithmType` | GetByIndex(`Int32` index) | Retrieves a CipherAlgorithmType by its enumeration index. | 
| `List<CipherAlgorithmType>` | Values() |  | 


## `EllipticCurveDerivationAlgorithmType`

Enumeration for elliptic curve cryptographic derivation algorithms used by the Absio service.
```csharp
public class Absio.Sdk.Crypto.Algorithm.EllipticCurveDerivationAlgorithmType
    : IAlgorithmType

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Algorithm | The cryptographic algorithm name. | 
| `Int32` | Index | The index of the algorithm. | 
| `Int32` | KeySize | Output size of keys generated with this algorithm type. | 
| `Int32` | KeySizeInBits | Get the key size in bits for this algorithm. | 
| `Int32` | OutputSize | Output size of keys generated with this algorithm type. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`Object` obj) |  | 
| `Int32` | GetHashCode() |  | 


Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `EllipticCurveDerivationAlgorithmType` | Curve25519 | Represents the Curve25519 algorithm for use with Elliptic-Curve Diffie-Hellman. | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `EllipticCurveDerivationAlgorithmType` | GetByIndex(`Int32` index) | Retrieves a EllipticCurveDerivationAlgorithmType by its enumeration index. | 
| `List<EllipticCurveDerivationAlgorithmType>` | Values() |  | 


## `EllipticCurveSigningAlgorithmType`

Enumeration for elliptic curve signing algorithms used by the Absio service.
```csharp
public class Absio.Sdk.Crypto.Algorithm.EllipticCurveSigningAlgorithmType
    : IAlgorithmType

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Algorithm | The cryptographic algorithm name. | 
| `Int32` | Index | The index of the algorithm. | 
| `Int32` | KeySize | Output size of keys generated with this algorithm type. | 
| `Int32` | KeySizeInBits | Get the key size in bits for this algorithm. | 
| `Int32` | OutputSize | Output size of signatures generated with this algorithm type. | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | Equals(`Object` obj) |  | 
| `Int32` | GetHashCode() |  | 


Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `EllipticCurveSigningAlgorithmType` | Ed25519 | Represents the Ed25519 algorithm for use with Elliptic-Curve signing. | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `EllipticCurveSigningAlgorithmType` | GetByIndex(`Int32` index) | Retrieves a EllipticCurveSigningAlgorithmType by its enumeration index. | 
| `List<EllipticCurveSigningAlgorithmType>` | Values() |  | 


## `IAlgorithmType`

Interface for cryptographic algorithm enumerations.
```csharp
public interface Absio.Sdk.Crypto.Algorithm.IAlgorithmType

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Algorithm | The name of the algorithm. | 
| `Int32` | Index | The index of the algorithm in its respective enumeration. | 
| `Int32` | KeySize | The size of the key in bytes for the algorithm. | 
| `Int32` | KeySizeInBits | The size of the key in bits for the algorithm. | 


## `MacAlgorithmType`

Enumeration for Mac algorithms used by the Absio service.
```csharp
public class Absio.Sdk.Crypto.Algorithm.MacAlgorithmType
    : IAlgorithmType

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Algorithm | The algorithm name. | 
| `Int32` | Index | Enumeration index. | 
| `Int32` | KeySize | Expected key size for the Algorithm. | 
| `Int32` | KeySizeInBits | Get the key size in bits for this algorithm. | 
| `Int32` | OutputSize | The expected output size in bytes for this Algorithm. | 


Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `MacAlgorithmType` | HmacSha256 | Algorithm specification for HMAC-SHA256. | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `MacAlgorithmType` | GetByIndex(`Int32` index) | Retrieves a MacAlgorithmType by its enumeration index. | 
| `List<MacAlgorithmType>` | Values() |  | 


