namespace Serial_Monitor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cBPortName = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cBBaudRate = new System.Windows.Forms.ComboBox();
            this.pnlLeftInterface = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.pnlBottomInterface = new System.Windows.Forms.Panel();
            this.btnAutoscroll = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnClearTextBox = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.richTextBoxOutput = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.pnlLeftInterface.SuspendLayout();
            this.pnlBottomInterface.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.DarkGray;
            this.groupBox1.Controls.Add(this.cBPortName);
            this.groupBox1.Font = new System.Drawing.Font("Lucida Bright", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(6, 49);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.groupBox1.Size = new System.Drawing.Size(219, 79);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Port Name";
            // 
            // cBPortName
            // 
            this.cBPortName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cBPortName.Font = new System.Drawing.Font("Lucida Fax", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cBPortName.FormattingEnabled = true;
            this.cBPortName.Location = new System.Drawing.Point(7, 29);
            this.cBPortName.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.cBPortName.Name = "cBPortName";
            this.cBPortName.Size = new System.Drawing.Size(200, 40);
            this.cBPortName.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.DarkGray;
            this.groupBox2.Controls.Add(this.cBBaudRate);
            this.groupBox2.Font = new System.Drawing.Font("Lucida Bright", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(6, 138);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.groupBox2.Size = new System.Drawing.Size(219, 79);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Baudrate";
            // 
            // cBBaudRate
            // 
            this.cBBaudRate.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cBBaudRate.Font = new System.Drawing.Font("Lucida Fax", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cBBaudRate.FormattingEnabled = true;
            this.cBBaudRate.Items.AddRange(new object[] {
            "9600",
            "14400",
            "19200",
            "38400",
            "57600",
            "115200",
            "128000",
            "230400",
            "250000"});
            this.cBBaudRate.Location = new System.Drawing.Point(7, 29);
            this.cBBaudRate.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.cBBaudRate.Name = "cBBaudRate";
            this.cBBaudRate.Size = new System.Drawing.Size(200, 40);
            this.cBBaudRate.TabIndex = 0;
            // 
            // pnlLeftInterface
            // 
            this.pnlLeftInterface.BackColor = System.Drawing.Color.DimGray;
            this.pnlLeftInterface.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlLeftInterface.Controls.Add(this.btnRefresh);
            this.pnlLeftInterface.Controls.Add(this.btnConnect);
            this.pnlLeftInterface.Controls.Add(this.groupBox1);
            this.pnlLeftInterface.Controls.Add(this.groupBox2);
            this.pnlLeftInterface.Location = new System.Drawing.Point(6, 6);
            this.pnlLeftInterface.Name = "pnlLeftInterface";
            this.pnlLeftInterface.Size = new System.Drawing.Size(235, 532);
            this.pnlLeftInterface.TabIndex = 3;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(13, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(200, 37);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh Port Names";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.DimGray;
            this.btnConnect.Location = new System.Drawing.Point(6, 251);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(219, 75);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // pnlBottomInterface
            // 
            this.pnlBottomInterface.BackColor = System.Drawing.Color.DimGray;
            this.pnlBottomInterface.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlBottomInterface.Controls.Add(this.btnAutoscroll);
            this.pnlBottomInterface.Controls.Add(this.button2);
            this.pnlBottomInterface.Controls.Add(this.btnClearTextBox);
            this.pnlBottomInterface.Location = new System.Drawing.Point(248, 397);
            this.pnlBottomInterface.Name = "pnlBottomInterface";
            this.pnlBottomInterface.Size = new System.Drawing.Size(630, 141);
            this.pnlBottomInterface.TabIndex = 4;
            // 
            // btnAutoscroll
            // 
            this.btnAutoscroll.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnAutoscroll.Location = new System.Drawing.Point(3, 46);
            this.btnAutoscroll.Name = "btnAutoscroll";
            this.btnAutoscroll.Size = new System.Drawing.Size(149, 37);
            this.btnAutoscroll.TabIndex = 7;
            this.btnAutoscroll.Text = "Autoscroll";
            this.btnAutoscroll.UseVisualStyleBackColor = false;
            this.btnAutoscroll.Click += new System.EventHandler(this.btnAutoscroll_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(3, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(149, 37);
            this.button2.TabIndex = 6;
            this.button2.Text = "Time Stamp";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnClearTextBox
            // 
            this.btnClearTextBox.Location = new System.Drawing.Point(460, 3);
            this.btnClearTextBox.Name = "btnClearTextBox";
            this.btnClearTextBox.Size = new System.Drawing.Size(163, 37);
            this.btnClearTextBox.TabIndex = 5;
            this.btnClearTextBox.Text = "Clear Text Box";
            this.btnClearTextBox.UseVisualStyleBackColor = true;
            this.btnClearTextBox.Click += new System.EventHandler(this.btnClearTextBox_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.DarkGray;
            this.groupBox3.Controls.Add(this.richTextBoxOutput);
            this.groupBox3.Font = new System.Drawing.Font("Lucida Bright", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(248, 6);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.groupBox3.Size = new System.Drawing.Size(630, 383);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Text Box";
            // 
            // richTextBoxOutput
            // 
            this.richTextBoxOutput.Location = new System.Drawing.Point(9, 31);
            this.richTextBoxOutput.Name = "richTextBoxOutput";
            this.richTextBoxOutput.Size = new System.Drawing.Size(612, 344);
            this.richTextBoxOutput.TabIndex = 6;
            this.richTextBoxOutput.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(888, 544);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.pnlBottomInterface);
            this.Controls.Add(this.pnlLeftInterface);
            this.Font = new System.Drawing.Font("Lucida Bright", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Serial Monitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.pnlLeftInterface.ResumeLayout(false);
            this.pnlBottomInterface.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cBPortName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cBBaudRate;
        private System.Windows.Forms.Panel pnlLeftInterface;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel pnlBottomInterface;
        private System.Windows.Forms.Button btnClearTextBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnAutoscroll;
        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.RichTextBox richTextBoxOutput;
    }
}

