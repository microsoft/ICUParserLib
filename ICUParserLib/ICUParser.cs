// <copyright file="ICUParser.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

/*
// Free Java runtime.
https://www.azul.com/downloads/zulu-community/?&version=java-8-lts&os=&os=windows&architecture=x86-64-bit&package=jdk
*/

namespace ICUParserLib
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Antlr4.Runtime;

    /// <summary>
    /// Implements the ICU message format parser.
    /// </summary>
    public class ICUParser
    {
        /// <summary>
        /// The message text error listener.
        /// </summary>
        private readonly MessageFormatErrorListener messageTextErrorListener = new MessageFormatErrorListener();

        /// <summary>
        /// The message format text token error listener.
        /// </summary>
        private readonly MessageFormatTokenErrorListener messageFormatTokenErrorListener = new MessageFormatTokenErrorListener();

        /// <summary>
        /// The message format visitor.
        /// </summary>
        private readonly MessageFormatVisitor messageFormatVisitor = new MessageFormatVisitor();

        /// <summary>
        /// The remove duplicate message strings.
        /// </summary>
        private readonly bool removeDuplicateMessageStrings = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ICUParser"/> class.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="removeDuplicateMessageStrings">if set to <c>true</c> [remove duplicate message strings].</param>
        public ICUParser(string input, bool removeDuplicateMessageStrings = false)
        {
            this.Input = input;
            this.removeDuplicateMessageStrings = removeDuplicateMessageStrings;

            try
            {
                // Setup Antlr.
                AntlrInputStream str = new AntlrInputStream(input);
                MessageFormatLexer lexer = new MessageFormatLexer(str);
                lexer.AddErrorListener(this.messageFormatTokenErrorListener);
                lexer.RemoveErrorListener(ConsoleErrorListener<int>.Instance);

                CommonTokenStream tokens = new CommonTokenStream(lexer);

                // Add error handler.
                MessageFormat parser = new MessageFormat(tokens);
                parser.AddErrorListener(this.messageTextErrorListener);
                parser.RemoveErrorListener(ConsoleErrorListener<IToken>.Instance);

                // Run the lexer and parser.
                MessageFormat.MessageContext messageContext = parser.message();
                this.messageFormatVisitor.Visit(messageContext);
            }
            catch (Exception e)
            {
                this.messageTextErrorListener.Errors.Add($"ANTLR Exception: {e.Message}");
            }

            // Check if content is strict (no leading/trailing text or concatenated ICU strings).
            if (this.Success && this.messageFormatVisitor.IsNoStrictType)
            {
                foreach (string text in this.messageFormatVisitor.LeadingTrailingTexts)
                {
                    this.messageTextErrorListener.Errors.Add($"Strict parse mode enabled. Content contains leading/trailing text '{text}'.");
                }
            }

            // Any ICU plural message needs to have an 'other' plural range.
            if (this.Success && this.messageFormatVisitor.PluralDataList.Count > 0)
            {
                foreach (string id in this.messageFormatVisitor.UsedPluralSelectors.Keys)
                {
                    PluralSelector otherPluralSelector = this.messageFormatVisitor.UsedPluralSelectors[id].Find(pluralSelector => pluralSelector.PluralSelectorType == PluralSelectorEnum.Other);
                    if (otherPluralSelector == null)
                    {
                        this.messageTextErrorListener.Errors.Add($"Missing 'other' plural selector in '{input}'.");
                    }
                }
            }

            if (this.Success)
            {
                // Resolve duplicate resource Ids.
                this.UpdateResourceIds(this.messageFormatVisitor.TextDataSet);

                // Provide the expanded plurals.
                foreach (string id in this.messageFormatVisitor.PluralDataList.Keys)
                {
                    this.messageFormatVisitor.PluralDataList[id].ExpandPlurals(this.messageFormatVisitor.TextDataSet, id);
                }

                // Resolve duplicate expanded plurals.
                this.UpdateExpandedPlurals(this.messageFormatVisitor.PluralDataList);
            }
            else
            {
                // If parsing fails, reset to the input.
                this.messageFormatVisitor.IsICU = false;
                this.PluralList.Clear();
                this.messageFormatVisitor.TextDataSet.Clear();
                this.messageFormatVisitor.TextDataSet.Add(new TextData()
                {
                    Text = input,
                    MessageItemType = MessageItemTypeEnum.Default,
                    StartIndex = 0,
                    StopIndex = input.Length,
                });
            }
        }

        /// <summary>
        /// Gets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        public string Input { get; } = string.Empty;

        /// <summary>
        /// Gets a value indicating whether this instance is icu.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is icu; otherwise, <c>false</c>.
        /// </value>
        public bool IsICU
        {
            get
            {
                return this.messageFormatVisitor.IsICU;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICUParser"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success
        {
            get
            {
                return this.messageFormatTokenErrorListener.Errors.Count == 0 && this.messageTextErrorListener.Errors.Count == 0;
            }
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public List<string> Errors
        {
            get
            {
                return this.messageFormatTokenErrorListener.Errors.Concat(this.messageTextErrorListener.Errors).ToList();
            }
        }

        /// <summary>
        /// Gets the plurals.
        /// </summary>
        /// <value>
        /// The plurals.
        /// </value>
        public Dictionary<string, PluralData> PluralList
        {
            get
            {
                return this.messageFormatVisitor.PluralDataList;
            }
        }

        /// <summary>
        /// Determines whether [is language supported] [the specified culture information].
        /// If the language is not supported, the ICUParserLib will fall back to the en-US plural range
        /// when ComposeMessageText() is run and the language Locked instructions are wrong.
        /// </summary>
        /// <param name="lcid">The culture lcid.</param>
        /// <returns>
        ///   <c>true</c> if [is language supported] [the specified culture information]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLanguageSupported(int lcid)
        {
            try
            {
                CultureInfo cultureInfo = CultureInfo.GetCultureInfo(lcid);
                return IsLanguageSupported(cultureInfo);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether [is language supported] [the specified culture information].
        /// If the language is not supported, the ICUParserLib will fall back to the en-US plural range
        /// when ComposeMessageText() is run and the language Locked instructions are wrong.
        /// </summary>
        /// <param name="cultureInfo">The culture information.</param>
        /// <returns>
        ///   <c>true</c> if [is language supported] [the specified culture information]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLanguageSupported(CultureInfo cultureInfo)
        {
            while (cultureInfo != null && !string.IsNullOrEmpty(cultureInfo.Name))
            {
                if (LanguagePluralRanges.LanguageList.Contains(cultureInfo.Name, StringComparer.InvariantCultureIgnoreCase))
                {
                    return true;
                }

                cultureInfo = cultureInfo.Parent;
            }

            return false;
        }

        /// <summary>
        /// Expands the plurals.
        /// </summary>
        /// <param name="pluralMap">The plural map to expand.</param>
        /// <param name="cultureInfo">cultureInfo.</param>
        /// <returns>List of expanded plurals in message Items.</returns>
        public static List<MessageItem> ExpandPlurals(OrderedDictionary pluralMap, CultureInfo cultureInfo)
        {
            // Any ICU plural message needs to have an 'other' plural range.
            if (!pluralMap.Contains("other"))
            {
                throw new ArgumentException("The plurals needs to have a 'other' plural range.");
            }

            LanguagePluralRangeData languagePluralRangeData = cultureInfo != null ? LanguagePluralRanges.GetLanguagePluralRangeData(cultureInfo) : null;

            List<MessageItem> messageItems = new List<MessageItem>();

            // Process the non-standard plurals first.
            foreach (string plural in pluralMap.Keys)
            {
                if (!PluralData.PluralMatchList.Contains(plural))
                {
                    messageItems.Add(new MessageItem()
                    {
                        Text = pluralMap[plural]?.ToString(),
                        ResourceId = plural,
                        Plural = plural,
                        Data = string.Empty,
                        MessageItemType = MessageItemTypeEnum.Default,
                    });
                }
            }

            // Enumerate the standard plurals and complete the list.
            foreach (string plural in PluralData.PluralMatchList)
            {
                // Standard plurals used?
                if (languagePluralRangeData == null || languagePluralRangeData.Contains(plural))
                {
                    // Use the plural 'other' if the plural is not used.
                    bool pluralUsed = pluralMap.Contains(plural);
                    string expandedplural = pluralUsed ? plural : "other";

                    messageItems.Add(new MessageItem()
                    {
                        Text = pluralMap[expandedplural].ToString(),
                        ResourceId = plural,
                        Plural = plural,
                        Data = LanguagePluralRanges.PluralLanguageList[plural],
                        MessageItemType = pluralUsed ? MessageItemTypeEnum.Default : MessageItemTypeEnum.ExpandedPlural,
                    });
                }
            }

            return messageItems;
        }

        /// <summary>
        /// Gets the message items.
        /// </summary>
        /// <returns>List of message items.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Invalid token. Startindex={textData.StartIndex}, StopIndex={textData.StopIndex}.
        /// or
        /// Overlapping data.
        /// </exception>
        public List<MessageItem> GetMessageItems()
        {
            List<TextData> textDataSet = this.messageFormatVisitor.TextDataSet;

            // Validate the data.
            for (int textDataSetIndex = 0; textDataSetIndex < textDataSet.Count; textDataSetIndex++)
            {
                TextData textData = textDataSet[textDataSetIndex];

                if (textData.StopIndex < textData.StartIndex)
                {
                    throw new ArgumentOutOfRangeException($"Invalid token. Startindex={textData.StartIndex}, StopIndex={textData.StopIndex}.");
                }

                // Check for overlapping data.
                for (int textDataSetIndexNext = textDataSetIndex + 1; textDataSetIndexNext < textDataSet.Count; textDataSetIndexNext++)
                {
                    TextData textDataNext = textDataSet[textDataSetIndexNext];
                    if (TextDataOverlapComparer.IsOverlap(textData, textDataNext))
                    {
                        throw new ArgumentOutOfRangeException("Overlapping data.");
                    }
                }
            }

            List<MessageItem> messageItems;

            // Create the message items.
            IEnumerable<MessageItem> messageItemsFromTextData = textDataSet.Select(textData => new MessageItem
            {
                Text = textData.Text,
                ResourceId = textData.ResourceId,
                Plural = textData.Plural,
                MessageItemType = textData.MessageItemType,
                LockedSubstrings = textData.LockedSubstrings,
            });

            if (this.removeDuplicateMessageStrings)
            {
                messageItems = messageItemsFromTextData.Distinct(new MessageItemContentComparer()).ToList();
            }
            else
            {
                messageItems = messageItemsFromTextData.ToList();
            }

            // Add the plural data for the non-expanded plurals.
            foreach (MessageItem messageItem in messageItems)
            {
                if (LanguagePluralRanges.PluralLanguageList.ContainsKey(messageItem.Plural))
                {
                    messageItem.Data = LanguagePluralRanges.PluralLanguageList[messageItem.Plural];
                }
            }

            // Add the plural message items.
            foreach (string pluralsId in this.PluralList.Keys)
            {
                PluralData pluralData = this.PluralList[pluralsId];
                foreach (string selector in pluralData.PluralsToAdd.Keys)
                {
                    messageItems.Add(pluralData.PluralsToAdd[selector]);
                }
            }

            return messageItems;
        }

        /// <summary>
        /// Composes the message text.
        /// </summary>
        /// <param name="messageItems">The message items.</param>
        /// <param name="lcid">The lcid.</param>
        /// <returns>Message text.</returns>
        public string ComposeMessageText(List<MessageItem> messageItems, int lcid)
        {
            // Throw exception for invalid lcid.
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo(lcid);
            List<string> messageStrings = messageItems.Where(msgItem => msgItem.MessageItemType != MessageItemTypeEnum.ExpandedPlural).Select(msgItem => msgItem.Text).ToList();

            return this.ComposeMessageText(messageStrings, cultureInfo);
        }

        /// <summary>
        /// Composes the message text.
        /// </summary>
        /// <param name="messageItems">The message items.</param>
        /// <param name="cultureInfo">The culture info.</param>
        /// <returns>Message text.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Mismatched number of message strings and parsed ICU message strings.</exception>
        public string ComposeMessageText(List<MessageItem> messageItems, CultureInfo cultureInfo = null)
        {
            List<TextData> textDataSet = this.messageFormatVisitor.TextDataSet;

            List<string> messageStrings = messageItems.Where(msgItem => msgItem.MessageItemType != MessageItemTypeEnum.ExpandedPlural).Select(msgItem => msgItem.Text).ToList();
            return this.ComposeMessageText(messageStrings, cultureInfo);
        }

        /// <summary>
        /// Composes the message text.
        /// </summary>
        /// <param name="messageStrings">The message strings.</param>
        /// <param name="cultureInfo">The culture info.</param>
        /// <returns>Composed message text.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Mismatched number of message strings and parsed ICU message strings.</exception>
        public string ComposeMessageText(List<string> messageStrings, CultureInfo cultureInfo = null)
        {
            List<TextData> textDataSet = this.messageFormatVisitor.TextDataSet;

            if (this.removeDuplicateMessageStrings)
            {
                IEnumerable<string> originaldataStrings = textDataSet.Select(textData => textData.Text);
                List<string> originaldataStringsDuplicatesRemoved = originaldataStrings.Distinct(StringComparer.Ordinal).ToList();
                List<string> originaldataStringsList = originaldataStrings.ToList();

                // Update the data set with the new data.
                for (int dataIndex = 0; dataIndex < textDataSet.Count; dataIndex++)
                {
                    // Get the index of the string in the original list.
                    string originaldataString = originaldataStringsList[dataIndex];
                    int index = originaldataStringsDuplicatesRemoved.FindIndex(dataString => dataString == originaldataString);

                    // Use this index to resolve the duplicate.
                    textDataSet[dataIndex].Text = messageStrings[index];
                }
            }
            else
            {
                if (messageStrings.Count != textDataSet.Count)
                {
                    throw new ArgumentOutOfRangeException("Mismatched number of message strings and parsed ICU message strings.");
                }

                // Update the data set with the new data.
                for (int dataIndex = 0; dataIndex < textDataSet.Count; dataIndex++)
                {
                    textDataSet[dataIndex].Text = messageStrings[dataIndex];
                }
            }

            LanguagePluralRangeData languagePluralRangeData = LanguagePluralRanges.GetLanguagePluralRangeData(cultureInfo);

            // Compose the output message text string.
            StringBuilder textStr = new StringBuilder();
            for (int inputIndex = 0; inputIndex < this.Input.Length; inputIndex++)
            {
                // Expanded plurals.
                foreach (string id in this.messageFormatVisitor.PluralDataList.Keys)
                {
                    PluralData pluralData = this.messageFormatVisitor.PluralDataList[id];

                    PluralSelector pluralSelector = this.messageFormatVisitor.UsedPluralSelectors[id].Find(selector => selector.PluralSelectorType == PluralSelectorEnum.Other);

                    // pluralData.StopIndex is the end of the plural argument.
                    // pluralSelector.StartIndex is the beginning of the 'other' plural range.
                    if (pluralSelector.StartIndex == inputIndex)
                    {
                        // Get the whitespaces for formatting.
                        string content = textStr.ToString();
                        int i = content.Length - 1;
                        while (i > 0 && char.IsWhiteSpace(content[i]))
                        {
                            i--;
                        }

                        string ws = content.Substring(i + 1);

                        foreach (string selector in pluralData.PluralsToAdd.Keys)
                        {
                            // Does language allow this selector?
                            bool isLanguagePluralValid = languagePluralRangeData.Contains(selector);
                            if (isLanguagePluralValid)
                            {
                                MessageItem message = pluralData.PluralsToAdd[selector];
                                textStr.Append($"{selector} {{{message.Text}}}{ws}");
                            }
                        }
                    }
                }

                bool indexUpdated = false;

                // Intern plural selectors.
                foreach (string id in this.messageFormatVisitor.UsedPluralSelectors.Keys)
                {
                    foreach (PluralSelector pluralSelector in this.messageFormatVisitor.UsedPluralSelectors[id])
                    {
                        if (pluralSelector.StartIndex == inputIndex)
                        {
                            // Does language allow this selector?
                            bool isLanguagePluralValid = languagePluralRangeData.Contains(pluralSelector.PluralSelectorType, true);

                            // Remove this selector if language does not allow it.
                            if (!isLanguagePluralValid)
                            {
                                inputIndex += pluralSelector.StopIndex - pluralSelector.StartIndex;
                                indexUpdated = true;
                                break;
                            }
                        }
                    }
                }

                // Run plurals first if index is updated.
                if (indexUpdated)
                {
                    continue;
                }

                // Translated text data.
                TextData textData = textDataSet.Find(element => element.StartIndex == inputIndex);
                if (textData != null)
                {
                    textStr.Append(textData.Text);
                    inputIndex += textData.StopIndex - textData.StartIndex;
                }
                else
                {
                    textStr.Append(this.Input[inputIndex]);
                }
            }

            return textStr.ToString();
        }

        /// <summary>
        /// Updates the resource ids.
        /// </summary>
        /// <param name="textDataSet">The text data set.</param>
        /// <exception cref="ArgumentException">Duplicate resource Ids: {msg}.</exception>
        internal void UpdateResourceIds(List<TextData> textDataSet)
        {
            // Get all resources grouped by resource Id.
            var resourceGroups = textDataSet.GroupBy(textData => textData.ResourceId);

            // Get all resources with more than one resource.
            foreach (var resourceGroup in resourceGroups.Where(group => group.Count() > 1))
            {
                int seqId = 0;
                foreach (TextData textData in resourceGroup)
                {
                    textData.ResourceId += $"#{seqId}";
                    seqId++;
                }
            }
        }

        /// <summary>
        /// Resolve duplicate expanded plurals.
        /// </summary>
        /// <param name="pluralDataList">The plural data list.</param>
        internal void UpdateExpandedPlurals(Dictionary<string, PluralData> pluralDataList)
        {
            // Get all resource Ids.
            List<MessageItem> messageItems = pluralDataList.SelectMany(pluralData => pluralData.Value.PluralsToAdd.Select(pluralsToAdd => pluralsToAdd.Value)).ToList();

            // Get all duplicates and resolve.
            var resourceGroups = messageItems.GroupBy(item => item.ResourceId);
            foreach (var resourceGroup in resourceGroups.Where(group => group.Count() > 1))
            {
                int seqId = 0;
                foreach (MessageItem messageItem in resourceGroup)
                {
                    messageItem.ResourceId += $"#{seqId}";
                    seqId++;
                }
            }
        }
    }
}
