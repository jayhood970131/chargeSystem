using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//TCP
using System.Net.Sockets;
using System.Net;
namespace ChargeSystem
{
    public class TCP_Frame
    {
        private byte length_H = 0;
        private byte length_L = 0;

        private byte CRC_H = 0;
        private byte CRC_L = 0;

        private byte[] tcpFrame = null;

        public const byte CTRL_CODE_INDEX = 0x04;

        /*将数据封装为一帧*/
        public void sealDataToFrame(ref byte[] dataArr)
        {
            this.tcpFrame = new byte[2 + 2 + 2 + dataArr.Length]; //加上6个额外的字节
            this.tcpFrame[0] = 0xff;        //帧头
            this.tcpFrame[1] = 0xff;

            this.length_H = (byte)((dataArr.Length & 0xff00) >> 8);
            this.length_L = (byte)(dataArr.Length & 0x00ff);
            this.tcpFrame[2] = this.length_H;       //数据长度
            this.tcpFrame[3] = this.length_L;

            for(uint i =0;i<dataArr.Length;i++)     //数据
            {
                this.tcpFrame[i + 4] = dataArr[i];
            }

            byte[] crcArr = new byte[this.tcpFrame.Length - 2 - 2]; //crc校验包含数据和长度
            for(uint i = 2;i<=this.tcpFrame.Length -3;i++)  //复制出数据加长度部分，送去计算CRC
            {
                crcArr[i - 2] = this.tcpFrame[i];
            }

            CRC16 crc16 = new CRC16();
            UInt16 oldVal = crc16.X25Crc16(ref crcArr,crcArr.Length);  //注意length被强制从int转成byte了，长度过了256会出错。
            UInt16 newVal = crc16.x25(ref crcArr, crcArr.Length);
            Console.WriteLine("旧的值：" + oldVal + "\r新的值：" + newVal);
            this.tcpFrame[this.tcpFrame.Length - 1] = crc16.getCRC16_L();
            this.tcpFrame[this.tcpFrame.Length - 2] = crc16.getCRC16_H();
        }

        public void sendTcpFrame(ref Socket sck,ref TextBox tbxLog)
        {
            sck.Send(this.tcpFrame);

            byte[] temp = new byte[1];
            temp[0] = this.tcpFrame[TCP_Frame.CTRL_CODE_INDEX];

            string str1 = BitConverter.ToString(temp);
            string str2 = BitConverter.ToString(this.tcpFrame).Replace("-", " ");
            tbxLog.Text += "\r\n"+DateTime.Now.TimeOfDay.ToString()+":>>"+str1 +"帧,"+str2+"\r\n";

        }

        public void beginSendBinaryFile(ref Socket sck, ref TextBox rtbxLogs)
        {
            sck.BeginSend(this.tcpFrame, 0, this.tcpFrame.Length ,SocketFlags.None, ar =>
            {

            }, null);
        }

    }

    
}
