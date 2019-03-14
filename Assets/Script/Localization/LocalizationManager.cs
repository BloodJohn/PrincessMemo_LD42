using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SimpleLocalization
{
	/// <summary>Localization manager.</summary>
    public static class LocalizationManager
    {
		/// <summary>Fired when localization changed.</summary>
        public static event Action LocalizationChanged = () => { }; 

        private static readonly Dictionary<string, Dictionary<string, string>> Dictionary = new Dictionary<string, Dictionary<string, string>>();
        private static string _language = "English";
        
        private static string[] LangList = {"Russian", "English"};
        private const string localizationKey = "localLang";

		/// <summary>Get or set language.</summary>
        public static string Language
        {
            get => _language;
		    set { _language = value; LocalizationChanged(); }
        }

		/// <summary>Set default language.</summary>
		private static void AutoLanguage()
		{
            if (PlayerPrefs.HasKey(localizationKey))
            {
                Language = PlayerPrefs.GetString(localizationKey);
            }
            else
            {
                Language = Application.systemLanguage == SystemLanguage.Russian ? "Russian" : "English";
            }

            Language = "Russian";
            PlayerPrefs.SetString(localizationKey, Language);
        }

        public static void SetNextLanguage()
        {
            var index = Array.IndexOf(LangList, Language);
            index++;
            if (index >= LangList.Length) index = 0;

            Language = LangList[index];
        }

        /// <summary>Read localization spreadsheets.</summary>
        public static void Read() //"Localization"
        {
            if (Dictionary.Count > 0) return;

            var textAssetList = Resources.LoadAll<TextAsset>(string.Empty);

            foreach (var textAsset in textAssetList)
            {
                var lineList = textAsset.text.Split('\n');

                var langList = lineList[0].Trim().Split(';');
                if (langList.Length <= 1) continue;

                for (var i = 1; i < langList.Length; i++)
                {
                    if (Dictionary.ContainsKey(langList[i])) continue;
                    Dictionary.Add(langList[i], new Dictionary<string, string>());
                }

                for (var i = 1; i < lineList.Length; i++)
                {
                    var columnList = lineList[i].Trim().Split(';');
                    if (columnList.Length != langList.Length)
                    {
                        if (columnList.Length > 1)
                            Debug.LogErrorFormat("localize error : {0}", lineList[i].Trim());
                        continue;
                    }

                    var key = columnList[0];

                    for (var j = 1; j < langList.Length; j++)
                    {
                        Dictionary[langList[j]].Add(key, columnList[j]);
                    }
                }
            }

            AutoLanguage();
        }

		/// <summary>Get localized value by localization key</summary>
        public static string Localize(string localizationKey)
        {
            if (Dictionary.Count == 0)
            {
                Read();
            }

            if (Dictionary.TryGetValue(Language, out var lang))
            {
                if (lang.TryGetValue(localizationKey, out var result))
                {
                    return result;
                }

                return string.Format("lang {0}[{1}] key {2}", Language, lang.Count, localizationKey);
            }

            return string.Format("lang {0} not found", Language);
        }

	    /// <summary>Get localized value by localization key</summary>
		public static string Localize(string localizationKey, params object[] args)
        {
            var pattern = Localize(localizationKey);

            return string.Format(pattern, args);
        }
    }
}