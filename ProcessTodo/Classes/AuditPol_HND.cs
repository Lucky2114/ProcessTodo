using System;
using System.Diagnostics;
using System.Windows;

namespace ProcessTodo.Classes
{
    class AuditPol_HND
    {
        private string polCategory = "";
        private string polSubcategory = "";
        public bool IsTrackingPolicySet()
        {
            bool res = false;
            if (SetPolKeys())
            {
                Process pProcess = new Process();
                pProcess.StartInfo.FileName = "auditpol.exe";
                pProcess.StartInfo.Arguments = $"/get /subcategory:\"{polSubcategory}\" /r";
                pProcess.StartInfo.UseShellExecute = false;
                pProcess.StartInfo.CreateNoWindow = true;
                pProcess.StartInfo.RedirectStandardOutput = true;
                pProcess.StartInfo.WorkingDirectory = Environment.SystemDirectory;
                pProcess.Start();
                string rawOutput = pProcess.StandardOutput.ReadToEnd();


                var tmp = rawOutput.Split('\n')[1].Split(',');
                string rRes = tmp[tmp.Length - 2];

                //TODO: Use better check. (Space is bad)
                if (!rRes.Contains(" "))
                    res = true;
            }

            return res;
        }

        private bool SetPolKeys()
        {
            bool res = false;

            if (polCategory.Length > 1 && polSubcategory.Length > 1)
                return true;
            Process pProcess = new Process();
            pProcess.StartInfo.FileName = "auditpol.exe";
            pProcess.StartInfo.Arguments = "/get /category:*";
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.WorkingDirectory = Environment.SystemDirectory;
            pProcess.Start();
            string rawOutput = pProcess.StandardOutput.ReadToEnd();
            pProcess.WaitForExit();

            string[] split1 = rawOutput.Split("\n".ToCharArray());

            try
            {
                string catTemp = split1[39].Split('\\')[0].Replace("\r", "");
                string subCatTemp = split1[40].Split(' ')[2];

                this.polCategory = catTemp;
                this.polSubcategory = subCatTemp;
                res = true;
            }
            catch
            {
                MessageBox.Show("Error in Policy indexing");
            }
            return res;
        }

        public bool SetTrackingPolicy(bool enable)
        {
            bool res = false;



            if (SetPolKeys() && !this.polSubcategory.Equals("") && !this.polSubcategory.Equals(""))
            {
                string whatToSet = "enable";

                //If enable == true then set whatToSet = true, else whatToSet = false
                whatToSet = enable ? "enable" : "disable";

                //Try to update the policy
                Process pProcessSet = new Process();
                pProcessSet.StartInfo.FileName = "auditpol.exe";
                pProcessSet.StartInfo.Arguments = $"/set /category:\"{this.polCategory}\" /subcategory:\"{this.polSubcategory}\" /success:{whatToSet}";
                pProcessSet.StartInfo.UseShellExecute = false;
                pProcessSet.StartInfo.RedirectStandardOutput = true;
                pProcessSet.StartInfo.RedirectStandardError = true;
                pProcessSet.StartInfo.WorkingDirectory = Environment.SystemDirectory;
                pProcessSet.Start();
                string strOutputSet = pProcessSet.StandardOutput.ReadToEnd();
                pProcessSet.WaitForExit();
                string error = pProcessSet.StandardError.ReadToEnd();

                if (error.Equals(""))
                    res = true;
            }

            return res;
        }

    }
}
