using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargeSystem
{
    public class BCD_Code
    {
        private byte octVal = 0;
        private byte bcdVal = 0;

        public byte octToBcd(byte octVal)
        {
            this.bcdVal = this.octVal = octVal;
            return octVal;
        }
    }


}
