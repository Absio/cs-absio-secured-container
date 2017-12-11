## `Ofs`

A set of utilities for interacting with the Obfuscated File System (OFS).  Based on how it is created (see CreateAsync) or  initialized (see Initialize) the methods will help with file location construction as well as a way to delete all the  data in the OFS represented by the current settings.
```csharp
public class Absio.Sdk.OFS.Ofs

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | RootDirectory | The root directory of the OFS | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task<String>` | GetHashedFileAsync(`String` fileSeed) | Gets a deterministic hashed file path in an directory. | 
| `Task<String>` | GetNewHashedFileAsync() | Finds a random, unused hashed file in an OFS directory path. | 
| `Task<Boolean>` | WipeDirectoryAsync() | Deletes the OFS directory and all underlying data associated with the initialized settings  (root directory and sub-directory seed). | 


Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | MaxFileCreationAttempts | The maximum number of times to attempt to create a file. | 
| `Int32` | MaxFilenameLength | This determines the length of file and folder names. This value should NOT be changed. Any more than 2 characters  can break older file systems. | 
| `Int32` | MaxLevelsDeepFilePath | This determines the depth of the path, not including the file name. | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Task<Ofs>` | CreateAsync(`String` baseDirectory, `String` subDirectorySeed = null) | Asynchronously creates and initializes the file system.  The RootDirectory of the OFS will be created from the  combination of the base directory and the sub-directory seed. | 


