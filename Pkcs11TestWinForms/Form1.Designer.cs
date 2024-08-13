namespace Pkcs11TestWinForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlMain = new Panel();
            pnlLog = new Panel();
            txtLog = new TextBox();
            pnlTop = new Panel();
            btnExecute = new Button();
            btnStop = new Button();
            btnInitialize = new Button();
            btnStart = new Button();
            txtURL = new TextBox();
            label1 = new Label();
            pnlMain.SuspendLayout();
            pnlLog.SuspendLayout();
            pnlTop.SuspendLayout();
            SuspendLayout();
            // 
            // pnlMain
            // 
            pnlMain.Controls.Add(pnlLog);
            pnlMain.Controls.Add(pnlTop);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 0);
            pnlMain.Margin = new Padding(4);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(1000, 562);
            pnlMain.TabIndex = 0;
            // 
            // pnlLog
            // 
            pnlLog.Controls.Add(txtLog);
            pnlLog.Dock = DockStyle.Fill;
            pnlLog.Location = new Point(0, 196);
            pnlLog.Margin = new Padding(4);
            pnlLog.Name = "pnlLog";
            pnlLog.Size = new Size(1000, 366);
            pnlLog.TabIndex = 1;
            // 
            // txtLog
            // 
            txtLog.Dock = DockStyle.Fill;
            txtLog.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLog.Location = new Point(0, 0);
            txtLog.Margin = new Padding(4);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ScrollBars = ScrollBars.Both;
            txtLog.Size = new Size(1000, 366);
            txtLog.TabIndex = 0;
            // 
            // pnlTop
            // 
            pnlTop.Controls.Add(btnExecute);
            pnlTop.Controls.Add(btnStop);
            pnlTop.Controls.Add(btnInitialize);
            pnlTop.Controls.Add(btnStart);
            pnlTop.Controls.Add(txtURL);
            pnlTop.Controls.Add(label1);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Margin = new Padding(4);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(1000, 196);
            pnlTop.TabIndex = 0;
            // 
            // btnExecute
            // 
            btnExecute.Location = new Point(79, 93);
            btnExecute.Margin = new Padding(4);
            btnExecute.Name = "btnExecute";
            btnExecute.Size = new Size(117, 37);
            btnExecute.TabIndex = 5;
            btnExecute.Text = "EXECUTE";
            btnExecute.UseVisualStyleBackColor = true;
            btnExecute.Click += btnExecute_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(204, 137);
            btnStop.Margin = new Padding(4);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(117, 37);
            btnStop.TabIndex = 4;
            btnStop.Text = "STOP";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // btnInitialize
            // 
            btnInitialize.Location = new Point(79, 49);
            btnInitialize.Margin = new Padding(4);
            btnInitialize.Name = "btnInitialize";
            btnInitialize.Size = new Size(117, 37);
            btnInitialize.TabIndex = 3;
            btnInitialize.Text = "INITIALIZE";
            btnInitialize.UseVisualStyleBackColor = true;
            btnInitialize.Click += btnInitialize_Click;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(79, 137);
            btnStart.Margin = new Padding(4);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(117, 37);
            btnStart.TabIndex = 2;
            btnStart.Text = "START";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // txtURL
            // 
            txtURL.Location = new Point(79, 7);
            txtURL.Margin = new Padding(4);
            txtURL.Name = "txtURL";
            txtURL.Size = new Size(905, 31);
            txtURL.TabIndex = 1;
            txtURL.Text = "http://localhost:5219/";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(27, 12);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(43, 25);
            label1.TabIndex = 0;
            label1.Text = "URL";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 562);
            Controls.Add(pnlMain);
            Margin = new Padding(4);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PKCS11 Test Client";
            Load += Form1_Load;
            pnlMain.ResumeLayout(false);
            pnlLog.ResumeLayout(false);
            pnlLog.PerformLayout();
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlMain;
        private Panel pnlLog;
        private Panel pnlTop;
        private TextBox txtURL;
        private Label label1;
        private Button btnStart;
        private TextBox txtLog;
        private Button btnInitialize;
        private Button btnStop;
        private Button btnExecute;
    }
}
