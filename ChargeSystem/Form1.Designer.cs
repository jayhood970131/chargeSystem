namespace ChargeSystem
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbxPort = new System.Windows.Forms.TextBox();
            this.tbxBill = new System.Windows.Forms.TextBox();
            this.tbxChargePeriod = new System.Windows.Forms.TextBox();
            this.tbxIP1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbxIP2 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbxIP3 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbxIP4 = new System.Windows.Forms.TextBox();
            this.cbxAutoCharge = new System.Windows.Forms.CheckBox();
            this.cobxChannel = new System.Windows.Forms.ComboBox();
            this.cobxPower = new System.Windows.Forms.ComboBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnInit = new System.Windows.Forms.Button();
            this.btnOpenInstrument = new System.Windows.Forms.Button();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.tbxLog = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tbxOBU_ID = new System.Windows.Forms.TextBox();
            this.tbxOBU_CarNum = new System.Windows.Forms.TextBox();
            this.tbxCardID = new System.Windows.Forms.TextBox();
            this.tbxCardVehicleNum = new System.Windows.Forms.TextBox();
            this.btnManualCharge = new System.Windows.Forms.Button();
            this.timerAutoCharge = new System.Windows.Forms.Timer(this.components);
            this.gpBox_IPCTRL = new System.Windows.Forms.GroupBox();
            this.btnSaveLog = new System.Windows.Forms.Button();
            this.gpBox_Para1 = new System.Windows.Forms.GroupBox();
            this.btnGetVst = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbxDisplayFilename = new System.Windows.Forms.RichTextBox();
            this.btnUpdataFile = new System.Windows.Forms.Button();
            this.btnChooseBin = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.tbxB5Display = new System.Windows.Forms.TextBox();
            this.lbB5Success = new System.Windows.Forms.Label();
            this.gpBoxTongji = new System.Windows.Forms.GroupBox();
            this.tbxB5Num = new System.Windows.Forms.TextBox();
            this.lbB5 = new System.Windows.Forms.Label();
            this.tbxB4Num = new System.Windows.Forms.TextBox();
            this.lbB4 = new System.Windows.Forms.Label();
            this.tbxB5Success = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.gpBox_IPCTRL.SuspendLayout();
            this.gpBox_Para1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gpBoxTongji.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "端口";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "信道";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "功率";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "金额(分）";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 130);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "扣费间隔(s)";
            // 
            // tbxPort
            // 
            this.tbxPort.Location = new System.Drawing.Point(58, 73);
            this.tbxPort.Name = "tbxPort";
            this.tbxPort.Size = new System.Drawing.Size(100, 21);
            this.tbxPort.TabIndex = 5;
            this.tbxPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericTbx_KeyPress);
            // 
            // tbxBill
            // 
            this.tbxBill.Location = new System.Drawing.Point(89, 90);
            this.tbxBill.MaxLength = 10;
            this.tbxBill.Name = "tbxBill";
            this.tbxBill.Size = new System.Drawing.Size(103, 21);
            this.tbxBill.TabIndex = 8;
            this.tbxBill.TextChanged += new System.EventHandler(this.tbxBill_TextChanged);
            this.tbxBill.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericTbx_KeyPress);
            // 
            // tbxChargePeriod
            // 
            this.tbxChargePeriod.Location = new System.Drawing.Point(89, 126);
            this.tbxChargePeriod.MaxLength = 2;
            this.tbxChargePeriod.Name = "tbxChargePeriod";
            this.tbxChargePeriod.Size = new System.Drawing.Size(103, 21);
            this.tbxChargePeriod.TabIndex = 9;
            this.tbxChargePeriod.TextChanged += new System.EventHandler(this.tbxChargePeriod_TextChanged);
            this.tbxChargePeriod.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericTbx_KeyPress);
            // 
            // tbxIP1
            // 
            this.tbxIP1.Location = new System.Drawing.Point(58, 28);
            this.tbxIP1.MaxLength = 3;
            this.tbxIP1.Name = "tbxIP1";
            this.tbxIP1.Size = new System.Drawing.Size(37, 21);
            this.tbxIP1.TabIndex = 1;
            this.tbxIP1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbxIP1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericTbx_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(101, 37);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(11, 12);
            this.label8.TabIndex = 13;
            this.label8.Text = ".";
            // 
            // tbxIP2
            // 
            this.tbxIP2.Location = new System.Drawing.Point(118, 28);
            this.tbxIP2.MaxLength = 3;
            this.tbxIP2.Name = "tbxIP2";
            this.tbxIP2.Size = new System.Drawing.Size(37, 21);
            this.tbxIP2.TabIndex = 2;
            this.tbxIP2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbxIP2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericTbx_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(161, 37);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(11, 12);
            this.label9.TabIndex = 15;
            this.label9.Text = ".";
            // 
            // tbxIP3
            // 
            this.tbxIP3.Location = new System.Drawing.Point(178, 28);
            this.tbxIP3.MaxLength = 3;
            this.tbxIP3.Name = "tbxIP3";
            this.tbxIP3.Size = new System.Drawing.Size(37, 21);
            this.tbxIP3.TabIndex = 3;
            this.tbxIP3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbxIP3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericTbx_KeyPress);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(221, 37);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(11, 12);
            this.label10.TabIndex = 17;
            this.label10.Text = ".";
            // 
            // tbxIP4
            // 
            this.tbxIP4.Location = new System.Drawing.Point(238, 28);
            this.tbxIP4.MaxLength = 3;
            this.tbxIP4.Name = "tbxIP4";
            this.tbxIP4.Size = new System.Drawing.Size(37, 21);
            this.tbxIP4.TabIndex = 4;
            this.tbxIP4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbxIP4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericTbx_KeyPress);
            // 
            // cbxAutoCharge
            // 
            this.cbxAutoCharge.AutoSize = true;
            this.cbxAutoCharge.Location = new System.Drawing.Point(14, 159);
            this.cbxAutoCharge.Name = "cbxAutoCharge";
            this.cbxAutoCharge.Size = new System.Drawing.Size(72, 16);
            this.cbxAutoCharge.TabIndex = 19;
            this.cbxAutoCharge.Text = "自动扣费";
            this.cbxAutoCharge.UseVisualStyleBackColor = true;
            this.cbxAutoCharge.CheckedChanged += new System.EventHandler(this.cbxAutoCharge_CheckedChanged);
            // 
            // cobxChannel
            // 
            this.cobxChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobxChannel.FormattingEnabled = true;
            this.cobxChannel.Location = new System.Drawing.Point(89, 17);
            this.cobxChannel.Name = "cobxChannel";
            this.cobxChannel.Size = new System.Drawing.Size(103, 20);
            this.cobxChannel.TabIndex = 20;
            // 
            // cobxPower
            // 
            this.cobxPower.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobxPower.FormattingEnabled = true;
            this.cobxPower.Location = new System.Drawing.Point(89, 55);
            this.cobxPower.Name = "cobxPower";
            this.cobxPower.Size = new System.Drawing.Size(103, 20);
            this.cobxPower.TabIndex = 21;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(20, 120);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(135, 27);
            this.btnConnect.TabIndex = 22;
            this.btnConnect.Text = "连接设备";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnInit
            // 
            this.btnInit.Location = new System.Drawing.Point(29, 192);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(100, 30);
            this.btnInit.TabIndex = 23;
            this.btnInit.Text = "初始化";
            this.btnInit.UseVisualStyleBackColor = true;
            this.btnInit.Click += new System.EventHandler(this.btnInit_Click);
            // 
            // btnOpenInstrument
            // 
            this.btnOpenInstrument.Location = new System.Drawing.Point(29, 227);
            this.btnOpenInstrument.Name = "btnOpenInstrument";
            this.btnOpenInstrument.Size = new System.Drawing.Size(100, 30);
            this.btnOpenInstrument.TabIndex = 24;
            this.btnOpenInstrument.Text = "打开设备";
            this.btnOpenInstrument.UseVisualStyleBackColor = true;
            this.btnOpenInstrument.Click += new System.EventHandler(this.btnOpenInstrument_Click);
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(147, 192);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(100, 30);
            this.btnClearLog.TabIndex = 25;
            this.btnClearLog.Text = "清空日志";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // tbxLog
            // 
            this.tbxLog.Location = new System.Drawing.Point(12, 345);
            this.tbxLog.Multiline = true;
            this.tbxLog.Name = "tbxLog";
            this.tbxLog.ReadOnly = true;
            this.tbxLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbxLog.Size = new System.Drawing.Size(801, 230);
            this.tbxLog.TabIndex = 26;
            this.tbxLog.TextChanged += new System.EventHandler(this.tbxLog_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(358, 218);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 27;
            this.label7.Text = "OBU ID";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(358, 250);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 28;
            this.label11.Text = "OBU 车牌";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(358, 286);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 12);
            this.label12.TabIndex = 29;
            this.label12.Text = "卡ID";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(358, 321);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 12);
            this.label13.TabIndex = 30;
            this.label13.Text = "卡车牌";
            // 
            // tbxOBU_ID
            // 
            this.tbxOBU_ID.Enabled = false;
            this.tbxOBU_ID.Location = new System.Drawing.Point(414, 209);
            this.tbxOBU_ID.MaxLength = 11;
            this.tbxOBU_ID.Name = "tbxOBU_ID";
            this.tbxOBU_ID.Size = new System.Drawing.Size(131, 21);
            this.tbxOBU_ID.TabIndex = 31;
            this.tbxOBU_ID.TextChanged += new System.EventHandler(this.OBUIDtbx_TextChanged);
            this.tbxOBU_ID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HexRichTbx_KeyPress);
            // 
            // tbxOBU_CarNum
            // 
            this.tbxOBU_CarNum.Enabled = false;
            this.tbxOBU_CarNum.Location = new System.Drawing.Point(414, 247);
            this.tbxOBU_CarNum.Name = "tbxOBU_CarNum";
            this.tbxOBU_CarNum.Size = new System.Drawing.Size(131, 21);
            this.tbxOBU_CarNum.TabIndex = 32;
            // 
            // tbxCardID
            // 
            this.tbxCardID.Enabled = false;
            this.tbxCardID.Location = new System.Drawing.Point(414, 283);
            this.tbxCardID.Name = "tbxCardID";
            this.tbxCardID.Size = new System.Drawing.Size(131, 21);
            this.tbxCardID.TabIndex = 33;
            // 
            // tbxCardVehicleNum
            // 
            this.tbxCardVehicleNum.Enabled = false;
            this.tbxCardVehicleNum.Location = new System.Drawing.Point(414, 318);
            this.tbxCardVehicleNum.Name = "tbxCardVehicleNum";
            this.tbxCardVehicleNum.Size = new System.Drawing.Size(131, 21);
            this.tbxCardVehicleNum.TabIndex = 34;
            // 
            // btnManualCharge
            // 
            this.btnManualCharge.Location = new System.Drawing.Point(147, 264);
            this.btnManualCharge.Name = "btnManualCharge";
            this.btnManualCharge.Size = new System.Drawing.Size(100, 30);
            this.btnManualCharge.TabIndex = 35;
            this.btnManualCharge.Text = "手动扣费";
            this.btnManualCharge.UseVisualStyleBackColor = true;
            this.btnManualCharge.Visible = false;
            this.btnManualCharge.Click += new System.EventHandler(this.btnManualCharge_Click);
            // 
            // timerAutoCharge
            // 
            this.timerAutoCharge.Interval = 10000;
            this.timerAutoCharge.Tick += new System.EventHandler(this.timerAutoCharge_Tick);
            // 
            // gpBox_IPCTRL
            // 
            this.gpBox_IPCTRL.Controls.Add(this.label1);
            this.gpBox_IPCTRL.Controls.Add(this.label2);
            this.gpBox_IPCTRL.Controls.Add(this.tbxPort);
            this.gpBox_IPCTRL.Controls.Add(this.tbxIP1);
            this.gpBox_IPCTRL.Controls.Add(this.label8);
            this.gpBox_IPCTRL.Controls.Add(this.tbxIP2);
            this.gpBox_IPCTRL.Controls.Add(this.label9);
            this.gpBox_IPCTRL.Controls.Add(this.tbxIP3);
            this.gpBox_IPCTRL.Controls.Add(this.label10);
            this.gpBox_IPCTRL.Controls.Add(this.tbxIP4);
            this.gpBox_IPCTRL.Controls.Add(this.btnConnect);
            this.gpBox_IPCTRL.Location = new System.Drawing.Point(29, 22);
            this.gpBox_IPCTRL.Name = "gpBox_IPCTRL";
            this.gpBox_IPCTRL.Size = new System.Drawing.Size(296, 162);
            this.gpBox_IPCTRL.TabIndex = 36;
            this.gpBox_IPCTRL.TabStop = false;
            this.gpBox_IPCTRL.Text = "TCP连接";
            // 
            // btnSaveLog
            // 
            this.btnSaveLog.Location = new System.Drawing.Point(147, 228);
            this.btnSaveLog.Name = "btnSaveLog";
            this.btnSaveLog.Size = new System.Drawing.Size(100, 30);
            this.btnSaveLog.TabIndex = 38;
            this.btnSaveLog.Text = "保存日志";
            this.btnSaveLog.UseVisualStyleBackColor = true;
            this.btnSaveLog.Click += new System.EventHandler(this.btnSaveLog_Click);
            // 
            // gpBox_Para1
            // 
            this.gpBox_Para1.Controls.Add(this.cobxPower);
            this.gpBox_Para1.Controls.Add(this.cobxChannel);
            this.gpBox_Para1.Controls.Add(this.tbxChargePeriod);
            this.gpBox_Para1.Controls.Add(this.tbxBill);
            this.gpBox_Para1.Controls.Add(this.label6);
            this.gpBox_Para1.Controls.Add(this.label5);
            this.gpBox_Para1.Controls.Add(this.label4);
            this.gpBox_Para1.Controls.Add(this.label3);
            this.gpBox_Para1.Controls.Add(this.cbxAutoCharge);
            this.gpBox_Para1.Location = new System.Drawing.Point(341, 22);
            this.gpBox_Para1.Name = "gpBox_Para1";
            this.gpBox_Para1.Size = new System.Drawing.Size(204, 181);
            this.gpBox_Para1.TabIndex = 39;
            this.gpBox_Para1.TabStop = false;
            this.gpBox_Para1.Text = "设置1";
            // 
            // btnGetVst
            // 
            this.btnGetVst.Location = new System.Drawing.Point(29, 264);
            this.btnGetVst.Name = "btnGetVst";
            this.btnGetVst.Size = new System.Drawing.Size(100, 30);
            this.btnGetVst.TabIndex = 41;
            this.btnGetVst.Text = "获取VST";
            this.btnGetVst.UseVisualStyleBackColor = true;
            this.btnGetVst.Visible = false;
            this.btnGetVst.Click += new System.EventHandler(this.btnGetVst_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbxDisplayFilename);
            this.groupBox1.Controls.Add(this.btnUpdataFile);
            this.groupBox1.Controls.Add(this.btnChooseBin);
            this.groupBox1.Location = new System.Drawing.Point(561, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(329, 176);
            this.groupBox1.TabIndex = 42;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "固件功能";
            // 
            // tbxDisplayFilename
            // 
            this.tbxDisplayFilename.Location = new System.Drawing.Point(100, 32);
            this.tbxDisplayFilename.Name = "tbxDisplayFilename";
            this.tbxDisplayFilename.ReadOnly = true;
            this.tbxDisplayFilename.Size = new System.Drawing.Size(178, 33);
            this.tbxDisplayFilename.TabIndex = 1;
            this.tbxDisplayFilename.Text = "";
            // 
            // btnUpdataFile
            // 
            this.btnUpdataFile.Location = new System.Drawing.Point(6, 78);
            this.btnUpdataFile.Name = "btnUpdataFile";
            this.btnUpdataFile.Size = new System.Drawing.Size(133, 33);
            this.btnUpdataFile.TabIndex = 0;
            this.btnUpdataFile.Text = "发送固件升级包";
            this.btnUpdataFile.UseVisualStyleBackColor = true;
            this.btnUpdataFile.Click += new System.EventHandler(this.btnUpdataFile_Click);
            // 
            // btnChooseBin
            // 
            this.btnChooseBin.Location = new System.Drawing.Point(6, 32);
            this.btnChooseBin.Name = "btnChooseBin";
            this.btnChooseBin.Size = new System.Drawing.Size(99, 33);
            this.btnChooseBin.TabIndex = 0;
            this.btnChooseBin.Text = "选择固件";
            this.btnChooseBin.UseVisualStyleBackColor = true;
            this.btnChooseBin.Click += new System.EventHandler(this.btnChooseBin_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(565, 221);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 16);
            this.label14.TabIndex = 43;
            this.label14.Text = "B5帧";
            // 
            // tbxB5Display
            // 
            this.tbxB5Display.Location = new System.Drawing.Point(611, 218);
            this.tbxB5Display.Name = "tbxB5Display";
            this.tbxB5Display.ReadOnly = true;
            this.tbxB5Display.Size = new System.Drawing.Size(206, 21);
            this.tbxB5Display.TabIndex = 44;
            // 
            // lbB5Success
            // 
            this.lbB5Success.AutoSize = true;
            this.lbB5Success.Font = new System.Drawing.Font("宋体", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbB5Success.Location = new System.Drawing.Point(614, 264);
            this.lbB5Success.Name = "lbB5Success";
            this.lbB5Success.Size = new System.Drawing.Size(52, 56);
            this.lbB5Success.TabIndex = 45;
            this.lbB5Success.Text = " ";
            // 
            // gpBoxTongji
            // 
            this.gpBoxTongji.Controls.Add(this.tbxB5Success);
            this.gpBoxTongji.Controls.Add(this.label15);
            this.gpBoxTongji.Controls.Add(this.tbxB5Num);
            this.gpBoxTongji.Controls.Add(this.lbB5);
            this.gpBoxTongji.Controls.Add(this.tbxB4Num);
            this.gpBoxTongji.Controls.Add(this.lbB4);
            this.gpBoxTongji.Location = new System.Drawing.Point(832, 218);
            this.gpBoxTongji.Name = "gpBoxTongji";
            this.gpBoxTongji.Size = new System.Drawing.Size(248, 134);
            this.gpBoxTongji.TabIndex = 46;
            this.gpBoxTongji.TabStop = false;
            this.gpBoxTongji.Text = "统计B4B5";
            // 
            // tbxB5Num
            // 
            this.tbxB5Num.Location = new System.Drawing.Point(99, 65);
            this.tbxB5Num.Name = "tbxB5Num";
            this.tbxB5Num.ReadOnly = true;
            this.tbxB5Num.Size = new System.Drawing.Size(115, 21);
            this.tbxB5Num.TabIndex = 47;
            this.tbxB5Num.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbB5
            // 
            this.lbB5.AutoSize = true;
            this.lbB5.Location = new System.Drawing.Point(16, 68);
            this.lbB5.Name = "lbB5";
            this.lbB5.Size = new System.Drawing.Size(77, 12);
            this.lbB5.TabIndex = 46;
            this.lbB5.Text = "B5帧出现次数";
            // 
            // tbxB4Num
            // 
            this.tbxB4Num.Location = new System.Drawing.Point(99, 28);
            this.tbxB4Num.Name = "tbxB4Num";
            this.tbxB4Num.ReadOnly = true;
            this.tbxB4Num.Size = new System.Drawing.Size(115, 21);
            this.tbxB4Num.TabIndex = 45;
            this.tbxB4Num.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbB4
            // 
            this.lbB4.AutoSize = true;
            this.lbB4.Location = new System.Drawing.Point(16, 31);
            this.lbB4.Name = "lbB4";
            this.lbB4.Size = new System.Drawing.Size(77, 12);
            this.lbB4.TabIndex = 0;
            this.lbB4.Text = "B4帧出现次数";
            // 
            // tbxB5Success
            // 
            this.tbxB5Success.Location = new System.Drawing.Point(99, 97);
            this.tbxB5Success.Name = "tbxB5Success";
            this.tbxB5Success.ReadOnly = true;
            this.tbxB5Success.Size = new System.Drawing.Size(115, 21);
            this.tbxB5Success.TabIndex = 49;
            this.tbxB5Success.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(16, 100);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(77, 12);
            this.label15.TabIndex = 48;
            this.label15.Text = "B5帧成功次数";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1092, 604);
            this.Controls.Add(this.gpBoxTongji);
            this.Controls.Add(this.lbB5Success);
            this.Controls.Add(this.tbxB5Display);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnGetVst);
            this.Controls.Add(this.gpBox_Para1);
            this.Controls.Add(this.btnSaveLog);
            this.Controls.Add(this.btnManualCharge);
            this.Controls.Add(this.tbxCardVehicleNum);
            this.Controls.Add(this.tbxCardID);
            this.Controls.Add(this.tbxOBU_CarNum);
            this.Controls.Add(this.tbxOBU_ID);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbxLog);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.btnOpenInstrument);
            this.Controls.Add(this.btnInit);
            this.Controls.Add(this.gpBox_IPCTRL);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gpBox_IPCTRL.ResumeLayout(false);
            this.gpBox_IPCTRL.PerformLayout();
            this.gpBox_Para1.ResumeLayout(false);
            this.gpBox_Para1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.gpBoxTongji.ResumeLayout(false);
            this.gpBoxTongji.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbxPort;
        private System.Windows.Forms.TextBox tbxBill;
        private System.Windows.Forms.TextBox tbxChargePeriod;
        private System.Windows.Forms.TextBox tbxIP1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbxIP2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbxIP3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbxIP4;
        private System.Windows.Forms.CheckBox cbxAutoCharge;
        private System.Windows.Forms.ComboBox cobxChannel;
        private System.Windows.Forms.ComboBox cobxPower;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnInit;
        private System.Windows.Forms.Button btnOpenInstrument;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.TextBox tbxLog;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbxOBU_ID;
        private System.Windows.Forms.TextBox tbxOBU_CarNum;
        private System.Windows.Forms.TextBox tbxCardID;
        private System.Windows.Forms.TextBox tbxCardVehicleNum;
        private System.Windows.Forms.Button btnManualCharge;
        private System.Windows.Forms.Timer timerAutoCharge;
        private System.Windows.Forms.GroupBox gpBox_IPCTRL;
        private System.Windows.Forms.Button btnSaveLog;
        private System.Windows.Forms.GroupBox gpBox_Para1;
        private System.Windows.Forms.Button btnGetVst;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnUpdataFile;
        private System.Windows.Forms.Button btnChooseBin;
        private System.Windows.Forms.RichTextBox tbxDisplayFilename;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbxB5Display;
        private System.Windows.Forms.Label lbB5Success;
        private System.Windows.Forms.GroupBox gpBoxTongji;
        private System.Windows.Forms.TextBox tbxB4Num;
        private System.Windows.Forms.Label lbB4;
        private System.Windows.Forms.TextBox tbxB5Num;
        private System.Windows.Forms.Label lbB5;
        private System.Windows.Forms.TextBox tbxB5Success;
        private System.Windows.Forms.Label label15;
    }
}

