
/*
***************************************************************************
*  Copyright 2007 Impinj, Inc.
*
*  Licensed under the Apache License, Version 2.0 (the "License");
*  you may not use this file except in compliance with the License.
*  You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
*  Unless required by applicable law or agreed to in writing, software
*  distributed under the License is distributed on an "AS IS" BASIS,
*  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
*  See the License for the specific language governing permissions and
*  limitations under the License.
*
***************************************************************************
*/

/*
***************************************************************************
* File Name:       LLRPEndPoint.cs
*
* Version:         1.0
* Author:          Impinj
* Organization:    Impinj
* Date:            Jan. 18, 2008
*
* Description:     This file contains implementation of LLRP Endpoint. LLRP
*                  Endpoint is used to build simulator or LLRP based
*                  application
***************************************************************************
*/


using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Remoting;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Data;

using LLRP.DataType;

namespace LLRP
{

    public class RAW_Message
    {
        public short version;
        public short msg_type;
        public int msg_id;
        public byte[] msg_body;

        public RAW_Message(short ver, short type, int id, byte[] data)
        {
            version = ver;
            msg_id = id;
            msg_type = type;

            msg_body = new byte[data.Length];
            Array.Copy(data, msg_body, data.Length);
        }
    }

    //add comments
    public class LLRPEndPoint : IDisposable
    {
        #region members
        private CommunicationInterface cI;
        private int LLRP1_TCP_PORT = 5084;
        private ManualResetEvent transactEvt;

        private short version;
        private short msg_type;
        private int msg_id;
        private byte[] data;

        private bool b_enqueue = false;
        private Queue<RAW_Message> raw_message_queue;



        public event delegateClientConnected OnClientConnected;
        public event delegateMessageReceived OnMessageReceived;

        #endregion

        #region Methods
        public LLRPEndPoint()
        {
        }
        public bool Create(string llrp_reader_name, bool server)
        {
            try
            {
                if (server)
                {
                    cI = new TCPIPServer();
                    cI.Open("", LLRP1_TCP_PORT);
                }
                else
                {
                    cI = new TCPIPClient();
                    cI.Open(llrp_reader_name, LLRP1_TCP_PORT);
                }

                cI.OnMessageReceived += new delegateMessageReceived(cI_OnMessageReceived);
                cI.OnClientConnected += new delegateClientConnected(cI_OnClientConnected);

            }
            catch
            {
                return false;
            }

            return true;
        }

        void cI_OnClientConnected()
        {
            if (OnClientConnected != null) OnClientConnected();
        }

        private void triggerMessageReceived(short ver, short msg_type, int msg_id, byte[] msg_data)
        {
            if (OnMessageReceived != null) OnMessageReceived(ver, msg_type, msg_id, msg_data);
        }

        void cI_OnMessageReceived(short ver, short msg_type, int msg_id, byte[] msg_data)
        {
            if ((msg_type == 100) || (msg_type == this.msg_type && msg_id == this.msg_id))
            {
                Array.Copy(msg_data, this.data, msg_data.Length);
                this.msg_type = msg_type;
                this.msg_id = msg_id;
                this.version = ver;
                transactEvt.Set();
            }

            if (OnMessageReceived != null)
            {
                delegateMessageReceived dmr = new delegateMessageReceived(triggerMessageReceived);
                dmr.BeginInvoke(ver, msg_type, msg_id, msg_data, null, null);
            }

            lock (this)
            {
                if (b_enqueue)
                {
                    raw_message_queue.Enqueue(new RAW_Message(ver, msg_type, msg_id, msg_data));
                    b_enqueue = false;
                }
            }

        }
        public void Close()
        {
            cI.Close();
        }

        public void Dispose()
        {
            this.Close();
        }

        #endregion

        public void SendMessage(Message msg)
        {
            bool[] bit_array = msg.ToBitArray();
            byte[] data = Util.ConvertBitArrayToByteArray(bit_array);

            lock (this)
            {
                b_enqueue = true;
            }

            try
            {
                Transaction.Send(cI, data);
            }
            catch
            {
                throw new Exception("Transaction Failed");
            }
        }

        public RAW_Message GetMessage()
        {
            lock (this)
            {
                return raw_message_queue.Dequeue();
            }
        }

        public Message TransactMessage(Message msg, int time_out)
        {
            bool[] bit_array = msg.ToBitArray();
            byte[] data = Util.ConvertBitArrayToByteArray(bit_array);
            try
            {
                transactEvt = new ManualResetEvent(false);
                Transaction.Send(cI, data);

                msg_id = (int)msg.MSG_ID;

                if (msg.MSG_TYPE != (uint)ENUM_LLRP_MSG_TYPE.CUSTOM_MESSAGE) msg_type = (short)(msg.MSG_TYPE + 10);
                else
                    msg_type = (short)msg.MSG_TYPE;

                if (transactEvt.WaitOne(time_out, false))
                {
                    BitArray bArr;
                    int length;
                    int cursor = 0;
                    switch ((ENUM_LLRP_MSG_TYPE)msg_type)
                    {

                        case ENUM_LLRP_MSG_TYPE.GET_READER_CAPABILITIES_RESPONSE:
                            {
                                bArr = Util.ConvertByteArrayToBitArray(data);
                                length = bArr.Count;
                                MSG_GET_READER_CAPABILITIES_RESPONSE r_msg = MSG_GET_READER_CAPABILITIES_RESPONSE.FromBitArray(ref bArr, ref cursor, length);
                                return r_msg;
                            }

                        case ENUM_LLRP_MSG_TYPE.ADD_ROSPEC_RESPONSE:
                            {
                                bArr = Util.ConvertByteArrayToBitArray(data);
                                length = bArr.Count;
                                MSG_ADD_ROSPEC_RESPONSE r_msg = MSG_ADD_ROSPEC_RESPONSE.FromBitArray(ref bArr, ref cursor, length);
                                return r_msg;
                            }

                        case ENUM_LLRP_MSG_TYPE.DELETE_ROSPEC_RESPONSE:
                            {
                                bArr = Util.ConvertByteArrayToBitArray(data);
                                length = bArr.Count;
                                MSG_DELETE_ROSPEC_RESPONSE r_msg = MSG_DELETE_ROSPEC_RESPONSE.FromBitArray(ref bArr, ref cursor, length);
                                return r_msg;
                            }

                        case ENUM_LLRP_MSG_TYPE.START_ROSPEC_RESPONSE:
                            {
                                bArr = Util.ConvertByteArrayToBitArray(data);
                                length = bArr.Count;
                                MSG_START_ROSPEC_RESPONSE r_msg = MSG_START_ROSPEC_RESPONSE.FromBitArray(ref bArr, ref cursor, length);
                                return r_msg;
                            }

                        case ENUM_LLRP_MSG_TYPE.STOP_ROSPEC_RESPONSE:
                            {
                                bArr = Util.ConvertByteArrayToBitArray(data);
                                length = bArr.Count;
                                MSG_STOP_ROSPEC_RESPONSE r_msg = MSG_STOP_ROSPEC_RESPONSE.FromBitArray(ref bArr, ref cursor, length);
                                return r_msg;
                            }

                        case ENUM_LLRP_MSG_TYPE.ENABLE_ROSPEC_RESPONSE:
                            {
                                bArr = Util.ConvertByteArrayToBitArray(data);
                                length = bArr.Count;
                                MSG_ENABLE_ROSPEC_RESPONSE r_msg = MSG_ENABLE_ROSPEC_RESPONSE.FromBitArray(ref bArr, ref cursor, length);
                                return r_msg;
                            }

                        case ENUM_LLRP_MSG_TYPE.DISABLE_ROSPEC_RESPONSE:
                            {
                                bArr = Util.ConvertByteArrayToBitArray(data);
                                length = bArr.Count;
                                MSG_DISABLE_ROSPEC_RESPONSE r_msg = MSG_DISABLE_ROSPEC_RESPONSE.FromBitArray(ref bArr, ref cursor, length);
                                return r_msg;
                            }

                        case ENUM_LLRP_MSG_TYPE.GET_ROSPECS_RESPONSE:
                            {
                                bArr = Util.ConvertByteArrayToBitArray(data);
                                length = bArr.Count;
                                MSG_GET_ROSPECS_RESPONSE r_msg = MSG_GET_ROSPECS_RESPONSE.FromBitArray(ref bArr, ref cursor, length);
                                return r_msg;
                            }

                        case ENUM_LLRP_MSG_TYPE.ADD_ACCESSSPEC_RESPONSE:
                            {
                                bArr = Util.ConvertByteArrayToBitArray(data);
                                length = bArr.Count;
                                MSG_ADD_ACCESSSPEC_RESPONSE r_msg = MSG_ADD_ACCESSSPEC_RESPONSE.FromBitArray(ref bArr, ref cursor, length);
                                return r_msg;
                            }

                        case ENUM_LLRP_MSG_TYPE.DELETE_ACCESSSPEC_RESPONSE:
                            {
                                bArr = Util.ConvertByteArrayToBitArray(data);
                                length = bArr.Count;
                                MSG_DELETE_ACCESSSPEC_RESPONSE r_msg = MSG_DELETE_ACCESSSPEC_RESPONSE.FromBitArray(ref bArr, ref cursor, length);
                                return r_msg;
                            }

                        case ENUM_LLRP_MSG_TYPE.ENABLE_ACCESSSPEC_RESPONSE:
                            {
                                bArr = Util.ConvertByteArrayToBitArray(data);
                                length = bArr.Count;
                                MSG_ENABLE_ACCESSSPEC_RESPONSE r_msg = MSG_ENABLE_ACCESSSPEC_RESPONSE.FromBitArray(ref bArr, ref cursor, length);
                                return r_msg;
                            }

                        case ENUM_LLRP_MSG_TYPE.DISABLE_ACCESSSPEC_RESPONSE:
                            {
                                bArr = Util.ConvertByteArrayToBitArray(data);
                                length = bArr.Count;
                                MSG_DISABLE_ACCESSSPEC_RESPONSE r_msg = MSG_DISABLE_ACCESSSPEC_RESPONSE.FromBitArray(ref bArr, ref cursor, length);
                                return r_msg;
                            }

                        case ENUM_LLRP_MSG_TYPE.GET_ACCESSSPECS_RESPONSE:
                            {
                                bArr = Util.ConvertByteArrayToBitArray(data);
                                length = bArr.Count;
                                MSG_GET_ACCESSSPECS_RESPONSE r_msg = MSG_GET_ACCESSSPECS_RESPONSE.FromBitArray(ref bArr, ref cursor, length);
                                return r_msg;
                            }

                        case ENUM_LLRP_MSG_TYPE.GET_READER_CONFIG_RESPONSE:
                            {
                                bArr = Util.ConvertByteArrayToBitArray(data);
                                length = bArr.Count;
                                MSG_GET_READER_CONFIG_RESPONSE r_msg = MSG_GET_READER_CONFIG_RESPONSE.FromBitArray(ref bArr, ref cursor, length);
                                return r_msg;
                            }

                        case ENUM_LLRP_MSG_TYPE.SET_READER_CONFIG_RESPONSE:
                            {
                                bArr = Util.ConvertByteArrayToBitArray(data);
                                length = bArr.Count;
                                MSG_SET_READER_CONFIG_RESPONSE r_msg = MSG_SET_READER_CONFIG_RESPONSE.FromBitArray(ref bArr, ref cursor, length);
                                return r_msg;
                            }

                        case ENUM_LLRP_MSG_TYPE.CLOSE_CONNECTION_RESPONSE:
                            {
                                bArr = Util.ConvertByteArrayToBitArray(data);
                                length = bArr.Count;
                                MSG_CLOSE_CONNECTION_RESPONSE r_msg = MSG_CLOSE_CONNECTION_RESPONSE.FromBitArray(ref bArr, ref cursor, length);
                                return r_msg;
                            }

                        default:
                            return null;
                    }
                }
                else
                {
                    return null;
                }

            }
            catch
            {
                throw new Exception("Transaction Failed");
            }
        }

    }
}
