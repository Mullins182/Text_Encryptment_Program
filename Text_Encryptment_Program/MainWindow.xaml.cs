using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Text_Encryptment_Program.Decryptment_Operations;
using Text_Encryptment_Program.Encryptment_Operations;
using Text_Encryptment_Program.Other_Methods;

namespace Text_Encryptment_Program
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer EncryptBoxLabelAnim     = new(DispatcherPriority.Send);
        private DispatcherTimer DecryptBoxLabelAnim     = new(DispatcherPriority.Send);

        private AccessWindow UserAccess                 = new();
        private Random generateRandoms                  = new();

        private Dictionary<double, double> keysTable    = [];

        private List<char> EncryptedData                = [];
        private List<char> DecryptedData                = [];
        private List<string> TextData                   = [];
        private List<string> textCache                  = [];

        private bool showKeyTable                       = false;
        private bool fastMode                           = false;
        private bool EncryptMeth2                       = true;
        private bool AutoEncryptMeth                    = true;

        private static readonly ulong access_code       = 52565854;
        private static ulong access_code_input          = 0;

        public MainWindow()
        {
            InitializeComponent();

            button_stackpanel.Visibility    = Visibility.Hidden;
            Options.Visibility              = Visibility.Hidden;

            AuthorizeAccess();
        }

        public static ulong GetAccessCode()
        {
            return access_code;
        }

        public async void AuthorizeAccess()
        {
            DecryptBox.Content              = "ACCESS DENIED";
            EncryptBox.Content              = "ACCESS DENIED";

            await Task.Delay(1000);

            UserAccess.Show();
            UserAccess.Focus();
            UserAccess.Topmost              = true;

            do
            {
                await Task.Delay(500);

                access_code_input = UserAccess.accessCode;

                if (!UserAccess.IsLoaded)
                {
                    this.Close();
                }
            }
            while (access_code_input != access_code);

            UserAccess.CodeBox.Foreground = Brushes.YellowGreen;

            UserAccess.pling.Play();

            await Task.Delay(1500);

            UserAccess.code_accepted.Play();

            UserAccess.CodeBox.Text = "Code Accepted !!!";

            await Task.Delay(2500);

            UserAccess.window_popup.Close();
            UserAccess.code_accepted.Close();
            UserAccess.keypad_sound.Close();
            UserAccess.keypad_reset.Close();

            await Task.Delay(333);

            UserAccess.Close();

            await Task.Delay(1000);

            EncryptBoxLabelAnim.Interval    = TimeSpan.FromMilliseconds(500);
            DecryptBoxLabelAnim.Interval    = TimeSpan.FromMilliseconds(500);
            EncryptBoxLabelAnim.Tick        += EncryptBoxLabelAnim_Tick;
            DecryptBoxLabelAnim.Tick        += DecryptBoxLabelAnim_Tick;

            button_stackpanel.Visibility    = Visibility.Visible;
            Options.Visibility              = Visibility.Visible;

            DecryptBox.Content              = "Decrypted Text";
            EncryptBox.Content              = "Encrypted Text";
        }

        public void DecryptBoxLabelAnim_Tick(object? sender, EventArgs e)
        {
            if(DecryptBox.Visibility == Visibility.Visible) 
            {
                DecryptBox.Visibility = Visibility.Hidden;            
            }
            else
            {
                DecryptBox.Visibility = Visibility.Visible;
            }
        }

        public void EncryptBoxLabelAnim_Tick(object? sender, EventArgs e)
        {
            if (EncryptBox.Visibility == Visibility.Visible)
            {
                EncryptBox.Visibility = Visibility.Hidden;
            }
            else
            {
                EncryptBox.Visibility = Visibility.Visible;
            }
        }

        public void DisableAllButtons()
        {
            Encrypt.IsEnabled       = false;
            Decrypt.IsEnabled       = false;
            KeyTable.IsEnabled      = false;
            OpenFile.IsEnabled      = false;
            ClearBox.IsEnabled      = false;
            ClearEncrBox.IsEnabled  = false;
            ManualText.IsEnabled    = false;
            ManualText2.IsEnabled   = false;
        }

        public void EnableAllButtons()
        {
            Encrypt.IsEnabled       = true;
            Decrypt.IsEnabled       = true;
            KeyTable.IsEnabled      = true;
            OpenFile.IsEnabled      = true;
            ClearBox.IsEnabled      = true;
            ClearEncrBox.IsEnabled  = true;
            ManualText.IsEnabled    = true;
            ManualText2.IsEnabled   = true;
        }

        public void OpenFile_Click(object sender, RoutedEventArgs e)
        {

            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Select a txt file !"; // Default file name
            dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
            dialog.InitialDirectory = Environment.CurrentDirectory; // Sets the initial Dir for File Dialog Box to actual working Directory !

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;

                TextData = LoadContentIntoDecryptedText.ReadFileData(filename);

                foreach (var item in TextData)
                {
                    DecryptedText.AppendText($"\n{item}");
                }
            }
        }

        private async void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            DisableAllButtons();

            List<int> randoms       = new List<int>();
            List<string> encrKeys   = new List<string>();

            string encrKey          = "";

            bool integerRange1      = true;
            bool integerRange2      = false;
            bool integerRange3      = false;
            bool keyFound           = false;

            int rN                  = 0;

            EncryptBox.Content      = "Encrypting in Progress ...";
            Encrypt.Content         = "ENCRYPTING ...";
            Encrypt.BorderBrush     = Brushes.GreenYellow;
            EncryptBox.Foreground   = Brushes.YellowGreen;

            EncryptBoxLabelAnim.Start();

            EncryptedText.Clear();
            DecryptedData.Clear();
            keysTable.Clear();

            if (AutoEncryptMeth)
            {
                if (DecryptedText.Text.Length > 6500)
                {
                    EncryptionMethodOne.IsChecked   = true;
                    FastEncrDecrOnOff.IsChecked     = true;
                }
                else
                {
                    EncryptionMethodTwo.IsChecked   = true;
                }
            }

            if (EncryptMeth2)
            {
                EncryptedText.AppendText(DecryptedText.Text);
            }

            await Task.Delay(2000);

            foreach (var item in DecryptedText.Text)
            {
                DecryptedData.Add(item);
            }

            for (int i = 0; i < DecryptedData.Count; i++) 
            {
                JumpHere:
                
                if (integerRange1)
                {
                    rN = generateRandoms.Next(5632, 5789);

                    integerRange1 = false;
                    integerRange2 = true;
                }
                else if (integerRange2)
                {
                    rN = generateRandoms.Next(5792, 5873);

                    integerRange2 = false;
                    integerRange3 = true;
                }
                else if (integerRange3)
                {
                    rN = generateRandoms.Next(5376, 5631);

                    integerRange3 = false;
                    integerRange1 = true;
                }

                if (randoms.Count != 0)
                {
                    foreach (var item in randoms)
                    {
                        if (rN == item)
                        {
                            goto JumpHere;
                        }
                    }
                }

                if(keysTable.Count == 0)
                {
                    string buffer = Convert.ToString((int)DecryptedData[i] + "~" + Convert.ToString(rN) + ";");

                    keysTable.Add(DecryptedData[i], rN);
                    randoms.Add(rN);

                    foreach (var item in buffer)
                    {
                        switch (item)
                        {
                            case '0':
                                encrKey += (char)7312;
                                break;
                            case '1':
                                encrKey += (char)7313;
                                break;
                            case '2':
                                encrKey += (char)7314;
                                break;
                            case '3':
                                encrKey += (char)7315;
                                break;
                            case '4':
                                encrKey += (char)7316;
                                break;
                            case '5':
                                encrKey += (char)7317;
                                break;
                            case '6':
                                encrKey += (char)7318;
                                break;
                            case '7':
                                encrKey += (char)7319;
                                break;
                            case '8':
                                encrKey += (char)7320;
                                break;
                            case '9':
                                encrKey += (char)7321;
                                break;
                            case '~':
                                encrKey += (char)7322;
                                break;
                            case ';':
                                encrKey += (char)7323;
                                break;
                            default: break;
                        }
                    }

                    encrKeys.Add(encrKey);
                    encrKey = "";
                }

                foreach (var item in keysTable)
                {
                    if (DecryptedData[i] == item.Key)
                    {
                        keyFound = true;
                    }
                }

                if (keyFound)
                {
                    
                }
                else
                {
                    string buffer = Convert.ToString((int)DecryptedData[i] + "~" + Convert.ToString(rN) + ";");

                    keysTable.Add(DecryptedData[i], rN);
                    randoms.Add(rN);

                    foreach (var item in buffer)
                    {
                        switch(item)
                        {
                            case '0':
                                encrKey += (char)7312;
                                break;
                            case '1':
                                encrKey += (char)7313;
                                break;
                            case '2':
                                encrKey += (char)7314;
                                break;
                            case '3':
                                encrKey += (char)7315;
                                break;
                            case '4':
                                encrKey += (char)7316;
                                break;
                            case '5':
                                encrKey += (char)7317;
                                break;
                            case '6':
                                encrKey += (char)7318;
                                break;
                            case '7':
                                encrKey += (char)7319;
                                break;
                            case '8':
                                encrKey += (char)7320;
                                break;
                            case '9':
                                encrKey += (char)7321;
                                break;
                            case '~':
                                encrKey += (char)7322;
                                break;
                            case ';':
                                encrKey += (char)7323;
                                break;
                                default: break;
                        }
                    }

                    encrKeys.Add(encrKey);
                    encrKey = "";
                }

                keyFound = false;
            }

            await Task.Delay(2000);

            if (!EncryptMeth2)
            {
                EncryptedData = TextEncryptionMethod1.EncryptText(DecryptedData, keysTable);

                foreach (var item in EncryptedData)
                {
                    EncryptedText.AppendText($"{item}");

                    if(!fastMode)
                    {
                        EncryptedText.ScrollToEnd();
                        await Task.Delay(5);
                    }
                }
            }
            else
            {
                for (int i = 0; i < keysTable.Count; i++)
                {
                    string encryptString = "";

                    EncryptedData = TextEncryptionMethod2.EncryptText(DecryptedData, keysTable, i);

                    foreach (var item in EncryptedData)
                    {
                        encryptString += item;
                    }

                    EncryptedText.Text = encryptString;
                    DecryptedData = EncryptedData;      // DecryptedData Points now to EncryptedData !
                    await Task.Delay(133);
                }

                // Encrypt Method for Spaces, Newlines and Tabs

                string encryptLastString = "";

                await Task.Delay(2777);

                EncryptedData = TextEncryptionMethod2.EncryptText(9, DecryptedData, keysTable);        // 9 is Tabstop
                foreach (var item in EncryptedData)
                {
                    encryptLastString += item;
                }

                EncryptedText.Text = encryptLastString;

                DecryptedData = EncryptedData;      // DecryptedData Points now to EncryptedData !

                encryptLastString = "";

                await Task.Delay(3777);

                EncryptedData = TextEncryptionMethod2.EncryptText(32, DecryptedData, keysTable);      // 32 is Space Char

                foreach (var item in EncryptedData)
                {
                    encryptLastString += item;
                }

                EncryptedText.Text = encryptLastString;

                DecryptedData = EncryptedData;      // DecryptedData Points now to EncryptedData !

                encryptLastString = "";

                await Task.Delay(3777);

                EncryptedData = TextEncryptionMethod2.EncryptText(10, DecryptedData, keysTable);     // 10 is NewLine

                foreach (var item in EncryptedData)
                {
                    encryptLastString += item;
                }

                EncryptedText.Text = encryptLastString;

                // Encrypt Spaces, Newlines and Tabs END !!

                DecryptedData = [];                 // Remove Pointer to EncryptedData !
            }

            await Task.Delay(3777);

            if (keysTable.Count > 0)
            {
                int posKeysList = encrKeys.Count - 1;

                rN = generateRandoms.Next(0, EncryptedText.Text.Length);

                EncryptedText.Text = EncryptedText.Text.Insert(rN, $"{(char)7347}{(char)7347}{(char)7347}");    // Achtung 7347 & 7348 werden durch insert
                EncryptedText.Text = EncryptedText.Text.Insert(rN, $"{(char)7348}{(char)7348}{(char)7348}");    // in der Ausgabe vertauscht !!!

                foreach (var item in encrKeys)
                {
                    EncryptedText.Text = EncryptedText.Text.Insert(rN + 3,
                        $"{encrKeys.ElementAt(posKeysList--)}");

                    if (!fastMode && !EncryptMeth2)
                    {
                        EncryptedText.ScrollToEnd();
                    }
                }
            }

            EncryptBox.Content              = "Successfully Encrypted";
            Encrypt.BorderBrush             = Brushes.OrangeRed;
            Encrypt.Content                 = "Start Encrypting";

            EncryptBoxLabelAnim.Interval    = TimeSpan.FromMilliseconds(150);

            await Task.Delay(3500);

            EncryptBoxLabelAnim.Stop();
            EncryptBoxLabelAnim.Interval    = TimeSpan.FromMilliseconds(500);
            EncryptBox.Foreground           = Brushes.OrangeRed;
            EncryptBox.Content              = "Encrypted Text";
            EncryptBox.Visibility           = Visibility.Visible;

            EnableAllButtons();
        }

        private void KeyTable_Click(object sender, RoutedEventArgs e)
        {

            if (!showKeyTable) 
            {
                showKeyTable = true;

                KeyTable.BorderBrush = Brushes.Green;

                textCache.Add(DecryptedText.Text);

                DecryptedText.Clear();

                DecryptedText.AppendText("\n_____________________________________\n");

                DecryptedText.AppendText($"\nEncrypt Key Count: {keysTable.Count}");

                DecryptedText.AppendText("\n_____________________________________\n\n");

                foreach (var item in keysTable)
                {
                    DecryptedText.AppendText($"\nDecrypted (Key):\t{item.Key}");
                    DecryptedText.AppendText($"\nEncrypted Value:\t{item.Value}");
                    DecryptedText.AppendText($"\n");
                    DecryptedText.AppendText($"---------------------------------------------");
                    DecryptedText.AppendText($"\n");
                }

                DecryptedText.ScrollToHome();
            }
            else
            {
                showKeyTable = false;

                KeyTable.BorderBrush = Brushes.OrangeRed;

                DecryptedText.Clear();

                foreach (var item in textCache)
                {
                    DecryptedText.Text = $"{item}";
                }

                DecryptedText.ScrollToHome();

                textCache.Clear();
            }
        }

        private async void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            DisableAllButtons();
            
            DecryptedText.Clear();
            EncryptedData.Clear();
            DecryptedData.Clear();
            keysTable.Clear();

            DecryptBoxLabelAnim.Start();

            DecryptBox.Content      = "Decrypting in Progress ...";
            Decrypt.Content         = "DECRYPTING ...";
            Decrypt.BorderBrush     = Brushes.GreenYellow;
            DecryptBox.Foreground   = Brushes.YellowGreen;

            bool keyAdd         = true;
            int startCharsCount = 0;
            int endCharsCount   = 0;
            string key          = "";
            string value        = "";

            foreach (var item in EncryptedText.Text)
            {
                if (endCharsCount == 3)
                {
                    startCharsCount = 0;
                }

                if(startCharsCount == 3)
                {
                    if (keyAdd)
                    {
                        switch ((int)item)
                        {
                            case 7312:
                                key += '0';
                                break;
                            case 7313:
                                key += '1';
                                break;
                            case 7314:
                                key += '2';
                                break;
                            case 7315:
                                key += '3';
                                break;
                            case 7316:
                                key += '4';
                                break;
                            case 7317:
                                key += '5';
                                break;
                            case 7318:
                                key += '6';
                                break;
                            case 7319:
                                key += '7';
                                break;
                            case 7320:
                                key += '8';
                                break;
                            case 7321:
                                key += '9';
                                break;
                            case 7322:
                                keyAdd = false;
                                continue;
                            case 7347:
                                endCharsCount++;
                                continue;
                            default: break;
                        }
                    }
                    else if (!keyAdd)
                    {
                        switch ((int)item)
                        {
                            case 7312:
                                value += '0';
                                break;
                            case 7313:
                                value += '1';
                                break;
                            case 7314:
                                value += '2';
                                break;
                            case 7315:
                                value += '3';
                                break;
                            case 7316:
                                value += '4';
                                break;
                            case 7317:
                                value += '5';
                                break;
                            case 7318:
                                value += '6';
                                break;
                            case 7319:
                                value += '7';
                                break;
                            case 7320:
                                value += '8';
                                break;
                            case 7321:
                                value += '9';
                                break;
                            case 7322:
                                keyAdd = false;
                                continue;
                            case 7323:
                                if (key == "" || value == "")
                                {
                                    DecryptBox.Content = "FAILED !!!";
                                    DecryptedText.Text = "Decrypting Failed: A Key Or Value In The Encryption Key Of The Text Is Missing !";
                                    goto DecryptFail;
                                }
                                else
                                {
                                    keysTable.Add(Convert.ToInt32(key), Convert.ToInt32(value));
                                    key     = "";
                                    value   = "";
                                    keyAdd  = true;
                                    continue;
                                }
                            default: break;
                        }
                    }
                }
                else
                {
                    if(item == 7348)
                    {
                        startCharsCount++;
                    }
                    else
                    {
                        EncryptedData.Add(item);
                    }
                }
            }

            if (keysTable.Count == 0)
            {
                DecryptBox.Content = "FAILED !!!";
                DecryptedText.Text = "Decrypting Failed: Encryption Key For Decryption Operation Could Not Be Found In Encrypted Textbox !";

                goto DecryptFail;
            }

            if (AutoEncryptMeth)
            {
                if (EncryptedData.Count > 6500)
                {
                    EncryptionMethodOne.IsChecked = true;
                    FastEncrDecrOnOff.IsChecked = true;
                }
                else
                {
                    EncryptionMethodTwo.IsChecked = true;
                }
            }

            if (EncryptMeth2)                   // Decryption of Spaces, Tabs and NewLines
            {
                string decryptString = "";

                DecryptedText.AppendText(EncryptedText.Text);

                await Task.Delay(3500);

                DecryptedData = TextDecryptionMethod2.DecryptText(10, keysTable, EncryptedData);     // 10 = NewLines
                EncryptedData = DecryptedData;

                foreach (var item in DecryptedData)
                {
                    decryptString += item;
                }

                DecryptedText.Text = decryptString;

                decryptString = "";

                await Task.Delay(3500);

                DecryptedData = TextDecryptionMethod2.DecryptText(32, keysTable, EncryptedData);     // 32 = Spaces
                EncryptedData = DecryptedData;

                foreach (var item in DecryptedData)
                {
                    decryptString += item;
                }

                DecryptedText.Text = decryptString;

                decryptString = "";

                await Task.Delay(3500);

                DecryptedData = TextDecryptionMethod2.DecryptText(9, keysTable, EncryptedData);      // 9 = Tabstops
                EncryptedData = DecryptedData;

                foreach (var item in DecryptedData)
                {
                    decryptString += item;
                }

                DecryptedText.Text = decryptString;
            }

            // Decryption of Tabs, Spaces and Newline END !

            await Task.Delay(4400);

            if (!EncryptMeth2)
            {
                foreach (var item in EncryptedData)
                {
                    DecryptedData.Add(TextDecryptionMethod1.DecryptText(keysTable, item));
                }

                foreach (var item in DecryptedData)
                {
                    DecryptedText.AppendText($"{item}");
                    DecryptedText.ScrollToEnd();

                    if(!fastMode)
                    {
                        await Task.Delay(5);
                    }
                }
            }
            else
            {
                for (int i = 0; i < keysTable.Count; i++)
                {
                    string decryptString = "";

                    DecryptedData = TextDecryptionMethod2.DecryptText(keysTable, EncryptedData, i);

                    foreach (var item in DecryptedData)
                    {
                        decryptString += item;
                    }

                    DecryptedText.Text = decryptString;
                    EncryptedData = DecryptedData;      // EncryptedData Points now to DecryptedData !
                    await Task.Delay(133);
                }

                EncryptedData = [];                     // Remove Pointer to DecryptedData
            }

            if (fastMode)
            {
                DecryptedText.ScrollToHome();
            }

            DecryptBoxLabelAnim.Interval    = TimeSpan.FromMilliseconds(150);
            DecryptBox.Content              = "Successfully Decrypted";

            DecryptFail:                                                        // JUMP POINT !

            Decrypt.BorderBrush             = Brushes.OrangeRed;
            Decrypt.Content                 = "Start Decrypting";

            await Task.Delay(3500);

            DecryptBoxLabelAnim.Interval    = TimeSpan.FromMilliseconds(500);
            DecryptBox.Foreground           = Brushes.OrangeRed;
            DecryptBox.Content              = "Decrypted Text";
            DecryptBox.Visibility           = Visibility.Visible;
            DecryptBoxLabelAnim.Stop();

            EnableAllButtons();

        }

        // Button Click-Events

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            UserAccess.Close();

            this.Close();
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            if (optionsCanvas.Visibility == Visibility.Visible)
            {
                optionsCanvas.Visibility = Visibility.Collapsed;
            }
            else
            {
                optionsCanvas.Visibility = Visibility.Visible;
            }
        }

        private void ClearBox_Click(object sender, RoutedEventArgs e)
        {
            DecryptedText.Clear();
        }

        private void ClearEncrBox_Click(object sender, RoutedEventArgs e)
        {
            EncryptedText.Clear();
        }

        private void ManualText_Click(object sender, RoutedEventArgs e)
        {
            if (DecryptedText.IsReadOnly)
            {
                DecryptedText.IsReadOnly = false;
                ManualText.BorderBrush = Brushes.Green;
            }
            else
            {
                DecryptedText.IsReadOnly = true;
                ManualText.BorderBrush = Brushes.OrangeRed;
            }
        }

        private void ManualText2_Click(object sender, RoutedEventArgs e)
        {
            if (EncryptedText.IsReadOnly)
            {
                EncryptedText.IsReadOnly = false;
                ManualText2.BorderBrush = Brushes.Green;
            }
            else
            {
                EncryptedText.IsReadOnly = true;
                ManualText2.BorderBrush = Brushes.OrangeRed;
            }
        }

        // Options Canvas Check-Events

        private void AutoEncryptMethod_Click(object sender, RoutedEventArgs e)
        {
            if (AutoEncryptMeth)
            {
                AutoEncryptMeth = false;
                AutoEncryptMethod.Foreground = Brushes.OrangeRed;
                AutoEncryptMethod.BorderBrush = Brushes.Black;
            }
            else
            {
                AutoEncryptMeth = true;
                AutoEncryptMethod.Foreground = Brushes.Green;
                AutoEncryptMethod.BorderBrush = Brushes.Green;
            }
        }

        private void EncryptionMethodOne_Checked(object sender, RoutedEventArgs e)
        {
            EncryptionMethodOne.Foreground = Brushes.Green;
            FastEncrDecrOnOff.Visibility = Visibility.Visible;
            EncryptMeth2 = false;
        }

        private void EncryptionMethodOne_Unchecked(object sender, RoutedEventArgs e)
        {
            FastEncrDecrOnOff.IsChecked = false;
            EncryptionMethodOne.Foreground = Brushes.OrangeRed;
            FastEncrDecrOnOff.Visibility = Visibility.Collapsed;
        }

        private void EncryptionMethodTwo_Checked(object sender, RoutedEventArgs e)
        {
            EncryptionMethodTwo.Foreground = Brushes.Green;
            EncryptMeth2 = true;
        }

        private void EncryptionMethodTwo_Unchecked(object sender, RoutedEventArgs e)
        {
            EncryptionMethodTwo.Foreground = Brushes.OrangeRed;
        }

        private void FastEncrDecrOnOff_Checked(object sender, RoutedEventArgs e)
        {
            fastMode = true;
        }

        private void FastEncrDecrOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            fastMode = false;
        }

        // Mouse Enter / Leave Events

        private void Quit_MouseEnter(object sender, MouseEventArgs e)
        {
            Quit.Background = Brushes.Green;
            Quit.Foreground = Brushes.Black;
        }

        private void Quit_MouseLeave(object sender, MouseEventArgs e)
        {
            Quit.Background = Brushes.Black;
            Quit.Foreground = Brushes.OrangeRed;
        }

        private void KeyTable_MouseEnter(object sender, MouseEventArgs e)
        {
            KeyTable.Background = Brushes.Green;
            KeyTable.Foreground = Brushes.Black;
        }
        private void KeyTable_MouseLeave(object sender, MouseEventArgs e)
        {
            KeyTable.Background = Brushes.Black;
            KeyTable.Foreground = Brushes.DarkSeaGreen;
        }
        private void Decrypt_MouseEnter(object sender, MouseEventArgs e)
        {
            Decrypt.Background = Brushes.Green;
            Decrypt.Foreground = Brushes.Black;
        }

        private void Decrypt_MouseLeave(object sender, MouseEventArgs e)
        {
            Decrypt.Background = Brushes.Black;
            Decrypt.Foreground = Brushes.DarkSeaGreen;
        }

        private void OpenFile_MouseEnter(object sender, MouseEventArgs e)
        {
            OpenFile.Background = Brushes.Green;
            OpenFile.Foreground = Brushes.Black;
        }

        private void OpenFile_MouseLeave(object sender, MouseEventArgs e)
        {
            OpenFile.Background = Brushes.Black;
            OpenFile.Foreground = Brushes.DarkSeaGreen;
        }

        private void ClearBox_MouseEnter(object sender, MouseEventArgs e)
        {
            ClearBox.Background = Brushes.Green;
            ClearBox.Foreground = Brushes.Black;
        }

        private void ClearBox_MouseLeave(object sender, MouseEventArgs e)
        {
            ClearBox.Background = Brushes.Black;
            ClearBox.Foreground = Brushes.DarkSeaGreen;
        }

        private void Encrypt_MouseEnter(object sender, MouseEventArgs e)
        {
            Encrypt.Background = Brushes.Green;
            Encrypt.Foreground = Brushes.Black;
        }

        private void Encrypt_MouseLeave(object sender, MouseEventArgs e)
        {
            Encrypt.Background = Brushes.Black;
            Encrypt.Foreground = Brushes.DarkSeaGreen;
        }

        private void ManualText_MouseEnter(object sender, MouseEventArgs e)
        {
            ManualText.Background = Brushes.Green;
            ManualText.Foreground = Brushes.Black;
        }

        private void ManualText_MouseLeave(object sender, MouseEventArgs e)
        {
            ManualText.Background = Brushes.Black;
            ManualText.Foreground = Brushes.DarkSeaGreen;
        }

        private void ClearEncrBox_MouseEnter(object sender, MouseEventArgs e)
        {
            ClearEncrBox.Background = Brushes.Green;
            ClearEncrBox.Foreground = Brushes.Black;
        }

        private void ClearEncrBox_MouseLeave(object sender, MouseEventArgs e)
        {
            ClearEncrBox.Background = Brushes.Black;
            ClearEncrBox.Foreground = Brushes.DarkSeaGreen;
        }

        private void Options_MouseEnter(object sender, MouseEventArgs e)
        {
            Options.Background = Brushes.Green;
            Options.Foreground = Brushes.Black;
        }

        private void Options_MouseLeave(object sender, MouseEventArgs e)
        {
            Options.Background = Brushes.Black;
            Options.Foreground = Brushes.DarkSeaGreen;
        }

        private void ManualText2_MouseEnter(object sender, MouseEventArgs e)
        {
            ManualText2.Background = Brushes.Green;
            ManualText2.Foreground = Brushes.Black;
        }

        private void ManualText2_MouseLeave(object sender, MouseEventArgs e)
        {
            ManualText2.Background = Brushes.Black;
            ManualText2.Foreground = Brushes.DarkSeaGreen;
        }
    }
}