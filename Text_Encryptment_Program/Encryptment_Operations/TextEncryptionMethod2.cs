namespace Text_Encryptment_Program.Encryptment_Operations
{
    public static class TextEncryptionMethod2
    {
        public static List<char> EncryptText(List<char> decrText, Dictionary<double, double> keyTable, int dictIndex)
        {
            List<char> workList = [.. decrText];

            for (int i = 0; i < workList.Count; i++)
            {
                if (workList[i] == (char)keyTable.Keys.ElementAt(dictIndex))
                {
                    workList[i] = (char)keyTable.Values.ElementAt(dictIndex);
                }
            }

            return workList;
        }
    }
}
