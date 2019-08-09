# EnumGenerator-Dotnet

[![Build](https://img.shields.io/azure-devops/build/bastian-blokland/EnumGenerator/5/master.svg)](https://dev.azure.com/bastian-blokland/EnumGenerator/_build/latest?definitionId=5&branchName=master)
[![Tests](https://img.shields.io/azure-devops/tests/bastian-blokland/EnumGenerator/5/master.svg)](https://dev.azure.com/bastian-blokland/EnumGenerator/_build/latest?definitionId=5&branchName=master)
[![codecov](https://codecov.io/gh/BastianBlokland/enum-generator-dotnet/branch/master/graph/badge.svg)](https://codecov.io/gh/BastianBlokland/enum-generator-dotnet)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

| Cli tool | Global tool | Core library |
|----------|-------------|--------------|
| [![NuGet](https://img.shields.io/nuget/v/EnumGenerator.Cli.svg)](https://www.nuget.org/packages/EnumGenerator.Cli/) | [![NuGet](https://img.shields.io/nuget/v/EnumGenerator.GlobalTool.svg)](https://www.nuget.org/packages/EnumGenerator.GlobalTool/) | [![NuGet](https://img.shields.io/nuget/v/EnumGenerator.Core.svg)](https://www.nuget.org/packages/EnumGenerator.Core/) |

Dotnet cli tool for generating c# / f# / cil enums from json files.

## Description
If you have config in json files it can be nice to have a enum to reference in the code instead of
having to hard code values, this tool allows you to generate that enum.

## Usage
There are 4 different ways to use the generator:

| Usecase | Project | Documentation |
|---------|---------|---------------|
| Build integration | [**Cli**](https://www.nuget.org/packages/EnumGenerator.Cli/) | [Cli Readme](https://github.com/BastianBlokland/enum-generator-dotnet/tree/master/src/EnumGenerator.Cli/readme.md) |
| Command line | [**GlobalTool**](https://www.nuget.org/packages/EnumGenerator.GlobalTool/) | [GlobalTool Readme](https://github.com/BastianBlokland/enum-generator-dotnet/tree/master/src/EnumGenerator.GlobalTool/readme.md) |
| Manual library integration | [**Core**](https://www.nuget.org/packages/EnumGenerator.Core/) | [Core Readme](https://github.com/BastianBlokland/enum-generator-dotnet/tree/master/src/EnumGenerator.Core/readme.md) |
| Unity3D package | [**UnityPackage**](https://github.com/BastianBlokland/enum-generator-unity) | [UnityPackage Readme](https://github.com/BastianBlokland/enum-generator-unity/blob/master/README.md) |

## Json file structure
To be able to handle many different file structures the generator takes in a number of [**JsonPath**](https://goessner.net/articles/JsonPath/) entries:

| Argument | Usage |
|----------|-------|
| collection | Path to the main collection in the json |
| entryname | Path to the string name of a single entry in the above collection |
| entryvalue | Path to the number value of a single entry in the above collection |
| entrycomment | Path to the string comment of a single entry in the above collection |

Note: If no `entryvalue` is provided the index in the collection will be used as the value.

Here's a couple structure examples:
* *Outer array*

  json:
  ```json
  [
    {
      "name": "A",
      "value": 1
    },
    {
      "name": "B",
      "value": 2
    },
  ]
  ```
  options:
  ```javascript
  collection = "[*]"
  entryname = "name"
  entryvalue = "value"
  ```

* *Inner array*

  json:
  ```json
  {
    "entries": [
        "A",
        "B"
    ]
  }
  ```
  options:
  ```javascript
  collection = "entries[*]"
  entryname = "$"
  ```

* *Inner object*

  json:
  ```json
  {
    "entries": [
      {
        "info": {
          "name": "A"
        },
        "value": 10
      },
      {
        "info": {
          "name": "B"
        },
        "value": 20
      }
    ]
  }
  ```
  options:
  ```javascript
  collection = "entries[*]"
  entryname = "info.name"
  entryvalue = "value"
  ```
* *Deep search*

  json:
  ```json
  {
    "collection1": {
      "entries": [
        {
          "name": "A"
        },
        {
          "name": "B"
        }
      ]
    },
    "collection2": {
      "entries": [
        {
          "name": "C"
        },
        {
          "name": "D"
        }
      ]
    }
  }
  ```
  options:
  ```javascript
  collection = "..entries[*]"
  entryname = "name"
  entryvalue = "value"
  ```

* *Filtering*

  json:
  ```json
  [
    {
      "name": "A",
      "value": 1,
      "active": false
    },
    {
      "name": "B",
      "value": 2,
      "active": true
    },
    {
      "name": "C",
      "value": 3,
      "active": true
    },
    {
      "name": "D",
      "value": 4,
      "active": false
    }
  ]
  ```
  options:
  ```javascript
  collection = "[?(@.active == true)]"
  entryname = "name"
  entryvalue = "value"
  ```

Note: Enable `verbose` logging to get more output about what the mapper is doing.

## Integration example
An example can be found in the [**example**](https://github.com/BastianBlokland/enum-generator-dotnet/tree/master/example) directory.
