using BankBusinessLayer;
using DVLD_BusinessLayer;
using DVLD_Full_Project.Licenses.Controls;
using DVLD_Full_Project.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Full_Project.Applications.Controls
{
    public partial class ctrlDrivingLicenseApplicationInfo : UserControl
    {
        private int _LocalApplicationID = -1;
        private clsLocalDrivingLicenseApplication _LocalApplication;

        private int _LicenseID;

        public int LocalDrivingLicenseApplicatoinID
        {
            get { return _LocalApplicationID; }
        }

        private DataTable _dtAllPassedTests;

        private bool _ShowLicenseInfo = true;
      
        public ctrlDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        public void LoadApplicationInfoByLocalDrivingAppID(int LocalApplicationID)
        {
            _LocalApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalApplicationID);
            if (_LocalApplication == null)
            {
                MessageBox.Show("No Local Application with LocalApplicationID = " + LocalApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetLocalApplicationInfo();
                return;
            }

            _FullLocalApplicationInfo();
        }

        public void LoadApplicationInfoByApplicationID(int ApplicationID)
        {
            _LocalApplication = clsLocalDrivingLicenseApplication.FindByApplicationID(ApplicationID);

            if (_LocalApplication == null)
            {
                ResetLocalApplicationInfo();
                MessageBox.Show("No Local Application with ApplicationID = " + ApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FullLocalApplicationInfo();
        }

        private void _FullLocalApplicationInfo()
        {
            _LicenseID = _LocalApplication.GetActiveLicenseID();

            llShowLicenseInfo.Enabled = (_LicenseID != -1);
            
            lblDLAppID.Text = _LocalApplication.LocalDrivingLicenseApplicationID.ToString();
            lblAppliedForLicense.Text = clsLicenseClass.Find(_LocalApplication.LicenseClassID).ClassName;
            lblPassedTests.Text = _LocalApplication.GetPassedTestCount().ToString() + "/3";
            ctrlApplicationBasicInfo1._LoadApplicationInfo(_LocalApplication.ApplicationID);
        }

        public void ResetLocalApplicationInfo()
        {
            _LocalApplicationID = -1;
            ctrlApplicationBasicInfo1.ResetApplicationInfo();
            lblDLAppID.Text = "[????]";
            lblAppliedForLicense.Text = "[????]";
            lblPassedTests.Text = "0";
        }

        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonInfo personInfo = new frmShowPersonInfo(_LocalApplication.ApplicantPersonID);
            personInfo.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int LicenseID = _LocalApplication.GetActiveLicenseID();

            frmShowLicenseInfo frmLicenseInfo = new frmShowLicenseInfo(LicenseID);
            frmLicenseInfo.ShowDialog();
        }
    }
}
