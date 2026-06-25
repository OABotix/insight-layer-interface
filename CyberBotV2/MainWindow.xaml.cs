using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace CybersecurityChatbotPartTwo
{
    public partial class MainWindow : Window
    {
        private ChatBot chatbot;
        private TaskManager taskManager;
        private QuizManager quizManager;
        private ActivityLogger activityLogger;
        private DispatcherTimer typingTimer;
        private string currentTypingMessage = "";
        private int currentCharIndex = 0;
        private TextBlock currentTypingTextBlock;
        private Border currentTypingBubble;

        public MainWindow()
        {
            InitializeComponent();

            // Create the chatbot instance
            chatbot = new ChatBot();
            activityLogger = new ActivityLogger();
            taskManager = new TaskManager(activityLogger);
            quizManager = new QuizManager(activityLogger);

            // Load ASCII logo for splash screen
            SplashLogo.Text = chatbot.LoadAsciiLogo();

            // Start voice greeting and then show interface
            PlayVoiceAndStart();
        }

        // Play voice greeting, wait for it to finish, then show main interface
        private async void PlayVoiceAndStart()
        {
            // Play voice greeting asynchronously
            await Task.Run(() => chatbot.PlayVoiceGreeting());

            // Update loading status
            LoadingStatus.Text = "Voice greeting! Loading chat...";
            await Task.Delay(500);

            // Hide loading overlay, show main interface
            LoadingOverlay.Visibility = Visibility.Collapsed;
            MainInterface.Visibility = Visibility.Visible;

            // Load ASCII logo in main interface
            AsciiLogo.Text = chatbot.LoadAsciiLogo();

            // Display welcome message with typing effect
            await TypeMessageWithDelay(chatbot.GetWelcomeMessage());

            // Focus on input box
            UserInputBox.Focus();
        }

        // Type a message letter by letter (Typing Effect)
        private async Task TypeMessageWithDelay(string message, int delayMs = 30)
        {
            // Create bot message bubble
            Border bubble = new Border();
            bubble.Style = (Style)FindResource("BotBubble");

            TextBlock textBlock = new TextBlock();
            textBlock.Text = $"🤖 {chatbot.BotName}: ";
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Foreground = Brushes.White;

            bubble.Child = textBlock;
            ChatPanel.Children.Add(bubble);
            ScrollToBottom();

            // Store for typing animation
            currentTypingTextBlock = textBlock;
            currentTypingMessage = message;
            currentCharIndex = 0;

            // Type each character with delay
            for (int i = 0; i < message.Length; i++)
            {
                currentTypingTextBlock.Text += message[i];
                await Task.Delay(delayMs);
                ScrollToBottom();
            }
        }

        // Alternative: Type message and then remove the "typing" indicator
        public async void AddBotMessageWithTyping(string message)
        {
            await TypeMessageWithDelay(message, 25);
        }

        // Add user message immediately (no typing effect needed)
        private void AddUserMessage(string message)
        {
            Border bubble = new Border();
            bubble.Style = (Style)FindResource("UserBubble");

            TextBlock textBlock = new TextBlock();
            textBlock.Text = $"👤 You: {message}";
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Foreground = Brushes.White;

            bubble.Child = textBlock;
            ChatPanel.Children.Add(bubble);
            ScrollToBottom();
        }

        // Add bot message with typing effect (public version)
        private void AddBotMessage(string message)
        {
            // This is called for immediate messages (like errors, quick responses)
            // Use typing effect for better UX
            _ = TypeMessageWithDelay(message, 25);
        }

        // Add immediate message without typing effect (for system messages)
        private void AddBotMessageImmediate(string message)
        {
            Border bubble = new Border();
            bubble.Style = (Style)FindResource("BotBubble");

            TextBlock textBlock = new TextBlock();
            textBlock.Text = $"🤖 {chatbot.BotName}: {message}";
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Foreground = Brushes.White;

            bubble.Child = textBlock;
            ChatPanel.Children.Add(bubble);
            ScrollToBottom();
        }

        // Auto-scroll to bottom of chat
        private void ScrollToBottom()
        {
            ChatScrollViewer.ScrollToBottom();
        }

        // Handle Send button click
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserMessage();
        }

        // Handle Enter key press
        private void UserInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessUserMessage();
            }
        }

        // Main processing method - calls the ChatBot class
        private async void ProcessUserMessage()
        {
            string userInput = UserInputBox.Text.Trim();

            // VALIDATION: Check if input is empty
            if (string.IsNullOrWhiteSpace(userInput))
            {
                await TypeMessageWithDelay("You didn't type anything. Please ask me a question!", 25);
                return;
            }

            // Add user message to display (immediate, no typing needed)
            AddUserMessage(userInput);

            // Clear input box
            UserInputBox.Clear();

            // ===== HANDLE NAME INPUT (First interaction) =====
            if (chatbot.IsWaitingForName())
            {
                chatbot.SetUserName(userInput);
                await TypeMessageWithDelay($"Nice to meet you, {chatbot.GetUserName()}! I'll remember that.", 25);
                await TypeMessageWithDelay("I can help you with topics like passwords, phishing, privacy, safe browsing, and malware.", 25);
                await TypeMessageWithDelay("What would you like to know about? Or type 'help' to see all options.", 25);
                UserInputBox.Focus();
                return;
            }

            // ===== PROCESS INPUT THROUGH CHATBOT =====
            string response = chatbot.ProcessInput(userInput);

            // Check if this is a help message or long response
            if (userInput.ToLower().Trim() == "help" || response.Length > 200)
            {
                await TypeMessageWithDelay(response, 20);
            }
            else
            {
                await TypeMessageWithDelay(response, 25);
            }

            // Update status bar
            StatusText.Text = $"Chatting with {chatbot.GetUserName()} - Ready";

            // Focus back on input box
            UserInputBox.Focus();

            // Check for exit - disable input if user said goodbye
            string lowerInput = userInput.ToLower().Trim();
            if (lowerInput != "exit" && lowerInput != "quit" && lowerInput != "goodbye")
            {
                UserInputBox.IsEnabled = true;
                SendButton.IsEnabled = true;
            }
        }

        // ---- TASK HANDLERS ----
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TaskTitleBox.Text.Trim();
            string desc = TaskDescBox.Text.Trim();
            string reminder = TaskReminderBox.Text.Trim();
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Please enter a task title.", "Missing Info", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            taskManager.AddTask(title, desc, reminder);
            RefreshTaskList();
            TaskTitleBox.Clear();
            TaskDescBox.Clear();
            TaskReminderBox.Clear();
        }

        private void CompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is CyberTask selected)
            {
                taskManager.MarkComplete(selected.Id);
                RefreshTaskList();
            }
            else
                MessageBox.Show("Select a task to mark complete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is CyberTask selected)
            {
                taskManager.DeleteTask(selected.Id);
                RefreshTaskList();
            }
            else
                MessageBox.Show("Select a task to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RefreshTaskList()
        {
            TaskListBox.ItemsSource = null;
            TaskListBox.ItemsSource = taskManager.GetAllTasks();
        }

        // ---- QUIZ HANDLERS ----
        private void QuizStartButton_Click(object sender, RoutedEventArgs e)
        {
            quizManager.StartQuiz();
            QuizStartButton.Visibility = Visibility.Collapsed;
            QuizSubmitButton.Visibility = Visibility.Visible;
            QuizNextButton.Visibility = Visibility.Collapsed;
            ShowCurrentQuizQuestion();
        }

        private void RegenerateQuizQuestions()
        {
            // Reset quiz and reshuffle questions
            quizManager = new QuizManager(activityLogger);
            MessageBox.Show("🔄 Questions regenerated! Click 'Start Quiz' to try again.", "Quiz Ready", MessageBoxButton.OK, MessageBoxImage.Information);
            QuizStartButton.Visibility = Visibility.Visible;
            QuizFeedbackText.Text = "";
            QuizQuestionText.Text = "Press 'Start Quiz' to begin!";
            QuizScoreText.Text = "Score: 0/0";
        }

        private void ShowCurrentQuizQuestion()
        {
            var q = quizManager.GetCurrentQuestion();
            if (q == null)
            {
                // Quiz finished - Show Report
                QuizQuestionText.Text = "🎉 Quiz Complete!";
                QuizScoreText.Text = $"Final Score: {quizManager.Score}/{quizManager.TotalQuestions}";

                // Show final message with color
                string finalMsg = quizManager.GetFinalMessage();
                string emoji = quizManager.Score >= 8 ? "🌟" : "📚";

                QuizFeedbackText.Text = $"{emoji} {finalMsg}";
                QuizFeedbackText.Foreground = new SolidColorBrush(quizManager.Score >= 8 ? Colors.LightGreen : Colors.Gold);

                QuizOptionsPanel.Children.Clear();
                QuizSubmitButton.Visibility = Visibility.Collapsed;
                QuizNextButton.Visibility = Visibility.Collapsed;
                QuizStartButton.Visibility = Visibility.Collapsed;

                // Add Retry and Regenerate buttons
                StackPanel buttonPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 10, 0, 0)
                };

                Button retryBtn = new Button
                {
                    Content = "🔁 Retry Quiz",
                    Background = new SolidColorBrush(Color.FromRgb(74, 144, 217)),
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 10, 0),
                    Padding = new Thickness(15, 8, 15, 8)
                };
                retryBtn.Click += (s, args) => { QuizStartButton_Click(s, args); };

                Button regenerateBtn = new Button
                {
                    Content = "🔄 New Questions",
                    Background = new SolidColorBrush(Color.FromRgb(74, 144, 217)),
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    Padding = new Thickness(15, 8, 15, 8)
                };
                regenerateBtn.Click += (s, args) => { RegenerateQuizQuestions(); };

                buttonPanel.Children.Add(retryBtn);
                buttonPanel.Children.Add(regenerateBtn);

                // Add to grid (find the grid parent)
                var parentGrid = QuizOptionsPanel.Parent as Grid;
                if (parentGrid != null)
                {
                    Grid.SetRow(buttonPanel, 3);
                    Grid.SetColumnSpan(buttonPanel, 2);
                    parentGrid.Children.Add(buttonPanel);
                }

                return;
            }

            QuizQuestionText.Text = $"Q{quizManager.CurrentQuestionNumber}/{quizManager.TotalQuestions}: {q.Question}";
            QuizScoreText.Text = $"Score: {quizManager.Score}";
            QuizOptionsPanel.Children.Clear();
            // Create radio buttons for each option
            for (int i = 0; i < q.Options.Count; i++)
            {
                var rb = new System.Windows.Controls.RadioButton
                {
                    Content = q.Options[i],
                    Tag = i,
                    Foreground = System.Windows.Media.Brushes.White,
                    Margin = new System.Windows.Thickness(5)
                };
                QuizOptionsPanel.Children.Add(rb);
            }
            QuizFeedbackText.Text = "";
            QuizSubmitButton.Visibility = Visibility.Visible;
            QuizNextButton.Visibility = Visibility.Collapsed;
        }

        private void QuizSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // Find selected radio button
            System.Windows.Controls.RadioButton selected = null;
            foreach (var child in QuizOptionsPanel.Children)
            {
                if (child is System.Windows.Controls.RadioButton rb && rb.IsChecked == true)
                {
                    selected = rb;
                    break;
                }
            }
            if (selected == null)
            {
                QuizFeedbackText.Text = "Please select an answer.";
                return;
            }

            int selectedIndex = (int)selected.Tag;
            var (correct, explanation, finished) = quizManager.SubmitAnswer(selectedIndex);

            // Color the feedback text
            if (correct)
            {
                QuizFeedbackText.Foreground = new SolidColorBrush(Colors.LightGreen);
                QuizFeedbackText.Text = "✅ Correct!\n" + explanation;
            }
            else
            {
                QuizFeedbackText.Foreground = new SolidColorBrush(Colors.IndianRed);
                QuizFeedbackText.Text = "❌ Incorrect.\n" + explanation;
            }

            QuizSubmitButton.Visibility = Visibility.Collapsed;
            if (!finished)
            {
                QuizNextButton.Visibility = Visibility.Visible;
            }
            else
            {
                // Quiz finished
                QuizQuestionText.Text = "🎉 Quiz complete!";
                QuizScoreText.Text = $"Final Score: {quizManager.Score}/{quizManager.TotalQuestions}\n\n{quizManager.GetFinalMessage()}";
                QuizOptionsPanel.Children.Clear();
                QuizNextButton.Visibility = Visibility.Collapsed;
                QuizStartButton.Visibility = Visibility.Visible;
            }
        }

        private void QuizNextButton_Click(object sender, RoutedEventArgs e)
        {
            QuizNextButton.Visibility = Visibility.Collapsed;
            ShowCurrentQuizQuestion();
            QuizSubmitButton.Visibility = Visibility.Visible;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // When switching to Chat tab, focus the input box
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is TabItem selected)
            {
                if (selected.Header.ToString().Contains("Chat"))
                {
                    UserInputBox.Focus();
                }
            }
        }

        // Window loaded event
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshTaskList(); // Load tasks from JSON
            UserInputBox.Focus();
        }
    }
}