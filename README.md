 # Cybersecurity Awareness Chatbot - Full Project

## Protecting South African Citizens Through Interactive Cybersecurity Education

This project is an advanced WPF-based cybersecurity awareness chatbot designed to educate South African citizens about modern cybersecurity threats including phishing scams, password attacks, malware, suspicious links, privacy concerns, and unsafe browsing habits.

Part 2 expands on the original command-line chatbot by introducing a modern graphical user interface, improved conversational flow, sentiment detection, memory functionality, typing animations, and enhanced user interaction.

**Part 3** completes the application by adding:
- A **Task Assistant** with persistent JSON storage  
- A **Cybersecurity Quiz** with 12 questions and immediate feedback  
- **Activity Logging** with recent‑entry limits and a *Show More* feature  
- **NLP Simulation** that understands tasks, reminders, quiz, and log requests phrased in multiple ways  

---

## Continuous Integration Status

[.NET Build](https://github.com/OABotix/insight-layer-interface/actions)

**Screenshot of Successful CI Run:**
<img width="838" height="431" alt="Screenshot of successful workflow" src="https://github.com/user-attachments/assets/f1089cc5-e36c-4a06-be06-4fa8ea000775" />

---

## Video Presentation

[Watch the demonstration video](https://youtu.be/BCvy5eG6w0o)

[![Watch Demonstration Video](https://img.shields.io/badge/Watch-Video-red)](https://youtu.be/BCvy5eG6w0o)

---

## Project Structure

```
CyberBotV2/
├── .github/workflows/
│   └── build.yml                 # CI workflow (GitHub Actions)
├── Audio/
│   └── welcome_greeting.wav      # Voice greeting
├── App.xaml / App.xaml.cs
├── AssemblyInfo.cs
├── ChatBot.cs                    # Main logic, NLP intent routing
├── Converters.cs                 # Value converters for XAML bindings
├── CyberBotV2.csproj
├── KeywordResponder.cs           # Keyword matching & synonyms
├── logo.txt                      # ASCII art
├── MainWindow.xaml               # GUI layout (Chat, Tasks, Quiz tabs)
├── MainWindow.xaml.cs            # UI event handlers
├── MemoryStore.cs                # User name / interest memory
├── SentimentDetector.cs          # Worried / curious / frustrated detection
├── ActivityLogger.cs             # Logging with timestamps
├── QuizManager.cs                # Quiz logic (12 questions, scoring)
├── TaskManager.cs                # Business logic for tasks
├── TaskStorageHelper.cs          # JSON read/write (CyberTask model)
├── README.md
└── (bin/ and obj/ – generated on build)

```

---

## Features


### Part 1 - Basic Chatbot Interaction
- **Voice Greeting:** Plays a recorded WAV file welcoming users
- **ASCII Art Display:** Shows a cybersecurity-themed logo on launch
- **Personalized Interaction:** Asks for and remembers the user's name
- **Response System:** Answers questions about:
  - Password safety
  - Phishing scams
  - Safe browsing
  - Suspicious links
- **Input Validation:** Gracefully handles empty or invalid inputs
- **Enhanced UI:** Colored text, borders, typing effect for realism

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

### 🔹 Part 3 Features
- **📋 Task Assistant**
  - Add, view, mark complete, and delete cybersecurity tasks.
  - Each task includes a title, optional description, and optional reminder text.
  - All tasks are stored persistently in a `tasks.json` file (auto‑created in the output folder).
  - The task list loads automatically when the application starts.
- **🎮 Cybersecurity Quiz**
  - 12 questions covering phishing, password safety, HTTPS, public Wi‑Fi, 2FA, malware, social engineering, and more.
  - One question at a time with radio‑button answers.
  - Immediate feedback with **green** (correct) or **red** (incorrect) text and an explanation.
  - Final score report with a motivational message, plus **Retry** and **New Questions** buttons.
- **📝 Activity Log**
  - Records every significant action: tasks added/completed/deleted, quiz starts/completions, NLP intents, and reminders.
  - Timestamps in `[HH:mm]` format.
  - Typing `show activity log` or `what have you done for me?` displays the last 10 entries.
  - If there are more than 10 entries, a *Show More* prompt reveals the full history.
- **🧠 NLP Simulation**
  - Uses keyword detection (`string.Contains()`) and synonym dictionaries to understand varied phrasings.
  - Recognises intents such as *add task*, *remind me*, *start quiz*, and *show activity log* even when phrased differently (e.g., *“create a new task to enable 2FA”*, *“test my knowledge”*).
  - Greatly reduces the default *“I didn’t quite understand that”* responses.

---

## How to Run

### Requirements

* Windows Operating System
* Visual Studio 2022
* .NET 8.0 SDK or later
* Speakers/Headphones for voice greeting playback
* Newtonsoft.Json` 13.0.4+ (for JSON storage)

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

Olwethu Bhiya
ST10497311

---

## Date

May 2026
