using System.IO;
using System.Collections.Generic;

namespace ProjectFileAnalyzer
{
    public class Project : Vertex<string>
    {
        public string FilePath { get; set; }

        public Project(string value) : base(value)
        {
        }

        public static Project CreateRecursivelyFromFile(string filePath, UniqueProjects container) 
        {
            Log.WriteInfo("Starting to explore project file: {0}", filePath);

            if (!File.Exists(filePath))
            {
                Log.WriteWarning("Can't process file {0} because it doesn't exist.", filePath);
                return null;
            }

            string projectName = Path.GetFileName(filePath);

            var content = File.ReadAllLines(filePath);
            string projectGuid = "";            
            // read guid first and end in case this was already explored
            for (int i = 2; i < 30; i++)
            {
                if (content[i].Contains("ProjectGuid"))
                {
                    projectGuid = content[i].TakeOut("<ProjectGuid>", "</ProjectGuid>");
                }
            }

            // if guid not found then this file is corrupted
            if (projectGuid == "")
            {
                Log.WriteWarning("ProjectGuid not found in file: {0}", projectName);
                return null;
            }

            if (container.ContainsKey(projectGuid))
            {
                Log.WriteInfo("Skipping project {0} because it has been already explored.", projectName);
                return null;
            }

            // Project is new, not explored, so create a new instance
            Project p = new Project(projectGuid);

            // add it to the container now so that everyone knows it's being explored
            container.Add(projectGuid, p);

            foreach (var line in content)
            {
                if (line.Contains("ProjectReference") && line != "</ProjectReference>")
                {
                    string nextProjectRelativePath = line.TakeOut("Include=\"=", "\"");
                    if (nextProjectRelativePath == null)
                    {
                        Log.WriteWarning("{0} - parsing ProjectReference info on the following line failed: {1} ", 
                            projectName,
                            line);
                    }
                    
                    // take a recursive call to explore found file
                    // compose a file path first -> relative to the currently parsed file
                    string nextProjectFullPath = Path.Combine(Path.GetDirectoryName(filePath), nextProjectRelativePath);
                    Project nextProject = CreateRecursivelyFromFile(nextProjectFullPath, container);

                    p.AdjacentVertices.Add(nextProject);
                }
            }

            return p;
        }
    }

    /// <summary>
    /// Quick access to a Project based on guid.
    /// </summary>
    public class UniqueProjects : Dictionary<string, Project>
    {
        public void AddSafe(Project p)
        {
            if (!ContainsKey(p.Value))
            {
                Add(p.Value, p);
            }    
        }
    }
}