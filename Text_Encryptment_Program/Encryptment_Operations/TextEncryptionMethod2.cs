namespace Text_Encryptment_Program.Encryptment_Operations
{
    public static class TextEncryptionMethod2
    {
        public static List<char> EncryptText(List<char> decrText, Dictionary<double, double> keyTable, int dictIndex)
        {
            List<char> workList = [.. decrText];

            if (keyTable.Keys.ElementAt(dictIndex) == 32 || keyTable.Keys.ElementAt(dictIndex) == 10)
            {

            }
            else
            {
                for (int i = 0; i < workList.Count; i++)
                {
                    if (workList[i] == (char)keyTable.Keys.ElementAt(dictIndex))
                    {
                        workList[i] = (char)keyTable.Values.ElementAt(dictIndex);
                    }
                }
            }

            return workList;
        }

        public static List<char> EncryptText(List<char> decrText, Dictionary<double, double> keyTable)   // Enryption of Spaces, Newlines and Tabs
        {
            List<char> workList = [.. decrText];

            for (int i = 0; i < workList.Count; i++)
            {
                if (workList[i] == 32)
                {
                    foreach (var item in keyTable)
                    {
                        if (item.Key == 32)
                        {
                            workList[i] = (char)item.Value;
                        }
                    }
                }
                else if (workList[i] == 10)
                {
                    foreach (var item in keyTable)
                    {
                        if (item.Key == 10)
                        {
                            workList[i] = (char)item.Value;
                        }
                    }
                }
            }

            return workList;
        }
    }
}
