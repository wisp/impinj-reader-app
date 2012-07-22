

/***
 *
 * Parts of this work are derived from sample code included in
 * https://developer.impinj.com/modules/PDdownloads/visit.php?cid=6&lid=45
 * and copyright 2007 by Impinj, Inc. That code is licensed under the Apache License, Version 2.0, and available at
 * http://www.apache.org/licenses/LICENSE-2.0
 ***/

/*
Copyright (c) 2009, University of Washington
Copyright (c) 2009, Intel Corporation
All rights reserved.
 
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following
conditions are met:
 
    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following
disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following
disclaimer in the documentation and/or other materials provided with the distribution.
    * Neither the name of the University of Washington nor Intel Corporation nor the names of its contributors may be
used to endorse or promote products derived from this software without specific prior written permission.
 
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

//
// MyTag WISP Tag Parsing Class
//
// EPC format definitions on wisp.wikispaces.com
// 
// General Format:
// [ 1 byte | tag type ] + [ 8 byte | data ] + [ 1 byte | WISP HW Version ] + [ 2 byte | HW Serial # ]
//

using System;
using System.Collections.Generic;
using System.Text;

namespace ReaderLibrary
{

    public enum TagType
    {
        UNKNOWN,
        WISP_STATIC_ID,
        WISP_ACCELEROMETER,
        WISP_TEMPERATURE,
        WISP_CAPACITANCE,
        WISP_DATA_LOGGER,
        WISP_SOC,
    }

    public class MyTag
    {
        string epcID = "";
        string time = "";
        int count;
        string firstSeen = "";
        string lastSeen = "";
        string accessResultData = "";
        string rssi = "";
        string frequency = "";

        static double quickAccelCorrectionX = 0.87;
        static double quickAccelCorrectionY = 0.886;
        static double quickAccelCorrectionZ = 1.034;

        int socVersion = 1; // version 1 or version 2.

        TagType tagType;

        public MyTag(string epcID, string time, string count, string firstSeen,
                     string lastSeen, string accessResultData, string rssi, string frequency)
        {
            this.epcID = epcID;
            this.time = time;
            this.count = Convert.ToInt32(count);
            this.firstSeen = firstSeen;
            this.lastSeen = lastSeen;
            this.accessResultData = accessResultData;
            this.tagType = EvalTagType();
            this.rssi = rssi;
            this.frequency = frequency;
        }



        #region ID and Type Parsing

        public string GetEpcID() { return epcID; }
        public string GetTime() { return time; }
        public int GetCount() { return count; }
        public string GetRSSI() { return rssi; }
        public string GetFrequency() { return frequency; }
        public TagType GetTagType() { return tagType; }
        public string GetAccessResultData() { return accessResultData; }

        public TagType EvalTagType()
        { 
            TagType type = TagType.UNKNOWN;
            string id = epcID.ToString().Substring(0, 2);
            switch (id)
            {
                case "0B":
                case "0D":
                    type = TagType.WISP_ACCELEROMETER;
                    break;
                case "0F":
                case "0E":
                    type = TagType.WISP_TEMPERATURE;
                    break;
                case "AA":
                    type = TagType.WISP_SOC;
                    break;
                default:
                    type = TagType.UNKNOWN;
                    break;
            }
            return type;
        }

        public string GetHardwareRev()
        {
            if (epcID.Length < 24) return "";
            return epcID.ToString().Substring(18, 2);
        }

        public int GetSerialNumber()
        {
            if (epcID.Length < 24) return 0;
            string data = epcID.ToString().Substring(20, 4);
            return Convert.ToInt32(data, 16); // convert hex to decimal
        }

        public int GetPacketCounter()
        {
            if (epcID.Length < 24) return 0;
            string data = epcID.ToString().Substring(14, 4);
            return Convert.ToInt32(data, 16); // convert hex to decimal
        }

        public string GetTagTypeName()
        {
            string type = "";
            string id = epcID.ToString().Substring(0, 2);
            switch (id)
            {
                case "0B":
                    type = "Quick Accel";
                    break;
                case "0D":
                    type = "Full Accel";
                    break;
                case "0F":
                    type = "Int Temp";
                    break;
                case "0E":
                    type = "Ext Temp";
                    break;
                case "AA":
                    type = "socWISP";
                    break;
                default:
                    type = "Unknown";
                    break;
            }
            return type;
        }

        #endregion

        #region Accelerometer Parsing
        

        // Get the raw ADC value
        public int GetRawAccel(string channel)
        {
            string data = "";
            switch (channel)
            {
                case "x":
                    data = epcID.Substring(6, 4);
                    break;
                case "y":
                    data = epcID.Substring(2, 4);
                    break;
                case "z":
                    data = epcID.Substring(10, 4);
                    break;
                default:
                    data = "0";
                    break;
            }
            return Convert.ToInt32(data, 16);
        }

        static public void SetAccelCorrection(double xcorr, double ycorr, double zcorr)
        {
            quickAccelCorrectionX = xcorr;
            quickAccelCorrectionY = ycorr;
            quickAccelCorrectionZ = zcorr;
        }

        static public double GetAccelCorrection(string channel)
        {
            channel = channel.ToLower();
            if (channel == "x")
                return quickAccelCorrectionX;
            else if (channel == "y")
                return quickAccelCorrectionY;
            else if (channel == "z")
                return quickAccelCorrectionZ;
            else
                return 0.0;
        }

        // Get the scaled, flipped value.
        public double GetAccel(string channel)
        {
            channel = channel.ToLower();

            double value = GetRawAccel(channel);

            // check data
            if (value < 0 || value > 1024) value = 0;

            // Scale 0 to 100 %
            value = 100.0 * value / 1024.0;

            // flip x,y by default
            if (channel == "x" || channel == "y") value = 100 - value;

            // WISP ID: "0D" and "0B" use different sampling methods
            // 0D lets the accel settle fully
            // 0B uses partial settling - the value is slighly higher than reported value.
            //    z channel of adxl330 has different resistance - that's why its scalar is different.
            string id = epcID.ToString().Substring(0, 2);
            if (id.ToLower() == "0b")
            {
                if (channel == "x") value = value * MyTag.quickAccelCorrectionX;
                if (channel == "y") value = value * MyTag.quickAccelCorrectionY;
                if (channel == "z") value = value * MyTag.quickAccelCorrectionZ;
            }

            return value;
        }

        #endregion Accelerometer Parsing

        #region Temperature Parsing
        public double GetTemperature()
        {
            string data = epcID.Substring(2, 4);
            double value = Convert.ToInt32(data, 16);
            // check data
            if (value < 0 || value > 1024) value = 0;
            return ((value - 673) * 423) / 1024;
        }
        public string GetTemperatureSensor()
        {
            return "Internal";
        }
        #endregion

        #region socWISP Parsing
        public int[] GetSOCData()
        {
            string data = GetAccessResultData();

            if (data.Length == 0)
                return new int[] { };

            if (data.Length < 4)
                return new int[] { };

            int[] socValues;

            if (socVersion == 2)
                socValues = HandleSOCV2(data);
            else
                socValues = HandleSOCV1(data);

            return socValues;
        }

        public void SetSOCVersion(int version)
        {
            socVersion = version;
        }

        public double socFilteredValue;
        public double socFilteredTemperature;

        // V1 has 8 bits starting at bit 3 == [2]
        public int[] HandleSOCV1(string data)
        {
            UInt16 thisvalue = Convert.ToUInt16(data, 16);
            thisvalue = (UInt16)(UInt16.MaxValue - thisvalue);

            string bits = Convert.ToString(thisvalue, 2).PadLeft(16, '0');

            bits = bits.Substring(3, 8);

            int socValue = Convert.ToInt32(bits, 2);

            return new int[] { socValue };
        }

        // V2 has two 8 bit samples from the fifo.  0x00 means fifo empty.
        private int[] HandleSOCV2(string data)
        {
            int[] values = new int[data.Length / 2];
            for (int idx = 0; idx < data.Length; idx += 2)
            {
                string str = data.Substring(idx, 2);
                if (str != "00" && str != "FF")
                {
                    UInt16 value = Convert.ToUInt16(str, 16);
                    // value = (UInt16)(255 - value);
                    string bits = Convert.ToString(value, 2).PadLeft(8, '0');
                    string flipbits = "";
                    for (int i = 0; i < 8; i++) flipbits = bits.Substring(i, 1) + flipbits;
                    int newvalue = Convert.ToInt32(bits, 2);
                    values[idx / 2] = newvalue;
                }
            }
            return values;
        }


        #endregion


        #region Attenuator Test Parsing
        /* not used.

        public struct AttenuatorTestValues
        {
            public int IDENTIFIERS;
            public int STATE_READY;
            public int STATE_REPLY;
            public int STATE_ACKGD;
            public int QUERY_PAKCT;
            public int ACK_PACKETS;
            public int TAR_OVRFLOW;
            public int SLEEP_START;
            public int DELNOTFOUND;
            public int QUERY_REPET;
            public int QUERY_ADJUT;
        }

        public AttenuatorTestValues GetAttenTestValues()
        {
            AttenuatorTestValues vars = new AttenuatorTestValues();
            vars.IDENTIFIERS = Convert.ToInt32(epcID.Substring(0, 2), 16);
            vars.STATE_READY = Convert.ToInt32(epcID.Substring(2, 2), 16);
            vars.STATE_REPLY = Convert.ToInt32(epcID.Substring(4, 2), 16);
            vars.STATE_ACKGD = Convert.ToInt32(epcID.Substring(6, 2), 16);
            vars.QUERY_PAKCT = Convert.ToInt32(epcID.Substring(8, 2), 16);
            vars.ACK_PACKETS = Convert.ToInt32(epcID.Substring(10, 2), 16);
            vars.TAR_OVRFLOW = Convert.ToInt32(epcID.Substring(12, 2), 16);
            vars.SLEEP_START = Convert.ToInt32(epcID.Substring(14, 2), 16);
            vars.DELNOTFOUND = Convert.ToInt32(epcID.Substring(16, 2), 16);
            vars.QUERY_REPET = Convert.ToInt32(epcID.Substring(18, 2), 16);
            vars.QUERY_ADJUT = Convert.ToInt32(epcID.Substring(20, 2), 16);
            return vars;
        }

        */
        #endregion
    }
}
