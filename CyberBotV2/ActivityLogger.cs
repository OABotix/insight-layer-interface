using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityChatbotPartTwo
{
    // Manages the activity log, storing timestamped actions
    public class ActivityLogger
    {
        private List<string> _log = new List<string>();
        private const int DefaultRecentCount = 10;

        // Adds a new action to the log with the current time
        public void LogAction(string action)
        {
            string entry = $"[{DateTime.Now:HH:mm}] {action}";
            _log.Add(entry);
        }
         
        // Returns the most recent entries (10)
        public List<string> GetRecentLog(int count = DefaultRecentCount)
        {
            if (_log.Count <= count)
                return _log.ToList();
            return _log.Skip(_log.Count - count).ToList();
        }

        // Returns the entire log history
        public List<string> GetFullLog()
        {
            return _log.ToList();
        }

        public int Count => _log.Count;
    }
}