using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Text_Encryptment_Program
{
    public static class TextEncryption                     // Hiermit wird der Text einer Textdatei verschlüsselt !
    {
        public static List<string> EncryptText(List<string> decrText, int randomNumber, int encryptionChar) 
        {
            List<string> encrText = new List<string>();
                        
            foreach (var item in decrText)
            {
                encrText.Add(item.Replace(Convert.ToChar(encryptionChar), Convert.ToChar(randomNumber)));
            }

            return encrText;
        }
    }
}
