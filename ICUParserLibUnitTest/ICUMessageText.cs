// <copyright file="ICUMessageText.cs" company="Microsoft Corporation">
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
    public partial class ICUTest
    {
        /// <summary>
        /// Tests the message text.
        /// </summary>
        [TestMethod]
        public void TestMessageTextOnlyMessage1()
        {
            // Initialize.
            string input = @"test {name} message text";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            // Assert.
            Assert.AreEqual(1, messageItems.Count);
        }

        /// <summary>
        /// Tests the message text.
        /// </summary>
        [TestMethod]
        public void TestMessageTextOnlyMessage2()
        {
            // Initialize.
            string input = @"test {0, number, 0.0} message text";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            // Assert.
            Assert.AreEqual(1, messageItems.Count);
        }

        /// <summary>
        /// Tests the message text.
        /// </summary>
        [TestMethod]
        public void TestMessageTextOnlyMessage3()
        {
            // Initialize.
            string input = @"test ""name"" message text";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            // Assert.
            Assert.AreEqual(1, messageItems.Count);
        }

        /// <summary>
        /// Tests the message text.
        /// </summary>
        [TestMethod]
        public void TestMessageTextOnlyMessage4()
        {
            // Initialize.
            string input = @"test "" } "" message text";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            // Assert.
            Assert.AreEqual(1, messageItems.Count);
        }

        /// <summary>
        /// Tests the message text.
        /// </summary>
        [TestMethod]
        public void TestMessageTextUnbalancedBracket()
        {
            // Initialize.
            string input = @"test } {name} message text";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsFalse(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            // Assert.
            Assert.AreEqual(1, icuParser.Errors.Count);
            Assert.AreEqual("ANTLR Exception: Operation is not valid due to the current state of the object.", icuParser.Errors[0]);
        }

        /// <summary>
        /// Tests the message text.
        /// </summary>
        [TestMethod]
        public void TestMessageTextInvalidPlaceholder1()
        {
            // Initialize.
            string input = @"test { name is not valid } message text";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsFalse(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            // Assert.
            Assert.AreEqual(1, icuParser.Errors.Count);
            Assert.AreEqual("Error in line 1, pos 12: no viable alternative at input '{nameis'", icuParser.Errors[0]);
        }

        /// <summary>
        /// Tests the message text.
        /// </summary>
        [TestMethod]
        public void TestMessageTextInvalidPlaceholder2()
        {
            // Initialize.
            string input = @"{ test name is not valid message text }";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsFalse(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            // Assert.
            Assert.AreEqual(1, icuParser.Errors.Count);
            Assert.AreEqual("Error in line 1, pos 7: no viable alternative at input '{testname'", icuParser.Errors[0]);
        }

        /// <summary>
        /// Tests the message text.
        /// </summary>
        [TestMethod]
        public void TestMessageTextUnbalancedQuote()
        {
            // Initialize.
            string input = @"test {name} ""message text";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsFalse(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            // Assert.
            Assert.AreEqual(1, icuParser.Errors.Count);
            Assert.AreEqual("Line 1, pos 12: token recognition error at: '\"message text'", icuParser.Errors[0]);
        }
    }
}
