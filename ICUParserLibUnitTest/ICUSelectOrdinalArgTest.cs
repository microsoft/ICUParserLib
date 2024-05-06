// <copyright file="ICUSelectOrdinalArgTest.cs" company="Microsoft Corporation">
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
        /// Tests the SelectOrdinal message.
        /// </summary>
        [TestMethod]
        public void TestSelectOrdinalMessage()
        {
            string input = @"{year, selectordinal,
                                one {#st}
                                two {#nd}
                                few {#rd}
                                other {#th}
                            }";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsTrue(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            // Assert.
            Assert.AreEqual(4, messageItems.Count);
            Assert.AreEqual("#st", messageItems[0].Text);
            Assert.AreEqual("SelectOrdinal.one", messageItems[0].ResourceId);
            Assert.AreEqual(1, messageItems[0].LockedSubstrings.Count);
            Assert.AreEqual("#", messageItems[0].LockedSubstrings[0]);

            Assert.AreEqual("#nd", messageItems[1].Text);
            Assert.AreEqual("SelectOrdinal.two", messageItems[1].ResourceId);
            Assert.AreEqual(1, messageItems[1].LockedSubstrings.Count);
            Assert.AreEqual("#", messageItems[1].LockedSubstrings[0]);

            Assert.AreEqual("#rd", messageItems[2].Text);
            Assert.AreEqual("SelectOrdinal.few", messageItems[2].ResourceId);
            Assert.AreEqual(1, messageItems[2].LockedSubstrings.Count);
            Assert.AreEqual("#", messageItems[2].LockedSubstrings[0]);

            Assert.AreEqual("#th", messageItems[3].Text);
            Assert.AreEqual("SelectOrdinal.other", messageItems[3].ResourceId);
            Assert.AreEqual(1, messageItems[3].LockedSubstrings.Count);
            Assert.AreEqual("#", messageItems[3].LockedSubstrings[0]);

            // Modify the strings, check if the composed string is different, revert back and test again.
            this.PostTestStringCheck(icuParser, messageItems);
        }
    }
}
