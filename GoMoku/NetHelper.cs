using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.Drawing;

namespace GoMoku
{
    class NetHelper
    {
        private bool isServer;
        private string ip;
        Socket server;
        Socket client;
        public bool isConn = false;
        private int rectPix;
        private Graphics gp;
        public string ServerIP="127.0.0.1";

        public NetHelper(bool s, int r, Graphics g, string ip)
        {
            this.ServerIP = ip;
            this.gp = g;
            this.rectPix = r;
            isConn = false;
            this.isServer = s;
            if(s)
            {
                Server();
            }
            else
            {
                Client();
            }
        }

        public void Server()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                IPAddress hostIP = (Dns.Resolve(IPAddress.Any.ToString())).AddressList[0];
                IPEndPoint ep = new IPEndPoint(hostIP, 9000);
                server.Bind(ep);

                // start listening
                server.Listen(10);

                client = server.Accept();
                if (client != null)
                {
                    isConn = true;
                    MessageBox.Show("有客户端连接成功\n\r游戏开始\n\r您执白棋先走");
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void Client()
        {
            client = new Socket(AddressFamily.InterNetwork,
  SocketType.Stream, ProtocolType.Tcp);
            bool b = true;
            int i = 1;
            while(b)
            {
                try
                {
                    IPAddress hostIP = (Dns.Resolve(ServerIP)).AddressList[0];
                    client.Connect(hostIP, 9000);
                    isConn = true;
                    b = false;

                }
                catch (Exception e)
                {
                    MessageBox.Show("尝试连接服务器失败"+i.ToString()+"次，请服务器端先点开始!!");
                    i++;
                    if (i == 5)
                        break;
                }
            }
            if(i!=5)
                MessageBox.Show("与服务器连接成功");
            else
            {
                MessageBox.Show("连接不到服务器!");
                this.Close();
            }
            
        }

        public void Send(QiZi qz)
        {
            if(isConn)
            {
                byte[] bq = new byte[20];
                string x = qz.Rect.X.ToString();
                string y = qz.Rect.Y.ToString();
                for (int i = 0; i < x.Length; i++)
                {
                    bq[i] = Convert.ToByte(x[i]);
                }
                for (int i = 0; i < y.Length; i++)
                {
                    bq[10 + i] = Convert.ToByte(y[i]);
                }
                try
                {
                    client.Send(bq);
                }
                catch (System.Exception e)
                {
                    MessageBox.Show(e.Message);
                    this.Close();
                }
            }
            
        
        }

        public QiZi Recv()
        {
            if(isConn)
            {
                byte[] bq = new byte[20];
                try
                {
                    client.Receive(bq);
                }
                catch (System.Exception e)
                {
                    MessageBox.Show(e.Message);
                    this.Close();
                }
                StringBuilder sbx = new StringBuilder();
                StringBuilder sby = new StringBuilder();
                for (int i = 0; i < 10; i++)
                {
                    if (bq[i] != 0)
                        sbx.Append(Convert.ToChar(bq[i]));
                    if (bq[10 + i] != 0)
                        sby.Append(Convert.ToChar(bq[10 + i]));//5-1-a-s-p-x
                }
                int x = Convert.ToInt32(sbx.ToString());
                int y = Convert.ToInt32(sby.ToString());

                Rectangle rect = new Rectangle(x, y, rectPix, rectPix);
                QiSe qs;
                if (isServer)
                    qs = QiSe.Hei;
                else
                    qs = QiSe.Bai;

                QiZi qz = new QiZi(qs, rect, gp, rectPix);
                return qz;
            }
            return null;
        }

        public void Close()
        {
            if (server != null)
            {
                server.Close();
                server = null;
            }

            if (client != null)
            {
                client.Close();
                client = null;
            }
        }
    }
}
