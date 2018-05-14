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
| `MacAlgorithmType` | HmacSha384 | Algorithm specification for HMAC-SHA384. | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `MacAlgorithmType` | GetByIndex(`Int32` index) | Retrieves a MacAlgorithmType by its enumeration index. | 
| `List<MacAlgorithmType>` | Values() |  | 


