using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace CiphersSolvers
{
    class RSACipher : ICipher
    {
        public BigInteger N { get; set; }

        public double E { get; set; }

        private static int MAXN = 100001;
        private static int[] spf = new int[MAXN];
        public static int[,] adj = new int[MAXN, MAXN];

        public RSACipher(BigInteger n, double e)
        {
            N = n;
            E = e;
        }

        private BigInteger Mod(BigInteger a, BigInteger b)
        {
            //var pom1 = BigInteger.Divide(a, b);
            //var pom2 = BigInteger.Multiply(pom1, b);
            //BigInteger.DivRem(a, b, out var c);
            //return BigInteger.Add(pom2, c);

            var r = BigInteger.Remainder(a, b);
            return r < 0 ? BigInteger.Add(r, b) : r;

        }

        public string Cipher(string text, bool encrypt)
        {
            string result = "";
            byte[] inn = new byte[8];
            byte[] outt = new byte[16];

            
            byte[] byteArray = Encoding.ASCII.GetBytes(text);
            var c = new MemoryStream(byteArray);

            if (encrypt)
            {
                var bw = new BinaryWriter(File.Open("C:\\Users\\Marek\\Desktop\\output.enc", FileMode.OpenOrCreate));
                while (true)
                {
                    if (c.Read(inn, 0, 8) == 0) break;
                    outt = BigInteger.ModPow(new BigInteger(inn), new BigInteger(E), N).ToByteArray();
                    bw.Write(outt);
                }
                
            }
            else
            {
                var primes = Factorize(N);
                var fi_n = BigInteger.Multiply(primes.First().Item1 - 1, primes.Last().Item1 - 1);
                var d = modInverse(new BigInteger(E), fi_n);

                //while (true)
                //{

                //    if (c.Read(outt, 0, 16) == 0) break;
                    var o = BigInteger.Parse(text);
                    result = BigInteger.ModPow(o, d, N).ToString();
                //    inn = pom2.ToByteArray();

                //    Array.Reverse(inn);

                //    String rsaDec = Encoding.UTF8.GetString(inn);

                //    String test = Encoding.UTF8.GetString(inn);
                //    result += System.Text.Encoding.ASCII.GetString(inn);
                //}
            }
            return result;
        }


        private BigInteger modInverse(BigInteger a, BigInteger n)
        {
            var tuple = EGCD2(a, n);
            if (tuple.Item1 == 1)
            {
                BigInteger.DivRem(tuple.Item2, n, out var c);
                var test = Mod(tuple.Item2, n);
                return test;
            }

            return -1;
        }

        private Tuple<BigInteger, BigInteger, BigInteger> EGCD(BigInteger a, BigInteger b)
        {
            BigInteger u0 = 1, v1 = 1;
            BigInteger u1 = 0, v0 = 0;
            while (b != 0)
            {
                var q = BigInteger.Divide(a, b);

                var pom = b;
                BigInteger.DivRem(a, b, out var c);
                a = pom;

                b = c;

                
                u1 = BigInteger.Subtract(u0, BigInteger.Multiply(q, u1));
                u0 = u1;

                v1 = BigInteger.Subtract(v0, BigInteger.Multiply(q, v1));
                v0 = v1;
            }

            return Tuple.Create(a, u0, v0);
        }

        public Tuple<BigInteger, BigInteger, BigInteger> EGCD2(BigInteger a, BigInteger b)
        {
            BigInteger r;
            BigInteger x0 = 1, xn = 1, y0 = 0, yn = 0, x1 = 0, y1 = 1, f;
            BigInteger.DivRem(a, b, out r);

            while (r > 0)
            {
                f = BigInteger.Divide(a , b);
                var pom1 = BigInteger.Multiply(f, x1);
                xn =  BigInteger.Subtract(x0, pom1);
                var pom2 = BigInteger.Multiply(f, y1);
                yn =  BigInteger.Subtract(y0, pom2); 

                x0 = x1;
                y0 = y1;
                x1 = xn;
                y1 = yn;
                a = b;
                b = r;
                BigInteger.DivRem(a, b, out r);
            }

            return Tuple.Create(b, xn, yn);
        }


        public static List<Tuple<BigInteger, BigInteger>> Factorize(BigInteger n)
        {
            var list = new List<Tuple<BigInteger, BigInteger>>();

            BigInteger i = 2;
            while (n > 1)
            {
                BigInteger j = 0;
                while (BigInteger.Remainder(n, i) == 0)
                {
                    n = BigInteger.Divide(n, i);
                    j = BigInteger.Add(j, 1);
                }
                if (j > 0) list.Add(new Tuple<BigInteger, BigInteger>(i, j));
                i = BigInteger.Add(i, 1);
            }

            return list;
        }

    }
}
