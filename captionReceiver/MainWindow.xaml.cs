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



namespace captionReceiver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    
    public partial class MainWindow : Window
    {
        private Socket blah;
        private int portnum;
        private delegate void myDelegate();
        private delegate void oneArgDelegate(String answer);
        


        public MainWindow()
        {
            Random portGen = new Random();
            //portnum = portGen.Next(49152, 65535);
            portnum = 1200;
            InitializeComponent();
            IPHostEntry host;
            String hostName = Dns.GetHostName();
            host = Dns.GetHostEntry(hostName);
            TextBox box = textBox;
            box.Text = "";
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    box.Text += String.Format("server {0} port {1}\n", ip.ToString(), portnum);
                    
                }
            }
            
            
            myDelegate blah = new myDelegate(message);
            blah.BeginInvoke(null, null);
            




        }
        public void message()
        {
            const int BUFMAX = 10000;
            IPEndPoint end = new IPEndPoint(IPAddress.Any, portnum);
            TcpListener l = new TcpListener(end);
            byte[] buffer = new byte[BUFMAX];
            int numBytes = 0;
            l.Start();
            while (true)
            {
                blah = l.AcceptSocket();
                int bufferindex = 0;
                while (blah.Connected)
                {
                    numBytes = blah.Receive(buffer, bufferindex, BUFMAX - bufferindex, 0);
                    if (numBytes == 0)
                        break;
                    bufferindex = bufferindex + numBytes;

                }

                String mess = Encoding.ASCII.GetString(buffer, 0, bufferindex);

                this.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new oneArgDelegate(updateUserInterface), mess);


            }
        }
        private void updateUserInterface(String data)
        {
            TextBox box = textBox;
            if (!data.EndsWith("\n"))
            {
                data += "\n";   
            }
            box.Text += data;
        }
    }
}
