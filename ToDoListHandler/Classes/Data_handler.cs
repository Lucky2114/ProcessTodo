using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListHandler.Classes.JSON;

namespace ToDoListHandler.Classes
{
    class Data_handler
    {
        public List<TodoListClass> getJsonObject(string jsonPath)
        {
            try
            {
                string json = File.ReadAllText(jsonPath);
                return JsonConvert.DeserializeObject<List<TodoListClass>>(json);
            } catch
            {
                return null;
            }
        }

        public void saveJsonObject(List<TodoListClass> obj, string jsonPath)
        {
            string json = JsonConvert.SerializeObject(obj);
            File.WriteAllText(jsonPath, json);
        }
    }
}
