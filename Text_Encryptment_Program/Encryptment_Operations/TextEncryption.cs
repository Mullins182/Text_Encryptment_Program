using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Text_Encryptment_Program.Encryptment_Operations
{
    public class TextEncryption                     // Hiermit wird der Text einer Textdatei verschlüsselt !
    {
        private int charOne = 0;
        private int charTwo = 0;
        private int encryptChar = 36;
        public TextEncryption() { }

        public List<string> EncryptText(List<string> decrText, int randomNumber) 
        {
            List<string> encrText = new List<string>();
                        
            foreach (var item in decrText)
            {
                encrText.Add(item.Replace('e', Convert.ToChar(randomNumber)));
            }

            return encrText;
        }
    }
}
