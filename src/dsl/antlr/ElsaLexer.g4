lexer grammar ElsaLexer;

EQ                  :   '=';
GREATER             :   '>';
TRIGGER             :   'trigger';
VARIABLE            :   'variable';
LET                 :   'let';
IF                  :   'if';
THEN                :   'then';
ELSE                :   'else';
FOR                 :   'for';
RETURN              :   'return';
VOID                :   'void';
FLOAT               :   'float';
INT                 :   'int';
STRING              :   'string';
OBJECT              :   'object';
SEQUENCE            :   'Sequence';
ID                  :   LETTER (LETTER | DIGIT)*;
SYMBOL              :   [=>$+`];
INTEGER_VAL         :   DIGIT+;
COLON               :   ':';
SEMICOLON           :   ';';
COMMA               :   ',';
PLUS                :   '+';
MINUS               :   '-';
STAR                :   '*';
EQUALS              :   '==';
NOT_EQUALS          :   '!=';
GREATER_EQUALS      :   '>=';
LESS                :   '<';
LESS_EQUALS         :   '<=';
DECREMENT           :   '--';
INCREMENT           :   '++';
PARENTHESES_OPEN    :   '(';
PARENTHESES_CLOSE   :   ')';
BRACKET_OPEN        :   '[';
BRACKET_CLOSE       :   ']';
CURLYBRACE_OPEN     :   '{';
CURLYBRACE_CLOSE    :   '}';
EXCLAMATION         :   '!';
LANGSPEC_BEGIN      :   '/' ~[/]+ '/' .*? '/';
STRING_VAL          :   '"' ('\\"' | .)*? '"';

LINE_COMMENT        :   '//' .*? '\r'? '\n' -> skip;
TEXT                :   (LETTER|SYMBOL)+;
CODE_VAL            :   '@' ('@' | .)*? '@' -> skip;
WS                  :   [ \t\r\n] -> skip;

fragment LETTER     :   [a-zA-Z_];
fragment DIGIT      :   [0-9];

//mode EXTERNAL_EXPRESSION;
//LANGSPEC_END        :   '/' -> popMode;