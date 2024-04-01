namespace Text_Encryptment_Program.Decryptment_Operations
{
    public static class TextDecryptionMethod2
    {
        public static List<char> DecryptText(Dictionary<double, double> EncrKeyTable, List<char> encryptedText, int dictIndex)
        {
            List<char> workList = [.. encryptedText];

            if (EncrKeyTable.Keys.ElementAt(dictIndex) == 32 || EncrKeyTable.Keys.ElementAt(dictIndex) == 10)
            {

            }
            else
            {
                for (int i = 0; i < workList.Count; i++)
                {
                    if (workList[i] == (char)EncrKeyTable.Values.ElementAt(dictIndex))
                    {
                        workList[i] = (char)EncrKeyTable.Keys.ElementAt(dictIndex);
                    }
                }
            }

            return workList;
        }

        public static List<char> DecryptText(Dictionary<double, double> EncrKeyTable, List<char> encryptedText)   // Deryption of Spaces, Newlines and Tabs
        {
            List<char> workList = [.. encryptedText];

            foreach (var item in EncrKeyTable)
            {
                if (item.Key == 32)
                {
                    for (int i = 0; i < workList.Count; i++)
                    {
                        if (workList[i] == item.Value)
                        {
                            workList[i] = (char)item.Key;
                        }
                    }
                }
                else if (item.Key == 10)
                {
                    for (int i = 0; i < workList.Count; i++)
                    {
                        if (workList[i] == item.Value)
                        {
                            workList[i] = (char)item.Key;
                        }
                    }
                }
            }

            return workList;
        }
    }
}
