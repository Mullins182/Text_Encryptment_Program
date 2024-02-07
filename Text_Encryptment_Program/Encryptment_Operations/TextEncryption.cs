using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Text_Encryptment_Program
{
    public static class TextEncryption                     // Hiermit wird der Text der Decrypted Text Box verschlüsselt !
    {
        public static List<string> EncryptText(List<string> decrText, int randomNumber, int encryptionChar) 
        {
            List<string> encrText   = new List<string>();

            int encrCharFound       = 0;

            foreach (var item in decrText)
            {                
                if(item.Contains(Convert.ToChar(encryptionChar)))
                {
                    if(encrCharFound > 0) 
                    {
                        encrText.Add(item);
                    }
                    else
                    {
                        encrText.Add(item.Replace(Convert.ToChar(encryptionChar), Convert.ToChar(randomNumber)));
                        encrCharFound++;
                    }
                }
                else
                {
                    encrText.Add(item);
                }
            }

            return encrText;
        }
    }
}
