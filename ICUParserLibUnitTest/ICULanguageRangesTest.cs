// <copyright file="ICULanguageRangesTest.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace ICUParserLibUnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using ICUParserLib;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests <see cref="ICUParser"/>.
    /// </summary>
    public partial class ICUTest
    {
        /// <summary>
        /// Tests the language ranges.
        /// </summary>
        [TestMethod]
        public void TestIsLanguageSupported()
        {
            Assert.IsFalse(ICUParser.IsLanguageSupported(0));
            Assert.IsFalse(ICUParser.IsLanguageSupported(-1));
            Assert.IsFalse(ICUParser.IsLanguageSupported(CultureInfo.InvariantCulture));

            Assert.IsTrue(ICUParser.IsLanguageSupported(1033));
            Assert.IsTrue(ICUParser.IsLanguageSupported(CultureInfo.GetCultureInfo("de")));
            Assert.IsTrue(ICUParser.IsLanguageSupported(CultureInfo.GetCultureInfo("de-DE")));
            Assert.IsTrue(ICUParser.IsLanguageSupported(CultureInfo.GetCultureInfo("ca-Es-VALENCIA")));
            Assert.IsTrue(ICUParser.IsLanguageSupported(CultureInfo.GetCultureInfo("sr-Latn-RS")));
            Assert.IsTrue(ICUParser.IsLanguageSupported(CultureInfo.GetCultureInfo("zh-HK")));

            Assert.IsFalse(ICUParser.IsLanguageSupported(CultureInfo.GetCultureInfo("doi")));
        }

        /// <summary>
        /// Tests the language ranges.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLanguageRangesNull()
        {
            string language = null;
            LanguagePluralRangeData languagePluralRangeData = LanguagePluralRanges.GetLanguagePluralRangeData(language);

            // Set ranges to 'en' in case the language cannot be found.
            Assert.AreEqual("English", languagePluralRangeData.Name);
            Assert.AreEqual("en", languagePluralRangeData.Lang);

            Assert.IsFalse(languagePluralRangeData.Zero);
            Assert.IsTrue(languagePluralRangeData.One);
            Assert.IsFalse(languagePluralRangeData.Two);
            Assert.IsFalse(languagePluralRangeData.Few);
            Assert.IsFalse(languagePluralRangeData.Many);
            Assert.IsTrue(languagePluralRangeData.Other);
        }

        /// <summary>
        /// Tests the language ranges.
        /// </summary>
        [TestMethod]
        public void TestLanguageRangesNullCulture()
        {
            CultureInfo cultureInfo = null;
            LanguagePluralRangeData languagePluralRangeData = LanguagePluralRanges.GetLanguagePluralRangeData(cultureInfo);

            // Set ranges to 'en' in case the language cannot be found.
            Assert.AreEqual("English", languagePluralRangeData.Name);
            Assert.AreEqual("en", languagePluralRangeData.Lang);

            Assert.IsFalse(languagePluralRangeData.Zero);
            Assert.IsTrue(languagePluralRangeData.One);
            Assert.IsFalse(languagePluralRangeData.Two);
            Assert.IsFalse(languagePluralRangeData.Few);
            Assert.IsFalse(languagePluralRangeData.Many);
            Assert.IsTrue(languagePluralRangeData.Other);
        }

        /// <summary>
        /// Tests the language ranges.
        /// </summary>
        [TestMethod]
        public void TestLanguageRangesNeutralCulture()
        {
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            LanguagePluralRangeData languagePluralRangeData = LanguagePluralRanges.GetLanguagePluralRangeData(cultureInfo);

            // Set ranges to 'en' in case the language cannot be found.
            Assert.AreEqual("English", languagePluralRangeData.Name);
            Assert.AreEqual("en", languagePluralRangeData.Lang);

            Assert.IsFalse(languagePluralRangeData.Zero);
            Assert.IsTrue(languagePluralRangeData.One);
            Assert.IsFalse(languagePluralRangeData.Two);
            Assert.IsFalse(languagePluralRangeData.Few);
            Assert.IsFalse(languagePluralRangeData.Many);
            Assert.IsTrue(languagePluralRangeData.Other);
        }

        /// <summary>
        /// Tests the language ranges.
        /// </summary>
        [TestMethod]
        public void TestLanguageRangesDefaultLanguageRange()
        {
            LanguagePluralRangeData languagePluralRangeData = LanguagePluralRanges.GetLanguagePluralRangeData("en");
            LanguagePluralRangeData languagePluralRangeDataEn = LanguagePluralRanges.LanguageRangeData.Find(data => data.Lang.Equals("en"));

            Assert.AreEqual(languagePluralRangeData.Name, languagePluralRangeDataEn.Name);
            Assert.AreEqual(languagePluralRangeData.Lang, languagePluralRangeDataEn.Lang);

            Assert.AreEqual(languagePluralRangeData.Zero, languagePluralRangeDataEn.Zero);
            Assert.AreEqual(languagePluralRangeData.One, languagePluralRangeDataEn.One);
            Assert.AreEqual(languagePluralRangeData.Two, languagePluralRangeDataEn.Two);
            Assert.AreEqual(languagePluralRangeData.Few, languagePluralRangeDataEn.Few);
            Assert.AreEqual(languagePluralRangeData.Many, languagePluralRangeDataEn.Many);
            Assert.AreEqual(languagePluralRangeData.Other, languagePluralRangeDataEn.Other);
        }

        /// <summary>
        /// Tests the language ranges.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.Globalization.CultureNotFoundException))]
        public void TestLanguageRangesInvalid()
        {
            _ = LanguagePluralRanges.GetLanguagePluralRangeData("invalid");
        }

        /// <summary>
        /// Tests the language ranges.
        /// </summary>
        [TestMethod]
        public void TestLanguageRangesStats()
        {
            // Including the 3 pseudo languages.
            int count = LanguagePluralRanges.LanguageRangeData.Count;

            // Make sure that none of the languages gets deleted or added without review.
            Assert.AreEqual(44 + 3, count);

            // Count of the plural ranges.
            int zero = 0;
            int one = 0;
            int two = 0;
            int few = 0;
            int many = 0;

            foreach (string language in LanguagePluralRanges.LanguageList)
            {
                LanguagePluralRangeData languagePluralRangeData = LanguagePluralRanges.GetLanguagePluralRangeData(language);

                // Each entry must define the plural range 'other'.
                Assert.IsTrue(languagePluralRangeData.Other);

                // Validate that each language entry has a valid language code.
                CultureInfo ci = CultureInfo.GetCultureInfo(languagePluralRangeData.Lang);

                // Assert.
                // Not a unknown language.
                Assert.IsFalse(ci.EnglishName.StartsWith("Unknown Language "));

                // Name matches.
                Assert.AreEqual(languagePluralRangeData.Name, ci.EnglishName);

                // Get the plural range stats.
                zero += languagePluralRangeData.Zero ? 1 : 0;
                one += languagePluralRangeData.One ? 1 : 0;
                two += languagePluralRangeData.Two ? 1 : 0;
                few += languagePluralRangeData.Few ? 1 : 0;
                many += languagePluralRangeData.Many ? 1 : 0;
            }

            Assert.AreEqual(3, zero);
            Assert.AreEqual(92, one);
            Assert.AreEqual(8, two);
            Assert.AreEqual(18, few);
            Assert.AreEqual(18, many);
        }

        /// <summary>
        /// Tests if there are no duplicate language range entries.
        /// </summary>
        [TestMethod]
        public void TestDuplicateLanguageRangeData()
        {
            Dictionary<string, LanguagePluralRangeData> entries = new Dictionary<string, LanguagePluralRangeData>();

            foreach (LanguagePluralRangeData language in LanguagePluralRanges.LanguageRangeData)
            {
                if (entries.ContainsKey(language.Lang))
                {
                    Assert.Fail($"The entriy for {language.Name} ({language.Lang}) is duplicated.");
                }

                entries.Add(language.Lang, language);
            }
        }

        /// <summary>
        /// Tests the language ranges.
        /// </summary>
        [TestMethod]
        public void TestLanguageRangesArabic()
        {
            LanguagePluralRangeData languagePluralRangeData = LanguagePluralRanges.GetLanguagePluralRangeData("AR");

            Assert.AreEqual("Arabic", languagePluralRangeData.Name);
            Assert.AreEqual("ar", languagePluralRangeData.Lang);

            Assert.AreEqual(true, languagePluralRangeData.Zero);
            Assert.AreEqual(true, languagePluralRangeData.One);
            Assert.AreEqual(true, languagePluralRangeData.Two);
            Assert.AreEqual(true, languagePluralRangeData.Few);
            Assert.AreEqual(true, languagePluralRangeData.Many);
            Assert.AreEqual(true, languagePluralRangeData.Other);
        }

        /// <summary>
        /// Tests the language ranges.
        /// </summary>
        [TestMethod]
        public void TestLanguageRangesDE()
        {
            LanguagePluralRangeData languagePluralRangeData = LanguagePluralRanges.GetLanguagePluralRangeData("DE");

            // Check the correct fall back to 'en'.
            Assert.AreEqual("English", languagePluralRangeData.Name);
            Assert.AreEqual("en", languagePluralRangeData.Lang);

            Assert.AreEqual(false, languagePluralRangeData.Zero);
            Assert.AreEqual(true, languagePluralRangeData.One);
            Assert.AreEqual(false, languagePluralRangeData.Two);
            Assert.AreEqual(false, languagePluralRangeData.Few);
            Assert.AreEqual(false, languagePluralRangeData.Many);
            Assert.AreEqual(true, languagePluralRangeData.Other);
        }

        /// <summary>
        /// Tests the language ranges.
        /// </summary>
        [TestMethod]
        public void TestLanguageRangesDeDE()
        {
            LanguagePluralRangeData languagePluralRangeData = LanguagePluralRanges.GetLanguagePluralRangeData("de-DE");

            // Check the correct fall back to 'en'.
            Assert.AreEqual("English", languagePluralRangeData.Name);
            Assert.AreEqual("en", languagePluralRangeData.Lang);

            Assert.AreEqual(false, languagePluralRangeData.Zero);
            Assert.AreEqual(true, languagePluralRangeData.One);
            Assert.AreEqual(false, languagePluralRangeData.Two);
            Assert.AreEqual(false, languagePluralRangeData.Few);
            Assert.AreEqual(false, languagePluralRangeData.Many);
            Assert.AreEqual(true, languagePluralRangeData.Other);
        }

        /// <summary>
        /// Tests the language ranges.
        /// </summary>
        [TestMethod]
        public void TestLanguageRangesScottishGaelic()
        {
            LanguagePluralRangeData languagePluralRangeData = LanguagePluralRanges.GetLanguagePluralRangeData("gd");

            Assert.AreEqual("Scottish Gaelic", languagePluralRangeData.Name);
            Assert.AreEqual("gd", languagePluralRangeData.Lang);

            Assert.AreEqual(false, languagePluralRangeData.Zero);
            Assert.AreEqual(true, languagePluralRangeData.One);
            Assert.AreEqual(true, languagePluralRangeData.Two);
            Assert.AreEqual(true, languagePluralRangeData.Few);
            Assert.AreEqual(false, languagePluralRangeData.Many);
            Assert.AreEqual(true, languagePluralRangeData.Other);
        }

        /// <summary>
        /// Tests the language ranges.
        /// </summary>
        [TestMethod]
        public void TestLanguageRangesFromLcid()
        {
            CultureInfo cultureInfoJaJP = CultureInfo.GetCultureInfo(1041);
            LanguagePluralRangeData languagePluralRangeData = LanguagePluralRanges.GetLanguagePluralRangeData(cultureInfoJaJP.TwoLetterISOLanguageName);

            Assert.AreEqual("Japanese", languagePluralRangeData.Name);
            Assert.AreEqual("ja", languagePluralRangeData.Lang);

            Assert.AreEqual(false, languagePluralRangeData.Zero);
            Assert.AreEqual(false, languagePluralRangeData.One);
            Assert.AreEqual(false, languagePluralRangeData.Two);
            Assert.AreEqual(false, languagePluralRangeData.Few);
            Assert.AreEqual(false, languagePluralRangeData.Many);
            Assert.AreEqual(true, languagePluralRangeData.Other);
        }

        /// <summary>
        /// Tests the language ranges.
        /// </summary>
        [TestMethod]
        public void TestLanguageRangesLanguage()
        {
            // Test each Language for a not empty Name entry.
            foreach (LanguagePluralRangeData languagePluralRangeData in LanguagePluralRanges.LanguageRangeData)
            {
                Assert.IsFalse(string.IsNullOrEmpty(languagePluralRangeData.Name));
            }

            // Exclude languages for knows differences.
            List<string> excludedLanguages = new List<string> { };

            // Test each Language for a valid language name. CultureInfo must not throw any exception.
            foreach (LanguagePluralRangeData languagePluralRangeData in LanguagePluralRanges.LanguageRangeData)
            {
                if (!excludedLanguages.Contains(languagePluralRangeData.Lang))
                {
                    CultureInfo ci = CultureInfo.GetCultureInfo(languagePluralRangeData.Lang);

                    // Assert.
                    // Not a unknown language.
                    Assert.IsFalse(ci.EnglishName.StartsWith("Unknown Language "));

                    // Name matches.
                    Assert.AreEqual(languagePluralRangeData.Name, ci.EnglishName);
                }
            }
        }

        /// <summary>
        /// Tests the language ranges.
        /// </summary>
        [TestMethod]
        [DeploymentItem("LocCultures.xml")]
        public void TestLanguageListIsInLocCulturesXml()
        {
            this.LoadLocCulturesXml();

            // Expect all languages to be defined in the LocCultures.xml.
            // Test will throw an exception if a language is not available
            // and fail the test automatically.

            // Check the entries in the LanguageList.
            foreach (string language in LanguagePluralRanges.LanguageList)
            {
                Assert.IsTrue(locCulturesXml.ContainsKey(language));
            }

            // Check the entries in the LanguageRangeData.
            foreach (LanguagePluralRangeData data in LanguagePluralRanges.LanguageRangeData)
            {
                Assert.IsTrue(locCulturesXml.ContainsKey(data.Lang));
            }
        }

        /// <summary>
        /// Tests the language ranges.
        /// </summary>
        [TestMethod]
        public void TestLanguageRangesPseudoDefined()
        {
            // Pseudo Locale        Locale Name  LCID
            // ===================  ===========  ======
            // Base                 qps-ploc     0x0501
            // Mirrored             qps-mirr     0x09ff
            // East Asian-language  qps-asia     0x05fe
            CultureInfo cultureInfo_ploc = CultureInfo.GetCultureInfo(0x0501);
            CultureInfo cultureInfo_mirr = CultureInfo.GetCultureInfo(0x09ff);
            CultureInfo cultureInfo_asia = CultureInfo.GetCultureInfo(0x05fe);

            LanguagePluralRangeData languagePluralRangeData_ploc = LanguagePluralRanges.GetLanguagePluralRangeData(cultureInfo_ploc);
            Assert.AreEqual("Pseudo (Pseudo)", languagePluralRangeData_ploc.Name);

            LanguagePluralRangeData languagePluralRangeData_mirr = LanguagePluralRanges.GetLanguagePluralRangeData(cultureInfo_mirr);
            Assert.AreEqual("Pseudo (Pseudo Mirrored)", languagePluralRangeData_mirr.Name);

            LanguagePluralRangeData languagePluralRangeData_asia = LanguagePluralRanges.GetLanguagePluralRangeData(cultureInfo_asia);
            Assert.AreEqual("Pseudo (Pseudo Asia)", languagePluralRangeData_asia.Name);
        }

        /// <summary>
        /// Tests the language ranges.
        /// </summary>
        [TestMethod]
        [DeploymentItem("LocCultures.xml")]
        public void TestLanguageAddedUpdatedLanguageData()
        {
            // The following language data were added/updated to the
            // initial set of language data:

            // changed:
            // French fr, from 'one other' to 'one many other'
            LanguagePluralRangeData languagePluralRangeData_fr = LanguagePluralRanges.GetLanguagePluralRangeData("fr");
            Assert.IsTrue(languagePluralRangeData_fr.Many);

            // Portuguese pt, from 'one other' to 'one many other'
            LanguagePluralRangeData languagePluralRangeData_pt = LanguagePluralRanges.GetLanguagePluralRangeData("pt");
            Assert.IsTrue(languagePluralRangeData_pt.Many);

            // Italian it, from 'one other' to 'one many other'
            LanguagePluralRangeData languagePluralRangeData_it = LanguagePluralRanges.GetLanguagePluralRangeData("it");
            Assert.IsTrue(languagePluralRangeData_it.Many);

            // Spanish es, from 'one other' to 'one many other'
            LanguagePluralRangeData languagePluralRangeData_es = LanguagePluralRanges.GetLanguagePluralRangeData("es");
            Assert.IsTrue(languagePluralRangeData_es.Many);

            // The following language data were added/updated:

            // changed:
            // Catalan ca, from 'one other' to 'one many other'
            LanguagePluralRangeData languagePluralRangeData_ca = LanguagePluralRanges.GetLanguagePluralRangeData("ca");
            Assert.IsTrue(languagePluralRangeData_ca.Many);

            // Hebrew he, from 'one two many other' to 'one two other'
            LanguagePluralRangeData languagePluralRangeData_he = LanguagePluralRanges.GetLanguagePluralRangeData("he");
            Assert.IsFalse(languagePluralRangeData_he.Many);

            // new:
            // Baluchi bal  one other
            // Dogri doi  one other
            // Hmong Njua hnj  other
            // Ligurian lij  one other
            // Nigerian Pidgin pcm  one other
            // Santali sat  one two other
            // Tok Pisin tpi  other

            // None of these languages are in loccultures.xml.
            this.LoadLocCulturesXml();

            // Not defined in CultureInfo and LocCultures.xml.
            Assert.IsTrue(CultureInfo.GetCultureInfo("bal").EnglishName.StartsWith("Unknown Language "));
            Assert.IsFalse(locCulturesXml.ContainsKey("bal"));

            Assert.IsTrue(CultureInfo.GetCultureInfo("hnj").EnglishName.StartsWith("Unknown Language "));
            Assert.IsFalse(locCulturesXml.ContainsKey("hnj"));

            Assert.IsTrue(CultureInfo.GetCultureInfo("lij").EnglishName.StartsWith("Unknown Language "));
            Assert.IsFalse(locCulturesXml.ContainsKey("hnj"));
        }

        /// <summary>
        /// Tests the plural message language range.
        /// </summary>
        [TestMethod]
        public void TestPluralMessageWithLanguageRange()
        {
            // Initialize.
            string input = @"{count, plural,
                =1 { Relaunch Microsoft Edge within a day}
                other { Relaunch Microsoft Edge within # days}}";

            ICUParser icuParser = new ICUParser(input, true);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsTrue(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            List<MessageItem> pluralMessageItems = messageItems.Where(messageItem => messageItem.MessageItemType == MessageItemTypeEnum.ExpandedPlural).ToList();
            Assert.AreEqual(5, pluralMessageItems.Count);

            // Add plural translations.
            int i = 0;
            foreach (MessageItem pluralMessageItem in pluralMessageItems)
            {
                pluralMessageItem.Text = $"{pluralMessageItem.ResourceId}.{i++}";
            }

            // Use ja-jp culture with no additional plurals defined.
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo("ja-JP");
            string output = icuParser.ComposeMessageText(messageItems, cultureInfo);

            // Assert.
            // No additional plural ranges added because 'ja-jp' defines only 'other'.
            Assert.AreEqual(input, output, "Different text output.");

            // Use culture with the 'one' plural.
            cultureInfo = CultureInfo.GetCultureInfo("de-de");
            output = icuParser.ComposeMessageText(messageItems, cultureInfo);

            // Assert.
            // One additional plural range added because "de-de" has the 'one' range.
            Assert.AreNotEqual(input, output, "Same text output.");
            Assert.IsFalse(output.Contains("zero {"));
            Assert.IsTrue(output.Contains("one {"));
            Assert.IsFalse(output.Contains("two {"));
            Assert.IsFalse(output.Contains("few {"));
            Assert.IsFalse(output.Contains("many {"));

            // Use culture with all plurals defined.
            cultureInfo = CultureInfo.GetCultureInfo("ar");
            output = icuParser.ComposeMessageText(messageItems, cultureInfo);

            // Assert.
            // Additional plural ranges added because Arabic has all ranges.
            Assert.AreNotEqual(input, output, "Same text output.");
            Assert.IsTrue(output.Contains("zero {"));
            Assert.IsTrue(output.Contains("one {"));
            Assert.IsTrue(output.Contains("two {"));
            Assert.IsTrue(output.Contains("few {"));
            Assert.IsTrue(output.Contains("many {"));

            // Verify the composed output.
            ICUParser icuParserVerify = new ICUParser(output);

            // Assert.
            Assert.IsTrue(icuParserVerify.Success);
        }

        /// <summary>
        /// Tests the plural message language range.
        /// </summary>
        [TestMethod]
        public void TestPluralMessageWithLanguageRangeFrenchExample()
        {
            // Initialize.
            string input = @"{count, plural, =1 {Feature} other {Features}}";

            ICUParser icuParser = new ICUParser(input, true);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsTrue(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            Assert.AreEqual(7, messageItems.Count);

            // Translate.
            messageItems[0].Text = "fonctionnalité masquée";
            messageItems[1].Text = "fonctionnalités masquées";
            messageItems[6].Text = "Features [many]";

            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("fr-FR"));

            // Assert.
            Assert.AreEqual("{count, plural, =1 {fonctionnalité masquée} one {Features} many {Features [many]} other {fonctionnalités masquées}}", output);
        }

        /// <summary>
        /// Tests the language ranges.
        /// </summary>
        [TestMethod]
        public void TestLanguageRangesInternPlurals()
        {
            string input = @"{notifications, plural,
                  zero {'zero' notifications}
                    =0 {'=0' notification}
                   one { 
                            {0, plural,
                                zero { Relaunch Microsoft Edge within 'zero'}
                                  =1 { Relaunch Microsoft Edge within '=1'}
                                 one { Relaunch Microsoft Edge within 'one'}
                                 two { Relaunch Microsoft Edge within 'two'}
                                 few { Relaunch Microsoft Edge within 'few'}
                                many { Relaunch Microsoft Edge within 'many'}
                               other { Relaunch Microsoft Edge within 'other'}}
                       }
                   two {'two' notification}
                   few {'few' notifications}
                  many {'many' notifications}
                 other {'other' notifications}
                }";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            // Name = "Croatian",
            // Lang = "hr",
            // Zero = false,
            // One = true,
            // Two = false,
            // Few = true,
            // Many = false,
            // Other = true,
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("hr"));
            Assert.IsTrue(output.Contains("=0 {"));
            Assert.IsFalse(output.Contains("zero {"));
            Assert.IsTrue(output.Contains("one {"));
            Assert.IsFalse(output.Contains("two {"));
            Assert.IsTrue(output.Contains("few {"));
            Assert.IsFalse(output.Contains("many {"));
            Assert.IsTrue(output.Contains("other {"));

            // Verify the composed output.
            ICUParser icuParserVerify = new ICUParser(output);

            // Assert.
            Assert.IsTrue(icuParserVerify.Success);

            // Name = "Arabic",
            // Lang = "ar",
            // Zero = true,
            // One = true,
            // Two = true,
            // Few = true,
            // Many = true,
            // Other = true,
            output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("ar"));
            Assert.IsTrue(output.Contains("=0 {"));
            Assert.IsTrue(output.Contains("=1 {"));
            Assert.IsTrue(output.Contains("zero {"));
            Assert.IsTrue(output.Contains("one {"));
            Assert.IsTrue(output.Contains("two {"));
            Assert.IsTrue(output.Contains("few {"));
            Assert.IsTrue(output.Contains("many {"));
            Assert.IsTrue(output.Contains("other {"));

            // Verify the composed output.
            icuParserVerify = new ICUParser(output);

            // Assert.
            Assert.IsTrue(icuParserVerify.Success);
        }

        /// <summary>
        /// Tests the language ranges.
        /// </summary>
        [TestMethod]
        public void TestLanguageRangesInternPluralsNoWhitespace()
        {
            string input = @"{notifications,plural,zero{'zero'notifications}=0{'=0'notification}one{'one'notification}two{'two'notification}few{'few'notifications}many{'many'notifications}other{'other'notifications}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            // Name = "Korean",
            // Lang = "ko",
            // Zero = false,
            // One = false,
            // Two = false,
            // Few = false,
            // Many = false,
            // Other = true,
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("ko"));
            Assert.AreEqual("{notifications,plural,=0{'=0'notification}other{'other'notifications}}", output);

            // Verify the composed output.
            ICUParser icuParserVerify = new ICUParser(output);

            // Assert.
            Assert.AreEqual(icuParserVerify.Success, true);

            // Name = "Arabic",
            // Lang = "ar",
            // Zero = true,
            // One = true,
            // Two = true,
            // Few = true,
            // Many = true,
            // Other = true,
            output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("ar"));
            Assert.AreEqual(input, output, "Different text output.");

            // Verify the composed output.
            icuParserVerify = new ICUParser(output);

            // Assert.
            Assert.AreEqual(icuParserVerify.Success, true);
        }

        /// <summary>
        /// Tests the language ranges.
        /// </summary>
        [TestMethod]
        public void TestLanguageRangesInternPluralsNoWhitespaceAddedPlural()
        {
            string input = @"{notifications,plural,=0{'=0'notification}other{'other'notifications}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("ar"));
            Assert.AreEqual("{notifications,plural,=0{'=0'notification}zero {'other'notifications}one {'other'notifications}two {'other'notifications}few {'other'notifications}many {'other'notifications}other{'other'notifications}}", output);

            // Verify the composed output.
            ICUParser icuParserVerify = new ICUParser(output);

            // Assert.
            Assert.IsTrue(icuParserVerify.Success);

            // Name = "Arabic",
            // Lang = "ar",
            // Zero = true,
            // One = true,
            // Two = true,
            // Few = true,
            // Many = true,
            // Other = true,
            string output2 = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("ar"));
            Assert.AreEqual(output, output2, "Different text output.");

            // Verify the composed output.
            icuParserVerify = new ICUParser(output);

            // Assert.
            Assert.IsTrue(icuParserVerify.Success);
        }

        /// <summary>
        /// Tests the language ranges.
        /// </summary>
        [TestMethod]
        public void TestIsLanguageListInLanguageRangeData()
        {
            // Check all 'en' languages.
            string en = string.Join(",", LanguagePluralRanges.LanguageRangeData.Where(language => !language.Zero && language.One && !language.Two && !language.Few && !language.Many && language.Other)
                .Select(language => language.Lang));

            Assert.AreEqual("qps-ploc,qps-plocm,en", en);
        }

        /// <summary>
        /// Tests the LanguageRangeData linking.
        /// </summary>
        [TestMethod]
        public void TestLanguageRangeDataLinking()
        {
            // Link Quechua to Spanish.
            LanguagePluralRangeData languagePluralRangeDataQuz = LanguagePluralRanges.GetLanguagePluralRangeData("quz");
            LanguagePluralRangeData languagePluralRangeDataEs = LanguagePluralRanges.GetLanguagePluralRangeData("es");

            Assert.AreEqual(languagePluralRangeDataQuz, languagePluralRangeDataEs, "Quechua is different to Spanish.");

            // Link Tatar to Russian.
            LanguagePluralRangeData languagePluralRangeDataTt = LanguagePluralRanges.GetLanguagePluralRangeData("tt");
            LanguagePluralRangeData languagePluralRangeDataRu = LanguagePluralRanges.GetLanguagePluralRangeData("ru");

            Assert.AreEqual(languagePluralRangeDataTt, languagePluralRangeDataRu, "Tatar is different to Russian.");
        }

        /// <summary>
        /// Tests the language linking.
        /// </summary>
        [TestMethod]
        public void TestLanguageLinkTable()
        {
            // Setup test table.
            Dictionary<string, string> languageLinkTableTest = new Dictionary<string, string>
            {
                {
                    "ab-cd", "qps-ploc"
                },
            };

            // Add test entry to link language.
            foreach (string link in languageLinkTableTest.Keys)
            {
                // The test entry is new.
                Assert.IsFalse(LanguagePluralRanges.LanguageLinkTable.ContainsKey(link));

                LanguagePluralRanges.LanguageLinkTable.Add(link, languageLinkTableTest[link]);
            }

            // Assert.
            // Expect 'ab-cd' linked to 'qps-ploc'.
            LanguagePluralRangeData languagePluralRangeData = LanguagePluralRanges.GetLanguagePluralRangeData("ab-cd");
            Assert.AreEqual("qps-ploc", languagePluralRangeData.Lang);

            // Restore table.
            foreach (string link in languageLinkTableTest.Keys)
            {
                LanguagePluralRanges.LanguageLinkTable.Remove(link);
            }
        }

        /// <summary>
        /// This test case validates the entries that are used for the PluralLanguageList.
        /// </summary>
        [TestMethod]
        public void TestPluralLanguageListEntries()
        {
            string zero = string.Join(",", LanguagePluralRanges.LanguageList.Where(language => LanguagePluralRanges.GetLanguagePluralRangeData(CultureInfo.GetCultureInfo(language)).Zero).Select(s => string.Concat('!', s)).ToList());
            Assert.AreEqual("!ar,!cy,!lv", zero);
            Assert.AreEqual(zero, LanguagePluralRanges.PluralLanguageList["zero"]);

            string one = string.Join(",", LanguagePluralRanges.LanguageList.Where(language => !LanguagePluralRanges.GetLanguagePluralRangeData(CultureInfo.GetCultureInfo(language)).One).ToList());
            Assert.AreEqual("qps-ploca,id,ig,ja,km,ko,lo,ms,th,vi,wo,yo,zh-Hans,zh-Hant", one);
            Assert.AreEqual(one, LanguagePluralRanges.PluralLanguageList["one"]);

            string two = string.Join(",", LanguagePluralRanges.LanguageList.Where(language => LanguagePluralRanges.GetLanguagePluralRangeData(CultureInfo.GetCultureInfo(language)).Two).Select(s => string.Concat('!', s)).ToList());
            Assert.AreEqual("!ar,!cy,!ga,!gd,!he,!iu,!mt,!sl", two);
            Assert.AreEqual(two, LanguagePluralRanges.PluralLanguageList["two"]);

            string few = string.Join(",", LanguagePluralRanges.LanguageList.Where(language => LanguagePluralRanges.GetLanguagePluralRangeData(CultureInfo.GetCultureInfo(language)).Few).Select(s => string.Concat('!', s)).ToList());
            Assert.AreEqual("!ar,!be,!bs,!cs,!cy,!ga,!gd,!hr,!lt,!mt,!pl,!ro,!ru,!sk,!sl,!sr,!tt,!uk", few);
            Assert.AreEqual(few, LanguagePluralRanges.PluralLanguageList["few"]);

            string many = string.Join(",", LanguagePluralRanges.LanguageList.Where(language => LanguagePluralRanges.GetLanguagePluralRangeData(CultureInfo.GetCultureInfo(language)).Many).Select(s => string.Concat('!', s)).ToList());
            Assert.AreEqual("!ar,!be,!ca,!cs,!cy,!es,!fr,!ga,!it,!lt,!mt,!pl,!pt,!quz,!ru,!sk,!tt,!uk", many);
            Assert.AreEqual(many, LanguagePluralRanges.PluralLanguageList["many"]);
        }

        /// <summary>
        /// This test case validates the entries from GetMessageItems match the PluralLanguageList data.
        /// </summary>
        [TestMethod]
        public void TestPluralLanguageListGetMessageItemsData()
        {
            // Initialize.
            string input = @"{count, plural,
                =1 { Relaunch Microsoft Edge within a day}
                many { Relaunch Microsoft Edge within a day}
                other { Relaunch Microsoft Edge within # days}}";

            // false: do not remove duplicate text.
            ICUParser icuParser = new ICUParser(input, false);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsTrue(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            // Assert.
            Assert.AreEqual(7, messageItems.Count);

            // No data for '=1' and 'other'
            Assert.AreEqual("=1", messageItems[0].Plural);
            Assert.AreEqual(string.Empty, messageItems[0].Data);

            Assert.AreEqual("other", messageItems[2].Plural);
            Assert.AreEqual(string.Empty, messageItems[2].Data);

            // Expect data for the other plurals.
            Assert.AreEqual("many", messageItems[1].Plural);
            Assert.AreEqual("!ar,!be,!ca,!cs,!cy,!es,!fr,!ga,!it,!lt,!mt,!pl,!pt,!quz,!ru,!sk,!tt,!uk", messageItems[1].Data);

            Assert.AreEqual("zero", messageItems[3].Plural);
            Assert.AreEqual("!ar,!cy,!lv", messageItems[3].Data);

            Assert.AreEqual("one", messageItems[4].Plural);
            Assert.AreEqual("qps-ploca,id,ig,ja,km,ko,lo,ms,th,vi,wo,yo,zh-Hans,zh-Hant", messageItems[4].Data);

            Assert.AreEqual("two", messageItems[5].Plural);
            Assert.AreEqual("!ar,!cy,!ga,!gd,!he,!iu,!mt,!sl", messageItems[5].Data);

            Assert.AreEqual("few", messageItems[6].Plural);
            Assert.AreEqual("!ar,!be,!bs,!cs,!cy,!ga,!gd,!hr,!lt,!mt,!pl,!ro,!ru,!sk,!sl,!sr,!tt,!uk", messageItems[6].Data);
        }

        /// <summary>
        /// This test case validates the nested entries from GetMessageItems match the PluralLanguageList data.
        /// </summary>
        [TestMethod]
        public void TestNestedPluralLanguageListGetMessageItemsData()
        {
            string input = @"{additionalSenderCount, plural, 
                one{
                    {replyCount, plural, 
                        one{ 'one' # more reply from {sender1}, {sender2}, {sender3}, and # other {numNewClause}} 
                        other{ 'one' # more replies from {sender1}, {sender2}, {sender3}, and # other {numNewClause}}
                    }} 
                other{
                    {replyCount, plural, 
                        one{ 'other' # more reply from {sender1}, {sender2}, {sender3}, and # others {numNewClause}} 
                        other{ 'other' # more replies from {sender1}, {sender2}, {sender3}, and # others {numNewClause}}
                    }}
                }";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            // Assert.
            Assert.AreEqual(12, messageItems.Count);

            // No data for 'other'
            // Plural.other#0
            Assert.AreEqual("Plural.other#0", messageItems[1].ResourceId);
            Assert.AreEqual("other", messageItems[1].Plural);
            Assert.AreEqual(string.Empty, messageItems[1].Data);

            // Plural.other#1
            Assert.AreEqual("Plural.other#1", messageItems[3].ResourceId);
            Assert.AreEqual("other", messageItems[3].Plural);
            Assert.AreEqual(string.Empty, messageItems[3].Data);

            // Expect data for the other plurals.
            // Plural.one#1
            Assert.AreEqual("Plural.one#1", messageItems[2].ResourceId);
            Assert.AreEqual("one", messageItems[2].Plural);
            Assert.AreEqual("qps-ploca,id,ig,ja,km,ko,lo,ms,th,vi,wo,yo,zh-Hans,zh-Hant", messageItems[2].Data);

            // Plural.one#0
            Assert.AreEqual("Plural.one#0", messageItems[0].ResourceId);
            Assert.AreEqual("one", messageItems[0].Plural);
            Assert.AreEqual("qps-ploca,id,ig,ja,km,ko,lo,ms,th,vi,wo,yo,zh-Hans,zh-Hant", messageItems[0].Data);

            // ExpandedPlural.one.zero
            Assert.AreEqual("ExpandedPlural.one.zero", messageItems[4].ResourceId);
            Assert.AreEqual("zero", messageItems[4].Plural);
            Assert.AreEqual("!ar,!cy,!lv", messageItems[4].Data);

            // ExpandedPlural.one.two
            Assert.AreEqual("ExpandedPlural.one.two", messageItems[5].ResourceId);
            Assert.AreEqual("two", messageItems[5].Plural);
            Assert.AreEqual("!ar,!cy,!ga,!gd,!he,!iu,!mt,!sl", messageItems[5].Data);

            // ExpandedPlural.one.few
            Assert.AreEqual("ExpandedPlural.one.few", messageItems[6].ResourceId);
            Assert.AreEqual("few", messageItems[6].Plural);
            Assert.AreEqual("!ar,!be,!bs,!cs,!cy,!ga,!gd,!hr,!lt,!mt,!pl,!ro,!ru,!sk,!sl,!sr,!tt,!uk", messageItems[6].Data);

            // ExpandedPlural.one.many
            Assert.AreEqual("ExpandedPlural.one.many", messageItems[7].ResourceId);
            Assert.AreEqual("many", messageItems[7].Plural);
            Assert.AreEqual("!ar,!be,!ca,!cs,!cy,!es,!fr,!ga,!it,!lt,!mt,!pl,!pt,!quz,!ru,!sk,!tt,!uk", messageItems[7].Data);

            // ExpandedPlural.other.zero
            Assert.AreEqual("ExpandedPlural.other.zero", messageItems[8].ResourceId);
            Assert.AreEqual("zero", messageItems[8].Plural);
            Assert.AreEqual("!ar,!cy,!lv", messageItems[8].Data);

            // ExpandedPlural.other.two
            Assert.AreEqual("ExpandedPlural.other.two", messageItems[9].ResourceId);
            Assert.AreEqual("two", messageItems[9].Plural);
            Assert.AreEqual("!ar,!cy,!ga,!gd,!he,!iu,!mt,!sl", messageItems[9].Data);

            // ExpandedPlural.other.few
            Assert.AreEqual("ExpandedPlural.other.few", messageItems[10].ResourceId);
            Assert.AreEqual("few", messageItems[10].Plural);
            Assert.AreEqual("!ar,!be,!bs,!cs,!cy,!ga,!gd,!hr,!lt,!mt,!pl,!ro,!ru,!sk,!sl,!sr,!tt,!uk", messageItems[10].Data);

            // ExpandedPlural.other.many
            Assert.AreEqual("ExpandedPlural.other.many", messageItems[11].ResourceId);
            Assert.AreEqual("many", messageItems[11].Plural);
            Assert.AreEqual("!ar,!be,!ca,!cs,!cy,!es,!fr,!ga,!it,!lt,!mt,!pl,!pt,!quz,!ru,!sk,!tt,!uk", messageItems[11].Data);
        }
    }
}