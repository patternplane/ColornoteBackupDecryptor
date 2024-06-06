using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorNoteBackupReader
{
    class Program
    {
        static readonly int NOTE_DATA_OFFSET = 28; // or 0 in last version
        static readonly string NOTE_DEFAULT_SALT = "ColorNote Fixed Salt";

        public byte[] decrypt(string passwordStr, byte[] input)
        {
            byte[] password = Encoding.UTF8.GetBytes(passwordStr);
            byte[] salt = Encoding.UTF8.GetBytes(NOTE_DEFAULT_SALT);

            return new PBKDF1WithMD5And128AES_BC().Decrypt(password, salt, input, NOTE_DATA_OFFSET);
        }

        public byte[] decrypt(string passwordStr, string path)
        {
            BinaryReader sr = new BinaryReader(new FileStream(path, FileMode.Open));
            List<byte> temp = new List<byte>();
            byte[] temp2;
            while ((temp2 = sr.ReadBytes(100)).Length != 0)
                temp.AddRange(temp2);
            sr.Close();
            byte[] cipherText = temp.ToArray();

            return decrypt(passwordStr, cipherText);
        }

        public void decrypt(string passwordStr, string path, string outPath)
        {
            byte[] plainText = decrypt(passwordStr, path);
            
            StreamWriter sw = new StreamWriter(outPath);
            if (plainText != null)
                sw.Write(Encoding.UTF8.GetString(plainText));
            sw.Close();
        }
    }
}
