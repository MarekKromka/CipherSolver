using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CiphersSolvers;

namespace VigenereCipherSolver
{
    static class KasiskiTest
    {

        public static List<int> GuessKeyLenght(string text)
        {
            var frequency = new Dictionary<int, int>();
            List<char> list = new List<char>();
            foreach (var character in text)
            {
                if (character >= 'A' && character <= 'Z')
                {
                    list.Add(character);
                }
            }

            for (var i = 0; i < list.Count - 3; i++)
            {
                for (var j = i + 1; j < list.Count - 3; j++)
                {
                    if (list[i] == list[j] && list[i + 1] == list[j + 1] && list[i + 2] == list[j + 2])
                    {
                        if (frequency.ContainsKey(j - i))
                            frequency[j - i]++;
                        else
                            frequency[j - i] = 1;
                    }
                }
            }

            var bestThree = frequency.OrderByDescending(x => x.Value).Take(10).ToList();
            Console.WriteLine("\nKey length will be the common divider of those values: ");
            bestThree.ForEach(x => Console.WriteLine($"{x.Key} - {x.Value}x"));
            return bestThree.Select(x => x.Key).ToList();
        }

        public static string guessKey(int length, string encryptedText, List<float> language)
        {
            var guessedKey = "";
            Console.WriteLine($"\nSplitting text by key's lenght ({length}): ");
            var splittedByLength = Helper.SplitText(encryptedText, length);
            var counter = 0;
            splittedByLength.ForEach(x => Console.WriteLine(IndexOfCoincidence(x)));
            foreach (var tail in splittedByLength)
            {
                //Console.WriteLine(indexOfCoincidence(tail));
                var encryptedProbs = GetProbabilities(tail);
                //Console.WriteLine(GetDistance(slovakProbs, encryptedProbs));
                var mostProbableLetter = new List<double>();
                for (int i = 0; i < Program.Alphabet.Length; i++)
                {
                    mostProbableLetter.Add(Math.Pow(GetDistance(language, encryptedProbs), 2));
                    //Console.WriteLine($"{Alphabet[i]} - {Math.Pow(GetDistance(slovakProbs, encryptedProbs), 2)}");
                    encryptedProbs = RotateLeft(encryptedProbs.ToArray()).ToList();
                }
                Console.WriteLine($"{++counter} letter of key is: {Program.Alphabet[mostProbableLetter.IndexOf(mostProbableLetter.Min())]}");
                guessedKey += Program.Alphabet[mostProbableLetter.IndexOf(mostProbableLetter.Min())];
            }

            return guessedKey;
        }


        static double IndexOfCoincidence(string text)
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

        static List<float> GetProbabilities(string text)
        {
            var multiplicities = CountChars(text);
            var total = multiplicities.Sum();

            var probabilities = multiplicities.Select(x => (float)x / (float)total).ToList();

            return probabilities;
        }

        static void PrintProbabilities(List<float> probabilities)
        {
            for (int i = 0; i < probabilities.Count; i++)
            {
                Console.WriteLine($"{Program.Alphabet[i]}  - {probabilities[i]}");
            }
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

        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        static double GetDistance(List<float> prob1, List<float> prob2)
        {
            var distance = 0.0;
            for (int i = 0; i < prob1.Count; i++)
            {
                distance += Math.Pow(prob1[i] - prob2[i], 2);
            }

            return distance;
        }

        static float[] RotateLeft(float[] arr)
        {
            var temp = arr[0];
            for (int i = 0; i < arr.Length - 1; i++)
            {
                arr[i] = arr[i + 1];
            }

            arr[arr.Length - 1] = temp;

            return arr;
        }
    }
}
