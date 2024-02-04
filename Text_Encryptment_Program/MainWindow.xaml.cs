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
        DispatcherTimer InfoBlink = new DispatcherTimer();
        
        TextEncryption EncryptStart = new TextEncryption();
        TextDecryption Decryption = new TextDecryption();

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

            InfoBlink.Interval  = TimeSpan.FromMilliseconds(250);
            InfoBlink.Tick      += InfoBlink_Tick;

            OpenFile.Content    = "Open File For Text-Encryption";
            Encrypt.Content     = "Encrypt Text";
            Decrypt.Content     = "Decrypt Text";
            KeyTable.Content    = "Show Used Randoms AND Key Table";
        }

        private void InfoBlink_Tick(object? sender, EventArgs e)
        {
            //StatusInfoLabel.Visibility = Visibility.Visible;   // For use with async method

            //await Task.Delay(500);

            //StatusInfoLabel.Visibility = Visibility.Hidden;

            //await Task.Delay(500);

            if (StatusInfoLabel.Visibility == Visibility.Visible)
            {
                StatusInfoLabel.Visibility = Visibility.Hidden;
            }
            else
            {
                StatusInfoLabel.Visibility = Visibility.Visible;
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

            int encryptChar         = 33;
            int rN                  = 0;

            foreach (var item in TextData)
            {
                EncryptedText.AppendText($"\n{item}");
            }

            StatusInfoLabel.Content = "Encrypting";
            StatusInfoLabel.Visibility = Visibility.Visible;
            InfoBlink.Start();

            await Task.Delay(5500);

            for (; encryptChar <= 127; encryptChar++)
            {
                for (int i = 1; i > 0; i--)
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

                    //if (rN == 252 || rN == 246 || rN == 220 || rN == 223 || rN == 228 || rN == 196 || rN == 214 || rN == 215)
                    //{
                    //    goto Jump;
                    //}
                    //else
                    //{
                        EncryptedData = EncryptStart.EncryptText(Cache, rN, encryptChar); // EncryptText(LISTE MIT ROHDATEN, RANDOM NUMBER, DEZIMALWERT UTF-16 TABELLE DES CHARS DER VERSCHL. WIRD)

                        EncryptedText.Clear();

                        foreach (var item in EncryptedData)
                        {
                            EncryptedText.AppendText($"\n{item}");
                        }

                    //}
                }

                usedRandoms.Add(rN);
                KeyDict.Add(encryptChar, rN);

                Cache = EncryptedData;
                await Task.Delay(100);
            }

            usedNumberFound = false;

            encryptChar = 246;
            EncryptionLogic(encryptChar, Cache);

            encryptChar = 252;
            EncryptionLogic(encryptChar, Cache);

            encryptChar = 220;
            EncryptionLogic(encryptChar, Cache);

            encryptChar = 223;
            EncryptionLogic(encryptChar, Cache);

            encryptChar = 228;
            EncryptionLogic(encryptChar, Cache);

            encryptChar = 196;
            EncryptionLogic(encryptChar, Cache);

            encryptChar = 214;
            EncryptionLogic(encryptChar, Cache);

            encryptChar = 215;
            EncryptionLogic(encryptChar, Cache);

            encryptChar = 32;
            EncryptionLogic(encryptChar, Cache);

            await Task.Delay(2500);

            InfoBlink.Stop();
            StatusInfoLabel.Visibility = Visibility.Collapsed;
        }

        private async void EncryptionLogic(int encryptChar, List<string> Cache)
        {
            int rN = 0;

            bool integerRange1 = false;
            bool integerRange2 = true;
            bool integerRange3 = false;

            for (int i = 22; i >= 0; i--)
            {
                Jump2:

                bool usedNumberFound = false;

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

                //if (rN == 252 || rN == 246 || rN == 220 || rN == 223 || rN == 228 || rN == 196 || rN == 214 || rN == 215)
                //{
                //    i++;
                //}
                //else
                //{
                EncryptedData = EncryptStart.EncryptText(Cache, rN, encryptChar); // EncryptText(LISTE MIT ROHDATEN, RANDOM NUMBER, DEZIMALWERT UTF-16 TABELLE DES CHARS DER VERSCHL. WIRD)

                    EncryptedText.Clear();

                    foreach (var item in EncryptedData)
                    {
                        EncryptedText.AppendText($"\n{item}");
                    }

                await Task.Delay(80);
                //}

            }

            Cache = EncryptedData;

            usedRandoms.Add(rN);
            KeyDict.Add(encryptChar, rN);

            EncryptedData = EncryptStart.EncryptText(Cache, rN, encryptChar); // EncryptText(LISTE MIT ROHDATEN, RANDOM NUMBER, DEZIMALWERT UTF-16 TABELLE DES CHARS DER VERSCHL. WIRD)

            EncryptedText.Clear();

            foreach (var item in EncryptedData)
            {
                EncryptedText.AppendText($"\n{item}");
            }
        }

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
            StatusInfoLabel.Content = "Decrypting";
            StatusInfoLabel.Visibility = Visibility.Visible;
            InfoBlink.Start();
            
            List<string> DecryptedData = new List<string>();

            foreach (var item in EncryptedData)
            {
                string cacheDecrpt  = "";
                string cache        = item;

                for (int i = 32; i <= 127; i++) // Complete decryption of first Line in the List EncryptedData
                {
                    
                    cacheDecrpt = TextDecryption.DecryptText(cache, KeyDict, i);
                    cache       = cacheDecrpt;    // Cache becomes new modified string, and is given to decrypt method in the next loop round !
                    
                    //await Task.Delay(1);
                }

                cacheDecrpt = TextDecryption.DecryptText(cache, KeyDict, 246);
                cache = cacheDecrpt;    // Cache becomes new modified string, and is given to decrypt method in the next loop round !

                cacheDecrpt = TextDecryption.DecryptText(cache, KeyDict, 252);
                cache = cacheDecrpt;    // Cache becomes new modified string, and is given to decrypt method in the next loop round !
                
                cacheDecrpt = TextDecryption.DecryptText(cache, KeyDict, 220);
                cache = cacheDecrpt;    // Cache becomes new modified string, and is given to decrypt method in the next loop round !

                cacheDecrpt = TextDecryption.DecryptText(cache, KeyDict, 223);
                cache = cacheDecrpt;    // Cache becomes new modified string, and is given to decrypt method in the next loop round !

                cacheDecrpt = TextDecryption.DecryptText(cache, KeyDict, 228);
                cache = cacheDecrpt;    // Cache becomes new modified string, and is given to decrypt method in the next loop round !

                cacheDecrpt = TextDecryption.DecryptText(cache, KeyDict, 196);
                cache = cacheDecrpt;    // Cache becomes new modified string, and is given to decrypt method in the next loop round !

                cacheDecrpt = TextDecryption.DecryptText(cache, KeyDict, 214);
                cache = cacheDecrpt;    // Cache becomes new modified string, and is given to decrypt method in the next loop round !

                cacheDecrpt = TextDecryption.DecryptText(cache, KeyDict, 215);
                cache = cacheDecrpt;    // Cache becomes new modified string, and is given to decrypt method in the next loop round !


                DecryptedData.Add(cacheDecrpt); // The complete decrypted Line from the list is added to the list !

                DecryptedText.Clear();

                foreach (var item2 in DecryptedData)
                {
                    DecryptedText.AppendText($"\n{item2}");
                }

                await Task.Delay(250);
            }

            InfoBlink.Stop();
            StatusInfoLabel.Visibility = Visibility.Collapsed;
        }
    }
}