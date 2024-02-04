using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Text_Encryptment_Program
{
    public class TextDecryption                         // Hiermit wird der Text einer Textdatei entschlüsselt !
    {
        public TextDecryption() { }

        public List<string> DecryptText(List<string> encryptedText, Dictionary<int, int> keyList, int key) 
        {
            List<string> result = new List<string>();

            foreach (var item in encryptedText)
            {
                string cacheDecrpt  = "";
                string cache        = item;

                for (int i = key; i <= 126; i++) // Complete decryption of first Line in the List
                {
                    cacheDecrpt = cache.Replace(Convert.ToChar(keyList[i]), Convert.ToChar(i)); // Decryption of one given char in the String (Actual line in the List)
                    cache = cacheDecrpt;    // Cache becomes new modified string, and is given to replace method in the next loop round !
                }
                    result.Add(cacheDecrpt); // The complete decrypted Line from the list is added to the list result !
            }

            return result;
        }
    }
}
