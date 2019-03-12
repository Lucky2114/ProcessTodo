using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToDoListHandler.Classes.JSON;

namespace ToDoListHandler.Classes
{
    class Data_userInterface
    {
        private DataTable dataTable;
        private Data_handler data_Handler;
        private string jsonPath;
        public Data_userInterface(DataTable dataTable, string jsonPath)
        {
            this.dataTable = dataTable;
            this.jsonPath = jsonPath;
            this.data_Handler = new Data_handler();
        }

        public DataTable updateDataTable()
        {
            List<TodoListClass> obj = data_Handler.getJsonObject(this.jsonPath);

            if (obj != null)
            {
                foreach (var item in obj)
                {
                    var tRow = dataTable.NewRow();
                    tRow["Process"] = item.processFullPath;
                    tRow["Item"] = item.todoItem;
                    dataTable.Rows.Add(tRow);
                }
            } else
            {
                
            }
            return dataTable;
        }
    }
}
