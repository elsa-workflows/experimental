parser grammar ElsaParser;

options { tokenVocab=ElsaLexer; }

file                
    :   LINE_COMMENT* trigger* root (stat | LINE_COMMENT)*
    ;
    
trigger
    :   TRIGGER id block_pairs
    ;
    
root
    :   activity
    ;
    
activity
    :   sequence
    |   id
    ;
    
sequence
    :   SEQUENCE
    |   block_statements
    ;

varDecl             
    :   VARIABLE id (':' type)? (EQ expr)? ';'
    ;
    
localVarDecl
    :   'let' id (':' type)? (EQ expr)? ';'
    ;

type                
    :   VOID
    |   FLOAT
    |   INT
    |   OBJECT
    |   STRING
    ;
                
funcCall
    :   id '(' args? ')' ';'
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
    :   id ':' expr
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
    :   id exprList
    |   expr ('++' | '--')
    |   '-' expr
    |   '!' expr
    |   expr '*' expr
    |   expr ('+' | '-') expr
    |   expr ('==' | '>' | '<') expr
    |   INTEGER_VAL
    |   STRING_VAL
    |   '(' expr ')'
    |   '[' expr ']'
    |   id
    ;
    
exprList
    :   expr (',' expr)*
    ;
    
id
    :   ID
    ;