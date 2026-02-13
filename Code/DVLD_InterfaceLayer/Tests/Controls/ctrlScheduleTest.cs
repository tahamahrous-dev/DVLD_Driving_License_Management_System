using DVLD_BusinessLayer;
using DVLD_Full_Project.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Full_Project.Tests.Controls
{
    public partial class ctrlScheduleTest : UserControl
    {
        public enum enMode { AddNew= 0, Update=1}
        private enMode _Mode = enMode.AddNew;


        public enum enCreationMode { FirstTimeSchedule = 0, RetakeTestSchedule = 1 }
        private enCreationMode _CreationMode = enCreationMode.FirstTimeSchedule;

        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;

        private clsLocalDrivingLicenseApplication _LocalAppliaction;
        private int _LocalApplicationID = -1;

        private clsTestAppointment _TestAppointment;

        private int _TestAppointmentID = -1;
        public clsTestType.enTestType TestTypeID
        {
            get 
            { 
                return _TestTypeID; 
            }

            set
            {
                _TestTypeID = value;
                switch(_TestTypeID)
                {
                    case clsTestType.enTestType.VisionTest:
                        {
                            gbTypeTest.Text = "Vision Test";
                            pbTestTypeImage.Image = Resources.Vision_512;
                            break;
                        }

                    case clsTestType.enTestType.WrittenTest:
                        {
                            gbTypeTest.Text = "Written Test";
                            pbTestTypeImage.Image = Resources.Written_Test_512;
                            break;
                        }

                    case clsTestType.enTestType.StreetTest:
                        {
                            gbTypeTest.Text = "Vision Test";
                            pbTestTypeImage.Image = Resources.driving_test_512;
                            break;
                        }
                }
            }

        }
       
        public ctrlScheduleTest()
        {
            InitializeComponent();
        }

        public void LoadInfo(int LocalApplicationID, int AppointmentID = -1)
        {
            if (AppointmentID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;

            _LocalApplicationID = LocalApplicationID;
            _TestAppointmentID = AppointmentID;
            _LocalAppliaction = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalApplicationID);

            if (_LocalAppliaction == null)
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + LocalApplicationID,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }


            if (_LocalAppliaction.DoesAttendTestType(_TestTypeID))
                _CreationMode = enCreationMode.RetakeTestSchedule;
            else
                _CreationMode = enCreationMode.FirstTimeSchedule;



            if (_CreationMode == enCreationMode.RetakeTestSchedule)
            {
                lblRAppFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).Fees.ToString();
                gbRetakeTest.Enabled = true;
                lblTitleTest.Text = "Schedule Retake Test";
                lblRTestAppID.Text = "0";
            }
            else
            {
                gbRetakeTest.Enabled = false;
                lblTitleTest.Text = "Schedule Test";
                lblRAppFees.Text = "0";
                lblRTestAppID.Text = "N/A";
            }

            lblDLAppID.Text = _LocalAppliaction.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _LocalAppliaction.LicenseClassInfo.ClassName;
            lblName.Text = _LocalAppliaction.ApplicantFullName;

            lblTrial.Text = _LocalAppliaction.TotalTrialsPerTest(_TestTypeID).ToString();

            if (_Mode == enMode.AddNew)
            {
                lblFees.Text = clsTestType.Find(_TestTypeID).Fees.ToString();
                dtpSchduleDate.MinDate = DateTime.Now;
                lblRTestAppID.Text = "N/A";

                _TestAppointment = new clsTestAppointment();
            }
            else
            {
                if (!_LoadTestAppointmentData())
                    return;
            }

            lblTotalFees.Text = (Convert.ToSingle(lblFees.Text) + Convert.ToSingle(lblRAppFees.Text)).ToString();

            if (!_HandleActiveTestAppointmentConstraint())
                return;

            if (!_HandleAppointmentLockedConstraint())
                return;

            if (!_HandlePrviousTestConstraint())
                return;


        }
        private bool _HandleActiveTestAppointmentConstraint()
        {
            if (_Mode == enMode.AddNew && clsLocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(_LocalApplicationID, TestTypeID))
            {
                lblMessage.Text = "Person Already have an active appointment for this test type";
                btnSave.Enabled = false;
                dtpSchduleDate.Enabled = false;
                return false;
            }

            return true;
        }
        private bool _HandleAppointmentLockedConstraint()
        {
            if (_TestAppointment.IsLocked)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Person Already sat for the user, appointment loacked.";
                dtpSchduleDate.Enabled = false;
                btnSave.Enabled = false;
                return false;
            }
            else
                lblMessage.Visible = false;


            return true;
        }
        private bool _HandlePrviousTestConstraint()
        {
            switch (TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                    lblMessage.Visible = false;
                    return true;

                case clsTestType.enTestType.WrittenTest:
                    if (!_LocalAppliaction.DoesPassTestType(clsTestType.enTestType.VisionTest))
                    {
                        lblMessage.Text = "Cannot Sechule, Vision Test Should be passed first";
                        lblMessage.Visible = true;
                        btnSave.Enabled = false;
                        dtpSchduleDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblMessage.Visible = false;
                        btnSave.Enabled = true;
                        dtpSchduleDate.Enabled = true;
                    }


                    return true;

                case clsTestType.enTestType.StreetTest:

                    //Street Test, you cannot sechdule it before person passes the written test.
                    //we check if pass Written 2.
                    if (!_LocalAppliaction.DoesPassTestType(clsTestType.enTestType.WrittenTest))
                    {
                        lblMessage.Text = "Cannot Sechule, Written Test should be passed first";
                        lblMessage.Visible = true;
                        btnSave.Enabled = false;
                        dtpSchduleDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblMessage.Visible = false;
                        btnSave.Enabled = true;
                        dtpSchduleDate.Enabled = true;
                    }


                    return true;

            }
            return true;
        }
        private bool _LoadTestAppointmentData()
        {
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);
            if (_TestAppointment == null)
            {
                MessageBox.Show("Error: No Appointment with ID = " + _TestAppointmentID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }

            lblFees.Text = _TestAppointment.PaidFees.ToString();

            if (DateTime.Compare(DateTime.Now, _TestAppointment.AppointmentDate) < 0)
                dtpSchduleDate.MinDate = DateTime.Now;
            else
                dtpSchduleDate.MinDate = _TestAppointment.AppointmentDate;

            dtpSchduleDate.Value = _TestAppointment.AppointmentDate;

            if (_TestAppointment.RetakeTestApplicationID == -1)
            {
                lblRAppFees.Text = "0";
                lblRTestAppID.Text = "N/A";
            }
            else
            {
                lblRAppFees.Text = _TestAppointment.RetakeTestAppInfo.PaidFees.ToString();
                gbRetakeTest.Enabled = true;
                lblTitleTest.Text = "Schedule Retake Test";
                lblRTestAppID.Text = _TestAppointment.RetakeTestApplicationID.ToString();
            }

            return true;
        }

        private bool _HandleRetakeApplication()
        {
            if (_Mode == enMode.AddNew && _CreationMode == enCreationMode.RetakeTestSchedule)
            {
                clsApplication Application = new clsApplication();

                Application.ApplicantPersonID = _LocalAppliaction.ApplicantPersonID;
                Application.ApplicationDate = DateTime.Now;
                Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;
                Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                Application.LastStatusDate = DateTime.Now;
                Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).Fees;
                Application.CreatedByUserID = clsGlobal.CurrentUser.UserID;

                if(!Application.Save())
                {
                    _TestAppointment.RetakeTestApplicationID = -1;
                    MessageBox.Show("Faild to Create application", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                _TestAppointment.RetakeTestApplicationID = Application.ApplicationID;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_HandleRetakeApplication())
                return;

            _TestAppointment.TestTypeID = _TestTypeID;
            _TestAppointment.LocalDrivingLicenseApplicationID = _LocalAppliaction.LocalDrivingLicenseApplicationID;
            _TestAppointment.AppointmentDate = dtpSchduleDate.Value;
            _TestAppointment.PaidFees = Convert.ToSingle(lblFees.Text);
            _TestAppointment.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (_TestAppointment.Save())
            {
                _Mode = enMode.Update;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
}
