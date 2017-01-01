
using System;
using System.IO;

namespace ProjectFileAnalyzer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Program p = new Program();
                p.Run(args);
            } catch (Exception ex)
            {
                Log.WriteError("The program ended on exception: {0}", ex);
                LogHelp();
            }
        }

        private void Run(string[] args)
        {
            if (args.Length != 1)
            {
                throw new Exception("Wrong number of arguments.");
            }

            string file = args[0];

            if (string.Compare(file, "help", true) == 0)
            {
                throw new Exception("Help request.");
            }

            if (!File.Exists(file))
            {
                throw new Exception("File {0} does not exist.");
            }

            UniqueProjects container = new UniqueProjects();

            if (file.EndsWith("csproj"))
            {
                HandleCsproj(file, container);
            }
            else if (file.EndsWith("sln"))
            {
                HandleSln(file, container);
            }
            else
            {
                throw new Exception("The file must end with csproj or sln.");
            }

            // ----------------------------------------------------------
            // here use the container to explore the graph
            // ----------------------------------------------------------
            
        }

        public Project HandleCsproj(string file, UniqueProjects container)
        {
            // the newly parsed project will be in the container
            return Project.CreateRecursivelyFromFile(file, container);
        }

        public void HandleSln(string file, UniqueProjects container)
        {
            string[] lines = File.ReadAllLines(file);
            string fileRoot = Path.GetDirectoryName(file);

            foreach (string line in lines)
            {
                if (line.StartsWith("Project(\""))
                {
                    string projectPath = line.GetProjectPath();
                    if (projectPath == null)
                    {
                        Log.WriteWarning("A candidate to carry an information about project file failed to parse. The line: {0}"
                        , line);
                    }
                    else
                    {
                        HandleCsproj(Path.Combine(fileRoot, projectPath.ToUnixPath()), container);
                    }
                }
            }
        }

        private static void LogHelp()
        {
            Log.WriteInfo("Usage: ");
        }

    }
}
