using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityChatbotPartTwo
{
    public class ActivityLogger
    {
        private List<string> _log = new List<string>();
        private const int DefaultRecentCount = 10;

        public void LogAction(string action)
        {
            string entry = $"[{DateTime.Now:HH:mm}] {action}";
            _log.Add(entry);
        }

        public List<string> GetRecentLog(int count = DefaultRecentCount)
        {
            if (_log.Count <= count)
                return _log.ToList();
            return _log.Skip(_log.Count - count).ToList();
        }

        public List<string> GetFullLog()
        {
            return _log.ToList();
        }

        public int Count => _log.Count;
    }
}