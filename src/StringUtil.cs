using System.Text.RegularExpressions;

namespace ProjectFileAnalyzer
{
    /// <summary>
    /// Some common string tasks.
    /// </summary>
    public static class StringUtil
    {
        /// <summary>
        /// Take a string opened by 'open' and closed by 'close'.
        /// It always takes the first occasion in the string.
        /// </summary>
        /// <param name="open"></param>
        /// <param name="close"></param>
        /// <returns>Returns null if open or close is not present in the input data.</returns>
        public static string TakeOut(this string data, string open, string close)
        {
            string pattern = open + "(.*)" + close;
            Match m = Regex.Match(data, pattern);
            // 2 groups, one for the whole string, second for the middle string
            if (m.Groups.Count == 2)
            {
                return m.Groups[1].Value;
            }

            return null;
        } 
    }
}