using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ProjectFileAnalyzer
{
    public class Project
    {
        public string Name { get; set; }

        /// <summary>
        /// Unique identifier of the project.
        /// Taken from Guid.
        /// </summary>
        /// <returns></returns>
        public string Guid { get; set; }
        public string CsProjPath { get; set; }
        public bool IsExe { get; set; }
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

        public static Project CreateRecursivelyFromFile(string filePath, UniqueProjects container, bool exploreAll) 
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
                    break;
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
                return container[projectGuid];
            }

            // find OutputType
            bool isExe = false;
            for (int i = 2; i < 30; i++)
            {
                if (content[i].Contains("OutputType"))
                {
                    isExe = content[i].TakeOut("\\<OutputType\\>", "\\</OutputType\\>").ToLower() == "exe";
                }
            }

            // Project is new, not explored, so create a new instance
            Project p = new Project(projectGuid);
            p.Name = projectName;
            p.CsProjPath = filePath;
            p.IsExe = isExe;

            // add it to the container now so that everyone knows it's being explored
            container.Add(projectGuid, p);

            foreach (var line in content)
            {
                if (line.Contains("<ProjectReference") && line.Trim() != "</ProjectReference>")
                {
                    string nextProjectRelativePath = line.TakeOut("Include=\"", "\"");
                    if (nextProjectRelativePath == null)
                    {
                        Log.WriteWarning("{0} - parsing ProjectReference info on the following line failed: {1} ", 
                            projectName,
                            line);
                    }
                    else
                    {
                        // take a recursive call to explore found file
                        // compose a file path first -> relative to the currently parsed file
                        string nextProjectFullPath = Path.GetFullPath(Path.Combine(
                            Path.GetDirectoryName(filePath), 
                        nextProjectRelativePath.ToUnixPath()));
                        Project nextProject = CreateRecursivelyFromFile(nextProjectFullPath, container, exploreAll);

                        if (nextProject != null)
                        {
                            p.References.Add(nextProject);
                            nextProject.ReferencedBy.Add(p);
                        }
                    }
                }
                else if (exploreAll && line.Contains("<Reference") && line.Trim() != "</Reference>")
                {
                    string name = line.TakeOut("Include=\"", "\"");
                    if (name == null)
                    {
                        Log.WriteWarning("{0} - parsing Reference info on the following line failed: {1} ", 
                            projectName,
                            line);
                    }
                    else
                    {
                        if (name.Contains(","))
                        {
                            name = name.Split(',')[0];
                        }

                        Project nextProject;
                        if (container.ContainsKey(name))
                        {
                            nextProject = container[name];
                        }
                        else 
                        {
                            // parsing assembly reference data
                            nextProject = new Project(name);
                            nextProject.Name = name;
                            nextProject.IsExe = name.EndsWith(".exe");
                            container.Add(name, nextProject);
                        }

                        p.References.Add(nextProject);
                        nextProject.ReferencedBy.Add(p);
                    }
                }
            }

            return p;
        }

        public List<Project> GetReferenciesRecursively()
        {
            return GetDirectAndIndirectDependencies(false);
        }

        public List<Project> GetReferencedByRecursively()
        {
            return GetDirectAndIndirectDependencies(true);
        }

        private List<Project> GetDirectAndIndirectDependencies(bool referencedBy)
        {
            var result = new List<Project>();
            foreach (Project project in referencedBy ? ReferencedBy : References)
            {
                Queue<Project> q = new Queue<Project>();
                project.PathLength = 0;
                q.Enqueue(project);

                while (q.Any())
                {
                    Project current = q.Dequeue();
                    result.Add(current);

                    foreach (Project p in referencedBy ? ReferencedBy : References)
                    {
                        if (p.PathLength < 0)
                        {
                            p.PathLength = current.PathLength + 1;
                            q.Enqueue(p);
                        }
                    }
                }
            }

            return result;
        }

        public override string ToString()
        {
            return Name + " - " + Guid.ToString();
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
