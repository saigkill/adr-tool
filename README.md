# adr-cli
A command-line tool for working with Architecture Decision Records (ADRs). This is a fork of the original [adr-cli](https://github.com/GingerTommy/adr-cli/edit/master/README.md) just delivered as dotnet tool.

## Badges

|What|Status|
|---|---|
|Language|C#|
|Framework|.NET 8 |
|Continuous Integration Prod | [![Build status](https://dev.azure.com/saigkill/Saigkill.Toolbox/_apis/build/status/Saigkill.Toolbox-.NET%20Desktop-CI-Prod)](https://dev.azure.com/saigkill/Saigkill.Toolbox/_build/latest?definitionId=65)|
|Continuous Integration Stage | [![Build status](https://dev.azure.com/saigkill/Saigkill.Toolbox/_apis/build/status/Saigkill.Toolbox-.NET%20Desktop-CI-Stage)](https://dev.azure.com/saigkill/Saigkill.Toolbox/_build/latest?definitionId=66) |
|Code Coverage|[![Coverage Status](https://coveralls.io/repos/github/saigkill/SaschasToolbox/badge.svg?branch=master)](https://coveralls.io/github/saigkill/SaschasToolbox?branch=master)
|Bugreports|[![GitHub issues](https://img.shields.io/github/issues/saigkill/SaschasToolbox)](https://github.com/saigkill/SaigkillsToolbox/issues)
|Bugreports|[![Board Status](https://dev.azure.com/saigkill/820066de-bb64-4006-87d1-70ca26310c2f/2988b49e-078f-47a8-810c-f179fa8efa81/_apis/work/boardbadge/745fc052-256a-4941-9d95-ee0e344b0563)](https://dev.azure.com/saigkill/820066de-bb64-4006-87d1-70ca26310c2f/_boards/board/t/2988b49e-078f-47a8-810c-f179fa8efa81/Stories/)|
|Blog|[![Blog](https://img.shields.io/badge/Blog-Saigkill-blue)](https://saschamanns.de)|

File a bug report [on Github](https://github.com/saigkill/SaigkillsToolbox/issues?q=sort%3Aupdated-desc+is%3Aissue+is%3Aopen).

File a bug report [on Azure DevOps](https://dev.azure.com/saigkill/AdrTool/_workitems/recentlyupdated/).

## Installation

To install the tool, run the following command in the powershell or cmd:
```powershell
dotnet tool install --global Saigkill.AdrTool.CLI
```

## Usage

To initialize the ADR directory, run the following command in the powershell or cmd:
```powershell
adr-tool init
```

To create a new ADR, run the following command in the powershell or cmd:
```powershell
adr-tool new "Title of the ADR"
```

To list all ADRs, run the following command in the powershell or cmd:
```powershell
adr-tool list
```

To supersede an ADR, run the following command in the powershell or cmd:
```powershell
adr-tool new "Title of the ADR" -s <ADR number>
```

This will create a new ADR file in the `docs/adr` directory with the title as the filename. Then just open and edit it in any editor or IDE.