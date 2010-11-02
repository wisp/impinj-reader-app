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
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Net;

using ReaderLibrary;

namespace Logging
{
    public partial class LoggingForm : Form
    {
        private LoggingManager log;
        private EditLogForm logOption;
        private Logger logOptChosen;
        
        public LoggingForm(LoggingManager logNew)
        {
            InitializeComponent();
            log = logNew;
        }

        private void btnLogApply_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoggingForm_Load(object sender, EventArgs e)
        {
            // set position of form
            base.Location = new Point(800, 500);

            PopulateCmbBox();
        }

        public void PopulateCmbBox()
        {
            cmbLog.DataSource = log.getLogList();
        }

        public  Logger getLogOption()
        {
            return logOptChosen;
        }
        
        private void btnEdit_Click(object sender, EventArgs e)
        {
            logOptChosen = (Logger)cmbLog.SelectedItem;
            
            if (logOption == null || logOption.IsDisposed)
            {
                logOption = new EditLogForm(log, logOptChosen, this);
            }
            logOption.Show();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {

            string newLogName = "Log " + (cmbLog.Items.Count);

            if (!cmbLog.Items.Contains(newLogName))
            {
                // Helps in repopulating of combobox.
                BindingManagerBase bmOrders = this.BindingContext[log.getLogList()];
                bmOrders.SuspendBinding();

                log.AddNewLogger(newLogName);

                bmOrders.ResumeBinding();
            }
            else
            {
                MessageBox.Show("?");
                return;
            }

            cmbLog.SelectedItem = log.FindLogger(newLogName);
            logOptChosen = (Logger)cmbLog.SelectedItem;

            if (logOption == null || logOption.IsDisposed)
            {
                logOption = new EditLogForm(log, logOptChosen, this);
            }
            logOption.Show();

            /*
            if (addOption == null || addOption.IsDisposed)
            {
                addOption = new AddLogForm(this, log);
            }
            addOption.Show();
             * */
        }
    }
}