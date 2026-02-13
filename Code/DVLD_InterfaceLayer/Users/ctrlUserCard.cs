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
    public partial class ctrlUserCard : UserControl
    {
        private int _UserID;
        private clsUser _User;

        public int UserID
        {
            get { return _UserID; }
        }

        public ctrlUserCard()
        {
            InitializeComponent();
        }

        public void LoadUserInfo(int UserID)
        {
            _UserID = UserID;
            _User = clsUser.FindByUserID(UserID);

            if (_User == null)
            {
                _ResetDefaltValues();
                MessageBox.Show("No User with UserID = " + UserID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FullUserInfo();
        }

        private void _ResetDefaltValues()
        {
            ctrlPersonCard1.ResetPersonInfo();
            lblUserName.Text = "???";
            lblUserID.Text = "???";
            lblIsActve.Text = "???";
        }

        private void _FullUserInfo()
        {
            ctrlPersonCard1.LoadPersonInfo(_User.PersonID);
            lblUserID.Text = _User.UserID.ToString();
            lblUserName.Text = _User.UserName;
            if (_User.IsActive)
                lblIsActve.Text = "Yes";
            else
                lblIsActve.Text = "No";
        }
    }
}
