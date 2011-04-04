using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GoMoku
{
    public partial class Form1 : Form
    {
        private const int rectPix = 31;//棋子大小
        private Color lineColor = Color.FromArgb(127, 127, 127);
        private const int w = 16;//棋盘边线数

        private enum Chess { none = 0, Black, White };
        private Chess[,] Box = new Chess[16, 16];

        private Chess mplayer = Chess.White;//假设持白棋 
        //网络通信部分 
        private bool ReadFlag = true;
        //设定侦听标示位，通过它来设定是否侦听端口号 
        private Thread th;//定义一个线程，在线程接收信息 

        private IPEndPoint remote;
        //定义一个远程结点，用以获取远程计算机IP地址和发送的信息 
        private UdpClient udpclient;//创建一个UDP网络服务 
        private bool can_go = false;  //能否走棋 
        private Chess winner;
        private bool th_flag = false;

        private bool Reset_flag = false; 

        public Form1()
        {
            InitializeComponent();
            setStatusDelegate = new SetStatusDelegate(SetStatus);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics gp = e.Graphics;
            Pen p = new Pen(lineColor, 2);
            gp.DrawLine(p, rectPix / 2, rectPix / 2, rectPix / 2, rectPix * w - rectPix / 2);
            gp.DrawLine(p, rectPix / 2, rectPix / 2, rectPix * w - rectPix / 2, rectPix / 2);
            gp.DrawLine(p, rectPix / 2, rectPix * w - rectPix / 2, rectPix * w - rectPix / 2, rectPix * w - rectPix / 2);
            gp.DrawLine(p, rectPix * w - rectPix / 2, rectPix / 2, rectPix * w - rectPix / 2, rectPix * w - rectPix / 2);
            p = new Pen(lineColor, 1);
            for (int i = rectPix + rectPix / 2; i < rectPix * w - rectPix / 2; i += rectPix)
            {
                gp.DrawLine(p, rectPix / 2, i, rectPix * w - rectPix / 2, i);
                gp.DrawLine(p, i, rectPix / 2, i, rectPix * w - rectPix / 2);
            }
        }

        private void read ( )
        {
            //侦听本地的端口号 
            udpclient = new UdpClient(Convert.ToInt32(txt_localport.Text));
            remote = null;
            //设定编码类型 
            Encoding enc = Encoding.Unicode;
            int x, y;
            while (ReadFlag == true)
            {
                Byte[] data = udpclient.Receive(ref remote);
                //得到对方发送来的信息 
                String strData = enc.GetString(data);
                string[] a = new string[5];
                a = strData.Split('|');
                switch (a[0])
                {
                    case "join":
                        //获取传送信息到本地端口号的远程计算机IP地址 
                        string remoteIP = remote.Address.ToString();
                        //显示接收信息以及传送信息的计算机IP地址 
                        SetStatus(remoteIP + "已经加入你是黑方请先走棋");
                        mplayer = Chess.Black;
                        can_go = true;  //能走棋 
                        btnStart.Enabled = false;
                        break;
                    case "move":
                        x = Convert.ToInt16(a[1]);
                        y = Convert.ToInt16(a[2]);
                        SetStatus("对方下棋子位置：" + x.ToString() + "," + y.ToString());
                        
                        //画对方棋子 
                        Graphics g = this.pictureBox1.CreateGraphics();
                        
                        if (Box[x, y] != Chess.none) 
                            return;
                      
                        if (mplayer == Chess.Black)
                        {
                            Image im = Image.FromFile(@"..\..\Resources\White.gif");
                            g.DrawImage(im, x * rectPix , y * rectPix , rectPix, rectPix);
                            //imageList1.Draw(g, x * rectPix + rectPix / 2, y * rectPix + rectPix / 2, (int)Chess.White);
                            Box[x, y] = Chess.White;
                        }
                        else
                        {
                            Image im = Image.FromFile(@"..\..\Resources\Black.gif");
                            g.DrawImage(im, x * rectPix , y * rectPix , rectPix, rectPix);
                            //imageList1.Draw(g, x * rectPix + rectPix / 2, y * rectPix + rectPix / 2, (int)Chess.Black);
                            Box[x, y] = Chess.Black;
                        }
                        //Box[p.X,p.Y]=mplayer; 
                        can_go = true;
                        break;
                    case "over":
                        SetStatus(a[1] + "赢了此局");
                        MessageBox.Show("你赢了此局","恭喜！");
                        if (a[1] == "Black")
                            winner = Chess.Black;
                        else
                            winner = Chess.White;
                        can_go = false;
                        button2.Enabled = true;
                        break;
                    case "reset":
                        Reset_flag = true;
                        if (button2.Enabled == false) { reset(); Reset_flag = false; }
                        break;
                }
            }
        } 

        private void btnStart_Click(object sender, EventArgs e)
        {
            send("join|");
            //创建一个线程 
            th = new Thread(new ThreadStart(read));
            th_flag = true;
            //启动线程 
            th.Start();
            SetStatus("程序处于等待联机状态！");
            btnStart.Enabled = false;          
        }

        private void Cls_Box()
        {
            pictureBox1.Refresh();
            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 16; j++)
                    Box[i, j] = Chess.none;
        } 

        private void reset()
        {
            if (mplayer == Chess.Black)
            {
                if (Reset_flag == true)// 对方已经重新开始 
                {
                    SetStatus("你是白方,处于重新开始状态！");
                    mplayer = Chess.White;
                    can_go = false;
                }
                else
                    SetStatus("你是白方,处于等待对方重新开始状态！");
            }
            else
            {
                if (Reset_flag == true)//对方已经重新开始 
                {
                    SetStatus("你是黑方，请先走棋");
                    mplayer = Chess.Black;
                    can_go = true;
                }
                else
                    SetStatus("你是黑方,处于等待对方重新开始状态！");
            }
        }

        private void send(string info)
        {
            //创建UDP网络服务 
            UdpClient SendUdp = new UdpClient();
            IPAddress remoteIP;
            //判断IP地址的正确性 
            try
            {
                remoteIP = IPAddress.Parse(txt_IP.Text);
            }
            catch
            {
                MessageBox.Show("请输入正确的IP地址！", "错误");
                return;
            }
            IPEndPoint remoteep = new IPEndPoint(remoteIP, Convert.ToInt32(txt_remoteport.Text));
            Byte[] buffer = null;
            Encoding enc = Encoding.Unicode;
            string str = info;
            buffer = enc.GetBytes(str.ToCharArray());
            //传送信息到指定计算机的txt_remoteport端口号 
            SendUdp.Send(buffer, buffer.Length, remoteep);
            //textBox1.Clear ( ) ; 
            //关闭UDP网络服务 
            SendUdp.Close();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {   
            //重新开始  
            Cls_Box();
            reset();
            send("reset|");
            button2.Enabled = false;
            Reset_flag = false;
        } 

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (can_go != true)
            { 
                MessageBox.Show("不能走棋，请等对方"); 
                return; 
            }
            //e.X为pictureBox1内的 
            Graphics g = this.pictureBox1.CreateGraphics();
            int x = e.X / rectPix;
            int y = e.Y / rectPix;
            if (x < 0 || y < 0 || x >= 16 || y >= 16)
            {
                MessageBox.Show("超边界了");
                return;
            }

            Point p = new Point(x * rectPix + rectPix / 2, y * rectPix + rectPix / 2);

            //label1.Text = p.X.ToString() + "|" + p.Y.ToString() + "|" + e.X.ToString() + "  " + e.Y.ToString();
            string s1 = p.X.ToString() + "|" + p.Y.ToString();

            if (Box[x, y] != Chess.none) 
                return;
            //label2.Text = p2.X.ToString() + "|" + p2.Y.ToString();

            Image im = Image.FromFile(@"..\..\Resources\" + mplayer.ToString() + ".gif");
            g.DrawImage(im, x * rectPix , y * rectPix , rectPix, rectPix);
            //imageList1.Draw(g, x * rectPix + rectPix / 2,y * rectPix + rectPix / 2, (int)mplayer);
            Box[x, y] = mplayer;
            //label2.Text = Box[x, y].ToString();

            string str_send = "move|" + x + "|" + y;
            can_go = false;

            send(str_send);

            if (win_lose() == true)//判断输赢否 
            {
                str_send = "over|" + mplayer.ToString();
                winner = mplayer;
                SetStatus(mplayer.ToString() + "赢了此局！");
                button2.Enabled = true;
                send(str_send);
            } 
        }
        
        private bool win_lose()//扫描整个棋盘，判断是否连成五颗 
        {
            int i, j;
            for (i = 0; i < w - 4; i++) //判断X= Y 轴上是否形成五子连珠 
                for (j = 0; j < w - 4; j++)
                    if (Box[i, j] == mplayer && Box[i + 1, j + 1] == mplayer && Box[i + 2, j
              + 2] == mplayer && Box[i + 3, j + 3] == mplayer && Box[i + 4, j + 4] == mplayer)
                        return true;

            for (i = 4; i < w; i++)   //判断 X= -Y轴上是否形成五子连珠 
                for (j = 0; j < w-4; j++)
                    if (Box[i, j] == mplayer && Box[i - 1, j + 1] == mplayer && Box[i - 2, j
              + 2] == mplayer && Box[i - 3, j + 3] == mplayer && Box[i - 4, j + 4] == mplayer)
                        return true;

            for (i = 0; i < w; i++) //判断Y轴上是否形成五子连珠 
                for (j = 4; j < w; j++)
                    if (Box[i, j] == mplayer && Box[i, j - 1] == mplayer && Box[i, j - 2] == mplayer
              && Box[i, j - 3] == mplayer && Box[i, j - 4] == mplayer)
                        return true;

            for (i = 0; i < w - 4; i++) //判断X轴上是否形成五子连珠 
                for (j = 0; j < w; j++)
                    if (Box[i, j] == mplayer && Box[i + 1, j] == mplayer && Box[i + 2, j] == mplayer &&
              Box[i + 3, j] == mplayer && Box[i + 4, j] == mplayer)
                        return true;
            return false;

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (th_flag == true)
                if (th.IsAlive) th.Abort();
        }

        #region 线程安全UI操作代码

        private delegate void SetStatusDelegate(string status);
        private SetStatusDelegate setStatusDelegate; // 赋值在构造函数里

        private void SetStatusSynchronized(string status)
        {
            label_state.Text = status;
            //statusBar1.Text = status;
        }

        private void SetStatus(string status)
        {
            if (InvokeRequired)
                Invoke(setStatusDelegate, new object[] { status });
            else
                //statusBar1.Text = status;
                label_state.Text = status;
        }
        #endregion

    }
}
