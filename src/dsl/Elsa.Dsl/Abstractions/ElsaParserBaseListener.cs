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
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IElsaParserListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.9.1")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CLSCompliant(false)]
public partial class ElsaParserBaseListener : IElsaParserListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElsaParser.file"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFile([NotNull] ElsaParser.FileContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElsaParser.file"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFile([NotNull] ElsaParser.FileContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElsaParser.trigger"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTrigger([NotNull] ElsaParser.TriggerContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElsaParser.trigger"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTrigger([NotNull] ElsaParser.TriggerContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElsaParser.object"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterObject([NotNull] ElsaParser.ObjectContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElsaParser.object"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitObject([NotNull] ElsaParser.ObjectContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElsaParser.newObject"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNewObject([NotNull] ElsaParser.NewObjectContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElsaParser.newObject"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNewObject([NotNull] ElsaParser.NewObjectContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElsaParser.varDecl"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVarDecl([NotNull] ElsaParser.VarDeclContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElsaParser.varDecl"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVarDecl([NotNull] ElsaParser.VarDeclContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElsaParser.localVarDecl"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLocalVarDecl([NotNull] ElsaParser.LocalVarDeclContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElsaParser.localVarDecl"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLocalVarDecl([NotNull] ElsaParser.LocalVarDeclContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElsaParser.type"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterType([NotNull] ElsaParser.TypeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElsaParser.type"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitType([NotNull] ElsaParser.TypeContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElsaParser.methodCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMethodCall([NotNull] ElsaParser.MethodCallContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElsaParser.methodCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMethodCall([NotNull] ElsaParser.MethodCallContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElsaParser.funcCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFuncCall([NotNull] ElsaParser.FuncCallContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElsaParser.funcCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFuncCall([NotNull] ElsaParser.FuncCallContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElsaParser.args"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArgs([NotNull] ElsaParser.ArgsContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElsaParser.args"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArgs([NotNull] ElsaParser.ArgsContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElsaParser.arg"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArg([NotNull] ElsaParser.ArgContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElsaParser.arg"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArg([NotNull] ElsaParser.ArgContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElsaParser.block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBlock([NotNull] ElsaParser.BlockContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElsaParser.block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBlock([NotNull] ElsaParser.BlockContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElsaParser.objectInitializer"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterObjectInitializer([NotNull] ElsaParser.ObjectInitializerContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElsaParser.objectInitializer"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitObjectInitializer([NotNull] ElsaParser.ObjectInitializerContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElsaParser.propertyList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPropertyList([NotNull] ElsaParser.PropertyListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElsaParser.propertyList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPropertyList([NotNull] ElsaParser.PropertyListContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElsaParser.property"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterProperty([NotNull] ElsaParser.PropertyContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElsaParser.property"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitProperty([NotNull] ElsaParser.PropertyContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>triggerStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTriggerStat([NotNull] ElsaParser.TriggerStatContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>triggerStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTriggerStat([NotNull] ElsaParser.TriggerStatContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>objectStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterObjectStat([NotNull] ElsaParser.ObjectStatContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>objectStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitObjectStat([NotNull] ElsaParser.ObjectStatContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>ifStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIfStat([NotNull] ElsaParser.IfStatContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>ifStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIfStat([NotNull] ElsaParser.IfStatContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>forStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterForStat([NotNull] ElsaParser.ForStatContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>forStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitForStat([NotNull] ElsaParser.ForStatContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>returnStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterReturnStat([NotNull] ElsaParser.ReturnStatContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>returnStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitReturnStat([NotNull] ElsaParser.ReturnStatContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>blockStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBlockStat([NotNull] ElsaParser.BlockStatContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>blockStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBlockStat([NotNull] ElsaParser.BlockStatContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>variableDeclarationStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVariableDeclarationStat([NotNull] ElsaParser.VariableDeclarationStatContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>variableDeclarationStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVariableDeclarationStat([NotNull] ElsaParser.VariableDeclarationStatContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>localVariableDeclarationStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLocalVariableDeclarationStat([NotNull] ElsaParser.LocalVariableDeclarationStatContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>localVariableDeclarationStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLocalVariableDeclarationStat([NotNull] ElsaParser.LocalVariableDeclarationStatContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>assignmentStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAssignmentStat([NotNull] ElsaParser.AssignmentStatContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>assignmentStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAssignmentStat([NotNull] ElsaParser.AssignmentStatContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>expressionStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExpressionStat([NotNull] ElsaParser.ExpressionStatContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>expressionStat</c>
	/// labeled alternative in <see cref="ElsaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExpressionStat([NotNull] ElsaParser.ExpressionStatContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>newObjectExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNewObjectExpr([NotNull] ElsaParser.NewObjectExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>newObjectExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNewObjectExpr([NotNull] ElsaParser.NewObjectExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>subtractExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterSubtractExpr([NotNull] ElsaParser.SubtractExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>subtractExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitSubtractExpr([NotNull] ElsaParser.SubtractExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>incrementExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIncrementExpr([NotNull] ElsaParser.IncrementExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>incrementExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIncrementExpr([NotNull] ElsaParser.IncrementExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>objectExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterObjectExpr([NotNull] ElsaParser.ObjectExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>objectExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitObjectExpr([NotNull] ElsaParser.ObjectExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>stringValueExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStringValueExpr([NotNull] ElsaParser.StringValueExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>stringValueExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStringValueExpr([NotNull] ElsaParser.StringValueExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>multiplyExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMultiplyExpr([NotNull] ElsaParser.MultiplyExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>multiplyExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMultiplyExpr([NotNull] ElsaParser.MultiplyExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>parenthesesExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterParenthesesExpr([NotNull] ElsaParser.ParenthesesExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>parenthesesExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitParenthesesExpr([NotNull] ElsaParser.ParenthesesExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>functionExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFunctionExpr([NotNull] ElsaParser.FunctionExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>functionExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFunctionExpr([NotNull] ElsaParser.FunctionExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>decrementExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDecrementExpr([NotNull] ElsaParser.DecrementExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>decrementExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDecrementExpr([NotNull] ElsaParser.DecrementExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>negateExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNegateExpr([NotNull] ElsaParser.NegateExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>negateExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNegateExpr([NotNull] ElsaParser.NegateExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>methodCallExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMethodCallExpr([NotNull] ElsaParser.MethodCallExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>methodCallExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMethodCallExpr([NotNull] ElsaParser.MethodCallExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>variableExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVariableExpr([NotNull] ElsaParser.VariableExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>variableExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVariableExpr([NotNull] ElsaParser.VariableExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>notExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNotExpr([NotNull] ElsaParser.NotExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>notExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNotExpr([NotNull] ElsaParser.NotExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>integerValueExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIntegerValueExpr([NotNull] ElsaParser.IntegerValueExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>integerValueExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIntegerValueExpr([NotNull] ElsaParser.IntegerValueExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>addExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAddExpr([NotNull] ElsaParser.AddExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>addExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAddExpr([NotNull] ElsaParser.AddExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>bracketsExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBracketsExpr([NotNull] ElsaParser.BracketsExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>bracketsExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBracketsExpr([NotNull] ElsaParser.BracketsExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>compareExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCompareExpr([NotNull] ElsaParser.CompareExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>compareExpr</c>
	/// labeled alternative in <see cref="ElsaParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCompareExpr([NotNull] ElsaParser.CompareExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElsaParser.exprList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExprList([NotNull] ElsaParser.ExprListContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElsaParser.exprList"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExprList([NotNull] ElsaParser.ExprListContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
