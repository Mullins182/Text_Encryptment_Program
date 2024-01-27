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
using Text_Encryptment_Program.Other_Methods;

namespace Text_Encryptment_Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<string> TextData = new List<string>();

            TextData = LoadContentIntoDecryptedText.ReadFileData("TextSample.txt");

            foreach (var item in TextData)
            {
                DecryptedText.AppendText($"\n{item}");
            }
        }
    }
}