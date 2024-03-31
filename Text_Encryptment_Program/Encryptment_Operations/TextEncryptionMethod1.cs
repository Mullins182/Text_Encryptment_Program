namespace Text_Encryptment_Program
{
    public static class TextEncryptionMethod1                     // Hiermit wird der Text der Decrypted Text Box verschlüsselt !
    {
        public static List<char> EncryptText(List<char> decrText, Dictionary<double, double> keyTable) 
        {
            List<char> result = [.. decrText];

            for (int i = 0; i < result.Count; i++)
            {
                foreach (var item_Dict in keyTable)
                {
                    if(item_Dict.Key == result[i])
                    {
                        result[i] = (char)item_Dict.Value;
                    }
                }
            }

            return result;
        }
    }
}
