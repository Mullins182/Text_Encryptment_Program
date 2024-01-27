using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Encryptment_Program.Encryptment_Operations
{
    public class TextEncryption                     // Hiermit wird der Text einer Textdatei verschlüsselt !
    {
        public TextEncryption() { }

        public TextEncryption(List<string> text) 
        {
            foreach (var item in text)
            {
                item.Replace('c', 'l');
            }
        }
    }
}
