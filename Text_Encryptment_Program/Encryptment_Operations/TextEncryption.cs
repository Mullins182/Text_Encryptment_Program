namespace Text_Encryptment_Program
{
    public static class TextEncryption                     // Hiermit wird der Text der Decrypted Text Box verschlüsselt !
    {
        public static List<char> EncryptText(List<char> decrText, Dictionary<int, int> keyTable) 
        {
            List<char> result = new List<char>();

            foreach (var item in decrText)
            {
                result.Add(item);
            }

            for (int i = 0; i < result.Count(); i++)
            {
                foreach (var item_Dict in keyTable)
                {
                    if(item_Dict.Key == (int)result[i])
                    {
                        result[i] = (char)item_Dict.Value;
                    }
                }
            }

            return result;
        }
    }
}
