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
using System.IO;

namespace DVLD_Full_Project.Licenses.Controls
{
    public partial class ctrlInternatioanlDriverInfo : UserControl
    {
        private clsInternationalLicense _InternationalLicense;
        public ctrlInternatioanlDriverInfo()
        {
            InitializeComponent();
        }


        public void ResetLicenseInfo()
        {

            lblName.Text = "[????]";
            lblInILicenseID.Text = "[????]";
            lblLicenseID.Text = "[????]";
            lblNashonalNo.Text = "[????]";
            lblGender.Text = "[????]";
            picGender.Image = Resources.Man_32;
            lblIssueDate.Text = "[????]";
            lblApplicationID.Text = "[????]";
            lblIsActive.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblDriverID.Text = "[????]";
            lblExpirationDate.Text = "[????]";
            picPerson.Image = Resources.Male_512;
        }

        private void _LoadLicenseImage()
        {
            if (_InternationalLicense.DriverInfo.PersonInfo.Gender == 0)
                picPerson.Image = Resources.Male_512;
            else
                picPerson.Image = Resources.Female_512;

            string ImagePath = _InternationalLicense.DriverInfo.PersonInfo.ImagePath;

            if (ImagePath != "")
                if (File.Exists(ImagePath))
                    picPerson.ImageLocation = ImagePath;
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void _FillInternationalLicenseInfo()
        {
            lblName.Text = _InternationalLicense.DriverInfo.PersonInfo.FullName;
            lblName.Text = _InternationalLicense.InternationalLicenseID.ToString();
            lblLicenseID.Text = _InternationalLicense.IssuedUsingLocalLicenseID.ToString();
            lblNashonalNo.Text = _InternationalLicense.DriverInfo.PersonInfo.NationalNo;
            lblGender.Text = _InternationalLicense.DriverInfo.PersonInfo.Gender == 0 ? "Mail" : "Femail";
            picGender.Image = _InternationalLicense.DriverInfo.PersonInfo.Gender == 0 ? picGender.Image = Resources.Man_32 : picGender.Image = Resources.Woman_32;
            lblIssueDate.Text = _InternationalLicense.IssueDate.ToString("yy/MMM/yyyy");
            lblApplicationID.Text = _InternationalLicense.ApplicationID.ToString();
            lblIsActive.Text = _InternationalLicense.IsActive ? "Yes" : "No";
            lblDateOfBirth.Text = _InternationalLicense.DriverInfo.PersonInfo.DateOfBirth.ToString("yy/MMM/yyyy");
            lblDriverID.Text = _InternationalLicense.DriverID.ToString();
            lblExpirationDate.Text = _InternationalLicense.ExpirationDate.ToString("yy/MMM/yyyy");

            _LoadLicenseImage();
        }
        public void LoadInternationalLicenseInfo(int InternationalLicenseID)
        {
            _InternationalLicense = clsInternationalLicense.Find(InternationalLicenseID);

            if (_InternationalLicense == null)
            {
                ResetLicenseInfo();
                MessageBox.Show("No License with InternationalLicenseID = " + InternationalLicenseID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillInternationalLicenseInfo();
        }
    }
}
