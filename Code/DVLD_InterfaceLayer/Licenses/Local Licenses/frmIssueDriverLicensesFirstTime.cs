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

namespace DVLD_Full_Project.Licenses
{
    public partial class frmIssueDriverLicensesFirstTime : Form
    {
        private int _LocalApplicationID;
        private clsLocalDrivingLicenseApplication _LocalApplication;

        public frmIssueDriverLicensesFirstTime(int LocalApplicationID)
        {
            InitializeComponent();
            _LocalApplicationID = LocalApplicationID;
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            int LicenseID = _LocalApplication.IssueLicenseForTheFirtTime(txtNotes.Text, clsGlobal.CurrentUser.UserID);

            if (LicenseID != -1)
            {
                MessageBox.Show("License Issued Successfully with License ID = " + LicenseID,
                    "Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("License Was not Issued ! ", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmIssueDriverLicensesFirstTime_Load(object sender, EventArgs e)
        {
            txtNotes.Focus();
            _LocalApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalApplicationID);

            if (_LocalApplication == null)
            {
                MessageBox.Show("No Application with ID=" + _LocalApplicationID, "Not Found!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }


            if (!_LocalApplication.PassedAllTests())
            {
                MessageBox.Show("Person Should Pass All Tests First.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            int LicenseID = _LocalApplication.GetActiveLicenseID();
            if (LicenseID != -1)
            {
                MessageBox.Show("Person already has License before with License ID= " + LicenseID, "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LocalApplicationID);
        }
    }
}
