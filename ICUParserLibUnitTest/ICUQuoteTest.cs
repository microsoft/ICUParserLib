// <copyright file="ICUQuoteTest.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace ICUParserLibUnitTest
{
    using System.Collections.Generic;
    using System.Globalization;
    using ICUParserLib;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests <see cref="ICUParser"/>.
    /// </summary>
    public partial class ICUTest
    {
        /// <summary>
        /// Tests the text message with quoting.
        /// </summary>
        [TestMethod]
        public void TestUnicodeChar2019QuoteMessage()
        {
            string input = "We’re still working to support searching for such file names";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(1, messageItems.Count);
            Assert.AreEqual(input, messageItems[0].Text);
        }

        /// <summary>
        /// Tests the text message with quoting.
        /// </summary>
        [TestMethod]
        public void TestDoubleQuoteMessage()
        {
            string input = @"a ""{"" b";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(1, messageItems.Count);
            Assert.AreEqual(input, messageItems[0].Text);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestMessageWithUnbalancedQuote()
        {
            // Initialize.
            string input = @"Relaunch Microsoft"" Edge within a day";

            ICUParser icuParser = new ICUParser(input, true);

            // Assert.
            Assert.IsFalse(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(1, messageItems.Count);
        }

        /// <summary>
        /// Tests the message with whitespaces.
        /// </summary>
        [TestMethod]
        public void TestMessageNoICUWithEscapedQuotes()
        {
            string input = @"Specify a list of websites to open automatically when the browser starts. If you don't configure this policy, no site is opened on startup.
                    This policy only works if you also set the <ph name=""RESTORE_ON_STARTUP_POLICY_NAME""><ex>""RestoreOnStartup""</ex>\""RestoreOnStartup\""</ph> policy to 'Open a list of URLs' (4).
                    This policy is only available on Windows instances that are joined to a <ph name=""MS_AD_NAME""><ex>Microsoft Active Directory</ex>Microsoft Active Directory</ph> domain or Windows 10 Pro or Enterprise instances that are enrolled for device management.
        ";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(1, messageItems.Count);
        }

        /// <summary>
        /// Tests the single quotes.
        /// </summary>
        [TestMethod]
        public void TestSingleQuotes()
        {
            string input = "{MINUTES, plural, =1 {1ד'} one {#ח'} two {'{123}'} other {#ח'}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsTrue(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("he-IL"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(7, messageItems.Count);
            Assert.AreEqual("1ד'", messageItems[0].Text);
            Assert.AreEqual("#ח'", messageItems[1].Text);
            Assert.AreEqual("'{123}'", messageItems[2].Text);
            Assert.AreEqual("#ח'", messageItems[3].Text);
            Assert.AreEqual("#ח'", messageItems[4].Text);
            Assert.AreEqual("#ח'", messageItems[5].Text);
            Assert.AreEqual("#ח'", messageItems[6].Text);
        }

        /// <summary>
        /// Tests the text message with escaping.
        /// </summary>
        [TestMethod]
        public void TestEscapedCharMessage()
        {
            string input = @"a \\{ b";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsFalse(icuParser.Success);

            Assert.AreEqual(1, icuParser.Errors.Count);
            Assert.AreEqual("Error in line 1, pos 7: no viable alternative at input '{b'", icuParser.Errors[0]);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralWithMatchedQuote()
        {
            // Initialize.
            string input = @"{COUNT, plural, one{# ""Matched Quote"" } other{# Test}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            Assert.AreEqual(6, messageItems.Count);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralWithUnmatchedQuote()
        {
            // Initialize.
            string input = @"{COUNT, plural, one{# ""Unmatched Quote} other{# Test}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            // Unmatched quotes are not allowed.
            Assert.IsFalse(icuParser.Success);

            // Expect 2 errors.
            Assert.AreEqual(2, icuParser.Errors.Count);
        }
    }
}
