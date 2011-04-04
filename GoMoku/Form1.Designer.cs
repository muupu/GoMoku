namespace GoMoku
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelIp = new System.Windows.Forms.Label();
            this.txt_IP = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_localport = new System.Windows.Forms.TextBox();
            this.txt_remoteport = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label_state = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Location = new System.Drawing.Point(2, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(500, 500);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // labelIp
            // 
            this.labelIp.AutoSize = true;
            this.labelIp.Location = new System.Drawing.Point(516, 37);
            this.labelIp.Name = "labelIp";
            this.labelIp.Size = new System.Drawing.Size(53, 12);
            this.labelIp.TabIndex = 9;
            this.labelIp.Text = "对方IP：";
            // 
            // txt_IP
            // 
            this.txt_IP.Location = new System.Drawing.Point(590, 34);
            this.txt_IP.Name = "txt_IP";
            this.txt_IP.Size = new System.Drawing.Size(112, 21);
            this.txt_IP.TabIndex = 8;
            this.txt_IP.Text = "127.0.0.1";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(569, 241);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(86, 36);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(569, 321);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(86, 37);
            this.button3.TabIndex = 10;
            this.button3.Text = "悔棋";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(569, 415);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(86, 38);
            this.button2.TabIndex = 22;
            this.button2.Text = "重新开始";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(518, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 25;
            this.label3.Text = "本地端口：";
            // 
            // txt_localport
            // 
            this.txt_localport.Location = new System.Drawing.Point(589, 114);
            this.txt_localport.Name = "txt_localport";
            this.txt_localport.Size = new System.Drawing.Size(112, 21);
            this.txt_localport.TabIndex = 26;
            // 
            // txt_remoteport
            // 
            this.txt_remoteport.Location = new System.Drawing.Point(589, 74);
            this.txt_remoteport.Name = "txt_remoteport";
            this.txt_remoteport.Size = new System.Drawing.Size(112, 21);
            this.txt_remoteport.TabIndex = 27;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(516, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 28;
            this.label4.Text = "对方端口：";
            // 
            // label_state
            // 
            this.label_state.AutoSize = true;
            this.label_state.Location = new System.Drawing.Point(508, 183);
            this.label_state.Name = "label_state";
            this.label_state.Size = new System.Drawing.Size(29, 12);
            this.label_state.TabIndex = 29;
            this.label_state.Text = "状态";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 517);
            this.Controls.Add(this.label_state);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_remoteport);
            this.Controls.Add(this.txt_localport);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.labelIp);
            this.Controls.Add(this.txt_IP);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "基于UDP的P2P网络五子棋游戏";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelIp;
        private System.Windows.Forms.TextBox txt_IP;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_localport;
        private System.Windows.Forms.TextBox txt_remoteport;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_state;
    }
}

