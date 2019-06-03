using System;

namespace ProcessTodo.Classes
{
    public static class Constants
    {
        public const string taskSchedFolder = @"\ProcessToDo\";
        public static readonly string todoListDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\processtodo_todolists\\";
    }
}
