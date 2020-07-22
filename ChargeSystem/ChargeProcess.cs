using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargeSystem
{
    public class ChargeProcess
    {
        public string ipAddress = "";
        public string port = "";
        public byte channel = 0;  //1~2
        public byte power = 0; //1~36
        public uint bill = 0;

        public uint OBU_ID = 0;
        public byte chargePeriod = 0;

        public uint Card_ID = 0;
        public uint Card_VehicleNum = 0;


    }
}
