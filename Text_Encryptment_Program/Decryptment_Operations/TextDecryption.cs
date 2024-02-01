using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Encryptment_Program
{
    public class TextDecryption                         // Hiermit wird der Text einer Textdatei entschlüsselt !
    {
        public TextDecryption() { }

        public List<string> DecryptText(List<string> text, Dictionary<int, int> keyList, int key) 
        {
            List<string> result = new List<string>();
            string cache;

            foreach (var item in text)
            {
                for (int i = key; i < 126; i++)
                {
                    cache = item.Replace(Convert.ToChar(keyList[i]), Convert.ToChar(i));
                    //cache = Convert.ToString(keyList[i]);
                    result.Add(cache);
                }
            }

            return result;
        }
    }
}
