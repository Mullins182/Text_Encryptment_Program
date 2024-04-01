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

        private Dictionary<double, double> EncrKeyTable = [];

        private List<char> EncryptedData                = [];
        private List<char> DecryptedData                = [];
        private List<string> TextData                   = [];
        private List<string> textCache                  = [];

        private bool showKeyTable                       = false;
        private bool fastMode                           = false;
        private bool EncryptMeth2                       = true;

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

            List<int> randoms = new List<int>();

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
            EncrKeyTable.Clear();

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

                if(EncrKeyTable.Count == 0)
                {
                    EncrKeyTable.Add(DecryptedData[i], rN);
                    randoms.Add(rN);
                }

                foreach (var item in EncrKeyTable)
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
                    EncrKeyTable.Add(DecryptedData[i], rN);
                    randoms.Add(rN);
                }

                keyFound = false;
            }

            await Task.Delay(2000);

            if (!EncryptMeth2)
            {
                EncryptedData = TextEncryptionMethod1.EncryptText(DecryptedData, EncrKeyTable);

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
                for (int i = 0; i < EncrKeyTable.Count; i++)
                {
                    string encryptString = "";

                    EncryptedData = TextEncryptionMethod2.EncryptText(DecryptedData, EncrKeyTable, i);

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

                await Task.Delay(1777);

                EncryptedData = TextEncryptionMethod2.EncryptText(DecryptedData, EncrKeyTable);

                foreach (var item in EncryptedData)
                {
                    encryptLastString += item;
                }

                EncryptedText.Text = encryptLastString;
                // Encrypt Spaces, Newlines and Tabs END !!

                DecryptedData = [];                 // Remove Pointer to EncryptedData !
            }

            if  (EncrKeyTable.Count > 0)
            {
                EncryptedText.AppendText($"{(char)7348}{(char)7348}{(char)7348}");
                
                foreach (var item in EncrKeyTable)
                {
                    EncryptedText.AppendText($"{Math.Round((item.Key / 4.00), 5)}~{Math.Round((item.Value / 500.00), 5)};");

                    if (!fastMode && !EncryptMeth2)
                    {
                        EncryptedText.ScrollToEnd();
                    }
                }

                //EncryptedText.AppendText($"{(char)7347}{(char)7347}{(char)7347}");
                //EncryptedText.ScrollToEnd();
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

                DecryptedText.AppendText($"\nEncrypt Key Count: {EncrKeyTable.Count}");

                DecryptedText.AppendText("\n_____________________________________\n\n");

                foreach (var item in EncrKeyTable)
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
            EncrKeyTable.Clear();

            DecryptBoxLabelAnim.Start();

            DecryptBox.Content      = "Decrypting in Progress ...";
            Decrypt.Content         = "DECRYPTING ...";
            Decrypt.BorderBrush     = Brushes.GreenYellow;
            DecryptBox.Foreground   = Brushes.YellowGreen;

            bool keyAdd         = true;
            int keyCharsFound   = 0;
            string key          = "";
            string value        = "";
            double keyDouble    = 0.00;
            double valueDouble  = 0.00;
            int key_Dec         = 0;
            int value_Dec       = 0;

            foreach (var item in EncryptedText.Text)
            {
                if(keyCharsFound == 3)
                {
                    if (item == '~')
                    {
                        keyAdd      = false;

                        continue;
                    }
                    else if (item == ';')
                    {
                        if (key == "" || value == "")
                        {
                            DecryptBox.Content = "FAILED !!!";
                            DecryptedText.Text = "Decrypting Failed: A Key Or Value In The Encryption Key Of The Text Is Empty !";

                            goto DecryptFail;
                        }
                        else
                        {
                            keyDouble   = Convert.ToDouble(key);
                            valueDouble = Convert.ToDouble(value);
                            key_Dec     = Convert.ToInt32(keyDouble * 4.00);
                            value_Dec   = Convert.ToInt32(valueDouble * 500.00);

                            EncrKeyTable.Add(key_Dec, value_Dec);

                            key         = "";
                            value       = "";

                            keyAdd      = true;

                            continue;
                        }
                    }

                    if(keyAdd)
                    {
                        key     += item;
                    }
                    else if(!keyAdd)
                    {
                        value   += item;
                    }
                }
                else
                {
                    if(item == 7348)
                    {
                        keyCharsFound++;
                    }
                    else
                    {
                        EncryptedData.Add(item);
                    }
                }
            }

            if (EncrKeyTable.Count == 0)
            {
                DecryptBox.Content = "FAILED !!!";
                DecryptedText.Text = "Decrypting Failed: Encryption Key For Decryption Operation Could Not Be Found In Encrypted Textbox !";

                goto DecryptFail;
            }

            if (EncryptMeth2)
            {
                string decryptString = "";

                DecryptedText.AppendText(EncryptedText.Text);

                await Task.Delay(3500);

                DecryptedData = TextDecryptionMethod2.DecryptText(EncrKeyTable, EncryptedData);
                EncryptedData = DecryptedData;

                foreach (var item in DecryptedData)
                {
                    decryptString += item;
                }

                DecryptedText.Text = decryptString;
            }

            await Task.Delay(4400);

            if (!EncryptMeth2)
            {
                foreach (var item in EncryptedData)
                {
                    DecryptedData.Add(TextDecryptionMethod1.DecryptText(EncrKeyTable, item));
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
                for (int i = 0; i < EncrKeyTable.Count; i++)
                {
                    string decryptString = "";

                    DecryptedData = TextDecryptionMethod2.DecryptText(EncrKeyTable, EncryptedData, i);

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

        private void EncryptionMethodOne_Checked(object sender, RoutedEventArgs e)
        {
            EncryptionMethodOne.Foreground = Brushes.YellowGreen;
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
            EncryptionMethodTwo.Foreground = Brushes.YellowGreen;
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