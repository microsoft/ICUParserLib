// <copyright file="LanguagePluralRanges.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//
// http://www.unicode.org/cldr/charts/latest/supplemental/language_plural_rules.html

namespace ICUParserLib
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Implements the Language plural range data.
    /// </summary>
    public class LanguagePluralRanges
    {
        /// <summary>
        /// The language list.
        /// Additional languages in %sdxroot%\tools\codes.txt
        /// de-AT
        /// de-CH
        /// en-HK
        /// en-IE
        /// en-NZ
        /// en-SG
        /// es-AR
        /// es-CL
        /// es-CO
        /// es-US
        /// fr-BE
        /// fr-CH
        /// nl-BE
        /// .
        /// </summary>
        public static readonly List<string> LanguageList = new List<string>
        {
            "qps-ploc",
            "qps-plocm",
            "qps-ploca",

            "af",
            "am",
            "ar",
            "as",
            "az",
            "be",
            "bg",
            "bn",
            "bs",
            "ca",
            "chr",
            "cs",
            "cy",
            "da",
            "de",
            "el",
            "en",
            "es",
            "et",
            "eu",
            "fa",
            "fi",
            "fil",
            "fr",
            "ga",
            "gd",
            "gl",
            "gu",
            "ha",
            "he",
            "hi",
            "hr",
            "hu",
            "hy",
            "id",
            "ig",
            "is",
            "it",
            "iu",
            "ja",
            "ka",
            "kk",
            "km",
            "kn",
            "ko",
            "kok",
            "ku",
            "ky",
            "lb",
            "lo",
            "lt",
            "lv",
            "mi",
            "mk",
            "ml",
            "mn",
            "mr",
            "ms",
            "mt",
            "ne",
            "nl",
            "no",
            "nso",
            "or",
            "pa",
            "pl",
            "prs",
            "ps",
            "pt",
            "quc",
            "quz",
            "rm",
            "ro",
            "ru",
            "rw",
            "sd",
            "si",
            "sk",
            "sl",
            "sq",
            "sr",
            "sv",
            "sw",
            "ta",
            "te",
            "tg",
            "th",
            "ti",
            "tk",
            "tn",
            "tr",
            "tt",
            "ug",
            "uk",
            "ur",
            "uz",
            "vi",
            "wo",
            "xh",
            "yo",
            "zh-Hans",
            "zh-Hant",
            "zu",
        };

        /// <summary>
        /// The language range data with cardinal range type.
        /// </summary>
        public static readonly List<LanguagePluralRangeData> LanguageRangeData = new List<LanguagePluralRangeData>()
        {
            // Pseudo languages:
            new LanguagePluralRangeData
            {
                Name = "Pseudo (Pseudo)",
                Lang = "qps-ploc",
                Zero = false,
                One = true,
                Two = false,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Pseudo (Pseudo Mirrored)",
                Lang = "qps-plocm",
                Zero = false,
                One = true,
                Two = false,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Pseudo (Pseudo Asia)",
                Lang = "qps-ploca",
                Zero = false,
                One = false,
                Two = false,
                Few = false,
                Many = false,
                Other = true,
            },

            // List of ICU languages http://www.unicode.org/cldr/charts/latest/supplemental/language_plural_rules.html
            // different to en-US:
            new LanguagePluralRangeData
            {
                Name = "Arabic",
                Lang = "ar",
                Zero = true,
                One = true,
                Two = true,
                Few = true,
                Many = true,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Belarusian",
                Lang = "be",
                Zero = false,
                One = true,
                Two = false,
                Few = true,
                Many = true,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Bosnian",
                Lang = "bs",
                Zero = false,
                One = true,
                Two = false,
                Few = true,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Breton",
                Lang = "br",
                Zero = false,
                One = true,
                Two = true,
                Few = true,
                Many = true,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Burmese",
                Lang = "my",
                Zero = false,
                One = false,
                Two = false,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Catalan",
                Lang = "ca",
                Zero = false,
                One = true,
                Two = false,
                Few = false,
                Many = true,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Chinese",
                Lang = "zh",
                Zero = false,
                One = false,
                Two = false,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Croatian",
                Lang = "hr",
                Zero = false,
                One = true,
                Two = false,
                Few = true,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Czech",
                Lang = "cs",
                Zero = false,
                One = true,
                Two = false,
                Few = true,
                Many = true,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "English",
                Lang = "en",
                Zero = false,
                One = true,
                Two = false,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "French",
                Lang = "fr",
                Zero = false,
                One = true,
                Two = false,
                Few = false,
                Many = true,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Portuguese",
                Lang = "pt",
                Zero = false,
                One = true,
                Two = false,
                Few = false,
                Many = true,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Italian",
                Lang = "it",
                Zero = false,
                One = true,
                Two = false,
                Few = false,
                Many = true,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Spanish",
                Lang = "es",
                Zero = false,
                One = true,
                Two = false,
                Few = false,
                Many = true,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Hebrew",
                Lang = "he",
                Zero = false,
                One = true,
                Two = true,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Igbo",
                Lang = "ig",
                Zero = false,
                One = false,
                Two = false,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Indonesian",
                Lang = "id",
                Zero = false,
                One = false,
                Two = false,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Inuktitut",
                Lang = "iu",
                Zero = false,
                One = true,
                Two = true,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Irish",
                Lang = "ga",
                Zero = false,
                One = true,
                Two = true,
                Few = true,
                Many = true,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Japanese",
                Lang = "ja",
                Zero = false,
                One = false,
                Two = false,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Khmer",
                Lang = "km",
                Zero = false,
                One = false,
                Two = false,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Korean",
                Lang = "ko",
                Zero = false,
                One = false,
                Two = false,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Lao",
                Lang = "lo",
                Zero = false,
                One = false,
                Two = false,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Latvian",
                Lang = "lv",
                Zero = true,
                One = true,
                Two = false,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Lithuanian",
                Lang = "lt",
                Zero = false,
                One = true,
                Two = false,
                Few = true,
                Many = true,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Malay",
                Lang = "ms",
                Zero = false,
                One = false,
                Two = false,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Maltese",
                Lang = "mt",
                Zero = false,
                One = true,
                Two = true,
                Few = true,
                Many = true,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Northern Sami",
                Lang = "se",
                Zero = false,
                One = true,
                Two = true,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Polish",
                Lang = "pl",
                Zero = false,
                One = true,
                Two = false,
                Few = true,
                Many = true,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Romanian",
                Lang = "ro",
                Zero = false,
                One = true,
                Two = false,
                Few = true,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Russian",
                Lang = "ru",
                Zero = false,
                One = true,
                Two = false,
                Few = true,
                Many = true,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Sakha",
                Lang = "sah",
                Zero = false,
                One = false,
                Two = false,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Scottish Gaelic",
                Lang = "gd",
                Zero = false,
                One = true,
                Two = true,
                Few = true,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Serbian",
                Lang = "sr",
                Zero = false,
                One = true,
                Two = false,
                Few = true,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Yi",
                Lang = "ii",
                Zero = false,
                One = false,
                Two = false,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Slovak",
                Lang = "sk",
                Zero = false,
                One = true,
                Two = false,
                Few = true,
                Many = true,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Slovenian",
                Lang = "sl",
                Zero = false,
                One = true,
                Two = true,
                Few = true,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Thai",
                Lang = "th",
                Zero = false,
                One = false,
                Two = false,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Tibetan",
                Lang = "bo",
                Zero = false,
                One = false,
                Two = false,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Ukrainian",
                Lang = "uk",
                Zero = false,
                One = true,
                Two = false,
                Few = true,
                Many = true,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Vietnamese",
                Lang = "vi",
                Zero = false,
                One = false,
                Two = false,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Welsh",
                Lang = "cy",
                Zero = true,
                One = true,
                Two = true,
                Few = true,
                Many = true,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Wolof",
                Lang = "wo",
                Zero = false,
                One = false,
                Two = false,
                Few = false,
                Many = false,
                Other = true,
            },
            new LanguagePluralRangeData
            {
                Name = "Yoruba",
                Lang = "yo",
                Zero = false,
                One = false,
                Two = false,
                Few = false,
                Many = false,
                Other = true,
            },
        };

        /// <summary>
        /// The plural language list.
        /// </summary>
        public static readonly Dictionary<string, string> PluralLanguageList = new Dictionary<string, string>
        {
            // TestPluralLanguageListEntries ensures the language list is correct.

            // Locked language range argument for the 'zero' plural range.
            {
                "zero", "!ar,!cy,!lv"
            },

            // Locked language range argument for the 'one' plural range. Use the inverted range of languages to minimize the list length.
            {
                "one", "qps-ploca,id,ig,ja,km,ko,lo,ms,th,vi,wo,yo,zh-Hans,zh-Hant"
            },

            // Locked language range argument for the 'two' plural range.
            {
                "two", "!ar,!cy,!ga,!gd,!he,!iu,!mt,!sl"
            },

            // Locked language range argument for the 'few' plural range.
            {
                "few", "!ar,!be,!bs,!cs,!cy,!ga,!gd,!hr,!lt,!mt,!pl,!ro,!ru,!sk,!sl,!sr,!tt,!uk"
            },

            // Locked language range argument for the 'many' plural range.
            {
                "many", "!ar,!be,!ca,!cs,!cy,!es,!fr,!ga,!it,!lt,!mt,!pl,!pt,!quz,!ru,!sk,!tt,!uk"
            },

            // The 'other' range is defined in any language.
            {
                "other", string.Empty
            },
        };

        /// <summary>
        /// Support linked ICU languages.
        /// Use only valid CultureInfo entries. Examples for ICU data with non valid CultureInfo are listed as comments.
        /// </summary>
        public static readonly Dictionary<string, string> LanguageLinkTable = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            // Quechua, fallback to 'es'
            {
                "quz", "es"
            },

            // Tatar, fallback to 'ru'
            {
                "tt", "ru"
            },

            // According to the ICU page, Moldavian mo = Romanian ro
            // 'mo' is not a valid CultureInfo. Moldavian is 'ro-MD'.
            // {
            //    "mo", "ro"
            // },

            // According to the ICU page, Tagalog tl = Filipino fil
            // 'tl' is not a valid CultureInfo.
            // {
            //    "tl", "fil"
            // },
        };

        /// <summary>
        /// Gets the language plural range data.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns>Language plural range data.</returns>
        public static LanguagePluralRangeData GetLanguagePluralRangeData(string language)
        {
            return GetLanguagePluralRangeData(CultureInfo.GetCultureInfo(language));
        }

        /// <summary>
        /// Gets the language plural range data.
        /// </summary>
        /// <param name="cultureInfo">The culture info.</param>
        /// <returns>Language plural range data.</returns>
        public static LanguagePluralRangeData GetLanguagePluralRangeData(CultureInfo cultureInfo)
        {
            // Return default 'en' if culture is null.
            if (cultureInfo == null)
            {
                return LanguageRangeData.Find(data => data.Lang.Equals("en"));
            }

            while (!string.IsNullOrEmpty(cultureInfo.Name))
            {
                LanguagePluralRangeData languagePluralRangeData = LanguageRangeData.Find(data => data.Lang.Equals(cultureInfo.Name, StringComparison.OrdinalIgnoreCase));

                // Linked language?
                if (languagePluralRangeData == null && LanguageLinkTable.ContainsKey(cultureInfo.Name))
                {
                    string linkedLanguage = LanguageLinkTable[cultureInfo.Name];
                    languagePluralRangeData = LanguageRangeData.Find(data => data.Lang.Equals(linkedLanguage, StringComparison.OrdinalIgnoreCase));
                }

                if (languagePluralRangeData != null)
                {
                    return languagePluralRangeData;
                }

                cultureInfo = cultureInfo.Parent;
            }

            // Return default 'en' if not found in the LanguageRangeData list.
            return LanguageRangeData.Find(data => data.Lang.Equals("en"));
        }
    }
}
