using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace CDS.Logic
{
    public static class EncyptionDcryption
    {
        static string key = "Rx12fN0M";
        static string vector = "oF27qZ7S";
        ////////////////////////////////////////////////////////////////////////////////
        // Decryption
        ////////////////////////////////////////////////////////////////////////////////
        public static string GetDecryptedText(string txt)
        {

            txt = txt.Replace(' ', '+');
            DESCryptoServiceProvider key = new DESCryptoServiceProvider();

            key.Key = ASCIIEncoding.ASCII.GetBytes("Rx12fN0M"); // decryption key

            key.IV = ASCIIEncoding.ASCII.GetBytes("oF27qZ7S");// initialization vector

            int length = txt.Length;
            byte[] buffer = new byte[length];
            buffer = Convert.FromBase64String(txt);

            string decText = Decrypt(buffer, key);

            return decText;
        }

        public static string Decrypt(byte[] CypherText, SymmetricAlgorithm key)
        {
            // Create a memory stream to the passed buffer.
            MemoryStream ms = new MemoryStream(CypherText);

            // Create a CryptoStream using the memory stream and the 
            // CSP DES key. 

            CryptoStream encStream = new CryptoStream(ms, key.CreateDecryptor(), CryptoStreamMode.Read);
            // Create a StreamReader for reading the stream.

            StreamReader sr = new StreamReader(encStream);
            // Read the stream as a string.

            string val = sr.ReadLine();

            // Close the streams.
            sr.Close();
            encStream.Close();
            ms.Close();

            return val;
        }

        ////////////////////////////////////////////////////////////////////////////////
        // Encryption
        ////////////////////////////////////////////////////////////////////////////////
        public static byte[] Encrypt(string PlainText, SymmetricAlgorithm key)
        {
            // Create a memory stream.
            MemoryStream ms = new MemoryStream();

            // Create a CryptoStream using the memory stream and the 
            // CSP DES key.  
            CryptoStream encStream = new CryptoStream(ms, key.CreateEncryptor(), CryptoStreamMode.Write);

            // Create a StreamWriter to write a string
            // to the stream.
            StreamWriter sw = new StreamWriter(encStream);

            // Write the plaintext to the stream.
            sw.WriteLine(PlainText);

            // Close the StreamWriter and CryptoStream.
            sw.Close();
            encStream.Close();

            // Get an array of bytes that represents
            // the memory stream.
            byte[] buffer = ms.ToArray();

            // Close the memory stream.
            ms.Close();

            // Return the encrypted byte array.
            return buffer;
        }

        public static string GetEncryptedText(string txt)
        {
            DESCryptoServiceProvider key = new DESCryptoServiceProvider();
            key.Key = ASCIIEncoding.ASCII.GetBytes("Rx12fN0M"); // decryption key
            key.IV = ASCIIEncoding.ASCII.GetBytes("oF27qZ7S");// initialization vector

            //  Encrypt a string to a byte array.
            byte[] buffer = Encrypt(txt, key);
            string encText;
            encText = Convert.ToBase64String(buffer);

            return encText;
        }
    }
}