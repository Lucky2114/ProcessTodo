using System.IO;
using System.Windows.Markup;
using ToDoListHandler.Classes.JSON;

namespace ToDoListHandler.Classes
{
    class Data_handler
    {
        public TodoListClass GetXmlObject(string XamlPath)
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

        public void SaveXamlObject(TodoListClass obj, string XamlPath)
        {
            string xml = XamlWriter.Save(obj);
            File.WriteAllText(XamlPath, xml);
        }
    }
}
