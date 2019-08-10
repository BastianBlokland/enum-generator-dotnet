# **EnumGenerator** Cli tool.

Can be used to generate c# / f# / cil enum files from json files.

## Installation

Add a reference to the cli-tool to a `ItemGroup` section your of your csproj.
```xml
<DotNetCliToolReference Include="EnumGenerator.Cli" Version="3.5.*" />
```

## Usage
There two ways to invoke the tool:
1. Manually execute `dotnet enum-generator` from a command prompt.
2. Add a pre-build step to your csproj file: (Outside of a `ItemGroup` or `PropertyGroup`)
```xml
<Target Name="GenerateEnum" BeforeTargets="BeforeBuild">
    <Exec Command="dotnet enum-generator \
    -i example.json \
    -o example.g.cs"/>
</Target>
```

To generate a fsharp file use the `-t fsharp` argument.
To generate a cil file use the `-t cil` argument.
To generate a dll file use the `-t classlibrary` argument.

## Help
For additional info on the arguments run `dotnet enum-generator --help`

For more general documentation and examples visit the github project [**enum-generator-dotnet**](https://github.com/BastianBlokland/enum-generator-dotnet).
