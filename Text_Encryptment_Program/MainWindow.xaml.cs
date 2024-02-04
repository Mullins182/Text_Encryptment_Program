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
        
        //TextEncryption EncryptStart = new TextEncryption();
        //TextDecryption Decryption = new TextDecryption();

        Dictionary<int, int> KeyDict = new Dictionary<int, int>();
        List<string> EncryptedData = new List<string>();
        List<string> DecryptedData = new List<string>();
        List<string> TextData = new List<string>();
        List<int> usedRandoms = new List<int>();

        Random generateRandoms = new Random();                                    // Neue Instanz der Random Klasse erstellen !
                                                                                 // GenerateRandoms.Next() = Zufallszahl zwischen (x, y) erzeugen ! (x ist inklusiv, y ist exklusiv)
        public MainWindow()
        {
            InitializeComponent();

            EncryptBoxLabelAnim.Interval    = TimeSpan.FromMilliseconds(500);
            DecryptBoxLabelAnim.Interval    = TimeSpan.FromMilliseconds(500);
            EncryptBoxLabelAnim.Tick        += EncryptBoxLabelAnim_Tick;
            DecryptBoxLabelAnim.Tick        += DecryptBoxLabelAnim_Tick;

            OpenFile.Content    = "Open File For Text-Encryption";
            Encrypt.Content     = "Encrypt Text";
            Decrypt.Content     = "Decrypt Text";
            KeyTable.Content    = "Show Used Randoms AND Key Table";
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
            List<string> Cache      = TextData;

            bool usedNumberFound    = false;

            bool integerRange1      = true;
            bool integerRange2      = false;
            bool integerRange3      = false;

            int encryptChar         = 32;
            int rN                  = 0;

            foreach (var item in TextData)
            {
                EncryptedText.AppendText($"\n{item}");
            }

            EncryptBox.Content = "Encrypting in Progress ...";
            EncryptBox.Foreground = Brushes.YellowGreen;

            EncryptBoxLabelAnim.Start();

            await Task.Delay(4000);

            //EncryptBoxLabelAnim.Stop();

            for (; encryptChar <= 126; encryptChar++)
            {
                for (int i = 8; i > 0; i--)
                {
                    Jump:

                    usedNumberFound = false;

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

                    foreach (var item in usedRandoms)
                    {
                        usedNumberFound = item.Equals(rN);

                        if(usedNumberFound)
                        {
                            goto Jump;
                        }
                    }


                    EncryptedData = TextEncryption.EncryptText(Cache, rN, encryptChar); // EncryptText(LISTE MIT ROHDATEN, RANDOM NUMBER, DEZIMALWERT UTF-16 TABELLE DES CHARS DER VERSCHL. WIRD)
                    
                    EncryptedText.Clear();

                    foreach (var item in EncryptedData)
                    {
                        EncryptedText.AppendText($"\n{item}");
                    }

                    await Task.Delay(6);
                }

                usedRandoms.Add(rN);
                KeyDict.Add(encryptChar, rN);

                Cache = EncryptedData;
            }

            encryptChar = 161;

            for (; encryptChar <= 255; encryptChar++)
            {
                for (int i = 8; i > 0; i--)
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

                    foreach (var item in usedRandoms)
                    {
                        usedNumberFound = item.Equals(rN);

                        if (usedNumberFound)
                        {
                            goto Jump2;
                        }
                    }


                    EncryptedData = TextEncryption.EncryptText(Cache, rN, encryptChar); // EncryptText(LISTE MIT ROHDATEN, RANDOM NUMBER, DEZIMALWERT UTF-16 TABELLE DES CHARS DER VERSCHL. WIRD)

                    EncryptedText.Clear();

                    foreach (var item in EncryptedData)
                    {
                        EncryptedText.AppendText($"\n{item}");
                    }

                    await Task.Delay(6);

                }

                usedRandoms.Add(rN);
                KeyDict.Add(encryptChar, rN);

                Cache = EncryptedData;
            }


            //encryptChar = 246;
            //EncryptionLogic(encryptChar, Cache);

            //await Task.Delay(250);

            //encryptChar = 252;
            //EncryptionLogic(encryptChar, Cache);

            //await Task.Delay(250);

            //encryptChar = 220;
            //EncryptionLogic(encryptChar, Cache);

            //await Task.Delay(250);

            //encryptChar = 223;
            //EncryptionLogic(encryptChar, Cache);

            //await Task.Delay(250);

            //encryptChar = 228;
            //EncryptionLogic(encryptChar, Cache);

            //await Task.Delay(250);

            //encryptChar = 196;
            //EncryptionLogic(encryptChar, Cache);

            //await Task.Delay(250);

            //encryptChar = 214;
            //EncryptionLogic(encryptChar, Cache);

            //await Task.Delay(250);

            //encryptChar = 215;
            //EncryptionLogic(encryptChar, Cache);

            //await Task.Delay(500);

            //encryptChar = 32;
            //EncryptionLogic(encryptChar, Cache);

            //await Task.Delay(500);

            //await Task.Delay(8000);

            EncryptBox.Content = "Encryption Successfully";
            EncryptBoxLabelAnim.Interval = TimeSpan.FromMilliseconds(150);

            await Task.Delay(3500);

            EncryptBoxLabelAnim.Stop();
            EncryptBoxLabelAnim.Interval = TimeSpan.FromMilliseconds(500);
            EncryptBox.Foreground = Brushes.OrangeRed;
            EncryptBox.Content = "Encrypted Text";
            EncryptBox.Visibility = Visibility.Visible;
        }

        //private async void EncryptionLogic(int encryptChar, List<string> Cache)
        //{
        //    List<string> EncrCache = Cache;
            
        //    int rN = 0;

        //    bool integerRange1 = false;
        //    bool integerRange2 = true;
        //    bool integerRange3 = false;

        //    for (int i = 22; i > 0; i--)
        //    {
        //        Jump2:

        //        bool usedNumberFound = false;

        //        if (integerRange1)
        //        {
        //            rN = generateRandoms.Next(5632, 5789);

        //            integerRange1 = false;
        //            integerRange2 = true;
        //        }
        //        else if (integerRange2)
        //        {
        //            rN = generateRandoms.Next(5792, 5873);

        //            integerRange2 = false;
        //            integerRange3 = true;
        //        }
        //        else if (integerRange3)
        //        {
        //            rN = generateRandoms.Next(5376, 5631);

        //            integerRange3 = false;
        //            integerRange1 = true;
        //        }

        //        foreach (var item in usedRandoms)
        //        {
        //            usedNumberFound = item.Equals(rN);

        //            if (usedNumberFound)
        //            {
        //                goto Jump2;
        //            }
        //        }

        //        EncryptedData = TextEncryption.EncryptText(EncrCache, rN, encryptChar); // EncryptText(LISTE MIT ROHDATEN, RANDOM NUMBER, DEZIMALWERT UTF-16 TABELLE DES CHARS DER VERSCHL. WIRD)
        //        EncrCache = EncryptedData;

        //            EncryptedText.Clear();

        //            foreach (var item in EncryptedData)
        //            {
        //                EncryptedText.AppendText($"\n{item}");
        //            }

        //        await Task.Delay(250);
        //    }

        //    //Cache = EncryptedData;

        //    usedRandoms.Add(rN);
        //    KeyDict.Add(encryptChar, rN);

        //    EncryptedData = TextEncryption.EncryptText(EncrCache, rN, encryptChar); // EncryptText(LISTE MIT ROHDATEN, RANDOM NUMBER, DEZIMALWERT UTF-16 TABELLE DES CHARS DER VERSCHL. WIRD)

        //    EncryptedText.Clear();

        //    foreach (var item in EncryptedData)
        //    {
        //        EncryptedText.AppendText($"\n{item}");
        //    }
        //}

        private void KeyTable_Click(object sender, RoutedEventArgs e)
        {
            DecryptedText.Clear();

            foreach (var item in usedRandoms)
            {
                DecryptedText.AppendText($"\n\n{item}");
            }

            DecryptedText.AppendText("\n\n____________________________________________________________________________\n");

            foreach (var item in KeyDict)
            {
                DecryptedText.AppendText($"\nKey:   {item.Key}");
                DecryptedText.AppendText($"\nValue: {item.Value}");
                DecryptedText.AppendText($"\n");
            }

            DecryptedText.AppendText("\n\n____________________________________________________________________________\n");

            DecryptedText.AppendText($"\nUsed Numbers Count:{usedRandoms.Count}\nKey Table Count: {KeyDict.Count}");
        }

        private async void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            DecryptedText.Clear();
            DecryptBox.Content = "Decrypting in Progress ...";
            DecryptBox.Foreground = Brushes.YellowGreen;
            DecryptBoxLabelAnim.Start();

            await Task.Delay(3000);
            
            List<string> DecryptedData = new List<string>();

            foreach (var item in EncryptedData)
            {
                string cacheDecrpt  = "";
                string cache        = item;

                for (int i = 32; i <= 255; i++) // Complete decryption of first Line in the List EncryptedData
                {
                    
                    cacheDecrpt = TextDecryption.DecryptText(cache, KeyDict, i);
                    cache       = cacheDecrpt;    // Cache becomes new modified string, and is given to decrypt method in the next loop round !
                    
                    //await Task.Delay(1);
                }

                DecryptedData.Add(cacheDecrpt); // The complete decrypted Line from the list is added to the list !

                DecryptedText.Clear();

                foreach (var item2 in DecryptedData)
                {
                    DecryptedText.AppendText($"\n{item2}");
                }

                await Task.Delay(500);
            }

            DecryptBoxLabelAnim.Interval = TimeSpan.FromMilliseconds(150);
            DecryptBox.Content = "Decryption Successfully";

            await Task.Delay(3500);

            DecryptBoxLabelAnim.Interval = TimeSpan.FromMilliseconds(500);
            DecryptBox.Foreground = Brushes.OrangeRed;
            DecryptBox.Content = "Decrypted Text";
            DecryptBox.Visibility = Visibility.Visible;
            DecryptBoxLabelAnim.Stop();
        }
    }
}