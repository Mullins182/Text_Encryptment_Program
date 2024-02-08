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
            List<char> result = new List<char>();

            foreach (var item in decrText)
            {
                result.Add(item);
            }

            if (Convert.ToInt32(result[charPos]) == 10 || Convert.ToInt32(result[charPos]) == 13 || Convert.ToInt32(result[charPos]) == 9)
            {

            }
            else
            {
                result[charPos] = Convert.ToChar(randomNumber);
            }


            return result;
        }
    }
}
