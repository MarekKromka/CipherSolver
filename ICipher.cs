using System;
using System.Collections.Generic;
using System.Text;

namespace CiphersSolvers
{
    public interface ICipher
    {
        string Cipher(string text, bool encrypt);
    }
}
