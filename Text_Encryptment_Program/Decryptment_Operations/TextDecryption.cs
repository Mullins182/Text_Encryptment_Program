using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Text_Encryptment_Program
{
    public class TextDecryption                         // Hiermit wird der Text einer Textdatei entschlüsselt !
    {
        public TextDecryption() { }

        public static string DecryptText(string encryptedText, Dictionary<int, int> keyList, int key) 
        {
            string result;

            result = encryptedText.Replace(Convert.ToChar(keyList[key]), Convert.ToChar(key)); // Decryption of one given char in the String (Actual line in the List)

            return result;
        }
    }
}
