// <copyright file="ICUBaseTest.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace ICUParserLibUnitTest
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using ICUParserLib;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests <see cref="ICUParser"/>.
    /// </summary>
    public partial class ICUTest
    {
        /// <summary>
        /// The test context instance.
        /// </summary>
        private TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get => this.testContextInstance;
            set => this.testContextInstance = value;
        }

        /// <summary>
        /// Test initialize function.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
        }

        /// <summary>
        /// Test clean up function.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
        }

        /// <summary>
        /// Tests the with message = null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void TestNullMessage()
        {
            string input = null;

            // Throws NullReferenceException exception.
            _ = new ICUParser(input);
        }

        /// <summary>
        /// Tests the empty message.
        /// </summary>
        [TestMethod]
        public void TestEmptyMessage()
        {
            string input = string.Empty;

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(0, messageItems.Count);
        }

        /// <summary>
        /// Tests the whitespace message.
        /// </summary>
        [TestMethod]
        public void TestWhitespaceMessage()
        {
            string input = "          ";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(0, messageItems.Count);
        }

        /// <summary>
        /// Tests the message with whitespaces.
        /// </summary>
        [TestMethod]
        public void TestMessageWithWhitespace()
        {
            string input = "    test    ";

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
        /// Tests the text message without ICU formatting.
        /// </summary>
        [TestMethod]
        public void TestTextMessage()
        {
            string input = "The cat.";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(1, messageItems.Count);
            Assert.AreEqual(input, messageItems[0].Text);
        }

        /// <summary>
        /// Tests with empty ICU message format.
        /// </summary>
        [TestMethod]
        public void TestEmptyFormatMessage()
        {
            string input = "{}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsFalse(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);
            Assert.AreEqual(1, icuParser.Errors.Count);
            Assert.AreEqual("Error in line 1, pos 1: no viable alternative at input '{}'", icuParser.Errors[0]);
        }

        /// <summary>
        /// Tests the minimal none arg ICU message format.
        /// </summary>
        [TestMethod]
        public void TestMinimalMessage()
        {
            string input = "{a}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            // Assert.
            Assert.AreEqual(1, messageItems.Count);
            Assert.AreEqual(input, messageItems[0].Text);

            // Modify the strings, check if the composed string is different, revert back and test again.
            this.PostTestStringCheck(icuParser, messageItems);
        }

        /// <summary>
        /// Tests the EDGE placeholder message.
        /// </summary>
        [TestMethod]
        public void TestPlaceholderMessage()
        {
            string input = @"<ph name=""TAB_TITLE""><ex>The Title of the Tab</ex>$1</ph> <ph name=""EMOJI_PLAYING""><ex>(playing)</ex>$2</ph>";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            // Assert.
            Assert.AreEqual(1, messageItems.Count);
            Assert.AreEqual(input, messageItems[0].Text);

            // Modify the strings, check if the composed string is different, revert back and test again.
            this.PostTestStringCheck(icuParser, messageItems);
        }

        /// <summary>
        /// Tests the message with a number sign message.
        /// </summary>
        [TestMethod]
        public void TestNumberSignPlaceholderMessage1()
        {
            string input = @"#";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            // Assert.
            Assert.AreEqual(1, messageItems.Count);
            Assert.AreEqual(input, messageItems[0].Text);
            Assert.AreEqual(0, messageItems[0].LockedSubstrings.Count);

            // Modify the strings, check if the composed string is different, revert back and test again.
            this.PostTestStringCheck(icuParser, messageItems);
        }

        /// <summary>
        /// Tests the message with a number sign message.
        /// </summary>
        [TestMethod]
        public void TestNumberSignPlaceholderMessage2()
        {
            string input = @"<ph name=""ENROLLMENT_DOMAIN""><ex>example.com</ex>$1</ph> Not used in code: task#23463408";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            // Assert.
            Assert.AreEqual(1, messageItems.Count);
            Assert.AreEqual(input, messageItems[0].Text);
            Assert.AreEqual(0, messageItems[0].LockedSubstrings.Count);

            // Modify the strings, check if the composed string is different, revert back and test again.
            this.PostTestStringCheck(icuParser, messageItems);
        }

        /// <summary>
        /// Tests the message with a number sign message.
        /// </summary>
        [TestMethod]
        public void TestNumberSignPlaceholderMessage3()
        {
            string input = @"PKCS #1 SHA-512 with RSA Encryption";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            // Assert.
            Assert.AreEqual(1, messageItems.Count);
            Assert.AreEqual(input, messageItems[0].Text);
            Assert.AreEqual(0, messageItems[0].LockedSubstrings.Count);

            // Modify the strings, check if the composed string is different, revert back and test again.
            this.PostTestStringCheck(icuParser, messageItems);
        }

        /// <summary>
        /// Tests an invalid message.
        /// </summary>
        [TestMethod]
        public void TestInvalidMessage()
        {
            // Initialize.
            string input = @"Extra curly brace: <ph name=""ERROR_LINE""><ex>} (without having a corresponding opening curly brace &apos;{&apos;)</ex>$1</ph>";

            // This input throws an exception.
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
        /// Tests the ComposeMessageText with mismatched message strings.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void TestComposeMessageText()
        {
            string input = "Test";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(1, messageItems.Count);

            List<MessageItem> mismatchedmessageItems = new List<MessageItem>
                {
                    new MessageItem
                    {
                        Text = "test1",
                        ResourceId = "#1",
                        LockedSubstrings = new List<string>(),
                    },

                    new MessageItem
                    {
                        Text = "test2",
                        ResourceId = "#2",
                        LockedSubstrings = new List<string>(),
                    },
                };

            // Throws ArgumentOutOfRangeException.
            _ = icuParser.ComposeMessageText(mismatchedmessageItems);
        }

        /// <summary>
        /// Tests the message resource identifier.
        /// </summary>
        [TestMethod]
        public void TestMessageEmptyResourceId()
        {
            string input = "text {test}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            // Assert.
            Assert.AreEqual(1, messageItems.Count);
            Assert.AreEqual(string.Empty, messageItems[0].ResourceId);
        }

        /// <summary>
        /// Tests the update resource ids with empty list.
        /// </summary>
        [TestMethod]
        public void TestUpdateResourceIdsEmptyList()
        {
            List<TextData> textDataSet = new List<TextData>
            {
                new TextData
                {
                },
            };

            ICUParser icuParser = new ICUParser(string.Empty);

            // Expected to not throw exception.
            icuParser.UpdateResourceIds(textDataSet);
        }

        /// <summary>
        /// Tests the update resource ids.
        /// </summary>
        [TestMethod]
        public void TestUpdateResourceIds()
        {
            ICUParser icuParser = new ICUParser(string.Empty);

            // Initialize.
            List<TextData> textDataSetNoDup1 = new List<TextData>
                {
                    new TextData() { ResourceId = "id1", },
                };

            icuParser.UpdateResourceIds(textDataSetNoDup1);

            // Assert.
            Assert.AreEqual("id1", textDataSetNoDup1[0].ResourceId);

            List<TextData> textDataSetNoDup2 = new List<TextData>
                {
                    new TextData() { ResourceId = "id1", },
                    new TextData() { ResourceId = "id2", },
                };

            icuParser.UpdateResourceIds(textDataSetNoDup2);

            // Assert.
            Assert.AreEqual("id1", textDataSetNoDup2[0].ResourceId);
            Assert.AreEqual("id2", textDataSetNoDup2[1].ResourceId);

            // Initialize.
            List<TextData> textDataSetDup1 = new List<TextData>
                {
                    new TextData() { ResourceId = "id1", },
                    new TextData() { ResourceId = "id1", },
                    new TextData() { ResourceId = "id2", },
                };

            icuParser.UpdateResourceIds(textDataSetDup1);

            // Assert.
            Assert.AreEqual("id1#0", textDataSetDup1[0].ResourceId);
            Assert.AreEqual("id1#1", textDataSetDup1[1].ResourceId);
            Assert.AreEqual("id2", textDataSetDup1[2].ResourceId);

            // Initialize.
            List<TextData> textDataSetDup2 = new List<TextData>
                {
                    new TextData() { ResourceId = "id1", },
                    new TextData() { ResourceId = "id2", },
                    new TextData() { ResourceId = "id2", },
                    new TextData() { ResourceId = "id3", },
                    new TextData() { ResourceId = "id3", },
                    new TextData() { ResourceId = "id3", },
                    new TextData() { ResourceId = "id4", },
                };

            icuParser.UpdateResourceIds(textDataSetDup2);

            // Assert.
            Assert.AreEqual("id1", textDataSetDup2[0].ResourceId);
            Assert.AreEqual("id2#0", textDataSetDup2[1].ResourceId);
            Assert.AreEqual("id2#1", textDataSetDup2[2].ResourceId);
            Assert.AreEqual("id3#0", textDataSetDup2[3].ResourceId);
            Assert.AreEqual("id3#1", textDataSetDup2[4].ResourceId);
            Assert.AreEqual("id3#2", textDataSetDup2[5].ResourceId);
            Assert.AreEqual("id4", textDataSetDup2[6].ResourceId);
        }

        /// <summary>
        /// Tests the update expanded plurals with empty list.
        /// </summary>
        [TestMethod]
        public void TestUpdateExpandedPluralsEmptyList()
        {
            Dictionary<string, PluralData> pluralDataList = new Dictionary<string, PluralData>
            {
            };

            ICUParser icuParser = new ICUParser(string.Empty);

            // Expected to not throw exception.
            icuParser.UpdateExpandedPlurals(pluralDataList);
        }

        /// <summary>
        /// Tests the update expanded plurals.
        /// </summary>
        [TestMethod]
        public void TestUpdateExpandedPlurals()
        {
            string duplicateId = "resId0";

            Dictionary<string, PluralData> pluralDataList = new Dictionary<string, PluralData>
            {
                {
                    "id0", new PluralData()
                    {
                        PluralsToAdd = new Dictionary<string, MessageItem>()
                        {
                            {
                                "msg#0", new MessageItem()
                                {
                                    ResourceId = duplicateId,
                                }
                            },
                        },
                    }
                },
                {
                    "id1", new PluralData()
                    {
                        PluralsToAdd = new Dictionary<string, MessageItem>()
                        {
                            {
                                "msg#0", new MessageItem()
                                {
                                    ResourceId = duplicateId,
                                }
                            },
                        },
                    }
                },
                {
                    "id2", new PluralData()
                    {
                        PluralsToAdd = new Dictionary<string, MessageItem>()
                        {
                            {
                                "msg#0", new MessageItem()
                                {
                                    ResourceId = "0123456789",
                                }
                            },
                        },
                    }
                },
                {
                    "id3", new PluralData()
                    {
                        PluralsToAdd = new Dictionary<string, MessageItem>()
                        {
                            {
                                "msg#0", new MessageItem()
                                {
                                    ResourceId = duplicateId,
                                }
                            },
                        },
                    }
                },
            };

            ICUParser icuParser = new ICUParser(string.Empty);

            // Resolve duplicate Ids.
            icuParser.UpdateExpandedPlurals(pluralDataList);

            // Assert.
            Assert.AreEqual($"{duplicateId}#0", pluralDataList["id0"].PluralsToAdd["msg#0"].ResourceId);
            Assert.AreEqual($"{duplicateId}#1", pluralDataList["id1"].PluralsToAdd["msg#0"].ResourceId);
            Assert.AreEqual($"{duplicateId}#2", pluralDataList["id3"].PluralsToAdd["msg#0"].ResourceId);
        }

        /// <summary>
        /// Executes the ICU parser with a large content multiple times.
        /// Make sure the execution time is within reasonable limits.
        /// </summary>
        [TestMethod]
        public void TestICUParserPerf()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Run a simulation for 1000 resources.
            foreach (int i in Enumerable.Range(1, 1000))
            {
                // Initialize.
                // Each text resource is 650 Bytes.
                int textSize = 100;
                StringBuilder text = new StringBuilder();

                // string input = @"... {notifications, plural,
                //      zero {...}
                //       one {...}
                //     other {...}
                //    }... {name} ...";
                text.Append("{notifications, plural, zero {");
                text.Append(this.RandomString(textSize));
                text.Append("} one {");
                text.Append(this.RandomString(textSize));
                text.Append("} other {");
                text.Append(this.RandomString(textSize));
                text.Append("}}");

                string input = text.ToString();

                ICUParser icuParser = new ICUParser(input);

                // Assert.
                Assert.IsTrue(icuParser.Success);

                List<MessageItem> messageItems = icuParser.GetMessageItems();
                string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("lv"));

                // Assert.
                Assert.AreEqual(input, output, "Different text output.");
                Assert.AreEqual(6, messageItems.Count);
            }

            sw.Stop();

            // Execution time is about 400ms in debug on a standard machine.
            Assert.IsTrue(sw.ElapsedMilliseconds < 1000);
        }
    }
}
