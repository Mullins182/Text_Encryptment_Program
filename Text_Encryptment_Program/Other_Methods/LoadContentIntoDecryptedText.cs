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

            if(!File.Exists(source)) 
            {
                Content.Add("QUELLDATEI NICHT GEFUNDEN !!!");
            }
            else
            {
                Content = [.. File.ReadAllLines(source, Encoding.UTF8)];      // [.. ] Konvertiert das übergebene String[] in eine Liste (Intellisense vereinfachung von methode ".ToList()"!

                if(Content.Count == 0) 
                {
                    Content.Add("DIE QUELLDATEI IST LEER !!!");
                }
            }

            return Content;
        }
    }
}
