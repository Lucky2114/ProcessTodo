using System.Windows.Controls;

namespace ToDoListHandler.Classes
{
    class Data_userInterface
    {
        private readonly RichTextBox richTextBox;
        private readonly Data_handler data_Handler;
        private readonly string xamlPath;
        public Data_userInterface(RichTextBox richTextBox, string jsonPath)
        {
            this.richTextBox = richTextBox;
            this.xamlPath = jsonPath;
            this.data_Handler = new Data_handler();
        }

        public void UpdateTextBox()
        {
            try
            {
                richTextBox.Document = data_Handler.GetXmlObject(this.xamlPath).Document;
            }
            catch
            {
                //ignored
            }
        }
    }
}
