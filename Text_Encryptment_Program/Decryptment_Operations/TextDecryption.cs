using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Text_Encryptment_Program
{
    public class TextDecryption                         // Hiermit wird Text Char-weise mithilfe eines Keytable entschlüsselt !
    {
        public static char DecryptText(Dictionary<int, int> EncrKeyTable, char item) 
        {
            char result = 'X';

            foreach (var item1 in EncrKeyTable)
            {
                if(item1.Value == Convert.ToInt32(item))
                {
                    result = Convert.ToChar(item1.Key);
                }
                else
                {
                    
                }
            }

            return result;
        }
    }
}
