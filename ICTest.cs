using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CiphersSolvers;

namespace VigenereCipherSolver
{
    static class ICTest
    {
        public static double IndexOfCoincidence(string text)
        {
            var count = new int[26];
            var total = 0;
            var upper = text.ToUpper();

            foreach (var c in upper.Where(c => c >= 'A' && c <= 'Z'))
            {
                count[c - 'A']++;
                total++;
            }

            var coincidence = 0.0;
            for (int i = 0; i < count.Length; i++)
            {
                coincidence += (double)count[i] / (double)total * ((double)count[i] - 1) / ((double)total - 1);
            }

            return coincidence;
        }

        public static List<int> GuessKeyLength(string text, int from, int to)
        {
            var ICAverages = new Dictionary<int, double>();

            for (int i = from; i < to + 1; i++)
            {
                var split = Helper.SplitText(text, i);
                var IC = split.Select(IndexOfCoincidence).ToList();
                ICAverages.Add(i, IC.Average());
            }

            var bestThree = ICAverages.OrderByDescending(x => x.Value).Take(3).ToList();
            Console.WriteLine("Top three key lengths with probabilities: ");
            bestThree.ForEach(x => Console.WriteLine(x.Key + " - " + x.Value));
            return bestThree.Select(x => x.Key).ToList();
        }

        public static string GuessKey(int length, string text, List<float> language)
        {
            var key = "";
            var split = Helper.SplitText(text, length);
            for (int j = 0; j < split.Count; j++)
            {
                var probs = new List<double>();
                for (int i = 0; i < 26; i++)
                {
                    
                    var caesar = new CaesarCipher(i);
                    probs.Add(ChiSquaredStatistic(caesar.Cipher(split[j],true), language));
                }

                key += Program.Alphabet[probs.IndexOf(probs.Min())];
            }

            Console.WriteLine("Guessed key is: " + key);
            return key;
        }

        static double ChiSquaredStatistic(string text, List<float> language)
        {
            var multiplicities = Helper.CountChars(text);
            var chi = 0.0;
            for (int i = 0; i < Program.Alphabet.Length; i++)
            {
                var E = language[i] * text.Length;
                if(E == 0) continue;
                var pow = Math.Pow(multiplicities[i] - E, 2);
                chi += pow / E;
            }

            return chi;
        }

    }
}
