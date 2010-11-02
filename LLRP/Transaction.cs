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
 * File Name:       Transaction.cs
 * 
 * Author:          Impinj
 * Organization:    Impinj
 * Date:            September, 2007
 * 
 * Description:     This file contains simple network send and receive 
 *                  command.
***************************************************************************
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using LLRP.DataType;

namespace LLRP
{
    /// <summary>
    /// Class for send and receice data
    /// </summary>
    class Transaction
    {
        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="ci">Communication interface</param>
        /// <param name="data">Data to be sent, byte array</param>
        public static void Send(CommunicationInterface ci, byte[] data)
        {
            ci.Send(data);
        }

        /// <summary>
        /// Receive data
        /// </summary>
        /// <param name="ci">Communication interface</param>
        /// <param name="buffer">Buffer for receiving data</param>
        /// <returns></returns>
        public static int Receive(CommunicationInterface ci, out byte[] buffer)
        {
            return ci.Receive(out buffer);
        }
    }
}
