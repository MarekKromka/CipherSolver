using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CiphersSolvers
{
    public class Person
    {

        public Person(string name, string salt, string md5)
        {
            Name = name;
            Salt = salt;
            Md5 = md5;
        }
        public string Name { get; set; }

        public string Salt { get; set; }

        public string Md5 { get; set; }
    }
}
