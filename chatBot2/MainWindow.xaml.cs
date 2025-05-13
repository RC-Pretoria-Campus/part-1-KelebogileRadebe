using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CybersecurityChatbotWPF
{
    public partial class MainWindow : Window
    {
        private Chatbot chatbot;

        public MainWindow()
        {
            InitializeComponent();
            chatbot = new Chatbot(AppendChat);
            chatbot.StartConversation();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput();
        }

        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessUserInput();
            }
        }

        private void ProcessUserInput()
        {
            string userText = UserInput.Text.Trim();
            if (!string.IsNullOrEmpty(userText))
            {
                AppendChat($"User: {userText}");
                UserInput.Clear();

                string botResponse = chatbot.ProcessInput(userText);
                if (!string.IsNullOrEmpty(botResponse))
                {
                    AppendChat($"Mercy: {botResponse}");
                }
            }
        }

        private void AppendChat(string message)
        {
            ChatHistory.Items.Add(message);
            ChatHistory.ScrollIntoView(ChatHistory.Items[ChatHistory.Items.Count - 1]);
        }
    }

    public class Chatbot
    {
        private Action<string> outputCallback;
        private SpeechSynthesizer synth;

        // Memory
        private string userName = "";
        private HashSet<string> userInterests = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Responses for topics
        private Dictionary<string, List<string>> topicResponses;

        // Sentiment keywords
        private List<string> worriedKeywords = new List<string> { "worried", "anxious", "concerned", "fear" };
        private List<string> curiousKeywords = new List<string> { "curious", "interested", "wonder", "want to know" };
        private List<string> frustratedKeywords = new List<string> { "frustrated", "confused", "lost", "overwhelmed" };

        private Dictionary<string, Action> keywordActions;
        private Random rand = new Random();

        public Chatbot(Action<string> output)
        {
            outputCallback = output;
            synth = new SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();

            InitializeResponses();
            InitializeKeywordActions();
        }

        public void StartConversation()
        {
            outputCallback("Initializing Mercy - Your Cybersecurity Assistant...");
            synth.SpeakAsync("Welcome to Mercy, your personal Cybersecurity Awareness Assistant!");

            // Ask for name
            AskUserName();
        }

        private void AskUserName()
        {
            // In real app, you'd wait for user input
            // For simplicity, assign default name
            userName = "User";

            outputCallback($"Hello, {userName}! How can I assist you today?");
        }

        private void InitializeResponses()
        {
            topicResponses = new Dictionary<string, List<string>>
            {
                ["phishing"] = new List<string>
                {
                    "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                    "Always verify the sender's email address and look for signs of phishing.",
                    "When in doubt, contact the organization directly through official channels."
                },
                ["password"] = new List<string>
                {
                    "Make sure to use strong, unique passwords for each account. Avoid using personal details in your passwords.",
                    "Consider using a reputable password manager to keep track of your passwords.",
                    "Enable two-factor authentication wherever possible for added security."
                },
                ["privacy"] = new List<string>
                {
                    "Review the privacy settings on your social media accounts regularly.",
                    "Be cautious about sharing personal information online.",
                    "Use privacy-focused browsers and VPNs to protect your data."
                }
            };
        }

        private void InitializeKeywordActions()
        {
            keywordActions = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase)
            {
                ["password"] = () =>
                {
                    RespondWithRandom("password");
                    userInterests.Add("password");
                },
                ["scam"] = () =>
                {
                    RespondWithRandom("phishing");
                },
                ["privacy"] = () =>
                {
                    RespondWithRandom("privacy");
                    userInterests.Add("privacy");
                },
                ["phishing"] = () =>
                {
                    RespondWithRandom("phishing");
                },
                ["browsing"] = () =>
                {
                    Speak($"For safe browsing, {userName}, follow these tips:");
                    outputCallback(" - Look for HTTPS and the padlock icon in your browser");
                    outputCallback(" - Keep your browser and plugins updated");
                    outputCallback(" - Use ad-blockers to reduce malicious ads");
                    outputCallback(" - Be cautious with downloads and extensions");
                    outputCallback(" - Use a VPN on public Wi-Fi networks");
                }
            };
        }

        public string ProcessInput(string input)
        {
            string lowerInput = input.ToLower();

            // Check for keyword actions
            foreach (var key in keywordActions.Keys)
            {
                if (lowerInput.Contains(key))
                {
                    keywordActions[key]?.Invoke();
                    return "";
                }
            }

            // Check menu options
            switch (lowerInput)
            {
                case "1":
                case "what's your purpose":
                case "purpose":
                    return $"My purpose, {userName}, is to educate and raise awareness about cybersecurity best practices to help keep you safe online.";
                case "2":
                case "what can i ask you about":
                case "topics":
                    return $"You can ask me about these topics, {userName}: Password safety, Phishing scams, Safe browsing practices, Privacy, and General cybersecurity awareness.";
                case "3":
                case "password":
                    RespondWithRandom("password");
                    userInterests.Add("password");
                    return "";
                case "4":
                case "phishing":
                    RespondWithRandom("phishing");
                    return "";
                case "5":
                case "browsing":
                    return $"For safe browsing, {userName}, follow these tips:\n" +
                        "- Look for HTTPS and the padlock icon in your browser\n" +
                        "- Keep your browser and plugins updated\n" +
                        "- Use ad-blockers to reduce malicious ads\n" +
                        "- Be cautious with downloads and extensions\n" +
                        "- Use a VPN on public Wi-Fi networks";
                default:
                    // Sentiment detection
                    if (ContainsAny(lowerInput, worriedKeywords))
                        return "It's completely understandable to feel that way. Scammers can be very convincing. Let me share some tips to help you stay safe.";
                    if (ContainsAny(lowerInput, curiousKeywords))
                        return "That's great! Feel free to ask me any cybersecurity questions.";
                    if (ContainsAny(lowerInput, frustratedKeywords))
                        return "I understand it can be overwhelming. Let's go through some simple tips to improve your online safety.";
                    return "I'm not sure I understand. Could you try rephrasing or ask about a specific topic?";
            }
        }

        private void RespondWithRandom(string topicKey)
        {
            if (topicResponses.ContainsKey(topicKey))
            {
                var responses = topicResponses[topicKey];
                int index = rand.Next(responses.Count);
                string response = responses[index];
                Speak(response);
            }
            else
            {
                Speak("I'm here to help with cybersecurity topics. Please ask me about password safety, phishing, or privacy.");
            }
        }

        private bool ContainsAny(string input, List<string> keywords)
        {
            foreach (var k in keywords)
                if (input.Contains(k))
                    return true;
            return false;
        }

        private void Speak(string message)
        {
            outputCallback($"[Mercy]: {message}");
            synth.SpeakAsync(message);
        }
    }
}