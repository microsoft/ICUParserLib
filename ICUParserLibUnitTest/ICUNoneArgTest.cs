// <copyright file="ICUNoneArgTest.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace ICUParserLibUnitTest
{
    using System.Collections.Generic;
    using ICUParserLib;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests <see cref="ICUParser"/>.
    /// </summary>
    [TestClass]
    public partial class ICUTest
    {
        /// <summary>
        /// Tests the none argument message as part of the translatable string.
        /// </summary>
        [TestMethod]
        public void TestNoneArgMessage()
        {
            // Initialize.
            string input = @" # {NoneArgText}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            // {NoneArgText} is added as not localizable content in the string.
            Assert.AreEqual(1, messageItems.Count);
            Assert.AreEqual(input, messageItems[0].Text);
            Assert.AreEqual(1, messageItems[0].LockedSubstrings.Count);
            Assert.AreEqual("{NoneArgText}", messageItems[0].LockedSubstrings[0]);

            // Modify the strings, check if the composed string is different, revert back and test again.
            this.PostTestStringCheck(icuParser, messageItems);
        }

        /// <summary>
        /// Tests the none argument message as part of the translatable string.
        /// </summary>
        [TestMethod]
        public void TestNoneArgMessageStrictParse()
        {
            // Initialize.
            string input = @"{NoneArgText}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            // {NoneArgText} is added as not localizable content in the string.
            Assert.AreEqual(1, messageItems.Count);
            Assert.AreEqual(input, messageItems[0].Text);
            Assert.AreEqual(1, messageItems[0].LockedSubstrings.Count);
            Assert.AreEqual(input, messageItems[0].LockedSubstrings[0]);
        }

        /// <summary>
        /// Tests the none argument message as part of the translatable string.
        /// </summary>
        [TestMethod]
        public void TestNoneArgMessageWithText()
        {
            // Initialize.
            string input = @"Textstart {NoneArgText} TextEnd";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            // {NoneArgText} is added as not localizable content in the string.
            Assert.AreEqual(1, messageItems.Count);
            Assert.AreEqual(input, messageItems[0].Text);
            Assert.AreEqual("{NoneArgText}", messageItems[0].LockedSubstrings[0]);

            // Modify the strings, check if the composed string is different, revert back and test again.
            this.PostTestStringCheck(icuParser, messageItems);
        }

        /// <summary>
        /// Tests the none argument message as part of the translatable string.
        /// </summary>
        [TestMethod]
        public void TestNoneArgMessageWithTextStrictParse()
        {
            // Initialize.
            string input = @"Textstart {NoneArgText} TextEnd";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            Assert.AreEqual(1, messageItems.Count);
            Assert.AreEqual(input, messageItems[0].Text);
            Assert.AreEqual("{NoneArgText}", messageItems[0].LockedSubstrings[0]);
        }
    }
}
