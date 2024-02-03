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
            string cacheDecrpt = "";
            string cache = "";

            //List<string> textList = new List<string>();
            //string cache2 = "";

            foreach (var item in encryptedText)
            {
                    cache       = item;

                for (int i = 32; i < 127; i++)
                {
                    cacheDecrpt = cache.Replace(Convert.ToChar(keyList[i]), Convert.ToChar(i));
                    cache = cacheDecrpt;
                }
                    result.Add(cacheDecrpt);
            }

            return result;
        }

        //public string DecryptedString(string text, Dictionary<int, int> keys) 
        //{
        //    text.Replace(Convert.ToChar(keys[32]), Convert.ToChar(32));

        //    return text;
        //}
    }
}
