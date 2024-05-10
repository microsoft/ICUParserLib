// <copyright file="PluralData.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace ICUParserLib
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Stores the optional plural data.
    /// </summary>
    public class PluralData
    {
        /// <summary>
        /// The plural match list.
        /// http://cldr.unicode.org/index/cldr-spec/plural-rules
        /// zero,one,two,few,many,other.
        /// </summary>
        internal static readonly List<string> PluralMatchList = new List<string>()
            {
                "zero",
                "one",
                "two",
                "few",
                "many",
                "other",
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="PluralData"/> class.
        /// </summary>
        public PluralData()
        {
        }

        /// <summary>
        /// Gets or sets the plural categories.
        /// </summary>
        /// <value>
        /// The plural categories.
        /// </value>
        public List<string> PluralCategories { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the plurals to add.
        /// </summary>
        /// <value>
        /// The plurals to add.
        /// </value>
        public Dictionary<string, MessageItem> PluralsToAdd { get; set; } = new Dictionary<string, MessageItem>();

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the index of the stop.
        /// </summary>
        /// <value>
        /// The index of the stop.
        /// </value>
        public int StopIndex { get; set; } = -1;

        /// <summary>
        /// Expands the plurals.
        /// </summary>
        /// <param name="textDataSet">The text data set.</param>
        /// <param name="pluralId">The plural identifier.</param>
        public void ExpandPlurals(List<TextData> textDataSet, string pluralId = "")
        {
            if (PluralMatchList.Count > 0)
            {
                TextData pluralMessage = textDataSet.Find(textData => textData.PluralDataId == pluralId && textData.ResourceId.StartsWith("Plural.other"));

                if (pluralMessage != null)
                {
                    // Enumerate the plurals.
                    foreach (string plural in PluralMatchList)
                    {
                        // Add to the plural list if plural is not used.
                        string matchingPlural = this.PluralCategories.Find(pluralCategory => plural.Equals(pluralCategory, StringComparison.OrdinalIgnoreCase));
                        if (matchingPlural == null)
                        {
                            this.PluralsToAdd.Add(plural, new MessageItem()
                            {
                                Text = pluralMessage.Text,
                                LockedSubstrings = pluralMessage.LockedSubstrings,
                                Data = LanguagePluralRanges.PluralLanguageList[plural],
                                Plural = plural,
                                MessageItemType = MessageItemTypeEnum.ExpandedPlural,
                                ResourceId = $"ExpandedPlural.{this.Id}.{plural}",
                            });
                        }
                    }
                }
            }
        }
    }
}
