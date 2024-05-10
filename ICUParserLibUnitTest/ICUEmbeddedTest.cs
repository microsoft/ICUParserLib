// <copyright file="ICUEmbeddedTest.cs" company="Microsoft Corporation">
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
        /// Tests the select message.
        /// </summary>
        [TestMethod]
        public void TestEmbeddedMessageTextOnly()
        {
            // Embedded messages are text only.
            string input = @"{1, select, 
                    female {allée} 
                    other {allé # }
                    }";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsTrue(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(2, messageItems.Count);

            // Modify the strings, check if the composed string is different, revert back and test again.
            this.PostTestStringCheck(icuParser, messageItems);
        }

        /// <summary>
        /// Tests the select message.
        /// </summary>
        [TestMethod]
        public void TestEmbeddedMessageStrictPlural()
        {
            // Embedded messages are strict plural type.
            string input = @"{1, select, 
                    female {allée} 
                    other {{age, plural, 
                            =0 {less than one year old} 
                            one {over # year old} 
                            other {over # years old}
                        }}
                    }";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            // Content is parsed in strict mode and content is strict with no leading/trailing text.
            Assert.IsTrue(icuParser.Success);
            Assert.IsTrue(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(8, messageItems.Count);

            // Modify the strings, check if the composed string is different, revert back and test again.
            this.PostTestStringCheck(icuParser, messageItems);
        }

        /// <summary>
        /// Tests the select message.
        /// </summary>
        [TestMethod]
        public void TestEmbeddedMessageNonStrictPlural()
        {
            // Embedded messages are non strict plural type.
            string input = @"{1, select, 
                    female {allée} 
                    other {Your device is {age, plural, 
                            =0 {less than one year old} 
                            one {over # year old} 
                            other {over # years old}
                        }.
                        }
                    }";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            // Content is parsed in strict mode, but embedded message is not strict (contains leading/trailing text).
            Assert.IsFalse(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            Assert.AreEqual(2, icuParser.Errors.Count);
            Assert.AreEqual("Strict parse mode enabled. Content contains leading/trailing text 'Your device is '.", icuParser.Errors[0]);
            Assert.AreEqual("Strict parse mode enabled. Content contains leading/trailing text '.\r\n                        '.", icuParser.Errors[1]);
        }

        /// <summary>
        /// Tests the select message.
        /// </summary>
        [TestMethod]
        public void TestEmbeddedMessageNonStrictPluralWithStrictParse()
        {
            // Embedded messages are non strict plural type.
            string input = @"{1, select, 
                    female {allée} 
                    other {Your device is {age, plural, 
                            =0 {less than one year old} 
                            one {over # year old} 
                            other {over # years old}
                        }.
                        }
                    }";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsFalse(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);
        }

        /// <summary>
        /// Tests the select message.
        /// </summary>
        [TestMethod]
        public void TestSelectPluralMessage()
        {
            // Initialize.
            string input = @"{ageAvailable, select, 
                yes {Your device is 
                        {age, plural, 
                            =0 {less than one year old} 
                            one {over # year old} 
                            other {over # years old}
                        }. 
                    All batteries become less effective as they age. This typically means shorter battery life. 
                    The battery in your PC can only charge up to <b>{batteryCapacityPercentage, number, ::percent} 
                    of its design capacity</b>. Changing power options can help conserve battery life and 
                    improve performance.
                    } 
                no {All batteries become less effective as they age. This typically means shorter battery life. 
                    The battery in your PC can only charge up to <b>{batteryCapacityPercentage, number, ::percent} 
                    of its design capacity</b>. Changing power options can help conserve battery life and 
                    improve performance.
                    }
                }";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsFalse(icuParser.Success);

            // Assert.
            Assert.AreEqual(2, icuParser.Errors.Count);
            Assert.AreEqual("Strict parse mode enabled. Content contains leading/trailing text 'Your device is \r\n                        '.", icuParser.Errors[0]);
            Assert.AreEqual("Strict parse mode enabled. Content contains leading/trailing text '. \r\n                    All batteries become less effective as they age. This typically means shorter battery life. \r\n                    The battery in your PC can only charge up to <b>{batteryCapacityPercentage,number,::percent} \r\n                    of its design capacity</b>. Changing power options can help conserve battery life and \r\n                    improve performance.\r\n                    '.", icuParser.Errors[1]);
        }
    }
}
