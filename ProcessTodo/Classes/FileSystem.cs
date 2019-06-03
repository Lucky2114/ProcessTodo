using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace ProcessTodo.Classes
{
    public static class FileSystem
    {
        public static bool DeleteTodoListFile(string xamlFilePath)
        {
            bool res = false;
            try
            {
                File.Delete(xamlFilePath);
                res = true;
            }
            catch
            {
                res = false;
            }

            return res;
        }

        public static string FindTodoListHandlerExe()
        {
            string res = "";

            string workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string[] files = Directory.GetFiles(workingDirectory, "ToDoListHandler.exe", SearchOption.AllDirectories);

            if (files.Length > 0)
            {
                res = files.First();
            }
            return res;
        }
    }
}
