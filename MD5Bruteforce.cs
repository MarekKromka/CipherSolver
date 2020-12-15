using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using Combinatorics.Collections;

namespace CiphersSolvers
{
    class MD5Bruteforce : ICipher
    {
        public List<Person> Persons { get; set; }
        public Random Random { get; set; }
        public int Shadow { get; set; }
        public MD5Bruteforce(int shadow)
        {
            Random = new Random();
            Shadow = shadow;

            Persons = new List<Person>();
        }

        public string Cipher(string text, bool encrypt)
        {
            string result = "";

            var randomLenght = new Random(Random.Next());
            var randomLetter = new Random(Random.Next());

            var data = text.Split(':').ToList();
            data.RemoveAt(data.Count - 1);
            for (int i = 0; i < data.Count; i += 3)
            {
                Persons.Add(new Person(data[i], data[i + 1], data[i + 2]));
            }

            //Console.WriteLine("SELECT METHOD TO LOOK UP PASSWORDS:\n 1.Names with one big letter\n 2. 6-7 long random lower letters passwords\n 3. 4-5 long random lower/upper + numbers passwords");
            var num = "3";

            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            switch (num)
            {
                case "1":
                    var sr = new StreamReader("..//..//..//test//updated_names.txt");
                    var allNames = sr.ReadToEnd();
                    var names = allNames.Split('\n', StringSplitOptions.None).ToList();

                    foreach (var person in Persons)
                    {
                        var pom = names.FirstOrDefault(s => crypt(s, person.Salt) == person.Md5);
                        if (!string.IsNullOrEmpty(pom)) result += "User: " + person.Name + " password is: " + pom + "\n";
                    }
                    break;
                case "2":
                    var alphabet = "abcdefghijklmnopqrstuvwxyz".ToList();
                    var alphabet2 = alphabet.OrderByDescending(x => x).ToList();
                    var alphabet3 = "mnopqrstuvwxyzabcdefghijkl".ToList();
                    var alphabet4 = "lkjihgfedcbazyxwvutsrpqonm".ToList();

                    var tasks = new Task<string>[alphabet.Count];
                    for (int i = 0; i < alphabet.Count; i++)
                    {
                        var cb = new Variations<char>(alphabet, 6, GenerateOption.WithRepetition);
                        var t = Task.Factory.StartNew(() => BruteForce(cb, i), token);
                        tasks[i] = t;
                        alphabet = alphabet.ShiftLeft(1);
                    }
                    var ended = Task.WaitAny(tasks);
                    result += tasks[ended].Result;
                    tokenSource.Cancel();

                    //var combs = new Variations<char>(alphabet, 6, GenerateOption.WithoutRepetition);
                    //var combs2 = new Variations<char>(alphabet2, 6, GenerateOption.WithoutRepetition);
                    //var combs3 = new Variations<char>(alphabet3, 6, GenerateOption.WithoutRepetition);
                    //var combs4 = new Variations<char>(alphabet4, 6, GenerateOption.WithoutRepetition);
                    //var tasks = new Task<string>[4];
                    //var q = alphabet.Select(x => x.ToString());
                    //int size = 7;
                    //for (int i = 0; i < size - 1; i++)
                    //    q = q.SelectMany(x => alphabet, (x, y) => x + y);
                    //var cc = q.ToList();
                    //var possiblePasswords = SplitList(cc, cc.Count / 10).ToList();


                    //var task1 = Task.Factory.StartNew(() => BruteForce(persons, combs, 1), token);
                    //tasks[0] = task1;
                    //var task2 = Task.Factory.StartNew(() => BruteForce(persons, combs2, 2), token);
                    //tasks[1] = task1;
                    //var task3 = Task.Factory.StartNew(() => BruteForce(persons, combs3, 3), token);
                    //tasks[2] = task1;
                    //var task4 = Task.Factory.StartNew(() => BruteForce(persons, combs4, 4), token);
                    //tasks[3] = task1;
                    //var task5 = Task.Factory.StartNew(() => BruteForce(persons, possiblePasswords[4], 4));
                    //var task6 = Task.Factory.StartNew(() => BruteForce(persons, possiblePasswords[5], 5));
                    //var task7 = Task.Factory.StartNew(() => BruteForce(persons, possiblePasswords[6], 6));
                    //var task8 = Task.Factory.StartNew(() => BruteForce(persons, possiblePasswords[7], 7));
                    //var task9 = Task.Factory.StartNew(() => BruteForce(persons, possiblePasswords[8], 8));
                    //var task10 = Task.Factory.StartNew(() => BruteForce(persons, possiblePasswords[9], 9));
                    //Task.WaitAll(task1);


                    //result += task1.Result + task2.Result + task3.Result + task4.Result + task5.Result + task6.Result +
                    //          task7.Result + task8.Result + task9.Result + task10.Result;

                    break;
                case "3":

                    var a = "aAbBcCdDeEfFgGhHiIjJkKlLmMnNoOpPqQrRsStTuUvVwWxXyYzZ0123456789".ToList();
                    var variation = new Variations<char>(a, 4, GenerateOption.WithRepetition);

                    var tasks2 = new Task<string>[a.Count];
                    for (int i = 0; i < a.Count; i++)
                    {
                        var cb = new Variations<char>(a, 4, GenerateOption.WithRepetition);
                        var t = Task.Factory.StartNew(() => BruteForce(cb, i), token);
                        tasks2[i] = t;
                        a = a.ShiftLeft(1);
                    }
                    var e = Task.WaitAny(tasks2);
                    result += tasks2[e].Result;
                    tokenSource.Cancel();

                    break;
                default:
                    break;
            }

            var bw = new BinaryWriter(File.Open($"C:\\Users\\Marek\\Desktop\\heslaShadow{Shadow}.txt", FileMode.OpenOrCreate));
            bw.Write(result);
            Console.WriteLine($"SHADOW{Shadow} FOUND!!!");

            return string.IsNullOrEmpty(result) ? "No results" : result;
        }

        public static IEnumerable<List<T>> SplitList<T>(List<T> locations, int nSize = 30)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }

        public string BruteForce(Variations<char> possiblePasswords, int id)
        {
            string result = "";
            var done = 0;
            foreach (var ch in possiblePasswords)
            {
                string pass = "";
                foreach (var c in ch)
                {
                    pass += c;
                }

                foreach (var person in Persons)
                {
                    if (crypt(pass, person.Salt) == person.Md5)
                    {
                        result += "User: " + person.Name + " password is: " + pass + "\n";
                        return result;
                    }
                }
                // Progress log
                
                var p = ++done /possiblePasswords.Count * 100;
            }

            return result;
        }

        /// <summary>
        /// Funkcia vygeneruje nahodnu sol pouzivanu pri ukladani odtalcku hesiel pouzivatelov
        /// </summary>
        /// <param name="lengthInBytes">pocet nahodne vygenerovanych bajtov</param>
        /// <returns></returns>
        public static string createSalt(int lengthInBytes)
        {
            // pripravim buffer, do ktoreho vygenerujem nahodne cislo
            byte[] buffer = new byte[lengthInBytes];

            // pomocny objekt pre generovanie nahody
            Random rnd = new Random();

            // vygenerujem nahodne cislo
            rnd.NextBytes(buffer);

            // vratim nahodu ako Base64 string
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// Funkcia na vypocet odtlacku zadaneho hesla s pouzitim zadanej soli
        /// </summary>
        /// <param name="passwd">heslo</param>
        /// <param name="salt">sol</param>
        /// <returns></returns>
        public static string crypt(string passwd, string salt)
        {
            // odtlacok budem pocitat z retazca pozostavajuceho z hesla a soli
            string s = passwd + salt;

            // retazec skonvertujem na pole bajtov
            byte[] b = Encoding.UTF8.GetBytes(s);

            // pomocny objekt pre vypocet MD5
            MD5 md5 = MD5.Create();

            // vypocitam odtlacok hesla a soli
            byte[] h = md5.ComputeHash(b);

            // vratim hash ako Base64 string
            return Convert.ToBase64String(h);
        }
    }

    static class ConsoleLocker
    {
        private static object _lock = new object();

        public static void Write(string s, int left, int top)
        {
            lock (_lock)
            {
                Console.SetCursorPosition(left, top);
                Console.Write(s);
            }
        }
    }

    public static class ShiftList
    {
        public static List<T> ShiftLeft<T>(this List<T> list, int shiftBy)
        {
            if (list.Count <= shiftBy)
            {
                return list;
            }

            var result = list.GetRange(shiftBy, list.Count - shiftBy);
            result.AddRange(list.GetRange(0, shiftBy));
            return result;
        }

        public static List<T> ShiftRight<T>(this List<T> list, int shiftBy)
        {
            if (list.Count <= shiftBy)
            {
                return list;
            }

            var result = list.GetRange(list.Count - shiftBy, shiftBy);
            result.AddRange(list.GetRange(0, list.Count - shiftBy));
            return result;
        }
    }
}
