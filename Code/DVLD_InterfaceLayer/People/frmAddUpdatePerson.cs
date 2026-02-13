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

namespace DVLD_Full_Project
{
    public partial class frmAddUpdatePerson : Form
    {
        enum enMode { AddNew = 0, Update = 1 };
        enum enGender { Mail = 0, Femail = 1};

        clsPerson _Person;
        private int _PersonID;
        private enMode _Mode;

        public delegate void DataBackEventHandler(Object sender, int PersonID);
        public event DataBackEventHandler DataBack;

        public frmAddUpdatePerson()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _PersonID = PersonID;
        }
        private void _FillCountriesInComboBox()
        {
            //DataTable dtCountries = clsCountry.GetAllCountries();

            //foreach (DataRow row in dtCountries.Rows)
            //{
            //    cbxCountries.Items.Add(row["CountryName"]);
            //}


            cbxCountries.DataSource = clsCountry.GetAllCountries();
            cbxCountries.DisplayMember = "CountryName";
            cbxCountries.ValueMember = "CountryID";
            //cbxCountries.SelectedIndex = 190;
        }
        private void _ResetDefualtValues()
        {
            _FillCountriesInComboBox();

            if (_Mode == enMode.AddNew)
            {
                this.Text = "Add New Person";
                _Person = new clsPerson();
            }
            else if (_Mode == enMode.Update)
                this.Text = "Update Person";



            if (rbMale.Checked)
                picPerson.Image = Resources.Male_512;
            else
                picPerson.Image = Resources.Female_512;

            klblImageRemove.Visible = (picPerson.ImageLocation != null);

            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;

            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);

            cbxCountries.SelectedIndex = cbxCountries.FindString("Yemen");

            txtFirst.Text = "";
            txtSecond.Text = "";
            txtThird.Text = "";
            txtLast.Text = "";
            txtNationalNo.Text = "";
            rbMale.Checked = true;
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
    
        }
        private void _LoadData()
        {
            _Person = clsPerson.Find(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show("No Person with ID = " + _PersonID, "Person Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }


            txtFirst.Text = _Person.FirstName;
            txtSecond.Text = _Person.SecondName;
            txtThird.Text = _Person.ThirdName;
            txtLast.Text = _Person.LastName;
            txtNationalNo.Text = _Person.NationalNo;
            dtpDateOfBirth.Value = _Person.DateOfBirth;
            if (_Person.Gender == 0)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;

            txtAddress.Text = _Person.Address;
            txtPhone.Text = _Person.Phone;
            txtEmail.Text = _Person.Email;
            cbxCountries.SelectedIndex = cbxCountries.FindString(_Person.CountryInfo.CountryName);

            if(_Person.ImagePath != "")
                picPerson.ImageLocation = _Person.ImagePath;

            klblImageRemove.Visible = (_Person.ImagePath != "");

            

        }
        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();
            if (_Mode == enMode.Update)
                _LoadData();
        }
        private bool _HandlePersonImage()
        {
            if (_Mode == enMode.Update)
            {
                if (_Person.ImagePath != picPerson.ImageLocation)
                {
                    if (_Person.ImagePath != "")
                    {
                        try
                        {
                            File.Delete(_Person.ImagePath);
                        }
                        catch (IOException)
                        {

                        }
                    }
                }
                else
                {
                    return true;
                }
            }
            

            if (picPerson.ImageLocation != null)
                {
                    string SourceImageFile = picPerson.ImageLocation.ToString();

                    if (clsUtil.CopyImageToProjectImagesFolder(ref SourceImageFile))
                    {
                        picPerson.ImageLocation = SourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            
            return true;
        }
        private void ValidateEmptyTextBox(Object sender, CancelEventArgs e)
        {
            TextBox Temp = ((TextBox)sender);
            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This field is required!");
            }
            else
            {
                errorProvider1.SetError(Temp, null);
            }
        }
        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (txtEmail.Text.Trim() == "")
                return;

            if (!clsValidation.ValidateEmail(txtEmail.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format");
            }
            else
            {
                errorProvider1.SetError(txtEmail, null);
            }
        }
        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNationalNo.Text))
            {
                errorProvider1.SetError(txtNationalNo, "The National No is Required");
                e.Cancel = true;
                return;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }

            switch (_Mode)
            {
                case enMode.AddNew:
                    if (clsPerson.IsPersonExist(txtNationalNo.Text.Trim()))
                    {
                        e.Cancel = true;
                        errorProvider1.SetError(txtNationalNo, "National Number is used for anther person");
                    }
                    else
                    {
                        errorProvider1.SetError(txtNationalNo, "");

                    }
                    break;
                case enMode.Update:
                    if (txtNationalNo.Text.Trim() != _Person.NationalNo && clsPerson.IsPersonExist(txtNationalNo.Text.Trim()))
                    {
                        e.Cancel = true;
                        errorProvider1.SetError(txtNationalNo, "National Number is used for anther person");
                    }
                    else
                    {
                        errorProvider1.SetError(txtNationalNo, "");

                    }
                    break;
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

            if (!_HandlePersonImage())
                return;


            _Person.FirstName = txtFirst.Text.Trim();
            _Person.SecondName = txtSecond.Text.Trim();
            _Person.ThirdName = txtThird.Text.Trim();
            _Person.LastName = txtLast.Text.Trim();
            _Person.NationalNo = txtNationalNo.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim();
            _Person.Address = txtAddress.Text.Trim();
            _Person.DateOfBirth = dtpDateOfBirth.Value;

            if (rbMale.Checked)
                _Person.Gender = (short)enGender.Mail;
            else
                _Person.Gender = (short)enGender.Femail;

            _Person.NationalityCountryID = cbxCountries.SelectedIndex + 1;

            if (picPerson.ImageLocation != null)
                _Person.ImagePath = picPerson.ImageLocation;
            else 
                _Person.ImagePath = "";

            if (_Person.Save())
            {
                lblPersonID.Text = _Person.PersonID.ToString();
                // Change form mode to update
                _Mode = enMode.Update;
                lblTitle.Text = "Update Person";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Trigger the event to send data back to the caller form.
                DataBack?.Invoke(this, _Person.PersonID);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (picPerson.ImageLocation == null)
                picPerson.Image = Resources.Male_512;
        }
        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (picPerson.ImageLocation == null)
                picPerson.Image = Resources.Female_512;
        }
        private void klblSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Process the selected file
                string selectedFilePath = openFileDialog1.FileName;
                picPerson.Load(selectedFilePath);
                klblImageRemove.Visible = true;
            }
        }
        private void klblImageRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            picPerson.ImageLocation = null;
            if (rbMale.Checked)
                picPerson.Image = Resources.Male_512;
            else
                picPerson.Image = Resources.Female_512;

            klblImageRemove.Visible = false;

        }


    }
}
