
using System;
using System.IO;
using System.Linq;

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
            if (args.Length != 1 || args.Length != 2)
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

            bool exploreAll = false;
            if (args.Length == 2)
            {
                if (args[1] == "-all")
                {
                    exploreAll = true;
                }
                else
                {
                    throw new Exception("Unknown switch " + args[1]);
                }
            }

            UniqueProjects container = new UniqueProjects();

            if (file.EndsWith("csproj"))
            {
                HandleCsproj(file, container, exploreAll);
            }
            else if (file.EndsWith("sln"))
            {
                HandleSln(file, container, exploreAll);
            }
            else
            {
                throw new Exception("The file must end with csproj or sln.");
            }

            // ----------------------------------------------------------
            // here use the container to explore the graph
            // ----------------------------------------------------------
            
        }

        /// <summary>
        /// Go and analyze csproj file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="container"></param>
        /// <param name="exploreAll"></param>
        /// <returns></returns>
        public Project HandleCsproj(string file, UniqueProjects container, bool exploreAll = false)
        {
            // the newly parsed project will be in the container
            return Project.CreateRecursivelyFromFile(file, container, exploreAll);
        }

        /// <summary>
        /// Go and analyze sln file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="container"></param>
        /// <param name="exploreAll"></param>
        public void HandleSln(string file, UniqueProjects container, bool exploreAll = false)
        {
            string[] lines = File.ReadAllLines(file);
            string fileRoot = Path.GetDirectoryName(file);

            foreach (string line in lines.Where(x => x.StartsWith("Project(\"")))
            {
                string projectPath = line.GetProjectPath();
                if (projectPath == null)
                {
                    Log.WriteWarning("A candidate to carry an information about project file failed to parse. The line: {0}"
                    , line);
                }
                else
                {
                    HandleCsproj(Path.Combine(fileRoot, projectPath.ToUnixPath()), container, exploreAll);
                }
            }
        }

        private static void LogHelp()
        {
            Log.WriteInfo("Usage: 2 arguments: ");
            Log.WriteInfo("  * file path, either csproj file or sln file");
            Log.WriteInfo("  * -all switch to analyze all references (default is only project references.");
        }
    }
}
