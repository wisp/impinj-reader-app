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


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using LLRP;
using LLRP.DataType;

namespace LLRPEndPointServerTest
{
    public partial class MainFrm : Form
    {
        LLRPEndPoint llrpEndP;

        public MainFrm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            llrpEndP = new LLRPEndPoint();

            llrpEndP.Create("", true);

            llrpEndP.OnClientConnected += new delegateClientConnected(llrpEndP_OnClientConnected);

            
        }

        void llrpEndP_OnClientConnected()
        {
            
        }


        //Extend your code here

    }
}