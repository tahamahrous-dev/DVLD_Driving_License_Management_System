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
    public partial class frmManageUsers : Form
    {
        private static DataTable _dtAllUsers;
        public frmManageUsers()
        {
            InitializeComponent();
        }

        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            _dtAllUsers = clsUser.GetAllUsers();
            dgvUsers.DataSource = _dtAllUsers;
            cbxFilterBy.SelectedIndex = 0;
            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();

            if (dgvUsers.Rows.Count > 0)
            {
                dgvUsers.Columns[0].HeaderText = "User ID";
                dgvUsers.Columns[0].Width = 100;

                dgvUsers.Columns[1].HeaderText = "Person ID";
                dgvUsers.Columns[1].Width = 100;

                dgvUsers.Columns[2].HeaderText = "Full Name";
                dgvUsers.Columns[2].Width = 350;

                dgvUsers.Columns[3].HeaderText = "User Name";
                dgvUsers.Columns[3].Width = 100;

                dgvUsers.Columns[4].HeaderText = "Is Active";
                dgvUsers.Columns[4].Width = 100;
            }
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frmAdd = new frmAddUpdateUser();
            frmAdd.ShowDialog();
            frmManageUsers_Load(null, null);
        }

        private void tsmAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frmAdd = new frmAddUpdateUser();
            frmAdd.ShowDialog();
            frmManageUsers_Load(null, null);
        }

        private void tsmEdit_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frmUpdate = new frmAddUpdateUser((int)dgvUsers.CurrentRow.Cells[0].Value);
            frmUpdate.ShowDialog();
            frmManageUsers_Load(null, null);
        }

        private void tsmDelete_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            if (clsUser.DeleteUser(UserID))
            {
                MessageBox.Show("User has been deleted successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                frmManageUsers_Load(null, null);
            }
            else
                MessageBox.Show("Person was not deleted because it has data linked to it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void tsmShowDetails_Click(object sender, EventArgs e)
        {
            frmUserInfo userInfo = new frmUserInfo((int)dgvUsers.CurrentRow.Cells[0].Value);
            userInfo.ShowDialog();
            frmManageUsers_Load(null, null);
        }

        private void tsmSendEmail_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature is not implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tsmPhoneCall_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature is not implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch (cbxFilterBy.Text)
            {
                case "User ID":
                    FilterColumn = "UserID";
                    break;

                case "User Name":
                    FilterColumn = "UserName";
                    break;

                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;
            }

            // Reset the filters in case nothing selected or filter value conains nothing
            if (txtFilter.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                lblRecordsCount.Text = _dtAllUsers.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "UserID" || FilterColumn == "PersonID")
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilter.Text);
            else
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text);


            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbxFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxFilterBy.Text == "Is Active")
            {
                txtFilter.Visible = false;
                cbxIsActive.Visible = true;
                cbxIsActive.Focus();
                cbxIsActive.SelectedIndex = 0;
            }
            else
            {
                txtFilter.Visible = (cbxFilterBy.Text != "None");
                cbxIsActive.Visible = false;
                txtFilter.Text = "";
                txtFilter.Focus();
            }
        }

        private void cbxIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue = cbxIsActive.Text;

            switch (FilterValue)
            {
                case "All":
                    break;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;
            }
            if (FilterValue == "All")
                _dtAllUsers.DefaultView.RowFilter = "";
            else
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);

            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
        }

        private void tsmChangePassword_Click(object sender, EventArgs e)
        {
            frmChangePassword frmChangePass = new frmChangePassword((int)dgvUsers.CurrentRow.Cells[0].Value);
            frmChangePass.ShowDialog();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                
            }

            if (cbxFilterBy.Text == "User ID" || cbxFilterBy.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    
    }
}
