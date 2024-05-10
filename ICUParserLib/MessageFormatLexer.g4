lexer grammar MessageFormatLexer;

// Default message tokens.
MESSAGEFORMAT_BEGIN : '{' -> pushMode(ARGUMENT_SECTION);
ARGUMENT_END : '}' -> popMode;

TEXT : STRING |
    DOUBLE_QUOTED_STRING;

DOUBLE_QUOTED_STRING : '"' .*? '"';
STRING : ~[{}"#]+;
NUMBER_SIGN : '#';

// '\u00A0' NO-BREAK SPACE
WHITESPACE : (' ' | '\u00A0' | '\t' | '\r' | '\n')+ -> skip;

// Message format argument Tokens.
mode ARGUMENT_SECTION;
ARGUMENT_BEGIN : '{' -> pushMode(DEFAULT_MODE);
MESSAGEFORMAT_END : '}' -> popMode;

OFFSET_KEYWORD : 'offset:';
CHOICE_KEYWORD : 'choice';
PLURAL_KEYWORD : 'plural';
SELECT_KEYWORD : 'select';
SELECTORDINAL_KEYWORD : 'selectordinal';

EQUAL_SIGN : '=';

ARGTYPE_NUMBER : 'number';
ARGTYPE_DATE : 'date';
ARGTYPE_TIME : 'time';
ARGTYPE_SPELLOUT : 'spellout';
ARGTYPE_ORDINAL : 'ordinal';
ARGTYPE_DURATION : 'duration';

ARGSTYLE_DOUBLECOLON : '::';

SINGLE_QUOTED_KEYWORD : '\'' ('\\\''|.)*? '\'';

NUMBER : [0-9]+[.]?[0-9]*;
KEYWORD : [a-zA-Z_][a-zA-Z_0-9]*;
SEPARATOR : ',';

// '\u00A0' NO-BREAK SPACE
ARGUMENT_WHITESPACE : (' ' | '\u00A0' | '\t' | '\r' | '\n')+ -> skip;
