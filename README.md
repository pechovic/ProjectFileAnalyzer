# ProjectFileAnalyzer

## Introduction
This program takes a solution file or csproj file as an input and creates two directed graphs of dependcies among all projects.
It does nothing more than that. It just prepares this data structure and lets you apply your graph algorithm on top of it.

The code is written in C# (.NET Core) in Visual Studio Code Editor. 

## How to build it and run it

To build the main application and tests run the following command or use build task from VS Code Editor
```
dotnet build ./ProjectFileAnalyzer/project.json ProjectFileAnalyzer.Test/project.json
```

Once you build the solution, you shoud have available _ProjectFileAnalyzer.dll_ file under _./ProjectFileAnalyzer/bin/Debug/netcoreapp1.0/_ folder.
You can run the program by the following command:

```
dotnet ./src/bin/Debug/netcoreapp1.1/ProjectFileAnalyzer.dll solution-file-or-csproj-file [-all]
```

To run tests, execute:
```
dotnet test ./ProjectFileAnalyzer.Test/project.json
```
or use test task from VS Code Editor

## Where to implement your graph algorithm

Please refer to this file [Program.cs](https://github.com/pechovic/ProjectFileAnalyzer/blob/master/ProjectFileAnalyzer/Program.cs) and find section:

```
// ----------------------------------------------------------
// here use the container to explore the graph
// ----------------------------------------------------------
```
Here you are provided with the _container_ variable which is a dictionary indexed by:
* project guid in case of project reference
* assembly name in case of regular reference

Every item in the _container_ dictionary is a type of _Project_ class that has 2 properties:
* References - a list of projects that are being referenced by this project
* ReferencedBy - a list of projects that reference this project