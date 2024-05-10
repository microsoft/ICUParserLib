// <copyright file="ICUParserUsageTest.cs" company="Microsoft Corporation">
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
        /// Tests the calling structure when used in a resource parser.
        /// </summary>
        [TestMethod]
        public void TestSubmitICUMessageFormatString()
        {
            // Initialize with invalid input.
            string input = @"{0, TESTplural,
                =1 { Relaunch Microsoft Edge within a day}
                other { Relaunch Microsoft Edge within # days}}";

            // Setup the ICU message format parser with the duplicate resources option set.
            ICUParser icuParser = new ICUParser(input, true);

            // Assert.
            Assert.IsFalse(icuParser.Success);

            // Parser checks success.
            if (!icuParser.Success)
            {
                string errorMsgs = string.Join(",", icuParser.Errors);
                Assert.AreEqual("Error in line 1, pos 4: no viable alternative at input '{0,TESTplural'", errorMsgs);
            }

            // Run again with valid input.
            input = @"{0, plural,
                =1 { Relaunch Microsoft Edge within a day}
                other { Relaunch Microsoft Edge within # days}}";

            icuParser = new ICUParser(input, true);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            bool isGenerating = true;
            string resourceId = "resourceId";

            // Process the message text using the ICU message format parser.
            List<MessageItem> messageItems = icuParser.GetMessageItems();

            // Assert
            Assert.AreEqual(7, messageItems.Count);

            // Unroll the message loop for Asserts.
            // foreach (MessageItem messageItem in messageItems)
            {
                MessageItem messageItem = messageItems[0];
                string msg = messageItem.Text;

                // Setup the locver instructions for the locked substrings.
                string locverInstructions = string.Empty;
                foreach (string lockedSubstring in messageItem.LockedSubstrings)
                {
                    locverInstructions += $" (ICU){{PlaceHolder=\"{lockedSubstring}\"}}";
                }

                // Update the resource Id.
                string msgId = resourceId;
                if (!string.IsNullOrEmpty(messageItem.ResourceId))
                {
                    msgId += $"#{messageItem.ResourceId}";
                }

                // Submit the resource and get back the string.
                string lSItemText = msg;

                // Update the resource with the loc content.
                if (isGenerating)
                {
                    messageItem.Text = lSItemText;
                }

                // Assert
                Assert.AreEqual(string.Empty, locverInstructions);
                Assert.AreEqual("resourceId#Plural.=1", msgId);
            }

            {
                MessageItem messageItem = messageItems[1];
                string msg = messageItem.Text;

                // Setup the locver instructions for the locked substrings.
                string locverInstructions = string.Empty;
                foreach (string lockedSubstring in messageItem.LockedSubstrings)
                {
                    locverInstructions += $" (ICU){{PlaceHolder=\"{lockedSubstring}\"}}";
                }

                // Update the resource Id.
                string msgId = resourceId;
                if (!string.IsNullOrEmpty(messageItem.ResourceId))
                {
                    msgId += $"#{messageItem.ResourceId}";
                }

                // Submit the resource and get back the string.
                string lSItemText = msg;

                // Update the resource with the loc content.
                if (isGenerating)
                {
                    messageItem.Text = lSItemText;
                }

                // Assert
                Assert.AreEqual(" (ICU){PlaceHolder=\"#\"}", locverInstructions);
                Assert.AreEqual("resourceId#Plural.other", msgId);
            }

            // Assert
            Assert.AreEqual(" Relaunch Microsoft Edge within a day", messageItems[0].Text);
            Assert.AreEqual("Plural.=1", messageItems[0].ResourceId);
            Assert.AreEqual(0, messageItems[0].LockedSubstrings.Count);
            Assert.AreEqual(" Relaunch Microsoft Edge within # days", messageItems[1].Text);
            Assert.AreEqual("Plural.other", messageItems[1].ResourceId);
            Assert.AreEqual("#", messageItems[1].LockedSubstrings[0]);

            if (isGenerating)
            {
                string messageText = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("my"));

                // The generated message must match the input.
                Assert.AreEqual(input, messageText);
            }
        }
    }
}
