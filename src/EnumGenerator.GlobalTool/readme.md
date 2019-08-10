# **EnumGenerator** Global tool.

Can be used to generate c# / f# / vb / cil enum files from json files.

## Installation

Install as a global tool.
```bash
dotnet tool install --global EnumGenerator.GlobalTool
```

## Usage
Invoke the tool using the `enum-generator` alias
```bash
enum-generator -i example.enum -o example.g.cs
```

To generate a fsharp file use the `-t fsharp` argument.
To generate a vb file use the `-t visualbasic` argument.
To generate a cil file use the `-t cil` argument.
To generate a dll file use the `-t classlibrary` argument.

## Help
For additional info on the arguments run `enum-generator --help`

For more general documentation and examples visit the github project [**enum-generator-dotnet**](https://github.com/BastianBlokland/enum-generator-dotnet).
