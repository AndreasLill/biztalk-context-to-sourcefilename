# BizTalk Pipeline Component
A small BizTalk pipeline component to write context properties to be used with %SourceFileName%.

## Properties

### ContextPropertySchemas
A collection of property schemas containing the context properties.
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
The target source file name using biztalk or custom context properties.

The context properties are found in the component using [%ContextName%].

Examples:
```
[%InterchangeID%]_AnyTextYouLike
```
```
FILE_[%FileCreationTime%][%CustomerID%]
```
