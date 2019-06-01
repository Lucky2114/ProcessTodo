using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace ProcessTodo.Classes
{
    class FileSystem
    {
        public bool DeleteTodoListFile(string name)
        {
            bool res = false;
            string tmp = name.Replace("_", "").Replace("[PTD] - ", "") + ".xaml";

            MessageBox.Show(tmp);

            string workingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\processtodo_todolists\\";

            string[] files = Directory.GetFiles(workingDirectory, tmp, SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                try
                {
                    File.Delete(files.First());
                    res = true;
                }
                catch
                {
                    res = false;
                }
            }
            return res;
        }
    }
}
