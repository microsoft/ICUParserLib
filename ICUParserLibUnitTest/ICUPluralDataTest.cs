// <copyright file="ICUPluralDataTest.cs" company="Microsoft Corporation">
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
        /// Tests the plural data.
        /// </summary>
        [TestMethod]
        public void TestPluralDataEmptySelector()
        {
            // Initialize.
            PluralData pluralData = new PluralData();

            // Execute.
            List<TextData> textDataList = new List<TextData>
            {
                new TextData
                {
                    ResourceId = "Plural.other",
                },
            };
            pluralData.ExpandPlurals(textDataList);

            // Assert.
            Assert.AreEqual(6, pluralData.PluralsToAdd.Count);
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("zero"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("one"));
        }

        /// <summary>
        /// Tests the plural data.
        /// </summary>
        [TestMethod]
        public void TestPluralDataWithoutSelectorOtherLowerCase()
        {
            // Initialize.
            PluralData pluralData = new PluralData();
            pluralData.PluralCategories.Add("one");

            // Execute.
            List<TextData> textDataList = new List<TextData>
            {
                new TextData
                {
                    ResourceId = "Plural.other",
                },
            };
            pluralData.ExpandPlurals(textDataList);

            // Assert.
            Assert.AreEqual(5, pluralData.PluralsToAdd.Count);
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("zero"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("two"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("few"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("many"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("other"));
        }

        /// <summary>
        /// Tests the plural data.
        /// </summary>
        [TestMethod]
        public void TestPluralDataWithoutSelectorOtherUpperCase()
        {
            // Initialize.
            PluralData pluralData = new PluralData();
            pluralData.PluralCategories.Add("ZERO");

            // Execute.
            List<TextData> textDataList = new List<TextData>
            {
                new TextData
                {
                    ResourceId = "Plural.other",
                },
            };
            pluralData.ExpandPlurals(textDataList);

            // Assert.
            Assert.AreEqual(5, pluralData.PluralsToAdd.Count);
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("one"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("two"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("few"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("many"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("other"));
        }

        /// <summary>
        /// Tests the plural data.
        /// </summary>
        [TestMethod]
        public void TestPluralDataWithSelectorOther()
        {
            // Initialize.
            PluralData pluralData = new PluralData();
            pluralData.PluralCategories.Add("Zero");
            pluralData.PluralCategories.Add("other");

            // Execute.
            List<TextData> textDataList = new List<TextData>
            {
                new TextData
                {
                    ResourceId = "Plural.other",
                },
            };
            pluralData.ExpandPlurals(textDataList);

            // Assert.
            Assert.AreEqual(4, pluralData.PluralsToAdd.Count);
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("one"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("two"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("few"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("many"));
        }

        /// <summary>
        /// Tests the plural data.
        /// </summary>
        [TestMethod]
        public void TestPluralDataWithAllSelectorsWithoutOther()
        {
            // Initialize.
            PluralData pluralData = new PluralData();
            pluralData.PluralCategories.Add("zero");
            pluralData.PluralCategories.Add("one");

            // Execute.
            List<TextData> textDataList = new List<TextData>
            {
                new TextData
                {
                    ResourceId = "Plural.other",
                },
            };
            pluralData.ExpandPlurals(textDataList);

            // Assert.
            Assert.AreEqual(4, pluralData.PluralsToAdd.Count);
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("two"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("few"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("many"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("other"));
        }

        /// <summary>
        /// Tests the plural data.
        /// </summary>
        [TestMethod]
        public void TestPluralDataWithAllSelectors()
        {
            // Initialize.
            PluralData pluralData = new PluralData();
            pluralData.PluralCategories.Add("zero");
            pluralData.PluralCategories.Add("one");
            pluralData.PluralCategories.Add("two");
            pluralData.PluralCategories.Add("few");
            pluralData.PluralCategories.Add("many");
            pluralData.PluralCategories.Add("other");

            // Execute.
            List<TextData> textDataList = new List<TextData>
            {
                new TextData
                {
                    ResourceId = "Plural.other",
                },
            };
            pluralData.ExpandPlurals(textDataList);

            // Assert.
            Assert.AreEqual(0, pluralData.PluralsToAdd.Count);
        }

        /// <summary>
        /// Tests the plural data.
        /// </summary>
        [TestMethod]
        public void TestPluralDataWithAllSelectorsNumWithoutOther()
        {
            // Initialize.
            PluralData pluralData = new PluralData();
            pluralData.PluralCategories.Add("=0");
            pluralData.PluralCategories.Add("one");

            // Execute.
            List<TextData> textDataList = new List<TextData>
            {
                new TextData
                {
                    ResourceId = "Plural.other",
                },
            };
            pluralData.ExpandPlurals(textDataList);

            // Assert.
            Assert.AreEqual(5, pluralData.PluralsToAdd.Count);
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("zero"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("two"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("few"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("many"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("other"));
        }

        /// <summary>
        /// Tests the plural data.
        /// </summary>
        [TestMethod]
        public void TestPluralDataWithAllSelectorsNumDuplicate()
        {
            // Initialize.
            PluralData pluralData = new PluralData();
            pluralData.PluralCategories.Add("=0");
            pluralData.PluralCategories.Add("zero");

            // Execute.
            List<TextData> textDataList = new List<TextData>
            {
                new TextData
                {
                    ResourceId = "Plural.other",
                },
            };
            pluralData.ExpandPlurals(textDataList);

            // Assert.
            Assert.AreEqual(5, pluralData.PluralsToAdd.Count);
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("one"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("two"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("few"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("many"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("other"));
        }

        /// <summary>
        /// Tests the plural data.
        /// </summary>
        [TestMethod]
        public void TestPluralDataWithAllSelectorsNum()
        {
            // Initialize.
            PluralData pluralData = new PluralData();
            pluralData.PluralCategories.Add("=0");
            pluralData.PluralCategories.Add("=1");
            pluralData.PluralCategories.Add("=2");
            pluralData.PluralCategories.Add("few");
            pluralData.PluralCategories.Add("many");
            pluralData.PluralCategories.Add("other");

            // Execute.
            List<TextData> textDataList = new List<TextData>
            {
                new TextData
                {
                    ResourceId = "Plural.other",
                },
            };
            pluralData.ExpandPlurals(textDataList);

            // Assert.
            Assert.AreEqual(3, pluralData.PluralsToAdd.Count);
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("zero"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("one"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("two"));
        }

        /// <summary>
        /// Tests the plural data.
        /// </summary>
        [TestMethod]
        public void TestPluralDataWithLargeNumSelector()
        {
            // Initialize.
            PluralData pluralData = new PluralData();
            pluralData.PluralCategories.Add("=42");

            // Execute.
            List<TextData> textDataList = new List<TextData>
            {
                new TextData
                {
                    ResourceId = "Plural.other",
                },
            };
            pluralData.ExpandPlurals(textDataList);

            // Assert.
            Assert.AreEqual(6, pluralData.PluralsToAdd.Count);
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("zero"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("one"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("two"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("few"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("many"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("other"));
        }

        /// <summary>
        /// Tests the plural data.
        /// </summary>
        [TestMethod]
        public void TestPluralDataWithLargeNumSelectorWithOther()
        {
            // Initialize.
            PluralData pluralData = new PluralData();
            pluralData.PluralCategories.Add("=42");
            pluralData.PluralCategories.Add("other");

            // Execute.
            List<TextData> textDataList = new List<TextData>
            {
                new TextData
                {
                    ResourceId = "Plural.other",
                },
            };
            pluralData.ExpandPlurals(textDataList);

            // Assert.
            Assert.AreEqual(5, pluralData.PluralsToAdd.Count);
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("zero"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("one"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("two"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("few"));
            Assert.IsTrue(pluralData.PluralsToAdd.ContainsKey("many"));
        }
    }
}
