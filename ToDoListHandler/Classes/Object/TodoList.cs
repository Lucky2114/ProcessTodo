using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessTodo.Classes.Objects
{
    
    public class TodoList
    {
        public string TaskName { get; set; }
        public string DisplayName { get; set; }
        public string XamlFilePath { get; set; }
        public string Id { get; set; }
    }
}
