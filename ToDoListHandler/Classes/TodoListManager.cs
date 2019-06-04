using Newtonsoft.Json;
using ToDoListHandler.Classes.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32.TaskScheduler;

namespace ToDoListHandler.Classes
{
    public static class TodoListManager
    {
        private const string jsonFileName = "todolists_managed.json";

        public static void UpdateJsonFile(List<TodoList> managedTodoLists)
        {
            string jsonFilePath = FindOrCreateJsonFile();
            string jsonRaw = JsonConvert.SerializeObject(managedTodoLists, Formatting.Indented);
            File.WriteAllText(jsonFilePath, jsonRaw);
        }

        public static List<TodoList> GetListFromJson()
        {
            string jsonFilePath = FindOrCreateJsonFile();
            string json = File.ReadAllText(jsonFilePath);
            return jsonFilePath != "" ? JsonConvert.DeserializeObject<List<TodoList>>(json) : null;
        }

        public static void AddTodoList(TodoList todoList)
        {
            string jsonFilePath = FindOrCreateJsonFile();
            string json = File.ReadAllText(jsonFilePath);
            List<TodoList> todoListOld = jsonFilePath != "" ? JsonConvert.DeserializeObject<List<TodoList>>(json) : null;
            todoListOld.Add(todoList);
            UpdateJsonFile(todoListOld);
        }

        public static TodoList FindTodoListFromJson(string processId)
        {
            List<TodoList> tmpList = GetListFromJson();
            foreach (var item in tmpList)
            {
                if (item.Id.Equals(processId))
                {
                    return item;
                }
            }
            return null;
        }

        private static string FindOrCreateJsonFile()
        {
            string res = "";

            string dir = Constants.todoListDataFolder;
            string[] files = Directory.GetFiles(dir, jsonFileName, SearchOption.AllDirectories);

            if (files.Length > 0)
            {
                res = files.First();
            } else
            {
                string json = JsonConvert.SerializeObject(new List<TodoList>(), Formatting.Indented);
                File.WriteAllText(dir + jsonFileName, json);
                res = dir + jsonFileName;
            }
            return res;
        }

        public static void UpdateJsonFileFromTaskScheduler()
        {
            List<TodoList> todoListsJson = GetListFromJson();
            List<Task> tasks = TaskSched_Communicator.GetProcessTodoTasks();

            foreach (var item in todoListsJson.Reverse<TodoList>())
            {
                //Check if a task is registered with this name
                if (tasks.Where(p => String.Equals(p.Name, item.TaskName, StringComparison.CurrentCulture)).ToArray().Length <= 0)
                {
                    todoListsJson.Remove(item);
                    File.Delete(item.XamlFilePath);
                }
            }
            UpdateJsonFile(todoListsJson);
            
        }
    }
}