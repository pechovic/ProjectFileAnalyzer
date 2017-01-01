using Xunit;
using ProjectFileAnalyzer;

namespace ProjectFileAnalyzerTest
{
    public class ProgramTest
    {

        private const string _testSlnFile = "./test_data/NetDb.sln";

        private const string _guidCore = "{6D85E6C7-B860-4ED4-8C3B-C3306BAA18EC}";
        private const string _guidCommands = "{1CD34AF6-7078-45FD-B888-2990402F924A}";
        private const string _guidCommandsTests = "{6E46E7CE-12D2-409A-B350-EB805CA6482F}";
        private const string _guidCoreTests = "{345263A3-E37E-4E97-9422-56AA0879DD64}";

        [Fact]
        public void HandleSlnTest_ProjectReferencesOnly()
        {
            // assume that test_data directory is available

            UniqueProjects container = new UniqueProjects();
            ProjectFileAnalyzer.Program p = new ProjectFileAnalyzer.Program();
            p.HandleSln(_testSlnFile, container);
            
            Assert.Equal(4, container.Count);

            Project core = container[_guidCore];
            Assert.Equal(3, core.ReferencedBy.Count);
            Assert.True(core.ReferencedBy.Contains(container[_guidCommands]));
            Assert.True(core.ReferencedBy.Contains(container[_guidCommandsTests]));
            Assert.True(core.ReferencedBy.Contains(container[_guidCoreTests]));
            Assert.Equal(0, core.References.Count);

            Project coreTests = container[_guidCoreTests];
            Assert.Equal(0, coreTests.ReferencedBy.Count);
            Assert.Equal(1, coreTests.References.Count);
            Assert.True(coreTests.References.Contains(container[_guidCore]));

            Project commands = container[_guidCommands];
            Assert.Equal(1, commands.References.Count);
            Assert.True(commands.References.Contains(container[_guidCore]));
            Assert.Equal(1, commands.ReferencedBy.Count);
            Assert.True(commands.ReferencedBy.Contains(container[_guidCommandsTests]));

            Project commandsTests = container[_guidCommandsTests];
            Assert.Equal(2, commandsTests.References.Count);
            Assert.True(commandsTests.References.Contains(container[_guidCore]));
            Assert.True(commandsTests.References.Contains(container[_guidCommands]));
            Assert.Equal(0, commandsTests.ReferencedBy.Count);
        }

        [Fact]
        public void HandleSlnTest_AllReferences()
        {
            // assume that test_data directory is available

            UniqueProjects container = new UniqueProjects();
            ProjectFileAnalyzer.Program p = new ProjectFileAnalyzer.Program();

            p.HandleSln(_testSlnFile, container, true);

            Project core = container[_guidCore];
            Assert.Equal(3, core.ReferencedBy.Count);
            Assert.Equal(9, core.References.Count);
            Assert.True(core.References.Contains(container["System"]));
            Assert.True(core.References.Contains(container["System.Configuration"]));
            Assert.True(core.References.Contains(container["System.Core"]));
            Assert.True(core.References.Contains(container["System.Xml.Linq"]));
            Assert.True(core.References.Contains(container["System.Data.DataSetExtensions"]));
            Assert.True(core.References.Contains(container["Microsoft.CSharp"]));
            Assert.True(core.References.Contains(container["System.Data"]));
            Assert.True(core.References.Contains(container["System.Xml"]));

            Project coreTests = container[_guidCoreTests];
            Assert.Equal(0, coreTests.ReferencedBy.Count);
            Assert.Equal(11, coreTests.References.Count);
            Assert.True(coreTests.References.Contains(container["log4net"]));
            Assert.True(coreTests.References.Contains(container["Microsoft.CSharp"]));
            Assert.True(coreTests.References.Contains(container["System"]));
            Assert.True(coreTests.References.Contains(container["System.Data"]));
            Assert.True(coreTests.References.Contains(container["System.XML"]));
            Assert.True(coreTests.References.Contains(container["Microsoft.VisualStudio.QualityTools.CodedUITestFramework"]));
            Assert.True(coreTests.References.Contains(container["Microsoft.VisualStudio.TestTools.UITest.Common"]));
            Assert.True(coreTests.References.Contains(container["Microsoft.VisualStudio.TestTools.UITest.Extension"]));
            Assert.True(coreTests.References.Contains(container["Microsoft.VisualStudio.TestTools.UITesting"]));
        }
    }
}