#define InsideVersion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//TCP
using System.Net.Sockets;
using System.Net;
//Thread
using System.Threading;
//BinaryReader
using System.IO;
namespace ChargeSystem
{
    public enum CTRL_CODE
    {
        INIT = 0xC0,
        INSTRUMENT_ONOFF = 0xC4,
        TRANSCATION_LAUNCHED = 0xC6,
        TRANSCATION_COMPLETED = 0xC1,
        INIT_AUTUHORIZED = 0xCA,
        AUTHORIZATION = 0xCB,
        GET_VST = 0xA0,
        SET_RF = 0xA1,
        DOWNLOAD_BINFILE = 0xA2

    };
    public enum RECV_CTRL_CODE : byte
    {
        INSMT_STATUS = 0xB0,
        HEART_BEAT_PCK = 0xB2,
        CAR_INFO = 0xB4,
        TRANSCATION_INFO = 0xB5,
        GET_VST_ACK = 0xD0,
        SET_RF_ACK = 0xD1,
        DOWNLOAD_BINFILE_ACK = 0xD2

    };
    public enum RSUSWITCH
    {
        CLOSE = 0x00,
        OPEN = 0x01
    };
    public partial class Form1 : Form
    {
        public static int[] nwf580_sense = { -80, -79, -78, -77, -76, -75, -74, -73, -72, -71, -70, -68, -67, -65, -62, -61, -58, -55, -53, -52, -51, -48, -45, -43 };
        public static double[] bandwidth = { 2.38, 3.51, 2.82, 4.23, 4.47, 4.92, 5.32, 5.82 };
        public bool isAutoCharge = false;   //是否自动扣费
        public uint chargePeriod = 0;       //自动扣费周期
        public bool RSUSwitch = false;      //0x00-close 0x01-open
        private bool isConnected = false;    //是否还有tcp连接

        public byte[] recvData = null;
        public int bytesToRead;
        public string filename = "";

        public ChargeProcess chargePara = null;   //保存各参数
        public VehicleInfo carInfo = null;
        public Socket localSocket = null;
        public Thread th;

        public List<Button> listLinkNeededBtn = null; //需要tcp连接上才能使用的按钮

        public ManualResetEventSlim manualResetSendBin;

        public bool isSendEnding = true;

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
#if InsideVersion
            this.Text = "ETCANT_P V1.0.5";
            // this.gpBox_RF.Visible = true;
            this.btnGetVst.Visible = true;
#else
            this.Text = "上位机系统";
            this.gpBox_RF.Visible = false;
            this.btnGetVst.Visible = false;
#endif
            this.cobxChannel.Items.Add("1");
            this.cobxChannel.Items.Add("2");
            this.cobxChannel.SelectedIndex = 0;

            this.tbxIP1.Text = "192";
            this.tbxIP2.Text = "168";
            this.tbxIP3.Text = "1";
            this.tbxIP4.Text = "199";

            this.tbxPort.Text = "5000";

            this.tbxBill.Text = "00000";

            this.tbxOBU_ID.Text = "00 00 00 00";
            this.tbxChargePeriod.Text = "10";


            //功率 0~16
            for (int i = 0; i <= 16; i++)
            {
                this.cobxPower.Items.Add((i + 1).ToString());
            }
            this.cobxPower.SelectedIndex = 16;



            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            //this.MinimizeBox = false;
            chargePara = new ChargeProcess();

            this.listLinkNeededBtn = new List<Button>();
            this.listLinkNeededBtn.Add(this.btnInit);
            this.listLinkNeededBtn.Add(this.btnOpenInstrument);
            this.listLinkNeededBtn.Add(this.btnManualCharge);
            this.listLinkNeededBtn.Add(this.btnGetVst);
            this.listLinkNeededBtn.Add(this.btnChooseBin);


            // 更新固件按钮只有在选择了固件文件后才能打开，初始化为false
            this.btnUpdataFile.Enabled = false;

            //没有连接成功前各按钮不能用
            tcpNeededBtnSet(false);
            //初始状态--打开设备
            this.RSUSwitch = true;
            //自动扣费按钮不可用
            this.cbxAutoCharge.Enabled = false;

            this.carInfo = new VehicleInfo();

            manualResetSendBin = new ManualResetEventSlim(false);

        }


        public void tcpNeededBtnSet(bool status)
        {
            if (status)  //各按钮可用
            {

                for (int i = 0; i < this.listLinkNeededBtn.Count; i++)
                {
                    this.listLinkNeededBtn[i].Enabled = true;
                }
            }
            else
            {
                for (int i = 0; i < this.listLinkNeededBtn.Count; i++)
                {
                    this.listLinkNeededBtn[i].Enabled = false;
                }
            }
        }
        public bool isTcpParaSet()
        {
            bool flag = true;
            TextBox[] tbxTcpArr = {
                                   this.tbxIP1,this.tbxIP2,this.tbxIP3,this.tbxIP4,this.tbxPort
                                };
            for (uint i = 0; i < tbxTcpArr.Length; i++)
            {
                if (tbxTcpArr[i].Text == "")
                {
                    tbxTcpArr[i].BackColor = Color.Red;
                    flag = false;
                }
                else
                {
                    tbxTcpArr[i].BackColor = Color.White;
                }
            }
            return flag;
        }
        /*
         * 获取所有信息
         * 执行这个函数前应做一次需要到的检查，看哪些需要的参数没有设置，不需要的参数不用管
         */
        public void chargeParaInit()
        {
            chargePara.ipAddress = tbxIP1.Text + "." + tbxIP2.Text + "." + tbxIP3.Text + "." + tbxIP4.Text;
            chargePara.port = tbxPort.Text;
            chargePara.channel = (byte)(cobxChannel.SelectedIndex); //信道1：0，信道2：1
            chargePara.power = (byte)(cobxPower.SelectedIndex);
            chargePara.bill = (uint)int.Parse(tbxBill.Text);
            //charPrgs.OBU_ID = (uint)int.Parse(tbxOBU_ID.Text);
            string[] s = this.tbxOBU_ID.Text.Split(' ');
            uint bits_in32 = (uint)int.Parse(s[0],
                System.Globalization.NumberStyles.HexNumber);
            uint bits_in24 = (uint)int.Parse(s[1],
                System.Globalization.NumberStyles.HexNumber);
            uint bits_in16 = (uint)int.Parse(s[2],
                System.Globalization.NumberStyles.HexNumber);
            uint bits_in8 = (uint)int.Parse(s[3],
                System.Globalization.NumberStyles.HexNumber);
            bits_in32 <<= 24;
            bits_in24 <<= 16;
            bits_in16 <<= 8;
            chargePara.OBU_ID = (uint)(bits_in32 | bits_in24 | bits_in16 | bits_in8);
            chargePara.chargePeriod = (byte)(int.Parse(this.tbxChargePeriod.Text));
        }
        /*
         * 点击 连接设备，按照填写的IP地址端口号发起TCP连接，在日志框打印是否连接成功
         */
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (this.isConnected)
            {
                this.btnConnect.Enabled = false;

                //关掉时钟
                this.timerAutoCharge.Enabled = false;
                //各按钮不可用
                tcpNeededBtnSet(false);
                try
                {
                    this.th.Abort();
                    localSocket.Shutdown(SocketShutdown.Both);
                    localSocket.Close();
                }
                catch (Exception ex)
                {
                    this.tbxLog.Text += "\r\n断开连接失败：" + ex.Message + "\r\n";
                    this.btnConnect.Enabled = true;
                    //开时钟
                    this.timerAutoCharge.Enabled = true;
                    //各按钮恢复可用
                    tcpNeededBtnSet(true);
                    return;
                }
                this.btnConnect.Enabled = true;

                if (!localSocket.Connected)
                {
                    this.isConnected = false;
                    this.btnConnect.Text = "连接设备";
                    this.tbxLog.Text += "\r\n已断开连接。" + "\r\n";
                    this.btnConnect.BackColor = Color.Transparent;
                }
            }
            else
            {
                this.btnConnect.Enabled = false;        //连接过程按钮不可用，不让用户多次连接
                if (!isTcpParaSet())
                {
                    MessageBox.Show("IP参数未配置好。", "ERROR");
                    this.btnConnect.Enabled = true;
                    return;
                }
                chargeParaInit();

                //IPEndPoint iep = new IPEndPoint(IPAddress.Parse("192.168.1.112"), 8080);//指定客户端地址与端口  
                IPEndPoint iep = new IPEndPoint(IPAddress.Parse(this.chargePara.ipAddress), int.Parse(this.chargePara.port));//指定客户端地址与端口
                try
                {
                    localSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    Exception exception = null;
                    //localSocket.Connect(iep);
                    ManualResetEventSlim manualReset = new ManualResetEventSlim();
                    localSocket.BeginConnect(iep, ar =>
                    {
                        try
                        {
                            localSocket.EndConnect(ar);
                        }
                        catch (Exception ex)
                        {

                            exception = ex;
                        }
                        finally
                        {
                            manualReset.Set();
                        }

                    }, null);
                    if (!manualReset.Wait(TimeSpan.FromSeconds(5)))
                    {
                        throw new TimeoutException("连接超时");
                    }
                    if (exception != null)
                    {
                        throw exception;
                    }
                    //连接成功后各按钮可用
                    tcpNeededBtnSet(true);

                    //自动扣费可用
                    this.cbxAutoCharge.Enabled = true;
                    if (localSocket.Connected)
                    {
                        this.tbxLog.Text += "\r\n连接成功。" + "\r\n";
                        this.btnConnect.Text = "断开连接";
                        this.isConnected = true;
                    }
                    else
                    {
                        this.tbxLog.Text += "\r\n连接失败。\r\n";
                        localSocket.Close();
                        this.isConnected = false;
                        this.btnConnect.BackColor = Color.Transparent;
                        this.btnConnect.Enabled = true;
                        //没有连接成功前各按钮不能用
                        tcpNeededBtnSet(false);
                        return;
                    }


                    this.th = new Thread(ReceiveMsg);
                    th.Start();
                    th.IsBackground = true;


                    this.btnConnect.BackColor = Color.LightGreen;
                    this.btnConnect.Enabled = true;
                }
                catch (Exception ex)
                {
                    this.tbxLog.Text += "\r\n连接失败:" + ex.Message + "\r\n";
                    localSocket.Close();
                    this.isConnected = false;
                    this.btnConnect.BackColor = Color.Transparent;
                    this.btnConnect.Enabled = true;
                    //没有连接成功前各按钮不能用
                    tcpNeededBtnSet(false);
                    return;
                }
            }
        }

        private delegate void delegateTbxLog(string str);
        private void setTbxLog(string str)
        {
            this.tbxLog.Text += str;
        }

        private delegate void delegateTbxOBUID(string str);
        private void setTbxOBUID(string str)
        {
            this.tbxOBU_ID.Text = str;
        }

        private delegate void delegateTbxCardVehicleNum(string str);
        private void setTbxCardVehicleNum(string str)
        {
            this.tbxCardVehicleNum.Text = str;
        }

        private delegate void delegateTbxOBUCarNum(string str);
        private void setTbxOBUCarNum(string str)
        {
            this.tbxOBU_CarNum.Text = str;
        }


        public void ReceiveMsg()
        {
            while (true)
            {
                //接收服务端信息
                byte[] buffer_Lv1 = new Byte[1024];
                byte[] recvData = null;         //从缓存buffer_Lv1中提取有效数据，包含帧头到CRC校验
                int recvBytes;
                try
                {
                    //if (localSocket.Available <= 0) continue;
                    recvBytes = localSocket.Receive(buffer_Lv1);        //接收数据
                }
                catch (SocketException e)
                {
                    MessageBox.Show(e.Message);
                    return;
                }
                if (buffer_Lv1[0] != 0xff)      //连接出错时下位机会发很多为全0的数据过来
                {
                    btnConnect.BeginInvoke((EventHandler)(delegate       //关掉时钟等
                    {
                        this.timerAutoCharge.Enabled = false;
                        this.btnConnect.BackColor = Color.Transparent;
                        this.btnConnect.Text = "连接设备";
                        //没有连接成功前各按钮不能用
                        this.btnInit.Enabled = false;
                        this.btnOpenInstrument.Enabled = false;
                        this.btnManualCharge.Enabled = false;

                    }));
                    localSocket.Close();
                    isConnected = false;

                    return;
                }
                if (recvBytes < 4)   //检查接收到的数据是否完整
                {
                    btnConnect.BeginInvoke((EventHandler)(delegate       //关掉时钟等
                    {
                        this.tbxLog.Text += "\r\n接收到不符合协议的帧。已丢弃此帧。\r\n";
                    }));

                    return;
                }
                uint len_H = (uint)buffer_Lv1[2] << 8;
                uint len_L = (uint)buffer_Lv1[3];
                uint length = len_H | len_L;        //获取数据长度

                recvData = new byte[length + 6];
                for (uint i = 0; i < recvData.Length; i++)
                {
                    recvData[i] = buffer_Lv1[i];
                }

                //  处理CMDTYPE
                /*
                * 打印收到的数据
                */
                byte[] temp = new byte[1];
                temp[0] = recvData[TCP_Frame.CTRL_CODE_INDEX];
                string str1 = BitConverter.ToString(temp);
                string str2 = BitConverter.ToString(recvData).Replace("-", " ");
                StringBuilder strbuilder = new StringBuilder();
                strbuilder.Append("\r\n" + DateTime.Now.TimeOfDay.ToString() + ":<<" + str1 + "帧," + str2 + "\r\n");
                this.BeginInvoke(new delegateTbxLog(setTbxLog), strbuilder.ToString());
                bool isCmdExist = false;
                foreach (RECV_CTRL_CODE type in Enum.GetValues(typeof(RECV_CTRL_CODE)))
                {
                    if ((byte)type == temp[0])
                    {
                        isCmdExist = true;
                        break;
                    }
                }
                if (!isCmdExist)
                {
                    strbuilder.Clear();
                    strbuilder.Append("\r\n收到未定义命令:" + temp[0].ToString("x2") + "\r\n");
                    this.BeginInvoke(new delegateTbxLog(setTbxLog), strbuilder.ToString());
                    return;
                }
                /*
                 * 进一步处理，提取信息等
                 */
                switch (recvData[4])
                {
                    case (byte)RECV_CTRL_CODE.TRANSCATION_INFO:
                        {
                            displayB5(ref recvData);

                            byte[] replyData = new byte[5];
                            replyData[0] = (byte)CTRL_CODE.TRANSCATION_COMPLETED;

                            chargeParaInit();
                            replyData[1] = (byte)((chargePara.OBU_ID & 0xff000000) >> 24);
                            replyData[2] = (byte)((chargePara.OBU_ID & 0x00ff0000) >> 16);
                            replyData[3] = (byte)((chargePara.OBU_ID & 0x0000ff00) >> 8);
                            replyData[4] = (byte)((chargePara.OBU_ID & 0x000000ff));

                            
                            TCP_Frame tcpFrame = new TCP_Frame();
                            tcpFrame.sealDataToFrame(ref replyData);
                            tcpFrame.sendTcpFrame(ref localSocket, ref this.tbxLog);
                            
                        }
                        break;
                    case (byte)RECV_CTRL_CODE.CAR_INFO:
                        {
                            /*
                            * 提取OBU_ID
                            */
                            byte[] obu_id = new byte[4];
                            obu_id[0] = recvData[5];
                            obu_id[1] = recvData[6];
                            obu_id[2] = recvData[7];
                            obu_id[3] = recvData[8];
                            str1 = BitConverter.ToString(obu_id).Replace("-", " ");
                            strbuilder.Clear();
                            strbuilder.Append("\r\n OBU ID:" + str1 + "\r\n");                           
                            this.BeginInvoke(new delegateTbxOBUID(setTbxOBUID), str1);
                            /*
                             * 提取卡车牌
                             */
                            str1 = "";
                            byte[] cardCarIdNameArr = new byte[2];
                            string cardCarIdName = "";
                            cardCarIdNameArr[0] = recvData[145];
                            cardCarIdNameArr[1] = recvData[146];
                            cardCarIdName = Encoding.GetEncoding("gb2312").GetString(cardCarIdNameArr);
                            for (byte i = 0; i < 12; i++)
                            {
                                //card_VehicleNum[i] = recvData[145 + i]; //卡车牌第一字节位置： 142+4-1 = 145
                                str1 += ((char)recvData[147 + i]).ToString();
                            }
                            //str1 = BitConverter.ToString(card_VehicleNum).Replace("-", " ");  
                            strbuilder.Clear();
                            strbuilder.Append("\r\n 卡车牌：" + cardCarIdName + str1 + "\r\n");
                            this.BeginInvoke(new delegateTbxLog(setTbxLog), strbuilder.ToString());
                            strbuilder.Clear();
                            strbuilder.Append(cardCarIdName + str1);
                            this.BeginInvoke(new delegateTbxCardVehicleNum(setTbxCardVehicleNum), strbuilder.ToString());
                            /*
                             *提取车辆信息
                             */

                            //obu车牌号码
                            byte startPos = 38;
                            byte[] carIdNameArr = new byte[2];
                            string carIdName = "";
                            carIdNameArr[0] = recvData[startPos];
                            carIdNameArr[1] = recvData[startPos + 1];
                            carIdName = Encoding.GetEncoding("gb2312").GetString(carIdNameArr); //车牌省份 共两个字节表示一个汉字

                            string carIdNum = "";       //车牌后的数字和字母，ASCII表示
                            for (byte i = 1; i <= 10; i++)
                            {
                                carIdNum += ((char)recvData[startPos + 1 + i]).ToString();
                            }

                            //显示车牌
                            byte[] carNum = new byte[12];
                            for (byte i = 0; i < 12; i++)
                            {
                                carNum[i] = recvData[startPos + i];
                            }
                            strbuilder.Clear();
                            strbuilder.Append(carIdName + carIdNum);
                            this.BeginInvoke(new delegateTbxOBUCarNum(setTbxOBUCarNum), strbuilder.ToString());
                            str1 = BitConverter.ToString(carNum).Replace("-", " ");
                            strbuilder.Clear();
                            strbuilder.Append("OBU车牌：" + str1 + "\r\n");
                            this.BeginInvoke(new delegateTbxLog(setTbxLog), strbuilder.ToString());

                            chargeParaInit();
                            byte[] data = new byte[59];
                            data[0] = (byte)CTRL_CODE.TRANSCATION_LAUNCHED;

                            data[1] = (byte)((chargePara.OBU_ID & 0xff000000) >> 24);
                            data[2] = (byte)((chargePara.OBU_ID & 0x00ff0000) >> 16);
                            data[3] = (byte)((chargePara.OBU_ID & 0x0000ff00) >> 8);
                            data[4] = (byte)((chargePara.OBU_ID & 0x000000ff));

                            data[5] = (byte)((chargePara.bill & 0xff000000) >> 24);
                            data[6] = (byte)((chargePara.bill & 0x00ff0000) >> 16);
                            data[7] = (byte)((chargePara.bill & 0x0000ff00) >> 8);
                            data[8] = (byte)((chargePara.bill & 0x000000ff));

                            BCD_Code bcd = new BCD_Code();

                            uint year = (uint)int.Parse(DateTime.Now.ToString("yyyy"));
                            //uint year = 2319;

                            byte yOct_th = (byte)Math.Floor((double)(year / 1000)); //千位
                            uint yBcd_th = bcd.octToBcd(yOct_th);

                            uint th_int = (uint)yOct_th * 1000;
                            byte yOct_hdr = (byte)Math.Floor((double)((year - th_int) / 100));  //百位
                            uint yBcd_hdr = bcd.octToBcd(yOct_hdr);

                            uint hunder_int = (uint)yOct_hdr * 100;
                            byte yOct_ten = (byte)Math.Floor((double)((year - th_int - hunder_int) / 10));  //十位
                            uint yBcd_ten = bcd.octToBcd(yOct_ten);

                            uint one_int = (uint)yOct_ten * 10;
                            byte yOct_one = (byte)Math.Floor((double)(year - th_int - hunder_int - one_int));  //个位
                            uint yBcd_one = bcd.octToBcd(yOct_one);

                            uint yearH = (yBcd_th << 4) | yBcd_hdr;
                            uint yearL = (yBcd_ten << 4) | yBcd_one;


                            byte month = (byte)int.Parse(DateTime.Now.ToString("MM"));  //月份
                            byte mOct_ten = (byte)Math.Floor((double)(month / 10));
                            uint mBcd_ten = bcd.octToBcd(mOct_ten);
                            byte mOct_one = (byte)(month - mOct_ten * 10);
                            uint mBcd_one = bcd.octToBcd(mOct_one);
                            uint bcd_month = (mBcd_ten << 4) | mBcd_one;



                            byte day = (byte)int.Parse(DateTime.Now.ToString("dd"));
                            byte dOct_ten = (byte)Math.Floor((double)(day / 10));
                            uint dBcd_ten = bcd.octToBcd(dOct_ten);
                            byte dOct_one = (byte)(day - dOct_ten * 10);
                            uint dBcd_one = bcd.octToBcd(dOct_one);
                            uint bcd_day = (dBcd_ten << 4) | dBcd_one;

                            byte hour = (byte)int.Parse(DateTime.Now.ToString("HH"));
                            byte hOct_ten = (byte)Math.Floor((double)(hour / 10));
                            uint hBcd_ten = bcd.octToBcd(hOct_ten);
                            byte hOct_one = (byte)(hour - hOct_ten * 10);
                            uint hBcd_one = bcd.octToBcd(hOct_one);
                            uint bcd_hour = (hBcd_ten << 4) | hBcd_one;

                            byte minute = (byte)int.Parse(DateTime.Now.ToString("mm")); //分钟
                            byte minOct_ten = (byte)Math.Floor((double)(minute / 10));
                            uint minBcd_ten = bcd.octToBcd(minOct_ten);
                            byte minOct_one = (byte)(minute - minOct_ten * 10);
                            uint minBcd_one = bcd.octToBcd(minOct_one);
                            uint bcd_minute = (minBcd_ten << 4) | minBcd_one;

                            byte sec = (byte)int.Parse(DateTime.Now.ToString("ss"));
                            byte sOct_ten = (byte)Math.Floor((double)(sec / 10));
                            uint sBcd_ten = bcd.octToBcd(sOct_ten);
                            byte sOct_one = (byte)(sec - sOct_ten * 10);
                            uint sBcd_one = bcd.octToBcd(sOct_one);
                            uint bcd_sec = (sBcd_ten << 4) | sBcd_one;

                            data[9] = (byte)yearH;
                            data[10] = (byte)yearL;
                            data[11] = (byte)bcd_month;
                            data[12] = (byte)bcd_day;
                            data[13] = (byte)bcd_hour;
                            data[14] = (byte)bcd_minute;
                            data[15] = (byte)bcd_sec;

                            TCP_Frame tcpFrame2 = new TCP_Frame();
                            tcpFrame2.sealDataToFrame(ref data);
                            tcpFrame2.sendTcpFrame(ref localSocket, ref this.tbxLog);
                        }
                        break;
#if InsideVersion
                    case (byte)RECV_CTRL_CODE.GET_VST_ACK:
                        {
                            /*
                            * 提取OBU_ID
                            */
                            byte[] obu_id = new byte[4];
                            obu_id[0] = recvData[5];
                            obu_id[1] = recvData[6];
                            obu_id[2] = recvData[7];
                            obu_id[3] = recvData[8];
                            str1 = BitConverter.ToString(obu_id).Replace("-", " ");
                            strbuilder.Clear();
                            strbuilder.Append("\r\n OBU ID: " + str1 + "\r\n");
                            this.BeginInvoke(new delegateTbxLog(setTbxLog), strbuilder.ToString());
                            this.BeginInvoke(new delegateTbxOBUID(setTbxOBUID), str1);

                            if (recvData.Length > 97) // 79 + 12 12位车牌号  4位是数据头 2位是crc
                            {
                                /*
                             * 提取OBU车牌号
                             */
                                //obu车牌号码
                                byte startPos = 85;
                                byte[] carIdNameArr = new byte[2];
                                string carIdName = "";
                                carIdNameArr[0] = recvData[startPos];
                                carIdNameArr[1] = recvData[startPos + 1];
                                carIdName = Encoding.GetEncoding("gb2312").GetString(carIdNameArr); //车牌省份 共两个字节表示一个汉字

                                string carIdNum = "";       //车牌后的数字和字母，ASCII表示
                                for (byte i = 1; i <= 10; i++)
                                {
                                    carIdNum += ((char)recvData[startPos + 1 + i]).ToString();
                                }

                                //显示车牌
                                byte[] carNum = new byte[12];
                                for (byte i = 0; i < 12; i++)
                                {
                                    carNum[i] = recvData[startPos + i];
                                }
                                strbuilder.Clear();
                                strbuilder.Append(carIdName + carIdNum);
                                this.BeginInvoke(new delegateTbxOBUCarNum(setTbxOBUCarNum), strbuilder.ToString());
                                str1 = BitConverter.ToString(carNum).Replace("-", " ");
                                strbuilder.Clear();
                                strbuilder.Append("OBU车牌：" + str1 + "\r\n");
                                this.BeginInvoke(new delegateTbxLog(setTbxLog), strbuilder.ToString());
                            }

                        }
                        break;
                    case (byte)RECV_CTRL_CODE.DOWNLOAD_BINFILE_ACK:
                        byte isRecieved = recvData[7];
                        if (isRecieved == 0)
                        {
                            manualResetSendBin.Set();
                        }
                        break;
#endif
                    default:
                        break;

                }
            }
        }

//        private void invokeMethodPrint(ref byte[] recvData)
//        {
//            /*
//             * 打印收到的数据
//             */
//            byte[] temp = new byte[1];
//            temp[0] = recvData[TCP_Frame.CTRL_CODE_INDEX];
//            string str1 = BitConverter.ToString(temp);
//            string str2 = BitConverter.ToString(recvData).Replace("-", " ");
//            tbxLog.Text += "\r\n" + DateTime.Now.TimeOfDay.ToString() + ":<<" + str1 + "帧," + str2 + "\r\n";
//            bool isCmdExist = false;
//            foreach (RECV_CTRL_CODE type in Enum.GetValues(typeof(RECV_CTRL_CODE)))
//            {
//                if ((byte)type == temp[0])
//                {
//                    isCmdExist = true;
//                    break;
//                }
//            }
//            if (!isCmdExist)
//            {
//                this.tbxLog.Text += "\r\n收到未定义命令:" + temp[0].ToString("x2") + "\r\n";
//                return;
//            }
//            /*
//             * 进一步处理，提取信息等
//             */
//            switch (recvData[4])
//            {
//                case (byte)RECV_CTRL_CODE.TRANSCATION_INFO:
//                    {
//                        byte[] replyData = new byte[5];
//                        replyData[0] = (byte)CTRL_CODE.TRANSCATION_COMPLETED;

//                        chargeParaInit();
//                        replyData[1] = (byte)((chargePara.OBU_ID & 0xff000000) >> 24);
//                        replyData[2] = (byte)((chargePara.OBU_ID & 0x00ff0000) >> 16);
//                        replyData[3] = (byte)((chargePara.OBU_ID & 0x0000ff00) >> 8);
//                        replyData[4] = (byte)((chargePara.OBU_ID & 0x000000ff));

//                        TCP_Frame tcpFrame = new TCP_Frame();
//                        tcpFrame.sealDataToFrame(ref replyData);
//                        tcpFrame.sendTcpFrame(ref localSocket, ref this.tbxLog);
//                    }
//                    break;
//                case (byte)RECV_CTRL_CODE.CAR_INFO:
//                    {
//                        /*
//                        * 提取OBU_ID
//                        */
//                        byte[] obu_id = new byte[4];
//                        obu_id[0] = recvData[5];
//                        obu_id[1] = recvData[6];
//                        obu_id[2] = recvData[7];
//                        obu_id[3] = recvData[8];
//                        str1 = BitConverter.ToString(obu_id).Replace("-", " ");
//                        tbxLog.Text += "\r\n OBU ID:" + str1 + "\r\n";
//                        this.tbxOBU_ID.Text = str1;
//                        /*
//                         * 提取卡车牌
//                         */
//                        str1 = "";
//                        byte[] cardCarIdNameArr = new byte[2];
//                        string cardCarIdName = "";
//                        cardCarIdNameArr[0] = recvData[145];
//                        cardCarIdNameArr[1] = recvData[146];
//                        cardCarIdName = Encoding.GetEncoding("gb2312").GetString(cardCarIdNameArr);
//                        for (byte i = 0; i < 12; i++)
//                        {
//                            //card_VehicleNum[i] = recvData[145 + i]; //卡车牌第一字节位置： 142+4-1 = 145
//                            str1 += ((char)recvData[147 + i]).ToString();
//                        }
//                        //str1 = BitConverter.ToString(card_VehicleNum).Replace("-", " ");              
//                        tbxLog.Text += "\r\n 卡车牌：" + cardCarIdName + str1 + "\r\n";
//                        this.tbxCardVehicleNum.Text = cardCarIdName + str1;
//                        /*
//                         *提取车辆信息
//                         */

//                        //obu车牌号码
//                        byte startPos = 38;
//                        byte[] carIdNameArr = new byte[2];
//                        string carIdName = "";
//                        carIdNameArr[0] = recvData[startPos];
//                        carIdNameArr[1] = recvData[startPos + 1];
//                        carIdName = Encoding.GetEncoding("gb2312").GetString(carIdNameArr); //车牌省份 共两个字节表示一个汉字

//                        string carIdNum = "";       //车牌后的数字和字母，ASCII表示
//                        for (byte i = 1; i <= 10; i++)
//                        {
//                            carIdNum += ((char)recvData[startPos + 1 + i]).ToString();
//                        }

//                        //显示车牌
//                        byte[] carNum = new byte[12];
//                        for (byte i = 0; i < 12; i++)
//                        {
//                            carNum[i] = recvData[startPos + i];
//                        }
//                        this.tbxOBU_CarNum.Text = carIdName + carIdNum;
//                        str1 = BitConverter.ToString(carNum).Replace("-", " ");
//                        this.tbxLog.Text += "OBU车牌：" + str1 + "\r\n";

//                        chargeParaInit();
//                        byte[] data = new byte[59];
//                        data[0] = (byte)CTRL_CODE.TRANSCATION_LAUNCHED;

//                        data[1] = (byte)((chargePara.OBU_ID & 0xff000000) >> 24);
//                        data[2] = (byte)((chargePara.OBU_ID & 0x00ff0000) >> 16);
//                        data[3] = (byte)((chargePara.OBU_ID & 0x0000ff00) >> 8);
//                        data[4] = (byte)((chargePara.OBU_ID & 0x000000ff));

//                        data[5] = (byte)((chargePara.bill & 0xff000000) >> 24);
//                        data[6] = (byte)((chargePara.bill & 0x00ff0000) >> 16);
//                        data[7] = (byte)((chargePara.bill & 0x0000ff00) >> 8);
//                        data[8] = (byte)((chargePara.bill & 0x000000ff));

//                        BCD_Code bcd = new BCD_Code();

//                        uint year = (uint)int.Parse(DateTime.Now.ToString("yyyy"));
//                        //uint year = 2319;

//                        byte yOct_th = (byte)Math.Floor((double)(year / 1000)); //千位
//                        uint yBcd_th = bcd.octToBcd(yOct_th);

//                        uint th_int = (uint)yOct_th * 1000;
//                        byte yOct_hdr = (byte)Math.Floor((double)((year - th_int) / 100));  //百位
//                        uint yBcd_hdr = bcd.octToBcd(yOct_hdr);

//                        uint hunder_int = (uint)yOct_hdr * 100;
//                        byte yOct_ten = (byte)Math.Floor((double)((year - th_int - hunder_int) / 10));  //十位
//                        uint yBcd_ten = bcd.octToBcd(yOct_ten);

//                        uint one_int = (uint)yOct_ten * 10;
//                        byte yOct_one = (byte)Math.Floor((double)(year - th_int - hunder_int - one_int));  //个位
//                        uint yBcd_one = bcd.octToBcd(yOct_one);

//                        uint yearH = (yBcd_th << 4) | yBcd_hdr;
//                        uint yearL = (yBcd_ten << 4) | yBcd_one;


//                        byte month = (byte)int.Parse(DateTime.Now.ToString("MM"));  //月份
//                        byte mOct_ten = (byte)Math.Floor((double)(month / 10));
//                        uint mBcd_ten = bcd.octToBcd(mOct_ten);
//                        byte mOct_one = (byte)(month - mOct_ten * 10);
//                        uint mBcd_one = bcd.octToBcd(mOct_one);
//                        uint bcd_month = (mBcd_ten << 4) | mBcd_one;



//                        byte day = (byte)int.Parse(DateTime.Now.ToString("dd"));
//                        byte dOct_ten = (byte)Math.Floor((double)(day / 10));
//                        uint dBcd_ten = bcd.octToBcd(dOct_ten);
//                        byte dOct_one = (byte)(day - dOct_ten * 10);
//                        uint dBcd_one = bcd.octToBcd(dOct_one);
//                        uint bcd_day = (dBcd_ten << 4) | dBcd_one;

//                        byte hour = (byte)int.Parse(DateTime.Now.ToString("HH"));
//                        byte hOct_ten = (byte)Math.Floor((double)(hour / 10));
//                        uint hBcd_ten = bcd.octToBcd(hOct_ten);
//                        byte hOct_one = (byte)(hour - hOct_ten * 10);
//                        uint hBcd_one = bcd.octToBcd(hOct_one);
//                        uint bcd_hour = (hBcd_ten << 4) | hBcd_one;

//                        byte minute = (byte)int.Parse(DateTime.Now.ToString("mm")); //分钟
//                        byte minOct_ten = (byte)Math.Floor((double)(minute / 10));
//                        uint minBcd_ten = bcd.octToBcd(minOct_ten);
//                        byte minOct_one = (byte)(minute - minOct_ten * 10);
//                        uint minBcd_one = bcd.octToBcd(minOct_one);
//                        uint bcd_minute = (minBcd_ten << 4) | minBcd_one;

//                        byte sec = (byte)int.Parse(DateTime.Now.ToString("ss"));
//                        byte sOct_ten = (byte)Math.Floor((double)(sec / 10));
//                        uint sBcd_ten = bcd.octToBcd(sOct_ten);
//                        byte sOct_one = (byte)(sec - sOct_ten * 10);
//                        uint sBcd_one = bcd.octToBcd(sOct_one);
//                        uint bcd_sec = (sBcd_ten << 4) | sBcd_one;

//                        data[9] = (byte)yearH;
//                        data[10] = (byte)yearL;
//                        data[11] = (byte)bcd_month;
//                        data[12] = (byte)bcd_day;
//                        data[13] = (byte)bcd_hour;
//                        data[14] = (byte)bcd_minute;
//                        data[15] = (byte)bcd_sec;

//                        TCP_Frame tcpFrame2 = new TCP_Frame();
//                        tcpFrame2.sealDataToFrame(ref data);
//                        tcpFrame2.sendTcpFrame(ref localSocket, ref this.tbxLog);
//                    }
//                    break;
//#if InsideVersion
//                case (byte)RECV_CTRL_CODE.GET_VST_ACK:
//                    {
//                        /*
//                        * 提取OBU_ID
//                        */
//                        byte[] obu_id = new byte[4];
//                        obu_id[0] = recvData[5];
//                        obu_id[1] = recvData[6];
//                        obu_id[2] = recvData[7];
//                        obu_id[3] = recvData[8];
//                        str1 = BitConverter.ToString(obu_id).Replace("-", " ");
//                        tbxLog.Text += "\r\n OBU ID:" + str1 + "\r\n";
//                        this.tbxOBU_ID.Text = str1;

//                        if (recvData.Length > 97) // 79 + 12 12位车牌号  4位是数据头 2位是crc
//                        {
//                            /*
//                         * 提取OBU车牌号
//                         */
//                            //obu车牌号码
//                            byte startPos = 85;
//                            byte[] carIdNameArr = new byte[2];
//                            string carIdName = "";
//                            carIdNameArr[0] = recvData[startPos];
//                            carIdNameArr[1] = recvData[startPos + 1];
//                            carIdName = Encoding.GetEncoding("gb2312").GetString(carIdNameArr); //车牌省份 共两个字节表示一个汉字

//                            string carIdNum = "";       //车牌后的数字和字母，ASCII表示
//                            for (byte i = 1; i <= 10; i++)
//                            {
//                                carIdNum += ((char)recvData[startPos + 1 + i]).ToString();
//                            }

//                            //显示车牌
//                            byte[] carNum = new byte[12];
//                            for (byte i = 0; i < 12; i++)
//                            {
//                                carNum[i] = recvData[startPos + i];
//                            }
//                            this.tbxOBU_CarNum.Text = carIdName + carIdNum;
//                            str1 = BitConverter.ToString(carNum).Replace("-", " ");
//                            this.tbxLog.Text += "OBU车牌：" + str1 + "\r\n";
//                        }

//                    }
//                    break;
//                case (byte)RECV_CTRL_CODE.DOWNLOAD_BINFILE_ACK:
//                    byte isRecieved = recvData[5];
//                    if (isRecieved == 0)
//                    {
//                        this.isBinFileReceived = true;
//                    }
//                    break;
//#endif
//                default:
//                    break;

//            }
//        }
        private void btnInit_Click(object sender, EventArgs e)
        {
            if (this.tbxChargePeriod.Text == "")
            {
                this.tbxChargePeriod.BackColor = Color.Red;
                MessageBox.Show("请填写自动扣费间隔。", "ERROR");
                return;
            }
            else
            {
                this.tbxChargePeriod.BackColor = Color.White;
            }

            chargeParaInit();
            byte[] data = new byte[8];
            data[0] = (byte)CTRL_CODE.INIT;


            ulong unixTime = 0;         //Unix时间，高位在前
            DateTime Epoch = new DateTime(1970, 1, 1);
            //unixTime = (ulong)((DateTime.UtcNow - Epoch).TotalSeconds - 28800); //GMT时间 = 北京时间 - 8H
            unixTime = (ulong)((DateTime.UtcNow - Epoch).TotalSeconds); //北京时间 
            data[1] = (byte)((unixTime & 0xff000000) >> 24);
            data[2] = (byte)((unixTime & 0x00ff0000) >> 16);
            data[3] = (byte)((unixTime & 0x0000ff00) >> 8);
            data[4] = (byte)((unixTime & 0x000000ff));

            data[5] = chargePara.power;
            data[6] = chargePara.channel;
            data[7] = (byte)(10 * this.chargePara.chargePeriod); //ucReserved

            TCP_Frame tcpFrame = new TCP_Frame();
            tcpFrame.sealDataToFrame(ref data);
            tcpFrame.sendTcpFrame(ref localSocket, ref this.tbxLog);



        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            this.tbxLog.Text = "";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.localSocket.Close();
        }

        private void btnOpenInstrument_Click(object sender, EventArgs e)
        {
            chargeParaInit();
            byte[] data = new byte[2];
            data[0] = (byte)CTRL_CODE.INSTRUMENT_ONOFF;
            if (this.RSUSwitch)
            {
                data[1] = (byte)RSUSWITCH.OPEN;
                this.btnOpenInstrument.Text = "关闭设备";
                this.btnOpenInstrument.BackColor = Color.LightGreen;
                this.btnGetVst.Enabled = false;
                this.btnChooseBin.Enabled = false;
            }
            else
            {
                data[1] = (byte)RSUSWITCH.CLOSE;
                this.btnOpenInstrument.Text = "打开设备";
                this.btnOpenInstrument.BackColor = Color.Transparent;
                this.btnGetVst.Enabled = true;
                this.btnChooseBin.Enabled = true;
            }
            this.RSUSwitch = !this.RSUSwitch;

            TCP_Frame tcpFrame = new TCP_Frame();
            tcpFrame.sealDataToFrame(ref data);
            tcpFrame.sendTcpFrame(ref localSocket, ref this.tbxLog);
        }

        private void btnManualCharge_Click(object sender, EventArgs e)
        {
            if (!isTransicationInfoSet())
                return;
            chargeParaInit();
            byte[] data = new byte[59];
            data[0] = (byte)CTRL_CODE.TRANSCATION_LAUNCHED;

            data[1] = (byte)((chargePara.OBU_ID & 0xff000000) >> 24);
            data[2] = (byte)((chargePara.OBU_ID & 0x00ff0000) >> 16);
            data[3] = (byte)((chargePara.OBU_ID & 0x0000ff00) >> 8);
            data[4] = (byte)((chargePara.OBU_ID & 0x000000ff));

            data[5] = (byte)((chargePara.bill & 0xff000000) >> 24);
            data[6] = (byte)((chargePara.bill & 0x00ff0000) >> 16);
            data[7] = (byte)((chargePara.bill & 0x0000ff00) >> 8);
            data[8] = (byte)((chargePara.bill & 0x000000ff));

            BCD_Code bcd = new BCD_Code();

            uint year = (uint)int.Parse(DateTime.Now.ToString("yyyy"));
            //uint year = 2319;

            byte yOct_th = (byte)Math.Floor((double)(year / 1000)); //千位
            uint yBcd_th = bcd.octToBcd(yOct_th);

            uint th_int = (uint)yOct_th * 1000;
            byte yOct_hdr = (byte)Math.Floor((double)((year - th_int) / 100));  //百位
            uint yBcd_hdr = bcd.octToBcd(yOct_hdr);

            uint hunder_int = (uint)yOct_hdr * 100;
            byte yOct_ten = (byte)Math.Floor((double)((year - th_int - hunder_int) / 10));  //十位
            uint yBcd_ten = bcd.octToBcd(yOct_ten);

            uint one_int = (uint)yOct_ten * 10;
            byte yOct_one = (byte)Math.Floor((double)(year - th_int - hunder_int - one_int));  //个位
            uint yBcd_one = bcd.octToBcd(yOct_one);

            uint yearH = (yBcd_th << 4) | yBcd_hdr;
            uint yearL = (yBcd_ten << 4) | yBcd_one;


            byte month = (byte)int.Parse(DateTime.Now.ToString("MM"));  //月份
            byte mOct_ten = (byte)Math.Floor((double)(month / 10));
            uint mBcd_ten = bcd.octToBcd(mOct_ten);
            byte mOct_one = (byte)(month - mOct_ten * 10);
            uint mBcd_one = bcd.octToBcd(mOct_one);
            uint bcd_month = (mBcd_ten << 4) | mBcd_one;



            byte day = (byte)int.Parse(DateTime.Now.ToString("dd"));
            byte dOct_ten = (byte)Math.Floor((double)(day / 10));
            uint dBcd_ten = bcd.octToBcd(dOct_ten);
            byte dOct_one = (byte)(day - dOct_ten * 10);
            uint dBcd_one = bcd.octToBcd(dOct_one);
            uint bcd_day = (dBcd_ten << 4) | dBcd_one;

            byte hour = (byte)int.Parse(DateTime.Now.ToString("HH"));
            byte hOct_ten = (byte)Math.Floor((double)(hour / 10));
            uint hBcd_ten = bcd.octToBcd(hOct_ten);
            byte hOct_one = (byte)(hour - hOct_ten * 10);
            uint hBcd_one = bcd.octToBcd(hOct_one);
            uint bcd_hour = (hBcd_ten << 4) | hBcd_one;

            byte minute = (byte)int.Parse(DateTime.Now.ToString("mm")); //分钟
            byte minOct_ten = (byte)Math.Floor((double)(minute / 10));
            uint minBcd_ten = bcd.octToBcd(minOct_ten);
            byte minOct_one = (byte)(minute - minOct_ten * 10);
            uint minBcd_one = bcd.octToBcd(minOct_one);
            uint bcd_minute = (minBcd_ten << 4) | minBcd_one;

            byte sec = (byte)int.Parse(DateTime.Now.ToString("ss"));
            byte sOct_ten = (byte)Math.Floor((double)(sec / 10));
            uint sBcd_ten = bcd.octToBcd(sOct_ten);
            byte sOct_one = (byte)(sec - sOct_ten * 10);
            uint sBcd_one = bcd.octToBcd(sOct_one);
            uint bcd_sec = (sBcd_ten << 4) | sBcd_one;

            //data[9] = (byte)((year & 0xff00) >> 8); //年份数字的高8位
            //data[10] = (byte)(year & 0x00ff);
            //data[11] = month;
            //data[12] = day;
            //data[13] = hour;
            //data[14] = minute;
            //data[15] = sec;

            data[9] = (byte)yearH;
            data[10] = (byte)yearL;
            data[11] = (byte)bcd_month;
            data[12] = (byte)bcd_day;
            data[13] = (byte)bcd_hour;
            data[14] = (byte)bcd_minute;
            data[15] = (byte)bcd_sec;

            TCP_Frame tcpFrame = new TCP_Frame();
            tcpFrame.sealDataToFrame(ref data);
            tcpFrame.sendTcpFrame(ref localSocket, ref this.tbxLog);

        }
        //交易过程中需要的文本框等是否填写完毕
        //C6 - B5 - C1
        private bool isTransicationInfoSet()
        {
            if (this.tbxBill.Text == "")
            {
                this.tbxBill.BackColor = Color.Red;
                MessageBox.Show("请填写金额。", "ERROR");
                return false;
            }
            else
            {
                this.tbxBill.BackColor = Color.White;
            }
            if (this.tbxOBU_ID.Text == "")
            {
                this.tbxOBU_ID.BackColor = Color.Red;
                MessageBox.Show("请填写OBU ID号。", "ERROR");
                return false;
            }
            else
            {
                this.tbxOBU_ID.BackColor = Color.White;
            }

            return true;
        }
        private void cbxAutoCharge_CheckedChanged(object sender, EventArgs e)
        {
            if (this.tbxChargePeriod.Text == "")
            {
                this.tbxChargePeriod.BackColor = Color.Red;
                MessageBox.Show("请填写自动扣费间隔。", "ERROR");
                return;
            }
            else
            {
                this.tbxChargePeriod.BackColor = Color.White;
            }
            if (!isTransicationInfoSet())
                return;
            if (cbxAutoCharge.Checked) //选中
            {
                chargeParaInit();
                this.timerAutoCharge.Interval = (int)(this.chargePara.chargePeriod * 1000);
                //计时器
                this.timerAutoCharge.Enabled = true;
                this.tbxChargePeriod.Enabled = false;
                //发送期间用到的数据
                this.tbxOBU_ID.Enabled = false;
                this.tbxBill.Enabled = false;

            }
            else
            {
                this.timerAutoCharge.Enabled = false;
                this.tbxChargePeriod.Enabled = true;

                //发送期间用到的数据
                this.tbxOBU_ID.Enabled = true;
                this.tbxBill.Enabled = true;
            }
        }

        private void timerAutoCharge_Tick(object sender, EventArgs e)
        {
            byte[] data = new byte[59];
            data[0] = (byte)CTRL_CODE.TRANSCATION_LAUNCHED;

            data[1] = (byte)((chargePara.OBU_ID & 0xff000000) >> 24);
            data[2] = (byte)((chargePara.OBU_ID & 0x00ff0000) >> 16);
            data[3] = (byte)((chargePara.OBU_ID & 0x0000ff00) >> 8);
            data[4] = (byte)((chargePara.OBU_ID & 0x00000000));

            data[5] = (byte)((chargePara.bill & 0xff000000) >> 24);
            data[6] = (byte)((chargePara.bill & 0x00ff0000) >> 16);
            data[7] = (byte)((chargePara.bill & 0x0000ff00) >> 8);
            data[8] = (byte)((chargePara.bill & 0x00000000));

            int year = int.Parse(DateTime.Now.ToString("yyyy"));
            byte month = (byte)int.Parse(DateTime.Now.ToString("MM"));  //月份
            byte day = (byte)int.Parse(DateTime.Now.ToString("dd"));
            byte hour = (byte)int.Parse(DateTime.Now.ToString("hh"));
            byte minute = (byte)int.Parse(DateTime.Now.ToString("mm")); //分钟
            byte sec = (byte)int.Parse(DateTime.Now.ToString("ss"));

            data[9] = (byte)((year & 0xff00) >> 8); //年份数字的高8位
            data[10] = (byte)(year & 0x00ff);
            data[11] = month;
            data[12] = day;
            data[13] = hour;
            data[14] = minute;
            data[15] = sec;

            TCP_Frame tcpFrame = new TCP_Frame();
            tcpFrame.sealDataToFrame(ref data);
            try
            {
                tcpFrame.sendTcpFrame(ref localSocket, ref this.tbxLog);
            }
            catch (Exception ex)
            {
                this.timerAutoCharge.Enabled = false;
                MessageBox.Show("通信出错，请检查TCP连接情况。\r\n" + ex.Message, "ERROR");
                return;
            }
        }

        private void tbxLog_TextChanged(object sender, EventArgs e)
        {
            this.tbxLog.SelectionStart = this.tbxLog.Text.Length;
            this.tbxLog.SelectionLength = 0;
            this.tbxLog.ScrollToCaret();
        }

        private void numericTbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键 
            {
                bool check_result_1 = (e.KeyChar >= '0') && (e.KeyChar <= '9');
                if (check_result_1)//这是允许输入0-9数字 
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                    MessageBox.Show("请勿输入非法字符。\r\n" +
                    "合法字符为 0~9,'\\b'(退格键)", "ERROR");
                }
            }
        }

        private void tbxChargePeriod_TextChanged(object sender, EventArgs e)
        {
            this.tbxChargePeriod.BackColor = Color.White;
            if (this.tbxChargePeriod.Text != "")
            {
                if (int.Parse(this.tbxChargePeriod.Text) > 25)
                {
                    MessageBox.Show("数值超出范围", "间隔不可超过25s");
                    this.tbxChargePeriod.Text = "";
                }
            }

        }

        private void tbxBill_TextChanged(object sender, EventArgs e)
        {
            this.tbxBill.BackColor = Color.White;
        }

        private void OBUIDtbx_TextChanged(object sender, EventArgs e)
        {
            this.tbxOBU_ID.BackColor = Color.White;
            if ((this.tbxOBU_ID.Text.Length + 1) % 3 == 0)
            {
                this.tbxOBU_ID.Text += ' ';//追加空格
                this.tbxOBU_ID.Select(this.tbxOBU_ID.Text.Length, 0);//选择文本末尾位置
                this.tbxOBU_ID.ScrollToCaret();//将光标滚动到末尾位置，不然追加一个空格光标会跳转到开头位置
            }
        }

        private void HexRichTbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键 
            {
                bool check_result_1 = (e.KeyChar >= '0') && (e.KeyChar <= '9');
                bool check_result_2 = (e.KeyChar >= 'a') && (e.KeyChar <= 'f');
                bool check_result_3 = (e.KeyChar >= 'A') && (e.KeyChar <= 'F');
                if (check_result_1 || check_result_2 || check_result_3)//这是允许输入0-9数字 
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                    MessageBox.Show("输入的不是十六进制字符。\r\n" +
                    "合法的十六进制字符为 0~9,abcdef,ABCDEF,'\\b'(退格键)", "ERROR");
                }
            }
            else
            {
                if (this.tbxOBU_ID.Text == "")
                {
                    e.Handled = true;
                }
                else if (this.tbxOBU_ID.Text.Substring(this.tbxOBU_ID.Text.Length - 1, 1) == " ")   //最后一个字符是空格时，把空格去掉，否则会在keypress事件里又补上一个空格
                {
                    this.tbxOBU_ID.TextChanged -= new System.EventHandler(this.OBUIDtbx_TextChanged);
                    this.tbxOBU_ID.Text = this.tbxOBU_ID.Text.Substring(0, this.tbxOBU_ID.Text.Length - 1);
                    this.tbxOBU_ID.Select(this.tbxOBU_ID.Text.Length, 0);//选择文本末尾位置
                    e.Handled = false;
                    this.tbxOBU_ID.TextChanged += new System.EventHandler(this.OBUIDtbx_TextChanged);
                }
                else
                {
                    e.Handled = false;
                }
            }


        }

        private void btnSaveLog_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileLog = new SaveFileDialog();
            fileLog.Filter = "文本文件|*.txt";
            string strDate = DateTime.Now.ToString("D");
            string strHour = DateTime.Now.ToString("HH");
            string strMin = DateTime.Now.ToString("mm");
            string strSec = DateTime.Now.ToString("ss");
            fileLog.FileName = "log_" + strDate + strHour + strMin + strSec;
            if (fileLog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter mySw = File.CreateText(fileLog.FileName);
                mySw.Write(this.tbxLog.Text);
                mySw.Flush();
                mySw.Close();
                //this.tbxLog.Text += "已保存日志";
                MessageBox.Show("已保存日志", "提示");
            }

        }

        private void btnGetVst_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[1];
            data[0] = (byte)CTRL_CODE.GET_VST;
            TCP_Frame tcpFrame = new TCP_Frame();
            tcpFrame.sealDataToFrame(ref data);
            tcpFrame.sendTcpFrame(ref localSocket, ref this.tbxLog);
        }

        private void btnChooseBin_Click(object sender, EventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "请选择固件";
            dialog.Filter = "固件(*.bin*)|*.bin";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                filename = dialog.FileName;
                if (string.IsNullOrEmpty(filename))
                {
                    return;
                }
                int index = filename.LastIndexOf("\\");
                string name = filename.Substring(index + 1);
                this.tbxDisplayFilename.Text = name;
                this.btnUpdataFile.Enabled = true;
            }
        }


        private void btnUpdataFile_Click(object sender, EventArgs e)
        {
            this.btnUpdataFile.Enabled = false;
            tcpNeededBtnSet(false);
            ThreadPool.QueueUserWorkItem(sendBinartyFile, null);
           
        }

        private void sendBinartyFile(Object state)
        {
            using (FileStream fstream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader binReader = new BinaryReader(fstream))
                {
                    uint packageNum = 0;
                    const int count = 1024;
                    int buffLength = 0;
                    byte[] buff;
                    long fileSize = fstream.Length;
                    long blocks = fileSize / count + 1;
                    long currentSize = 0;
                    buff = binReader.ReadBytes(count);
                    while ((buffLength = buff.Length) > 0)
                    {
                        currentSize += buffLength;
                        byte[] data = new byte[1 + 1 + 2 + 2 + buffLength];     // cmdtype 是否最后一个包 升级包序号 升级包长度 数据
                        data[0] = (byte)CTRL_CODE.DOWNLOAD_BINFILE;
                        if (currentSize == fileSize)
                        {
                            data[1] = 1;
                        }
                        else
                        {
                            data[1] = 0;
                        }
                        data[2] = (byte)(packageNum >> 8);
                        data[3] = (byte)(packageNum & 0xFF);
                        data[4] = (byte)(buffLength >> 8);
                        data[5] = (byte)(buffLength & 0xFF);
                        for (uint i = 0; i < buffLength; i++)
                        {
                            data[6 + i] = buff[i];
                        }

                        TCP_Frame tcpFrame = new TCP_Frame();
                        tcpFrame.sealDataToFrame(ref data);

                        int restartNum = 1;

                        while (restartNum <= 3)
                        {
                            this.BeginInvoke((EventHandler)(delegate
                            {
                                tcpFrame.sendTcpFrame(ref this.localSocket, ref this.tbxLog);
                            }));
                            if (!manualResetSendBin.Wait(TimeSpan.FromSeconds(1)))
                            {
                                ++restartNum;
                            }
                            else
                            {
                                break;
                            }

                        }
                        if (restartNum == 4)
                        {

                            this.BeginInvoke((EventHandler)(delegate
                            {
                                tcpNeededBtnSet(true);
                                this.btnUpdataFile.Enabled = true;
                                this.tbxLog.Text += "\r\n发送固件: 发送固件发生错误，重发三次仍然无法收到回复包，请检查通信设备并且重新选择固件文件\r\n";
                            }));
                            return;
                        }
                        ++packageNum;
                        buff = binReader.ReadBytes(count);
                        manualResetSendBin.Reset();
                    }
                    this.BeginInvoke((EventHandler)(delegate
                    {
                        tcpNeededBtnSet(true);
                        this.btnUpdataFile.Enabled = false;
                        this.tbxLog.Text += "\r\n发送固件: 发送固件完毕\r\n";
                        this.tbxLog.Text += "\r\n正在执行更新，请勿断电，等待控制板LED颜色停止循环即完成更新\r\n";
                        this.btnConnect.PerformClick();
                    }));
                }
            }
        }

        private void displayB5(ref byte[] recvData)
        {
            byte[] temp = new byte[7];

            for (int i = 4, j = 0; i < 11; ++i, ++j)
            {
                temp[j] = recvData[i];
            }

            string str = BitConverter.ToString(temp).Replace("-", " ");
            this.BeginInvoke((EventHandler)(delegate       
            {
                this.tbxB5Display.Text = str;
            }));
            
            if (recvData[10] == 0)
            {
                this.BeginInvoke((EventHandler)(delegate
                {
                    this.lbB5Success.Text = "成功";
                    this.lbB5Success.ForeColor = Color.Green;
                }));
            }
            else
            {
                this.BeginInvoke((EventHandler)(delegate
                {
                    this.lbB5Success.Text = "失败";
                    this.lbB5Success.ForeColor = Color.Red;
                }));
            }

        }
    }
}
