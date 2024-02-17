using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Text_Encryptment_Program
{
    /// <summary>
    /// Interaktionslogik für AccessWindow.xaml
    /// </summary>
    public partial class AccessWindow : Window
    {
        public ulong accessCode = 0;

        public AccessWindow()
        {
            InitializeComponent();
        }

        private void one_Click(object sender, RoutedEventArgs e)
        {
            if(CodeBox.Text == "ENTER ACCESS CODE !")
            {
                CodeBox.Clear();
            }

            if(CodeBox.Text.Length < 10) 
            {
                CodeBox.AppendText("1");
            }
        }

        private void two_Click(object sender, RoutedEventArgs e)
        {
            if (CodeBox.Text == "ENTER ACCESS CODE !")
            {
                CodeBox.Clear();
            }

            if (CodeBox.Text.Length < 10)
            {
                CodeBox.AppendText("2");
            }
        }

        private void three_Click(object sender, RoutedEventArgs e)
        {
            if (CodeBox.Text == "ENTER ACCESS CODE !")
            {
                CodeBox.Clear();
            }

            if (CodeBox.Text.Length < 10)
            {
                CodeBox.AppendText("3");
            }
        }

        private void four_Click(object sender, RoutedEventArgs e)
        {
            if (CodeBox.Text == "ENTER ACCESS CODE !")
            {
                CodeBox.Clear();
            }

            if (CodeBox.Text.Length < 10)
            {
                CodeBox.AppendText("4");
            }

        }

        private void five_Click(object sender, RoutedEventArgs e)
        {
            if (CodeBox.Text == "ENTER ACCESS CODE !")
            {
                CodeBox.Clear();
            }

            if (CodeBox.Text.Length < 10)
            {
                CodeBox.AppendText("5");
            }

        }

        private void six_Click(object sender, RoutedEventArgs e)
        {
            if (CodeBox.Text == "ENTER ACCESS CODE !")
            {
                CodeBox.Clear();
            }

            if (CodeBox.Text.Length < 10)
            {
                CodeBox.AppendText("6");
            }

        }

        private void seven_Click(object sender, RoutedEventArgs e)
        {
            if (CodeBox.Text == "ENTER ACCESS CODE !")
            {
                CodeBox.Clear();
            }

            if (CodeBox.Text.Length < 10)
            {
                CodeBox.AppendText("7");
            }

        }

        private void eight_Click(object sender, RoutedEventArgs e)
        {
            if (CodeBox.Text == "ENTER ACCESS CODE !")
            {
                CodeBox.Clear();
            }

            if (CodeBox.Text.Length < 10)
            {
                CodeBox.AppendText("8");
            }

        }

        private void nine_Click(object sender, RoutedEventArgs e)
        {
            if (CodeBox.Text == "ENTER ACCESS CODE !")
            {
                CodeBox.Clear();
            }

            if (CodeBox.Text.Length < 10)
            {
                CodeBox.AppendText("9");
            }

        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            CodeBox.Clear();
            CodeBox.Text = "ENTER ACCESS CODE !";
        }

        private void enter_Click(object sender, RoutedEventArgs e)
        {
            if(CodeBox.Text == "ENTER ACCESS CODE !")
            {

            }
            else
            {
                accessCode = Convert.ToUInt64(CodeBox.Text);
            }
        }

        private void one_KeyDown(object sender, KeyEventArgs e)
        {
            //CodeBox.AppendText(e.ToString());
        }

        private void reset_MouseEnter(object sender, MouseEventArgs e)
        {
            reset.Foreground = Brushes.GreenYellow;
        }

        private void reset_MouseLeave(object sender, MouseEventArgs e)
        {
            reset.Foreground = Brushes.Black;
        }

        private void enter_MouseEnter(object sender, MouseEventArgs e)
        {
            enter.Foreground = Brushes.GreenYellow;
        }

        private void enter_MouseLeave(object sender, MouseEventArgs e)
        {
            enter.Foreground = Brushes.Black;
        }
    }
}
