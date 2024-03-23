namespace Text_Encryptment_Program
{
    public class TextDecryption                         // Hiermit wird Text Char-weise mithilfe eines Keytable entschlüsselt !
    {
        public static char DecryptText(Dictionary<int, int> EncrKeyTable, char item) 
        {
            char result = 'X';

            foreach (var item1 in EncrKeyTable)
            {
                if(item1.Value == (int)item)
                {
                    result = (char)item1.Key;
                }
            }

            return result;
        }
    }
}
