# Resource Usage Documentation

## Introduction
This document explains the structure and usage of the .NET Core template project created using the BBT.Prism SDK. This template includes all the necessary configurations and structure for developers to get started quickly.

## Folder Structure

The project folder structure is as follows:

```
.vscode
etc
  ????????? dapr
      ????????? components
      ????????? config.yaml
  ????????? docker
      ????????? config
      ????????? zeebe-exporters
      ????????? docker-compose.yml
src
  ????????? BBT.Resource.Application
  ????????? BBT.Resource.Application.Contracts
  ????????? BBT.Resource.Domain
  ????????? BBT.Resource.Domain.Shared
  ????????? BBT.Resource.EntityFrameworkCore
  ????????? BBT.Resource.HttpApi.Host
test
  ????????? BBT.Resource.Application.Tests
  ????????? BBT.Resource.Domain.Tests
  ????????? BBT.Resource.EntityFrameworkCore.Tests
  ????????? BBT.Resource.TestBase
.gitattributes
.gitignore
.prettierrc
BBT.Resource.sln
BBT.Resource.sln.DotSettings
common.props
delete-bin-obj.ps1
global.json
NuGet.Config
run.ps1
```

### .vscode
Contains configuration files for Visual Studio Code.

### etc
Contains configuration files.
- **dapr**: Contains Dapr components and configuration files.
  - `components`: Dapr component configurations.
  - `config.yaml`: Dapr general configuration file.
- **docker**: Contains Docker configuration files.
  - `config`: Docker configurations.
  - `zeebe-exporters`: Zeebe Exporters configurations.
  - `docker-compose.yml`: Docker Compose configuration file.

### src
Contains the application source code.
- **BBT.Resource.Application**: Application layer.
- **BBT.Resource.Application.Contracts**: Contracts for the application layer.
- **BBT.Resource.Domain**: Domain layer.
- **BBT.Resource.Domain.Shared**: Shared components for the domain layer.
- **BBT.Resource.EntityFrameworkCore**: Entity Framework Core configurations.
- **BBT.Resource.HttpApi.Host**: HTTP API Host.

### test
Contains test projects.
- **BBT.Resource.Application.Tests**: Application layer tests.
- **BBT.Resource.Domain.Tests**: Domain layer tests.
- **BBT.Resource.EntityFrameworkCore.Tests**: Entity Framework Core tests.
- **BBT.Resource.TestBase**: Base classes for tests.

### Other Files
- **.gitattributes**: Specifies Git attributes.
- **.gitignore**: Specifies files to be ignored by Git.
- **.prettierrc**: Prettier configuration file.
- **BBT.Resource.sln**: Visual Studio solution file.
- **BBT.Resource.sln.DotSettings**: Visual Studio settings file.
- **common.props**: Contains common project properties.
- **delete-bin-obj.ps1**: PowerShell script to clean `bin` and `obj` folders.
- **global.json**: Specifies the .NET SDK version.
- **NuGet.Config**: Contains NuGet sources and settings.
- **run.ps1**: Script to run the project.

---

## Required Tools and Setup Steps

The following tools need to be installed to build and run the project:

1. **Docker**
2. **.NET Core 8.0 SDK**

## Building and Running the Project

### Building the Project
To build the project, run the following commands in order:

```sh
dotnet restore
dotnet build
```

### Running Docker and Dapr
You can run Docker and Dapr using the `run.ps1` script located in the root directory. This script contains different terminal commands for Windows and Mac.

#### Windows
To run the script on Windows using PowerShell, execute the following command:

```powershell
.\run.ps1
```

#### Mac
To run the script on MacOS, you need to install PowerShell. You can find the official documentation for installing PowerShell on MacOS [here](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-macos).

After the installation is complete, you can run the script in the terminal with the following command:

```sh
pwsh ./run.ps1
```
---