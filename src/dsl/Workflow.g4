grammar Workflow;

metadata: id description version root? EOF?;

root: NEWLINE activity;

activity: WORD NEWLINE WHITESPACE properties;

properties: (property)*;

property: WORD COLON WHITESPACE? SENTENCE;

id: 'Id:' WHITESPACE? WORD+ NEWLINE?;
description: 'Description:' WHITESPACE? SENTENCE+ NEWLINE?;
version: 'Version:' WHITESPACE? INTEGER NEWLINE?;

fragment LOWERCASE  : [a-z] ;
fragment UPPERCASE  : [A-Z] ;

COLON: ':';
WORD: (LOWERCASE | UPPERCASE | '_')+;
SENTENCE: (WORD WHITESPACE?)+;
WHITESPACE: [ \t]+;
NEWLINE: [\r\n]+;
INTEGER: DIGIT+;
DIGIT: [0-9];