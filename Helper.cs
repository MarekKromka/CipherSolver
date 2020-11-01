using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VigenereCipherSolver;

namespace CiphersSolvers
{
    public class Helper
    {
        public static List<string> SplitText(string text, int keyLength)
        {
            string withoutSpaces = CleanText(text);

            var list = new List<string>();
            for (int i = 0; i < keyLength; i++)
            {
                list.Add(new string(EveryNthChar(withoutSpaces, keyLength).ToArray()));
                withoutSpaces = withoutSpaces.Remove(0, 1);
            }

            return list;
        }

        public static int[] CountChars(string text)
        {
            var upper = text.ToUpper();
            var multiplicities = new int[26];

            foreach (var c in upper.Where(c => c >= 'A' && c <= 'Z'))
            {
                multiplicities[c - 'A']++;
            }

            return multiplicities;
        }

        public static IEnumerable<char> EveryNthChar(string word, int n)
        {
            for (int i = 0; i < word.Length; i += n)
                yield return word[i];
        }

        static string CleanText(string text)
        {
            return new string(text.Where(char.IsLetter).ToArray()).ToUpper();
        }

    }
}
