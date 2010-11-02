


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
using System.Collections.Generic;
using System.Text;
using System.Collections;

using ReaderLibrary;

namespace WISPDemo
{
    public class Fifo
    {
        private int maxSize;
        private Queue<MyTag> dataQ;
        
        public Fifo(int maxSize)
        {
            this.maxSize = maxSize;
            this.dataQ = new Queue<MyTag>();
        }

        // Adds a new element to the FIFO
        public void AddLast(MyTag data)
        {
            if(data != null)
            {
               if(dataQ.Count >= this.maxSize)
               {
                   dataQ.Dequeue();
               }
                dataQ.Enqueue(data);
            }
        }

        public void Clear()
        {
            dataQ.Clear();
        }

        /// <summary>
        /// Removes the top most element in the FIFO
        /// If the list contains not elemement, it returns null
        /// </summary>
        /// <returns></returns>
        public MyTag GetFirst()
        {
            MyTag element = null;
            if (dataQ.Count >= 1)
            {
                element = dataQ.Dequeue();
            }
            return element;
        }

        /// <summary>
        /// Returns the current max size of the FIFO
        /// </summary>
        /// <returns></returns>
        public int MaxSize()
        {
            return this.maxSize;
        }

        public int Size()
        {
            return dataQ.Count;
        }

        /// <summary>
        /// Returns true if the the FIFO is empty and false otherwise
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return (dataQ.Count == 0);
        }

        
    }
}
