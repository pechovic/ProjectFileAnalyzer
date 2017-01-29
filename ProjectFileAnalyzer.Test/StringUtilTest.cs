using Xunit;
using ProjectFileAnalyzer;

namespace ProjectFileAnalyzer.Test
{
    public class StringUtilTest
    {

        [Fact]
        public void TakeOut_SimpleTest()
        {
            string data = "this is some string that will be used as input";
            string open = "string ";
            string close = " will";
            string expected = "that";

            string actual = data.TakeOut(open, close);

            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TakeOut_TextWithBracketsTest()
        {
            string data = "will be bla bla used this is some string that (will be used) as input";
            string open = "\\(will ";
            string close = " used\\)";
            string expected = "be";

            string actual = data.TakeOut(open, close);

            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TakeOut_TextWithXmlTagsTest()
        {
            string data = "<ProjectGuid>Hello</ProjectGuid>";
            string open = "\\<ProjectGuid\\>";
            string close = "\\</ProjectGuid\\>";
            string expected = "Hello";

            string actual = data.TakeOut(open, close);

            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetProjectPath_Test()
        {
            string dataLine = "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"NetDb.Core\", \"NetDb.Core\\NetDb.Core.csproj\", \"{6D85E6C7-B860-4ED4-8C3B-C3306BAA18EC}\"";
            string actual = dataLine.GetProjectPath();
            string expected = "NetDb.Core\\NetDb.Core.csproj";

            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }
    }
}