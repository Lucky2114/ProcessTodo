using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.TaskScheduler;

namespace ProcessTodo.Classes
{
    class TaskSched_Handler
    {
        //Handles the events of external programms. Uses the inbuilt Windows Task Scheduler, therefore application doesn't need to run in the background
        //Make sure to enable proccess creation logging

        public bool createTask(string processPath, string taskName)
        {
            bool res = false;
            using (TaskService ts = new TaskService())
            {
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "This Task in generated from ProcessToDo. It calls the ToDo-List that is set up for this this programm: " + processPath;

                EventTrigger trigger = new EventTrigger();
                trigger.Subscription = "<QueryList>\n" +
                "  <Query Id=\"0\" Path=\"Security\">\n" +
                "    <Select Path=\"Security\">\n" +
                "     *[System[Provider[@Name='Microsoft-Windows-Security-Auditing'] and Task = 13312 and (band(Keywords,9007199254740992)) and (EventID=4688)]] \n" +
                "   and \n" +
                "     *[EventData[Data[@Name='NewProcessName'] and (Data='" + processPath + "')]]\n" +
                "    </Select>\n" +
                "  </Query>\n" +
                "</QueryList>";
                td.Triggers.Add(trigger);
                
                string todoListHandler = findTodoListHandlerExe();

                td.Actions.Add(new ExecAction(todoListHandler, "caller=" + processPath, null));

                Microsoft.Win32.TaskScheduler.Task t = ts.RootFolder.RegisterTaskDefinition(Constants.taskSchedFolder + taskName, td);
                if (t.Enabled)
                    res = true;

                //Now everytime "processPath" is opend, the ToDoListHandler is called with "processPath" as parameter.
            }
            return res;
        }

        private string findTodoListHandlerExe()
        {
            string res = "";

            string workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string[] files = Directory.GetFiles(workingDirectory, "ToDoListHandler.exe", SearchOption.AllDirectories);

            if (files.Length > 0)
            {
                res = files.First();
            }
            return res;
        }

        public void deleteTask(string taskFullName)
        {
            using (TaskService ts = new TaskService())
            {
                ts.RootFolder.DeleteTask(Constants.taskSchedFolder + taskFullName);
            }
        }

        public List<Microsoft.Win32.TaskScheduler.Task> getTasks()
        {
            List<Microsoft.Win32.TaskScheduler.Task> tasks = new List<Microsoft.Win32.TaskScheduler.Task>();
            using (TaskService ts = new TaskService())
            {
                foreach (Microsoft.Win32.TaskScheduler.Task t in ts.AllTasks)
                {
                    if (t.Name.StartsWith("[PTD]"))
                        tasks.Add(t);
                }
            }

            return tasks;
        }

        public bool runTask(string taskFullName)
        {
            bool res = false;
            using (TaskService ts = new TaskService())
            {
                Microsoft.Win32.TaskScheduler.Task task = ts.FindTask(taskFullName);
                if (task != null)
                {
                    task.Run();
                    res = true;
                }

                return res;
            }

        }

    }
}
