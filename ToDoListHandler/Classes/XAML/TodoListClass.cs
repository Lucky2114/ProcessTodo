using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ToDoListHandler.Classes.JSON
{
    [Serializable()]
    public class TodoListClass
    {
        public FlowDocument document { get; set; }
    }
}
