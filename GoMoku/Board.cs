using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;

namespace GoMoku
{
    class Board
    {
        private int rectPix;
        private int w;//棋盘宽度
        private QiZi[,] qzArr;
        private QiSe turn = QiSe.Bai;//标记该谁下棋
        private Graphics gp;
        private Color lineColor = Color.FromArgb(127, 127, 127);
        private bool start = false;

        private QiZi q1;
        private QiZi q2;
        private bool isFull = false;
        private QiZi[] winQizi;//标记连成一条线的五个棋子
        private NetHelper localnet;


        private bool isServer;
        public Thread thread;
        private bool islocalnet = false;
        public bool shu = false;//局域网模式时判断是不是输了
        public bool bt = true;//标记线程是否运行

        public NetHelper Localnet
        {
            get { return localnet; }
            set { localnet = value; }
        }

        public bool Islocalnet
        {
            get { return islocalnet; }
            set { islocalnet = value; }
        }

        public bool IsFull
        {
            get { return isFull; }
        }


        public bool Start
        {
            get { return start; }
            set { start = value; }
        }

        public QiSe Turn
        {
            get { return turn; }
            set { turn = value; }
        }

        public QiZi[,] QzArr
        {
            get { return qzArr; }
            set { qzArr = value; }
        }

        public Board(int r, Graphics g, int w, int rectPix, bool isServer, string ip)
            : this(r, g, w, rectPix)
        {
            this.isServer = isServer;
            localnet = new NetHelper(isServer, r, g, ip);
        }

        public Board(int r, Graphics g, int w, int rectPix)
        {

            winQizi = new QiZi[5];
            isFull = false;
            this.w = w;
            this.rectPix = rectPix;
            qzArr = new QiZi[w, w];
            this.rectPix = r;
            this.gp = g;
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    Rectangle rect = new Rectangle(i * rectPix, j * rectPix, rectPix, rectPix);
                    this.qzArr[i, j] = new QiZi(QiSe.Null, rect, gp, rectPix);
                    this.qzArr[i, j].Qs = QiSe.Null;
                }
            }
        }

        public void Paint()
        {
            gp.Clear(Color.FromArgb(255, 255, 192));
            PaintLine();
            PaintQizi();
        }

        public void PaintLine()
        {
            Pen p = new Pen(lineColor, 2);
            gp.DrawLine(p, rectPix / 2, rectPix / 2, rectPix / 2, rectPix * w - rectPix / 2);//边线
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

        public void PaintQizi()
        {
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    qzArr[i, j].Paint();
                }
            }
        }

        public void ThreadProc()//接收线程函数
        {

            while (bt)
            {
                if (start)
                {
                    if (isServer)
                    {
                        QiZi qz = localnet.Recv();
                        if (qz != null)
                        {
                            int i = qz.Rect.X / rectPix;
                            int j = qz.Rect.Y / rectPix;
                            QzArr[i, j] = qz;
                            qz.Paint();
                            if (ShuYin())
                            {
                                bt = false;
                                localnet.Close();
                                shu = true;
                            }
                            this.turn = QiSe.Bai;
                        }

                    }
                    else
                    {

                        QiZi qz = localnet.Recv();
                        if (qz != null)
                        {
                            int i = qz.Rect.X / rectPix;
                            int j = qz.Rect.Y / rectPix;
                            QzArr[i, j] = qz;
                            qz.Paint();
                            if (ShuYin())
                            {
                                bt = false;
                                localnet.Close();
                                shu = true;
                            }
                            this.turn = QiSe.Hei;
                        }

                    }
                }
            }
        }

        public bool LocalnetXiaQi(int x, int y)//局域网游戏
        {
            if (isServer && turn == QiSe.Bai)
            {
                if (qzArr[x, y].Qs == QiSe.Null)
                {
                    qzArr[x, y].Qs = turn;
                    qzArr[x, y].Paint();
                    q1 = qzArr[x, y];
                    if (ShuYin())
                    {
                        localnet.Send(QzArr[x, y]);
                        this.turn = QiSe.Hei;
                        localnet.Close();
                        return true;
                    }

                    localnet.Send(QzArr[x, y]);
                    this.turn = QiSe.Hei;
                }
            }

            if (!isServer && turn == QiSe.Hei)
            {
                if (qzArr[x, y].Qs == QiSe.Null)
                {
                    qzArr[x, y].Qs = turn;
                    qzArr[x, y].Paint();
                    q1 = qzArr[x, y];
                    if (ShuYin())
                    {
                        localnet.Send(QzArr[x, y]);
                        this.turn = QiSe.Hei;
                        localnet.Close();
                        return true;
                    }
                    localnet.Send(QzArr[x, y]);
                    this.turn = QiSe.Bai;
                }
            }

            return false;
        }        

        private bool ShuYin()
        {
            if (SyHeng() || SyZong() || SyZhudj() || SyFudj())
                return true;
            else
                return false;
        }

        private bool SyHeng()//判断横向是不是连线
        {

            for (int i = 0; i < w - 4; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    int k = 1;
                    for (k = 1; k < 5; k++)
                    {
                        if (qzArr[i, j].Qs != turn || qzArr[i, j].Qs != qzArr[i + k, j].Qs)
                        {
                            break;
                        }

                    }
                    if (k == 5)
                    {
                        for (k = 0; k < 5; k++)
                        {
                            qzArr[i + k, j].IsWin = true;
                            qzArr[i + k, j].PaintWin();
                        }
                        return true;
                    }

                }
            }
            return false;
        }

        private bool SyZong()//判断纵向是不是连线
        {

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < w - 4; j++)
                {
                    int k = 1;
                    for (k = 1; k < 5; k++)
                    {
                        if (qzArr[i, j].Qs != turn || qzArr[i, j].Qs != qzArr[i, j + k].Qs)
                        {
                            break;
                        }

                    }
                    if (k == 5)
                    {
                        for (k = 0; k < 5; k++)
                        {
                            qzArr[i, j + k].IsWin = true;
                            qzArr[i, j + k].PaintWin();
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        private bool SyZhudj()//判断主对角是不是连线
        {

            for (int i = 0; i < w - 4; i++)
            {
                for (int j = 0; j < w - 4; j++)
                {
                    int k = 1;
                    for (k = 1; k < 5; k++)
                    {
                        if (qzArr[i, j].Qs != turn || qzArr[i, j].Qs != qzArr[i + k, j + k].Qs)
                        {
                            break;
                        }

                    }
                    if (k == 5)
                    {
                        for (k = 0; k < 5; k++)
                        {
                            qzArr[i + k, j + k].IsWin = true;
                            qzArr[i + k, j + k].PaintWin();
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        private bool SyFudj()//判断辅对角是不是连线
        {

            for (int i = 0; i < w - 4; i++)
            {
                for (int j = 4; j < w; j++)
                {
                    int k = 1;
                    for (k = 1; k < 5; k++)
                    {
                        if (qzArr[i, j].Qs != turn || qzArr[i, j].Qs != qzArr[i + k, j - k].Qs)
                        {
                            break;
                        }

                    }
                    if (k == 5)
                    {
                        for (k = 0; k < 5; k++)
                        {
                            qzArr[i + k, j - k].IsWin = true;
                            qzArr[i + k, j - k].PaintWin();
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        public void QinLing()
        {
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if (this.qzArr[i, j].Qs != QiSe.Null)
                    {
                        this.qzArr[i, j].Qs = QiSe.Null;
                    }
                    if (this.qzArr[i, j].IsWin)
                    {
                        this.qzArr[i, j].IsWin = false;
                    }

                }
            }
            this.Paint();
        }

        public bool HuiQi()//悔棋
        {
            if (q1 != null && q2 != null)
            {
                q1.Qs = QiSe.Null;
                q2.Qs = QiSe.Null;

                q1.Paint();
                q2.Paint();
                q1 = null; q2 = null;
                return true;
            }
            return false;

        }


    }
}
