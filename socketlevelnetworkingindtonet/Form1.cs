using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace socketlevelnetworkingindtonet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private AsyncCallback acceptCallBack;
        private AsyncCallback receiveCallBack;
        public Socket listenerSocket;
        public Socket clientSocket;
        public byte[] recv;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            acceptCallBack = new AsyncCallback(acceptHandler);
            listenerSocket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp
            );

            //IPHostEntry IPHost = Dns.GetHostByName(Dns.GetHostName());
            IPHostEntry IPHost = Dns.GetHostEntry(Dns.GetHostName());
            //IPHostEntry IPHost = Dns.GetHostByName(Dns.GetHostName());
            IPEndPoint ipepServer = new IPEndPoint(IPHost.AddressList[0], 8080);
            listenerSocket.Bind(ipepServer);
            listenerSocket.Listen(-1);
            listenerSocket.BeginAccept(acceptCallBack, null);
        }

        public void acceptHandler(IAsyncResult asyncResult)
        {
            receiveCallBack = new AsyncCallback(receiveHandler);
            clientSocket = listenerSocket.EndAccept(asyncResult);
            recv = new byte[1];
            clientSocket.BeginReceive(recv, 0, 1,
            SocketFlags.None, receiveCallBack, null);
        }



        public void receiveHandler(IAsyncResult asyncResult)
        {
            int bytesReceived = 0;
            bytesReceived = clientSocket.EndReceive(asyncResult);
            if (bytesReceived != 0)
            {
                tbStatus.Text += Encoding.UTF8.GetString(recv);
                recv = new byte[1];
                clientSocket.BeginReceive(recv, 0, 1,
                SocketFlags.None, receiveCallBack, null);
            }
        }


    }
}
