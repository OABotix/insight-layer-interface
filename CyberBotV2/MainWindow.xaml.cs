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
            if (lowerInput == "exit" || lowerInput == "quit" || lowerInput == "goodbye")
            {
                UserInputBox.IsEnabled = false;
                SendButton.IsEnabled = false;
            }
        }

        // Window loaded event
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Optional: Any startup animations or focus
            UserInputBox.Focus();
        }
    }
}