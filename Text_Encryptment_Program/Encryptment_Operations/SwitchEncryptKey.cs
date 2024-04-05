using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Encryptment_Program.Encryptment_Operations
{
    public static class SwitchEncryptKey
    {
        public static char encryptedChar(char Key)
        {
            switch (Key)
            {
                case '0':
                    return (char)5470;
                case '1':
                    return (char)5471;
                case '2':
                    return (char)5472;
                case '3':
                    return (char)5473;
                case '4':
                    return (char)5474;
                case '5':
                    return (char)5475;
                case '6':
                    return (char)5476;
                case '7':
                    return (char)5477;
                case '8':
                    return (char)5478;
                case '9':
                    return (char)5479;
                case '~':
                    return (char)5487;
                case ';':
                    return (char)5482;
                default: return 'X';
            }
        }
    }
}
