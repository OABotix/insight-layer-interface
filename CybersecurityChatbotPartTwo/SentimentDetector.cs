using System;
using System.Collections.Generic;

namespace CybersecurityChatbotPartTwo
{
    public class SentimentDetector
    {
        // Lists of sentiment keywords (Requirement 6)
        private List<string> worriedWords;
        private List<string> curiousWords;
        private List<string> frustratedWords;

        // Encouragement messages (Requirement 6)
        private List<string> encouragementMessages;

        private Random random;

        public SentimentDetector()
        {
            random = new Random();

            // Initialize sentiment word lists
            worriedWords = new List<string> {
                "worried", "scared", "afraid", "concerned", "nervous", "anxious", "fear", "unsafe"
            };

            curiousWords = new List<string> {
                "curious", "interested", "want to learn", "tell me", "explain", "teach", "how do I"
            };

            frustratedWords = new List<string> {
                "frustrated", "annoyed", "confused", "don't understand", "hard", "difficult", "complicated"
            };

            // Encouragement messages
            encouragementMessages = new List<string> {
                "You're taking an important step by learning about cybersecurity!",
                "Everyone starts somewhere - the fact that you're here means you care about staying safe.",
                "Don't worry - cybersecurity can seem complex, but small steps make a big difference.",
                "Remember: every expert was once a beginner. You've got this!",
                "Learning about online safety is the best investment you can make.",
                "You're doing great just by asking questions!"
            };
        }

        // Detect sentiment from user input (Requirement 6)
        public string DetectSentiment(string input)
        {
            string lowerInput = input.ToLower();

            foreach (string word in worriedWords)
            {
                if (lowerInput.Contains(word))
                    return "worried";
            }

            foreach (string word in frustratedWords)
            {
                if (lowerInput.Contains(word))
                    return "frustrated";
            }

            foreach (string word in curiousWords)
            {
                if (lowerInput.Contains(word))
                    return "curious";
            }

            return "neutral";
        }

        // Get empathetic response based on sentiment (Requirement 6)
        public string GetSentimentResponse(string sentiment)
        {
            switch (sentiment)
            {
                case "worried":
                    return "It's completely understandable to feel worried about online threats. " +
                           "The good news is that with the right knowledge, you can protect yourself effectively.\n\n" +
                           GetRandomEncouragement();

                case "frustrated":
                    return "I hear you! Cybersecurity can feel overwhelming sometimes. " +
                           "Let's take it step by step.\n\n" + GetRandomEncouragement();

                case "curious":
                    return "That's great that you're curious! Learning about cybersecurity is the best way to stay safe.\n\n" +
                           GetRandomEncouragement();

                default:
                    return "";
            }
        }

        // Get random encouragement message
        private string GetRandomEncouragement()
        {
            return encouragementMessages[random.Next(encouragementMessages.Count)];
        }
    }
}