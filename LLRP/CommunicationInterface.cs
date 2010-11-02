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
 * File Name:       CommunicationInterface.cs
 * 
 * Author:          Impinj
 * Organization:    Impinj
 * Date:            September, 2007
 * 
 * Description:     This file contains interfaces, base classes and parser 
 *                  for estabilishing communication between reader and 
 *                  application
***************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace LLRP
{

    public delegate void delegateMessageReceived(Int16 ver, Int16 msg_type, int msg_id, byte[] data);
    public delegate void delegateClientConnected();

    /// <summary>
    /// Async. read state class
    /// </summary>
    [Serializable]
    class AsynReadState
    {
        public byte[] data;
        public AsynReadState(Int32 buffer_size)
        {
            data = new byte[buffer_size];
        }
    }

    /// <summary>
    /// Enumeration of communication interface
    /// </summary>
    public enum ENUM_INTERFACE_TYPE
    {
        TCPIP = 0,
        USB = 1,
        RS232 = 2
    }

    /// <summary>
    /// Communication base class
    /// </summary>
    [Serializable]
    abstract class CommunicationInterface : IDisposable
    {
        protected AsynReadState state;
        public event delegateMessageReceived OnMessageReceived;
        public event delegateClientConnected OnClientConnected;

        protected void TriggerMessageEvent(Int16 ver, Int16 msg_type, int msg_id, byte[] data)
        {
            try { if (OnMessageReceived != null) OnMessageReceived(ver, msg_type, msg_id, data); }
            catch { }
        }

        protected void TriggerOnClientConnect()
        {
            try { if (OnClientConnected != null) OnClientConnected(); }
            catch { }
        }

        /// <summary>
        /// Open the communication interface.
        /// </summary>
        /// <param name="client">Name or address of the device</param>
        /// <param name="port">port number of device. if the device does not support port, this parameter is ignored.</param>
        /// <returns>True if the the device is opened successfully.</returns>
        public virtual bool Open(string device_name, int port) { return false; }
        
        /// <summary>
        /// Close the communication interface
        /// </summary>
        public virtual void Close(){}

        /// <summary>
        /// Send data through communication interface
        /// </summary>
        /// <param name="data">data to be sent. byte array</param>
        /// <returns>length of data that have been sent</returns>
        public virtual int Send(byte[] data) { return 0; }

        /// <summary>
        /// Receive data
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns>the data length that received</returns>
        public virtual int Receive(out byte[] buffer) { buffer = null; return 0; }

        /// <summary>
        /// Set buffer for receiving data
        /// </summary>
        /// <param name="size">size of the buffer, byte</param>
        /// <returns>true if the buffer is allocated successfully</returns>
        public virtual bool SetBufferSize(int size) { return false; }

        /// <summary>
        /// Release resource
        /// </summary>
        public virtual void Dispose() { }
    }
}
