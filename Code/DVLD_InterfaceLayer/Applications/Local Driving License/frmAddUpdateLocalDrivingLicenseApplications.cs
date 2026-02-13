using BankBusinessLayer;
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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Full_Project.Applications
{
    public partial class frmAddUpdateLocalDrivingLicenseApplications: Form
    {
        enum enMode { AddNew = 0, Update = 1 };
        enMode _Mode;

        private int _LocalApplicationID;
        private clsLocalDrivingLicenseApplication _LocalApplication;

        private int _SelectedPersonID;
        public frmAddUpdateLocalDrivingLicenseApplications()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddUpdateLocalDrivingLicenseApplications(int LocalApplicationID)
        {
            InitializeComponent();
            _LocalApplicationID = LocalApplicationID;
            _Mode = enMode.Update;
        }
        private void _FillLicenesClassesInComboBox()
        {
            cbLicenesClasses.DataSource = clsLicenseClass.GetAllLicenseClasses();
            cbLicenesClasses.DisplayMember = "ClassName";
            cbLicenesClasses.ValueMember = "LicenseClassID";
            cbLicenesClasses.SelectedIndex = 2;
        }

        private void _ResetDefualtValuse()
        {
            _FillLicenesClassesInComboBox();

            if (_Mode == enMode.AddNew)
            {
                this.Text = "New Local Driving Licenes Application";
                lblTitle.Text = "New Local Driving Licenes Application";
                _LocalApplication = new clsLocalDrivingLicenseApplication();
                ctrlPersonCardWithFilter1.FilterFocus();
                tpApplicationInfo.Enabled = false;

                cbLicenesClasses.SelectedIndex = 2;
                lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewDrivingLicense).Fees.ToString();
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
            }
            else
            {
                this.Text = "Update Local Driving Licenes Application";
                lblTitle.Text = "Update Local Driving Licenes Application";

                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true;
            }

            cbLicenesClasses.SelectedIndex = 2;

        }
        private void _LoadData()
        {
            ctrlPersonCardWithFilter1.FilterEnabled = false;
            _LocalApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalApplicationID);
            if (_LocalApplication == null)
            {
                MessageBox.Show("No Application with ID = " + _LocalApplicationID, "Not Found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();

                return;
            }


            ctrlPersonCardWithFilter1.LoadPersonInfo(_LocalApplication.ApplicantPersonID);
            lblLocalApplicationID.Text = _LocalApplication.LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = _LocalApplication.ApplicationDate.ToString("dd/MMM/yyyy");
            cbLicenesClasses.SelectedIndex = cbLicenesClasses.FindString(clsLicenseClass.Find(_LocalApplication.LicenseClassID).ClassName);
            lblApplicationFees.Text = _LocalApplication.PaidFees.ToString();
            // Dr.Abu-Hadhoud Solution
            lblCreatedBy.Text = clsUser.FindByUserID(_LocalApplication.CreatedByUserID).UserName;

            // My Solution
            //lblCreatedBy.Text = _LocalApplication.CreatedByUserInfo.UserName;

        }

        private void DataBackEvent(object sender, int PersonID)
        {
            // Handle the data received
            _SelectedPersonID = PersonID;
            ctrlPersonCardWithFilter1.LoadPersonInfo(PersonID);


        }
        private void frmAddUpdateLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            _ResetDefualtValuse();
            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tabControl1.SelectedTab = tabControl1.TabPages["tpApplicationInfo"];
                return;
            }

            if (ctrlPersonCardWithFilter1.PersonID != -1)
            { 
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tabControl1.SelectedTab = tabControl1.TabPages["tpApplicationInfo"];
            }
            else
            {
                MessageBox.Show("Please select a Person", "Selct a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icons(s)", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            int LicenseClassID = clsLicenseClass.Find(cbLicenesClasses.Text).LicenseClassID;

            int ActiveApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass(_SelectedPersonID,
                clsApplication.enApplicationType.NewDrivingLicense, LicenseClassID);


            if (ActiveApplicationID != -1)
            {
                MessageBox.Show("Choose anather Licenses Class, the selected Person Already\n have an active Application for the selected class with id = "
                  + ActiveApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbLicenesClasses.Focus();

                return;
            }

            if (clsLicense.IsLicenseExistByPersonID(ctrlPersonCardWithFilter1.PersonID, LicenseClassID))
            {
                MessageBox.Show("Person already have a license with the same applied driving class Choose deffrent driving class", "Not Allowed",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LocalApplication.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID;
            _LocalApplication.ApplicationDate     = DateTime.Now;
            _LocalApplication.ApplicationTypeID   = 1;
            _LocalApplication.ApplicationStatus   = clsApplication.enApplicationStatus.New;
            _LocalApplication.LastStatusDate      = DateTime.Now;
            _LocalApplication.PaidFees            = 15;
            _LocalApplication.CreatedByUserID     = clsGlobal.CurrentUser.UserID;
            _LocalApplication.LicenseClassID = LicenseClassID;

            if(_LocalApplication.Save())
            {
                lblLocalApplicationID.Text = _LocalApplication.LocalDrivingLicenseApplicationID.ToString();

                _Mode = enMode.Update;
                lblTitle.Text = "Update Local Driving Licenes Application";
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _SelectedPersonID = obj;
        }

        private void frmAddUpdateLocalDrivingLicenseApplications_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }
    }
}
