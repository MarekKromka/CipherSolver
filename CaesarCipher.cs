using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CiphersSolvers;

namespace VigenereCipherSolver
{
    class CaesarCipher : ICipher
    {
        public int Shift { get; set; }
        public CaesarCipher(int shift)
        {
            Shift = shift;
        }

        public CaesarCipher()
        {
            Console.WriteLine("Please specify the number of characters shifted: ");
            if (int.TryParse(Console.ReadLine(), out int shift))
            {
                Shift = shift;
            }
        }

        static string Cipher(string text, int shift, bool encrypt)
        {
            if (encrypt) shift = 26 - shift;

            return text.Aggregate("", (current, ch) => current + Cipher(ch, shift));
        }

        static char Cipher(char ch, int shift)
        {
            if (!char.IsLetter(ch))
            {
                return ch;
            }

            char d = char.IsUpper(ch) ? 'A' : 'a';
            return (char)((ch + shift - d) % 26 + d);
        }

        public string Cipher(string text, bool encrypt)
        {
            return Cipher(text, Shift, encrypt);
        }
    }
}
