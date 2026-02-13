using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Full_Project
{
    public class clsGlobal
    {
        public static clsUser CurrentUser;

        public static bool RememberUsernameAndPassword(string Username, string Password)
        {
            string FileName = "RememberMe.txt";
            bool IsRememberMe;

            if (string.IsNullOrEmpty(Username) && string.IsNullOrEmpty(Password))
            {
                File.Delete(FileName);
                IsRememberMe = false;
            }
            else
            { 
                try
                {
                    string LoginData = $"{Username}#{Password}";
                    File.WriteAllText(FileName, LoginData);
                    IsRememberMe = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving login info: " + ex.Message, "Error Saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    IsRememberMe = false;
                }
            }

            return IsRememberMe;
        }

        public static bool GetStoredCredential(ref string UserName, ref string Password)
        {
            string FileName = "RememberMe.txt";
            bool IsRememberMe;

            try
            {
                string[] DataParts = File.ReadAllText(FileName).Split('#');
                if (DataParts.Length == 2)
                {
                    UserName = DataParts[0];
                    Password = DataParts[1];
                }
                    IsRememberMe = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving login info: " + ex.Message, "Error Saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
                IsRememberMe = false;

            }

            return IsRememberMe;
        }

        
    }
}
