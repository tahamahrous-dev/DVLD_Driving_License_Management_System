using DVLD_BusinessLayer;
using DVLD_Full_Project.People.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Full_Project.Applications
{
    public partial class frmEditApplicationType : Form
    {

        private int _ApplicationTypeID=-1;
        private clsApplicationType _ApplicationType;

        public frmEditApplicationType(int AppTypeID)
        {
            InitializeComponent();
            _ApplicationTypeID = AppTypeID;
        }

        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {
            lblAppTypeID.Text = _ApplicationType.ToString();

            _ApplicationType = clsApplicationType.Find(_ApplicationTypeID);

            if (_ApplicationType != null)
            {
                txtType.Text = _ApplicationType.Title;
                txtFees.Text = _ApplicationType.Fees.ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!," +
                   " put the mouse over the red icon", "Validate Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _ApplicationType.Title = txtType.Text.Trim();
            _ApplicationType.Fees = Convert.ToSingle(txtFees.Text);

            if (_ApplicationType.Save())
            { 
                MessageBox.Show("Application Type Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
                MessageBox.Show("Wrong Update the Application Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void txtType_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtType.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtType, "Title cannot be empty!");
            }
            else
            {
                errorProvider1.SetError(txtType, null);
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Fees cannot be empty!");
            }
            else
            {
                errorProvider1.SetError(txtFees, null);
            }



            if(!clsValidation.IsNumber(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Invalid Number.");
            }
            else
            {
                errorProvider1.SetError(txtFees, null);
            }
        }
    }
}
