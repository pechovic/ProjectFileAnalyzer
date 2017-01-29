using Xunit;
using ProjectFileAnalyzer;
using System.Linq;

namespace ProjectFileAnalyzer.Test
{
    public class ProjectTest : DataBasedTest
    {
        [Fact]
        public void IsExeTest()
        {
            UniqueProjects container = new UniqueProjects();
            ProjectFileAnalyzer.Program p = new ProjectFileAnalyzer.Program();
            p.HandleSln(TestSlnFile, container);

            var exeProjects = container.Where(x => x.Value.IsExe).ToArray();
            Assert.Equal(0, exeProjects.Length);
        }
    }
}