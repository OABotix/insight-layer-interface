using System;
using System.Collections.Generic;

namespace CybersecurityChatbotPartTwo
{
    public class KeywordResponder
    {
        // Dictionary for keyword - list of responses
        private Dictionary<string, List<string>> keywordResponses;

        // Dictionary for keyword - display name (for memory)
        private Dictionary<string, string> keywordDisplayNames;

        // Dictionary for keyword - list of synonyms
        private Dictionary<string, List<string>> keywordSynonyms;

        private Random random;

        public KeywordResponder()
        {
            random = new Random();
            keywordResponses = new Dictionary<string, List<string>>();
            keywordSynonyms = new Dictionary<string, List<string>>();
            keywordDisplayNames = new Dictionary<string, string>();
            InitializeResponses();
        }

        private void InitializeResponses()
        {
            // ===== PASSWORD RESPONSEs =====
            keywordResponses.Add("password", new List<string> {
                "PASSWORD SAFETY: Use a unique, complex password for each account. A strong password should be at least 12 characters long and include uppercase, lowercase, numbers, and symbols.",
                "PASSWORD SAFETY: Consider using a password manager to keep track of all your passwords securely. Never write passwords on sticky notes!",
                "PASSWORD SAFETY: Enable Two-Factor Authentication (2FA) whenever possible. This adds an extra layer of security to your accounts.",
                "PASSWORD SAFETY: Never share your passwords with anyone - not even with someone claiming to be from IT support."
            });
            keywordDisplayNames.Add("password", "password safety");

            // ===== PHISHING RESPONSES =====
            keywordResponses.Add("phish", new List<string> {
                "PHISHING WARNING: Phishing emails try to trick you into revealing personal information. NEVER click links in suspicious emails.",
                "PHISHING WARNING: Check the sender's email address carefully - scammers often use addresses that look similar to real ones.",
                "PHISHING WARNING: If an email creates urgency ('Your account will be closed immediately!'), it's likely a scam.",
                "PHISHING WARNING: Hover over links before clicking to see where they really go. Don't trust shortened URLs from unknown sources."
            });
            keywordDisplayNames.Add("phish", "phishing awareness");

            // ===== SCAM RESPONSES =====
            keywordResponses.Add("scam", new List<string> {
                "SCAM ALERT: Scammers often pretend to be from banks, the government, or tech support. Hang up and call the official number.",
                "SCAM ALERT: Never send money to someone you've only met online - romance scams are very common in South Africa.",
                "SCAM ALERT: If something seems too good to be true (lottery winnings, huge discounts), it probably is a scam."
            });
            keywordDisplayNames.Add("scam", "scam awareness");

            // ===== PRIVACY RESPONSES =====
            keywordResponses.Add("privacy", new List<string> {
                "PRIVACY TIP: Review your privacy settings on social media regularly. Limit what the public can see.",
                "PRIVACY TIP: Be careful about what personal information you share online - your address, phone number, and ID number are valuable to criminals.",
                "PRIVACY TIP: South Africa's POPIA (Protection of Personal Information Act) gives you rights over your personal data."
            });
            keywordDisplayNames.Add("privacy", "privacy protection");

            // ===== BROWSING RESPONSES =====
            keywordResponses.Add("brows", new List<string> {
                "SAFE BROWSING: Always look for 'https://' and the padlock icon before entering sensitive information online.",
                "SAFE BROWSING: Avoid using public Wi-Fi for banking or shopping - use your mobile data instead for sensitive transactions.",
                "SAFE BROWSING: Keep your browser updated to protect against security vulnerabilities."
            });
            keywordDisplayNames.Add("brows", "safe browsing");

            // ===== LINK RESPONSES =====
            keywordResponses.Add("link", new List<string> {
                "SUSPICIOUS LINKS: Hover over links before clicking to see the actual URL. Watch for misspellings like 'amaz0n.com'.",
                "SUSPICIOUS LINKS: When in doubt, type the website address directly into your browser instead of clicking links in emails.",
                "SUSPICIOUS LINKS: Be wary of QR codes in public places - scammers can put fake stickers over real QR codes."
            });
            keywordDisplayNames.Add("link", "link safety");

            // ===== MALWARE RESPONSES =====
            keywordResponses.Add("malware", new List<string> {
                "MALWARE INFO: Malware (malicious software) includes viruses, ransomware, and spyware.",
                "MALWARE INFO: Keep your antivirus software updated and run regular scans.",
                "MALWARE INFO: Don't download software from unknown websites - use official app stores and vendor websites."
            });
            keywordDisplayNames.Add("malware", "malware protection");
        }


        // Match user input to a keyword
        public string MatchKeyword(string input)
        {
            string lowerInput = input.ToLower();

            // Check each keyword
            if (lowerInput.Contains("password") || lowerInput.Contains("passphrase"))
                return "password";

            if (lowerInput.Contains("phish") || lowerInput.Contains("scam") || lowerInput.Contains("fraud"))
                return "phish";

            if (lowerInput.Contains("privacy") || lowerInput.Contains("personal information"))
                return "privacy";

            if (lowerInput.Contains("brows") || lowerInput.Contains("https") || lowerInput.Contains("wi-fi"))
                return "brows";

            if (lowerInput.Contains("link") || lowerInput.Contains("url") || lowerInput.Contains("click"))
                return "link";

            if (lowerInput.Contains("malware") || lowerInput.Contains("virus") || lowerInput.Contains("ransomware"))
                return "malware";

            foreach (var kvp in keywordSynonyms)
            {
                if (lowerInput.Contains(kvp.Key)) return kvp.Key;
                foreach (string syn in kvp.Value)
                    if (lowerInput.Contains(syn)) return kvp.Key;
            }

            return null; // No keyword matched
        }

        private void InitializeSynonyms()
        {
            keywordSynonyms = new Dictionary<string, List<string>>
            {
                ["password"] = new List<string> { "passphrase", "credentials", "login", "authentication" },
                ["phish"] = new List<string> { "scam", "fraud", "deceive", "trick" },
                ["privacy"] = new List<string> { "personal info", "data", "confidential" },
                ["brows"] = new List<string> { "internet", "web", "online", "surf" },
                ["link"] = new List<string> { "url", "click", "address", "website" },
                ["malware"] = new List<string> { "virus", "ransomware", "spyware", "trojan" }
            };
        }

        // Get random response for a keyword
        public string GetRandomResponse(string keyword)
        {
            if (keywordResponses.ContainsKey(keyword))
            {
                List<string> responses = keywordResponses[keyword];
                return responses[random.Next(responses.Count)];
            }
            return null;
        }

        // Get display name for memory
        public string GetDisplayName(string keyword)
        {
            if (keywordDisplayNames.ContainsKey(keyword))
            {
                return keywordDisplayNames[keyword];
            }
            return keyword;
        }
    }
}