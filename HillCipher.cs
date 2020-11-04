using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiphersSolvers
{
    public class HillCipher : ICipher
    {

        public string Key { get; set; }
        public int MetrixSize { get; set; }
        public bool Encrypt { get; set; }

        public HillCipher()
        {
        }

        public HillCipher(string key, int size)
        {
            Key = key;
            MetrixSize = size;
        }

        private void setKey(string text)
        {
            Console.WriteLine("Do you know key?");
            if (Console.ReadLine().ToLower().StartsWith('y'))
            {
                Console.WriteLine("Please provide key to encrypt this message: ");
                Key = Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Please provide matrix size (one number eg. 3 means 3x3 matrix): ");
                MetrixSize = int.Parse(Console.ReadLine());
                Console.WriteLine("Please provide known message text: ");
                var uncryptedText = Console.ReadLine().Substring(0, MetrixSize * MetrixSize);
                var cryptedText = text.Substring(0, MetrixSize * MetrixSize);

                Key = countKeyMatrix(cryptedText, uncryptedText);
            }
            
        }

        public string Cipher(string text, bool encrypt)
        {
            setKey(text);

            // Get key matrix from the key string  
            double[,] keyMatrix = new double[MetrixSize, MetrixSize];
            getKeyMatrix(Key, keyMatrix);
            if (!encrypt)
            {
                keyMatrix = To2D(GetInvertedKey(keyMatrix));
            }

            List<double> messageVector = new List<double>();

            // Generate vector for the message  
            for (int i = 0; i < text.Length; i++)
                messageVector.Add((text[i]) % 65);

            double[,] cipherMatrix = new double[text.Length, 1];

            // Following function generates  
            // the encrypted vector  
            this.encrypt(cipherMatrix, keyMatrix, messageVector);

            var cipherText = "";

            // Generate the encrypted text from  
            // the encrypted vector  
            for (int i = 0; i < cipherMatrix.Length; i++)
                cipherText += (char)(cipherMatrix[i, 0] + 65);

            return cipherText;
        }

        private double[][] GetInvertedKey(double[,] metrix)
        {
            var inverse = Matrix.Inverse(ToJaggedArray(metrix));
            var determinant = Matrix.MatrixDeterminant(ToJaggedArray(metrix));
            var modedDeterminant = Helper.Mod((int)determinant, 26);
            var doplnok = 0;
            for (doplnok = 0; doplnok < 26; doplnok++)
            {
                if (modedDeterminant * doplnok % 26 == 1) break;

            }
            var deter = Matrix.Multiply(inverse, determinant);
            var moded = Matrix.Mode(26, deter);
            var multiplied = Matrix.Multiply(moded, doplnok);
            var modedMult = Matrix.Mode(26, multiplied);

            return modedMult;
        }

        public string countKeyMatrix(string crypted, string uncrypted)
        {
            var size = MetrixSize * MetrixSize;
            var metrix = new double[size, size];
            int p = 0;
            for (int i = 0; i < size; i++)
            {
                int k = 0;
                if (p == (size))
                {
                    uncrypted = uncrypted.Remove(0, MetrixSize);
                    p = 0;
                }

                for (int j = 0; j < size; j++)
                {
                    
                    if (k < p + MetrixSize && k >= p) metrix[i, j] = (uncrypted[k - p]) % 65;
                    else metrix[i, j] = 0;
                    k++;
                }

                p += MetrixSize;
            }

            var inverse = Matrix.Inverse(ToJaggedArray(metrix));
            var determinant = Matrix.MatrixDeterminant(ToJaggedArray(metrix));
            var modedDeterminant = Helper.Mod(determinant, 26);
            var doplnok = 0;
            for (doplnok = 0; doplnok < modedDeterminant; doplnok++)
            {
                if ((int)modedDeterminant * doplnok % 26 == 1) break;

            }
            var deter = Matrix.Multiply(inverse, determinant);
            var moded = Matrix.Mode(26, deter);
            var multiplied = Matrix.Multiply(moded, doplnok);
            var modedMult = Matrix.Mode(26, multiplied);
            var druha = new double[size,1];
            for (int i = 0; i < size; i++)
            {
                druha[i,0] = (crypted[i]) % 65;
            }

            var vysledok = Matrix.MultiplyMatrix(To2D(modedMult), druha);
            var vysl2 = Matrix.Mode(26, ToJaggedArray(vysledok));
            var key = "";
            for (int i = 0; i < size; i++)
            {
                key += (char)(65 + vysl2[i][0]);
            }

            return key;
        }

        void getKeyMatrix(String key,
            double[,] keyMatrix)
        {
            int k = 0;
            for (int i = 0; i < MetrixSize; i++)
            {
                for (int j = 0; j < MetrixSize; j++)
                {
                    keyMatrix[i, j] = (key[k]) % 65;
                    k++;
                }
            }
        }

        // Following function encrypts the message  
        private void encrypt(double[,] cipherMatrix, double[,] keyMatrix, List<double> messageVector)
        {
            int x, i, j;
            var pismeno = 0;
            var messageSize = messageVector.Count;
            for (int k = 1; k < messageSize / MetrixSize + 1; k++)
            {
                for (i = 0; i < MetrixSize; i++)
                {
                    for (j = 0; j < 1; j++)
                    {
                        for (x = 0; x < MetrixSize; x++)
                        {
                            cipherMatrix[pismeno, j] += keyMatrix[i, x] *
                                                        messageVector[x];
                        }

                        cipherMatrix[pismeno, j] = cipherMatrix[pismeno, j] % 26;
                    }

                    pismeno++;
                }
                messageVector.RemoveRange(0,MetrixSize);
            }
            
        }

        static T[,] To2D<T>(T[][] source)
        {
            try
            {
                int FirstDim = source.Length;
                int SecondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

                var result = new T[FirstDim, SecondDim];
                for (int i = 0; i < FirstDim; ++i)
                for (int j = 0; j < SecondDim; ++j)
                    result[i, j] = source[i][j];

                return result;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("The given jagged array is not rectangular.");
            }
        }

        static T[][] ToJaggedArray<T>(T[,] twoDimensionalArray)
        {
            int rowsFirstIndex = twoDimensionalArray.GetLowerBound(0);
            int rowsLastIndex = twoDimensionalArray.GetUpperBound(0);
            int numberOfRows = rowsLastIndex + 1;

            int columnsFirstIndex = twoDimensionalArray.GetLowerBound(1);
            int columnsLastIndex = twoDimensionalArray.GetUpperBound(1);
            int numberOfColumns = columnsLastIndex + 1;

            T[][] jaggedArray = new T[numberOfRows][];
            for (int i = rowsFirstIndex; i <= rowsLastIndex; i++)
            {
                jaggedArray[i] = new T[numberOfColumns];

                for (int j = columnsFirstIndex; j <= columnsLastIndex; j++)
                {
                    jaggedArray[i][j] = twoDimensionalArray[i, j];
                }
            }
            return jaggedArray;
        }
    }
}
