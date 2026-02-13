using DVLD_Full_Project.Applications;
using DVLD_Full_Project.Applications.Replacement;
using DVLD_Full_Project.Applications.Rlease_Detained_License;
using DVLD_Full_Project.Drivers;
using DVLD_Full_Project.Licenses;
using DVLD_Full_Project.Licenses.Detain_License;
using DVLD_Full_Project.Licenses.Local_Licenses;
using DVLD_Full_Project.Login;
using DVLD_Full_Project.People;
using DVLD_Full_Project.Tests;
using DVLD_Full_Project.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Full_Project
{
    public partial class frmMain : Form
    {
        frmLogin _frmLogin;
        public frmMain(frmLogin frm)
        {
            InitializeComponent();
            _frmLogin = frm;
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManagePeople frmManage = new frmManagePeople();
            frmManage.ShowDialog();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageUsers manageUsers = new frmManageUsers();
            manageUsers.ShowDialog();
        }

        private void tsmSignOut_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUser = null;
            _frmLogin.Show();
            this.Close();
        }

        private void tsmChangePassword_Click(object sender, EventArgs e)
        {
            frmChangePassword changePassword = new frmChangePassword(clsGlobal.CurrentUser.UserID);
            changePassword.ShowDialog();
        }

        private void tsmCurrentUser_Click(object sender, EventArgs e)
        {
            frmUserInfo userInfo = new frmUserInfo(clsGlobal.CurrentUser.UserID);
            userInfo.ShowDialog();
        }

        private void manageApplictionTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListApplicationTypes applicationTypes = new frmListApplicationTypes();
            applicationTypes.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmListLocalDrivingLicesnseApplications frmLocalApplications = new frmListLocalDrivingLicesnseApplications();
            frmLocalApplications.ShowDialog();
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenseApplications frmNLDLA = new frmAddUpdateLocalDrivingLicenseApplications();
            frmNLDLA.ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListDrivers Drivers = new frmListDrivers();
            Drivers.ShowDialog();
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            frmNewInternationalLicenseApplication frmInternational = new frmNewInternationalLicenseApplication();
            frmInternational.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListTestTypes frmListTestTypes = new frmListTestTypes();
            frmListTestTypes.ShowDialog();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            frmRenewLocalDrivingLicense frm = new frmRenewLocalDrivingLicense();
            frm.ShowDialog();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            frmReplacementLocalDrivingLicense frm = new frmReplacementLocalDrivingLicense();
            frm.ShowDialog();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            frmDetainLicenseApplication frm = new frmDetainLicenseApplication();
            frm.ShowDialog();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            frmListDetainedLicenses frmListDetained = new frmListDetainedLicenses();
            frmListDetained.ShowDialog();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicenseApplication frm = new frmReleaseDetainedLicenseApplication();
            frm.ShowDialog();
        }
    }
}
