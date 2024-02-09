using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Text_Encryptment_Program
{
    public static class TextDecryption                         // Hiermit wird der Text einer Textdatei entschlüsselt !
    {
        public static char DecryptText(char encryptedChar, Dictionary<int, int> DecrKeyTable, Dictionary<int, int> EncrKeyTable, int index) 
        {
            char result;

            //if (EncrKeyTable[index] == ((int)encryptedChar))
            //{
                result = Convert.ToChar(DecrKeyTable[index]);

                return result;
            //}

            //return encryptedChar;
        }
    }
}
