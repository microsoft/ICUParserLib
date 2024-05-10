// <copyright file="ICUSimpleArgTest.cs" company="Microsoft Corporation">
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
        /// Tests the simple arg message.
        /// </summary>
        [TestMethod]
        public void TestSimpleArgMessage()
        {
            string input = @" {1, number}{1, number, integer}{1, number, currency}
                             {1, date}{1, date, short}{1, number, full}
                             {1, time}{1, time, short}{1, time, full}
                             {1, spellout}{1, spellout, ::text}{1, spellout, medium}
                             {1, ordinal}{1, date, short}{1, number, long}
                             {1, duration}{1, date, short}{1, number, percent}
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
            Assert.AreEqual(18, messageItems[0].LockedSubstrings.Count);
        }

        /// <summary>
        /// Tests an invalid simple arg message.
        /// </summary>
        [TestMethod]
        public void TestInvalidSimpleArgMessage()
        {
            string input = @"{1, unknown}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsFalse(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);
        }

        /// <summary>
        /// Tests the simple arg message.
        /// </summary>
        [TestMethod]
        public void TestUnclosedSimpleArgMessage()
        {
            string input = @"{1, time, short";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsFalse(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);
        }

        /// <summary>
        /// Tests the Edge Date argument.
        /// </summary>
        [TestMethod]
        public void TestEdgeDateArgument()
        {
            // Initialize.
            string input = @"<ph name=""YEAR""><ex>2016</ex>{0,date,y}</ph> Microsoft Corporation. All rights reserved.";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(1, messageItems.Count);
            Assert.AreEqual("<ph name=\"YEAR\"><ex>2016</ex>{0,date,y}</ph> Microsoft Corporation. All rights reserved.", messageItems[0].Text);
            Assert.AreEqual("{0,date,y}", messageItems[0].LockedSubstrings[0]);

            // Modify the strings, check if the composed string is different, revert back and test again.
            this.PostTestStringCheck(icuParser, messageItems);
        }

        /// <summary>
        /// Tests the Edge rating argument.
        /// </summary>
        [TestMethod]
        public void TestEdgeRatingArgument()
        {
            // Initialize.
            string input = @"{1, plural,
                  =1 {Rated <ph name=""AVERAGE_RATING""><ex>3.2</ex>{0, number,0.0}</ph> by one user.}
                  other{Rated <ph name=""AVERAGE_RATING""><ex>3.2</ex>{0, number,0.0}</ph> by # users.}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsTrue(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("my"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(7, messageItems.Count);
            Assert.AreEqual("Rated <ph name=\"AVERAGE_RATING\"><ex>3.2</ex>{0, number,0.0}</ph> by one user.", messageItems[0].Text);
            Assert.AreEqual("{0, number,0.0}", messageItems[0].LockedSubstrings[0]);
            Assert.AreEqual("Rated <ph name=\"AVERAGE_RATING\"><ex>3.2</ex>{0, number,0.0}</ph> by # users.", messageItems[1].Text);
            Assert.AreEqual("{0, number,0.0}", messageItems[1].LockedSubstrings[0]);
        }
    }
}
