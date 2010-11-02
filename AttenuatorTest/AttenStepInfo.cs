using System;
using System.Collections.Generic;
using System.Text;

namespace AttenuatorTest
{
    public class AttenStepInfo
    {

        public double attenuation = 0;
        public double tagRate = 0;
        public double tagCount = 0;

        //In milliseconds
        public int attenRunTime = 30000;
        public int attenSettleTime = 10000;



        public void setDefaultAttenConfig()
        {
            attenRunTime = 30000;
            attenSettleTime = 10000;
        }
    }
}
