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

        Dictionary<int, int> KeyDict    = new Dictionary<int, int>();
        List<string> EncryptedData      = new List<string>();
        List<string> DecryptedData      = new List<string>();
        List<char> encryptionCache      = new List<char>();
        List<string> TextData           = new List<string>();
        List<string> textCache          = new List<string>();

        bool showKeyTable = false;

        Random generateRandoms          = new Random();                           // Neue Instanz der Random Klasse erstellen !
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
            TextData.Clear();

            // Configure open file dialog box
            var dialog              = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName         = "Select a txt file !"; // Default file name
            dialog.DefaultExt       = ".txt"; // Default file extension
            dialog.Filter           = "Text documents (.txt)|*.txt"; // Filter files by extension
            dialog.InitialDirectory = Environment.CurrentDirectory; // Sets the initial Dir for File Dialog Box to actual working Directory !

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;

                DecryptedData = LoadContentIntoDecryptedText.ReadFileData(filename);

                foreach (var item in DecryptedData)
                {
                    TextData.Add(item);
                    DecryptedText.AppendText($"\n{item}");
                }
            }
        }

        private async void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            DisableAllButtons();

            bool usedNumberFound    = false;
            bool integerRange1      = true;
            bool integerRange2      = false;
            bool integerRange3      = false;
            int encryptChar         = 33;
            int rN                  = 0;

            EncryptBox.Content      = "Encrypting in Progress ...";
            Encrypt.Content         = "ENCRYPTING ...";
            Encrypt.BorderBrush     = Brushes.GreenYellow;
            EncryptBox.Foreground   = Brushes.YellowGreen;

            EncryptBoxLabelAnim.Start();

            EncryptedText.Clear();
            DecryptedData.Clear();
            KeyDict.Clear();

            foreach (var item in DecryptedText.Text)                // DecryptedData wird mit Chars gefüllt !
            {
                DecryptedData.Add(Convert.ToString(item));
                encryptionCache.Add(item);
            }

            foreach (var item in DecryptedData)
            {
                EncryptedText.AppendText($"{item}");
            }

            await Task.Delay(3200);
                                    
            for (; encryptChar <= 126; encryptChar++)
            {
                int charCounter = 0;

                foreach (var item in encryptionCache)
                {
                    if(encryptChar == Convert.ToInt64(Convert.ToChar(item)))
                    {
                        charCounter++;
                    }
                }

                for (int i = 1 + charCounter; i > 0; i--)
                {
                    Jump:

                    usedNumberFound   = false;

                    if(integerRange1)
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

                    foreach (var item in KeyDict.Values)
                    {
                        usedNumberFound = item.Equals(rN);

                        if(usedNumberFound)
                        {
                            goto Jump;
                        }
                    }                                        

                    EncryptedData = TextEncryption.EncryptText(DecryptedData, rN, encryptChar); // EncryptText(LISTE MIT ROHDATEN, RANDOM NUMBER, DEZIMALWERT UTF-16 TABELLE DES CHARS DER VERSCHL. WIRD)
                    
                    EncryptedText.Clear();

                    foreach (var item in EncryptedData)
                    {
                        EncryptedText.AppendText($"{item}");
                    }

                    await Task.Delay(10);
                }

                KeyDict.Add(encryptChar, rN);

                DecryptedData.Clear();

                foreach(var item in EncryptedData)
                {
                    DecryptedData.Add(item);
                }                
            }

            encryptChar = 161;

            for (; encryptChar <= 255; encryptChar++)
            {
                int charCounter = 0;

                foreach (var item in encryptionCache)
                {
                    if (encryptChar == Convert.ToInt64(Convert.ToChar(item)))
                    {
                        charCounter++;
                    }
                }

                for (int i = 1 + charCounter; i > 0; i--)
                {
                    Jump2:

                    usedNumberFound = false;

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

                    foreach (var item in KeyDict.Values)
                    {
                        usedNumberFound = item.Equals(rN);

                        if (usedNumberFound)
                        {
                            goto Jump2;
                        }
                    }


                    EncryptedData = TextEncryption.EncryptText(DecryptedData, rN, encryptChar); // EncryptText(LISTE MIT ROHDATEN, RANDOM NUMBER, DEZIMALWERT UTF-16 TABELLE DES CHARS DER VERSCHL. WIRD)

                    EncryptedText.Clear();

                    foreach (var item in EncryptedData)
                    {
                        EncryptedText.AppendText($"{item}");
                    }

                    await Task.Delay(10);

                }

                KeyDict.Add(encryptChar, rN);

                DecryptedData.Clear();

                foreach (var item in EncryptedData)
                {
                    DecryptedData.Add(item);
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

        private void KeyTable_Click(object sender, RoutedEventArgs e)       // Hier noch Daten in Liste sichern und beim erneuten Click wieder in Box laden !
        {

            if (!showKeyTable) 
            {
                showKeyTable = true;

                KeyTable.BorderBrush = Brushes.Green;

                //KeyTable.BorderBrush is  ? Brushes.GreenYellow : Brushes.OrangeRed;

                textCache.Add(DecryptedText.Text);

                DecryptedText.Clear();

                DecryptedText.AppendText("\n_________________________\n");

                DecryptedText.AppendText($"\nKey Table Count: {KeyDict.Count}");

                DecryptedText.AppendText("\n_________________________\n\n");

                foreach (var item in KeyDict)
                {
                    DecryptedText.AppendText($"\nKey:   {item.Key}");
                    DecryptedText.AppendText($"\nValue: {item.Value}");
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

            DecryptBoxLabelAnim.Start();

            DecryptBox.Content      = "Decrypting in Progress ...";
            Decrypt.Content         = "DECRYPTING ...";
            Decrypt.BorderBrush     = Brushes.GreenYellow;
            DecryptBox.Foreground   = Brushes.YellowGreen;

            //List<string> Test = new List<string>();

            EncryptedData.Add(EncryptedText.Text);

            await Task.Delay(3000);

            foreach (var item in EncryptedData)
            {
                string cacheDecrpt  = "";
                string cache        = item;

                for (int i = 33; i <= 255; i++) // Complete decryption of first Line in the List EncryptedData
                {                    
                    cacheDecrpt     = TextDecryption.DecryptText(cache, KeyDict, i);
                    cache           = cacheDecrpt;    // Cache becomes new modified string, and is given to decrypt method in the next loop round !
                }

                DecryptedData.Add(cacheDecrpt); // The complete decrypted Line from the list is added to the list !

                DecryptedText.Clear();

                foreach (var item2 in DecryptedData)
                {
                    DecryptedText.AppendText($"{item2}");
                }

                await Task.Delay(500);
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