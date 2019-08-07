# **EnumGenerator** Global tool.

Can be used to generate c# / cil enum files from json files.

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

To generate a cil file use the `-t cil` argument.

## Help
For additional info on the arguments run `enum-generator --help`

For more general documentation and examples visit the github project [**enum-generator-dotnet**](https://github.com/BastianBlokland/enum-generator-dotnet).
