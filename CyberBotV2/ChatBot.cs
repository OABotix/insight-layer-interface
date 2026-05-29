using System;
using System.Collections.Generic;
using System.IO;
using System.Media;

namespace CybersecurityChatbotPartTwo
{
    public class ChatBot
    {
        // Properties
        public string BotName { get; private set; } = "CyberGuard";

        // Component classes
        private KeywordResponder keywordResponder;
        private SentimentDetector sentimentDetector;
        private MemoryStore memoryStore;

        private Random random;

        public ChatBot()
        {
            random = new Random();
            keywordResponder = new KeywordResponder();
            sentimentDetector = new SentimentDetector();
            memoryStore = new MemoryStore();
        }

        // Play voice greeting when app starts
        public void PlayVoiceGreeting()
        {
            try
            {
                string audioPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Audio",
                    "welcome_greeting.wav");

                if (File.Exists(audioPath))
                {
                    SoundPlayer player = new SoundPlayer(audioPath);
                    player.Play();
                }
            }
            catch (Exception)
            {
                // Silently fail - app continues working
            }
        }

        // Load ASCII logo as string
        public string LoadAsciiLogo()
        {
            try
            {
                string logoPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "logo.txt");

                if (File.Exists(logoPath))
                {
                    return File.ReadAllText(logoPath);
                }
            }
            catch (Exception) { }

            // Fallback logo if file not found
            return @">>======================================================================<<
||                                                                      ||
||                           CYBER GUARD                                ||
||       Informative Cybersecurity Awareness Chatbot for Everyone       ||
||                                                                      ||
>>======================================================================<<";
        }

        // Get welcome message (asks for name)
        public string GetWelcomeMessage()
        {
            return "Hello! Welcome to the Cybersecurity Awareness Bot!\n\n" +
                   "I'm here to help you stay safe online. Before we begin, " +
                   "could you please tell me your name?";
        }

        // Set user's name (for memory)
        public void SetUserName(string name)
        {
            memoryStore.UserName = name;
        }

        // Get user's name
        public string GetUserName()
        {
            return memoryStore.UserName;
        }

        // Check if bot is waiting for name
        public bool IsWaitingForName()
        {
            return string.IsNullOrEmpty(memoryStore.UserName);
        }

        // Process user input
        public string ProcessInput(string userInput)
        {
            string input = userInput?.Trim().ToLower() ?? "";

            // ===== HANDLE EXIT COMMANDS =====
            if (input == "exit" || input == "quit" || input == "goodbye")
            {
                return $"Thank you for chatting with me, {memoryStore.UserName}! Stay safe online. Goodbye!";
            }

            // ===== HANDLE HELP COMMAND =====
            if (input == "help" || input == "what can you do" || input == "topics")
            {
                return GetHelpMessage();
            }

            // ===== DETECT SENTIMENT FIRST =====
            string sentiment = sentimentDetector.DetectSentiment(input);
            string sentimentResponse = "";

            if (sentiment != "neutral")
            {
                sentimentResponse = sentimentDetector.GetSentimentResponse(sentiment);
            }

            // ===== CONVERSATION FLOW: "tell me more" =====
            if (input.Contains("tell me more") || input.Contains("another tip") || input.Contains("explain more"))
            {
                if (!string.IsNullOrEmpty(memoryStore.CurrentTopic))
                {
                    string moreTip = keywordResponder.GetRandomResponse(memoryStore.CurrentTopic);
                    if (moreTip != null)
                    {
                        return $"Here's another tip about {memoryStore.CurrentTopic}:\n\n{moreTip}";
                    }
                }
                return "What topic would you like to learn more about? Try asking about passwords, phishing, or privacy!";
            }

            // ===== RECALL MEMORY =====
            if (input.Contains("remember") || input.Contains("what did I ask") || input.Contains("my interest"))
            {
                if (memoryStore.HasInterest())
                {
                    return $"I remember that you're interested in {memoryStore.UserInterest}. Would you like more tips on this topic?";
                }
                return "I haven't learned about your interests yet. Tell me what cybersecurity topic you'd like to know about!";
            }

            // ===== GREETINGS =====
            if (input.Contains("hello") || input.Contains("hi ") || input.Contains("hey") || input == "hi")
            {
                string[] greetings = {
                    $"Hello {memoryStore.UserName}! 👋",
                    $"Hi there {memoryStore.UserName}! 😊",
                    $"Hey {memoryStore.UserName}! Ready to learn about cybersecurity?"
                };
                return greetings[random.Next(greetings.Length)];
            }

            // ===== HOW ARE YOU =====
            if (input.Contains("how are you"))
            {
                string[] responses = {
                    $"I'm doing great, {memoryStore.UserName}! Thanks for asking.",
                    $"Functioning perfectly, {memoryStore.UserName}! Ready to share cybersecurity tips.",
                    $"All systems operational, {memoryStore.UserName}! What can I help you with?"
                };
                return responses[random.Next(responses.Length)];
            }

            // ===== PURPOSE QUESTION =====
            if (input.Contains("purpose") || input.Contains("what do you do"))
            {
                return $"My purpose is to educate South Africans about cybersecurity threats like phishing, " +
                       $"malware, and scams. I provide practical tips to keep you safe online.";
            }

            // ===== KEYWORD RECOGNITION =====
            string matchedKeyword = keywordResponder.MatchKeyword(input);

            if (matchedKeyword != null)
            {
                // Store the topic in memory for later recall
                memoryStore.CurrentTopic = matchedKeyword;
                memoryStore.UserInterest = keywordResponder.GetDisplayName(matchedKeyword);

                // Get random response for this keyword (Requirement 3)
                string response = keywordResponder.GetRandomResponse(matchedKeyword);

                // Combine sentiment response with keyword response if needed
                if (!string.IsNullOrEmpty(sentimentResponse))
                {
                    return $"{sentimentResponse}\n\n{response}";
                }

                return response;
            }

            // ===== DEFAULT RESPONSE - Error Handling =====
            string[] defaults = {
                $"I'm not sure I understand, {memoryStore.UserName}. Could you rephrase?",
                $"I didn't quite catch that, {memoryStore.UserName}. Try asking about passwords, phishing, or privacy!",
                $"Hmm, I'm not sure about that. Type 'help' to see what topics I can discuss."
            };

            string defaultResponse = defaults[random.Next(defaults.Length)];

            if (!string.IsNullOrEmpty(sentimentResponse))
            {
                return $"{sentimentResponse}\n\n{defaultResponse}";
            }

            return defaultResponse;
        }

        // Get help message
        private string GetHelpMessage()
        {
            return "I can help you with these topics:\n\n" +
                   "• Password safety (try: 'password tips', 'how to create strong passwords')\n" +
                   "• Phishing scams (try: 'what is phishing', 'phishing tips')\n" +
                   "• Privacy (try: 'privacy tips', 'how to protect my privacy')\n" +
                   "• Safe browsing (try: 'safe browsing', 'how to browse safely')\n" +
                   "• Suspicious links (try: 'how to spot fake links')\n" +
                   "• Malware (try: 'what is malware')\n\n" +
                   "You can also ask for 'another tip' to get more information on the same topic!\n" +
                   "Type 'exit' when you're done.";
        }
    }
}
