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
        public static char keyAddDecryptChar(int Key)
        {
            switch (Key)
            {
                case 5470:
                    return '0';
                case 5471:
                    return '1';
                case 5472:
                    return '2';
                case 5473:
                    return '3';
                case 5474:
                    return '4';
                case 5475:
                    return '5';
                case 5476:
                    return '6';
                case 5477:
                    return '7';
                case 5478:
                    return '8';
                case 5479:
                    return '9';
                case 5487:
                    return 'y';
                case 7347:
                    return 'x';
                default: return 'E';
            }
        }

        public static char noKeyAddDecryptChar(int Key)
        {
            switch (Key)
            {
                case 5470:
                    return '0';
                case 5471:
                    return '1';
                case 5472:
                    return '2';
                case 5473:
                    return '3';
                case 5474:
                    return '4';
                case 5475:
                    return '5';
                case 5476:
                    return '6';
                case 5477:
                    return '7';
                case 5478:
                    return '8';
                case 5479:
                    return '9';
                case 5482:
                    return 'x';
                default: return 'E';
            }
        }
    }
}
