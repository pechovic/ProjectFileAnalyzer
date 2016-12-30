using System.IO;
using System.Collections.Generic;

namespace ProjectFileAnalyzer
{
    public class Project
    {
        public string FilePath { get; set; }

        /// <summary>
        /// Unique identifier of the project.
        /// Taken from Guid.
        /// </summary>
        /// <returns></returns>
        public string Guid { get; set; }
        public UniqueVertices References { get; set; } = new UniqueVertices();
        public UniqueVertices ReferencedBy {get; set; } = new UniqueVertices();

        /// <summary>
        /// -1 will also serve as WasExplored information
        /// </summary>
        public int PathLength { get; set; } = -1;

        public Project(string value)
        {
            Guid = value;
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
                    projectGuid = content[i].TakeOut("\\<ProjectGuid\\>", "\\</ProjectGuid\\>");
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
                if (line.Contains("ProjectReference") && line.Trim() != "</ProjectReference>")
                {
                    string nextProjectRelativePath = line.TakeOut("Include=\"", "\"");
                    if (nextProjectRelativePath == null)
                    {
                        Log.WriteWarning("{0} - parsing ProjectReference info on the following line failed: {1} ", 
                            projectName,
                            line);
                    }
                    
                    // take a recursive call to explore found file
                    // compose a file path first -> relative to the currently parsed file
                    string nextProjectFullPath = Path.Combine(
                        Path.GetDirectoryName(filePath), 
                        nextProjectRelativePath.ToUnixPath());
                    Project nextProject = CreateRecursivelyFromFile(nextProjectFullPath, container);

                    if (nextProject != null)
                    {
                        p.References.Add(nextProject);
                        nextProject.ReferencedBy.Add(p);
                    }
                }
            }

            return p;
        }

        public override string ToString()
        {
            return Guid.ToString();
        }

    #region UniqueVertices

        public class UniqueVertices : List<Project>
        {
            new public void Add(Project v)
            {
                if (!Contains(v))
                {
                    base.Add(v);
                }
            }
        }
    #endregion

    }

    /// <summary>
    /// Quick access to a Project based on a guid.
    /// </summary>
    public class UniqueProjects : Dictionary<string, Project>
    {
        public void AddSafe(Project p)
        {
            if (!ContainsKey(p.Guid))
            {
                Add(p.Guid, p);
            }    
        }
    }
}
