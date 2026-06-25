using System;
using System.Collections.Generic;

namespace CybersecurityChatbotPartTwo
{
    public class QuizQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public int CorrectIndex { get; set; }
        public string Explanation { get; set; }
    }

    public class QuizManager
    {
        private List<QuizQuestion> _questions;
        private int _currentIndex = 0;
        private int _score = 0;
        private bool _isActive = false;
        private ActivityLogger _logger;

        public QuizManager(ActivityLogger logger)
        {
            _logger = logger;
            _questions = new List<QuizQuestion>();
            InitializeQuestions();
        }

        private void InitializeQuestions()
        {
            // 12 questions covering phishing, password, browsing, social engineering, 2FA, malware, privacy
            _questions.Add(new QuizQuestion
            {
                Question = "What should you do if you receive an email asking for your password?",
                Options = new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report it as phishing", "D) Ignore it" },
                CorrectIndex = 2,
                Explanation = "Reporting phishing emails helps prevent scams and protects others."
            });
            _questions.Add(new QuizQuestion
            {
                Question = "What makes a strong password?",
                Options = new List<string> { "A) Your birthday", "B) A common word", "C) A mix of uppercase, lowercase, numbers, and symbols", "D) Your pet's name" },
                CorrectIndex = 2,
                Explanation = "Strong passwords use a combination of character types and are at least 12 characters long."
            });
            _questions.Add(new QuizQuestion
            {
                Question = "What does 'https' indicate in a website URL?",
                Options = new List<string> { "A) The site is from a government", "B) The site is secure for data transmission", "C) The site is fast", "D) The site is free" },
                CorrectIndex = 1,
                Explanation = "HTTPS encrypts data between your browser and the website."
            });
            _questions.Add(new QuizQuestion
            {
                Question = "What is a common sign of a phishing email?",
                Options = new List<string> { "A) Personalized greeting", "B) Urgent language demanding immediate action", "C) Known sender", "D) Proper spelling" },
                CorrectIndex = 1,
                Explanation = "Scammers often create urgency to pressure victims into acting quickly."
            });
            _questions.Add(new QuizQuestion
            {
                Question = "True or False: Public Wi-Fi is safe for online banking.",
                Options = new List<string> { "A) True", "B) False" },
                CorrectIndex = 1,
                Explanation = "Public Wi-Fi networks are often unencrypted and can be intercepted by hackers."
            });
            _questions.Add(new QuizQuestion
            {
                Question = "What is Two-Factor Authentication (2FA)?",
                Options = new List<string> { "A) A password manager", "B) An extra security layer requiring two verification methods", "C) A type of virus", "D) A firewall" },
                CorrectIndex = 1,
                Explanation = "2FA adds an extra layer of security by requiring a second form of verification."
            });
            _questions.Add(new QuizQuestion
            {
                Question = "Which is an example of a strong password?",
                Options = new List<string> { "A) password123", "B) John1990", "C) S3cur3#P@ssw0rd!", "D) qwerty" },
                CorrectIndex = 2,
                Explanation = "A strong password includes uppercase, lowercase, numbers, and special characters."
            });
            _questions.Add(new QuizQuestion
            {
                Question = "True or False: You should share your password with IT support over the phone.",
                Options = new List<string> { "A) True", "B) False" },
                CorrectIndex = 1,
                Explanation = "Legitimate organizations will never ask for your password. This is a common social engineering tactic."
            });
            _questions.Add(new QuizQuestion
            {
                Question = "What is malware?",
                Options = new List<string> { "A) A type of hardware", "B) Malicious software designed to harm your system", "C) A security tool", "D) A web browser" },
                CorrectIndex = 1,
                Explanation = "Malware includes viruses, ransomware, spyware, and other malicious software."
            });
            _questions.Add(new QuizQuestion
            {
                Question = "How often should you update your software?",
                Options = new List<string> { "A) Never", "B) Only when asked", "C) Regularly, when updates are available", "D) Once a year" },
                CorrectIndex = 2,
                Explanation = "Regular updates patch security vulnerabilities and protect against known threats."
            });
            _questions.Add(new QuizQuestion
            {
                Question = "What should you do before clicking a link in an email?",
                Options = new List<string> { "A) Click it immediately", "B) Hover to see the actual URL", "C) Forward it to friends", "D) Reply to the sender" },
                CorrectIndex = 1,
                Explanation = "Hovering reveals the real destination, helping you spot fraudulent URLs."
            });
            _questions.Add(new QuizQuestion
            {
                Question = "True or False: Using the same password for multiple accounts is safe.",
                Options = new List<string> { "A) True", "B) False" },
                CorrectIndex = 1,
                Explanation = "If one account is compromised, all accounts using the same password are at risk."
            });
        }

        public void StartQuiz()
        {
            _currentIndex = 0;
            _score = 0;
            _isActive = true;
            _logger.LogAction("Quiz started");
        }

        public bool IsActive => _isActive;
        public bool IsFinished => _currentIndex >= _questions.Count;

        public QuizQuestion GetCurrentQuestion()
        {
            if (_isActive && !IsFinished)
                return _questions[_currentIndex];
            return null;
        }

        public int CurrentQuestionNumber => _currentIndex + 1;
        public int TotalQuestions => _questions.Count;
        public int Score => _score;

        public (bool correct, string explanation, bool finished) SubmitAnswer(int selectedIndex)
        {
            if (!_isActive || IsFinished)
                return (false, "", true);

            var q = _questions[_currentIndex];
            bool correct = selectedIndex == q.CorrectIndex;
            if (correct) _score++;

            string logMsg = $"Quiz: Q{_currentIndex + 1} - {(correct ? "Correct" : "Incorrect")}";
            _logger.LogAction(logMsg);

            _currentIndex++;
            bool finished = IsFinished;
            if (finished)
            {
                _isActive = false;
                _logger.LogAction($"Quiz completed - Score: {_score}/{TotalQuestions}");
                // Return final score message
            }
            return (correct, q.Explanation, finished);
        }

        public string GetFinalMessage()
        {
            double pct = (double)_score / TotalQuestions * 100;
            if (pct >= 80) return "🌟 Excellent! You're a cybersecurity pro!";
            if (pct >= 60) return "👍 Good job! You have solid cybersecurity knowledge.";
            return "📚 Keep learning! Cybersecurity is important for everyone. Review the topics and try again!";
        }
    }
}