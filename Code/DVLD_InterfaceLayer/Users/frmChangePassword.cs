using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Full_Project.Users
{
    public partial class frmChangePassword : Form
    {
        private int _UserID;
        private clsUser _User;

        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;

        }


        private void _ResetDefualtValues()
        {
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtCurrentPassword.Focus();
        }
        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();
            _User = clsUser.FindByUserID(_UserID);
            if(_User == null)
            {
                MessageBox.Show("Could not FindBaseApplication User with id = " + _UserID,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            ctrlUserCard1.LoadUserInfo(_UserID);
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCurrentPassword.Text.Trim()))
            {
                e.Cancel = true;
                epChangePassword.SetError(txtCurrentPassword, "The Current Password is Required");
                return;
            }
            else
                epChangePassword.SetError(txtCurrentPassword, null);


            if (txtCurrentPassword.Text != _User.Password)
            {
                e.Cancel = true;
                epChangePassword.SetError(txtCurrentPassword, "is Current Password Wrong");
            }
            else
                epChangePassword.SetError(txtCurrentPassword, null);
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPassword.Text.Trim()))
            {
                e.Cancel = true;
                epChangePassword.SetError(txtNewPassword, "The Current Password is Required");
                return;
            }
            else
                epChangePassword.SetError(txtNewPassword, null);



            if (txtNewPassword.Text.Length < 6)
            {
                e.Cancel = true;
                epChangePassword.SetError(txtNewPassword, "Password minmem 6 numbers");
            }
            else
            {
                epChangePassword.SetError(txtNewPassword, null);
            }

        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
            {
                e.Cancel = true;
                epChangePassword.SetError(txtConfirmPassword, "The Current Password is Required");
                return;
            }
            else
                epChangePassword.SetError(txtConfirmPassword, null);


            if (txtConfirmPassword.Text.Trim() != txtNewPassword.Text.Trim())
            {
                e.Cancel = true;
                epChangePassword.SetError(txtConfirmPassword, "Confirm Password not Equl Password");
            }
            else
            {
                epChangePassword.SetError(txtConfirmPassword, null);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icons(s)",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            _User.Password = txtNewPassword.Text;

            if (_User.Save())
            { 
                MessageBox.Show("Data Saved Successfully.",
                    "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ResetDefualtValues();
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
}
