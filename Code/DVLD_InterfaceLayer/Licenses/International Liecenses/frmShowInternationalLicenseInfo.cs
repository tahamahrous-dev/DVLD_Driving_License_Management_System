using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Full_Project.Licenses
{
    public partial class frmShowInternationalLicenseInfo : Form
    {
        
        public frmShowInternationalLicenseInfo(int InternationalID)
        {
            InitializeComponent();

            ctrlInternatioanlDriverInfo1.LoadInternationalLicenseInfo(InternationalID);
        }
    }
}
