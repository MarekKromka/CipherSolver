using System;
using System.Collections.Generic;
using VigenereCipherSolver;

namespace CiphersSolvers
{
    public class StreamCipher : ICipher
    {
        public double MyRand { get; set; }
        public Dictionary<double, string> best;
        public double Key { get; set; }
        public string Text { get; set; }

        public StreamCipher(string text)
        {
            best = new Dictionary<double, string>();
            Text = text;
        }

        public string Cipher(string text, bool encrypt)
        {
            SetKey();

            SetSeed(Key);
            text = text.ToUpper();
            int i;
            var cipherText = "";
            for (i = 0; i < text.Length; i++)
            {
                int ch = text[i];
                if (ch >= 'A' && ch <= 'Z')
                {
                    int p = ch - 'A';
                    int k = (int)(26 * NextRand());
                    int c = 0;
                    if (encrypt) c = (p + k) % 26;
                    else c = (p + (26 - k)) % 26;
                    cipherText += (char)('A' + c);
                }
                else
                {
                    cipherText += (char)ch;
                }

            }

            return cipherText;
        }

        private void BruteForce(int from, int to)
        {
            var s = new Spinner(Console.CursorLeft,Console.CursorTop);
            s.Start();
            for (int j = from; j < to; j++)
            {
                SetSeed(j);
                Text = Text.ToUpper();
                int i;
                var cipherText = "";
                for (i = 0; i < Text.Length; i++)
                {
                    int ch = Text[i];
                    if (ch >= 'A' && ch <= 'Z')
                    {
                        int p = ch - 'A';
                        int k = (int)(26 * NextRand());
                        int c = (p + (26 - k)) % 26;
                        cipherText += (char)('A' + c);
                    }
                    else
                    {
                        cipherText += (char)ch;
                    }
                }

                var ic = ICTest.IndexOfCoincidence(cipherText);
                if (ic > 0.055)
                {
                    best.Add(j,"IC: "+ ic.ToString().Substring(0,5) + " SAMPLE: " + cipherText.Substring(0, 60));
                }
            }
            s.Stop();
        }

        private void SetKey()
        {
            Console.WriteLine("Do you know key?");
            if (Console.ReadLine().ToLower().StartsWith('y'))
            {
                Console.WriteLine("Please provide key to encrypt this message: ");
                Key = double.Parse(Console.ReadLine());
            }
            else
            {
                Console.WriteLine("Please provide range to test seeds for random gen... FROM: ");
                int from = int.Parse(Console.ReadLine());
                Console.WriteLine("TO: ");
                int to = int.Parse(Console.ReadLine());
                BruteForce(from, to);
                Console.WriteLine("Best IC for these texts:");
                foreach (var keyValuePair in best)
                {
                    Console.WriteLine("SEED: " + keyValuePair.Key + " " + keyValuePair.Value + "...");
                }
                Console.WriteLine("Select SEED:");
                Key = double.Parse(Console.ReadLine());
            }
        }

        void SetSeed(double val)
        {
            MyRand = val;
        }

        double NextRand()
        {
            MyRand = (84589 * MyRand + 45989) % 217728;
            return (double)MyRand / 217728.0;
        }
    }
}
