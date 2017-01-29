using Xunit;
using ProjectFileAnalyzer;

namespace ProjectFileAnalyzer.Test
{
    public class ProgramTest : DataBasedTest
    {
        [Fact]
        public void HandleSlnTest_ProjectReferencesOnly()
        {
            // assume that test_data directory is available

            UniqueProjects container = new UniqueProjects();
            ProjectFileAnalyzer.Program p = new ProjectFileAnalyzer.Program();
            p.HandleSln(TestSlnFile, container);
            
            Assert.Equal(4, container.Count);

            Project core = container[GuidCore];
            Assert.Equal(3, core.ReferencedBy.Count);
            Assert.True(core.ReferencedBy.Contains(container[GuidCommands]));
            Assert.True(core.ReferencedBy.Contains(container[GuidCommandsTests]));
            Assert.True(core.ReferencedBy.Contains(container[GuidCoreTests]));
            Assert.Equal(0, core.References.Count);

            Project coreTests = container[GuidCoreTests];
            Assert.Equal(0, coreTests.ReferencedBy.Count);
            Assert.Equal(1, coreTests.References.Count);
            Assert.True(coreTests.References.Contains(container[GuidCore]));

            Project commands = container[GuidCommands];
            Assert.Equal(1, commands.References.Count);
            Assert.True(commands.References.Contains(container[GuidCore]));
            Assert.Equal(1, commands.ReferencedBy.Count);
            Assert.True(commands.ReferencedBy.Contains(container[GuidCommandsTests]));

            Project commandsTests = container[GuidCommandsTests];
            Assert.Equal(2, commandsTests.References.Count);
            Assert.True(commandsTests.References.Contains(container[GuidCore]));
            Assert.True(commandsTests.References.Contains(container[GuidCommands]));
            Assert.Equal(0, commandsTests.ReferencedBy.Count);
        }

        [Fact]
        public void HandleSlnTest_AllReferences()
        {
            // assume that test_data directory is available

            UniqueProjects container = new UniqueProjects();
            ProjectFileAnalyzer.Program p = new ProjectFileAnalyzer.Program();

            p.HandleSln(TestSlnFile, container, true);

            Project core = container[GuidCore];
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

            Project coreTests = container[GuidCoreTests];
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