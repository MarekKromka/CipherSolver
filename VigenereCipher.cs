using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using VigenereCipherSolver;

namespace CiphersSolvers
{
    public class VigenereCipher : ICipher
    {
        public static List<float> slovakProbs = new List<float>()
        {
            0.11160F, 0.01778F, 0.02463F, 0.03760F, 0.09316F, 0.00165F, 0.00175F, 0.02482F, 0.05745F, 0.02158F,
            0.03961F, 0.04375F, 0.03578F, 0.05949F, 0.09540F, 0.03007F, 0.00000F, 0.04706F, 0.06121F, 0.05722F,
            0.03308F, 0.04604F, 0.00001F, 0.00028F, 0.02674F, 0.03064F
        };

        public static List<float> engProbs = new List<float>()
        {
            0.08496F, 0.02072F, 0.04538F, 0.03384F, 0.11160F, 0.01812F, 0.02470F, 0.03003F, 0.07544F, 0.00196F,
            0.01101F, 0.05489F, 0.03012F, 0.06654F, 0.07163F, 0.03167F, 0.00196F, 0.07580F, 0.05735F, 0.06950F,
            0.03630F, 0.01007F, 0.01289F, 0.00290F, 0.01777F, 0.00272F
        };

        public string Key { get; set; }
        public string Text { get; set; }

        public VigenereCipher(string key, string text)
        {
            Key = key;
            Text = text;
        }

        public VigenereCipher(bool encrypt, string text)
        {
            Text = text;
            if (encrypt)
            {
                setKey();
            }
            else
            {
                GuessKey();
            }
        }

        private void GuessKey(bool knownKey = false)
        {
            while (!knownKey)
            {
                int method = 0;
                int lang = 0;
                Console.WriteLine("Do you know the key ? ");
                if (Console.ReadLine().ToLower().StartsWith("y"))
                {
                    setKey();
                    knownKey = true;
                }
                else
                {
                    Console.WriteLine("We are gonna guess the key.");
                    Console.WriteLine("SELECT KEY DISCOVERY METHOD: \n1. KASINSKI \n2. INDEX OF COINCIDENCE");
                    method = int.Parse(Console.ReadLine());
                    Console.WriteLine("SELECT LANGUAGE: \n1. SVK \n2. ENG");
                    lang = int.Parse(Console.ReadLine());
                }

                Console.Clear();
                switch (method)
                {
                    case 1:
                        KasiskiTest.GuessKeyLenght(Text);
                        var length = specifyKeyLength();
                        Key = KasiskiTest.guessKey(length, Text, lang == 1 ? Program.slovakProbs : Program.engProbs);
                        knownKey = true;
                        break;
                    case 2:
                        ICTest.GuessKeyLength(Text, 1, 50);
                        
                        Key = ICTest.GuessKey(specifyKeyLength(), Text, lang == 1 ? Program.slovakProbs : Program.engProbs);
                        knownKey = true;
                        break;
                    default:
                        Console.WriteLine("You selected invalid method number! Please select again!");
                        break;
                }
            }
        }

        private int specifyKeyLength()
        {
            Console.WriteLine("Specify key length: ");
            return int.Parse(Console.ReadLine());
        }

        private void setKey()
        {
            Console.WriteLine("Please provide key to encrypt this message: ");
            Key = Console.ReadLine();
        }

        public string Cipher(string text, bool encrypt)
        {
            string output = string.Empty;
            int nonAlphaCharCount = 0;

            for (int i = 0; i < text.Length; ++i)
            {
                if (char.IsLetter(text[i]))
                {
                    bool cIsUpper = char.IsUpper(text[i]);
                    char offset = cIsUpper ? 'A' : 'a';
                    int keyIndex = (i - nonAlphaCharCount) % Key.Length;
                    int k = (cIsUpper ? char.ToUpper(Key[keyIndex]) : char.ToLower(Key[keyIndex])) - offset;
                    k = encrypt ? k : -k;
                    char ch = (char)((Mod(((text[i] + k) - offset), 26)) + offset);
                    output += ch;
                }
                else
                {
                    output += text[i];
                    ++nonAlphaCharCount;
                }
            }

            return output;
        }

        private static int Mod(int a, int b)
        {
            return (a % b + b) % b;
        }
    }
}
