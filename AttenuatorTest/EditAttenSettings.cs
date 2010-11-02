using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AttenuatorTest
{
    public partial class EditAttenSettings : Form
    {

        private AttenuatorTest attenTestMgr;

        public EditAttenSettings(AttenuatorTest mgr)
        {
            attenTestMgr = mgr;
            InitializeComponent();
            ApplyAttenToForm();
        }


        public void ApplyAttenToForm()
        {
            AttenStepInfo attenConfig = attenTestMgr.GetAttenConfig();

            txtAttnStepOnTime.Text = attenConfig.attenRunTime/1000.0 + "";
            txtAttnStepOffTime.Text = attenConfig.attenSettleTime / 1000.0 + "";
        }

        public void ApplyFormToAtten()
        {
            AttenStepInfo attenConfig = attenTestMgr.GetAttenConfig();


            // Collect required settings
            attenConfig.attenRunTime = Int32.Parse(txtAttnStepOnTime.Text) * 1000;
            attenConfig.attenSettleTime = Int32.Parse(txtAttnStepOffTime.Text) * 1000;
            if (attenConfig.attenRunTime == 0)
                attenConfig.attenRunTime = 1;
            if (attenConfig.attenSettleTime == 0)
                attenConfig.attenSettleTime = 1;
        }

        private void btnSettingsApply_Click(object sender, EventArgs e)
        {
            try
            {
                ApplyFormToAtten();
                this.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSettingsDefault_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}