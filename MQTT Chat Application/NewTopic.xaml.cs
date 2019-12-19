using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.ComponentModel;

namespace MQTT_Chat_Application
{
    public partial class NewTopic : Window
    {
        string broker, topic, username;
        public NewTopic(string _value, string _value3)
        {
            InitializeComponent();
            broker = _value; // spremi lokalne prenesene varijable u globalne
            username = _value3;
            broker_ip.AppendText(broker);
            Username.AppendText(username);
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
            broker = broker_ip.Text;
            topic = Topic.Text;
            username = Username.Text;
            Window1 chat = new Window1(broker, topic, username); // Otvori novi prozor i prenesi potrebne varijable
            this.Close(); // Zatvori MainWindow
            chat.ShowDialog();
        }
    }
}
