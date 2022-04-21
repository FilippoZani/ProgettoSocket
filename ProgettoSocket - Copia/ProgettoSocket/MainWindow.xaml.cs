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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Windows.Threading;


namespace ProgettoSocket
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Socket socket = null;
        DispatcherTimer dTimer = null;
        public MainWindow()
        {
            InitializeComponent();

            //Con la scritta "AddressFamily.InterNetwork" specifico che la famiglia di indirizzi IP che utilizzo è IPv4
            //Con la scritta "SocketType.Dgram" specifico che il tipo di socket che utilizzo è il Dgram
            //Con la scritta "ProtocolType.Udp" specifico che il tipo di protocollo che utilizzo è l'Udp
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //Assegnazione di un indirizzo IP al mittente
            IPAddress mittente_address = IPAddress.Any;

            //Stabilisco il punto di fine della comunicazione, specificando l'indirizzo IP del mittente e la porta attraverso quale avverrà la comunicazione
            IPEndPoint mittente_endpoint = new IPEndPoint(mittente_address.MapToIPv4(), 64000);

            //Definisce il mittente che comunica sul socket
            socket.Bind(mittente_endpoint);

            //Invece di usare un thread che gestisce l'ascolto dall'altro lato della comunicazione costantemente, utilizzo un timer che ascolta solamente ogni tot di tempo
            dTimer = new DispatcherTimer();

            //Aggiungo l'evento da eseguire quando scatta il timer
            dTimer.Tick += new EventHandler(Aggiornamento_dTimer);

            //Aggiungo l'intervallo di tempo di funzionamento del timer (250 millisec.)
            dTimer.Interval = new TimeSpan(0, 0, 0, 0, 250);

            //Avvio il timer
            dTimer.Start();
        }

        private void btnInvia_Click(object sender, RoutedEventArgs e)
        {
            IPAddress destinatario_address = IPAddress.Parse(txtIndirizzo.Text);

            IPEndPoint destinatario_endpoint = new IPEndPoint(destinatario_address, int.Parse(txtPorta.Text));

            byte[] messaggio = Encoding.UTF8.GetBytes(txtMessaggio.Text);

            socket.SendTo(messaggio, destinatario_endpoint);
        }

        private void Aggiornamento_dTimer(object sender, EventArgs e)
        {
            int nBytes = 0;

            //Ogni 250 millisec., attraverso l'if mi chiedo se mi è arrivato qualcosa; se no, lo salto, altrimenti faccio il resto
            if((nBytes = socket.Available) > 0)
            {
                //Ricezione dei caratteri in attesa
                byte[] buffer = new byte[nBytes];

                EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

                nBytes = socket.ReceiveFrom(buffer, ref remoteEndPoint);

                string from = ((IPEndPoint)remoteEndPoint).Address.ToString();

                string messaggio = Encoding.UTF8.GetString(buffer, 0, nBytes);

                lstRoba.Items.Add(from + ": " + messaggio);
            }
        }
    }
}
