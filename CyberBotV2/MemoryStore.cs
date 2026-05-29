using System;

namespace CybersecurityChatbotPartTwo
{
    public class MemoryStore
    {
        // User information
        public string UserName { get; set; }
        public string UserInterest { get; set; }
        public string CurrentTopic { get; set; }

        public MemoryStore()
        {
            UserName = "";
            UserInterest = "";
            CurrentTopic = "";
        }

        // Check if user has an interest stored
        public bool HasInterest()
        {
            return !string.IsNullOrEmpty(UserInterest);
        }

        // Clear all memory (for reset functionality if needed)
        public void ClearMemory()
        {
            UserName = "";
            UserInterest = "";
            CurrentTopic = "";
        }
    }
}