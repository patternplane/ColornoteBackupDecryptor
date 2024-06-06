using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorNoteBackupReader
{
    class main
    {
        [STAThreadAttribute]
        static public void Main()
        {
            // Parser
            System.Windows.Forms.OpenFileDialog fd = new System.Windows.Forms.OpenFileDialog();
            fd.Multiselect = true;
            fd.ShowDialog();
            System.Windows.Forms.FolderBrowserDialog fb = new System.Windows.Forms.FolderBrowserDialog();
            fb.ShowDialog();

            Parser p = new Parser();
            Program d = new Program();

            string input, output;
            foreach (string file in fd.FileNames)
            {
                input = file;
                output = fb.SelectedPath + "\\" + Path.GetFileNameWithoutExtension(file) + ".txt";

                p.parse(Encoding.UTF8.GetChars(d.decrypt("0000", input)),output,  false);
            }
        }
    }
}
