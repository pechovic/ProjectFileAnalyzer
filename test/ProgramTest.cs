using Xunit;
using ProjectFileAnalyzer;

namespace ProjectFileAnalyzerTest
{
    public class ProgramTest
    {
        [Fact]
        public void HandleSlnTest_ProjectReferencesOnly()
        {
            // assume that test_data directory is available

            UniqueProjects container = new UniqueProjects();
            ProjectFileAnalyzer.Program p = new ProjectFileAnalyzer.Program();
            p.HandleSln("./test/test_data/NetDb.sln", container);
            
            Assert.Equal(4, container.Count);

            string guidCore = "{6D85E6C7-B860-4ED4-8C3B-C3306BAA18EC}";
            string guidCommands = "{1CD34AF6-7078-45FD-B888-2990402F924A}";
            string guidCommandsTests = "{6E46E7CE-12D2-409A-B350-EB805CA6482F}";
            string guidCoreTests = "{345263A3-E37E-4E97-9422-56AA0879DD64}";

            Project core = container[guidCore];
            Assert.Equal(3, core.ReferencedBy.Count);
            Assert.NotNull(core.ReferencedBy.Contains(container[guidCommands]));
            Assert.NotNull(core.ReferencedBy.Contains(container[guidCommandsTests]));
            Assert.NotNull(core.ReferencedBy.Contains(container[guidCoreTests]));
            Assert.Equal(0, core.References.Count);

            Project coreTests = container[guidCoreTests];
            Assert.Equal(0, coreTests.ReferencedBy.Count);
            Assert.Equal(1, coreTests.References.Count);
            Assert.NotNull(coreTests.References.Contains(container[guidCore]));

            Project commands = container[guidCommands];
            Assert.Equal(1, commands.References.Count);
            Assert.NotNull(commands.References.Contains(container[guidCore]));
            Assert.Equal(1, commands.ReferencedBy.Count);
            Assert.NotNull(commands.ReferencedBy.Contains(container[guidCommandsTests]));

            Project commandsTests = container[guidCommandsTests];
            Assert.Equal(2, commandsTests.References.Count);
            Assert.NotNull(commandsTests.References.Contains(container[guidCore]));
            Assert.NotNull(commandsTests.References.Contains(container[guidCommands]));
            Assert.Equal(0, commandsTests.ReferencedBy.Count);
        }
    }
}