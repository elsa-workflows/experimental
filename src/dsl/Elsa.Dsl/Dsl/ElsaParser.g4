parser grammar ElsaParser;

options { tokenVocab = ElsaLexer; }

file                
    :   (stat | LINE_COMMENT)*
    ;
    
trigger
    :   TRIGGER object
    ;
    
object
    :   ID objectInitializer?
    ;
    
newObject
    :   NEW ID ('<' type '>')? '(' args? ')'
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
    |   ID
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
    
block
    :   '{' stat* '}'
    ;
    
objectInitializer
    :   '{' propertyList? '}'
    ;
    
propertyList
    :   property (',' property)*
    ;
    
property
    :   ID ':' expr
    ;
    
stat
    :   trigger ';'                                         #triggerStat
    |   object ';'                                          #objectStat       
    |   'if' expr 'then' stat ('else' stat)?                #ifStat
    |   'for' '(' ID '=' expr ';' expr ';' expr ')' stat    #forStat
    |   'return' expr? ';'                                  #returnStat                              
    |   block                                               #blockStat
    |   varDecl ';'                                         #variableDeclarationStat
    |   localVarDecl ';'                                    #localVariableDeclarationStat
    |   expr '=' expr ';'                                   #assignmentStat
    |   expr ';'                                            #expressionStat
    ;

expr
    :   funcCall                         #functionExpr
    |   object                           #objectExpr
    |   newObject                        #newObjectExpr
    |   expr '++'                        #incrementExpr
    |   expr '--'                        #decrementExpr
    |   '-' expr                         #negateExpr
    |   '!' expr                         #notExpr
    |   expr '*' expr                    #multiplyExpr
    |   expr '+' expr                    #addExpr
    |   expr '-' expr                    #subtractExpr
    |   expr ('==' | '>' | '<') expr     #compareExpr
    |   INTEGER_VAL                      #integerValueExpr
    |   STRING_VAL                       #stringValueExpr
    |   '(' exprList? ')'                #parenthesesExpr
    |   '[' exprList? ']'                #bracketsExpr
    |   methodCall                       #methodCallExpr
    |   ID                               #variableExpr
    ;
    
exprList
    :   expr (',' expr)*
    ;