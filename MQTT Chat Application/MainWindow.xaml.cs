using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.ComponentModel;

namespace MQTT_Chat_Application
{
    public partial class MainWindow : Window
    {
        string text1, text2, text3; // Varijable za prijenos brokera, username i topica u novi window
        public MainWindow()
        {
            InitializeComponent();
        }

        private void KeyFocus(object sender, KeyboardFocusChangedEventArgs e) // Oznaci tekst prilikom fokusa
        {
            ((TextBox)sender).SelectAll();
        }

        private void Button_Click(object sender, RoutedEventArgs e) // Klikom na gumb Connect izvrsi "povezi"
        {
            povezi();
        }

        private void ButtonPress(object sender, KeyEventArgs e) // Na Username textboxu ako se detektira Enter gumb izvrsi "povezi"
        {
            if (e.Key == Key.Return)
            {
                povezi();
            }
        }

        private void povezi() // otvori novi window
        {
            text1 = broker_ip.Text;
            text2 = Topic.Text;
            text3 = Username.Text;
            Window1 chat = new Window1(text1, text2, text3); // Otvori novi prozor i prenesi potrebne varijable
            this.Close(); // Zatvori MainWindow
            chat.ShowDialog(); 
        }
    }
}
