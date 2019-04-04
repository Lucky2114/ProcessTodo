using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using ToDoListHandler.Classes.JSON;

namespace ToDoListHandler.Classes
{
    class Data_userInterface
    {
        private RichTextBox richTextBox;
        private Data_handler data_Handler;
        private string jsonPath;
        public Data_userInterface(RichTextBox richTextBox, string jsonPath)
        {
            this.richTextBox = richTextBox;
            this.jsonPath = jsonPath;
            this.data_Handler = new Data_handler();
        }

        public void updateDataTable()
        {
            try
            {
                richTextBox.Document = data_Handler.getXmlObject(this.jsonPath).document;
            } catch
            {

            }
        }
    }
}
