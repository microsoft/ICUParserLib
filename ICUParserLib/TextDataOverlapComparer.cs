// <copyright file="TextDataOverlapComparer.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace ICUParserLib
{
    using System;

    /// <summary>
    /// Comparer to check if data overlaps.
    /// </summary>
    public static class TextDataOverlapComparer
    {
        /// <summary>
        /// Determines whether the specified x is overlap.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        ///   <c>true</c> if the specified x is overlap; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// x
        /// or
        /// y.
        /// </exception>
        public static bool IsOverlap(TextData x, TextData y)
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            if (object.ReferenceEquals(x, y))
            {
                return true;
            }

            bool overlap = (x.StartIndex >= y.StartIndex && x.StartIndex <= y.StopIndex) ||
                (x.StopIndex >= y.StartIndex && x.StopIndex <= y.StopIndex) ||
                (y.StartIndex >= x.StartIndex && y.StartIndex <= x.StopIndex);

            return overlap;
        }
    }
}
