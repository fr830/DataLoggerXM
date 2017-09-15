using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace DemoClient
{
        public partial class Form1 : Form
        {
            TcpClient client = null;
            public Form1()
            {
                InitializeComponent();
            }

            private void Form1_Load(object sender, EventArgs e)
            {
                msg("Client Started");
                Int32 port = 3001;
                IPAddress localAddr = IPAddress.Parse(GetLocalIPAddress());
            //client = new TcpClient("192.168.1.43", port);
            //clientSocket.Connect("127.0.0.1", 8888);
            //client = new TcpClient("192.168.1.43", port); 
            try
            {
                TcpClient client = new TcpClient("222.252.156.75", port);
                
                //client = new TcpClient("118.70.109.152", port);
                //client = new TcpClient("113.160.178.18", port);
                //client = new TcpClient("192.168.1.43", port);
            }
            catch(Exception ex) {
                msg(ex.StackTrace);
            }
                //label1.Text = "Client Socket Program - Server Connected ...";
            }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
        private void button1_Click(object sender, EventArgs e)
            {
            try
            {
                NetworkStream networkStream = client.GetStream();
                byte[] send = Encoding.ASCII.GetBytes("Message from Client$");
                networkStream.Write(send, 0, send.Length);
                networkStream.Flush();

                byte[] recv = new byte[1024];
                networkStream.Read(recv, 0, recv.Length);
                string data = Encoding.ASCII.GetString(recv);
                msg("Data from Server : " + data);
            }
            catch(Exception ex) {
                msg(ex.StackTrace);
            }
            }

            public void msg(string mesg)
            {
                textBox1.Text = textBox1.Text + Environment.NewLine + " >> " + mesg;
            }
        }
    }
