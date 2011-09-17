using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcDojoGrid
{
    internal class ColumnParser
    {
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
