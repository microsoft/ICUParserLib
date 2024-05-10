// <copyright file="ICUPluralListTest.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace ICUParserLibUnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using ICUParserLib;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests <see cref="ICUParser"/>.
    /// </summary>
    public partial class ICUTest
    {
        /// <summary>
        /// Tests the plural list API.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestPluralEmptyList()
        {
            // Initialize.
            // Plural 'other' is missing. Expect exception.
            List<MessageItem> messageItems = ICUParser.ExpandPlurals(new OrderedDictionary(), CultureInfo.GetCultureInfo("ja"));
        }

        /// <summary>
        /// Tests the plural list API.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestPluralListNoOther()
        {
            // Initialize.
            OrderedDictionary pluralMap = new OrderedDictionary
            {
                { "one", "Text One" },
            };

            // Plural 'other' is missing. Expect exception.
            List<MessageItem> messageItems = ICUParser.ExpandPlurals(pluralMap, CultureInfo.GetCultureInfo("en"));
        }

        /// <summary>
        /// Tests the plural list API.
        /// </summary>
        [TestMethod]
        public void TestPluralListOneOther()
        {
            // Initialize.
            OrderedDictionary pluralMap = new OrderedDictionary
            {
                { "one", "Text One" },
                { "other", "Text Other" },
            };

            List<MessageItem> messageItems = ICUParser.ExpandPlurals(pluralMap, CultureInfo.GetCultureInfo("de-DE"));

            // Assert.
            Assert.AreEqual(2, messageItems.Count);

            Assert.AreEqual("Text One", messageItems[0].Text);
            Assert.AreEqual("one", messageItems[0].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[0].MessageItemType);

            Assert.AreEqual("Text Other", messageItems[1].Text);
            Assert.AreEqual("other", messageItems[1].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[1].MessageItemType);
        }

        /// <summary>
        /// Tests the plural list API.
        /// </summary>
        [TestMethod]
        public void TestPluralListAll()
        {
            // Initialize.
            OrderedDictionary pluralMap = new OrderedDictionary
            {
                { "other", "Text Other" },
                { "many", "Text Many" },
                { "few", "Text Few" },
                { "zero", "Text Zero" },
                { "two", "Text Two" },
                { "one", "Text One" },
            };

            List<MessageItem> messageItems = ICUParser.ExpandPlurals(pluralMap, CultureInfo.GetCultureInfo("ar-SA"));

            // Assert.
            Assert.AreEqual(6, messageItems.Count);

            // Expect all Default type.
            Assert.AreEqual("Text Zero", messageItems[0].Text);
            Assert.AreEqual("zero", messageItems[0].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[0].MessageItemType);

            Assert.AreEqual("Text One", messageItems[1].Text);
            Assert.AreEqual("one", messageItems[1].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[1].MessageItemType);

            Assert.AreEqual("Text Two", messageItems[2].Text);
            Assert.AreEqual("two", messageItems[2].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[2].MessageItemType);

            Assert.AreEqual("Text Few", messageItems[3].Text);
            Assert.AreEqual("few", messageItems[3].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[3].MessageItemType);

            Assert.AreEqual("Text Many", messageItems[4].Text);
            Assert.AreEqual("many", messageItems[4].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[4].MessageItemType);

            Assert.AreEqual("Text Other", messageItems[5].Text);
            Assert.AreEqual("other", messageItems[5].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[5].MessageItemType);
        }

        /// <summary>
        /// Tests the plural list API.
        /// </summary>
        [TestMethod]
        public void TestPluralListOneOtherLanguage()
        {
            // Initialize.
            OrderedDictionary pluralMap = new OrderedDictionary
            {
                { "zero", "Text Zero" },
                { "one", "Text One" },
                { "two", "Text Two" },
                { "few", "Text Few" },
                { "many", "Text Many" },
                { "other", "Text Other" },
            };

            List<MessageItem> messageItems = ICUParser.ExpandPlurals(pluralMap, CultureInfo.GetCultureInfo("de-DE"));

            // Assert.
            Assert.AreEqual(2, messageItems.Count);

            // Expect only one and other.
            Assert.AreEqual("Text One", messageItems[0].Text);
            Assert.AreEqual("one", messageItems[0].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[0].MessageItemType);

            Assert.AreEqual("Text Other", messageItems[1].Text);
            Assert.AreEqual("other", messageItems[1].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[1].MessageItemType);
        }

        /// <summary>
        /// Tests the plural list API.
        /// </summary>
        [TestMethod]
        public void TestPluralListOneLanguage()
        {
            // Initialize.
            OrderedDictionary pluralMap = new OrderedDictionary
            {
                { "other", "Text Other" },
            };

            List<MessageItem> messageItems = ICUParser.ExpandPlurals(pluralMap, CultureInfo.GetCultureInfo("de-DE"));

            // Assert.
            Assert.AreEqual(2, messageItems.Count);

            // Expect only one and other and one as an expanded plural.
            Assert.AreEqual("Text Other", messageItems[0].Text);
            Assert.AreEqual("one", messageItems[0].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.ExpandedPlural, messageItems[0].MessageItemType);

            Assert.AreEqual("Text Other", messageItems[1].Text);
            Assert.AreEqual("other", messageItems[1].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[1].MessageItemType);
        }

        /// <summary>
        /// Tests the plural list API.
        /// </summary>
        [TestMethod]
        public void TestPluralListSingleJA()
        {
            // Initialize.
            OrderedDictionary pluralMap = new OrderedDictionary
            {
                { "zero", "Text Zero" },
                { "one", "Text One" },
                { "two", "Text Two" },
                { "few", "Text Few" },
                { "many", "Text Many" },
                { "other", "Text Other" },
            };

            List<MessageItem> messageItems = ICUParser.ExpandPlurals(pluralMap, CultureInfo.GetCultureInfo("ja"));

            // Assert.
            Assert.AreEqual(1, messageItems.Count);

            // Expect only one plural.
            Assert.AreEqual("Text Other", messageItems[0].Text);
            Assert.AreEqual("other", messageItems[0].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[0].MessageItemType);
        }

        /// <summary>
        /// Tests the plural list API.
        /// </summary>
        [TestMethod]
        public void TestPluralListSingleZH()
        {
            // Initialize.
            OrderedDictionary pluralMap = new OrderedDictionary
            {
                { "zero", "Text Zero" },
                { "one", "Text One" },
                { "two", "Text Two" },
                { "few", "Text Few" },
                { "many", "Text Many" },
                { "other", "Text Other" },
            };

            List<MessageItem> messageItems = ICUParser.ExpandPlurals(pluralMap, CultureInfo.GetCultureInfo("zh-hant"));

            // Assert.
            Assert.AreEqual(1, messageItems.Count);

            // Expect only one plural.
            Assert.AreEqual("Text Other", messageItems[0].Text);
            Assert.AreEqual("other", messageItems[0].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[0].MessageItemType);
        }

        /// <summary>
        /// Tests the plural list API.
        /// </summary>
        [TestMethod]
        public void TestPluralListNoLanguage()
        {
            // Initialize.
            OrderedDictionary pluralMap = new OrderedDictionary
            {
                { "zero", "Text Zero" },
                { "other", "Text Other" },
            };

            List<MessageItem> messageItems = ICUParser.ExpandPlurals(pluralMap, null);

            // Assert.
            Assert.AreEqual(6, messageItems.Count);

            // Expect all plurals.
            Assert.AreEqual("Text Zero", messageItems[0].Text);
            Assert.AreEqual("zero", messageItems[0].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[0].MessageItemType);

            Assert.AreEqual("Text Other", messageItems[1].Text);
            Assert.AreEqual("one", messageItems[1].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.ExpandedPlural, messageItems[1].MessageItemType);

            Assert.AreEqual("Text Other", messageItems[2].Text);
            Assert.AreEqual("two", messageItems[2].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.ExpandedPlural, messageItems[2].MessageItemType);

            Assert.AreEqual("Text Other", messageItems[3].Text);
            Assert.AreEqual("few", messageItems[3].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.ExpandedPlural, messageItems[3].MessageItemType);

            Assert.AreEqual("Text Other", messageItems[4].Text);
            Assert.AreEqual("many", messageItems[4].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.ExpandedPlural, messageItems[4].MessageItemType);

            Assert.AreEqual("Text Other", messageItems[5].Text);
            Assert.AreEqual("other", messageItems[5].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[5].MessageItemType);
        }

        /// <summary>
        /// Tests the plural list API.
        /// </summary>
        [TestMethod]
        public void TestPluralListNoLanguageWithAdditionalPlurals()
        {
            // Initialize.
            OrderedDictionary pluralMap = new OrderedDictionary
            {
                { "other", "Text Other" },
                { "1", "Text 1" },
                { "=2", "Text =2" },
            };

            List<MessageItem> messageItems = ICUParser.ExpandPlurals(pluralMap, null);

            // Assert.
            Assert.AreEqual(8, messageItems.Count);

            // Expect the additional plurals
            Assert.AreEqual("Text 1", messageItems[0].Text);
            Assert.AreEqual("1", messageItems[0].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[0].MessageItemType);

            Assert.AreEqual("Text =2", messageItems[1].Text);
            Assert.AreEqual("=2", messageItems[1].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[1].MessageItemType);

            // Expect all plurals.
            Assert.AreEqual("Text Other", messageItems[2].Text);
            Assert.AreEqual("zero", messageItems[2].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.ExpandedPlural, messageItems[2].MessageItemType);

            Assert.AreEqual("Text Other", messageItems[3].Text);
            Assert.AreEqual("one", messageItems[3].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.ExpandedPlural, messageItems[3].MessageItemType);

            Assert.AreEqual("Text Other", messageItems[4].Text);
            Assert.AreEqual("two", messageItems[4].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.ExpandedPlural, messageItems[4].MessageItemType);

            Assert.AreEqual("Text Other", messageItems[5].Text);
            Assert.AreEqual("few", messageItems[5].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.ExpandedPlural, messageItems[5].MessageItemType);

            Assert.AreEqual("Text Other", messageItems[6].Text);
            Assert.AreEqual("many", messageItems[6].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.ExpandedPlural, messageItems[6].MessageItemType);

            Assert.AreEqual("Text Other", messageItems[7].Text);
            Assert.AreEqual("other", messageItems[7].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[7].MessageItemType);
        }

        /// <summary>
        /// Tests the plural list API.
        /// </summary>
        [TestMethod]
        public void TestPluralListOneOtherLanguageWithAdditionalPlurals()
        {
            // Initialize.
            OrderedDictionary pluralMap = new OrderedDictionary
            {
                { "1", "Text 1" },
                { "=2", "Text =2" },
                { "zero", "Text Zero" },
                { "one", "Text One" },
                { "two", "Text Two" },
                { "few", "Text Few" },
                { "many", "Text Many" },
                { "other", "Text Other" },
            };

            List<MessageItem> messageItems = ICUParser.ExpandPlurals(pluralMap, CultureInfo.GetCultureInfo("de-DE"));

            // Assert.
            Assert.AreEqual(4, messageItems.Count);

            // Expect the additional plurals
            Assert.AreEqual("Text 1", messageItems[0].Text);
            Assert.AreEqual("1", messageItems[0].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[0].MessageItemType);

            Assert.AreEqual("Text =2", messageItems[1].Text);
            Assert.AreEqual("=2", messageItems[1].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[1].MessageItemType);

            // Expect only one and other.
            Assert.AreEqual("Text One", messageItems[2].Text);
            Assert.AreEqual("one", messageItems[2].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[2].MessageItemType);

            Assert.AreEqual("Text Other", messageItems[3].Text);
            Assert.AreEqual("other", messageItems[3].ResourceId);
            Assert.AreEqual(MessageItemTypeEnum.Default, messageItems[3].MessageItemType);
        }

        /// <summary>
        /// Tests the plural list API.
        /// </summary>
        [TestMethod]
        public void TestPluralListWithEmptyPlurals()
        {
            // Initialize.
            OrderedDictionary pluralMap = new OrderedDictionary
            {
                { string.Empty, null },
                { "other", "Text Other" },
            };

            List<MessageItem> messageItems = ICUParser.ExpandPlurals(pluralMap, null);

            // Assert.
            Assert.AreEqual(7, messageItems.Count);

            // The ExpandPlurals API accept empty plural.
            // Expect the empty plural.
            Assert.AreEqual(string.Empty, messageItems[0].ResourceId);
            Assert.AreEqual(null, messageItems[0].Text);

            // Expect the standard plurals
            Assert.AreEqual("zero", messageItems[1].ResourceId);
            Assert.AreEqual("one", messageItems[2].ResourceId);
            Assert.AreEqual("two", messageItems[3].ResourceId);
            Assert.AreEqual("few", messageItems[4].ResourceId);
            Assert.AreEqual("many", messageItems[5].ResourceId);
            Assert.AreEqual("other", messageItems[6].ResourceId);
        }

        /// <summary>
        /// Tests the plural list API.
        /// </summary>
        [TestMethod]
        public void TestPluralListWithLeadingSpacePlurals()
        {
            // Initialize.
            OrderedDictionary pluralMap = new OrderedDictionary
            {
                { " other", "Text Other with leading space" },
                { "other", "Text Other" },
            };

            List<MessageItem> messageItems = ICUParser.ExpandPlurals(pluralMap, null);

            // Assert.
            Assert.AreEqual(7, messageItems.Count);

            // The ExpandPlurals API does not trim the strings.
            // Expect the other plural with the preserved space.
            Assert.AreEqual(" other", messageItems[0].ResourceId);
            Assert.AreEqual("Text Other with leading space", messageItems[0].Text);

            // Expect the standard plurals
            Assert.AreEqual("zero", messageItems[1].ResourceId);
            Assert.AreEqual("one", messageItems[2].ResourceId);
            Assert.AreEqual("two", messageItems[3].ResourceId);
            Assert.AreEqual("few", messageItems[4].ResourceId);
            Assert.AreEqual("many", messageItems[5].ResourceId);
            Assert.AreEqual("other", messageItems[6].ResourceId);
        }

        /// <summary>
        /// Tests the plural list API.
        /// </summary>
        [TestMethod]
        public void TestPluralListCasingWithAdditionalPlurals()
        {
            // Initialize.
            OrderedDictionary pluralMap = new OrderedDictionary
            {
                { "Other", "Text Other uppercase" },
                { "ZERO", "Text ZERO" },
                { "other", "Text Other" },
            };

            List<MessageItem> messageItems = ICUParser.ExpandPlurals(pluralMap, null);

            // Assert.
            Assert.AreEqual(8, messageItems.Count);

            // Expect the additional plurals with different casing than the standard plurals
            Assert.AreEqual("Other", messageItems[0].ResourceId);
            Assert.AreEqual("ZERO", messageItems[1].ResourceId);

            // Expect the standard plurals
            Assert.AreEqual("zero", messageItems[2].ResourceId);
            Assert.AreEqual("one", messageItems[3].ResourceId);
            Assert.AreEqual("two", messageItems[4].ResourceId);
            Assert.AreEqual("few", messageItems[5].ResourceId);
            Assert.AreEqual("many", messageItems[6].ResourceId);
            Assert.AreEqual("other", messageItems[7].ResourceId);
        }
    }
}
