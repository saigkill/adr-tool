# adr-cli
A command-line tool for working with Architecture Decision Records (ADRs). This is a fork of the original [adr-cli](https://github.com/GingerTommy/adr-cli/edit/master/README.md) just delivered as dotnet tool.

## Badges

|What|Status|
|---|---|
|Language|C#|
|Framework|.NET 8 |
|Continuous Integration Prod | [![Build status](https://dev.azure.com/saigkill/AdrTool/_apis/build/status/AdrTool-ASP.NET%20Core-CI-Prod)](https://dev.azure.com/saigkill/AdrTool/_build/latest?definitionId=68)|
|Continuous Integration Stage | [![Build status](https://dev.azure.com/saigkill/AdrTool/_apis/build/status/AdrTool-.NET%20Desktop-CI)](https://dev.azure.com/saigkill/AdrTool/_build/latest?definitionId=67) |
|Bugreports|[![GitHub issues](https://img.shields.io/github/issues/saigkill/adr-tool)](https://github.com/saigkill/adr-tool/issues)
|Bugreports|[![Board Status](https://dev.azure.com/saigkill/d45d65b6-85d0-4829-a3b1-e6794b2ec791/dfc2a578-bbce-40ef-8f27-a27be1669c61/_apis/work/boardbadge/e630e7d7-d883-4899-bcb7-b86f22e09010)](https://dev.azure.com/saigkill/d45d65b6-85d0-4829-a3b1-e6794b2ec791/_boards/board/t/dfc2a578-bbce-40ef-8f27-a27be1669c61/Stories/)|
|Blog|[![Blog](https://img.shields.io/badge/Blog-Saigkill-blue)](https://saschamanns.de)|

File a bug report [on Github](https://github.com/saigkill/adr-tool/issues?q=sort%3Aupdated-desc+is%3Aissue+is%3Aopen).

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