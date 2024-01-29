using System.Linq;
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
using Text_Encryptment_Program.Encryptment_Operations;
using Text_Encryptment_Program.Other_Methods;

namespace Text_Encryptment_Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TextEncryption EncryptStart = new TextEncryption();

        List<string> EncryptedData = new List<string>();
        List<string> TextData = new List<string>();

        Random generateRandoms = new Random();                                    // Neue Instanz der Random Klasse erstellen !
                                                                                 // GenerateRandoms.Next() = Zufallszahl zwischen (x, y) erzeugen ! (x ist inklusiv, y ist exklusiv)

        public MainWindow()
        {
            InitializeComponent();

            OpenFile.Content    = "Open File For Text-Encryption";
            Encrypt.Content     = "Encrypt Text";
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
            List<int> usedRandoms = new List<int>();

            //Dictionary<int, int> Key = new Dictionary<int, int>();

            int encryptChar = 32;
            int rN = 0;

            EncryptedData = EncryptStart.EncryptText(TextData, rN, 174);

            foreach (var item in TextData)
            {
                EncryptedText.AppendText($"\n{item}");
            }

            await Task.Delay(5500);

            for (; encryptChar <= 126; encryptChar++)
            {
                for (int i = 8; i >= 0; i--)
                {
                    var usedNumber = false;

                    do
                    {
                        rN = generateRandoms.Next(191, 256);

                        foreach (var item in usedRandoms)
                        {
                            usedNumber = item.Equals(rN);
                        }

                    } while (usedNumber);

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

                usedRandoms.Add(rN);

                Cache = EncryptedData;
            }
         
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

            EncryptedData = EncryptStart.EncryptText(Cache, rN, encryptChar); // EncryptText(LISTE MIT ROHDATEN, RANDOM NUMBER, DEZIMALWERT UTF-16 TABELLE DES CHARS DER VERSCHL. WIRD)
            
            EncryptedText.Clear();

            foreach (var item in EncryptedData)
            {
                EncryptedText.AppendText($"\n{item}");
            }
        }
    }
}