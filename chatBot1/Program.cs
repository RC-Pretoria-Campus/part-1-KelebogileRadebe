using System;
using System.Speech.Synthesis;
using System.Threading;

class Program
{
    static SpeechSynthesizer synth = new SpeechSynthesizer();

    static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Initializing Mercy - Your Cybersecurity Assistant...\n");
        Console.ResetColor();

        // Configure voice settings
        synth.SetOutputToDefaultAudioDevice();
        synth.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);

        // Display Mercy ASCII art
        DisplayMercyAsciiArt();

        // Play voice greeting
        Speak("Welcome to Mercy, your personal Cybersecurity Awareness Assistant!");

        // Display welcome message and get user's name
        string userName = GetUserName();

        // Main chat loop
        bool running = true;
        while (running)
        {
            ShowMenu(userName);
            string input = GetUserInput();

            if (input.ToLower() == "6" || input.ToLower() == "exit")
            {
                running = false;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Speak($"Stay safe online, {userName}! Goodbye!");
                Console.WriteLine($"\nStay safe online, {userName}! Goodbye!\n");
                Console.ResetColor();
                continue;
            }

            HandleResponse(input, userName);
        }
    }

    static void DisplayMercyAsciiArt()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"  __  __           _                ");
        Console.WriteLine(@" |  \/  | ___ _ __| | _____ _   _   ");
        Console.WriteLine(@" | |\/| |/ _ \ '__| |/ / __| | | |  ");
        Console.WriteLine(@" | |  | |  __/ |  |   <\__ \ |_| |  ");
        Console.WriteLine(@" |_|  |_|\___|_|  |_|\_\___/\__, |  ");
        Console.WriteLine(@"                             |___/   ");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(@"    _____ _           _   _              ");
        Console.WriteLine(@"   | ____| |_   _ ___| |_(_) ___  _ __   ");
        Console.WriteLine(@"   |  _| | | | | / __| __| |/ _ \| '_ \  ");
        Console.WriteLine(@"   | |___| | |_| \__ \ |_| | (_) | | | | ");
        Console.WriteLine(@"   |_____|_|\__,_|___/\__|_|\___/|_| |_| ");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("========================================================");
        Console.ResetColor();
    }

    static void Speak(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine($"\n[Mercy]: {message}");
        Console.ResetColor();
        synth.Speak(message);
    }

    static void TypewriterEffect(string text, int delayMs)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(delayMs);
        }
        Console.WriteLine();
        Console.ResetColor();
    }

    static string GetUserName()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n════════════════════════════════════════════════════");
        TypewriterEffect("Welcome to Mercy - Your Cybersecurity Assistant", 50);
        Console.WriteLine("════════════════════════════════════════════════════\n");
        Console.ResetColor();

        string name;
        do
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Before we begin, what should I call you? ");
            Console.ResetColor();
            name = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(name))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please enter a valid name.");
                Console.ResetColor();
            }
        } while (string.IsNullOrEmpty(name));

        Speak($"Nice to meet you, {name}! I'm here to help with your cybersecurity questions.");
        return name;
    }

    static void ShowMenu(string userName)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        Console.ResetColor();

        Speak($"How can I help you today, {userName}?");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n1. What's your purpose?");
        Console.WriteLine("2. What can I ask you about?");
        Console.WriteLine("3. Tell me about password safety");
        Console.WriteLine("4. How to recognize phishing attempts?");
        Console.WriteLine("5. Tips for safe browsing");
        Console.WriteLine("6. Exit");

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        Console.ResetColor();
    }

    static string GetUserInput()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\nYour choice: ");
        Console.ResetColor();
        return Console.ReadLine().Trim();
    }

    static void HandleResponse(string input, string userName)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("\n------------------------------------------------");
        Console.ResetColor();

        switch (input.ToLower())
        {
            case "1":
            case "purpose":
                Speak($"My purpose, {userName}, is to educate and raise awareness about cybersecurity best practices to help keep you safe online.");
                break;

            case "2":
            case "topics":
                Speak($"You can ask me about these topics, {userName}: Password security, Phishing scams, Safe browsing practices, and General cybersecurity awareness.");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("- Password security");
                Console.WriteLine("- Phishing scams");
                Console.WriteLine("- Safe browsing practices");
                Console.WriteLine("- General cybersecurity awareness");
                Console.ResetColor();
                break;

            case "3":
            case "password":
                Speak($"Here are some password safety tips, {userName}:");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("- Use long, complex passwords (12+ characters)");
                Console.WriteLine("- Don't reuse passwords across different sites");
                Console.WriteLine("- Consider using a password manager");
                Console.WriteLine("- Enable two-factor authentication where available");
                Console.WriteLine("- Change passwords immediately if a breach is suspected");
                Console.ResetColor();
                break;

            case "4":
            case "phishing":
                Speak($"Phishing attempts often look legitimate, {userName}, but here's how to spot them:");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("- Check the sender's email address carefully");
                Console.WriteLine("- Look for poor grammar and spelling");
                Console.WriteLine("- Hover over links to see the real destination");
                Console.WriteLine("- Be wary of urgent requests for personal information");
                Console.WriteLine("- When in doubt, contact the organization directly");
                Console.ResetColor();
                break;

            case "5":
            case "browsing":
                Speak($"For safe browsing, {userName}, follow these tips:");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("- Look for HTTPS and the padlock icon in your browser");
                Console.WriteLine("- Keep your browser and plugins updated");
                Console.WriteLine("- Use ad-blockers to reduce malicious ads");
                Console.WriteLine("- Be cautious with downloads and extensions");
                Console.WriteLine("- Use a VPN on public Wi-Fi networks");
                Console.ResetColor();
                break;

            default:
                Speak("I didn't quite understand that. Could you rephrase? Please enter a number between 1-6 or the topic name.");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("I didn't quite understand that. Could you rephrase?");
                Console.WriteLine("Please enter a number between 1-6 or the topic name.");
                Console.ResetColor();
                break;
        }

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("------------------------------------------------");
        Console.ResetColor();
    }
}