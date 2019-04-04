using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;
using ToDoListHandler.Classes.JSON;

namespace ToDoListHandler.Classes
{
    class Data_handler
    {
        public TodoListClass getXmlObject(string XamlPath)
        {
            try
            {
                string xaml = File.ReadAllText(XamlPath);

                TodoListClass r = (TodoListClass)XamlReader.Parse(xaml);
                return r;
            }
            catch 
            {
                return null;
            }
        }

        public void saveXamlObject(TodoListClass obj, string XamlPath)
        {

            string xml = XamlWriter.Save(obj);
            File.WriteAllText(XamlPath, xml);
        }
    }
}
