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

using System.Diagnostics;

using ReaderLibrary;

namespace Logging
{
    public partial class EditLogForm : Form
    {
        private Logger logOptChosen;
        private LoggingManager log;

        private LoggingForm logForm;

        public EditLogForm(LoggingManager logNew, Logger chosen, LoggingForm logFormNew)
        {
            InitializeComponent();
            logOptChosen = chosen;
            log = logNew;
            logForm = logFormNew;
        }

        private void EditLogForm_Load(object sender, EventArgs e)
        {
            // set position of form
            base.Location = new Point(800, 500);

           ApplySettingsToForm(logOptChosen);
        }

        private void btnOpenLogDialog_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "Log File Name";
            string currTime = ((DateTime.Now.ToString()).Replace(":", ".")).Replace("/", "-");
            save.FileName = currTime + " - " + logOptChosen.GetName() + ".csv";
            save.ShowDialog();

            if (save.FileName != "")
            {
                txtLogFileName.Text = save.FileName;
            }
        }

        private void btnLogOptApply_Click(object sender, EventArgs e)
        {
            setLogOption();
            this.Close();
        }

        private void setLogOption()
        {
            BindingManagerBase bmOrders = logForm.BindingContext[log.getLogList()];
            bmOrders.SuspendBinding();

            logOptChosen.SetName(txtLogName.Text);
            logOptChosen.SetFileName(txtLogFileName.Text);

            logOptChosen.SetEnabled(chkEnableLog.Checked);


            List<Logger.LoggingOption> options = logOptChosen.getOptions();

            System.Collections.IEnumerator myEnumerator = options.GetEnumerator();

            while (myEnumerator.MoveNext())
            {
                Logger.LoggingOption option = (Logger.LoggingOption)(myEnumerator.Current);

                int idx = lstOptions.Items.IndexOf(option);
                bool sel = lstOptions.GetSelected(idx);
                int idx2 = options.IndexOf(option);
                //options[idx2].SetEnabled(sel);
                option.enabled = sel;
            }

            bmOrders.ResumeBinding();

            //logFrm.setLogOption(logOptChosen);
        }

        public Logger getLogOption()
        {
            return logOptChosen;
        }

        private void ApplySettingsToForm(Logger logOptChosen)
        {
            if (logOptChosen.IsEnabled())
            {
                chkEnableLog.Checked = true;
            }
            
            txtLogFileName.Text = logOptChosen.GetFileName();
            txtLogName.Text = logOptChosen.GetName();

            lstOptions.DataSource = logOptChosen.getOptions();

            List<Logger.LoggingOption> options = logOptChosen.getOptions();
            System.Collections.IEnumerator myEnumerator = options.GetEnumerator();

            while (myEnumerator.MoveNext())
            {
                Logger.LoggingOption option = (Logger.LoggingOption)(myEnumerator.Current);

                int idx = lstOptions.Items.IndexOf(option);
                int idx2 = options.IndexOf(option);
                lstOptions.SetSelected(idx, options[idx2].enabled);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}