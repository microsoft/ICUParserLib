// <copyright file="LanguagePluralRangeData.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace ICUParserLib
{
    using System;

    /// <summary>
    /// LanguagePluralRangeData.
    /// </summary>
    public class LanguagePluralRangeData : IEquatable<LanguagePluralRangeData>
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        public string Lang { get; set; } = string.Empty;

        // Set default values to match "en":

        /// <summary>
        /// Gets or sets a value indicating whether the plural range is 'zero'.
        /// </summary>
        public bool Zero { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the plural range is 'one'.
        /// </summary>
        public bool One { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the plural range is 'two'.
        /// </summary>
        public bool Two { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the plural range is 'few'.
        /// </summary>
        public bool Few { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the plural range is 'many'.
        /// </summary>
        public bool Many { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the plural range is 'other'.
        /// </summary>
        public bool Other { get; set; } = true;

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified selector]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string selector)
        {
            switch (selector.ToLowerInvariant())
            {
                case "zero": return this.Zero;
                case "one": return this.One;
                case "two": return this.Two;
                case "few": return this.Few;
                case "many": return this.Many;
                case "other": return this.Other;
                default: return false;
            }
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <param name="returnValueIfNotFound">if set to <c>true</c> [return value if not found].</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified selector]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(PluralSelectorEnum selector, bool returnValueIfNotFound = true)
        {
            switch (selector)
            {
                case PluralSelectorEnum.Zero: return this.Zero;
                case PluralSelectorEnum.One: return this.One;
                case PluralSelectorEnum.Two: return this.Two;
                case PluralSelectorEnum.Few: return this.Few;
                case PluralSelectorEnum.Many: return this.Many;
                case PluralSelectorEnum.Other: return this.Other;
                default: return returnValueIfNotFound;
            }
        }

        /// <summary>
        /// Compare two instances of LanguagePluralRangeData.
        /// </summary>
        /// <param name="other">Other instance.</param>
        /// <returns>True if the same.</returns>
        public bool Equals(LanguagePluralRangeData other)
        {
            return this.Name == other.Name
                && this.Lang == other.Lang
                && this.Zero == other.Zero
                && this.One == other.One
                && this.Two == other.Two
                && this.Few == other.Few
                && this.Many == other.Many
                && this.Other == other.Other;
        }
    }
}
