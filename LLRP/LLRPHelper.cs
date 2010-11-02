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
 * File Name:       LLRPHelper.cs
 * 
 * Author:          Impinj
 * Organization:    Impinj
 * Date:            Jan. 22, 2008
 * 
 * Description:     This file contains class definitions for encapsulating LLRP 
 *                  Messages 
***************************************************************************
*/

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Runtime.InteropServices;

namespace LLRP
{
    /// <summary>
    /// Abstract class for all three types of Asyn. message sent from reader
    /// These three messages are: READER_EVENT_NOTIFICATION, RO_ACCESS_REPORT, and KEEP_ALIVE
    /// </summary>
    public abstract class ENCAPED_READER_ASYN_MSG
    {
        public string reader;
    }

    /// <summary>
    /// Class to associate the reader name with the returned READER_EVENT_NOTIFICATION message
    /// </summary>
    public class ENCAPED_READER_EVENT_NOTIFICATION : ENCAPED_READER_ASYN_MSG
    {
        public MSG_READER_EVENT_NOTIFICATION ntf;
    }

    /// <summary>
    /// Class to associate the reader name with the returned RO_ACCESS_REPORT message
    /// </summary>
    public class ENCAPED_RO_ACCESS_REPORT : ENCAPED_READER_ASYN_MSG
    {
        public string reader;
        public MSG_RO_ACCESS_REPORT report;
    }

    /// <summary>
    /// Class to associate the reader name with the returned KEEP_ALIVE message
    /// </summary>
    public class ENCAPED_KEEP_ALIVE : ENCAPED_READER_ASYN_MSG
    {
        public string reader;
        public MSG_KEEPALIVE keep_alive;
    }
}