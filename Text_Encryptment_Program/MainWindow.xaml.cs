﻿using System.Windows;
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

        private readonly AccessWindow UserAccess        = new();
        private readonly Random generateRandoms         = new();

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
            DecryptBox.Visibility = DecryptBox.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        public void EncryptBoxLabelAnim_Tick(object? sender, EventArgs e)
        {
            EncryptBox.Visibility = EncryptBox.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
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

        private async void EncryptingConfigIsActive(bool config)
        {
            if (config)
            {
                DisableAllButtons();

                EncryptBox.Content              = "Encrypting in Progress ...";
                Encrypt.Content                 = "ENCRYPTING ...";
                Encrypt.BorderBrush             = Brushes.GreenYellow;
                EncryptBox.Foreground           = Brushes.YellowGreen;

                EncryptBoxLabelAnim.Start();

                EncryptedText.Clear();
                DecryptedData.Clear();
            }
            else
            {
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
        }

        private async void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            EncryptingConfigIsActive(true);

            List<int> randoms       = [];
            List<string> encrKeys   = [];

            string encrKey          = "";

            bool integerRange1      = true;
            bool integerRange2      = false;
            bool integerRange3      = false;
            bool keyFound           = false;

            int rN                  = 0;

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
                    string buffer = Convert.ToString((int)DecryptedData[i] + "~" + rN.ToString() + ";");

                    keysTable.Add(DecryptedData[i], rN);
                    randoms.Add(rN);

                    foreach (var item in buffer)
                    {
                        encrKey += SwitchEncryptKey.EncryptedChar(item);
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
                        encrKey += SwitchEncryptKey.EncryptedChar(item);
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

            EncryptingConfigIsActive(false);
        }

        private async void DecryptingConfigIsActive(bool config)
        {
            if (config)
            {
                DisableAllButtons();

                DecryptedText.Clear();
                EncryptedData.Clear();
                DecryptedData.Clear();
                keysTable.Clear();

                DecryptBoxLabelAnim.Start();

                DecryptBox.Content              = "Decrypting in Progress ...";
                Decrypt.Content                 = "DECRYPTING ...";
                Decrypt.BorderBrush             = Brushes.GreenYellow;
                DecryptBox.Foreground           = Brushes.YellowGreen;
            }
            else
            {
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
        }
        private async void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            DecryptingConfigIsActive(true);

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

                        if (SwitchDecryptKey.KeyAddDecryptChar((int)item) == 'x')
                        {
                            endCharsCount++;
                        }
                        else if (SwitchDecryptKey.KeyAddDecryptChar((int)item) == 'y')
                        {
                            keyAdd = false;
                            continue;
                        }
                        else if (SwitchDecryptKey.KeyAddDecryptChar((int)item) == 'E')
                        {
                            DecryptBox.Content = "FAILED !!!";
                            DecryptedText.Text = "Decrypting Failed: A key or value in the encryption key of the text has an invalid value !";
                            DecryptBoxLabelAnim.Interval = TimeSpan.FromMilliseconds(150);
                            goto DecryptFail;
                        }
                        else
                        {
                            key += SwitchDecryptKey.KeyAddDecryptChar((int)item);
                        }
                    }
                    else if (!keyAdd)
                    {

                        if (SwitchDecryptKey.NoKeyAddDecryptChar((int)item) == 'x')
                        {
                            keysTable.Add(Convert.ToInt32(key), Convert.ToInt32(value));
                            key = "";
                            value = "";
                            keyAdd = true;
                            continue;
                        }
                        else if (SwitchDecryptKey.NoKeyAddDecryptChar((int)item) == 'E')
                        {
                            DecryptBox.Content = "FAILED !!!";
                            DecryptedText.Text = "Decrypting Failed: A key or value in the encryption key of the text has an invalid value !";
                            DecryptBoxLabelAnim.Interval = TimeSpan.FromMilliseconds(150);
                            goto DecryptFail;
                        }
                        else
                        {
                            value += SwitchDecryptKey.NoKeyAddDecryptChar((int)item);
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
                DecryptedText.Text = "Decrypting Failed: Encryption key for decryption operation could not be found in encrypted text !";
                DecryptBoxLabelAnim.Interval = TimeSpan.FromMilliseconds(150);
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

            DecryptingConfigIsActive(false);
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
                ManualText.BorderBrush = Brushes.YellowGreen;
                ManualText.Foreground = Brushes.Green;
                ManualText.Background = Brushes.Black;
            }
            else
            {
                DecryptedText.IsReadOnly = true;
                ManualText.BorderBrush = Brushes.OrangeRed;
                ManualText.Foreground = Brushes.DarkSeaGreen;
                ManualText.Background = Brushes.Black;
            }
        }

        private void ManualText2_Click(object sender, RoutedEventArgs e)
        {
            if (EncryptedText.IsReadOnly)
            {
                EncryptedText.IsReadOnly = false;
                ManualText2.BorderBrush = Brushes.YellowGreen;
                ManualText2.Foreground = Brushes.Green;
                ManualText2.Background = Brushes.Black;
            }
            else
            {
                EncryptedText.IsReadOnly = true;
                ManualText2.BorderBrush = Brushes.OrangeRed;
                ManualText2.Foreground = Brushes.DarkSeaGreen;
                ManualText2.Background = Brushes.Black;
            }
        }
        private void KeyTable_Click(object sender, RoutedEventArgs e)
        {

            if (!showKeyTable)
            {
                showKeyTable = true;

                KeyTable.BorderBrush    = Brushes.YellowGreen;
                KeyTable.Foreground     = Brushes.Green;
                KeyTable.Background     = Brushes.Black;

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

                KeyTable.BorderBrush    = Brushes.OrangeRed;
                KeyTable.Foreground     = Brushes.DarkSeaGreen;
                KeyTable.Background     = Brushes.Black;

                DecryptedText.Clear();

                foreach (var item in textCache)
                {
                    DecryptedText.Text = $"{item}";
                }

                DecryptedText.ScrollToHome();

                textCache.Clear();
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
            Quit.Background = Brushes.DarkRed;
            Quit.Foreground = Brushes.Yellow;
        }

        private void Quit_MouseLeave(object sender, MouseEventArgs e)
        {
            Quit.Background = Brushes.Black;
            Quit.Foreground = Brushes.OrangeRed;
        }

        private void KeyTable_MouseEnter(object sender, MouseEventArgs e)
        {
            KeyTable.Background = showKeyTable ? Brushes.Black : Brushes.DarkRed;
            KeyTable.Foreground = showKeyTable ? Brushes.Green : Brushes.DarkSeaGreen;
        }
        private void KeyTable_MouseLeave(object sender, MouseEventArgs e)
        {
            KeyTable.Background = Brushes.Black;
            KeyTable.Foreground = showKeyTable ? Brushes.Green : Brushes.DarkSeaGreen;
        }
        private void Decrypt_MouseEnter(object sender, MouseEventArgs e)
        {
            Decrypt.Background = Brushes.DarkRed;
            Decrypt.Foreground = Brushes.DarkSeaGreen;
        }

        private void Decrypt_MouseLeave(object sender, MouseEventArgs e)
        {
            Decrypt.Background = Brushes.Black;
            Decrypt.Foreground = Brushes.DarkSeaGreen;
        }

        private void OpenFile_MouseEnter(object sender, MouseEventArgs e)
        {
            OpenFile.Background = Brushes.DarkRed;
            OpenFile.Foreground = Brushes.DarkSeaGreen;
        }

        private void OpenFile_MouseLeave(object sender, MouseEventArgs e)
        {
            OpenFile.Background = Brushes.Black;
            OpenFile.Foreground = Brushes.DarkSeaGreen;
        }

        private void ClearBox_MouseEnter(object sender, MouseEventArgs e)
        {
            ClearBox.Background = Brushes.DarkRed;
            ClearBox.Foreground = Brushes.DarkSeaGreen;
        }

        private void ClearBox_MouseLeave(object sender, MouseEventArgs e)
        {
            ClearBox.Background = Brushes.Black;
            ClearBox.Foreground = Brushes.DarkSeaGreen;
        }

        private void Encrypt_MouseEnter(object sender, MouseEventArgs e)
        {
            Encrypt.Background = Brushes.DarkRed;
            Encrypt.Foreground = Brushes.DarkSeaGreen;
        }

        private void Encrypt_MouseLeave(object sender, MouseEventArgs e)
        {
            Encrypt.Background = Brushes.Black;
            Encrypt.Foreground = Brushes.DarkSeaGreen;
        }

        private void ManualText_MouseEnter(object sender, MouseEventArgs e)
        {
            ManualText.Background = DecryptedText.IsReadOnly ? Brushes.DarkRed : Brushes.Black;
            ManualText.Foreground = ManualText.Background == Brushes.DarkRed ? Brushes.DarkSeaGreen : DecryptedText.IsReadOnly ? Brushes.DarkSeaGreen : Brushes.Green;
        }

        private void ManualText_MouseLeave(object sender, MouseEventArgs e)
        {
            ManualText.Background = Brushes.Black;
            ManualText.Foreground = DecryptedText.IsReadOnly ? Brushes.DarkSeaGreen : Brushes.Green;
        }

        private void ClearEncrBox_MouseEnter(object sender, MouseEventArgs e)
        {
            ClearEncrBox.Background = Brushes.DarkRed;
            ClearEncrBox.Foreground = Brushes.DarkSeaGreen;
        }

        private void ClearEncrBox_MouseLeave(object sender, MouseEventArgs e)
        {
            ClearEncrBox.Background = Brushes.Black;
            ClearEncrBox.Foreground = Brushes.DarkSeaGreen;
        }

        private void Options_MouseEnter(object sender, MouseEventArgs e)
        {
            Options.Background = Brushes.DarkRed;
            Options.Foreground = Brushes.DarkSeaGreen;
        }

        private void Options_MouseLeave(object sender, MouseEventArgs e)
        {
            Options.Background = Brushes.Black;
            Options.Foreground = Brushes.DarkSeaGreen;
        }

        private void ManualText2_MouseEnter(object sender, MouseEventArgs e)
        {
            ManualText2.Background = EncryptedText.IsReadOnly ? Brushes.DarkRed : Brushes.Black;
            ManualText2.Foreground = ManualText2.Background == Brushes.DarkRed ? Brushes.DarkSeaGreen : EncryptedText.IsReadOnly ? Brushes.DarkSeaGreen : Brushes.Green;
        }

        private void ManualText2_MouseLeave(object sender, MouseEventArgs e)
        {
            ManualText2.Background = Brushes.Black;
            ManualText2.Foreground = EncryptedText.IsReadOnly ? Brushes.DarkSeaGreen : Brushes.Green;
        }
    }
}