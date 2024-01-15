# BizTalk Pipeline Component
A small BizTalk pipeline component to read context properties and write it to %SourceFileName%.

## Properties

### ContextPropertySchemas
A collection of property schemas containing the context properties to read.
Delimited by | (pipe character).

Example using one property schema:
```
http://schemas.microsoft.com/BizTalk/2003/file-properties
```

Example using two property schemas:
```
http://schemas.microsoft.com/BizTalk/2003/file-properties|http://schemas.microsoft.com/BizTalk/2003/system-properties
```

### OverrideFileExtension
Used to replace current file extension in source file name.
Leave empty to keep current file extension.

Example:

```
.xml
```

### TargetSourceFileName
The target source file name containing the context property macros.
The context properties are found in the component using [%ContextName%].
This will be written to %SourceFileName% (ReceivedFileName) context.

Examples:
```
[%InterchangeID%]_AnyTextYouLike
```
```
FILE_[%FileCreationTime%][%CustomerID%]
```
