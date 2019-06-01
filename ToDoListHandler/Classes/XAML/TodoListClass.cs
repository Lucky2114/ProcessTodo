using System;
using System.Windows.Documents;

namespace ToDoListHandler.Classes.JSON
{
    [Serializable()]
    public class TodoListClass
    {
        public FlowDocument document { get; set; }
    }
}
