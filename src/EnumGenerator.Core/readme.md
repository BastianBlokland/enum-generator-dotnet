# **EnumGenerator** Core library.

Library that can be used to generate c# / f# / vb / cil enum files based on json files.

Can be used for more complex integration into a build pipeline, for simple use-cases consider one of the pre-made tools:
* [**EnumGenerator.Cli**](https://www.nuget.org/packages/EnumGenerator.Cli/)
* [**EnumGenerator.GlobalTool**](https://www.nuget.org/packages/EnumGenerator.GlobalTool/)

## Installation

There are two ways to add the nuget package:
1. Run:
```bash
dotnet add package EnumGenerator.Core --version '4.1.*'
```
2. Add the following to a `ItemGroup` section of your csproj:
```xml
<PackageReference Include="EnumGenerator.Core" Version="4.1.*" />
```

## Usage
Steps for generating a enum:
1. Add usings:
```c#
using EnumGenerator.Core.Mapping;
using EnumGenerator.Core.Definition;
using EnumGenerator.Core.Exporter;
```

2. Create a mapping context:
```c#
// Create context object containing all the settings for the mapping.
var context = Context.Create("[*]", "name");
```
Note: To get more diagnostics you can also supply a `Microsoft.Extensions.Logging.ILogger` implementation here.

3. Map the enum:
```c#
var inputJson = File.ReadAllText("input.json");
var enumDefinition = context.MapEnum(inputJson, "TestEnum");
```

4. Export the csharp enum:
```c#
var csharp = enumDefinition.ExportCSharp();
```
or
4. Export the fsharp enum:
```c#
var fsharp = enumDefinition.ExportFSharp(@namespace: "Generated");
```
or
4. Export the vb enum:
```c#
var vb = enumDefinition.ExportVisualBasic();
```
or
4. Export the cil enum:
```c#
var cil = enumDefinition.ExportCil(assemblyName: "TestEnum");
```
or
4. Export a class-library:
```c#
var assemblyFile = enumDefinition.ExportClassLibrary(assemblyName: "TestEnum");
```

## Help
For more general documentation and examples visit the github project [**enum-generator-dotnet**](https://github.com/BastianBlokland/enum-generator-dotnet).
