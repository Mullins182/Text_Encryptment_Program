using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Encryptment_Program.Encryptment_Operations
{
    public static class SwitchEncryptKey
    {
        public static char EncryptedChar(char Key)
        {
            switch (Key)
            {
                case '0':
                    return (char)5821;
                case '1':
                    return (char)5822;
                case '2':
                    return (char)5823;
                case '3':
                    return (char)5824;
                case '4':
                    return (char)5825;
                case '5':
                    return (char)5826;
                case '6':
                    return (char)5828;
                case '7':
                    return (char)5829;
                case '8':
                    return (char)5830;
                case '9':
                    return (char)5831;
                case '~':
                    return (char)5867;
                case ';':
                    return (char)5863;
                default: return 'X';
            }
        }
    }
}