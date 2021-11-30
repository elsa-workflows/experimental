export type ExpressionType = string;
export type Expression = LiteralExpression | JavaScriptExpression;

export interface LiteralExpression {
  type: ExpressionType;
  value: any;
}

export interface JavaScriptExpression {
  type: ExpressionType;
  scriptExpression: string;
}
