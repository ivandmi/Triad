using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TriadNSim.Forms
{
    public partial class frmParamForGraphs : Form
    {
        public frmParamForGraphs()
        {
            InitializeComponent();
            cbGraphModels.SelectedIndex = 0;
        }


        public int CountOfVertex
        {
            get
            {
                return Convert.ToInt32(txtCountVertex.Text);
            }
        }

        public double Probability
        {
            get
            {
                return Convert.ToDouble(txtProb.Text);
            }
        }

        public int k
        {
            get
            {
                return Convert.ToInt32(txtK.Text);
            }
        }

        public int a
        {
            get
            {
                return Convert.ToInt32(txtParamA.Text);
            }
        }

        public int CountStep
        {
            get
            {
                return Convert.ToInt32(txtCountStep.Text);
            }
        }

        public enum GraphModels
        {
            Erdos_Renyi,
            Barabashi_Albert,
            Bollobash_Riordan
        }

        public int GraphModel
        {
            get
            {
                return cbGraphModels.SelectedIndex;
            }
        }

        public string NodeName
        {
            get
            {
                return string.IsNullOrWhiteSpace(txtNodeName.Text)?"node":txtNodeName.Text.Trim();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
           
            //if (txtCountVertex.Text != "")
            //{
            //    if (rbErdesh.Checked == true)
            //    {
            //        StatClass.CountOfVertex = Convert.ToInt32(txtCountVertex.Text);
            //        StatClass.Probability = Convert.ToDouble(txtProb.Text);
            //    }
            //    if (rbBarabash.Checked == true)
            //    {
            //        StatClass.CountOfVertex = Convert.ToInt32(txtCountVertex.Text);
            //        StatClass.CountStep = Convert.ToInt32(txtCountStep.Text);
            //    }
            //    if (rbBollobashRiordan.Checked == true)
            //    {
            //        StatClass.CountOfVertex = Convert.ToInt32(txtCountVertex.Text);
            //        StatClass.CountStep = Convert.ToInt32(txtK.Text);
            //    }
            //}
            //this.Hide();
            //this.Close();
        }

        private void cbGraphModels_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbGraphModels.SelectedIndex)
            {
                case 0:
                    txtProb.Enabled = true;
                    txtParamA.Enabled = false;
                    txtK.Enabled= false;
                    txtCountStep.Enabled = false;
                    break;
                case 1:
                    txtProb.Enabled = false;
                    txtParamA.Enabled = false;
                    txtK.Enabled = false;
                    txtCountStep.Enabled = true;
                    break;
                case 2:
                    txtProb.Enabled = false;
                    txtParamA.Enabled = false;
                    txtK.Enabled = true;
                    txtCountStep.Enabled = false;
                    break;
                case 3:
                    txtProb.Enabled = false;
                    txtParamA.Enabled = true;
                    txtK.Enabled = true;
                    txtCountStep.Enabled = false;
                    break;
                case 4:
                    txtProb.Enabled = true;
                    txtCountStep.Enabled = false;
                    txtK.Enabled = true;
                    txtParamA.Enabled = false;
                    break;
            }
        }
    }
}
