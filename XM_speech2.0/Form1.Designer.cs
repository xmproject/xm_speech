namespace XM_speech2._0
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.srRichTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.confidenceTrackBar = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.confidenceTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.srPortTextBox = new System.Windows.Forms.TextBox();
            this.srIpTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.srPicBox = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmdfilecomboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.srBtn = new System.Windows.Forms.PictureBox();
            this.GPSRtimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.confidenceTrackBar)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.srPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.srBtn)).BeginInit();
            this.SuspendLayout();
            // 
            // srRichTextBox
            // 
            this.srRichTextBox.ForeColor = System.Drawing.Color.Lime;
            this.srRichTextBox.Location = new System.Drawing.Point(6, 191);
            this.srRichTextBox.Name = "srRichTextBox";
            this.srRichTextBox.Size = new System.Drawing.Size(364, 147);
            this.srRichTextBox.TabIndex = 0;
            this.srRichTextBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(6, 161);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 27);
            this.label1.TabIndex = 1;
            this.label1.Text = "语音识别及输出窗口";
            // 
            // confidenceTrackBar
            // 
            this.confidenceTrackBar.Location = new System.Drawing.Point(64, 113);
            this.confidenceTrackBar.Maximum = 100;
            this.confidenceTrackBar.Name = "confidenceTrackBar";
            this.confidenceTrackBar.Size = new System.Drawing.Size(306, 45);
            this.confidenceTrackBar.TabIndex = 2;
            this.confidenceTrackBar.TickFrequency = 10;
            this.confidenceTrackBar.Value = 30;
            this.confidenceTrackBar.Scroll += new System.EventHandler(this.confidenceTrackBar_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(6, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 27);
            this.label2.TabIndex = 3;
            this.label2.Text = "信度";
            // 
            // confidenceTextBox
            // 
            this.confidenceTextBox.Location = new System.Drawing.Point(281, 161);
            this.confidenceTextBox.Name = "confidenceTextBox";
            this.confidenceTextBox.Size = new System.Drawing.Size(89, 21);
            this.confidenceTextBox.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.srPortTextBox);
            this.groupBox1.Controls.Add(this.srIpTextBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.srPicBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cmdfilecomboBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.srBtn);
            this.groupBox1.Controls.Add(this.srRichTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.confidenceTextBox);
            this.groupBox1.Controls.Add(this.confidenceTrackBar);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(785, 344);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "语音识别(speech recognize)";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(394, 138);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 19);
            this.label7.TabIndex = 14;
            this.label7.Text = "接收port";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(394, 99);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 19);
            this.label6.TabIndex = 13;
            this.label6.Text = "接收ip";
            // 
            // srPortTextBox
            // 
            this.srPortTextBox.Enabled = false;
            this.srPortTextBox.Location = new System.Drawing.Point(547, 138);
            this.srPortTextBox.Name = "srPortTextBox";
            this.srPortTextBox.Size = new System.Drawing.Size(142, 21);
            this.srPortTextBox.TabIndex = 12;
            // 
            // srIpTextBox
            // 
            this.srIpTextBox.Enabled = false;
            this.srIpTextBox.Location = new System.Drawing.Point(546, 97);
            this.srIpTextBox.Name = "srIpTextBox";
            this.srIpTextBox.Size = new System.Drawing.Size(232, 21);
            this.srIpTextBox.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("STCaiyun", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(157, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 30);
            this.label5.TabIndex = 10;
            this.label5.Text = "工作状态";
            // 
            // srPicBox
            // 
            this.srPicBox.Image = global::XM_speech2._0.Properties.Resources.msg_error;
            this.srPicBox.Location = new System.Drawing.Point(11, 20);
            this.srPicBox.Name = "srPicBox";
            this.srPicBox.Size = new System.Drawing.Size(84, 75);
            this.srPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.srPicBox.TabIndex = 9;
            this.srPicBox.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(383, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(156, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "请选择识别环境";
            // 
            // cmdfilecomboBox
            // 
            this.cmdfilecomboBox.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmdfilecomboBox.FormattingEnabled = true;
            this.cmdfilecomboBox.Items.AddRange(new object[] {
            "follow.ini",
            "whoiswho.ini",
            "GPSR.ini",
            "shopping.ini",
            "general.ini",
            "whatdidyousay.ini",
            "emmergency.ini"});
            this.cmdfilecomboBox.Location = new System.Drawing.Point(545, 48);
            this.cmdfilecomboBox.Name = "cmdfilecomboBox";
            this.cmdfilecomboBox.Size = new System.Drawing.Size(234, 34);
            this.cmdfilecomboBox.TabIndex = 7;
            this.cmdfilecomboBox.SelectedIndexChanged += new System.EventHandler(this.cmdfilecomboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("STHupo", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(519, 297);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(186, 22);
            this.label3.TabIndex = 6;
            this.label3.Text = "开启语音识别服务";
            // 
            // srBtn
            // 
            this.srBtn.Image = global::XM_speech2._0.Properties.Resources._131;
            this.srBtn.Location = new System.Drawing.Point(570, 191);
            this.srBtn.Name = "srBtn";
            this.srBtn.Size = new System.Drawing.Size(75, 75);
            this.srBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.srBtn.TabIndex = 5;
            this.srBtn.TabStop = false;
            this.srBtn.Click += new System.EventHandler(this.srBtn_Click);
            this.srBtn.MouseEnter += new System.EventHandler(this.strbtn_MouseEnter);
            this.srBtn.MouseLeave += new System.EventHandler(this.strbtn_MouseLeave);
            // 
            // GPSRtimer
            // 
            this.GPSRtimer.Tick += new System.EventHandler(this.GPSRtimer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 366);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "小萌语音模块";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.confidenceTrackBar)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.srPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.srBtn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox srRichTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar confidenceTrackBar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox confidenceTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmdfilecomboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox srBtn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox srPortTextBox;
        private System.Windows.Forms.TextBox srIpTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox srPicBox;
        private System.Windows.Forms.Timer GPSRtimer;
    }
}

