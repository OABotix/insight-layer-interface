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

        private TaskManager taskManager;
        private QuizManager quizManager;
        private ActivityLogger activityLogger;

        private Random random;

        public ChatBot(ActivityLogger logger)
        {
            random = new Random();
            keywordResponder = new KeywordResponder();
            sentimentDetector = new SentimentDetector();
            memoryStore = new MemoryStore();

            activityLogger = logger;
            taskManager = new TaskManager(activityLogger);
            quizManager = new QuizManager(activityLogger);
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

            // ===== EXIT =====
            if (input == "exit" || input == "quit" || input == "goodbye")
            {
                activityLogger.LogAction("User ended conversation");
                return $"Thank you for chatting with me, {memoryStore.UserName}! Stay safe online. Goodbye!";
            }

            // ===== HELP =====
            if (input == "help" || input == "what can you do" || input == "topics")
            {
                activityLogger.LogAction("User requested help");
                return GetHelpMessage();
            }

            // ===== 1. TASK INTENT =====
            if (input.Contains("add task") || input.Contains("add a task") ||
                input.Contains("create task") || input.Contains("new task") ||
                input.Contains("enable") || input.Contains("set up"))
            {
                // Extract task title (remove the command prefix)
                string title = input;
                string[] prefixes = { "add task", "add a task", "create task", "new task" };
                foreach (string p in prefixes)
                {
                    if (input.Contains(p))
                    {
                        title = input.Substring(input.IndexOf(p) + p.Length).Trim();
                        break;
                    }
                }
                // If still empty, use a default
                if (string.IsNullOrWhiteSpace(title))
                    title = "New cybersecurity task";

                // Check if a reminder is mentioned (e.g., "remind me in 3 days")
                string reminder = "";
                if (input.Contains("remind"))
                {
                    // Simple extraction: take everything after "remind"
                    int idx = input.IndexOf("remind");
                    if (idx >= 0)
                        reminder = input.Substring(idx).Trim();
                }

                string result = taskManager.AddTask(title, "", reminder);
                activityLogger.LogAction($"NLP: task intent detected from '{userInput}'");
                return result + "\n\nWould you like to set a reminder? (say 'remind me in X days')";
            }

            // ===== 2. REMINDER INTENT =====
            if (input.Contains("remind me") || input.Contains("set a reminder") ||
                input.Contains("reminder") || input.Contains("don't forget"))
            {
                // Extract what to remind about
                string reminderText = input;
                if (input.Contains("remind me"))
                    reminderText = input.Substring(input.IndexOf("remind me") + "remind me".Length).Trim();
                else if (input.Contains("reminder"))
                    reminderText = input.Substring(input.IndexOf("reminder") + "reminder".Length).Trim();

                if (string.IsNullOrWhiteSpace(reminderText))
                    reminderText = "your cybersecurity task";

                activityLogger.LogAction($"Reminder set: '{reminderText}'");
                return $"✅ Reminder set for '{reminderText}' on tomorrow's date (simulated).";
            }

            // ===== 3. QUIZ INTENT =====
            if (input.Contains("start quiz") || input.Contains("take quiz") ||
                input.Contains("quiz me") || input.Contains("play game") ||
                input.Contains("test my knowledge"))
            {
                activityLogger.LogAction("NLP: quiz intent detected");
                if (!quizManager.IsActive)
                {
                    quizManager.StartQuiz();
                    var q = quizManager.GetCurrentQuestion();
                    if (q != null)
                        return $"🎮 Starting quiz!\n\n{q.Question}\n\n" + string.Join("\n", q.Options) + "\n\nEnter the letter (A, B, C, D) of your answer.";
                    else
                        return "Quiz is unavailable.";
                }
                else
                {
                    return "You're already in a quiz! Answer the current question.";
                }
            }

            // ===== 4. ACTIVITY LOG INTENT =====
            if (input.Contains("show activity log") || input.Contains("what have you done") ||
                input.Contains("what did you do") || input.Contains("show log") ||
                input.Contains("recent actions"))
            {
                var recent = activityLogger.GetRecentLog(10);
                if (recent.Count == 0)
                    return "📝 No activity logged yet. Start a conversation or use some features!";

                string response = "📝 **Recent Activity Log:**\n\n";
                int i = 1;
                foreach (string entry in recent)
                {
                    response += $"  {i}. {entry}\n";
                    i++;
                }
                if (activityLogger.Count > 10)
                    response += $"\n(You have {activityLogger.Count - 10} more entries. Type 'show more' to see them.)";
                else
                    response += "\n(Full log shown.)";
                return response;
            }

            // ===== 5. SHOW MORE LOG =====
            if (input.Contains("show more") && activityLogger.Count > 10)
            {
                var full = activityLogger.GetFullLog();
                string response = "📝 **Full Activity Log:**\n\n";
                int i = 1;
                foreach (string entry in full)
                {
                    response += $"  {i}. {entry}\n";
                    i++;
                }
                return response;
            }

            // ===== 6. QUIZ ANSWER HANDLING =====
            if (quizManager.IsActive && !quizManager.IsFinished)
            {
                // Map letter input (A, B, C, D) or number to index
                int? selected = null;
                string clean = input.Trim().ToUpper();
                if (clean.Length == 1 && clean[0] >= 'A' && clean[0] <= 'D')
                    selected = clean[0] - 'A';
                else if (int.TryParse(input, out int num) && num >= 1 && num <= 4)
                    selected = num - 1;

                if (selected.HasValue)
                {
                    var (correct, explanation, finished) = quizManager.SubmitAnswer(selected.Value);
                    string feedback = correct ? "✅ Correct!" : "❌ Incorrect.";
                    feedback += $"\n\n{explanation}\n";
                    if (finished)
                    {
                        string finalMsg = quizManager.GetFinalMessage();
                        return $"{feedback}\n\n🎉 **Quiz Complete!**\nScore: {quizManager.Score}/{quizManager.TotalQuestions}\n\n{finalMsg}";
                    }
                    else
                    {
                        var next = quizManager.GetCurrentQuestion();
                        return $"{feedback}\n\n**Next Question:**\n{next.Question}\n\n" +
                               string.Join("\n", next.Options) + "\n\nEnter your answer (A-D).";
                    }
                }
                else
                {
                    return "Please enter the letter (A, B, C, D) of your answer.";
                }
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
