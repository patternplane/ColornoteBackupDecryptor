using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ColorNoteBackupReader
{
    /// <summary>
    /// BouncyCastle의 PBEWithMD5And128bitAES-CBC-OPENSSL 알고리즘
    /// </summary>
    public class PBKDF1WithMD5And128AES_BC
    {
        readonly int FIXED_INTERATION = 1;
        readonly byte[] FIXED_IV = new byte[16];

        public byte[] Decrypt(byte[] password, byte[] salt, byte[] encrypted, int offset = 0)
        {
            List<byte> passwordAndSaltList = new List<byte>();
            passwordAndSaltList.AddRange(password);
            passwordAndSaltList.AddRange(salt);
            byte[] passwordAndSalt = passwordAndSaltList.ToArray();
            // ^ Not tested

            // PBE derived key With MD5
            MD5 hash = MD5.Create();
            for (int i = 0; i < FIXED_INTERATION; i++)
                passwordAndSalt = hash.ComputeHash(passwordAndSalt);

            // debug Output
            Console.WriteLine(passwordAndSalt.Length);
            Console.WriteLine(Convert.ToBase64String(passwordAndSalt));

            // Decrypt
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.Key = passwordAndSalt;
            aes.IV = FIXED_IV;
            aes.Padding = PaddingMode.PKCS7;
            try
            {
                return aes.CreateDecryptor(aes.Key, aes.IV).TransformFinalBlock(encrypted, offset, encrypted.Length - offset);
            }
            catch
            {
                // error - can't decrypt
                return null;
            }
        }
    }
}
