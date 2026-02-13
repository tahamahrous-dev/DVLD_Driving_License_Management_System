using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace DVLD_Full_Project
{
    public class clsUtil
    {

        public static bool CreateFolderIfDoesNotExist(string FileName)
        {
            if (!Directory.Exists(FileName))
            {
                Directory.CreateDirectory(FileName);
            }

            return true;
        }

        public static string GenerateGUID()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }

        public static string ReplaceFileNameWithGUID(string FileName)
        {
            FileInfo fi = new FileInfo(FileName);
            string extn = fi.Extension;

            return GenerateGUID() + extn;
        }

        public static bool CopyImageToProjectImagesFolder(ref string sourceFile)
        {
            string DestinationFolder = @"D:\DVLD_People_Images\";
            if (!CreateFolderIfDoesNotExist(DestinationFolder))
            {
                return false;
            }

            string destinationFile = DestinationFolder + ReplaceFileNameWithGUID(sourceFile);

            try
            {
                File.Copy(sourceFile, destinationFile, true);

            }
            catch (IOException iox)
            {
                MessageBox.Show(iox.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            sourceFile = destinationFile;
            return true;
        }

    }
}
