//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.9.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:/Projects/Elsa/experimental/src/dsl/Elsa.Dsl/Dsl\ElsaParser.g4 by ANTLR 4.9.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="ElsaParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.9.1")]
[System.CLSCompliant(false)]
public interface IElsaParserVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElsaParser.file"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFile([NotNull] ElsaParser.FileContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElsaParser.trigger"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTrigger([NotNull] ElsaParser.TriggerContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElsaParser.object"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitObject([NotNull] ElsaParser.ObjectContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElsaParser.newObject"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNewObject([NotNull] ElsaParser.NewObjectContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElsaParser.varDecl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVarDecl([NotNull] ElsaParser.VarDeclContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElsaParser.localVarDecl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLocalVarDecl([NotNull] ElsaParser.LocalVarDeclContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElsaParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitType([NotNull] ElsaParser.TypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElsaParser.methodCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodCall([NotNull] ElsaParser.MethodCallContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElsaParser.funcCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFuncCall([NotNull] ElsaParser.FuncCallContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElsaParser.args"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArgs([NotNull] ElsaParser.ArgsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElsaParser.arg"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArg([NotNull] ElsaParser.ArgContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElsaParser.expr_external"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpr_external([NotNull] ElsaParser.Expr_externalContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElsaParser.expr_external_value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpr_external_value([NotNull] ElsaParser.Expr_external_valueContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElsaParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBlock([NotNull] ElsaParser.BlockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElsaParser.objectInitializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitObjectInitializer([NotNull] ElsaParser.ObjectInitializerContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElsaParser.propertyList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPropertyList([NotNull] ElsaParser.PropertyListContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElsaParser.property"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProperty([NotNull] ElsaParser.PropertyContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>triggerStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTriggerStat([NotNull] ElsaParser.TriggerStatContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>objectStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitObjectStat([NotNull] ElsaParser.ObjectStatContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ifStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIfStat([NotNull] ElsaParser.IfStatContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>forStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitForStat([NotNull] ElsaParser.ForStatContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>returnStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitReturnStat([NotNull] ElsaParser.ReturnStatContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>blockStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBlockStat([NotNull] ElsaParser.BlockStatContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>variableDeclarationStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariableDeclarationStat([NotNull] ElsaParser.VariableDeclarationStatContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>localVariableDeclarationStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLocalVariableDeclarationStat([NotNull] ElsaParser.LocalVariableDeclarationStatContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>assignmentStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignmentStat([NotNull] ElsaParser.AssignmentStatContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpressionStat([NotNull] ElsaParser.ExpressionStatContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElsaParser.thenStat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitThenStat([NotNull] ElsaParser.ThenStatContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElsaParser.elseStat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitElseStat([NotNull] ElsaParser.ElseStatContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>newObjectExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNewObjectExpr([NotNull] ElsaParser.NewObjectExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>subtractExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSubtractExpr([NotNull] ElsaParser.SubtractExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>incrementExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIncrementExpr([NotNull] ElsaParser.IncrementExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>objectExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitObjectExpr([NotNull] ElsaParser.ObjectExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>stringValueExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStringValueExpr([NotNull] ElsaParser.StringValueExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>multiplyExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMultiplyExpr([NotNull] ElsaParser.MultiplyExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>parenthesesExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParenthesesExpr([NotNull] ElsaParser.ParenthesesExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>functionExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunctionExpr([NotNull] ElsaParser.FunctionExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>decrementExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDecrementExpr([NotNull] ElsaParser.DecrementExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>negateExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNegateExpr([NotNull] ElsaParser.NegateExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>methodCallExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodCallExpr([NotNull] ElsaParser.MethodCallExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>variableExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariableExpr([NotNull] ElsaParser.VariableExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>notExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNotExpr([NotNull] ElsaParser.NotExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>integerValueExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIntegerValueExpr([NotNull] ElsaParser.IntegerValueExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>addExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAddExpr([NotNull] ElsaParser.AddExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>backTickStringValueExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBackTickStringValueExpr([NotNull] ElsaParser.BackTickStringValueExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>bracketsExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBracketsExpr([NotNull] ElsaParser.BracketsExprContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>compareExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCompareExpr([NotNull] ElsaParser.CompareExprContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElsaParser.exprList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExprList([NotNull] ElsaParser.ExprListContext context);
}
