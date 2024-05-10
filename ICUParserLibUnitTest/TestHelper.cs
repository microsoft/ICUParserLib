// <copyright file="TestHelper.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace ICUParserLibUnitTest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using ICUParserLib;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test helper for <see cref="ICUParser"/>.
    /// </summary>
    public partial class ICUTest
    {
        private static Random random = new Random();
        private static Dictionary<string, string> locCulturesXml = new Dictionary<string, string>();

        /// <summary>
        /// Modify the strings, check if the composed string is different, revert back and test again.
        /// </summary>
        /// <param name="icuParser">The icu parser.</param>
        /// <param name="messageItems">The message items.</param>
        private void PostTestStringCheck(ICUParser icuParser, List<MessageItem> messageItems)
        {
            // Check for duplicate resource Ids.
            var duplicates = messageItems.GroupBy(s => s.ResourceId).SelectMany(grp => grp.Skip(1));
            if (duplicates.Any())
            {
                string duplicateResourceIds = string.Join(",", messageItems.Select(dataString => $"'{dataString.Text}'='{dataString.ResourceId}'"));
                throw new ArgumentException($"Duplicate resource Ids: {duplicateResourceIds}");
            }

            // Modify the strings by prepending and appending a string.
            // The modification string content must be different from the content of any test string.
            string modifyString = "¦";
            foreach (MessageItem messageItem in messageItems)
            {
                messageItem.Text = Utilities.Enclose(messageItem.Text, modifyString);
            }

            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            // The modified string must be different than the original input string.
            Assert.AreNotEqual(icuParser.Input, output, "Same text output.");

            // Remove the added modification string.
            output = output.Replace(modifyString, string.Empty);

            // Assert.
            // Now the string must be the same as the original input string.
            Assert.AreEqual(icuParser.Input, output, "Different text output.");
        }

        /// <summary>
        /// Create a random string.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns>Random string.</returns>
        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Loads the loccultures.xml.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA3075:Insecure DTD processing in XML", Justification = "Processing of internal project files for test only.")]
        private void LoadLocCulturesXml()
        {
            if (locCulturesXml.Count == 0)
            {
                // Load LocCultures.xml directly.
                string locCulturesXmlFile = Path.Combine(this.TestContext.DeploymentDirectory, "LocCultures.xml");

                XmlDocument doc = new XmlDocument();
                doc.Load(locCulturesXmlFile);

                XmlNodeList locCultureNodes = doc.SelectNodes("//LocCulture");

                foreach (XmlElement locCultureNode in locCultureNodes)
                {
                    locCulturesXml.Add(locCultureNode.Attributes["RFC3066Name"].Value, locCultureNode.Attributes["EnglishName"].Value);
                }
            }
        }
    }
}
