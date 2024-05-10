// <copyright file="Utilities.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace ICUParserLibUnitTest
{
    using System.Linq;
    using ICUParserLib;

    /// <summary>
    /// Tests <see cref="ICUParser"/>.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Reverse the string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Test-Translated string.</returns>
        internal static string Reverse(string input)
        {
            return new string(Enumerable.Range(1, input.Length).Select(i => input[input.Length - i]).ToArray());
        }

        /// <summary>
        /// Enclose the string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="str">The string to prepend and append to the input string.</param>
        /// <returns>Test-Translated string.</returns>
        internal static string Enclose(string input, string str)
        {
            return $"{str}{input}{str}";
        }
    }
}