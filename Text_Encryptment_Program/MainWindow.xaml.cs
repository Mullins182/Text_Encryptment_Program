using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Text_Encryptment_Program.Other_Methods;

namespace Text_Encryptment_Program
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer EncryptBoxLabelAnim = new(DispatcherPriority.Send);
        private DispatcherTimer DecryptBoxLabelAnim = new(DispatcherPriority.Send);

        private AccessWindow UserAccess             = new();
        private Random generateRandoms              = new();

        private Dictionary<int, int> EncrKeyTable   = [];

        private List<char> EncryptedData            = [];
        private List<char> DecryptedData            = [];
        private List<string> TextData               = [];
        private List<string> textCache              = [];

        private bool showKeyTable                   = false;
        private bool fastMode                       = false;

        private static readonly ulong access_code   = 52565854;
        private static ulong access_code_input      = 0;


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

            await Task.Delay(2000);

            foreach (var item in DecryptedText.Text)                // DecryptedData wird mit Chars aus der Decrypted Text Box beschrieben !
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

            EncryptedData = TextEncryption.EncryptText(DecryptedData, EncrKeyTable);

            foreach (var item in EncryptedData)
            {
                EncryptedText.AppendText($"{item}");
                EncryptedText.ScrollToEnd();

                if(!fastMode)
                {
                    await Task.Delay(5);
                }
            }

            if  (EncrKeyTable.Count > 0)
            {
                EncryptedText.AppendText($"{(char)7348}{(char)7348}{(char)7348}");
                EncryptedText.ScrollToEnd();

                foreach (var item in EncrKeyTable)
                {
                    EncryptedText.AppendText($"{item.Key * 7}~{item.Value * 2};");
                    EncryptedText.ScrollToEnd();
                }

                //EncryptedText.AppendText($"{(char)7347}{(char)7347}{(char)7347}");
                //EncryptedText.ScrollToEnd();
            }

            if (fastMode)
            {
                EncryptedText.ScrollToHome();
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

        private void KeyTable_Click(object sender, RoutedEventArgs e)       // Hier noch Daten in Liste sichern und beim erneuten Click wieder in Box laden !
        {

            if (!showKeyTable) 
            {
                showKeyTable = true;

                KeyTable.BorderBrush = Brushes.Green;

                //KeyTable.BorderBrush is  ? Brushes.GreenYellow : Brushes.OrangeRed;   // Test vereinfachte if-abfrage

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

            await Task.Delay(2000);

            bool keyAdd         = true;
            int keyCharsFound   = 0;
            string key          = "";
            string value        = "";
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
                        key_Dec     = Convert.ToInt32(key) / 7;
                        value_Dec   = Convert.ToInt32(value) / 2;

                        EncrKeyTable.Add(key_Dec, value_Dec);

                        key         = "";
                        value       = "";

                        keyAdd      = true;

                        continue;
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

            await Task.Delay(2000);

            foreach (var item in EncryptedData)
            {
                DecryptedData.Add(TextDecryption.DecryptText(EncrKeyTable, item));
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

            if (fastMode)
            {
                DecryptedText.ScrollToHome();
            }

            DecryptBoxLabelAnim.Interval    = TimeSpan.FromMilliseconds(150);
            DecryptBox.Content              = "Successfully Decrypted";

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

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            UserAccess.Close();

            this.Close();
        }
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
        private void ClearBox_Click(object sender, RoutedEventArgs e)
        {
            DecryptedText.Clear();
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

        private void ManualText_Click(object sender, RoutedEventArgs e)
        {
            if(DecryptedText.IsReadOnly) 
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

        private void ClearEncrBox_Click(object sender, RoutedEventArgs e)
        {
            EncryptedText.Clear();
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

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            if(optionsCanvas.Visibility == Visibility.Visible) 
            {
                optionsCanvas.Visibility = Visibility.Collapsed;
            }
            else
            {
                optionsCanvas.Visibility = Visibility.Visible;
            }
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

        private void FastEncrDecrOnOff_Checked(object sender, RoutedEventArgs e)
        {
            fastMode = true;
        }

        private void FastEncrDecrOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            fastMode = false;
        }

        private void ManualText2_Click(object sender, RoutedEventArgs e)
        {
            if(EncryptedText.IsReadOnly)
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