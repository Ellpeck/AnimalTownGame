using System;
using System.Collections.Generic;
using System.Globalization;

namespace AnimalTownGame.Main {
    public static class Locale {

        private const string DefaultLanguage = "en-US";
        private static readonly Dictionary<string, Dictionary<string, string>> Locales = new Dictionary<string, Dictionary<string, string>>();

        public static string GetInterface(string key) {
            return Get("Interface", key);
        }

        public static string GetItem(string key) {
            return Get("Items", key);
        }

        private static string Get(string category, string key) {
            Dictionary<string, string> cat;
            if (!Locales.TryGetValue(category, out cat)) {
                try {
                    cat = Load(CultureInfo.CurrentCulture.Name, category);
                } catch (Exception) {
                    cat = Load(DefaultLanguage, category);
                }
                Locales[category] = cat;
            }

            string value;
            if (!cat.TryGetValue(key, out value))
                return category + "." + key;
            return value;
        }

        private static Dictionary<string, string> Load(string langKey, string category) {
            return GameImpl.LoadContent<Dictionary<string, string>>("Locale/" + langKey + "/" + category);
        }

    }
}