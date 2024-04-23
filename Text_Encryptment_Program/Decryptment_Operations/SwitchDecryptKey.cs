using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Text_Encryptment_Program.Decryptment_Operations
{
    public static class SwitchDecryptKey
    {
        public static char KeyAddDecryptChar(int Key)
        {
            switch (Key)
            {
                case 5821:
                    return '0';
                case 5822:
                    return '1';
                case 5823:
                    return '2';
                case 5824:
                    return '3';
                case 5825:
                    return '4';
                case 5826:
                    return '5';
                case 5828:
                    return '6';
                case 5829:
                    return '7';
                case 5830:
                    return '8';
                case 5831:
                    return '9';
                case 5867:
                    return 'y';
                case 7347:
                    return 'x';
                default: return 'E';
            }
        }

        public static char NoKeyAddDecryptChar(int Key)
        {
            switch (Key)
            {
                case 5821:
                    return '0';
                case 5822:
                    return '1';
                case 5823:
                    return '2';
                case 5824:
                    return '3';
                case 5825:
                    return '4';
                case 5826:
                    return '5';
                case 5828:
                    return '6';
                case 5829:
                    return '7';
                case 5830:
                    return '8';
                case 5831:
                    return '9';
                case 5863:
                    return 'x';
                default: return 'E';
            }
        }
    }
}