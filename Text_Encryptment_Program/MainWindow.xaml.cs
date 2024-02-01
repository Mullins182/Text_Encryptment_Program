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
using Text_Encryptment_Program.Other_Methods;

namespace Text_Encryptment_Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

            OpenFile.Content    = "Open File For Text-Encryption";
            Encrypt.Content     = "Encrypt Text";
            Decrypt.Content     = "Decrypt Text";
            KeyTable.Content    = "Show Encryption Key Table";
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
            List<string> Cache = TextData;

            int encryptChar = 33;
            int rN = 0;
            //EncryptedData = EncryptStart.EncryptText(TextData, rN, 130);

            foreach (var item in TextData)
            {
                EncryptedText.AppendText($"\n{item}");
            }

            await Task.Delay(5500);

            for (; encryptChar <= 126; encryptChar++)
            {
                for (int i = 8; i >= 0; i--)
                {
                    bool usedNumberFound = false;

                    Jump:

                    rN = generateRandoms.Next(161, 256);

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

                        await Task.Delay(15);
                    //}
                }

                usedRandoms.Add(rN);
                KeyDict.Add(encryptChar, rN);

                Cache = EncryptedData;
            }

            encryptChar = 32;
            EncryptionLogic(encryptChar, Cache);

            //encryptChar = 246;
            //EncryptionLogic(encryptChar, Cache);

            //encryptChar = 252;
            //EncryptionLogic(encryptChar, Cache);

            //encryptChar = 220;
            //EncryptionLogic(encryptChar, Cache);

            //encryptChar = 223;
            //EncryptionLogic(encryptChar, Cache);

            //encryptChar = 228;
            //EncryptionLogic(encryptChar, Cache);

            //encryptChar = 196;
            //EncryptionLogic(encryptChar, Cache);

            //encryptChar = 214;
            //EncryptionLogic(encryptChar, Cache);

            //encryptChar = 215;
            //EncryptionLogic(encryptChar, Cache);
        }

        private async void EncryptionLogic(int encryptChar, List<string> Cache)
        {
            int rN = 0;

            for (int i = 8; i >= 0; i--)
            {
                rN = generateRandoms.Next(191, 256);

                if (rN == 252 || rN == 246 || rN == 220 || rN == 223 || rN == 228 || rN == 196 || rN == 214 || rN == 215)
                {
                    i++;
                }
                else
                {
                    EncryptedData = EncryptStart.EncryptText(Cache, rN, encryptChar); // EncryptText(LISTE MIT ROHDATEN, RANDOM NUMBER, DEZIMALWERT UTF-16 TABELLE DES CHARS DER VERSCHL. WIRD)

                    EncryptedText.Clear();

                    foreach (var item in EncryptedData)
                    {
                        EncryptedText.AppendText($"\n{item}");
                    }

                    await Task.Delay(15);
                }

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

            foreach (var item in KeyDict)
            {
                DecryptedText.AppendText($"\n Key: {item.Key}");
                DecryptedText.AppendText($"\n Value: {item.Value}");
            }
        }

        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            List<string> DecryptedData = new List<string>();


            DecryptedData = Decryption.DecryptText(EncryptedData, KeyDict, 32);

            DecryptedText.Clear();

            foreach (var item in DecryptedData)
            {

                DecryptedText.AppendText($"\n{item}");
            }
        }
    }
}