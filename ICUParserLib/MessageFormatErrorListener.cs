// <copyright file="MessageFormatErrorListener.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace ICUParserLib
{
    using System.Collections.Generic;
    using System.IO;
    using Antlr4.Runtime;

    /// <summary>
    /// Error listener for the message format parser.
    /// </summary>
    /// <seealso cref="Antlr4.Runtime.BaseErrorListener" />
    public class MessageFormatErrorListener : BaseErrorListener
    {
        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public List<string> Errors { get; } = new List<string>();

        /// <inheritdoc/>
        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            this.Errors.Add($"Error in line {line}, pos {charPositionInLine}: {msg}");

            base.SyntaxError(output, recognizer, offendingSymbol, line, charPositionInLine, msg, e);
        }
    }
}