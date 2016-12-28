
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

            if (file.EndsWith("csproj"))
            {
                HandleCsproj(file);
            }
            else if (file.EndsWith("sln"))
            {
                HandleSln(file);
            }
            else
            {
                throw new Exception("The file must end with csproj or sln.");
            }
        }

        private void HandleCsproj(string file)
        {
            UniqueProjects container = new UniqueProjects();
            Project p = Project.CreateRecursivelyFromFile(file, container);

            Log.WriteInfo("Number of projects found: {0}", container.Count);
        }

        private void HandleSln(string file)
        {
            throw new NotImplementedException();
        }

        private static void LogHelp()
        {
            Log.WriteInfo("Usage: ");
        }

    }
}
