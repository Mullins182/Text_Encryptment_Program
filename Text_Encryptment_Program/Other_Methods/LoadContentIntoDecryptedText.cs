using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Encryptment_Program.Other_Methods
{
    public static class LoadContentIntoDecryptedText
    {
        public static List<string> Content { get; set; } = new List<string>();

        public static List<string> ReadFileData(string source)
        {
            Content.Clear();

            Content = File.ReadAllLines(source, Encoding.UTF8).ToList();

            return Content;
        }
    }
}
