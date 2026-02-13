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

namespace DVLD_Full_Project.Tests
{
    public partial class frmEditTestType : Form
    {

        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private clsTestType _TestType;

        public frmEditTestType(clsTestType.enTestType AppTypeID)
        {
            InitializeComponent();
            _TestTypeID = AppTypeID;
        }

        private void frmEditTestType_Load(object sender, EventArgs e)
        {

            _TestType = clsTestType.Find(_TestTypeID);

            if (_TestType != null)
            {
                lblAppTypeID.Text = ((int)_TestTypeID).ToString();
                txtType.Text = _TestType.Title;
                txtDescription.Text = _TestType.Description;
                txtFees.Text = _TestType.Fees.ToString();
            }
            else
            {
                MessageBox.Show("Could not find Test Type with id = " + _TestTypeID, "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
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

            _TestType.Description = txtDescription.Text;
            _TestType.Title = txtType.Text.Trim();
            _TestType.Fees = Convert.ToSingle(txtFees.Text);

            if (_TestType.Save())
            { 
                MessageBox.Show("Test Type Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
                MessageBox.Show("Wrong Update the Test Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
