// <copyright file="ICUTeamsSkypeTest.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace ICUParserLibUnitTest
{
    using System.Collections.Generic;
    using System.Globalization;
    using ICUParserLib;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests <see cref="ICUParser"/> using samples from Teams and Skype.
    /// </summary>
    public partial class ICUTest
    {
        /// <summary>
        /// Tests simple plurals.
        /// </summary>
        [TestMethod]
        public void TestSimplePlurals()
        {
            string input = "{numOfGuests, plural, one{1 guest} other{# guests}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsTrue(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("bg"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(6, messageItems.Count);
            Assert.AreEqual("1 guest", messageItems[0].Text);
            Assert.AreEqual("# guests", messageItems[1].Text);
            Assert.AreEqual("#", messageItems[1].LockedSubstrings[0]);
        }

        /// <summary>
        /// Tests Plurals with placeholder.
        /// </summary>
        [TestMethod]
        public void TestPluralsWithPlaceholder1()
        {
            string input = "{othersCount, plural, =0{ {name} has left the conversation.} =1{ {name} and {name2} have left the conversation.} other{ {name} and # others have left the conversation.} }";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsTrue(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("my"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(8, messageItems.Count);
            Assert.AreEqual(" {name} has left the conversation.", messageItems[0].Text);
            Assert.AreEqual("{name}", messageItems[0].LockedSubstrings[0]);

            Assert.AreEqual(" {name} and {name2} have left the conversation.", messageItems[1].Text);
            Assert.AreEqual("{name}", messageItems[1].LockedSubstrings[0]);
            Assert.AreEqual("{name2}", messageItems[1].LockedSubstrings[1]);

            Assert.AreEqual(" {name} and # others have left the conversation.", messageItems[2].Text);
            Assert.AreEqual("{name}", messageItems[2].LockedSubstrings[0]);
            Assert.AreEqual("#", messageItems[2].LockedSubstrings[1]);
        }

        /// <summary>
        /// Tests Plurals with placeholder.
        /// </summary>
        [TestMethod]
        public void TestPluralsWithPlaceholder2()
        {
            string input = "{shareHistoryDaysCount, plural, =0{<0></0> added <1></1> and <2></2> to the chat and shared all chat history.} =1{<0></0> added <1></1> and <2></2> to the chat and shared chat history from the past day.} other{<0></0> added <1></1> and <2></2> to the chat and shared chat history from the past # days.} }";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);
            Assert.IsTrue(icuParser.IsICU);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("my"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(8, messageItems.Count);
            Assert.AreEqual("<0></0> added <1></1> and <2></2> to the chat and shared all chat history.", messageItems[0].Text);
            Assert.AreEqual("<0></0> added <1></1> and <2></2> to the chat and shared chat history from the past day.", messageItems[1].Text);
            Assert.AreEqual("<0></0> added <1></1> and <2></2> to the chat and shared chat history from the past # days.", messageItems[2].Text);
        }

        /// <summary>
        /// Tests Plurals with placeholder.
        /// </summary>
        [TestMethod]
        public void TestPluralsWithPlaceholder3()
        {
            string input = "{interval, plural, =1{Occurs every month on the {weekInMonthIndex} {weekday} starting {startDate} until {endDate}} other {Occurs every # months on the {weekInMonthIndex} {weekday} starting {startDate} until {endDate}}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("my"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(7, messageItems.Count);
            Assert.AreEqual("Occurs every month on the {weekInMonthIndex} {weekday} starting {startDate} until {endDate}", messageItems[0].Text);
            Assert.AreEqual("{weekInMonthIndex}", messageItems[0].LockedSubstrings[0]);
            Assert.AreEqual("{weekday}", messageItems[0].LockedSubstrings[1]);
            Assert.AreEqual("{startDate}", messageItems[0].LockedSubstrings[2]);
            Assert.AreEqual("{endDate}", messageItems[0].LockedSubstrings[3]);

            Assert.AreEqual("Occurs every # months on the {weekInMonthIndex} {weekday} starting {startDate} until {endDate}", messageItems[1].Text);
            Assert.AreEqual("#", messageItems[1].LockedSubstrings[0]);
            Assert.AreEqual("{weekInMonthIndex}", messageItems[1].LockedSubstrings[1]);
            Assert.AreEqual("{weekday}", messageItems[1].LockedSubstrings[2]);
            Assert.AreEqual("{startDate}", messageItems[1].LockedSubstrings[3]);
            Assert.AreEqual("{endDate}", messageItems[1].LockedSubstrings[4]);
        }

        /// <summary>
        /// Tests Plurals with literal ranges.
        /// </summary>
        [TestMethod]
        public void TestPluralsWithLiteralRanges1()
        {
            string input = "{count, plural, =1{One person liked this message} other{# people liked this message}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("my"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(7, messageItems.Count);
            Assert.AreEqual("One person liked this message", messageItems[0].Text);
            Assert.AreEqual("# people liked this message", messageItems[1].Text);
        }

        /// <summary>
        /// Tests Plurals with literal ranges.
        /// </summary>
        [TestMethod]
        public void TestPluralsWithLiteralRanges2()
        {
            string input = "{num, plural, =1{Liked by {userName}} other{Liked by # people}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("my"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(7, messageItems.Count);
            Assert.AreEqual("Liked by {userName}", messageItems[0].Text);
            Assert.AreEqual("Liked by # people", messageItems[1].Text);
        }

        /// <summary>
        /// Tests Select Used for Gender.
        /// </summary>
        [TestMethod]
        public void TestSelectUsedForGender1()
        {
            string input = "{emoji, select, enabled{📷  Image} other{Image}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(2, messageItems.Count);
            Assert.AreEqual("📷  Image", messageItems[0].Text);
            Assert.AreEqual("Image", messageItems[1].Text);

            // Modify the strings, check if the composed string is different, revert back and test again.
            this.PostTestStringCheck(icuParser, messageItems);
        }

        /// <summary>
        /// Tests Select Used for Gender.
        /// </summary>
        [TestMethod]
        public void TestSelectUsedForGender2()
        {
            string input = "{context, select, chats{Bots for your chat} channel{Bots for your team} private_channel{Bots for your private channel} other{} }";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(3, messageItems.Count);
            Assert.AreEqual("Bots for your chat", messageItems[0].Text);
            Assert.AreEqual("Bots for your team", messageItems[1].Text);
            Assert.AreEqual("Bots for your private channel", messageItems[2].Text);

            // Modify the strings, check if the composed string is different, revert back and test again.
            this.PostTestStringCheck(icuParser, messageItems);
        }

        /// <summary>
        /// Tests Select Used for Gender.
        /// </summary>
        [TestMethod]
        public void TestSelectUsedForGender3()
        {
            string input = "{context, select, chats{Tabs for your chat} channel{Tabs for your team} private_channel{Tabs for your private channel} other{} }";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems);

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(3, messageItems.Count);
            Assert.AreEqual("Tabs for your chat", messageItems[0].Text);
            Assert.AreEqual("Tabs for your team", messageItems[1].Text);
            Assert.AreEqual("Tabs for your private channel", messageItems[2].Text);

            // Modify the strings, check if the composed string is different, revert back and test again.
            this.PostTestStringCheck(icuParser, messageItems);
        }

        /// <summary>
        /// Tests Nested Select Plural strings.
        /// </summary>
        [TestMethod]
        public void TestNestedSelectPluralStrings()
        {
            string input = "{gender, select, female{{count, plural, =1{Last seen 1m ago} other{Last seen #m ago}}} male{{count, plural, =1{Last seen 1m ago} other{Last seen #m ago}}} other{{count, plural, =1{Last seen 1m ago} other{Last seen #m ago}}}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("my"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(21, messageItems.Count);
            Assert.AreEqual("Last seen 1m ago", messageItems[0].Text);
            Assert.AreEqual("Last seen #m ago", messageItems[1].Text);
            Assert.AreEqual("Last seen 1m ago", messageItems[2].Text);
            Assert.AreEqual("Last seen #m ago", messageItems[3].Text);
            Assert.AreEqual("Last seen 1m ago", messageItems[4].Text);
            Assert.AreEqual("Last seen #m ago", messageItems[5].Text);

            // Rerun the parser with option to remove duplicate message strings.
            icuParser = new ICUParser(input, true);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            messageItems = icuParser.GetMessageItems();
            output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("my"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(17, messageItems.Count);
            Assert.AreEqual("Last seen 1m ago", messageItems[0].Text);
            Assert.AreEqual("Last seen #m ago", messageItems[1].Text);
        }

        /// <summary>
        /// Tests Nested Plural -> Plural strings.
        /// </summary>
        [TestMethod]
        public void TestNestedPluralPluralStrings()
        {
            string input = @"{additionalSenderCount, plural, 
                one{
                    {replyCount, plural, 
                        one{ 'one' # more reply from {sender1}, {sender2}, {sender3}, and # other {numNewClause}} 
                        other{ 'one' # more replies from {sender1}, {sender2}, {sender3}, and # other {numNewClause}}
                    }} 
                other{
                    {replyCount, plural, 
                        one{ 'other' # more reply from {sender1}, {sender2}, {sender3}, and # others {numNewClause}} 
                        other{ 'other' # more replies from {sender1}, {sender2}, {sender3}, and # others {numNewClause}}
                    }}
                }";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("ar-SA"));

            /*
            {additionalSenderCount, plural,
                one{
                    {replyCount, plural,
                        one{ 'one' # more reply from {sender1}, {sender2}, {sender3}, and # other {numNewClause}}
                        zero { 'one' # more replies from {sender1}, {sender2}, {sender3}, and # other {numNewClause}}
                        two { 'one' # more replies from {sender1}, {sender2}, {sender3}, and # other {numNewClause}}
                        few { 'one' # more replies from {sender1}, {sender2}, {sender3}, and # other {numNewClause}}
                        many { 'one' # more replies from {sender1}, {sender2}, {sender3}, and # other {numNewClause}}
                        other{ 'one' # more replies from {sender1}, {sender2}, {sender3}, and # other {numNewClause}}
                    }}
                other{
                    {replyCount, plural,
                        one{ 'other' # more reply from {sender1}, {sender2}, {sender3}, and # others {numNewClause}}
                        zero { 'other' # more replies from {sender1}, {sender2}, {sender3}, and # others {numNewClause}}
                        two { 'other' # more replies from {sender1}, {sender2}, {sender3}, and # others {numNewClause}}
                        few { 'other' # more replies from {sender1}, {sender2}, {sender3}, and # others {numNewClause}}
                        many { 'other' # more replies from {sender1}, {sender2}, {sender3}, and # others {numNewClause}}
                        other{ 'other' # more replies from {sender1}, {sender2}, {sender3}, and # others {numNewClause}}
                    }}
                }
            */

            // Assert.
            Assert.IsTrue(output.Length > input.Length);
            Assert.AreEqual(12, messageItems.Count);
            Assert.AreEqual(" 'one' # more replies from {sender1}, {sender2}, {sender3}, and # other {numNewClause}", messageItems[4].Text);
            Assert.AreEqual(" 'other' # more replies from {sender1}, {sender2}, {sender3}, and # others {numNewClause}", messageItems[8].Text);
        }

        /// <summary>
        /// Tests Poorly constructed ICU - functionally correct, concatenated strings.
        /// </summary>
        [TestMethod]
        public void TestPoorlyConstructedStrings1()
        {
            string input = "Mark message as urgent and repeatedly notify {count, plural, one{recipient} other{recipients}} every {interval} minutes for {window} minutes";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsFalse(icuParser.Success);

            Assert.AreEqual(2, icuParser.Errors.Count);
            Assert.AreEqual("Strict parse mode enabled. Content contains leading/trailing text 'Mark message as urgent and repeatedly notify '.", icuParser.Errors[0]);
            Assert.AreEqual("Strict parse mode enabled. Content contains leading/trailing text ' every {interval} minutes for {window} minutes'.", icuParser.Errors[1]);
        }

        /// <summary>
        /// Tests Poorly constructed ICU - functionally correct, concatenated strings.
        /// </summary>
        [TestMethod]
        public void TestPoorlyConstructedStrings2()
        {
            string input = "{replyCount, plural, one{1 reply} other{# replies}} from {sender1}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsFalse(icuParser.Success);

            Assert.AreEqual(1, icuParser.Errors.Count);
            Assert.AreEqual("Strict parse mode enabled. Content contains leading/trailing text ' from {sender1}'.", icuParser.Errors[0]);
        }

        /// <summary>
        /// Tests Poorly constructed ICU - functionally correct, concatenated strings.
        /// </summary>
        [TestMethod]
        public void TestPoorlyConstructedStrings3()
        {
            string input = "{memberCount, plural, =1{# member} other{# members}}{guestCount, plural, =0{} =1{ and # guest} other{ and # guests}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("my"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(14, messageItems.Count);
            Assert.AreEqual("# member", messageItems[0].Text);
            Assert.AreEqual("# members", messageItems[1].Text);
            Assert.AreEqual(" and # guest", messageItems[2].Text);
            Assert.AreEqual(" and # guests", messageItems[3].Text);
        }

        /// <summary>
        /// Tests other strings.
        /// </summary>
        [TestMethod]
        public void TestOther1()
        {
            string input = @"{replyCount, plural, 
                one{one} 
                =2{2} 
                =3{3} 
                =4{4} 
                other{# replies}
                }";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("bg"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(9, messageItems.Count);
            Assert.AreEqual("one", messageItems[0].Text);
            Assert.AreEqual("# replies", messageItems[4].Text);
        }

        /// <summary>
        /// Tests other strings.
        /// </summary>
        [TestMethod]
        public void TestOther2()
        {
            // string input = "{hour, plural, =0{} one{# hour} other{# hours}} {min, plural, =0{} one{# minute} other{# minutes}} {sec, plural, =0{} one{# second} other{# seconds}} ";
            string input = "{hour, plural, =0{0} one{# hour} other{# hours}}";

            ICUParser icuParser = new ICUParser(input);

            // Assert.
            Assert.IsTrue(icuParser.Success);

            List<MessageItem> messageItems = icuParser.GetMessageItems();
            string output = icuParser.ComposeMessageText(messageItems, CultureInfo.GetCultureInfo("bg"));

            // Assert.
            Assert.AreEqual(input, output, "Different text output.");
            Assert.AreEqual(7, messageItems.Count);
            Assert.AreEqual("# hour", messageItems[1].Text);
            Assert.AreEqual("# hours", messageItems[2].Text);
        }
    }
}
