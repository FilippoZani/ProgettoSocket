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
            IPEndPoint mittente_endpoint = new IPEndPoint(mittente_address.MapToIPv4(), 65000);

            //Definisce il mittente che comunica sul socket
            socket.Bind(mittente_endpoint);
        }

        private void btnInvia_Click(object sender, RoutedEventArgs e)
        {
            IPAddress destinatario_address = IPAddress.Parse(txtIndirizzo.Text);

            IPEndPoint destinatario_endpoint = new IPEndPoint(destinatario_address, int.Parse(txtPorta.Text));

            byte[] messaggio = Encoding.UTF8.GetBytes(txtMessaggio.Text);

            socket.SendTo(messaggio, destinatario_endpoint);
        }
    }
}
