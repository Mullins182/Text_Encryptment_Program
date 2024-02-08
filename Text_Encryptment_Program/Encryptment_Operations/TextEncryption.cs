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
        public static List<char> EncryptText(List<char> decrText, int randomNumber, int charPos) 
        {

            decrText[charPos] = Convert.ToChar(randomNumber);

            return decrText;
        }
    }
}
