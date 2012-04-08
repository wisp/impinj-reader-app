

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


/*
 * How to add new tag statistics:
 * 1. Add your statistic to the StatColumnIdx Enumeration
 * 2. Edit SetupDataGridView() to add your column to the tagStatsDataGridView
 * 3. Edit ParseTag(..) to parse and display the particular statistic you are adding.
 * 
 */




using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Drawing;

using System.Diagnostics;
using ReaderLibrary;

using System.Windows.Forms;

namespace WISPDemo
{
    class TagStats
    {
        private enum StatColumnIdx
        {
            SERIAL_NUM,
            HW_REV,
            SENSOR_TYPE,
            COUNT,
            LAST_SEEN,
            TX_COUNTER,
            EPC,
            DATA,
            ENUM_MAX_IDX,
        }

        DataGridView tagStatsDataGridView;

        public TagStats(DataGridView dgv)
        {
            tagStatsDataGridView = dgv;

            SetupDataGridView();
        }

        public void Clear()
        {
            tagStatsDataGridView.Rows.Clear();
        }

        public void AddTags(ArrayList tags)
        {
            System.Collections.IEnumerator myEnumerator = tags.GetEnumerator();
            while (myEnumerator.MoveNext())
                ParseTag((MyTag)(myEnumerator.Current));
        }

        public string GetSelectedTagID()
        {
            if (tagStatsDataGridView.SelectedRows.Count >= 1)
            {
                if(tagStatsDataGridView.SelectedRows[0].Cells[(int)StatColumnIdx.EPC].Value != null)
                    return tagStatsDataGridView.SelectedRows[0].Cells[(int)StatColumnIdx.EPC].Value.ToString();
            }
            return null;
        }

        private void ParseTag(MyTag tag)
        {
            bool foundTag = false;
            int id = -1;
            int count;
            int rowIdx;
            string idstr = "";

            // Look for the tag
            for (rowIdx = 0; rowIdx < tagStatsDataGridView.Rows.Count; rowIdx++)
            {
                idstr = (string)(tagStatsDataGridView.Rows[rowIdx].Cells[(int)StatColumnIdx.SERIAL_NUM].Value);
                try
                {
                    id = Convert.ToInt32(idstr);
                }
                catch(Exception e)
                {
                    Trace.WriteLine("exception: " + e.ToString());
                }

                if (tag != null && id == tag.GetSerialNumber())
                {
                    foundTag = true;
                    break;
                }
                
            }

            // Update the info
            if (foundTag)
            {
                // Update Count
                count = Convert.ToInt32(tagStatsDataGridView.Rows[rowIdx].Cells[(int)StatColumnIdx.COUNT].Value);
                count += tag.GetCount();
                tagStatsDataGridView.Rows[rowIdx].Cells[(int)StatColumnIdx.COUNT].Value = count;
                tagStatsDataGridView.Rows[rowIdx].Cells[(int)StatColumnIdx.SENSOR_TYPE].Value = tag.GetTagTypeName();
                tagStatsDataGridView.Rows[rowIdx].Cells[(int)StatColumnIdx.LAST_SEEN].Value = System.DateTime.Now.ToString();
                tagStatsDataGridView.Rows[rowIdx].Cells[(int)StatColumnIdx.TX_COUNTER].Value = tag.GetPacketCounter().ToString();
                tagStatsDataGridView.Rows[rowIdx].Cells[(int)StatColumnIdx.EPC].Value = tag.GetEpcID().ToString();
                tagStatsDataGridView.Rows[rowIdx].Cells[(int)StatColumnIdx.DATA].Value = tag.GetAccessResultData();
            }
            else // add tag
            {
                string[] newRow = new string[(int)StatColumnIdx.ENUM_MAX_IDX];
                newRow[(int)StatColumnIdx.SERIAL_NUM] = tag.GetSerialNumber().ToString();
                newRow[(int)StatColumnIdx.HW_REV] = tag.GetHardwareRev();
                newRow[(int)StatColumnIdx.SENSOR_TYPE] = tag.GetTagTypeName();
                newRow[(int)StatColumnIdx.COUNT] = tag.GetCount().ToString();
                newRow[(int)StatColumnIdx.LAST_SEEN] = System.DateTime.Now.ToString();
                newRow[(int)StatColumnIdx.TX_COUNTER] = tag.GetPacketCounter().ToString();
                newRow[(int)StatColumnIdx.EPC] = tag.GetEpcID().ToString();
                newRow[(int)StatColumnIdx.DATA] = tag.GetAccessResultData().ToString();
                tagStatsDataGridView.Rows.Add(newRow);
                Trace.WriteLine("Stats Added New Tag: " + tag.GetSerialNumber().ToString());
            }
            
            // resize columns - this kills performance :(
            // button added instead.
            // tagStatsDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void SetupDataGridView()
        {

            tagStatsDataGridView.ColumnCount = (int)StatColumnIdx.ENUM_MAX_IDX;

            tagStatsDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            tagStatsDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            tagStatsDataGridView.ColumnHeadersDefaultCellStyle.Font =
                new Font(tagStatsDataGridView.Font, FontStyle.Bold);

            tagStatsDataGridView.Name = "tagStatsDataGridView";
            tagStatsDataGridView.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            tagStatsDataGridView.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            tagStatsDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            tagStatsDataGridView.GridColor = Color.Black;
            tagStatsDataGridView.RowHeadersVisible = false;

            // Size Columns Reasonably
            tagStatsDataGridView.Columns[(int)StatColumnIdx.SERIAL_NUM].Width = 44;
            tagStatsDataGridView.Columns[(int)StatColumnIdx.HW_REV].Width = 35;
            tagStatsDataGridView.Columns[(int)StatColumnIdx.SENSOR_TYPE].Width = 51;
            tagStatsDataGridView.Columns[(int)StatColumnIdx.COUNT].Width = 46;
            tagStatsDataGridView.Columns[(int)StatColumnIdx.LAST_SEEN].Width = 118;
            tagStatsDataGridView.Columns[(int)StatColumnIdx.TX_COUNTER].Width = 57;
            tagStatsDataGridView.Columns[(int)StatColumnIdx.EPC].Width = 180;
            tagStatsDataGridView.Columns[(int)StatColumnIdx.DATA].Width = 180;

            // Add column labels
            tagStatsDataGridView.Columns[(int)StatColumnIdx.SERIAL_NUM].Name = "Serial #";
            tagStatsDataGridView.Columns[(int)StatColumnIdx.HW_REV].Name = "HW Rev";
            tagStatsDataGridView.Columns[(int)StatColumnIdx.SENSOR_TYPE].Name = "Sensor";
            tagStatsDataGridView.Columns[(int)StatColumnIdx.COUNT].Name = "Count";
            tagStatsDataGridView.Columns[(int)StatColumnIdx.LAST_SEEN].Name = "Last Seen";
            tagStatsDataGridView.Columns[(int)StatColumnIdx.TX_COUNTER].Name = "Tx Counter";
            tagStatsDataGridView.Columns[(int)StatColumnIdx.EPC].Name = "EPC";
            tagStatsDataGridView.Columns[(int)StatColumnIdx.DATA].Name = "Data";
            
            tagStatsDataGridView.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            tagStatsDataGridView.MultiSelect = false;
            tagStatsDataGridView.Dock = DockStyle.Fill;

            tagStatsDataGridView.CellFormatting += new
                DataGridViewCellFormattingEventHandler(
                tagStatsDataGridView_CellFormatting);
        }


        private void tagStatsDataGridView_CellFormatting(object sender,
    System.Windows.Forms.DataGridViewCellFormattingEventArgs e)
        {
            if (this.tagStatsDataGridView.Columns[e.ColumnIndex].Name == "Release Date")
            {
                if (e != null)
                {
                    if (e.Value != null)
                    {
                        try
                        {
                            e.Value = DateTime.Parse(e.Value.ToString())
                                .ToLongDateString();
                            e.FormattingApplied = true;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("{0} is not a valid date.", e.Value.ToString());
                        }
                    }
                }
            }
        }




    }
}
