using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Text_Encryptment_Program
{
    /// <summary>
    /// Interaktionslogik für AccessWindow.xaml
    /// </summary>
    public partial class AccessWindow : Window
    {
        public MediaPlayer window_popup     = new MediaPlayer();
        public MediaPlayer keypad_sound     = new MediaPlayer();
        public MediaPlayer keypad_reset     = new MediaPlayer();
        public MediaPlayer pling            = new MediaPlayer();
        public MediaPlayer code_accepted    = new MediaPlayer();
        public MediaPlayer access_denied    = new MediaPlayer();

        public ulong accessCode             = 0;

        public AccessWindow()
        {
            InitializeComponent();

            pling.Open(new Uri("sound_effects/pling.mp3", UriKind.RelativeOrAbsolute));
            window_popup.Open(new Uri("sound_effects/window_popup.mp3", UriKind.RelativeOrAbsolute));
            keypad_sound.Open(new Uri("sound_effects/button_pressed.wav", UriKind.RelativeOrAbsolute));
            keypad_reset.Open(new Uri("sound_effects/reset_numpad.wav", UriKind.RelativeOrAbsolute));
            code_accepted.Open(new Uri("sound_effects/access_granted.mp3", UriKind.RelativeOrAbsolute));
            access_denied.Open(new Uri("sound_effects/access_denied.mp3", UriKind.RelativeOrAbsolute));

            pling.IsMuted           = true;
            window_popup.IsMuted    = true;
            keypad_sound.IsMuted    = true;
            keypad_reset.IsMuted    = true;
            code_accepted.IsMuted   = true;
            access_denied.IsMuted   = true;

            InitializeCompleted();
        }

        private async void InitializeCompleted()
        {
            await Task.Delay(1000);

            pling.IsMuted           = false;
            window_popup.IsMuted    = false;
            keypad_sound.IsMuted    = false;
            keypad_reset.IsMuted    = false;
            code_accepted.IsMuted   = false;
            access_denied.IsMuted   = false;

            window_popup.Volume     = 1.0;
            code_accepted.Volume    = 1.0;

            window_popup.Play();

            CodeBox.Focus();
        }
        private void one_Click(object sender, RoutedEventArgs e)
        {
            if (CodeBox.Text == "ENTER ACCESS CODE !")
            {
                CodeBox.Clear();
            }

            if (CodeBox.Text.Length < 10)
            {
                CodeBox.AppendText("1");
            }

            keypad_sound.Position = TimeSpan.Zero;
            keypad_sound.Play();
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

            keypad_sound.Position = TimeSpan.Zero;
            keypad_sound.Play();
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

            keypad_sound.Position = TimeSpan.Zero;
            keypad_sound.Play();
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

            keypad_sound.Position = TimeSpan.Zero;
            keypad_sound.Play();
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

            keypad_sound.Position = TimeSpan.Zero;
            keypad_sound.Play();
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

            keypad_sound.Position = TimeSpan.Zero;
            keypad_sound.Play();
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

            keypad_sound.Position = TimeSpan.Zero;
            keypad_sound.Play();
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

            keypad_sound.Position = TimeSpan.Zero;
            keypad_sound.Play();
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

            keypad_sound.Position = TimeSpan.Zero;
            keypad_sound.Play();
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            CodeBox.Clear();
            CodeBox.Text = "ENTER ACCESS CODE !";
            CodeBox.Focus();

            keypad_reset.Position = TimeSpan.Zero;
            keypad_reset.Play();
        }

        private void enter_Click(object sender, RoutedEventArgs e)
        {
            if (CodeBox.Text == "ENTER ACCESS CODE !")
            {

            }
            else
            {
                accessCode = Convert.ToUInt64(CodeBox.Text);

                if (accessCode != MainWindow.GetAccessCode())
                {
                    access_denied.Position = TimeSpan.Zero;
                    access_denied.Play();
                }
            }
        }

        private void CodeBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (CodeBox.Text == "ENTER ACCESS CODE !")
            {
                CodeBox.Clear();
            }
            else if (e.Key == Key.Enter)
            {
                enter_Click(sender, e);
            }
            else if (e.Key == Key.Back)    // Doesn't work yet :(
            {
                reset_Click(sender, e);
            }

            if (CodeBox.Text.Length > 9)
            {
                CodeBox.IsReadOnly = true;
            }
            else
            {
                CodeBox.IsReadOnly = false;
            }

            keypad_sound.Position = TimeSpan.Zero;
            keypad_sound.Play();
        }

        //  MOUSE ENTER - MOUSE LEAVE METHODS ...

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