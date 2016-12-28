using Xunit;
using ProjectFileAnalyzer;

namespace ProjectFileAnalyzerTest
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
    }
}