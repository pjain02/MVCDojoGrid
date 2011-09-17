using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcDojoGrid
{
    internal class ColumnParser
    {
        /// <summary>
        /// Parses the text to break them up based on '_' and capitals
        /// </summary>
        /// <param name="text">The text to be parsed</param>
        /// <returns>An array of strings (Broken up words in the text)</returns>
        internal string[] Parse(string text)
        {
            if (text.Length == 0)
                return null;

            List<string> words = new List<string>();
            //Rule1 - Remove UnderScore
            string[] temp = text.Split('_');


            //Rule2 - Seperate words based on Capitals
            foreach (string s in temp)
            {
                if (s == "ID")  //Special case where the word is ID, its left out
                { 
                    words.Add(s);
                    continue;
                }
                
                bool wordBreak;
                int wordStart = 0;

                if (s.ToLower()[0] == s[0])
                    wordBreak = false;
                else
                    wordBreak = true;

                for (int i = 1; i < s.Length; i++)
                {
                    if ((s.ToUpper()[i] == s[i]) && (wordBreak == true) && (s.ToUpper()[i-1] != s[i-1]))
                    {
                        words.Add(s.Substring(wordStart, i - wordStart));
                        wordStart = i;
                        wordBreak = true;
                    }
                    else if ((s.ToUpper()[i] == s[i]) && (wordBreak == false))
                        wordBreak = true;
                }
                words.Add(s.Substring(wordStart, s.Length - wordStart));
            }
            return words.ToArray();
        }

        /// <summary>
        /// Seperates the column name in to individual words based on parser rules
        /// </summary>
        /// <param name="text">Column Name to be parsed</param>
        /// <returns>Column Name after the rules are applied</returns>
        internal string ColumnName(string text)
        {
            string[] words = Parse(text);
            if (words == null)
                return "";
            StringBuilder name = new StringBuilder();
            foreach (string word in words)
            {
                name.Append(word + " ");
            }
            return name.ToString().TrimEnd(' ');
        }
    }
}
