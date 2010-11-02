/*
 ***************************************************************************
 *  Copyright 2008 Impinj, Inc.
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
 * File Name:       TCPIPConnection.cs
 * 
 * Author:          Impinj
 * Organization:    Impinj
 * Date:            18 Jan, 2008
 * 
 * Description:     This file contains implementation of TCPIP communication
 *                  classes including client and server
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
    /// <summary>
    /// TCPIPClient, used for building LLRPClient
    /// </summary>
    class TCPIPClient : CommunicationInterface
    {
        private const int BUFFER_SIZE = 1024;
        
        private TcpClient tcp_client;
        private NetworkStream ns;
        private bool new_message = true;
        
        private Int16 msg_ver;
        private Int16 msg_type;        
        private Int32 msg_len = 0;
        private Int32 msg_id;
        private byte[] msg_data;
        private Int32 msg_cursor = 0;

        private bool trying_to_close = false;

        private object syn_msg = new object();

        /// <summary>
        /// Message received event.
        /// </summary>

        public TCPIPClient()
        {
            state = new AsynReadState(BUFFER_SIZE);
        }

        /// <summary>
        /// Open network connection
        /// </summary>
        /// <param name="device_name">Device name or IP address</param>
        /// <param name="port">TCP port</param>
        /// <returns>true if opened succefully, otherwise false</returns>
        public override bool Open(string device_name, int port)
        {
            try
            {
                //Estabilish connection, get network stream for reading and writing
                tcp_client = new TcpClient(device_name, port);
                ns = tcp_client.GetStream();

                if (ns == null) return false;

                trying_to_close = false;
            }
            catch 
            {
                return false;
            }

            ns.Flush();

            //Start asyn-read
            ns.BeginRead(state.data, 0, BUFFER_SIZE, new AsyncCallback(OnDataRead), state);

            return true;
        }
        /// <summary>
        /// Asyn read result process
        /// </summary>
        /// <param name="ar"></param>
        private void OnDataRead(IAsyncResult ar)
        {
            int offset = 0;                     //used to keep the start position of a LLRP message in 
                                                //byte array returned from the read
            AsynReadState ss = (AsynReadState)ar.AsyncState;    //used to keep data

            lock (syn_msg)
            {
                try
                {
                REPEAT:
                    if (new_message)                //new_message is a flag to indicate if the data is part of unfinished message
                    {
                        msg_cursor = 0;
                        int reserved_date_len = ss.data.Length - offset;

                        if (reserved_date_len > 10)
                        {

                            //Calculate message type, version, length and id
                            int header = (ss.data[offset] << 8) + ss.data[offset + 1];
                            try
                            {
                                msg_type = (Int16)(header & 0x03FF);
                                msg_ver = (Int16)((header >> 10) & 0x07);
                                msg_len = (ss.data[offset + 2] << 24) + (ss.data[offset + 3] << 16) + (ss.data[offset + 4] << 8) + ss.data[offset + 5];
                                msg_id = (ss.data[offset + 6] << 24) + (ss.data[offset + 7] << 16) + (ss.data[offset + 8] << 8) + ss.data[offset + 9];
                            }
                            catch
                            {
                                msg_len = 0;
                            }

                            if (msg_len > 20000)
                            {
                                int i = 0;
                            }

                            //if data length larger than needed data for a complete message, 
                            //copy data into existing message and triggered message event 
                            if (msg_len > 0 && msg_ver == 1)
                            {

                                msg_data = new byte[msg_len];
                                //if message length greater than the calcualted message length. copy message and trigger message event

                                if (ss.data.Length >= (offset + msg_len))
                                {
                                    Array.Copy(ss.data, offset, msg_data, 0, msg_len);
                                    delegateMessageReceived msgRecv = new delegateMessageReceived(TriggerMessageEvent);
                                    msgRecv.BeginInvoke(msg_ver, msg_type, msg_id, msg_data, null, null);

                                    offset += msg_len;

                                    new_message = true;

                                    goto REPEAT;
                                }
                                else//If the received data is shorter than the message length, keep reading for the next data 
                                {
                                    new_message = false;

                                    Array.Copy(ss.data, offset, msg_data, 0, ss.data.Length - offset);
                                    msg_cursor = ss.data.Length - offset;

                                }
                            }
                        }
                        else
                        {
                            new_message = true;

                            //if ns !=null, do next asyn-read, to ensure that read
                            if (ns != null && ns.CanRead)
                            {
                                try
                                {
                                    ns.Flush();
                                    state = new AsynReadState(BUFFER_SIZE);

                                    Array.Copy(ss.data, offset, state.data, 0, reserved_date_len);

                                    if (!trying_to_close) ns.BeginRead(state.data, reserved_date_len, BUFFER_SIZE-reserved_date_len, new AsyncCallback(OnDataRead), state);
                                }
                                catch { }
                            }

                            return;
                        }
                    }
                    else
                    {
                        //if data length larger than needed data for a complete message, 
                        //copy data into existing message and triggered message event 
                        if (ss.data.Length >= msg_len - msg_cursor)
                        {
                            Array.Copy(ss.data, 0, msg_data, msg_cursor, msg_len - msg_cursor);
                            delegateMessageReceived msgRecv = new delegateMessageReceived(TriggerMessageEvent);
                            msgRecv.BeginInvoke(msg_ver, msg_type, msg_id, msg_data, null, null);

                            offset += msg_len - msg_cursor;

                            new_message = true;

                            goto REPEAT;
                        }
                        else //keep reading
                        {
                            new_message = false;

                            Array.Copy(ss.data, 0, msg_data, msg_cursor, ss.data.Length);
                            msg_cursor += ss.data.Length;
                        }
                    }

                    //if ns !=null, do next asyn-read, to ensure that read
                    if (ns != null && ns.CanRead)
                    {
                        try
                        {
                            ns.Flush();
                            state = new AsynReadState(BUFFER_SIZE);

                            if (!trying_to_close) ns.BeginRead(state.data, 0, BUFFER_SIZE, new AsyncCallback(OnDataRead), state);
                        }
                        catch { }
                    }
                }
                catch
                {
                }
            }
        }
        
        /// <summary>
        /// Release network resource
        /// </summary>
        public override void Close()
        {
            trying_to_close = true;

            if (ns!=null)ns.Close();
            if (tcp_client != null)tcp_client.Close();
        }

        /// <summary>
        /// Send data to device
        /// </summary>
        /// <param name="data">data to be sent. byte array</param>
        /// <returns></returns>
        public override int Send(byte[] data)
        {
            if (ns == null) return 0;

            try
            {
                ns.Flush();
                lock (syn_msg)
                {
                    new_message = true;
                }
                ns.BeginWrite(data, 0, data.Length, null, null);
                return data.Length;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Received data
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public override int Receive(out byte[] buffer)
        {
            try
            {
                ns.ReadTimeout = 200;
               
                byte[] buf = new byte[8096];
                int readSize = ns.Read(buf, 0, 8096);

                buffer = new byte[readSize];

                Array.Copy(buf, 0, buffer, 0, readSize);

                return readSize;
            }
            catch
            {
                buffer = null;
                return -1;
            }
        }

        public override void Dispose()
        {
            this.Close();
        }
    }

    /// <summary>
    /// TCPIPServer. Used for building LLRPServer, for example: LLRP reader simulator
    /// </summary>
    class TCPIPServer : CommunicationInterface
    {
        private const Int32 BUFFER_SIZE = 1024;
        
        private TcpListener server;
        private NetworkStream ns;
        private bool new_message = true;
        private Int16 msg_ver;
        private Int16 msg_type;
        private Int32 msg_len = 0;
        private Int32 msg_id;
        private byte[] msg_data;
        private Int32 msg_cursor = 0;

        public TCPIPServer()
        {
            state = new AsynReadState(BUFFER_SIZE);
        }

        //Asyn-call back
        private void DoAcceptTCPClientCallBack(IAsyncResult ar)
        {
            try
            {
                TcpListener listener = (TcpListener)ar.AsyncState;     
                TcpClient client = listener.EndAcceptTcpClient(ar);
    
                ns = client.GetStream();
                delegateClientConnected clientConn = new delegateClientConnected(TriggerOnClientConnect);
                clientConn.BeginInvoke(null, null);

                ns.BeginRead(state.data, 0, BUFFER_SIZE, new AsyncCallback(OnDataRead), state);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Close connection
        /// </summary>
        public override void Close()
        {
            if(ns!=null)ns.Close();
            if (server != null) server.Stop();
        }

        /// <summary>
        /// Open socket to accept connection
        /// </summary>
        /// <param name="device_name">ignored</param>
        /// <param name="port">TCP port</param>
        /// <returns></returns>
        public override bool Open(string device_name, int port)
        {
            try
            {
                IPAddress ipAddr = new IPAddress(new byte[] { 127, 0, 0, 1 });
                server = new TcpListener(ipAddr, port);
                server.Start();
                
                server.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTCPClientCallBack), server);

            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Receive data
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public override int Receive(out byte[] buffer)
        {
             try
            {
                ns.ReadTimeout = 200;
               
                byte[] buf = new byte[8096];
                int readSize = ns.Read(buf, 0, 8096);

                buffer = new byte[readSize];

                Array.Copy(buf, 0, buffer, 0, readSize);

                return readSize;
            }
            catch
            {
                buffer = null;
                return -1;
            }
        }

        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override int Send(byte[] data)
        {
            try
            {
                ns.BeginWrite(data, 0, data.Length, null, null);
                return data.Length;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Data process
        /// </summary>
        /// <param name="ar"></param>
        private void OnDataRead(IAsyncResult ar)
        {
            int offset = 0;
            AsynReadState ss = (AsynReadState)ar.AsyncState;

            //Detect client disconnection. then start anther acception
            if (ss.data[0] == 0)
            {
                server.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTCPClientCallBack), server);
                return;
            }

            try
            {
            REPEAT:                             //if multiple messages exist in the result. repeat the process
                if (new_message)                //new_message is a flag to indicate if the data is part of unfinished message
                {
                    //Calculate message type, version, length and id
                    int header = (ss.data[offset] << 8) + ss.data[offset + 1];
                    msg_type = (Int16)(header & 0x03FF);
                    msg_ver = (Int16)((header >> 10) & 0x07);
                    msg_len = (ss.data[offset + 2] << 24) + (ss.data[offset + 3] << 16) + (ss.data[offset + 4] << 8) + ss.data[offset + 5];
                    msg_id = (ss.data[offset + 6] << 24) + (ss.data[offset + 7] << 16) + (ss.data[offset + 8] << 8) + ss.data[offset + 9];

                    //if the message length is 0. the rest of byte array are empty. restart non-block reading
                    if (msg_len == 0)
                    {
                        if (ns != null)
                        {
                            try
                            {
                                ns.Flush();
                                state = new AsynReadState(BUFFER_SIZE);
                                ns.BeginRead(state.data, 0, state.data.Length, new AsyncCallback(OnDataRead), state);
                            }
                            catch { }

                            return;
                        }
                        else
                            return;
                    }

                    msg_data = new byte[msg_len];
                    //if message length greater than the calcualted message length. copy message and trigger message event
                    if (ss.data.Length >= msg_len)
                    {
                        Array.Copy(ss.data, offset, msg_data, 0, msg_len);

                        delegateMessageReceived msgRecv = new delegateMessageReceived(TriggerMessageEvent);
                        msgRecv.BeginInvoke(msg_ver, msg_type, msg_id, msg_data, null, null);

                        offset += msg_len;
                        msg_len = 0;

                        goto REPEAT;
                    }
                    else//If the received data is shorter than the message length, keep reading for the next data 
                    {
                        new_message = false;
                        Array.Copy(ss.data, 0, msg_data, 0, ss.data.Length);
                        msg_cursor += ss.data.Length;
                    }
                }
                else
                {
                    //if data length larger than needed data for a complete message, 
                    //copy data into existing message and triggered message event 
                    if (ss.data.Length >= msg_len - msg_cursor)
                    {
                        Array.Copy(ss.data, 0, msg_data, msg_cursor, msg_len - msg_cursor);
                        delegateMessageReceived msgRecv = new delegateMessageReceived(TriggerMessageEvent);
                        msgRecv.BeginInvoke(msg_ver, msg_type, msg_id, msg_data, null, null);

                        offset += msg_len - msg_cursor;

                        

                        msg_len = 0;
                        msg_cursor = 0;

                        new_message = true;
                        goto REPEAT;
                    }
                    else //keep reading
                    {
                        new_message = false;
                        Array.Copy(ss.data, 0, msg_data, msg_cursor, ss.data.Length);
                        msg_cursor += ss.data.Length;
                    }
                }

                //if ns !=null, do next asyn-read, to ensure that read
                if (ns != null && ns.CanRead)
                {
                    try
                    {
                        ns.Flush();
                        state = new AsynReadState(BUFFER_SIZE);
                        ns.BeginRead(state.data, 0, state.data.Length, new AsyncCallback(OnDataRead), state);
                    }
                    catch { }
                }
            }
            catch
            {
            }
        }
    }
}
