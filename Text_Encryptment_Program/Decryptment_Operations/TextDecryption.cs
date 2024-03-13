using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Text_Encryptment_Program
{
    public static class TextDecryption                         // Hiermit wird Text Char-weise mithilfe eines Keytable entschlüsselt !
    {
        public static char DecryptText(Dictionary<int, int> EncrKeyTable, int item) 
        {
            char result = ' ';

            foreach (var item1 in EncrKeyTable)
            {
                if(item1.Value == item)
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
