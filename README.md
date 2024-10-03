# adr-cli
A command-line tool for working with Architecture Decision Records (ADRs). This is a fork of the original [adr-cli](https://github.com/GingerTommy/adr-cli/edit/master/README.md) just delivered as dotnet tool.

## Badges

|What|Status|
|---|---|
|Language|C#|
|Framework|.NET 8 |
|Continuous Integration Prod | [![Build status](https://dev.azure.com/saigkill/AdrTool/_apis/build/status/AdrTool-ASP.NET%20Core-CI-Prod)](https://dev.azure.com/saigkill/AdrTool/_build/latest?definitionId=68)|
|Continuous Integration Stage | [![Build status](https://dev.azure.com/saigkill/AdrTool/_apis/build/status/AdrTool-.NET%20Desktop-CI)](https://dev.azure.com/saigkill/AdrTool/_build/latest?definitionId=67) |
|Deployment Prod | [![Deployment status](https://vsrm.dev.azure.com/saigkill/_apis/public/Release/badge/d45d65b6-85d0-4829-a3b1-e6794b2ec791/2/2)](https://dev.azure.com/saigkill/AdrTool/_release?_a=releases&view=mine&definitionId=2) |
|Deployment Stage | [![Deployment status](https://vsrm.dev.azure.com/saigkill/_apis/public/Release/badge/d45d65b6-85d0-4829-a3b1-e6794b2ec791/1/1)](https://dev.azure.com/saigkill/AdrTool/_release?definitionId=1&view=mine&_a=releases) |
|Code Coverage | [![Coverage](https://img.shields.io/azure-devops/coverage/saigkill/AdrTool/68)](https://dev.azure.com/saigkill/AdrTool/_build/latest?definitionId=68) |
|Bugreports|[![GitHub issues](https://img.shields.io/github/issues/saigkill/adr-tool)](https://github.com/saigkill/adr-tool/issues)
|Bugreports|[![Board Status](https://dev.azure.com/saigkill/d45d65b6-85d0-4829-a3b1-e6794b2ec791/dfc2a578-bbce-40ef-8f27-a27be1669c61/_apis/work/boardbadge/e630e7d7-d883-4899-bcb7-b86f22e09010)](https://dev.azure.com/saigkill/d45d65b6-85d0-4829-a3b1-e6794b2ec791/_boards/board/t/dfc2a578-bbce-40ef-8f27-a27be1669c61/Stories/)|
|Blog|[![Blog](https://img.shields.io/badge/Blog-Saigkill-blue)](https://saschamanns.de)|

|Name|Status|Version|
|---|---|---|
|Saigkill.adr-tool.CLI| ![Nuget Downloads](https://img.shields.io/nuget/dt/Saigkill.adr-tool.CLI) | ![Nuget Version](https://img.shields.io/nuget/v/Saigkill.adr-tool.CLI) |


File a bug report [on Github](https://github.com/saigkill/adr-tool/issues?q=sort%3Aupdated-desc+is%3Aissue+is%3Aopen).

File a bug report [on Azure DevOps](https://dev.azure.com/saigkill/AdrTool/_workitems/recentlyupdated/).

## Deployment

The deployment is done by Azure DevOps.
The development branch is deployed to [Azure Artifacts Nuget Feed](https://pkgs.dev.azure.com/saigkill/AdrTool/_packaging/SaigkillsAdrFeed/nuget/v3/index.json).
The master branch is deployed to [NuGet.org](https://www.nuget.org/packages/Saigkill.adr-tool.CLI/).

## Installation

Alternate to the NuGet.org packaage, you can use the Azure Feed: https://pkgs.dev.azure.com/saigkill/AdrTool/_packaging/SaigkillsAdrFeed/nuget/v3/index.json

To install the tool, run the following command in the powershell or cmd:
```powershell
dotnet tool install --global Saigkill.adr-tool.CLI
```

## Usage

To initialize the ADR directory, run the following command in the powershell or cmd:
```powershell
adr init
```

To create a new ADR, run the following command in the powershell or cmd:
```powershell
adr new "Title of the ADR"
```

To list all ADRs, run the following command in the powershell or cmd:
```powershell
adr list
```


This will create a new ADR file in the `docs/adr` directory with the title as the filename. It will openened in the default editor.