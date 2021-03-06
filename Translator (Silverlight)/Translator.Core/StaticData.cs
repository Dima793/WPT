﻿using System.Collections.Generic;


namespace Translator.Core
{
    public static class StaticData
    {
        public static List<Language> Languages { get; } = new List<Language>
        {
            new Language("en-US", "en", "English", true),
            new Language("de-DE", "de", "German", true),
            new Language("fr-FR", "fr", "French", true),
            new Language("it-IT", "it", "Italian", true),
            new Language("ru-RU", "ru", "Russian", false),
            new Language("es-ES", "es", "Spanish", true)
        };

        public static Language SourceLanguage { get; set; } = Languages[0];

        public static Language TargetLanguage { get; set; } = Languages[1];

        public static string SourceText { get; set; } = "Enter here";

        public static string FinalText { get; set; } = "Receive here";

        public static Language SpeakLanguage { get; set; } = Languages[0];

        public static string SpeakText { get; set; } = "I love exams <3";

        public static bool NotSpeaking { get; set; } = true;

        public static bool HistoryLoaded { get; set; } = false;

        public static bool HistoryEnabled { get; set; } = false;

        public static bool NeedToSoundTranslator = false;
    }
}
