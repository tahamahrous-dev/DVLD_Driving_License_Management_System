using DVLD_BusinessLayer;
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

namespace DVLD_Full_Project.Applications.Controls
{
    public partial class ctrlApplicationBasicInfo : UserControl
    {
        private int _ApplicationID;
        private clsApplication _Application;


        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
        }

        private void _FullApplicationInfo()
        {
            _ApplicationID = _Application.ApplicationID;
            lblID.Text = _Application.ApplicationID.ToString();
            lblStatus.Text = _Application.StatusText;
            lblFees.Text = _Application.PaidFees.ToString();
            lblType.Text = clsApplicationType.Find(_Application.ApplicationTypeID).Title;
            lblApplicant.Text = _Application.ApplicantFullName;
            lblDate.Text = _Application.ApplicationDate.ToString("dd/MMM/yyyy");
            lblStatuseDate.Text = _Application.LastStatusDate.ToString("dd/MMM/yyyy");
            lblCreatedBy.Text = _Application.CreatedByUserInfo.UserName;

        }

        public void ResetApplicationInfo()
        {
            lblID.Text = "[????]";
            lblStatus.Text = "[????]";
            lblFees.Text = "[????]";
            lblType.Text = "[????]";
            lblApplicant.Text = "[????]";
            lblDate.Text = "[????]";
            lblStatuseDate.Text = "[????]";
            lblCreatedBy.Text = "[????]";
        }

        public void _LoadApplicationInfo(int ApplicationID)
        {
            _Application = clsApplication.FindBaseApplication(ApplicationID);
            if (_Application == null)
            {
                MessageBox.Show("No Person with PersonID = " + ApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetApplicationInfo();
                return;
            }

            _FullApplicationInfo();
        }

        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonInfo personInfo = new frmShowPersonInfo(_Application.ApplicantPersonID);
            personInfo.ShowDialog();
        }
    }
}
