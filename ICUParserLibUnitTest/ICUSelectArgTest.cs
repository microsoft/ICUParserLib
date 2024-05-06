// <copyright file="ICUSelectArgTest.cs" company="Microsoft Corporation">
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
        public void TestSelectMessage1()
        {
            string input = @"{1, select, female {allée} other {allé # }}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsTrue(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();

            // Assert.
            Assert.AreEqual(2, messageItems.Count);
            Assert.AreEqual("allée", messageItems[0].Text);
            Assert.AreEqual("Select.female", messageItems[0].ResourceId);

            Assert.AreEqual("allé # ", messageItems[1].Text);
            Assert.AreEqual(1, messageItems[1].LockedSubstrings.Count);
            Assert.AreEqual("Select.other", messageItems[1].ResourceId);

            // Modify the strings, check if the composed string is different, revert back and test again.
            this.PostTestStringCheck(icuParser, messageItems);
        }

        /// <summary>
        /// Tests the select message.
        /// </summary>
        [TestMethod]
        public void TestSelectMessageStrictParse()
        {
            string input = @"{1, select, female {allée} other {allé}} à Paris.";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsFalse(icuParser.Success);
            Assert.IsFalse(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("ja"));

            // Assert.
            Assert.AreEqual(1, messageItems.Count);
            Assert.AreEqual(input, output, "Different text output.");
        }

        /// <summary>
        /// Tests the select message.
        /// </summary>
        [TestMethod]
        public void TestSelectMessageDuplicates()
        {
            string input = @"{gender, select, female{{userName} started screensharing.} male{{userName} started screensharing.} other{{userName} started screensharing.}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsTrue(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("ar-SA"));

            // Assert.
            Assert.AreEqual(3, messageItems.Count);
            Assert.AreEqual(input, output, "Different text output.");
        }

        /// <summary>
        /// Tests the select message.
        /// </summary>
        [TestMethod]
        public void TestSelectMessageWithDuplicateMessageStrings()
        {
            // Initialize.
            string input = @"{color1, select, 
                red {{color2, select, 
                    red {Red and Red make Red} 
                    yellow {Red and Yellow make Orange} 
                    blue {Red and Blue make Purple} 
                    other {Choose primary colors.}}}
                yellow {{color2, select, 
                    red {Yellow and Red make Orange} 
                    yellow {Yellow and Yellow make Yellow} 
                    blue {Yellow and Blue make Green} 
                    other {Choose primary colors.}}}
                blue {{color2, select, 
                    red {Blue and Red make Purple} 
                    yellow {Blue and Yellow make Green} 
                    blue {Blue and Blue make Blue} 
                    other {Choose primary colors.}}}
                other {Choose primary colors.}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsTrue(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(13, messageItems.Count);
            Assert.AreEqual("Red and Red make Red", messageItems[0].Text);
            Assert.AreEqual("Red and Yellow make Orange", messageItems[1].Text);
            Assert.AreEqual("Red and Blue make Purple", messageItems[2].Text);
            Assert.AreEqual("Choose primary colors.", messageItems[3].Text);
            Assert.AreEqual("Yellow and Red make Orange", messageItems[4].Text);
            Assert.AreEqual("Yellow and Yellow make Yellow", messageItems[5].Text);
            Assert.AreEqual("Yellow and Blue make Green", messageItems[6].Text);
            Assert.AreEqual("Choose primary colors.", messageItems[7].Text);
            Assert.AreEqual("Blue and Red make Purple", messageItems[8].Text);
            Assert.AreEqual("Blue and Yellow make Green", messageItems[9].Text);
            Assert.AreEqual("Blue and Blue make Blue", messageItems[10].Text);
            Assert.AreEqual("Choose primary colors.", messageItems[11].Text);
            Assert.AreEqual("Choose primary colors.", messageItems[12].Text);

            // Modify the strings, check if the composed string is different, revert back and test again.
            this.PostTestStringCheck(icuParser, messageItems);
        }

        /// <summary>
        /// Tests the select message.
        /// </summary>
        [TestMethod]
        public void TestSelectMessageRemoveDuplicateMessageStrings()
        {
            // Initialize.
            string input = @"{color1, select, 
                red {{color2, select, 
                    red {Red and Red make Red} 
                    yellow {Red and Yellow make Orange} 
                    blue {Red and Blue make Purple} 
                    other {Choose primary colors.}}}
                yellow {{color2, select, 
                    red {Yellow and Red make Orange} 
                    yellow {Yellow and Yellow make Yellow} 
                    blue {Yellow and Blue make Green} 
                    other {Choose primary colors.}}}
                blue {{color2, select, 
                    red {Blue and Red make Purple} 
                    yellow {Blue and Yellow make Green} 
                    blue {Blue and Blue make Blue} 
                    other {Choose primary colors.}}}
                other {Choose primary colors.}}";

            // removeDuplicateMessageStrings = true;
            ICUParser icuParser = new ICUParser(input, true);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(10, messageItems.Count);
            Assert.AreEqual("Red and Red make Red", messageItems[0].Text);
            Assert.AreEqual("Red and Yellow make Orange", messageItems[1].Text);
            Assert.AreEqual("Red and Blue make Purple", messageItems[2].Text);
            Assert.AreEqual("Choose primary colors.", messageItems[3].Text);
            Assert.AreEqual("Yellow and Red make Orange", messageItems[4].Text);
            Assert.AreEqual("Yellow and Yellow make Yellow", messageItems[5].Text);
            Assert.AreEqual("Yellow and Blue make Green", messageItems[6].Text);
            Assert.AreEqual("Blue and Red make Purple", messageItems[7].Text);
            Assert.AreEqual("Blue and Yellow make Green", messageItems[8].Text);
            Assert.AreEqual("Blue and Blue make Blue", messageItems[9].Text);

            // Modify the strings, check if the composed string is different, revert back and test again.
            this.PostTestStringCheck(icuParser, messageItems);
        }

        /// <summary>
        /// Tests an invalid select message.
        /// </summary>
        [TestMethod]
        public void TestInvalidSelectMessage()
        {
            string input = @"{1, tceles, female {allée} other {allé}} à Paris.";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsFalse(icuParser.Success);
        }

        /// <summary>
        /// Tests an invalid select message.
        /// </summary>
        [TestMethod]
        public void TestInvalidParameterSelectMessage()
        {
            string input = @"{1, select, none}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsFalse(icuParser.Success);
        }

        /// <summary>
        /// Tests the gender message.
        /// </summary>
        [TestMethod]
        public void TestGenderMessage()
        {
            // Initialize.
            string input = @"{gender, select, male {He} female {She} other {They} }";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(3, messageItems.Count);
            Assert.AreEqual("He", messageItems[0].Text);
            Assert.AreEqual("She", messageItems[1].Text);
            Assert.AreEqual("They", messageItems[2].Text);

            // Modify the strings, check if the composed string is different, revert back and test again.
            this.PostTestStringCheck(icuParser, messageItems);
        }

        /// <summary>
        /// Tests the gender message.
        /// </summary>
        [TestMethod]
        public void TestGenderMessageStrictParse()
        {
            // Initialize.
            string input = @"{gender, select, male {He} female {She} other {They} } will respond shortly.";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsFalse(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(1, messageItems.Count);
        }

        /// <summary>
        /// Tests the select message.
        /// </summary>
        [TestMethod]
        public void TestSelectMessageUsingNumber()
        {
            // Initialize.
            string input = @"{ taxable_Area, select, yes { An additional { taxRate, number, percent} tax will be collected.} other { No taxes apply.} }";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(2, messageItems.Count);
            Assert.AreEqual(" An additional { taxRate, number, percent} tax will be collected.", messageItems[0].Text);
            Assert.AreEqual("{ taxRate, number, percent}", messageItems[0].LockedSubstrings[0]);
            Assert.AreEqual(" No taxes apply.", messageItems[1].Text);

            // Modify the strings, check if the composed string is different, revert back and test again.
            this.PostTestStringCheck(icuParser, messageItems);
        }

        /// <summary>
        /// Tests the select message.
        /// </summary>
        [TestMethod]
        public void TestSelectMessageDuplicate()
        {
            // Initialize.
            string input = @"{isFaster, select, 
                    yes 
                    {Your average startup time is <b>
                        {minutes, plural, 
                            =0 {} 
                            one {# minute, } 
                            other {# minutes, }
                        }
                        {seconds, plural, 
                            one {# second} 
                            other {# seconds}
                        }
                        </b>, which is faster than the average of Windows PCs. Startup times may vary based on how many apps you have installed and the speed of your storage and processor.
                    } 
                    no 
                    {Your average startup time is <b>
                        {minutes, plural, 
                            =0 {} 
                            one {# minute, } 
                            other {# minutes, }
                        }
                        {seconds, plural, 
                            one {# second} 
                            other {# seconds}
                        }
                        </b>, which is slower than the average of Windows PCs. Startup times may vary based on how many apps you have installed and the speed of your storage and processor.
                    }
                }";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsFalse(icuParser.Success);

            Assert.AreEqual(4, icuParser.Errors.Count);
            Assert.AreEqual("Strict parse mode enabled. Content contains leading/trailing text 'Your average startup time is <b>\r\n                        '.", icuParser.Errors[0]);
        }
    }
}
