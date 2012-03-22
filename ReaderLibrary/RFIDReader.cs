
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
//using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Xml;

using LLRP;
using LLRP.DataType;

using ReaderLibrary;

namespace ReaderLibrary
{
    public class RFIDReader
    {

        // Create an instance of LLRP client:
        private LLRPClient client = new LLRPClient();

        // Store reader-capable modulation modes
        //private ReaderMode[] readerModes;

        // Configurations
        //protected ReaderConfig readerconfig = new ReaderConfig();
        //protected InventoryConfig inventoryconfig = new InventoryConfig();
        //protected ReadCmdSettings readcmdsettings = new ReadCmdSettings();

        //To accomodate all the all the messages.
        private MSG_ERROR_MESSAGE msg_err;



        //private bool connected = false;
        //private bool inventorymode = false;
        //private bool readmode = false;

        // Reader needs a reference to the gui so it can pass received tags up.
        private ReaderManager readerMgr;

        public RFIDReader(ReaderManager newMgr)
        {
            readerMgr = newMgr;
            //SetDefaultReaderConfig();
            //SetDefaultInventoryConfig();
        }




        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////


        public bool connectTo(string ipAddress)
        {
            ENUM_ConnectionAttemptStatusType status;
            bool ret = client.Open(ipAddress, 4000, out status);   // try to connect

            if (!ret || status != ENUM_ConnectionAttemptStatusType.Success)
            {
                return false;
            }
            else
            {   // we have a connection 

                return true;
            }
        }

        public void Initialize()
        {
            //subscribe to client event notification and ro access report
            client.OnReaderEventNotification += new delegateReaderEventNotification(reader_OnReaderEventNotification);
            client.OnRoAccessReportReceived += new delegateRoAccessReport(reader_OnRoAccessReportReceived);
        }

        public void CleanSubscriptionClient()
        {
            //clean up subscriptions.
            client.OnReaderEventNotification -= new delegateReaderEventNotification(reader_OnReaderEventNotification);
            client.OnRoAccessReportReceived -= new delegateRoAccessReport(reader_OnRoAccessReportReceived);

            client.Close();
        }

        #region Parsing and Handling

        //Thread myThread = new Thread(new ThreadStart(this.UpdateROReport));

        public void reader_OnRoAccessReportReceived(MSG_RO_ACCESS_REPORT msg)
        {
            delegateRoAccessReport del = UpdateROReport;
            del.BeginInvoke(msg, OnAsyncCallBack, null);
        }

        
        void OnAsyncCallBack(IAsyncResult asyncResult)   
        {
            
        }

        public void reader_OnReaderEventNotification(MSG_READER_EVENT_NOTIFICATION msg)
        {
            //delegateReaderEventNotification del = UpdateReaderEvent;
            delegateReaderEventNotification del = UpdateReaderEvent;
            del.BeginInvoke(msg, OnAsyncCallBack, null);
        }


        public void UpdateROReport(MSG_RO_ACCESS_REPORT msg) 
        {
            if (msg.TagReportData == null)
            {
                readerMgr.HandleTagReceived(null);
                return;
            }

            string result = null;

            try
            {
                for (int i = 0; i < msg.TagReportData.Length; i++)
                {
                    result = ParseTag(msg, result, i);
                    //MessageBox.Show(result);
                }
            }
            catch (Exception ex)
            {
                WriteString(ex.Message);
            }
        }



        // str = string to parse for relevant information
        // target = if several arguements of string, specify which is relevant string number (min: 1)
        // example: <access><access>1001<\access><\access><access><access>1002<\access><\access>
        //          want: 1002 -----> target: 2
        private string getRelevantInfoFromXML(string str, int target)
        {
            try
            {
                string[] temp = str.Split(new string[] { "><" }, StringSplitOptions.None);
                temp = temp[target].Split(new string[] { ">" }, StringSplitOptions.None);
                temp = temp[1].Split(new string[] { "<" }, StringSplitOptions.None);
                str = temp[0];
            }
            catch (Exception ex)
            {
                WriteString("UpdateReaderEvent " + ex.ToString());
                str = null;
            }

            return str;
        }

        private string ParseTag(MSG_RO_ACCESS_REPORT msg, string result, int i)
        {
            if (msg.TagReportData[i].EPCParameter.Count > 0)
            {
                string epcInfo = "";
                Type t = msg.TagReportData[i].EPCParameter[0].GetType();
                if (t.Name == "PARAM_EPC_96")
                    epcInfo = ((PARAM_EPC_96)(msg.TagReportData[i].EPCParameter[0])).EPC.ToHexString();
                else if (t.Name == "PARAM_EPCData")
                    epcInfo = ((PARAM_EPCData)(msg.TagReportData[i].EPCParameter[0])).EPC.ToHexString();
                else
                    throw new Exception("ParseTag method couldn't resolve EPC type");
 
                string time = (msg.TagReportData[i].LastSeenTimestampUTC).ToString();
                string count = (msg.TagReportData[i].TagSeenCount).ToString();
                string accessSpecID = (msg.TagReportData[i].AccessSpecID).ToString();
                string firstSeen = (msg.TagReportData[i].FirstSeenTimestampUTC).ToString();
                string lastSeen = (msg.TagReportData[i].LastSeenTimestampUTC).ToString();
                string rssi = (msg.TagReportData[i].PeakRSSI).ToString();
                string freq = (msg.TagReportData[i].ChannelIndex).ToString();

                result = null;
                //Tag newTag = BuildTag(epcInfo);

                // extract information
                count = getRelevantInfoFromXML(count, 1);
                //accessSpecID = getRelevantInfoFromXML(accessSpecID, 1);
                time = getRelevantInfoFromXML(time, 1);
                rssi = getRelevantInfoFromXML(rssi, 1);
                freq = getRelevantInfoFromXML(freq, 1);
                
                string accessResultData = "";
                if (accessSpecID != "0")
                {
                    for (int j = 0; j < msg.TagReportData[i].AccessCommandOpSpecResult.Length; j++)
                    {
                        result = msg.TagReportData[i].AccessCommandOpSpecResult[0].ToString();
                        // MessageBox.Show(result); - evil - just keeps popping up!
                        // extract access result
                        if (result != null && result.Contains("Success"))
                        {
                            Type t2 = msg.TagReportData[i].AccessCommandOpSpecResult[0].GetType();
                            if (t2.Name == "PARAM_C1G2ReadOpSpecResult")
                            {
                                PARAM_C1G2ReadOpSpecResult accessResult;
                                accessResult = ((PARAM_C1G2ReadOpSpecResult)(msg.TagReportData[i].AccessCommandOpSpecResult[0]));
                                string marker = "<ReadData>";
                                int start = result.IndexOf(marker);
                                int end = result.IndexOf("<", start + 1);
                                accessResultData = result.Substring(start + marker.Length, end - marker.Length - start);
                            }
                        }
                    }
                }

                // Pass all this information to the GUI in a MyTag object.
                MyTag newTag = new MyTag(epcInfo, time, count, firstSeen, lastSeen, accessResultData, rssi, freq);
                readerMgr.HandleTagReceived(newTag);

            }
            return result;
        }




        private void UpdateReaderEvent(MSG_READER_EVENT_NOTIFICATION msg)
        {
            try
            {
                WriteMessage(msg.ToString());
            }
            catch (Exception e)
            {
                WriteMessage("UpdateReaderEvent " + e.Message);
            }
        }

        #endregion



        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////




        #region RO Spec
        /// <summary>
        /// Communicates the information of a ROSpec to the Reader.
        /// </summary>
        public void Add_RoSpec(ReaderManager.InventoryConfig inventoryconfig, ReaderManager.ReaderConfig readerconfig)
        {
            // Create a new message to be sent to the client
            MSG_ADD_ROSPEC msg = new MSG_ADD_ROSPEC();

            msg.ROSpec = new PARAM_ROSpec();
            msg.ROSpec.CurrentState = ENUM_ROSpecState.Disabled;   // Reader's current state: Disable 
            msg.ROSpec.Priority = 0x00;     // specifies the priority of the rospect                           
            msg.ROSpec.ROSpecID = 123;

            //==============================
            // Start condition
            msg.ROSpec.ROBoundarySpec = new PARAM_ROBoundarySpec();
            msg.ROSpec.ROBoundarySpec.ROSpecStartTrigger = new PARAM_ROSpecStartTrigger();
            // Null – No start trigger. The only way to start the ROSpec is with a START_ROSPEC from the Client.
            // 1 Immediate
            // 2 Periodic
            // 3 GPI
            // Note: This ROSpect starts immediatelly
            msg.ROSpec.ROBoundarySpec.ROSpecStartTrigger.ROSpecStartTriggerType = inventoryconfig.startTrigger;


            // Stop condition
            msg.ROSpec.ROBoundarySpec.ROSpecStopTrigger = new PARAM_ROSpecStopTrigger();
            // 0 Null – Stop when all AISpecs are done, or when preempted, or with a STOP_ROSPEC from the Client.
            // 1 Duration
            // 2 GPI with a timeout value
            // DurationTriggerValue: Duration in milliseconds

            // Trigger 1
            msg.ROSpec.ROBoundarySpec.ROSpecStopTrigger.ROSpecStopTriggerType = inventoryconfig.stopTrigger;

            //msg.ROSpec.ROBoundarySpec.ROSpecStopTrigger.DurationTriggerValue = 1000;


            // ROReportSpec triger 
            // 0 None
            // 1 (Upon N TagReportData Parameters or End of AISpec) Or (End of RFSurveySpec) - N=0 is unlimited.
            // 2 Upon N TagReportData Parameters or End of ROSpec N=0 is unlimited.

            // N: Unsigned Short Integer. This is the number of TagReportData Parameters used in ROReportTrigger = 1 and 2.
            // If N = 0, there is no limit on the number of TagReportData Parameters. 
            // This field SHALL be ignored when ROReportTrigger = 0.
            msg.ROSpec.ROReportSpec = new PARAM_ROReportSpec();
            msg.ROSpec.ROReportSpec.ROReportTrigger = inventoryconfig.reportTrigger;
            msg.ROSpec.ROReportSpec.N = inventoryconfig.reportN;


            // Report 2
            //msg.ROSpec.ROReportSpec.ROReportTrigger = ENUM_ROReportTriggerType.Upon_N_Tags_Or_End_Of_ROSpec;
            //msg.ROSpec.ROReportSpec.N = 0; 

            msg.ROSpec.ROReportSpec.TagReportContentSelector = new PARAM_TagReportContentSelector();
            msg.ROSpec.ROReportSpec.TagReportContentSelector.EnableAccessSpecID = true;
            msg.ROSpec.ROReportSpec.TagReportContentSelector.EnableAntennaID = true;
            msg.ROSpec.ROReportSpec.TagReportContentSelector.EnableChannelIndex = true;
            msg.ROSpec.ROReportSpec.TagReportContentSelector.EnableFirstSeenTimestamp = true;
            msg.ROSpec.ROReportSpec.TagReportContentSelector.EnableInventoryParameterSpecID = true;
            msg.ROSpec.ROReportSpec.TagReportContentSelector.EnableLastSeenTimestamp = true;
            msg.ROSpec.ROReportSpec.TagReportContentSelector.EnablePeakRSSI = true;
            msg.ROSpec.ROReportSpec.TagReportContentSelector.EnableROSpecID = true;
            msg.ROSpec.ROReportSpec.TagReportContentSelector.EnableSpecIndex = true;
            msg.ROSpec.ROReportSpec.TagReportContentSelector.EnableTagSeenCount = true;

            msg.ROSpec.SpecParameter = new UNION_SpecParameter();

            // Antena inventory operation
            PARAM_AISpec aiSpec = new PARAM_AISpec();

            // 12345

            aiSpec.AntennaIDs = new UInt16Array();
            //aiSpec.AntennaIDs.Add(0);       //0 :  applys to all antennae,
            for (ushort i = 1; i < readerconfig.antennaID.Length + 1; i++)
                if (readerconfig.antennaID[i - 1])
                {
                    aiSpec.AntennaIDs.Add(i);
                }
            // Stop trigger parameter 
            // 0 Null – Stop when ROSpec is done.
            // 1 Duration
            // 2 GPI with a timeout value
            // 3 Tag observation

            //if (inventorymode)
            //{
                // TriggerType: Integer
                // Possible Values: Value Modulation
                // ------ ------------
                // 0 Upon seeing N tag observations, or timeout
                // 1 Upon seeing no more new tag observations for t ms,or timeout
                // 2 N attempts to see all tags in the FOV, or timeout

                if (inventoryconfig.AITriggerType == ENUM_AISpecStopTriggerType.Tag_Observation)
                {
                    // Antena inventory operation
                    aiSpec.AISpecStopTrigger = new PARAM_AISpecStopTrigger();
                    aiSpec.AISpecStopTrigger.AISpecStopTriggerType = ENUM_AISpecStopTriggerType.Tag_Observation;
                    aiSpec.AISpecStopTrigger.TagObservationTrigger = new PARAM_TagObservationTrigger();

                    if (inventoryconfig.numAttempts == 0)
                    {
                        // Trigger type 1: works
                        aiSpec.AISpecStopTrigger.TagObservationTrigger.TriggerType = ENUM_TagObservationTriggerType.Upon_Seeing_N_Tags_Or_Timeout;
                        aiSpec.AISpecStopTrigger.TagObservationTrigger.NumberOfTags = inventoryconfig.numTags;
                        aiSpec.AISpecStopTrigger.TagObservationTrigger.Timeout = inventoryconfig.AITimeout;   // There is no time out
                    }
                    else
                    {
                        // Trigger type 2
                        aiSpec.AISpecStopTrigger.TagObservationTrigger.TriggerType = ENUM_TagObservationTriggerType.N_Attempts_To_See_All_Tags_In_FOV_Or_Timeout;
                        aiSpec.AISpecStopTrigger.TagObservationTrigger.NumberOfAttempts = inventoryconfig.numAttempts;
                        aiSpec.AISpecStopTrigger.TagObservationTrigger.Timeout = inventoryconfig.AITimeout;   // There is no time out
                    }

                }
                else if (inventoryconfig.AITriggerType == ENUM_AISpecStopTriggerType.Duration)
                {
                    // Antena inventory operation
                    aiSpec.AISpecStopTrigger = new PARAM_AISpecStopTrigger();
                    aiSpec.AISpecStopTrigger.AISpecStopTriggerType = ENUM_AISpecStopTriggerType.Duration;
                    aiSpec.AISpecStopTrigger.DurationTrigger = inventoryconfig.duration;
                }

                else if (inventoryconfig.AITriggerType == ENUM_AISpecStopTriggerType.GPI_With_Timeout)
                {
                    aiSpec.AISpecStopTrigger = new PARAM_AISpecStopTrigger();
                    aiSpec.AISpecStopTrigger.AISpecStopTriggerType = ENUM_AISpecStopTriggerType.GPI_With_Timeout;
                    aiSpec.AISpecStopTrigger.GPITriggerValue.Timeout = inventoryconfig.AITimeout;
                }

            //}


            // Operational parameters for an inventory using a single air protocol.
            aiSpec.InventoryParameterSpec = new PARAM_InventoryParameterSpec[1];
            aiSpec.InventoryParameterSpec[0] = new PARAM_InventoryParameterSpec();
            aiSpec.InventoryParameterSpec[0].InventoryParameterSpecID = 1234;
            aiSpec.InventoryParameterSpec[0].ProtocolID = ENUM_AirProtocols.EPCGlobalClass1Gen2;

            msg.ROSpec.SpecParameter.Add(aiSpec);   // Add operational parameters to Add_ROSpec msg

            // Send message to client and get the response

            MSG_ADD_ROSPEC_RESPONSE rsp = client.ADD_ROSPEC(msg, out msg_err, 3000);


            if (rsp != null)
            {
                //textBox2.Text = rsp.ToString() + "\n";
                WriteMessage(rsp.ToString(), "Add_RoSpec");
                WriteMessage("Add_RoSpec \n");
            }
            else if (msg_err != null)
            {
                WriteMessage("Add_RoSpec " + msg_err.ToString() + "\n");
            }
            else
                WriteMessage("Add_RoSpec Command time out!" + "\n");

        }




        /// <summary>
        /// Deletes the ROSpec at the Reader corresponding to ROSpecID passed in this message.
        /// </summary>
        public void Delete_RoSpec()
        {
            MSG_DELETE_ROSPEC msg = new MSG_DELETE_ROSPEC();
            msg.ROSpecID = 0;   // Deletes ROSpec for the given id

            MSG_DELETE_ROSPEC_RESPONSE rsp = client.DELETE_ROSPEC(msg, out msg_err, 3000);

            if (rsp != null)
            {
                //textBox2.Text = rsp.ToString();
                WriteMessage(rsp.ToString(), "Delete_RoSpec");
            }
            else if (msg_err != null)
            {
                WriteMessage("Delete_RoSpec " + msg_err.ToString());
            }
            else
                WriteMessage("Delete_RoSpec Command time out!");
        }



        /// <summary>
        /// This message is issued by the Client to the Reader. Upon receiving the message, 
        /// the Reader moves the ROSpec corresponding to the ROSpecID passed in this message 
        /// from the disabled to the inactive state.
        /// </summary>
        public void Enable_RoSpec()
        {
            MSG_ENABLE_ROSPEC msg = new MSG_ENABLE_ROSPEC();
            msg.ROSpecID = 123;

            MSG_ENABLE_ROSPEC_RESPONSE rsp = client.ENABLE_ROSPEC(msg, out msg_err, 3000);

            if (rsp != null)
            {
                //textBox2.Text = rsp.ToString();
                WriteMessage(rsp.ToString(), "Enable_RoSpec");
            }
            else if (msg_err != null)
            {
                WriteMessage("Enable_RoSpec " + msg_err.ToString());
            }
            else
                WriteMessage("Enable_RoSpec Command time out!");

        }

        /// <summary>
        /// Upon receiving the message, the Reader starts the ROSpec corresponding to ROSpecID passed 
        /// in this message, if the ROSpec is in the enabled state.
        /// </summary>
        private void Start_RoSpec()
        {
            MSG_START_ROSPEC msg = new MSG_START_ROSPEC();
            msg.ROSpecID = 123;     // the id of the ROSpec to be executed
            MSG_START_ROSPEC_RESPONSE rsp = client.START_ROSPEC(msg, out msg_err, 3000);
            if (rsp != null)
            {
                WriteMessage(rsp.ToString(), "Start_RoSpec");
            }
            else if (msg_err != null)
            {
                WriteMessage("Start_RoSpec " + msg_err.ToString());
            }
            else
                WriteMessage("Start_RoSpec Command time out!");

        }

        /// <summary>
        /// If the Reader was currently executing the ROSpec corresponding to the ROSpecID, 
        /// and the Reader was able to stop executing that ROSpec, then the success code is returned. 
        /// </summary>
        public void Stop_RoSpec()
        {
            MSG_STOP_ROSPEC msg = new MSG_STOP_ROSPEC();
            msg.ROSpecID = 123;

            MSG_STOP_ROSPEC_RESPONSE rsp = client.STOP_ROSPEC(msg, out msg_err, 3000);

            if (rsp != null)
            {
                //textBox2.Text = rsp.ToString();
                WriteMessage(rsp.ToString(), "Stop_RoSpec");
            }
            else if (msg_err != null)
            {
                WriteMessage("Stop_RoSpec " + msg_err.ToString());
            }
            else
                WriteMessage("Stop_RoSpec Command time out!");
        }

        /// <summary>
        /// Requests to the Reader to retrieve all the ROSpecs that have been configured at the Reader.
        /// </summary>
        private void Get_RoSpec()
        {
            MSG_GET_ROSPECS msg = new MSG_GET_ROSPECS();

            MSG_GET_ROSPECS_RESPONSE rsp = client.GET_ROSPECS(msg, out msg_err, 3000);


            if (rsp != null)
            {
                //textBox2.Text = rsp.ToString() + "\n";
                WriteMessage(rsp.ToString(), "Get_RoSpec");
            }
            else if (msg_err != null)
            {
                WriteMessage("Get_RoSpec " + msg_err.ToString() + "\n");
            }
            else
                WriteMessage("Get_RoSpec Command time out!" + "\n");

        }

        #endregion




        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////



        #region Access Spec

        public void AddReadAccessSpec(string tagID, string maskBits, ushort wordCount, ushort wordPtr, ReaderManager.ReaderConfig readerconfig)
        {
            MSG_ADD_ACCESSSPEC msg = GetAccessSpecMsg(readerconfig);
            AddTagSpec(tagID, maskBits, msg);
            AddReadAccessCommand(wordCount, wordPtr, msg);

            msg.AccessSpec.AccessReportSpec = new PARAM_AccessReportSpec();
            msg.AccessSpec.AccessReportSpec.AccessReportTrigger = ENUM_AccessReportTriggerType.Whenever_ROReport_Is_Generated;

            SendAccessSpec(msg);
        }

        private void AddWriteAccessSpec(string tagID, string maskBits, string writeData, ReaderManager.ReaderConfig readerconfig)
        {
            MSG_ADD_ACCESSSPEC msg = GetAccessSpecMsg(readerconfig);
            AddTagSpec(tagID, maskBits, msg);
            AddWriteAccessCommand(msg, writeData);

            msg.AccessSpec.AccessReportSpec = new PARAM_AccessReportSpec();
            msg.AccessSpec.AccessReportSpec.AccessReportTrigger = ENUM_AccessReportTriggerType.Whenever_ROReport_Is_Generated;

            SendAccessSpec(msg);
        }

        private static void AddReadAccessCommand(ushort wordCount, ushort wordPtr, MSG_ADD_ACCESSSPEC msg)
        {
            PARAM_C1G2Read rd = new PARAM_C1G2Read();
            rd.AccessPassword = 0;
            rd.MB = new TwoBits(2);
            rd.OpSpecID = 111;
            rd.WordCount = wordCount;
            rd.WordPointer = wordPtr;
            msg.AccessSpec.AccessCommand.AccessCommandOpSpec.Add(rd);
        }

        private static void AddWriteAccessCommand(MSG_ADD_ACCESSSPEC msg, string data)
        {
            //define access spec
            msg.AccessSpec.AccessCommand.AccessCommandOpSpec = new UNION_AccessCommandOpSpec();

            PARAM_C1G2Write wr = new PARAM_C1G2Write();
            wr.AccessPassword = 0;
            wr.MB = new TwoBits(1);
            wr.OpSpecID = 111;
            wr.WordPointer = 2;
            //Data to be written. ex: "EEEE11112222333344445555"
            wr.WriteData = UInt16Array.FromString(data);

            msg.AccessSpec.AccessCommand.AccessCommandOpSpec.Add(wr);
        }

        private void SendAccessSpec(MSG_ADD_ACCESSSPEC msg)
        {
            MSG_ADD_ACCESSSPEC_RESPONSE rsp = client.ADD_ACCESSSPEC(msg, out msg_err, 3000);
            if (rsp != null)
            {
                //textBox2.Text = rsp.ToString() + "\n";
                WriteMessage(rsp.ToString(), "Add_AccessSpec");
            }
            else if (msg_err != null)
            {
                readerMgr.AppendToDebugTextBox("Add_AccessSpec " + msg_err.ToString() + "\n");
            }
            else
                readerMgr.AppendToDebugTextBox("Add_AccessSpec Command time out!" + "\n");
        }

        private static void AddTagSpec(string tagID, string maskBits, MSG_ADD_ACCESSSPEC msg)
        {
            //define access command

            //define air protocol spec
            msg.AccessSpec.AccessCommand = new PARAM_AccessCommand();
            msg.AccessSpec.AccessCommand.AirProtocolTagSpec = new UNION_AirProtocolTagSpec();

            PARAM_C1G2TagSpec tagSpec = new PARAM_C1G2TagSpec();
            tagSpec.C1G2TargetTag = new PARAM_C1G2TargetTag[1];
            tagSpec.C1G2TargetTag[0] = new PARAM_C1G2TargetTag();
            tagSpec.C1G2TargetTag[0].Match = true; //change to "true" if you want to the following parameters take effect.
            tagSpec.C1G2TargetTag[0].MB = new TwoBits(1);
            tagSpec.C1G2TargetTag[0].Pointer = 0x20;
            tagSpec.C1G2TargetTag[0].TagData = LLRPBitArray.FromString(tagID);
            tagSpec.C1G2TargetTag[0].TagMask = LLRPBitArray.FromBinString(maskBits);

            msg.AccessSpec.AccessCommand.AirProtocolTagSpec.Add(tagSpec);
        }

        private MSG_ADD_ACCESSSPEC GetAccessSpecMsg(ReaderManager.ReaderConfig readerconfig)
        {
            MSG_ADD_ACCESSSPEC msg = new MSG_ADD_ACCESSSPEC();
            msg.AccessSpec = new PARAM_AccessSpec();

            msg.AccessSpec.AccessSpecID = 1001;
            msg.AccessSpec.AntennaID = 0;        //0 :  applys to all antennae,
            msg.AccessSpec.ProtocolID = ENUM_AirProtocols.EPCGlobalClass1Gen2;
            msg.AccessSpec.CurrentState = ENUM_AccessSpecState.Disabled;
            msg.AccessSpec.ROSpecID = 123;

            //define trigger
            msg.AccessSpec.AccessSpecStopTrigger = new PARAM_AccessSpecStopTrigger();
            msg.AccessSpec.AccessSpecStopTrigger.AccessSpecStopTrigger = ENUM_AccessSpecStopTriggerType.Null;
            msg.AccessSpec.AccessSpecStopTrigger.OperationCountValue = 3;

            return msg;
        }

        public void DELETE_ACCESSSPEC()
        {
            MSG_DELETE_ACCESSSPEC msg = new MSG_DELETE_ACCESSSPEC();
            msg.AccessSpecID = 1001;

            MSG_DELETE_ACCESSSPEC_RESPONSE rsp = client.DELETE_ACCESSSPEC(msg, out msg_err, 3000);
            if (rsp != null)
            {
                //textBox2.Text = rsp.ToString() + "\n";
                WriteMessage(rsp.ToString(), "Delete_AccessSpec");
            }
            else if (msg_err != null)
            {
                readerMgr.AppendToDebugTextBox("Delete_AccessSpec " + msg_err.ToString() + "\n");
            }
            else
                readerMgr.AppendToDebugTextBox("Delete_AccessSpec " + "Command time out!" + "\n");

        }

        public void ENABLE_ACCESSSPEC()
        {
            MSG_ENABLE_ACCESSSPEC msg = new MSG_ENABLE_ACCESSSPEC();
            msg.AccessSpecID = 1001;

            MSG_ENABLE_ACCESSSPEC_RESPONSE rsp = client.ENABLE_ACCESSSPEC(msg, out msg_err, 3000);

            if (rsp != null)
            {
                //textBox2.Text = rsp.ToString() + "\n";
                WriteMessage(rsp.ToString(), "ENABLE_ACCESSSPEC");
            }
            else if (msg_err != null)
            {
                readerMgr.AppendToDebugTextBox("ENABLE_ACCESSSPEC " + msg_err.ToString() + "\n");
            }
            else
                readerMgr.AppendToDebugTextBox("ENABLE_ACCESSSPEC Command time out!" + "\n");
        }

        private void DISABLE_ACCESSPEC()
        {
            MSG_DISABLE_ACCESSSPEC msg = new MSG_DISABLE_ACCESSSPEC();
            msg.AccessSpecID = 1001;

            MSG_DISABLE_ACCESSSPEC_RESPONSE rsp = client.DISABLE_ACCESSSPEC(msg, out msg_err, 3000);
            if (rsp != null)
            {
                //textBox2.Text = rsp.ToString() + "\n";
                WriteMessage(rsp.ToString(), "DISABLE_ACCESSPEC");
            }
            else if (msg_err != null)
            {
                readerMgr.AppendToDebugTextBox("DISABLE_ACCESSPEC " + msg_err.ToString() + "\n");
            }
            else
                readerMgr.AppendToDebugTextBox("DISABLE_ACCESSPEC Command time out!" + "\n");
        }

        private void GET_ACCESSSPEC()
        {
            MSG_GET_ACCESSSPECS msg = new MSG_GET_ACCESSSPECS();
            MSG_GET_ACCESSSPECS_RESPONSE rsp = client.GET_ACCESSSPECS(msg, out msg_err, 3000);

            if (rsp != null)
            {
                //textBox2.Text = rsp.ToString() + "\n";
                WriteMessage(rsp.ToString(), "GET_ACCESSSPEC");
            }
            else if (msg_err != null)
            {
                readerMgr.AppendToDebugTextBox("GET_ACCESSSPEC " + msg_err.ToString() + "\n");
            }
            else
                readerMgr.AppendToDebugTextBox("GET_ACCESSSPEC Command time out!" + "\n");
        }


        #endregion




        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////



        #region Reader Config

        public void Get_Reader_Config()
        {
            MSG_GET_READER_CONFIG msg = new MSG_GET_READER_CONFIG();
            msg.AntennaID = 1;
            msg.GPIPortNum = 0;
            MSG_GET_READER_CONFIG_RESPONSE rsp = client.GET_READER_CONFIG(msg, out msg_err, 3000);

            if (rsp != null)
            {
                //textBox2.Text = rsp.ToString() + "\n";
                WriteMessage(rsp.ToString(), "Get_Reader_Config");
            }
            else if (msg_err != null)
            {
                WriteMessage("Get_Reader_Config " + msg_err.ToString() + "\n");
            }
            else
                WriteMessage("Get_Reader_Config Command time out!" + "\n");

        }
        
        // Sets the client's configuration 
        public void Set_Reader_Config(ReaderManager.ReaderConfig readerconfig)
        {
            // find number of antennas to set
            byte numAntennaToSet = 0;
            ushort antennaSet = 0;
            for (int i = 0; i < readerconfig.antennaID.Length; i++)
                if (readerconfig.antennaID[i]) 
                {
                    antennaSet = (ushort)i;
                    numAntennaToSet++;
                }
            MSG_SET_READER_CONFIG msg = new MSG_SET_READER_CONFIG();
            msg.AccessReportSpec = new PARAM_AccessReportSpec();
            msg.AccessReportSpec.AccessReportTrigger = ENUM_AccessReportTriggerType.End_Of_AccessSpec;

            PARAM_C1G2InventoryCommand cmd = new PARAM_C1G2InventoryCommand();
            cmd.C1G2RFControl = new PARAM_C1G2RFControl();
            //cmd.C1G2RFControl.ModeIndex = 1000;
            cmd.C1G2RFControl.ModeIndex = readerconfig.modeIndex;
            cmd.C1G2RFControl.Tari = 0;
            cmd.C1G2SingulationControl = new PARAM_C1G2SingulationControl();
            cmd.C1G2SingulationControl.Session = new TwoBits(0);
            cmd.C1G2SingulationControl.TagPopulation = readerconfig.tagPopulation;
            cmd.C1G2SingulationControl.TagTransitTime = readerconfig.tagTransitTime;
            cmd.TagInventoryStateAware = false;

            msg.AntennaConfiguration = new PARAM_AntennaConfiguration[numAntennaToSet];
            for (ushort i = 1; i < numAntennaToSet + 1; i++)
            {
                msg.AntennaConfiguration[i - 1] = new PARAM_AntennaConfiguration();
                msg.AntennaConfiguration[i - 1].AirProtocolInventoryCommandSettings = new UNION_AirProtocolInventoryCommandSettings();
                msg.AntennaConfiguration[i - 1].AirProtocolInventoryCommandSettings.Add(cmd);

                msg.AntennaConfiguration[i - 1].AntennaID = antennaSet;

                msg.AntennaConfiguration[i - 1].RFReceiver = new PARAM_RFReceiver();
                msg.AntennaConfiguration[i - 1].RFReceiver.ReceiverSensitivity = readerconfig.readerSensitivity;

                msg.AntennaConfiguration[i - 1].RFTransmitter = new PARAM_RFTransmitter();
                msg.AntennaConfiguration[i - 1].RFTransmitter.ChannelIndex = readerconfig.channelIndex;
                msg.AntennaConfiguration[i - 1].RFTransmitter.HopTableID = readerconfig.hopTableIndex;
                msg.AntennaConfiguration[i - 1].RFTransmitter.TransmitPower = (ushort)(61 - (readerconfig.attenuation * 4));
            }
            //msg.AntennaProperties = new PARAM_AntennaProperties[1];
            //msg.AntennaProperties[0] = new PARAM_AntennaProperties();
            //msg.AntennaProperties[0].AntennaConnected = true;
            //msg.AntennaProperties[0].AntennaGain = 0;
            //msg.AntennaProperties[0].AntennaID = 1;

            msg.EventsAndReports = new PARAM_EventsAndReports();
            msg.EventsAndReports.HoldEventsAndReportsUponReconnect = false;

            msg.KeepaliveSpec = new PARAM_KeepaliveSpec();
            msg.KeepaliveSpec.KeepaliveTriggerType = ENUM_KeepaliveTriggerType.Null;
            msg.KeepaliveSpec.PeriodicTriggerValue = readerconfig.periodicTriggerValue;

            msg.ReaderEventNotificationSpec = new PARAM_ReaderEventNotificationSpec();
            msg.ReaderEventNotificationSpec.EventNotificationState = new PARAM_EventNotificationState[5];
            msg.ReaderEventNotificationSpec.EventNotificationState[0] = new PARAM_EventNotificationState();
            msg.ReaderEventNotificationSpec.EventNotificationState[0].EventType = ENUM_NotificationEventType.AISpec_Event;
            msg.ReaderEventNotificationSpec.EventNotificationState[0].NotificationState = false;

            msg.ReaderEventNotificationSpec.EventNotificationState[1] = new PARAM_EventNotificationState();
            msg.ReaderEventNotificationSpec.EventNotificationState[1].EventType = ENUM_NotificationEventType.Antenna_Event;
            msg.ReaderEventNotificationSpec.EventNotificationState[1].NotificationState = true;

            msg.ReaderEventNotificationSpec.EventNotificationState[2] = new PARAM_EventNotificationState();
            msg.ReaderEventNotificationSpec.EventNotificationState[2].EventType = ENUM_NotificationEventType.GPI_Event;
            msg.ReaderEventNotificationSpec.EventNotificationState[2].NotificationState = false;

            msg.ReaderEventNotificationSpec.EventNotificationState[3] = new PARAM_EventNotificationState();
            msg.ReaderEventNotificationSpec.EventNotificationState[3].EventType = ENUM_NotificationEventType.Reader_Exception_Event;
            msg.ReaderEventNotificationSpec.EventNotificationState[3].NotificationState = true;

            msg.ReaderEventNotificationSpec.EventNotificationState[4] = new PARAM_EventNotificationState();
            msg.ReaderEventNotificationSpec.EventNotificationState[4].EventType = ENUM_NotificationEventType.RFSurvey_Event;
            msg.ReaderEventNotificationSpec.EventNotificationState[4].NotificationState = true;

            msg.ROReportSpec = new PARAM_ROReportSpec();
            msg.ROReportSpec.N = 1;
            msg.ROReportSpec.ROReportTrigger = ENUM_ROReportTriggerType.Upon_N_Tags_Or_End_Of_AISpec;
            msg.ROReportSpec.TagReportContentSelector = new PARAM_TagReportContentSelector();
            msg.ROReportSpec.TagReportContentSelector.AirProtocolEPCMemorySelector = new UNION_AirProtocolEPCMemorySelector();
            PARAM_C1G2EPCMemorySelector c1g2mem = new PARAM_C1G2EPCMemorySelector();
            c1g2mem.EnableCRC = false;
            c1g2mem.EnablePCBits = false;
            msg.ROReportSpec.TagReportContentSelector.AirProtocolEPCMemorySelector.Add(c1g2mem);

            msg.ROReportSpec.TagReportContentSelector.EnableAccessSpecID = false;
            msg.ROReportSpec.TagReportContentSelector.EnableAntennaID = true;
            msg.ROReportSpec.TagReportContentSelector.EnableChannelIndex = false;
            msg.ROReportSpec.TagReportContentSelector.EnableFirstSeenTimestamp = true;
            msg.ROReportSpec.TagReportContentSelector.EnableInventoryParameterSpecID = false;
            msg.ROReportSpec.TagReportContentSelector.EnableLastSeenTimestamp = false;
            msg.ROReportSpec.TagReportContentSelector.EnablePeakRSSI = true;
            msg.ROReportSpec.TagReportContentSelector.EnableROSpecID = false;
            msg.ROReportSpec.TagReportContentSelector.EnableSpecIndex = false;
            msg.ROReportSpec.TagReportContentSelector.EnableTagSeenCount = true;

            msg.ResetToFactoryDefault = false;

            MSG_SET_READER_CONFIG_RESPONSE rsp = client.SET_READER_CONFIG(msg, out msg_err, 3000);

            if (rsp != null)
            {
                //textBox2.Text = rsp.ToString() + "\n";
                WriteMessage(rsp.ToString(), "setReaderConfig");
            }
            else if (msg_err != null)
            {
                WriteMessage("setReaderConfig " + rsp.ToString() + "\n");
            }
            else
                WriteMessage("setReaderConfig Commmand time out!" + "\n");

        }

        public ReaderManager.ReaderMode[] Get_Reader_Capability()
        {
            ReaderManager.ReaderMode[] readerModes = null;

            MSG_GET_READER_CAPABILITIES msg = new MSG_GET_READER_CAPABILITIES();
            msg.RequestedData = ENUM_GetReaderCapabilitiesRequestedData.Regulatory_Capabilities;
            MSG_GET_READER_CAPABILITIES_RESPONSE rsp = client.GET_READER_CAPABILITIES(msg, out msg_err, 3000);

            if (rsp != null)
            {
                LLRP.UNION_AirProtocolUHFRFModeTable modulationModes;
                modulationModes = rsp.RegulatoryCapabilities.UHFBandCapabilities.AirProtocolUHFRFModeTable;
                modulationModes[0].ToString();

                PARAM_C1G2UHFRFModeTable modeTable;
                try
                {
                    modeTable = (PARAM_C1G2UHFRFModeTable)modulationModes[0];
                    readerModes = new ReaderManager.ReaderMode[modeTable.Length];
                    for (int idx = 0; idx < modeTable.C1G2UHFRFModeTableEntry.Length; idx++)
                    {
                        PARAM_C1G2UHFRFModeTableEntry mode = modeTable.C1G2UHFRFModeTableEntry[idx];
                        readerModes[idx] = new ReaderManager.ReaderMode(mode);
                    }
                }catch{};

                // gui.AppendToDebugTextBox(rsp.ToString() + "\n");
                // writeMessage(rsp.ToString(), "Get_Reader_Capability");
            }
            else if (msg_err != null)
            {
                WriteMessage("Get_Reader_Capability " + msg_err.ToString() + "\n");
            }
            else
                WriteMessage("Get_Reader_Capability Command time out!" + "\n");

            return readerModes;
        }

        #endregion



        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        private void WriteMessage(string rsp)
        {
            WriteMessage(rsp, null);
        }
        private void WriteMessage(string rsp, string source)
        {
            readerMgr.WriteMessage(rsp, source);
        }

        public void WriteString(string info)
        {
            readerMgr.WriteString(info);
        }
    }
}
