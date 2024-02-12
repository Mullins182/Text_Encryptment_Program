using System;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using System.Windows.Threading;
using Text_Encryptment_Program.Other_Methods;

namespace Text_Encryptment_Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer EncryptBoxLabelAnim = new DispatcherTimer(DispatcherPriority.Send);
        DispatcherTimer DecryptBoxLabelAnim = new DispatcherTimer(DispatcherPriority.Send);

        Dictionary<int, int> EncrKeyTable   = new Dictionary<int, int>();
        Dictionary<int, int> DecrKeyTable   = new Dictionary<int, int>();

        List<char> EncryptedData            = new List<char>();
        List<char> DecryptedData            = new List<char>();
        List<string> TextData               = new List<string>();
        List<string> textCache              = new List<string>();
        List<string> output                 = new List<string>();


        bool showKeyTable                   = false;

        Random generateRandoms              = new Random();                       // Neue Instanz der Random Klasse erstellen !
                                                                                 // GenerateRandoms.Next() = Zufallszahl zwischen (x, y) erzeugen ! (x ist inklusiv, y ist exklusiv)
        public MainWindow()
        {
            InitializeComponent();

            EncryptBoxLabelAnim.Interval    = TimeSpan.FromMilliseconds(500);
            DecryptBoxLabelAnim.Interval    = TimeSpan.FromMilliseconds(500);
            EncryptBoxLabelAnim.Tick        += EncryptBoxLabelAnim_Tick;
            DecryptBoxLabelAnim.Tick        += DecryptBoxLabelAnim_Tick;

            OpenFile.Content                = "Add Text From File";
            ClearBox.Content                = "Clear Box";
            Encrypt.Content                 = "Start Encrypting";
            Decrypt.Content                 = "Start Decrypting";
            KeyTable.Content                = "Show Used Randoms AND Key-Table";
            ManualText.Content              = "Edit Text";
        }

        private void DecryptBoxLabelAnim_Tick(object? sender, EventArgs e)
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

        private void EncryptBoxLabelAnim_Tick(object? sender, EventArgs e)
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

        private void DisableAllButtons()
        {
            Encrypt.IsEnabled       = false;
            Decrypt.IsEnabled       = false;
            KeyTable.IsEnabled      = false;
            OpenFile.IsEnabled      = false;
            ClearBox.IsEnabled      = false;
            ManualText.IsEnabled    = false;
        }

        private void EnableAllButtons()
        {
            Encrypt.IsEnabled       = true;
            Decrypt.IsEnabled       = true;
            KeyTable.IsEnabled      = true;
            OpenFile.IsEnabled      = true;
            ClearBox.IsEnabled      = true;
            ManualText.IsEnabled    = true;
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
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

            bool integerRange1      = true;
            bool integerRange2      = false;
            bool integerRange3      = false;
            int rN                  = 0;

            EncryptBox.Content      = "Encrypting in Progress ...";
            Encrypt.Content         = "ENCRYPTING ...";
            Encrypt.BorderBrush     = Brushes.GreenYellow;
            EncryptBox.Foreground   = Brushes.YellowGreen;

            EncryptBoxLabelAnim.Start();

            EncryptedText.Clear();
            DecryptedData.Clear();
            EncrKeyTable.Clear();
            DecrKeyTable.Clear();

            foreach (var item in DecryptedText.Text)                // DecryptedData wird mit Chars aus der Decrypted Text Box beschrieben !
            {
                //if ((int)item == 10 || (int)item == 13)
                //{

                //}
                //else
                //{
                    DecryptedData.Add(item);
                //}
            }

            //foreach (var item in DecryptedData)
            //{
            //    EncryptedText.AppendText($"{item}");
            //}

            for (int i = 0; i < DecryptedData.Count; i++) 
            {
                
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

                EncrKeyTable.Add(i, rN);
                DecrKeyTable.Add(i, DecryptedData[i]);
            }

            await Task.Delay(3200);

            EncryptedData = TextEncryption.EncryptText(DecryptedData, EncrKeyTable);

            //int Pos = 0;

            foreach (var item in EncryptedData)
            {
                //EncryptedText.Text = EncryptedText.Text.Replace((char)DecrKeyTable.ElementAt(Pos++).Value, item);

                EncryptedText.AppendText($"{item}");
                EncryptedText.ScrollToEnd();

                await Task.Delay(5);
            }

            EncryptedText.AppendText($"{(char)7348}{(char)7348}{(char)7348}");
            EncryptedText.ScrollToEnd();

            foreach (var item in DecrKeyTable)
            {
                EncryptedText.AppendText($"{item.Key},{item.Value};");
                EncryptedText.ScrollToEnd();
            }

            EncryptedText.AppendText($"{(char)7347}{(char)7347}{(char)7347}");
            EncryptedText.ScrollToEnd();

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

                //KeyTable.BorderBrush is  ? Brushes.GreenYellow : Brushes.OrangeRed;

                textCache.Add(DecryptedText.Text);

                DecryptedText.Clear();

                DecryptedText.AppendText("\n_____________________________________\n");

                DecryptedText.AppendText($"\nEncrypt Key Count: {EncrKeyTable.Count}\nDecryp Key Count: {DecrKeyTable.Count}");

                DecryptedText.AppendText("\n_____________________________________\n\n");

                foreach (var item in EncrKeyTable)
                {
                    DecryptedText.AppendText($"\nChar Pos (Key):\t{item.Key}");
                    DecryptedText.AppendText($"\n\nDecrypted Value:\t{DecrKeyTable[item.Key]}");
                    DecryptedText.AppendText($"\nEncrypted Value:\t{item.Value}");
                    DecryptedText.AppendText($"\n");
                    DecryptedText.AppendText($"---------------------------------------------");
                    DecryptedText.AppendText($"\n");
                }
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
            }
        }

        private async void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            DisableAllButtons();
            
            DecryptedText.Clear();
            EncryptedData.Clear();
            DecryptedData.Clear();
            DecrKeyTable.Clear();
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

            foreach (var item in EncryptedText.Text)
            {
                if(keyCharsFound == 3)
                {
                    if (item == ',')
                    {
                        keyAdd = false;

                        continue;
                    }
                    else if (item == ';')
                    {
                        keyAdd = true;
                        DecrKeyTable.Add(Convert.ToInt32(key), Convert.ToInt32(value));

                        continue;
                    }

                    if(keyAdd)
                    {
                        key = Convert.ToString(item);
                    }
                    else if(!keyAdd)
                    {
                        value = Convert.ToString(item);
                    }

                }

                if((int)item == 7348)
                {
                    keyCharsFound++;
                }


                EncryptedData.Add(item);
            }

            //foreach (var item in EncryptedData)
            //{
            //    DecryptedText.AppendText($"{item}");
            //}

            await Task.Delay(3000);

            int index = 0;

            foreach (var item in EncryptedData)
            {
                DecryptedData.Add(TextDecryption.DecryptText(item, DecrKeyTable, EncrKeyTable, index++));
            }

            foreach (var item in DecryptedData)
            {
                //DecryptedText.Text = DecryptedText.Text.Replace((char)EncrKeyTable.ElementAt(Pos).Value, item);

                //Pos++;

                DecryptedText.AppendText($"{item}");

                await Task.Delay(5);
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
            this.Close();
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
    }
}