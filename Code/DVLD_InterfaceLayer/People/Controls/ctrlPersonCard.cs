using DVLD_BusinessLayer;
using DVLD_Full_Project.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace DVLD_Full_Project.People.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        private clsPerson _Person;

        private int _PersonID = -1;

        public int PersonID
        {
            get { return _PersonID; }
        }

        public clsPerson SelectPersonInfo
        { 
            get { return _Person; }
        }

        private bool _EditPersonEnabled = true;
        public bool EditPersonEnabled
        {
            get
            {
                return _EditPersonEnabled;
            }
            set
            {
                _EditPersonEnabled = value;
                llEditPersonInfo.Enabled = _EditPersonEnabled;
            }
        }

        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        public void ResetPersonInfo()
        {
            EditPersonEnabled = false;
            lblPersonID.Text = "[????]";
            lblName.Text = "[????]";
            lblNashonalNo.Text = "[????]";
            lblGender.Text = "[????]";
            picGender.Image = Resources.Man_32;
            lblDateOfBirth.Text = "[????]";
            lblEmail.Text = "[????]";
            lblAddress.Text = "[????]";
            lblPhone.Text = "[????]";
            lblCountry.Text = "[????]";
            picPerson.Image = Resources.Male_512;
        }

        private void _LoadPersonImage()
        {
            if (_Person.Gender == 0)
                picPerson.Image = Resources.Male_512;
            else
                picPerson.Image = Resources.Female_512;

            string ImagePath = _Person.ImagePath;
            if (ImagePath != "")
                if (File.Exists(ImagePath))
                    picPerson.ImageLocation = ImagePath;
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void _FillPersonInfo()
        {
            llEditPersonInfo.Enabled = true;
            _PersonID = _Person.PersonID;
            lblPersonID.Text = _Person.PersonID.ToString();
            lblNashonalNo.Text = _Person.NationalNo;
            lblName.Text = _Person.FullName;
            lblGender.Text = _Person.Gender == 0 ? "Mail" : "Femail";
            picGender.Image = _Person.Gender == 0 ? picGender.Image = Resources.Man_32 : picGender.Image = Resources.Woman_32;
            lblEmail.Text = _Person.Email;
            lblPhone.Text = _Person.Phone;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblAddress.Text = _Person.Address;
            lblCountry.Text = _Person.CountryInfo.CountryName;
            _LoadPersonImage();
        }
        public void LoadPersonInfo(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);

            if (_Person == null)
            {
                MessageBox.Show("No Person with PersonID = " + PersonID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetPersonInfo();
                return;
            }

            _FillPersonInfo();
        }

        public void LoadPersonInfo(string NashonalNo)
        {
            _Person = clsPerson.Find(NashonalNo);

            if (_Person == null)
            {
                MessageBox.Show("No Person with Nashonal No = " + NashonalNo, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetPersonInfo();
                return;
            }

            _FillPersonInfo();
        }

        private void lkEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdatePerson updatePerson = new frmAddUpdatePerson(_PersonID);
            updatePerson.ShowDialog();

            // refresh
            LoadPersonInfo(_PersonID);
        }
    }
}
