using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CybersecurityChatbotPartTwo
{
    public partial class MainWindow : Window
    {
        private ChatBot chatbot;

        public MainWindow()
        {
            InitializeComponent();

            // Create the chatbot instance
            chatbot = new ChatBot();

            // Load and display ASCII logo
            AsciiLogo.Text = chatbot.LoadAsciiLogo();

            // Play voice greeting
            chatbot.PlayVoiceGreeting();

            // Display welcome message
            AddBotMessage(chatbot.GetWelcomeMessage());

            // Focus on input box
            UserInputBox.Focus();
        }

        // Add user message to chat display
        private void AddUserMessage(string message)
        {
            Border bubble = new Border();
            bubble.Style = (Style)FindResource("UserBubble");

            TextBlock textBlock = new TextBlock();
            textBlock.Text = $"👤 You: {message}";
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Foreground = System.Windows.Media.Brushes.White;

            bubble.Child = textBlock;
            ChatPanel.Children.Add(bubble);

            ScrollToBottom();
        }

        // Add bot message to chat display
        private void AddBotMessage(string message)
        {
            Border bubble = new Border();
            bubble.Style = (Style)FindResource("BotBubble");

            TextBlock textBlock = new TextBlock();
            textBlock.Text = $"🤖 {chatbot.BotName}: {message}";
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Foreground = System.Windows.Media.Brushes.White;

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
        private void ProcessUserMessage()
        {
            string userInput = UserInputBox.Text.Trim();

            // VALIDATION: Check if input is empty (Requirement 7)
            if (string.IsNullOrWhiteSpace(userInput))
            {
                AddBotMessage("You didn't type anything. Please ask me a question!");
                return;
            }

            // Add user message to display
            AddUserMessage(userInput);

            // Clear input box
            UserInputBox.Clear();

            // ========================================
            // HANDLE NAME INPUT (First interaction)
            // ========================================
            if (chatbot.IsWaitingForName())
            {
                chatbot.SetUserName(userInput);
                AddBotMessage($"Nice to meet you, {chatbot.GetUserName()}! I'll remember that.");
                AddBotMessage("I can help you with topics like passwords, phishing, privacy, safe browsing, and malware.");
                AddBotMessage("What would you like to know about? Or type 'help' to see all options.");
                UserInputBox.Focus();
                return;
            }

            // ========================================
            // PROCESS INPUT THROUGH CHATBOT
            // ========================================
            string response = chatbot.ProcessInput(userInput);
            AddBotMessage(response);

            // Update status bar
            StatusText.Text = $"Chatting with {chatbot.GetUserName()} - Ready";

            // Focus back on input box
            UserInputBox.Focus();

            // Check for exit - disable input if user said goodbye (Requirement 7)
            string lowerInput = userInput.ToLower().Trim();
            if (lowerInput == "exit" || lowerInput == "quit" || lowerInput == "goodbye")
            {
                UserInputBox.IsEnabled = false;
                SendButton.IsEnabled = false;
            }
        }
    }
}