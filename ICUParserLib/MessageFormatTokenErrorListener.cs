// <copyright file="MessageFormatTokenErrorListener.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace ICUParserLib
{
    using System.Collections.Generic;
    using System.IO;
    using Antlr4.Runtime;

    /// <summary>
    /// Token error listener for the message format parser.
    /// </summary>
    /// <seealso cref="Antlr4.Runtime.BaseErrorListener" />
    public class MessageFormatTokenErrorListener : IAntlrErrorListener<int>
    {
        /// <summary>
        /// Gets the token errors.
        /// </summary>
        /// <value>
        /// The token errors.
        /// </value>
        public List<string> Errors { get; } = new List<string>();

        /// <inheritdoc/>
        public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            this.Errors.Add($"Line {line}, pos {charPositionInLine}: {msg}");
        }
    }
}