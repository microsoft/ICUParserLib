// <copyright file="PluralSelector.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace ICUParserLib
{
    /// <summary>
    /// PluralSelectorEnum.
    /// </summary>
    public enum PluralSelectorEnum
    {
        /// <summary>
        /// Not set.
        /// </summary>
        Null,

        /// <summary>
        /// The zero.
        /// </summary>
        Zero,

        /// <summary>
        /// The one.
        /// </summary>
        One,

        /// <summary>
        /// The two.
        /// </summary>
        Two,

        /// <summary>
        /// The few.
        /// </summary>
        Few,

        /// <summary>
        /// The many.
        /// </summary>
        Many,

        /// <summary>
        /// The other.
        /// </summary>
        Other,
    }

    /// <summary>
    /// Stores the used plural data.
    /// </summary>
    public class PluralSelector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluralSelector"/> class.
        /// </summary>
        public PluralSelector()
        {
        }

        /// <summary>
        /// Gets or sets the plural selector type.
        /// </summary>
        public PluralSelectorEnum PluralSelectorType { get; set; } = PluralSelectorEnum.Null;

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

        /// <summary>
        /// Gets the type of the plural selector.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <returns>Plural selector enum.</returns>
        public static PluralSelectorEnum GetPluralSelectorType(string selector)
        {
            switch (selector.ToLowerInvariant())
            {
                case "zero": return PluralSelectorEnum.Zero;
                case "one": return PluralSelectorEnum.One;
                case "two": return PluralSelectorEnum.Two;
                case "few": return PluralSelectorEnum.Few;
                case "many": return PluralSelectorEnum.Many;
                case "other": return PluralSelectorEnum.Other;
            }

            return PluralSelectorEnum.Null;
        }
    }
}
