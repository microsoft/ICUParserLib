parser grammar MessageFormat;

options
{
    tokenVocab = MessageFormatLexer;
}

// Start rule.
message : (messageText | argument)*;

// Include message format structures without arguments in the message text.
//  Example: {host} invites {guest} and # other people to her party.
messageText : (noneArg | simpleArg | numberSign | TEXT)+;

/*
    https://unicode-org.github.io/icu-docs/apidoc/released/icu4j/com/ibm/icu/text/MessageFormat.html
        The choice argument type is deprecated. Use plural arguments for proper plural selection,
        and select arguments for simple selection among a fixed set of choices.

http://cldr.unicode.org/index/cldr-spec/plural-rules
*/

argument : complexArg;
//complexArg : choiceArg | pluralArg | selectArg | selectordinalArg;
complexArg : pluralArg | selectArg | selectOrdinalArg;

numberSign : NUMBER_SIGN;

// Example: {host} invites {guest} to her party.
noneArg : MESSAGEFORMAT_BEGIN noneArgNameOrNumber MESSAGEFORMAT_END;
noneArgNameOrNumber : argNameOrNumber;

// Example: Rated <ph name=""AVERAGE_RATING""><ex>3.2</ex>{0, number,0.0}</ph> by one user.
simpleArg : MESSAGEFORMAT_BEGIN simpleArgNameOrNumber SEPARATOR argType (SEPARATOR argStyle)? MESSAGEFORMAT_END;
simpleArgNameOrNumber : argNameOrNumber;

//choiceArg : MESSAGEFORMAT_BEGIN argNameOrNumber SEPARATOR CHOICE_KEYWORD SEPARATOR choiceStyle MESSAGEFORMAT_END;
//choiceStyle: see ChoiceFormat

// Example: {count, plural,
//            =1 { Relaunch Microsoft Edge within a day}
//            other { Relaunch Microsoft Edge within # days}}
pluralArg : MESSAGEFORMAT_BEGIN pluralArgNameOrNumber SEPARATOR PLURAL_KEYWORD SEPARATOR offsetValue? pluralStyles MESSAGEFORMAT_END;
pluralArgNameOrNumber : argNameOrNumber;
pluralStyles : pluralStyle+;
pluralStyle : selectorPlural ARGUMENT_BEGIN message ARGUMENT_END;
offsetValue : OFFSET_KEYWORD NUMBER;
selectorPlural : explicitValue | keyword;
explicitValue : EQUAL_SIGN NUMBER;
keyword : KEYWORD;

// Example: {gender, select, female {allée} other {allé}} à Paris.
selectArg : MESSAGEFORMAT_BEGIN selectArgNameOrNumber SEPARATOR SELECT_KEYWORD SEPARATOR selectStyle+ MESSAGEFORMAT_END;
selectArgNameOrNumber : argNameOrNumber;
selectStyle : selectorSelect ARGUMENT_BEGIN message ARGUMENT_END;
selectorSelect : KEYWORD;

// Example: It's my cat's {year, selectordinal,
//                                one {#st}
//                                two {#nd}
//                                few {#rd}
//                                other {#th}
//                            } birthday!
selectOrdinalArg : MESSAGEFORMAT_BEGIN selectOrdinalArgNameOrNumber SEPARATOR SELECTORDINAL_KEYWORD SEPARATOR selectOrdinalStyles MESSAGEFORMAT_END;
selectOrdinalArgNameOrNumber : argNameOrNumber;
selectOrdinalStyles : selectOrdinalStyle+;
selectOrdinalStyle : selectorOrdinalPlural ARGUMENT_BEGIN message ARGUMENT_END;
selectorOrdinalPlural : explicitValue | keyword;

argNameOrNumber : argName | argNumber | argType | argKeywords;

argName : KEYWORD;
argNumber : NUMBER;

argType : ARGTYPE_NUMBER |
    ARGTYPE_DATE |
    ARGTYPE_TIME |
    ARGTYPE_SPELLOUT |
    ARGTYPE_ORDINAL |
    ARGTYPE_DURATION;

argKeywords : OFFSET_KEYWORD |
    CHOICE_KEYWORD |
    PLURAL_KEYWORD |
    SELECT_KEYWORD |
    SELECTORDINAL_KEYWORD;

argStyle :
    ARGSTYLE_DOUBLECOLON? argStyleText;

argStyleText : (SINGLE_QUOTED_KEYWORD | KEYWORD | NUMBER)+;
