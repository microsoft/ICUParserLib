// <copyright file="MessageFormatVisitor.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace ICUParserLib
{
    using System;
    using System.Collections.Generic;
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;

    /// <summary>
    /// Visitor the message format parser.
    /// </summary>
    /// <seealso cref="MessageFormatBaseVisitor{T}" />
    [System.CLSCompliant(false)]
    public class MessageFormatVisitor : MessageFormatBaseVisitor<object>
    {
        /// <summary>
        /// Context names of the MessageFormat grammar.
        /// </summary>
        private readonly string selector = "Selector";
        private readonly string plural = "Plural";
        private readonly string styleContext = "StyleContext";
        private readonly string pluralStyleContext = "PluralStyleContext";
        private readonly string pluralStylesContext = "PluralStylesContext";
        private readonly string selectStyleContext = "SelectStyleContext";
        private readonly string selectOrdinalStyleContext = "SelectOrdinalStyleContext";

        /// <summary>
        /// The locked substrings.
        /// </summary>
        private readonly List<string> lockedSubstrings = new List<string>();

        /// <summary>
        /// The plural identifier.
        /// </summary>
        private int pluralId = 0;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is icu.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is icu; otherwise, <c>false</c>.
        /// </value>
        public bool IsICU { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether this is not a strict type.
        /// </summary>
        /// <value>
        ///   <c>true</c> if not a strict type; otherwise, <c>false</c>.
        /// </value>
        public bool IsNoStrictType { get; set; } = false;

        /// <summary>
        /// Gets or sets the used plural selectors.
        /// </summary>
        public Dictionary<string, List<PluralSelector>> UsedPluralSelectors { get; set; } = new Dictionary<string, List<PluralSelector>>();

        /// <summary>
        /// Gets or sets the optional plural data.
        /// </summary>
        public Dictionary<string, PluralData> PluralDataList { get; set; } = new Dictionary<string, PluralData>();

        /// <summary>
        /// Gets the text data set.
        /// </summary>
        /// <value>
        /// The text data set.
        /// </value>
        public List<TextData> TextDataSet { get; } = new List<TextData>();

        /// <summary>
        /// Gets the leading trailing texts.
        /// </summary>
        /// <value>
        /// The leading trailing texts.
        /// </value>
        public List<string> LeadingTrailingTexts { get; } = new List<string>();

        /// <inheritdoc/>
        public override object VisitComplexArg([NotNull] MessageFormat.ComplexArgContext context)
        {
            this.IsICU = true;

            return this.VisitChildren(context);
        }

        /// <inheritdoc/>
        public override object VisitMessage([NotNull] MessageFormat.MessageContext context)
        {
            if (context.children != null && context.children.Count > 1)
            {
                foreach (var child in context.children)
                {
                    if (child != null && child.GetType().Name == "MessageTextContext")
                    {
                        string text = child.GetText();
                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            this.LeadingTrailingTexts.Add(text);
                            this.IsNoStrictType = true;
                        }
                    }
                }
            }

            return base.VisitMessage(context);
        }

        /// <inheritdoc/>
        public override object VisitMessageText([NotNull] MessageFormat.MessageTextContext context)
        {
            string name = context.GetText();

            this.lockedSubstrings.Clear();
            var ret = this.VisitChildren(context);
            (string, string) resourceId = this.GetResourceId(context);

            string id = string.Empty;

            if (resourceId.Item1 == this.plural)
            {
                id = this.GetPluralListId(context);

                // The ICU structure is not valid if the parent of the plurals is missing.
                if (!this.PluralDataList.ContainsKey(id))
                {
                    throw new ArgumentException("PluralDataList id '{id}' could not be found.");
                }

                if (!this.PluralDataList[id].PluralCategories.Contains(resourceId.Item2))
                {
                    this.PluralDataList[id].PluralCategories.Add(resourceId.Item2);
                }
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                this.TextDataSet.Add(new TextData
                {
                    Text = context.Start.InputStream.GetText(new Interval(context.Start.StartIndex, context.Stop.StopIndex)),
                    ResourceId = string.IsNullOrEmpty(resourceId.Item1) && string.IsNullOrEmpty(resourceId.Item2) ? string.Empty : $"{resourceId.Item1}.{resourceId.Item2}",
                    Plural = resourceId.Item2,
                    PluralDataId = id,
                    MessageItemType = MessageItemTypeEnum.Default,
                    StartIndex = context.Start.StartIndex,
                    StopIndex = context.Stop.StopIndex,
                    LockedSubstrings = new List<string>(this.lockedSubstrings),
                });
            }

            return ret;
        }

        /// <inheritdoc/>
        public override object VisitNumberSign([NotNull] MessageFormat.NumberSignContext context)
        {
            if (this.IsStyleContext(context))
            {
                string name = context.GetText();
                this.lockedSubstrings.Add(name);
            }

            return this.VisitChildren(context);
        }

        /// <inheritdoc/>
        public override object VisitNoneArg([NotNull] MessageFormat.NoneArgContext context)
        {
            string name = this.GetStringFromContext(context);
            if (!string.IsNullOrWhiteSpace(name))
            {
                this.lockedSubstrings.Add(name);
            }

            return this.VisitChildren(context);
        }

        /// <inheritdoc/>
        public override object VisitSimpleArg([NotNull] MessageFormat.SimpleArgContext context)
        {
            string name = this.GetStringFromContext(context);
            if (!string.IsNullOrWhiteSpace(name))
            {
                this.lockedSubstrings.Add(name);
            }

            return this.VisitChildren(context);
        }

        /// <inheritdoc/>
        public override object VisitPluralStyles([NotNull] MessageFormat.PluralStylesContext context)
        {
            string id = $"{context.Stop.StopIndex + 1}";

            string pluralParentId = this.GetPluralParentId(context);
            if (string.IsNullOrEmpty(pluralParentId))
            {
                pluralParentId = $"{this.pluralId}";
                this.pluralId++;
            }

            this.PluralDataList[id] = new PluralData()
            {
                Id = pluralParentId,
                StopIndex = context.Stop.StopIndex + 1,
            };

            return this.VisitChildren(context);
        }

        /// <inheritdoc/>
        public override object VisitSelectorPlural([NotNull] MessageFormat.SelectorPluralContext context)
        {
            ParserRuleContext parentContext = (ParserRuleContext)context.Parent;
            Type contextType = parentContext.GetType();
            if (contextType.Name != this.pluralStyleContext)
            {
                throw new ArgumentException("Grammar error: 'pluralStyle' must define 'selectorPlural'.");
            }

            string id = this.GetPluralListId(context);
            if (!this.UsedPluralSelectors.ContainsKey(id))
            {
                this.UsedPluralSelectors.Add(id, new List<PluralSelector>());
            }

            string selector = context.GetText();
            PluralSelectorEnum pluralSelectorType = PluralSelector.GetPluralSelectorType(selector);

            if (pluralSelectorType != PluralSelectorEnum.Null)
            {
                // string msg = this.GetPluralMessage(context);
                this.UsedPluralSelectors[id].Add(new PluralSelector()
                    {
                        PluralSelectorType = pluralSelectorType,
                        StartIndex = parentContext.Start.StartIndex,
                        StopIndex = parentContext.Stop.StopIndex,
                    });
            }

            return this.VisitChildren(context);
        }

        /// <summary>
        /// Gets the plural parent identifier.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Plural parent identifier.</returns>
        private string GetPluralParentId(MessageFormat.PluralStylesContext context)
        {
            string id = string.Empty;

            ParserRuleContext parentContext = (ParserRuleContext)context.Parent;
            while (parentContext != null)
            {
                Type contextType = parentContext.GetType();

                // Get the resource Id from the parent style in the parsing tree.
                if (contextType.Name == this.pluralStyleContext ||
                    contextType.Name == this.selectStyleContext ||
                    contextType.Name == this.selectOrdinalStyleContext)
                {
                    // Use the Selector as part of the resource Id.
                    foreach (var child in parentContext.children)
                    {
                        Type childContextType = child.GetType();
                        if (childContextType.Name.StartsWith(this.selector))
                        {
                            string name = child.GetText();

                            if (!string.IsNullOrEmpty(id))
                            {
                                name = $"{name}.";
                            }

                            id = name + id;

                            break;
                        }
                    }
                }

                // Check if within a parser rule.
                if (parentContext.Parent is ParserRuleContext)
                {
                    parentContext = (ParserRuleContext)parentContext.Parent;
                }
                else
                {
                    // Plain text.
                    break;
                }
            }

            return id;
        }

        /// <summary>
        /// Gets the plural list identifier.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Plural list identifier.</returns>
        private string GetPluralListId(ParserRuleContext context)
        {
            string id = string.Empty;

            ParserRuleContext parentContext = (ParserRuleContext)context.Parent;
            while (parentContext != null)
            {
                Type contextType = parentContext.GetType();

                // Get the resource Id from the parent style in the parsing tree.
                if (contextType.Name == this.pluralStylesContext)
                {
                    id = $"{parentContext.Stop.StopIndex + 1}";

                    break;
                }

                // Check if within a parser rule.
                if (parentContext.Parent is ParserRuleContext)
                {
                    parentContext = (ParserRuleContext)parentContext.Parent;
                }
                else
                {
                    // Plain text.
                    break;
                }
            }

            return id;
        }

        /// <summary>
        /// Gets the string from the context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The text of the context.</returns>
        private string GetStringFromContext(ParserRuleContext context)
        {
            string inputStream = context.Start.InputStream.ToString();
            string text = inputStream.Substring(context.Start.StartIndex, context.Stop.StopIndex - context.Start.StartIndex + 1);

            return text;
        }

        /// <summary>
        /// Gets the resource identifier.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>resource Id.</returns>
        private (string, string) GetResourceId(MessageFormat.MessageTextContext context)
        {
            (string, string) resourceId = (string.Empty, string.Empty);

            ParserRuleContext parentContext = (ParserRuleContext)context.Parent;
            while (parentContext != null)
            {
                Type contextType = parentContext.GetType();

                // Get the resource Id from the parent style in the parsing tree.
                if (contextType.Name.EndsWith(this.styleContext))
                {
                    string style = contextType.Name.Replace(this.styleContext, string.Empty);
                    string name = string.Empty;

                    // Use the Selector as part of the resource Id.
                    foreach (var child in parentContext.children)
                    {
                        Type childContextType = child.GetType();
                        if (childContextType.Name.StartsWith(this.selector))
                        {
                            name = child.GetText();

                            break;
                        }
                    }

                    resourceId = (style, name);

                    break;
                }

                // Check if within a parser rule.
                if (parentContext.Parent is ParserRuleContext)
                {
                    parentContext = (ParserRuleContext)parentContext.Parent;
                }
                else
                {
                    // Plain text.
                    break;
                }
            }

            return resourceId;
        }

        /// <summary>
        /// Determines whether [is style context] [the specified context].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if [is style context] [the specified context]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsStyleContext(MessageFormat.NumberSignContext context)
        {
            ParserRuleContext parentContext = (ParserRuleContext)context.Parent;
            while (parentContext != null)
            {
                Type contextType = parentContext.GetType();

                // Check if the context is in the style context.
                if (contextType.Name.EndsWith(this.styleContext))
                {
                    return true;
                }

                // Check if within a parser rule.
                if (parentContext.Parent is ParserRuleContext)
                {
                    parentContext = (ParserRuleContext)parentContext.Parent;
                }
                else
                {
                    // Plain text.
                    break;
                }
            }

            return false;
        }
    }
}
