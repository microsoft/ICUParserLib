// <copyright file="ComparerTest.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace ICUParserLibUnitTest
{
    using System;
    using ICUParserLib;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests <see cref="ICUParser"/>.
    /// </summary>
    public partial class ICUTest
    {
        /// <summary>
        /// Tests the overlap comparer x = null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestOverlapComparerXNull()
        {
            // Initialize.
            TextData x = null;
            TextData y = new TextData();

            // Assert.
            TextDataOverlapComparer.IsOverlap(x, y);
        }

        /// <summary>
        /// Tests the overlap comparer y = null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestOverlapComparerYNull()
        {
            // Initialize.
            TextData x = new TextData();
            TextData y = null;

            // Assert.
            TextDataOverlapComparer.IsOverlap(x, y);
        }

        /// <summary>
        /// Tests the overlap comparer with the same element.
        /// </summary>
        [TestMethod]
        public void TestOverlapComparerSameElement()
        {
            // Initialize.
            TextData x = new TextData();

            x.StartIndex = 0;
            x.StopIndex = 10;

            // Assert.
            Assert.IsTrue(TextDataOverlapComparer.IsOverlap(x, x));
        }

        /// <summary>
        /// Tests the overlap comparer.
        /// </summary>
        [TestMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1005:Single line comments should begin with single space", Justification = "Allow commenting of the overlap areas:                   |---------|")]
        public void TestOverlapComparer()
        {
            TextData x = new TextData();
            TextData y = new TextData();

            // |---------|
            //            |---------|
            x.StartIndex = 0;
            x.StopIndex = 10;
            y.StartIndex = 21;
            y.StopIndex = 31;
            Assert.IsFalse(TextDataOverlapComparer.IsOverlap(x, y));

            //            |---------|
            // |---------|
            x.StartIndex = 21;
            x.StopIndex = 31;
            y.StartIndex = 0;
            y.StopIndex = 10;
            Assert.IsFalse(TextDataOverlapComparer.IsOverlap(x, y));

            // |---------|
            //           |---------|
            x.StartIndex = 0;
            x.StopIndex = 10;
            y.StartIndex = 10;
            y.StopIndex = 20;
            Assert.IsTrue(TextDataOverlapComparer.IsOverlap(x, y));

            //           |---------|
            // |---------|
            x.StartIndex = 10;
            x.StopIndex = 20;
            y.StartIndex = 0;
            y.StopIndex = 10;
            Assert.IsTrue(TextDataOverlapComparer.IsOverlap(x, y));

            // |---------|
            //          |---------|
            x.StartIndex = 0;
            x.StopIndex = 10;
            y.StartIndex = 9;
            y.StopIndex = 19;
            Assert.IsTrue(TextDataOverlapComparer.IsOverlap(x, y));

            //          |---------|
            // |---------|
            x.StartIndex = 9;
            x.StopIndex = 19;
            y.StartIndex = 0;
            y.StopIndex = 10;
            Assert.IsTrue(TextDataOverlapComparer.IsOverlap(x, y));

            // |-----------------------------|
            //           |---------|
            x.StartIndex = 0;
            x.StopIndex = 30;
            y.StartIndex = 10;
            y.StopIndex = 20;
            Assert.IsTrue(TextDataOverlapComparer.IsOverlap(x, y));

            //           |---------|
            // |-----------------------------|
            x.StartIndex = 10;
            x.StopIndex = 20;
            y.StartIndex = 0;
            y.StopIndex = 30;
            Assert.IsTrue(TextDataOverlapComparer.IsOverlap(x, y));
        }

        /// <summary>
        /// Tests the overlap comparer with the same element.
        /// </summary>
        [TestMethod]
        public void TestMessageItemContentComparer()
        {
            // Initialize.
            MessageItem dataX = new MessageItem { Text = "textX" };
            MessageItem dataXdup = new MessageItem { Text = "textX" };
            MessageItem dataY = new MessageItem { Text = "textY" };

            MessageItemContentComparer messageItemContentComparer = new MessageItemContentComparer();

            // Assert.
            Assert.IsTrue(messageItemContentComparer.Equals(dataX, dataX));
            Assert.IsTrue(messageItemContentComparer.Equals(dataX, dataXdup));
            Assert.IsFalse(messageItemContentComparer.Equals(dataX, dataY));
        }
    }
}
