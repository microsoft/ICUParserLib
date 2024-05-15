// <copyright file="ICUPluralArgTest.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace ICUParserLibUnitTest
{
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
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralMessageWithLockedSubstring()
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
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("my"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(7, messageItems.Count);
            Assert.AreEqual(" Relaunch Microsoft Edge within a day", messageItems[0].Text);
            Assert.AreEqual(" Relaunch Microsoft Edge within # days", messageItems[1].Text);
            Assert.AreEqual("#", messageItems[1].LockedSubstrings[0]);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralMessageOneOtherSelector()
        {
            // Initialize.
            string input = @"{days, plural,
                one {Relaunch Microsoft Edge within # day}
                other {Relaunch Microsoft Edge within # days}}";

            ICUParser icuParser = new ICUParser(input, true);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsTrue(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            // Assert.
            Assert.AreEqual(6, messageItems.Count);
            Assert.AreEqual("one", messageItems[0].Plural);
            Assert.AreEqual("Relaunch Microsoft Edge within # day", messageItems[0].Text);

            Assert.AreEqual("other", messageItems[1].Plural);
            Assert.AreEqual("Relaunch Microsoft Edge within # days", messageItems[1].Text);

            Assert.AreEqual("zero", messageItems[2].Plural);
            Assert.AreEqual("Relaunch Microsoft Edge within # days", messageItems[2].Text);

            Assert.AreEqual("two", messageItems[3].Plural);
            Assert.AreEqual("Relaunch Microsoft Edge within # days", messageItems[3].Text);

            Assert.AreEqual("few", messageItems[4].Plural);
            Assert.AreEqual("Relaunch Microsoft Edge within # days", messageItems[4].Text);

            Assert.AreEqual("many", messageItems[5].Plural);
            Assert.AreEqual("Relaunch Microsoft Edge within # days", messageItems[5].Text);

            // Composed string for 'fr'.
            string outputFR = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("fr"));

            // {days, plural,
            //                one {Relaunch Microsoft Edge within # day}
            //                many {Relaunch Microsoft Edge within # days}
            //                other {Relaunch Microsoft Edge within # days}}
            Assert.AreEqual("{days, plural,\r\n                one {Relaunch Microsoft Edge within # day}\r\n                many {Relaunch Microsoft Edge within # days}\r\n                other {Relaunch Microsoft Edge within # days}}", outputFR);

            // Composed string for 'ja'.
            string outputJA = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("ja"));

            // {days, plural,
            //                other { Relaunch Microsoft Edge within # days}}
            Assert.AreEqual("{days, plural,\r\n                \r\n                other {Relaunch Microsoft Edge within # days}}", outputJA);

            // Composed string for 'ar'.
            string outputAR = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("ar"));

            // {days, plural,
            //                one {Relaunch Microsoft Edge within # day}
            //                zero {Relaunch Microsoft Edge within # days}
            //                two {Relaunch Microsoft Edge within # days}
            //                few {Relaunch Microsoft Edge within # days}
            //                many {Relaunch Microsoft Edge within # days}
            //                other {Relaunch Microsoft Edge within # days}}
            Assert.AreEqual("{days, plural,\r\n                one {Relaunch Microsoft Edge within # day}\r\n                zero {Relaunch Microsoft Edge within # days}\r\n                two {Relaunch Microsoft Edge within # days}\r\n                few {Relaunch Microsoft Edge within # days}\r\n                many {Relaunch Microsoft Edge within # days}\r\n                other {Relaunch Microsoft Edge within # days}}", outputAR);

            // Verify order.
            Assert.IsTrue(outputAR.IndexOf("other") > outputAR.IndexOf("one"));
            Assert.IsTrue(outputAR.IndexOf("other") > outputAR.IndexOf("zero"));
            Assert.IsTrue(outputAR.IndexOf("other") > outputAR.IndexOf("two"));
            Assert.IsTrue(outputAR.IndexOf("other") > outputAR.IndexOf("many"));
            Assert.IsTrue(outputAR.IndexOf("other") > outputAR.IndexOf("few"));
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralMessageWithStringArgName()
        {
            // Initialize.
            string input = @"{NUM_DOWNLOAD, plural,
                =1 {You might lose it if you close the browser.}
                other {You might lose them if you close the browser.}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("my"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(7, messageItems.Count);
            Assert.AreEqual("You might lose it if you close the browser.", messageItems[0].Text);
            Assert.AreEqual("You might lose them if you close the browser.", messageItems[1].Text);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralMessageLargeSelector()
        {
            // Initialize.
            string input = @"{notifications, plural,
              zero {no notifications}
               one {one notification}
               =42 {a universal amount of notifications}
             other {# notifications}
            }";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsTrue(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("lv"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(7, messageItems.Count);
            Assert.AreEqual("no notifications", messageItems[0].Text);
            Assert.AreEqual("one notification", messageItems[1].Text);
            Assert.AreEqual("a universal amount of notifications", messageItems[2].Text);
            Assert.AreEqual("# notifications", messageItems[3].Text);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralMessageStrictParseWhiteSpace()
        {
            // Initialize.
            string input = @"  {0, plural,
                =1 { Relaunch Microsoft Edge within a day}
                other { Relaunch Microsoft Edge within # days}}  ";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsTrue(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("my"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(7, messageItems.Count);
            Assert.AreEqual(" Relaunch Microsoft Edge within a day", messageItems[0].Text);
            Assert.AreEqual(" Relaunch Microsoft Edge within # days", messageItems[1].Text);
            Assert.AreEqual("#", messageItems[1].LockedSubstrings[0]);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralMessageStrictParseWith0And1()
        {
            // Initialize.
            string input = @"{count, plural,
                  =0 {A Microsoft Edge update is available}
                  =1 {A Microsoft Edge update is available}
                  other {A Microsoft Edge update has been available for # days}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsTrue(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("my"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(8, messageItems.Count);
            Assert.AreEqual("A Microsoft Edge update is available", messageItems[0].Text);
            Assert.AreEqual("A Microsoft Edge update is available", messageItems[1].Text);
            Assert.AreEqual("#", messageItems[2].LockedSubstrings[0]);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralMessageLargeSelectorStrictParse()
        {
            // Initialize.
            string input = @"You have {notifications, plural,
              zero {no notifications}
               one {one notification}
               =42 {a universal amount of notifications}
             other {# notifications}
            }. Have a nice day, {name}!";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsFalse(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            Assert.AreEqual(2, icuParser.Errors.Count);
            Assert.AreEqual("Strict parse mode enabled. Content contains leading/trailing text 'You have '.", icuParser.Errors[0]);
            Assert.AreEqual("Strict parse mode enabled. Content contains leading/trailing text '. Have a nice day, {name}!'.", icuParser.Errors[1]);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralMessageInSelectMessage()
        {
            // Initialize.
            string input = @"{gender_of_host, select,
                female {
                {num_guests, plural, offset:1
                    =0 {{host} does not give a party.}
                    =1 {{host} invites {guest} to her party.}
                    =2 {{host} invites {guest} and one other person to her party.}
                    other {{host} invites {guest} and # other people to her party.}}}
                male {
                {num_guests, plural, offset:1
                    =0 {{host} does not give a party.}
                    =1 {{host} invites {guest} to his party.}
                    =2 {{host} invites {guest} and one other person to his party.}
                    other {{host} invites {guest} and # other people to his party.}}}
                other {
                {num_guests, plural, offset:1
                    =0 {{host} does not give a party.}
                    =1 {{host} invites {guest} to their party.}
                    =2 {{host} invites {guest} and one other person to their party.}
                    other {{host} invites {guest} and # other people to their party.}}}}";

            // removeDuplicateMessageStrings = true;
            ICUParser icuParser = new ICUParser(input, true);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("my"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(25, messageItems.Count);

            Assert.AreEqual("{host} does not give a party.", messageItems[0].Text);
            Assert.AreEqual("Plural.=0#0", messageItems[0].ResourceId);
            Assert.AreEqual("{host}", messageItems[0].LockedSubstrings[0]);

            Assert.AreEqual("{host} invites {guest} to her party.", messageItems[1].Text);
            Assert.AreEqual("Plural.=1#0", messageItems[1].ResourceId);
            Assert.AreEqual("{host}", messageItems[1].LockedSubstrings[0]);
            Assert.AreEqual("{guest}", messageItems[1].LockedSubstrings[1]);
            Assert.AreEqual("{host} invites {guest} and one other person to her party.", messageItems[2].Text);
            Assert.AreEqual("Plural.=2#0", messageItems[2].ResourceId);

            Assert.AreEqual("{host}", messageItems[2].LockedSubstrings[0]);
            Assert.AreEqual("{guest}", messageItems[2].LockedSubstrings[1]);
            Assert.AreEqual("{host} invites {guest} and # other people to her party.", messageItems[3].Text);
            Assert.AreEqual("Plural.other#0", messageItems[3].ResourceId);

            Assert.AreEqual("{host}", messageItems[3].LockedSubstrings[0]);
            Assert.AreEqual("{guest}", messageItems[3].LockedSubstrings[1]);
            Assert.AreEqual("#", messageItems[3].LockedSubstrings[2]);
            Assert.AreEqual("{host} invites {guest} to his party.", messageItems[4].Text);
            Assert.AreEqual("Plural.=1#1", messageItems[4].ResourceId);

            Assert.AreEqual("{host}", messageItems[4].LockedSubstrings[0]);
            Assert.AreEqual("{guest}", messageItems[4].LockedSubstrings[1]);
            Assert.AreEqual("{host} invites {guest} and one other person to his party.", messageItems[5].Text);
            Assert.AreEqual("Plural.=2#1", messageItems[5].ResourceId);

            Assert.AreEqual("{host}", messageItems[5].LockedSubstrings[0]);
            Assert.AreEqual("{guest}", messageItems[5].LockedSubstrings[1]);
            Assert.AreEqual("{host} invites {guest} and # other people to his party.", messageItems[6].Text);
            Assert.AreEqual("Plural.other#1", messageItems[6].ResourceId);
            Assert.AreEqual("{host}", messageItems[6].LockedSubstrings[0]);
            Assert.AreEqual("{guest}", messageItems[6].LockedSubstrings[1]);
            Assert.AreEqual("#", messageItems[6].LockedSubstrings[2]);

            Assert.AreEqual("{host} invites {guest} to their party.", messageItems[7].Text);
            Assert.AreEqual("Plural.=1#2", messageItems[7].ResourceId);
            Assert.AreEqual("{host}", messageItems[7].LockedSubstrings[0]);
            Assert.AreEqual("{guest}", messageItems[7].LockedSubstrings[1]);

            Assert.AreEqual("{host} invites {guest} and one other person to their party.", messageItems[8].Text);
            Assert.AreEqual("Plural.=2#2", messageItems[8].ResourceId);
            Assert.AreEqual("{host}", messageItems[8].LockedSubstrings[0]);
            Assert.AreEqual("{guest}", messageItems[8].LockedSubstrings[1]);

            Assert.AreEqual("{host} invites {guest} and # other people to their party.", messageItems[9].Text);
            Assert.AreEqual("Plural.other#2", messageItems[9].ResourceId);
            Assert.AreEqual("{host}", messageItems[9].LockedSubstrings[0]);
            Assert.AreEqual("{guest}", messageItems[9].LockedSubstrings[1]);
            Assert.AreEqual("#", messageItems[9].LockedSubstrings[2]);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralMessageInSelectMessageExpandedPlurals()
        {
            // Initialize.
            string input = @"{gender_of_host, select,
                female {
                {num_guests, plural, offset:1
                    =0 {{host} does not give a party.}
                    =1 {{host} invites {guest} to her party.}
                    =2 {{host} invites {guest} and one other person to her party.}
                    other { female {host} invites {guest} and # other people to her party.}}}
                male {
                {num_guests, plural, offset:1
                    =0 {{host} does not give a party.}
                    =1 {{host} invites {guest} to his party.}
                    =2 {{host} invites {guest} and one other person to his party.}
                    other { male {host} invites {guest} and # other people to his party.}}}
                other {
                {num_guests, plural, offset:1
                    =0 {{host} does not give a party.}
                    =1 {{host} invites {guest} to their party.}
                    =2 {{host} invites {guest} and one other person to their party.}
                    other { other {host} invites {guest} and # other people to their party.}}}}";

            // removeDuplicateMessageStrings = true;
            ICUParser icuParser = new ICUParser(input, true);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            /*
            {gender_of_host, select,
                female {
                {num_guests, plural, offset:1
                    =0 {{host} does not give a party.}
                    =1 {{host} invites {guest} to her party.}
                    =2 {{host} invites {guest} and one other person to her party.}
                    zero { female {host} invites {guest} and # other people to her party.}
                    one { female {host} invites {guest} and # other people to her party.}
                    two { female {host} invites {guest} and # other people to her party.}
                    few { female {host} invites {guest} and # other people to her party.}
                    many { female {host} invites {guest} and # other people to her party.}
                    other { female {host} invites {guest} and # other people to her party.}}}
                male {
                {num_guests, plural, offset:1
                    =0 {{host} does not give a party.}
                    =1 {{host} invites {guest} to his party.}
                    =2 {{host} invites {guest} and one other person to his party.}
                    zero { male {host} invites {guest} and # other people to his party.}
                    one { male {host} invites {guest} and # other people to his party.}
                    two { male {host} invites {guest} and # other people to his party.}
                    few { male {host} invites {guest} and # other people to his party.}
                    many { male {host} invites {guest} and # other people to his party.}
                    other { male {host} invites {guest} and # other people to his party.}}}
                other {
                {num_guests, plural, offset:1
                    =0 {{host} does not give a party.}
                    =1 {{host} invites {guest} to their party.}
                    =2 {{host} invites {guest} and one other person to their party.}
                    zero { other {host} invites {guest} and # other people to their party.}
                    one { other {host} invites {guest} and # other people to their party.}
                    two { other {host} invites {guest} and # other people to their party.}
                    few { other {host} invites {guest} and # other people to their party.}
                    many { other {host} invites {guest} and # other people to their party.}
                    other { other {host} invites {guest} and # other people to their party.}}}}
            */

            // Assert.
            Assert.IsTrue(output.Length > input.Length);
            Assert.AreEqual(25, messageItems.Count);
            Assert.AreEqual(" female {host} invites {guest} and # other people to her party.", messageItems[10].Text);
            Assert.AreEqual(" male {host} invites {guest} and # other people to his party.", messageItems[15].Text);
            Assert.AreEqual(" other {host} invites {guest} and # other people to their party.", messageItems[20].Text);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralMessagePluralFillInText()
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
            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreNotEqual(input, output, "Same text output.");
            Assert.AreEqual(7, messageItems.Count);
            string otherText = " Relaunch Microsoft Edge within # days";
            Assert.AreEqual(otherText, messageItems[2].Text);
            Assert.AreEqual(otherText, messageItems[3].Text);
            Assert.AreEqual(otherText, messageItems[4].Text);
            Assert.AreEqual(otherText, messageItems[5].Text);
            Assert.AreEqual(otherText, messageItems[6].Text);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralMessageEmptyId()
        {
            // Initialize.
            string input = @"{DOC_MODE_VERSION, plural,other {#}}";

            ICUParser icuParser = new ICUParser(input, true);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsTrue(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("my"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(6, messageItems.Count);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralMessageAddPluralOtherSelector()
        {
            string input = @"{count, plural,
                =1 { Relaunch Microsoft Edge within a day}
                other { Relaunch Microsoft Edge within # days}}";

            ICUParser icuParser = new ICUParser(input, true);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            Assert.AreEqual(7, messageItems.Count);

            List<MessageItem> pluralMessageItems = messageItems.Where(messageItem => messageItem.MessageItemType == MessageItemTypeEnum.ExpandedPlural).ToList();
            Assert.AreEqual(5, pluralMessageItems.Count);

            // Add plural translations.
            int i = 0;
            foreach (MessageItem pluralMessageItem in pluralMessageItems)
            {
                pluralMessageItem.Text = $"{pluralMessageItem.ResourceId}.{i++}";
            }

            string output = icuParser.ComposeMessageText(messageItems);

            // Verify the composed output.
            ICUParser icuParserVerify = new ICUParser(output);

            // Assert.
            Assert.IsTrue(icuParserVerify.Success);
            Assert.IsTrue(output.Length > input.Length);
            Assert.AreEqual(" Relaunch Microsoft Edge within a day", messageItems[0].Text);
            Assert.AreEqual(" Relaunch Microsoft Edge within # days", messageItems[1].Text);
            Assert.AreEqual("#", messageItems[1].LockedSubstrings[0]);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralMessageAddPluralNoOtherSelector()
        {
            string input = @"{count, plural,
                =1 { Relaunch Microsoft Edge within a day}
                other { Relaunch Microsoft Edge within # days}}";

            ICUParser icuParser = new ICUParser(input, true);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            Assert.AreEqual(7, messageItems.Count);

            List<MessageItem> pluralMessageItems = messageItems.Where(messageItem => messageItem.MessageItemType == MessageItemTypeEnum.ExpandedPlural).ToList();
            Assert.AreEqual(5, pluralMessageItems.Count);

            // Add plural translations.
            int i = 0;
            foreach (MessageItem pluralMessageItem in pluralMessageItems)
            {
                pluralMessageItem.Text = $"{pluralMessageItem.ResourceId}.{i++}";
            }

            string output = icuParser.ComposeMessageText(messageItems);

            // Verify the composed output.
            ICUParser icuParserVerify = new ICUParser(output);

            // Assert.
            Assert.IsTrue(icuParserVerify.Success);
            Assert.IsTrue(output.Length > input.Length);
            Assert.AreEqual(" Relaunch Microsoft Edge within a day", messageItems[0].Text);
            Assert.AreEqual(" Relaunch Microsoft Edge within # days", messageItems[1].Text);
            Assert.AreEqual("#", messageItems[1].LockedSubstrings[0]);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralMessagePluralId()
        {
            string input = @"{count, plural,
                =1 { 
                    {1, select, female {
                        {0, plural,
                        =1 { 1Relaunch Microsoft Edge within a day}
                        =2 { 1Relaunch Microsoft Edge within # days}
                        other { 1Relaunch Microsoft Edge within # days}}
                        } 
                other {allé}}
                }
                other { 2Relaunch Microsoft Edge within # days}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsTrue(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            /*
            {0, plural,
                =1 {
                    {1, select, female {
                        {0, plural,
                        =1 { 1Relaunch Microsoft Edge within a day}
                        =2 { 1Relaunch Microsoft Edge within # days}
                        zero { 1Relaunch Microsoft Edge within # days}
                        one { 1Relaunch Microsoft Edge within # days}
                        two { 1Relaunch Microsoft Edge within # days}
                        few { 1Relaunch Microsoft Edge within # days}
                        many { 1Relaunch Microsoft Edge within # days}
                        other { 1Relaunch Microsoft Edge within # days}}
                    allée}
                other {allé}} à Paris.
                }
                zero { 2Relaunch Microsoft Edge within # days}
                one { 2Relaunch Microsoft Edge within # days}
                two { 2Relaunch Microsoft Edge within # days}
                few { 2Relaunch Microsoft Edge within # days}
                many { 2Relaunch Microsoft Edge within # days}
                other { 2Relaunch Microsoft Edge within # days}}
            */

            Assert.IsTrue(output.Length > input.Length);
            Assert.AreEqual(15, messageItems.Count);
            Assert.AreEqual("ExpandedPlural.=1.female.zero", messageItems[10].ResourceId);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralMessageAddPluralWithPlaceholder()
        {
            string input = @"{1, plural,
                 =1 {This server could not prove that it is <ph name=""DOMAIN""><ex>paypal.com</ex>&lt;strong&gt;{0}&lt;/strong&gt;</ph>; its security certificate expired in the last day. This may be caused by a misconfiguration or an attacker intercepting your connection. Your computer&apos;s clock is currently set to <ph name=""CURRENT_DATE""><ex>Monday, July 16, 2012</ex>{2, date, full}</ph>. Does that look right? If not, you should correct your system&apos;s clock and then refresh this page.}
                 other {This server could not prove that it is <ph name=""DOMAIN""><ex>paypal.com</ex>&lt;strong&gt;{0}&lt;/strong&gt;</ph>; its security certificate expired # days ago. This may be caused by a misconfiguration or an attacker intercepting your connection. Your computer&apos;s clock is currently set to <ph name=""CURRENT_DATE""><ex>Monday, July 16, 2012</ex>{2, date, full}</ph>. Does that look right? If not, you should correct your system&apos;s clock and then refresh this page.}}";

            ICUParser icuParser = new ICUParser(input, true);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            Assert.AreEqual(7, messageItems.Count);

            List<MessageItem> pluralMessageItems = messageItems.Where(messageItem => messageItem.MessageItemType == MessageItemTypeEnum.ExpandedPlural).ToList();
            Assert.AreEqual(5, pluralMessageItems.Count);

            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("my"));

            // Verify the composed output.
            ICUParser icuParserVerify = new ICUParser(output);

            // Assert.
            Assert.IsTrue(icuParserVerify.Success);
            Assert.AreEqual(input.Length, output.Length);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralMessageAddPluralLockExpandedPlurals()
        {
            string input = @"{num_guests, plural, offset:1 =0 {[{host} ]} other {[{host} {guest}]} one {[{host} {guest}]}}";

            ICUParser icuParser = new ICUParser(input, true);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            Assert.AreEqual(6, messageItems.Count);

            Assert.AreEqual("{host}", messageItems[5].LockedSubstrings[0]);
            Assert.AreEqual("{guest}", messageItems[5].LockedSubstrings[1]);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralMessageAddPluralGenderExample()
        {
            string input = @"{gender_of_host, select,
                female {
                {num_guests, plural, offset:1
                    =0 {{host} does not give a party.}
                    =1 {{host} invites {guest} to her party.}
                    =2 {{host} invites {guest} and one other person to her party.}
                    other {{host} invites {guest} and # other people to her party.}}}
                male {
                {num_guests, plural, offset:1
                    =0 {{host} does not give a party.}
                    =1 {{host} invites {guest} to his party.}
                    =2 {{host} invites {guest} and one other person to his party.}
                    other {{host} invites {guest} and # other people to his party.}}}
                other {
                {num_guests, plural, offset:1
                    =0 {{host} does not give a party.}
                    =1 {{host} invites {guest} to their party.}
                    =2 {{host} invites {guest} and one other person to their party.}
                    other {{host} invites {guest} and # other people to their party.}}}}";

            ICUParser icuParser = new ICUParser(input, true);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            Assert.AreEqual(25, messageItems.Count);

            List<MessageItem> pluralMessageItems = messageItems.Where(messageItem => messageItem.MessageItemType == MessageItemTypeEnum.ExpandedPlural).ToList();
            Assert.AreEqual(15, pluralMessageItems.Count);
            Assert.AreEqual("ExpandedPlural.female.zero", pluralMessageItems[0].ResourceId);
            Assert.AreEqual("ExpandedPlural.female.one", pluralMessageItems[1].ResourceId);
            Assert.AreEqual("ExpandedPlural.female.two", pluralMessageItems[2].ResourceId);
            Assert.AreEqual("ExpandedPlural.female.few", pluralMessageItems[3].ResourceId);
            Assert.AreEqual("ExpandedPlural.female.many", pluralMessageItems[4].ResourceId);
            Assert.AreEqual("ExpandedPlural.male.zero", pluralMessageItems[5].ResourceId);
            Assert.AreEqual("ExpandedPlural.male.one", pluralMessageItems[6].ResourceId);
            Assert.AreEqual("ExpandedPlural.male.two", pluralMessageItems[7].ResourceId);
            Assert.AreEqual("ExpandedPlural.male.few", pluralMessageItems[8].ResourceId);
            Assert.AreEqual("ExpandedPlural.male.many", pluralMessageItems[9].ResourceId);
            Assert.AreEqual("ExpandedPlural.other.zero", pluralMessageItems[10].ResourceId);
            Assert.AreEqual("ExpandedPlural.other.one", pluralMessageItems[11].ResourceId);
            Assert.AreEqual("ExpandedPlural.other.two", pluralMessageItems[12].ResourceId);
            Assert.AreEqual("ExpandedPlural.other.few", pluralMessageItems[13].ResourceId);
            Assert.AreEqual("ExpandedPlural.other.many", pluralMessageItems[14].ResourceId);

            // Add plural translations.
            int i = 0;
            foreach (MessageItem pluralMessageItem in pluralMessageItems)
            {
                pluralMessageItem.Text = $"{pluralMessageItem.ResourceId}.{i++}";
            }

            string output = icuParser.ComposeMessageText(messageItems);

            // Verify the composed output.
            ICUParser icuParserVerify = new ICUParser(output);

            // Assert.
            Assert.IsTrue(icuParserVerify.Success);
            Assert.IsTrue(output.Length > input.Length);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralMessageWithDuplicateRemoval()
        {
            // Initialize.
            string input = @"{count, plural, 
                =1{Relaunch Microsoft Edge within {testNoneArgText} a day} 
                =2{Relaunch Microsoft Edge within {testNoneArgText} a day}
                other {Relaunch Microsoft Edge within # days}}";

            // removeDuplicateMessageStrings = true;
            ICUParser icuParser = new ICUParser(input, true);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("my"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(7, messageItems.Count);

            // Duplicate string removal is active.
            Assert.AreEqual("Relaunch Microsoft Edge within {testNoneArgText} a day", messageItems[0].Text);
            Assert.AreEqual("{testNoneArgText}", messageItems[0].LockedSubstrings[0]);
            Assert.AreEqual("Relaunch Microsoft Edge within # days", messageItems[4].Text);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralMessageWithInvalidOffsetKeyword()
        {
            // Initialize.
            string input = @"{num_guests, plural, no_offset_keyword:1
                    =0 {{host} does not give a party.}
                    =1 {{host} invites {guest} to her party.}
                    =2 {{host} invites {guest} and one other person to her party.}
                    other {{host} invites {guest} and # other people to her party.}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsFalse(icuParser.Success);
            Assert.AreEqual("Line 1, pos 38: token recognition error at: ':1'", icuParser.Errors[0]);
            Assert.AreEqual("Error in line 2, pos 20: mismatched input '=' expecting ARGUMENT_BEGIN", icuParser.Errors[1]);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralMessageWithMissingOffsetColon()
        {
            // Initialize.
            string input = @"{num_guests, plural, offset 1
                    =0 {{host} does not give a party.}
                    =1 {{host} invites {guest} to her party.}
                    =2 {{host} invites {guest} and one other person to her party.}
                    other {{host} invites {guest} and # other people to her party.}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsFalse(icuParser.Success);
            Assert.AreEqual("Error in line 1, pos 28: mismatched input '1' expecting ARGUMENT_BEGIN", icuParser.Errors[0]);
        }

        /// <summary>
        /// Tests an invalid plural message.
        /// </summary>
        [TestMethod]
        public void TestInvalidPluralMessage()
        {
            // Initialize.
            string input = @"{NUM_DOWNLOAD, plural,
                == 1 {You might lose it if you close the browser.}
                other {You might lose it if you close the browser.}}";

            ICUParser icuParser = new ICUParser(input, true);

            // Assert.
            Assert.IsFalse(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(1, messageItems.Count);
            Assert.AreEqual(input, output, "Different text output.");

            string translation = "abc";
            messageItems[0].Text = translation;
            output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(translation, output, "Different text output.");
        }

        /// <summary>
        /// Tests an invalid plural message.
        /// </summary>
        [TestMethod]
        public void TestInvalidSyntaxPluralMessage()
        {
            // Initialize.
            string input = @"{{, plural,
                == 1 {You might lose it if you close the browser.}
                other {You might lose them if you close the browser.}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsFalse(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(1, messageItems.Count);
            Assert.AreEqual(input, output, "Different text output.");
        }

        /// <summary>
        /// Tests the Edge plural message.
        /// </summary>
        [TestMethod]
        public void TestEdgePluralMessage()
        {
            // Initialize.
            string input = @"{NUM_OTHER_TABS, plural, 
                =0 {<ph name=""TAB_TITLE""><ex>Google News</ex>$1</ph>} 
                =1 {""<ph name=""TAB_TITLE""><ex>Google News</ex>$1</ph>"" and 1 other tab} 
                other {<ph name=""TAB_TITLE""><ex>Google News</ex>$1</ph> and # other tabs}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("my"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(8, messageItems.Count);

            Assert.AreEqual(@"<ph name=""TAB_TITLE""><ex>Google News</ex>$1</ph>", messageItems[0].Text);
            Assert.AreEqual("Plural.=0", messageItems[0].ResourceId);

            Assert.AreEqual(@"""<ph name=""TAB_TITLE""><ex>Google News</ex>$1</ph>"" and 1 other tab", messageItems[1].Text);
            Assert.AreEqual("Plural.=1", messageItems[1].ResourceId);

            Assert.AreEqual(@"<ph name=""TAB_TITLE""><ex>Google News</ex>$1</ph> and # other tabs", messageItems[2].Text);
            Assert.AreEqual("Plural.other", messageItems[2].ResourceId);
            Assert.AreEqual("#", messageItems[2].LockedSubstrings[0]);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestNumberArgTypePluralMessage()
        {
            // Initialize.
            string input = @"{number, plural, =1{Call group} other{Call group}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("my"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(7, messageItems.Count);

            Assert.AreEqual("Call group", messageItems[0].Text);
            Assert.AreEqual("Plural.=1", messageItems[0].ResourceId);

            Assert.AreEqual("Call group", messageItems[1].Text);
            Assert.AreEqual("Plural.other", messageItems[1].ResourceId);
        }

        /// <summary>
        /// Tests the plural message.
        /// </summary>
        [TestMethod]
        public void TestPluralKeywordPluralMessage()
        {
            // Initialize.
            string input = @"{plural, plural, =1{Call group} other{Call group}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("my"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(7, messageItems.Count);
        }
    }
}
