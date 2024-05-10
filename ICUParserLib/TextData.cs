// <copyright file="TextData.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace ICUParserLib
{
    /// <summary>
    /// Stores the text data.
    /// </summary>
    public class TextData : MessageItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextData"/> class.
        /// </summary>
        public TextData()
        {
        }

        /// <summary>
        /// Gets or sets the plural data identifier.
        /// </summary>
        /// <value>
        /// The plural data identifier.
        /// </value>
        public string PluralDataId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start index.
        /// </summary>
        /// <value>
        /// The start index.
        /// </value>
        public int StartIndex { get; set; } = -1;

        /// <summary>
        /// Gets or sets the index of the stop.
        /// </summary>
        /// <value>
        /// The index of the stop.
        /// </value>
        public int StopIndex { get; set; } = -1;
    }
}
