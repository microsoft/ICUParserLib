// <copyright file="MessageItem.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace ICUParserLib
{
    using System.Collections.Generic;

    /// <summary>
    /// Stores the message item.
    /// </summary>
    public class MessageItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageItem"/> class.
        /// </summary>
        public MessageItem()
        {
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the plural.
        /// </summary>
        /// <value>
        /// The plural.
        /// </value>
        public string Plural { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public string Data { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the message item.
        /// </summary>
        /// <value>
        /// The type of the message item.
        /// </value>
        public MessageItemTypeEnum MessageItemType { get; set; } = MessageItemTypeEnum.Default;

        /// <summary>
        /// Gets or sets the resource identifier.
        /// </summary>
        /// <value>
        /// The resource identifier.
        /// </value>
        public string ResourceId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the locked substrings.
        /// </summary>
        /// <value>
        /// The locked substrings.
        /// </value>
        public List<string> LockedSubstrings { get; set; } = new List<string>();
    }
}
