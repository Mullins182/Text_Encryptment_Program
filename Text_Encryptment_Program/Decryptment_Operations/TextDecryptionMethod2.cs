namespace Text_Encryptment_Program.Decryptment_Operations
{
    public static class TextDecryptionMethod2
    {
        public static List<char> DecryptText(Dictionary<double, double> EncrKeyTable, List<char> encryptedText, int dictIndex)
        {
            List<char> workList = [.. encryptedText];

            for (int i = 0; i < workList.Count; i++)
            {
                if (workList[i] == (char)EncrKeyTable.Values.ElementAt(dictIndex))
                {
                    workList[i] = (char)EncrKeyTable.Keys.ElementAt(dictIndex);
                }
            }

            return workList;
        }
    }
}
