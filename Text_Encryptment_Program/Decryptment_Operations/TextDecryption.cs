﻿using System;
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
        public static char DecryptText(char encryptedText, Dictionary<int, int> keyList, int key) 
        {
            string result;
            char decryptedChar = 'X';

            if(keyList.ContainsKey(key))
            {
                result = encryptedText.ToString().Replace(Convert.ToChar(keyList[key]), Convert.ToChar(key)); // Decryption of one given char in the String (Actual line in the List)
                return decryptedChar;
            }

            return encryptedText;
        }
    }
}
