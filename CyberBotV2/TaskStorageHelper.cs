using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace CybersecurityChatbotPartTwo
{
    public class CyberTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Reminder { get; set; }
        public bool IsComplete { get; set; }
        public string CreatedAt { get; set; }
    }

    public class TaskStorageHelper
    {
        private const string FilePath = "tasks.json";

        public List<CyberTask> LoadTasks()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string json = File.ReadAllText(FilePath);
                    return JsonConvert.DeserializeObject<List<CyberTask>>(json) ?? new List<CyberTask>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadTasks error: {ex.Message}");
            }
            return new List<CyberTask>();
        }

        public void SaveTasks(List<CyberTask> tasks)
        {
            try
            {
                string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SaveTasks error: {ex.Message}");
            }
        }

        public void AddTask(string title, string description, string reminder)
        {
            var tasks = LoadTasks();
            int newId = tasks.Count > 0 ? tasks[^1].Id + 1 : 1;
            tasks.Add(new CyberTask
            {
                Id = newId,
                Title = title,
                Description = description ?? "",
                Reminder = reminder ?? "",
                IsComplete = false,
                CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            });
            SaveTasks(tasks);
        }

        public void MarkComplete(int id)
        {
            var tasks = LoadTasks();
            var task = tasks.Find(t => t.Id == id);
            if (task != null)
            {
                task.IsComplete = true;
                SaveTasks(tasks);
            }
        }

        public void DeleteTask(int id)
        {
            var tasks = LoadTasks();
            tasks.RemoveAll(t => t.Id == id);
            SaveTasks(tasks);
        }
    }
}