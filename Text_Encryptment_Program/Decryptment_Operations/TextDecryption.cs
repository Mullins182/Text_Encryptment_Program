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
                for(; key < 126; key++) 
                {
                    cache = item.Replace(Convert.ToChar(keyList[key]), Convert.ToChar(key));
                    result.Add(cache);                
                }

                key = 32;
            }

            return result;
        }
    }
}
