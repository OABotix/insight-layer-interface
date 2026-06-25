using System.Collections.Generic;

namespace CybersecurityChatbotPartTwo
{
    public class TaskManager
    {
        private TaskStorageHelper _storage;
        private ActivityLogger _logger;

        public TaskManager(ActivityLogger logger)
        {
            _storage = new TaskStorageHelper();
            _logger = logger;
        }

        // Adds a new task, logs the action, and returns a confirmation message.
        public string AddTask(string title, string description, string reminder)
        {
            _storage.AddTask(title, description, reminder);
            string logMsg = $"Task added: '{title}'" + (string.IsNullOrEmpty(reminder) ? "" : $" (Reminder: {reminder})");
            _logger.LogAction(logMsg);
            return $"✅ Task added: '{title}'" + (string.IsNullOrEmpty(reminder) ? "" : $" with reminder: {reminder}");
        }

        // Retrieves all tasks from storage.
        public List<CyberTask> GetAllTasks()
        {
            return _storage.LoadTasks();
        }

        // Marks a task as complete, logs the action, and returns confirmation.
        public string MarkComplete(int id)
        {
            _storage.MarkComplete(id);
            _logger.LogAction($"Task #{id} marked as complete");
            return $"✅ Task #{id} marked as complete!";
        }

        // Deletes a task, logs the action, and returns confirmation.
        public string DeleteTask(int id)
        {
            _storage.DeleteTask(id);
            _logger.LogAction($"Task #{id} deleted");
            return $"🗑️ Task #{id} deleted.";
        }
    }
}