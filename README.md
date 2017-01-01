# ProjectFileAnalyzer

## Introduction
This program takes a solution file or csproj file as an input and creates two directed graphs of dependcies among all projects.
It does nothing more than that. It just prepares this data structure and lets you apply your graph algorithm on top of it.

The code is written in C# (DOT .NET Core) in Visual Studio Code Editor. 

## How to build it

Run build.sh to build the solution.

## How to run it

Once you build the solution, you shoud have available _ProjectFileAnalyzer.dll_ file under _./src/bin/Debug/netcoreapp1.0/_ folder.
You can run the program by the following command:

```
dotnet ./src/bin/Debug/netcoreapp1.0/ProjectFileAnalyzer.dll [solution-file-or-csproj-file]
```
## How to run tests

* Open root folder in VS Code Editor
* Run task: Clean
* Run task: Build
* Run task: Run Tests 

## Where to implement your graph algorithm

Please refer to this file [Program.cs](https://github.com/pechovic/ProjectFileAnalyzer/blob/master/src/Program.cs) and find section:

```
// ----------------------------------------------------------
// here use the container to explore the graph
// ----------------------------------------------------------
```
Here you are provided with the _container_ variable which is a dictionary indexed by:
* project guid in case of project reference
* assembly name in case of regular reference

Every item in the _container_ dictionary is a type of _Project_ class that has 2 properties:
* References - a dictionary of projects that are being referenced by this project
* ReferencedBy - a dictionary of projects that reference this project
