# Cybersecurity Awareness Chatbot - Part 2

## Protecting South African Citizens Through Interactive Cybersecurity Education

This project is an advanced WPF-based cybersecurity awareness chatbot designed to educate South African citizens about modern cybersecurity threats including phishing scams, password attacks, malware, suspicious links, privacy concerns, and unsafe browsing habits.

Part 2 expands on the original command-line chatbot by introducing a modern graphical user interface, improved conversational flow, sentiment detection, memory functionality, typing animations, and enhanced user interaction.

---

## Continuous Integration Status

[.NET Build](https://github.com/YOUR_USERNAME/YOUR_REPOSITORY/actions)

**Screenshot of Successful CI Run:**
(Add your screenshot here after the workflow succeeds.)

---

## Video Presentation

[Watch the demonstration video](YOUR_YOUTUBE_LINK)

[![Watch Demonstration Video](https://img.shields.io/badge/Watch-Video-red)](YOUR_YOUTUBE_LINK)

---

## Project Structure

```
CyberBotV2/
├── Audio/
│   └── welcome_greeting.wav
├── obj/
├── bin/
├── App.xaml
├── App.xaml.cs
├── AssemblyInfo.cs
├── ChatBot.cs
├── CyberBotV2.csproj
├── KeywordResponder.cs
├── logo.txt
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── MemoryStore.cs
├── SentimentDetector.cs
└── README.md

```

---

## Features

### Part 2 - Advanced WPF Chatbot Experience

* **Modern WPF GUI:** Interactive graphical interface with chat bubbles and auto-scrolling conversation window
* **Voice Greeting:** Plays a recorded WAV greeting when the application launches
* **ASCII Art Display:** Displays a cybersecurity-themed logo during startup
* **Splash Screen:** Professional startup sequence before entering the chatbot interface
* **Keyword Recognition:** Detects cybersecurity-related topics such as:

  * Password safety
  * Phishing scams
  * Privacy protection
  * Malware threats
  * Safe browsing
  * Suspicious links
* **Randomized Responses:** Uses dictionaries and response lists to create more natural conversations
* **Conversation Flow Support:** Users can continue discussions using prompts like:

  * “tell me more”
  * “another tip”
  * “help”
* **Memory Functionality:** Remembers:

  * User names
  * User interests
  * Previous discussion topics
* **Sentiment Detection:** Detects emotional cues such as:

  * Worried
  * Curious
  * Frustrated
    and adapts responses accordingly
* **Typing Animation Effect:** Simulates realistic chatbot response typing
* **Improved Error Handling:** Handles:

  * Empty input
  * Invalid responses
  * Unknown questions
    gracefully
* **Enhanced User Experience:** Uses colored UI elements, animations, borders, and organized layouts for readability

---

## How to Run

### Requirements

* Windows Operating System
* Visual Studio 2022
* .NET 8.0 SDK or later
* Speakers/Headphones for voice greeting playback

### Steps

1. Clone this repository
2. Open `CyberBotV2.sln` in Visual Studio 2022
3. Restore NuGet packages if prompted
4. Press `F5` or click the green **Start** button
5. The chatbot will launch with a splash screen, voice greeting, and interactive GUI

---

## Example Interaction

```
╔════════════════════════════════════════════════════╗
║                                                    ║
║              CYBER GUARD ASSISTANT                 ║
║      Interactive Cybersecurity Education Bot       ║
║                                                    ║
╚════════════════════════════════════════════════════╝
```

[Bot]: Welcome to the Cybersecurity Awareness Assistant!
[Bot]: Please enter your name to begin.

[User]: Olwethu

[Bot]: Welcome, Olwethu!
[Bot]: I’m here to help you stay safe online.

[User]: what is phishing?

[Bot]: PHISHING WARNING:
Phishing scams attempt to trick users into revealing
passwords, banking details, or personal information.
Always verify the sender before clicking suspicious links.

[User]: tell me more

[Bot]: Cybercriminals often create fake urgency such as:
"Your account will be suspended immediately!"
This is designed to pressure victims into reacting quickly.

[User]: another tip

[Bot]: Always enable multi-factor authentication whenever possible.

---

## Continuous Integration and Automation

This project uses GitHub Actions for Continuous Integration (CI).

The workflow automatically:

* Restores dependencies
* Builds the application
* Runs tests (if available)

Workflow configuration file:

```
.github/workflows/build.yml
```

---

## Releases

* **v1.0** - Original command-line cybersecurity chatbot
* **v2.0** - Advanced WPF chatbot with GUI and intelligent interaction features

---

## Technologies Used

* C#
* WPF (.NET 8)
* XAML
* GitHub Actions
* Visual Studio 2022

---

## Future Improvements

Potential future enhancements include:

* AI-powered natural language processing
* Database integration for conversation history
* Dark mode UI theme
* Speech-to-text functionality
* Expanded cybersecurity knowledge base
* User authentication and profiles

---

## References

Pieterse, H. 2021. *The Cyber Threat Landscape in South Africa: A 10-Year Review.* The African Journal of Information and Communication, 28(28). doi: https://doi.org/10.23962/10539/32213.

Microsoft Documentation. 2026. *Windows Presentation Foundation (WPF).* Available at: https://learn.microsoft.com/

GitHub Documentation. 2026. *GitHub Actions CI/CD.* Available at: https://docs.github.com/en/actions

ASCII Art Logo adapted from original project assets.

Voice greeting recorded by Olwethu.

---

## Author

Olwethu [Surname]
ST[Student Number]

---

## Date

May 2026
