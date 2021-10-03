parser grammar ElsaParser;

options { tokenVocab=ElsaLexer; }

file                
    :   (trigger | root | stat | LINE_COMMENT)*
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
    :   VARIABLE ID (':' type)? (EQ expr)?
    ;
    
localVarDecl
    :   'let' ID (':' type)? (EQ expr)?
    ;

type
    :   VOID
    |   FLOAT
    |   INT
    |   OBJECT
    |   STRING
    ;
     
methodCall
    :   ID '.' funcCall
    ;
                
funcCall
    :   ID '(' args? ')'
    ;
    
args
    :   arg (',' arg)*
    ;
    
arg
    :   expr
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
    :   'if' expr 'then' stat ('else' stat)?                #if
    |   'for' '(' ID '=' expr ';' expr ';' expr ')' stat    #for
    |   'return' expr? ';'                                  #return
    |   block_statements                                    #blockStatements
    |   varDecl ';'                                         #variableDeclaration
    |   localVarDecl ';'                                    #localVariableDeclaration
    |   expr '=' expr ';'                                   #assignment
    |   expr ';'                                            #expression
    ;
    
    
expr
    :   funcCall                         #functionCall
    |   expr '++'                        #increment
    |   expr '--'                        #decrement
    |   '-' expr                         #negate
    |   '!' expr                         #not
    |   expr '*' expr                    #multiply
    |   expr '+' expr                    #add
    |   expr '-' expr                    #subtract
    |   expr ('==' | '>' | '<') expr     #compare
    |   INTEGER_VAL                      #integerValue
    |   STRING_VAL                       #stringValue
    |   '(' expr ')'                     #parentheses
    |   '[' exprList ']'                 #brackets
    |   methodCall                       #methodInvocation
    |   ID                               #variableReference
    ;
    
exprList
    :   expr (',' expr)*
    ;