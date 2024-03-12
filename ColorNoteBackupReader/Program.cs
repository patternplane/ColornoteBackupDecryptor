using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ColorNoteBackupReader
{
    class Program
    {

        // === MS Official Reference ===
        // AesCryptoServiceProvider Class
        // https://learn.microsoft.com/ko-kr/dotnet/api/system.security.cryptography.aescryptoserviceprovider?view=netframework-4.8.1

        // === ColorNote-backup-decryptor Github Reference ===
        // https://github.com/olejorgenb/ColorNote-backup-decryptor

        static void Main(string[] args)
        {
            MD5 hash = MD5.Create();
            string salt = "ColorNote Fixed Salt";
            byte[] saltByte = Encoding.UTF8.GetBytes(salt);
            string password = "passwordTest";

            List<byte> li = new List<byte>(Encoding.UTF8.GetBytes(password));
            li.AddRange(saltByte);

            // PBE With MD5
            byte[] result = hash.ComputeHash(li.ToArray());
            Console.WriteLine(result.Length);
            Console.WriteLine(Convert.ToBase64String(result));

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.Key = result;
            aes.IV = new byte[16];
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key,aes.IV);

            string path = Console.ReadLine();
            BinaryReader sr = new BinaryReader(new FileStream(path, FileMode.Open));
            for (int i = 0; i < 28; i++) sr.ReadByte();
            List<byte> temp = new List<byte>();
            byte[] temp2;
            while ((temp2 = sr.ReadBytes(100)).Length != 0)
            {
                temp.AddRange(temp2);
            }
            sr.Close();
            byte[] cipherText = temp.ToArray();
            string plainText;

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.

                        StreamWriter sw = new StreamWriter(@"./결과.txt");
                        char[] buffer = new char[500];
                        int readLen;
                        while (true)
                        {
                            readLen = srDecrypt.Read(buffer, 0, 500);
                            if (readLen == 0)
                                break;
                            sw.Write(buffer);
                        }
                        sw.Close();
                    }
                }
            }

            // Remove Unreadable Character
            /*string path = Console.ReadLine();
            string outPath = @"C:\Users\Sun\Desktop\Result3.txt";
            StreamReader sr = new StreamReader(path);
            StreamWriter sw = new StreamWriter(outPath);

            while (!sr.EndOfStream)
            {
                while (!sr.EndOfStream)
                {
                    int c = sr.Read();
                    if (c == 0)
                    {
                        sr.Read();
                        sr.Read();
                        sr.Read();
                        break;
                    }
                    sw.Write((char)c);
                }
                sw.Write(',');
            }

            sr.Close();
            sw.Close();*/
        }

        static void runDecryption()
        {
            string password = Console.ReadLine();
            byte[] passwordByte = Encoding.UTF8.GetBytes(password);
            byte[] salt = Encoding.UTF8.GetBytes("ColorNote Fixed Salt");
            /*Aes aes = new Aes();
            aes.

            string result = DecryptStringFromBytes_Aes(encrypted, plainByte, myAes.IV);*/
        }
    }
}
