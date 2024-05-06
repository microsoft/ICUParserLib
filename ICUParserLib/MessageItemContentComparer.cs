// <copyright file="MessageItemContentComparer.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace ICUParserLib
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Compares the content of two MessageItem items.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEqualityComparer{T}" />
    public class MessageItemContentComparer : IEqualityComparer<MessageItem>
    {
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        ///   <see langword="true" /> if the specified objects are equal; otherwise, <see langword="false" />.
        /// </returns>
        public bool Equals(MessageItem x, MessageItem y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }

            return x.Text.Equals(y.Text, StringComparison.Ordinal);
        }

        /// <summary>
        /// Gets the hash code for an instance.
        /// </summary>
        /// <param name="obj">
        /// The object instance.
        /// </param>
        /// <returns>
        /// The hash code.
        /// </returns>
        public int GetHashCode(MessageItem obj)
        {
            return obj.Text.GetHashCode();
        }
    }
}
