# Getting Started

First, ensure you have the `adr-tools` installed. You can install it using the following command in your terminal or command prompt:
```powershell
dotnet tool install --global Saigkill.adr-tool.CLI
```

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