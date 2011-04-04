using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GoMoku
{
    enum QiSe { Null, Hei, Bai };
    
    class QiZi
    {
        private QiSe qs;//棋子颜色
        private Rectangle rect;//棋子所在的四边行
        private Graphics gp;//绘制布棋子的画
        private int rectPix;//棋子大小
        private bool isWin = false;

        public bool IsWin
        {
            get { return isWin; }
            set { isWin = value; }
        }
        public Rectangle Rect
        {
            get { return rect; }
        }

        public QiSe Qs
        {
            get { return qs; }
            set { qs = value; }
        }

        public QiZi(QiSe qs, Rectangle r, Graphics g, int rp)
        {

            this.qs = qs;
            rect = r;
            gp = g;
            rectPix = rp;
        }

        public void Paint()
        {
            if (isWin)
            {
                PaintWin();
                return;
            }

            if (this.Qs == QiSe.Hei)
            {
                Image im = Image.FromFile(@"..\Resources\black.gif");
                //Bitmap b = new Bitmap(im,rectPix,rectPix);
                gp.DrawImage(im, rect);

                return;
            }

            if (this.Qs == QiSe.Bai)
            {
                Image im = Image.FromFile(@"..\Resources\white.gif");
                //Bitmap b = new Bitmap(im, rectPix, rectPix);
                gp.DrawImage(im, rect);
                return;
            }

        }

        public void PaintWin()//绘制五个连成线的棋子
        {
            if (isWin)
            {
                Pen p = new Pen(Color.Red, 3);
                gp.SmoothingMode = SmoothingMode.HighQuality;
                gp.DrawEllipse(p, this.rect);
            }
        }
    }
}
