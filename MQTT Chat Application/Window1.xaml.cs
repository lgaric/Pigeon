using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MQTT_Chat_Application
{

    public partial class Window1 : Window
    {
        string broker, topic, username;
        MqttClient Client;
        public Window1(string _value, string _value2, string _value3) // Prenesene varijable
        {
            InitializeComponent();
            broker = _value; // spremi lokalne prenesene varijable u globalne
            topic = _value2;
            username = _value3;

            Client = new MqttClient(broker); // Povezi se na broker

            Chat.AppendText("\nWelcome to chat!\nTopic: ");
            Chat.AppendText(topic.ToString() + "\n");
            Client.MqttMsgPublishReceived += poruka; // Dohvati poruku
            Client.Connect(Guid.NewGuid().ToString()); // Povezi klijenta (Klijent ID)
            Client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }); // Subscribe korisnika na temu

            string pristup = GetTimestamp(DateTime.Now); // Dohvati trenutno vrijeme
            Client.Publish(topic, Encoding.UTF8.GetBytes("[" + pristup + "] [Korisnik " + username + " je ušao u sobu " + topic + ".]"),
                                                                                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false); // Poruka o novom korisniku
  

        }
        private void Button_Click(object sender, RoutedEventArgs e) // Na klik gumba otvori prozor MainWindow
        {
            Client.Publish(topic, Encoding.UTF8.GetBytes("[" + GetTimestamp(DateTime.Now) + "] [Korisnik " + username + " je izašao iz sobe " + topic + ".]"),
                                                    MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true); // Poruka o izlasku 
            Client.Unsubscribe(new string[] { topic }); // Unsubscribe klijenta s teme
            NewTopic nova_tema = new NewTopic(broker, username);
            Close();
            nova_tema.ShowDialog();
            Client.Disconnect(); // Prekini vezu s brokerom
        }

        private void poruka(object sender, MqttMsgPublishEventArgs e) // Ispis pristigle poruke
        {
            Dispatcher.Invoke((Action)(() => // sinkronizira dretve ukoliko stigne poruka tako da ju moze koristiti prilikom ispisa
            {
                Chat.AppendText("\n" + Encoding.UTF8.GetString(e.Message)); //ispisi text
            }));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) // Na klik gumba izvrsi "send"
        {
            send();
            e.Handled = true; // označava da je programer izvršio potrebnu radnju i da mu ne treba potpora windowsa 
        }

        private void ButtonPress(object sender, KeyEventArgs e) // Na chat textboxu ako se detektira Enter gumb izvrsi "send"
        {
            if (e.Key == Key.Return)
            {
                if (!(string.IsNullOrEmpty(Type.Text))) // ako nema teksta u textboxu ignoriraj pritisak gumba (nemoj poslati poruku)
                {
                    send();
                    e.Handled = true;
                }
            }
        }

        private void Chat_TextChanged(object sender, TextChangedEventArgs e) // Uvijek prikazuj zadnju pristiglu poruku / dno chat boxa
        {
            Chat.ScrollToEnd();
        }

        public static String GetTimestamp(DateTime vrijeme) // Dohvati vrijeme i pretvori u string
        {
            return vrijeme.ToString("HH:mm");
        }

        private void send() // Posalji poruku brokeru
        {
            String vrijeme = GetTimestamp(DateTime.Now); 
            Client.Publish(topic,
            Encoding.UTF8.GetBytes("[" + vrijeme + "] " + username + ": " + Type.Text),
            MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
            true);
            Type.Clear();
        }
    }
}
