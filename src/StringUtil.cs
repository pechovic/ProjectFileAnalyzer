namespace ProjectFileAnalyzer
{
    /// <summary>
    /// There is a reason behind why this is not regular expression.
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
            return null;
        } 
    }
}