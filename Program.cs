using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using System.CommandLine.DragonFruit;
using System.Threading.Tasks;
using CiphersSolvers;
using Combinatorics.Collections;
using Extreme.Mathematics;
using BigInteger = System.Numerics.BigInteger;

namespace VigenereCipherSolver
{
    static class Program
    {
        public static char[] Alphabet { get; set; } = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">Path to input text file </param>
        /// <param name="type">What kind of Ciphres you want to use to Encrypt/decrypt
        static void Main(string input, CipherEnum type)
        {
            //5. semestralka

            for (int i = 2; i < 5; i++)
            {
                
                var sr = new StreamReader($"..//..//..//test//shadow{i}.txt");
                var encryptedText = sr.ReadToEnd();
                encryptedText = encryptedText.Replace('\n', ':');
                var MD5Bruteforce = new MD5Bruteforce(i);
                Console.WriteLine($"Shadow{i} started");
                Console.WriteLine(MD5Bruteforce.Cipher(encryptedText, false));
                //var task = Task.Factory.StartNew(() => MD5Bruteforce.Cipher(encryptedText, false));
                
                //tasks[i - 1] = task;
            }

            //Task.WaitAll(tasks);

            //4. semka
            //var cipher = new RSACipher(BigInteger.Parse("56341958081545199783"), 65537);
            //cipher.Cipher("17014716723435111315", false);

            //var sr = new StreamReader(input);
            //var encryptedText = sr.ReadToEnd();

            //Console.WriteLine("Do you want encrypt or decrypt (y - encrypt, n - decrypt) ?");
            //var encrypt = Console.ReadLine().ToLower().StartsWith("y");

            //ICipher cipher = null;
            //switch (type)
            //{
            //    case CipherEnum.CAESAR:
            //        cipher = new CaesarCipher();
            //        break;
            //    case CipherEnum.VIGENERE:
            //        cipher = new VigenereCipher(encrypt, encryptedText);
            //        break;
            //    case CipherEnum.HILL:
            //        cipher = new HillCipher();
            //        break;
            //    case CipherEnum.STREAM:
            //        cipher = new StreamCipher(encryptedText);
            //        break;
            //    case CipherEnum.RSA:
            //        cipher = new RSACipher(43811,7);
            //        break;
            //    default:
            //        Console.WriteLine("Please select one of the provided ciphers!");
            //        Environment.Exit(99);
            //        break;
            //}
            //Console.WriteLine("Text: ");
            //Console.WriteLine(cipher.Cipher(encryptedText, encrypt));
        }

        }
}
