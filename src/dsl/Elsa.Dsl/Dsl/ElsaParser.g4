parser grammar ElsaParser;

options { tokenVocab=ElsaLexer; }

file                
    :   LINE_COMMENT* trigger* root? (stat | LINE_COMMENT)*
    ;
    
trigger
    :   TRIGGER ID block_pairs
    ;
    
root
    :   activity
    ;
    
activity
    :   sequence
    |   ID
    ;
    
sequence
    :   SEQUENCE
    |   block_statements
    ;

varDecl             
    :   VARIABLE ID (':' type)? (EQ expr)? ';'
    ;
    
localVarDecl
    :   'let' ID (':' type)? (EQ expr)? ';'
    ;

type
    :   VOID
    |   FLOAT
    |   INT
    |   OBJECT
    |   STRING
    ;
                
funcCall
    :   ID '(' args? ')' ';'
    ;
    
args
    :   arg (',' arg)*
    ;
    
arg
    :   CODE_VAL expr
    |   expr
    ;
    
block_statements
    :   '{' stat* '}'
    ;
    
block_pairs
    :   '{' pairList '}'
    ;
    
pairList
    :   pair (',' pair)*
    ;
    
pair
    :   ID ':' expr
    ; 
    
stat
    :   'if' expr 'then' stat ('else' stat)?
    |   'for' '(' ID '=' expr ';' expr ';' expr ')' stat   
    |   'return' expr? ';'
    |   block_statements
    |   varDecl
    |   localVarDecl
    |   funcCall
    |   expr '=' expr ';'
    |   expr ';'
    ;
    
    
expr
    :   ID exprList
    |   expr '++'
    |   expr '--'
    |   '-' expr
    |   '!' expr
    |   expr '*' expr
    |   expr ('+' | '-') expr
    |   expr ('==' | '>' | '<') expr
    |   INTEGER_VAL
    |   STRING_VAL
    |   '(' expr ')'
    |   '[' expr ']'
    |   ID
    ;
    
exprList
    :   expr (',' expr)*
    ;