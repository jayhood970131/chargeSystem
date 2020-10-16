using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargeSystem
{
    public enum B5Frame
    {
        NOT_HAS = 0,
        INCORRECT,
        CORRECT
    }
    public class B4B5Statistics
    {
        public B5Frame b5Frame; // b5帧情况
        public string carNum;   // 车牌号

        public B4B5Statistics(B5Frame b5Frame, string carNum)
        {
            this.b5Frame = b5Frame;
            this.carNum = carNum;
        }
    }

}
