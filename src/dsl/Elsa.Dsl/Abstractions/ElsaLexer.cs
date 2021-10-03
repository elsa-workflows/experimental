//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.9.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:/Projects/Elsa/experimental/src/dsl/Elsa.Dsl/Dsl\ElsaLexer.g4 by ANTLR 4.9.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.9.1")]
[System.CLSCompliant(false)]
public partial class ElsaLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		EQ=1, GREATER=2, INCREMENT=3, DECREMENT=4, TRIGGER=5, VARIABLE=6, LET=7, 
		IF=8, THEN=9, ELSE=10, FOR=11, RETURN=12, VOID=13, FLOAT=14, INT=15, STRING=16, 
		OBJECT=17, SEQUENCE=18, SYMBOL=19, COLON=20, SEMICOLON=21, COMMA=22, PLUS=23, 
		MINUS=24, STAR=25, EQUALS=26, NOT_EQUALS=27, GREATER_EQUALS=28, LESS=29, 
		LESS_EQUALS=30, PARENTHESES_OPEN=31, PARENTHESES_CLOSE=32, BRACKET_OPEN=33, 
		BRACKET_CLOSE=34, CURLYBRACE_OPEN=35, CURLYBRACE_CLOSE=36, EXCLAMATION=37, 
		PERIOD=38, LANGSPEC_BEGIN=39, STRING_VAL=40, LINE_COMMENT=41, INTEGER_VAL=42, 
		ID=43, WS=44;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"EQ", "GREATER", "INCREMENT", "DECREMENT", "TRIGGER", "VARIABLE", "LET", 
		"IF", "THEN", "ELSE", "FOR", "RETURN", "VOID", "FLOAT", "INT", "STRING", 
		"OBJECT", "SEQUENCE", "SYMBOL", "COLON", "SEMICOLON", "COMMA", "PLUS", 
		"MINUS", "STAR", "EQUALS", "NOT_EQUALS", "GREATER_EQUALS", "LESS", "LESS_EQUALS", 
		"PARENTHESES_OPEN", "PARENTHESES_CLOSE", "BRACKET_OPEN", "BRACKET_CLOSE", 
		"CURLYBRACE_OPEN", "CURLYBRACE_CLOSE", "EXCLAMATION", "PERIOD", "LANGSPEC_BEGIN", 
		"STRING_VAL", "LINE_COMMENT", "INTEGER_VAL", "ID", "WS", "LETTER", "DIGIT"
	};


	public ElsaLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public ElsaLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'='", "'>'", "'++'", "'--'", "'trigger'", "'variable'", "'let'", 
		"'if'", "'then'", "'else'", "'for'", "'return'", "'void'", "'float'", 
		"'int'", "'string'", "'object'", "'Sequence'", null, "':'", "';'", "','", 
		"'+'", "'-'", "'*'", "'=='", "'!='", "'>='", "'<'", "'<='", "'('", "')'", 
		"'['", "']'", "'{'", "'}'", "'!'", "'.'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "EQ", "GREATER", "INCREMENT", "DECREMENT", "TRIGGER", "VARIABLE", 
		"LET", "IF", "THEN", "ELSE", "FOR", "RETURN", "VOID", "FLOAT", "INT", 
		"STRING", "OBJECT", "SEQUENCE", "SYMBOL", "COLON", "SEMICOLON", "COMMA", 
		"PLUS", "MINUS", "STAR", "EQUALS", "NOT_EQUALS", "GREATER_EQUALS", "LESS", 
		"LESS_EQUALS", "PARENTHESES_OPEN", "PARENTHESES_CLOSE", "BRACKET_OPEN", 
		"BRACKET_CLOSE", "CURLYBRACE_OPEN", "CURLYBRACE_CLOSE", "EXCLAMATION", 
		"PERIOD", "LANGSPEC_BEGIN", "STRING_VAL", "LINE_COMMENT", "INTEGER_VAL", 
		"ID", "WS"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "ElsaLexer.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override string SerializedAtn { get { return new string(_serializedATN); } }

	static ElsaLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static char[] _serializedATN = {
		'\x3', '\x608B', '\xA72A', '\x8133', '\xB9ED', '\x417C', '\x3BE7', '\x7786', 
		'\x5964', '\x2', '.', '\x127', '\b', '\x1', '\x4', '\x2', '\t', '\x2', 
		'\x4', '\x3', '\t', '\x3', '\x4', '\x4', '\t', '\x4', '\x4', '\x5', '\t', 
		'\x5', '\x4', '\x6', '\t', '\x6', '\x4', '\a', '\t', '\a', '\x4', '\b', 
		'\t', '\b', '\x4', '\t', '\t', '\t', '\x4', '\n', '\t', '\n', '\x4', '\v', 
		'\t', '\v', '\x4', '\f', '\t', '\f', '\x4', '\r', '\t', '\r', '\x4', '\xE', 
		'\t', '\xE', '\x4', '\xF', '\t', '\xF', '\x4', '\x10', '\t', '\x10', '\x4', 
		'\x11', '\t', '\x11', '\x4', '\x12', '\t', '\x12', '\x4', '\x13', '\t', 
		'\x13', '\x4', '\x14', '\t', '\x14', '\x4', '\x15', '\t', '\x15', '\x4', 
		'\x16', '\t', '\x16', '\x4', '\x17', '\t', '\x17', '\x4', '\x18', '\t', 
		'\x18', '\x4', '\x19', '\t', '\x19', '\x4', '\x1A', '\t', '\x1A', '\x4', 
		'\x1B', '\t', '\x1B', '\x4', '\x1C', '\t', '\x1C', '\x4', '\x1D', '\t', 
		'\x1D', '\x4', '\x1E', '\t', '\x1E', '\x4', '\x1F', '\t', '\x1F', '\x4', 
		' ', '\t', ' ', '\x4', '!', '\t', '!', '\x4', '\"', '\t', '\"', '\x4', 
		'#', '\t', '#', '\x4', '$', '\t', '$', '\x4', '%', '\t', '%', '\x4', '&', 
		'\t', '&', '\x4', '\'', '\t', '\'', '\x4', '(', '\t', '(', '\x4', ')', 
		'\t', ')', '\x4', '*', '\t', '*', '\x4', '+', '\t', '+', '\x4', ',', '\t', 
		',', '\x4', '-', '\t', '-', '\x4', '.', '\t', '.', '\x4', '/', '\t', '/', 
		'\x3', '\x2', '\x3', '\x2', '\x3', '\x3', '\x3', '\x3', '\x3', '\x4', 
		'\x3', '\x4', '\x3', '\x4', '\x3', '\x5', '\x3', '\x5', '\x3', '\x5', 
		'\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', 
		'\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\a', '\x3', '\a', '\x3', 
		'\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', 
		'\x3', '\a', '\x3', '\b', '\x3', '\b', '\x3', '\b', '\x3', '\b', '\x3', 
		'\t', '\x3', '\t', '\x3', '\t', '\x3', '\n', '\x3', '\n', '\x3', '\n', 
		'\x3', '\n', '\x3', '\n', '\x3', '\v', '\x3', '\v', '\x3', '\v', '\x3', 
		'\v', '\x3', '\v', '\x3', '\f', '\x3', '\f', '\x3', '\f', '\x3', '\f', 
		'\x3', '\r', '\x3', '\r', '\x3', '\r', '\x3', '\r', '\x3', '\r', '\x3', 
		'\r', '\x3', '\r', '\x3', '\xE', '\x3', '\xE', '\x3', '\xE', '\x3', '\xE', 
		'\x3', '\xE', '\x3', '\xF', '\x3', '\xF', '\x3', '\xF', '\x3', '\xF', 
		'\x3', '\xF', '\x3', '\xF', '\x3', '\x10', '\x3', '\x10', '\x3', '\x10', 
		'\x3', '\x10', '\x3', '\x11', '\x3', '\x11', '\x3', '\x11', '\x3', '\x11', 
		'\x3', '\x11', '\x3', '\x11', '\x3', '\x11', '\x3', '\x12', '\x3', '\x12', 
		'\x3', '\x12', '\x3', '\x12', '\x3', '\x12', '\x3', '\x12', '\x3', '\x12', 
		'\x3', '\x13', '\x3', '\x13', '\x3', '\x13', '\x3', '\x13', '\x3', '\x13', 
		'\x3', '\x13', '\x3', '\x13', '\x3', '\x13', '\x3', '\x13', '\x3', '\x14', 
		'\x3', '\x14', '\x3', '\x15', '\x3', '\x15', '\x3', '\x16', '\x3', '\x16', 
		'\x3', '\x17', '\x3', '\x17', '\x3', '\x18', '\x3', '\x18', '\x3', '\x19', 
		'\x3', '\x19', '\x3', '\x1A', '\x3', '\x1A', '\x3', '\x1B', '\x3', '\x1B', 
		'\x3', '\x1B', '\x3', '\x1C', '\x3', '\x1C', '\x3', '\x1C', '\x3', '\x1D', 
		'\x3', '\x1D', '\x3', '\x1D', '\x3', '\x1E', '\x3', '\x1E', '\x3', '\x1F', 
		'\x3', '\x1F', '\x3', '\x1F', '\x3', ' ', '\x3', ' ', '\x3', '!', '\x3', 
		'!', '\x3', '\"', '\x3', '\"', '\x3', '#', '\x3', '#', '\x3', '$', '\x3', 
		'$', '\x3', '%', '\x3', '%', '\x3', '&', '\x3', '&', '\x3', '\'', '\x3', 
		'\'', '\x3', '(', '\x3', '(', '\x6', '(', '\xEB', '\n', '(', '\r', '(', 
		'\xE', '(', '\xEC', '\x3', '(', '\x3', '(', '\a', '(', '\xF1', '\n', '(', 
		'\f', '(', '\xE', '(', '\xF4', '\v', '(', '\x3', '(', '\x3', '(', '\x3', 
		')', '\x3', ')', '\x3', ')', '\x3', ')', '\a', ')', '\xFC', '\n', ')', 
		'\f', ')', '\xE', ')', '\xFF', '\v', ')', '\x3', ')', '\x3', ')', '\x3', 
		'*', '\x3', '*', '\x3', '*', '\x3', '*', '\a', '*', '\x107', '\n', '*', 
		'\f', '*', '\xE', '*', '\x10A', '\v', '*', '\x3', '*', '\x5', '*', '\x10D', 
		'\n', '*', '\x3', '*', '\x3', '*', '\x3', '*', '\x3', '*', '\x3', '+', 
		'\x6', '+', '\x114', '\n', '+', '\r', '+', '\xE', '+', '\x115', '\x3', 
		',', '\x3', ',', '\x3', ',', '\a', ',', '\x11B', '\n', ',', '\f', ',', 
		'\xE', ',', '\x11E', '\v', ',', '\x3', '-', '\x3', '-', '\x3', '-', '\x3', 
		'-', '\x3', '.', '\x3', '.', '\x3', '/', '\x3', '/', '\x5', '\xF2', '\xFD', 
		'\x108', '\x2', '\x30', '\x3', '\x3', '\x5', '\x4', '\a', '\x5', '\t', 
		'\x6', '\v', '\a', '\r', '\b', '\xF', '\t', '\x11', '\n', '\x13', '\v', 
		'\x15', '\f', '\x17', '\r', '\x19', '\xE', '\x1B', '\xF', '\x1D', '\x10', 
		'\x1F', '\x11', '!', '\x12', '#', '\x13', '%', '\x14', '\'', '\x15', ')', 
		'\x16', '+', '\x17', '-', '\x18', '/', '\x19', '\x31', '\x1A', '\x33', 
		'\x1B', '\x35', '\x1C', '\x37', '\x1D', '\x39', '\x1E', ';', '\x1F', '=', 
		' ', '?', '!', '\x41', '\"', '\x43', '#', '\x45', '$', 'G', '%', 'I', 
		'&', 'K', '\'', 'M', '(', 'O', ')', 'Q', '*', 'S', '+', 'U', ',', 'W', 
		'-', 'Y', '.', '[', '\x2', ']', '\x2', '\x3', '\x2', '\a', '\x6', '\x2', 
		'&', '&', '-', '-', '?', '@', '\x62', '\x62', '\x3', '\x2', '\x31', '\x31', 
		'\x5', '\x2', '\v', '\f', '\xF', '\xF', '\"', '\"', '\x5', '\x2', '\x43', 
		'\\', '\x61', '\x61', '\x63', '|', '\x3', '\x2', '\x32', ';', '\x2', '\x12D', 
		'\x2', '\x3', '\x3', '\x2', '\x2', '\x2', '\x2', '\x5', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\a', '\x3', '\x2', '\x2', '\x2', '\x2', '\t', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\v', '\x3', '\x2', '\x2', '\x2', '\x2', '\r', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\xF', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '\x11', '\x3', '\x2', '\x2', '\x2', '\x2', '\x13', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\x15', '\x3', '\x2', '\x2', '\x2', '\x2', '\x17', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\x19', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '\x1B', '\x3', '\x2', '\x2', '\x2', '\x2', '\x1D', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\x1F', '\x3', '\x2', '\x2', '\x2', '\x2', '!', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '#', '\x3', '\x2', '\x2', '\x2', '\x2', '%', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\'', '\x3', '\x2', '\x2', '\x2', '\x2', 
		')', '\x3', '\x2', '\x2', '\x2', '\x2', '+', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '-', '\x3', '\x2', '\x2', '\x2', '\x2', '/', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x31', '\x3', '\x2', '\x2', '\x2', '\x2', '\x33', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x35', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x37', '\x3', '\x2', '\x2', '\x2', '\x2', '\x39', '\x3', '\x2', '\x2', 
		'\x2', '\x2', ';', '\x3', '\x2', '\x2', '\x2', '\x2', '=', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '?', '\x3', '\x2', '\x2', '\x2', '\x2', '\x41', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x43', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x45', '\x3', '\x2', '\x2', '\x2', '\x2', 'G', '\x3', '\x2', '\x2', '\x2', 
		'\x2', 'I', '\x3', '\x2', '\x2', '\x2', '\x2', 'K', '\x3', '\x2', '\x2', 
		'\x2', '\x2', 'M', '\x3', '\x2', '\x2', '\x2', '\x2', 'O', '\x3', '\x2', 
		'\x2', '\x2', '\x2', 'Q', '\x3', '\x2', '\x2', '\x2', '\x2', 'S', '\x3', 
		'\x2', '\x2', '\x2', '\x2', 'U', '\x3', '\x2', '\x2', '\x2', '\x2', 'W', 
		'\x3', '\x2', '\x2', '\x2', '\x2', 'Y', '\x3', '\x2', '\x2', '\x2', '\x3', 
		'_', '\x3', '\x2', '\x2', '\x2', '\x5', '\x61', '\x3', '\x2', '\x2', '\x2', 
		'\a', '\x63', '\x3', '\x2', '\x2', '\x2', '\t', '\x66', '\x3', '\x2', 
		'\x2', '\x2', '\v', 'i', '\x3', '\x2', '\x2', '\x2', '\r', 'q', '\x3', 
		'\x2', '\x2', '\x2', '\xF', 'z', '\x3', '\x2', '\x2', '\x2', '\x11', '~', 
		'\x3', '\x2', '\x2', '\x2', '\x13', '\x81', '\x3', '\x2', '\x2', '\x2', 
		'\x15', '\x86', '\x3', '\x2', '\x2', '\x2', '\x17', '\x8B', '\x3', '\x2', 
		'\x2', '\x2', '\x19', '\x8F', '\x3', '\x2', '\x2', '\x2', '\x1B', '\x96', 
		'\x3', '\x2', '\x2', '\x2', '\x1D', '\x9B', '\x3', '\x2', '\x2', '\x2', 
		'\x1F', '\xA1', '\x3', '\x2', '\x2', '\x2', '!', '\xA5', '\x3', '\x2', 
		'\x2', '\x2', '#', '\xAC', '\x3', '\x2', '\x2', '\x2', '%', '\xB3', '\x3', 
		'\x2', '\x2', '\x2', '\'', '\xBC', '\x3', '\x2', '\x2', '\x2', ')', '\xBE', 
		'\x3', '\x2', '\x2', '\x2', '+', '\xC0', '\x3', '\x2', '\x2', '\x2', '-', 
		'\xC2', '\x3', '\x2', '\x2', '\x2', '/', '\xC4', '\x3', '\x2', '\x2', 
		'\x2', '\x31', '\xC6', '\x3', '\x2', '\x2', '\x2', '\x33', '\xC8', '\x3', 
		'\x2', '\x2', '\x2', '\x35', '\xCA', '\x3', '\x2', '\x2', '\x2', '\x37', 
		'\xCD', '\x3', '\x2', '\x2', '\x2', '\x39', '\xD0', '\x3', '\x2', '\x2', 
		'\x2', ';', '\xD3', '\x3', '\x2', '\x2', '\x2', '=', '\xD5', '\x3', '\x2', 
		'\x2', '\x2', '?', '\xD8', '\x3', '\x2', '\x2', '\x2', '\x41', '\xDA', 
		'\x3', '\x2', '\x2', '\x2', '\x43', '\xDC', '\x3', '\x2', '\x2', '\x2', 
		'\x45', '\xDE', '\x3', '\x2', '\x2', '\x2', 'G', '\xE0', '\x3', '\x2', 
		'\x2', '\x2', 'I', '\xE2', '\x3', '\x2', '\x2', '\x2', 'K', '\xE4', '\x3', 
		'\x2', '\x2', '\x2', 'M', '\xE6', '\x3', '\x2', '\x2', '\x2', 'O', '\xE8', 
		'\x3', '\x2', '\x2', '\x2', 'Q', '\xF7', '\x3', '\x2', '\x2', '\x2', 'S', 
		'\x102', '\x3', '\x2', '\x2', '\x2', 'U', '\x113', '\x3', '\x2', '\x2', 
		'\x2', 'W', '\x117', '\x3', '\x2', '\x2', '\x2', 'Y', '\x11F', '\x3', 
		'\x2', '\x2', '\x2', '[', '\x123', '\x3', '\x2', '\x2', '\x2', ']', '\x125', 
		'\x3', '\x2', '\x2', '\x2', '_', '`', '\a', '?', '\x2', '\x2', '`', '\x4', 
		'\x3', '\x2', '\x2', '\x2', '\x61', '\x62', '\a', '@', '\x2', '\x2', '\x62', 
		'\x6', '\x3', '\x2', '\x2', '\x2', '\x63', '\x64', '\a', '-', '\x2', '\x2', 
		'\x64', '\x65', '\a', '-', '\x2', '\x2', '\x65', '\b', '\x3', '\x2', '\x2', 
		'\x2', '\x66', 'g', '\a', '/', '\x2', '\x2', 'g', 'h', '\a', '/', '\x2', 
		'\x2', 'h', '\n', '\x3', '\x2', '\x2', '\x2', 'i', 'j', '\a', 'v', '\x2', 
		'\x2', 'j', 'k', '\a', 't', '\x2', '\x2', 'k', 'l', '\a', 'k', '\x2', 
		'\x2', 'l', 'm', '\a', 'i', '\x2', '\x2', 'm', 'n', '\a', 'i', '\x2', 
		'\x2', 'n', 'o', '\a', 'g', '\x2', '\x2', 'o', 'p', '\a', 't', '\x2', 
		'\x2', 'p', '\f', '\x3', '\x2', '\x2', '\x2', 'q', 'r', '\a', 'x', '\x2', 
		'\x2', 'r', 's', '\a', '\x63', '\x2', '\x2', 's', 't', '\a', 't', '\x2', 
		'\x2', 't', 'u', '\a', 'k', '\x2', '\x2', 'u', 'v', '\a', '\x63', '\x2', 
		'\x2', 'v', 'w', '\a', '\x64', '\x2', '\x2', 'w', 'x', '\a', 'n', '\x2', 
		'\x2', 'x', 'y', '\a', 'g', '\x2', '\x2', 'y', '\xE', '\x3', '\x2', '\x2', 
		'\x2', 'z', '{', '\a', 'n', '\x2', '\x2', '{', '|', '\a', 'g', '\x2', 
		'\x2', '|', '}', '\a', 'v', '\x2', '\x2', '}', '\x10', '\x3', '\x2', '\x2', 
		'\x2', '~', '\x7F', '\a', 'k', '\x2', '\x2', '\x7F', '\x80', '\a', 'h', 
		'\x2', '\x2', '\x80', '\x12', '\x3', '\x2', '\x2', '\x2', '\x81', '\x82', 
		'\a', 'v', '\x2', '\x2', '\x82', '\x83', '\a', 'j', '\x2', '\x2', '\x83', 
		'\x84', '\a', 'g', '\x2', '\x2', '\x84', '\x85', '\a', 'p', '\x2', '\x2', 
		'\x85', '\x14', '\x3', '\x2', '\x2', '\x2', '\x86', '\x87', '\a', 'g', 
		'\x2', '\x2', '\x87', '\x88', '\a', 'n', '\x2', '\x2', '\x88', '\x89', 
		'\a', 'u', '\x2', '\x2', '\x89', '\x8A', '\a', 'g', '\x2', '\x2', '\x8A', 
		'\x16', '\x3', '\x2', '\x2', '\x2', '\x8B', '\x8C', '\a', 'h', '\x2', 
		'\x2', '\x8C', '\x8D', '\a', 'q', '\x2', '\x2', '\x8D', '\x8E', '\a', 
		't', '\x2', '\x2', '\x8E', '\x18', '\x3', '\x2', '\x2', '\x2', '\x8F', 
		'\x90', '\a', 't', '\x2', '\x2', '\x90', '\x91', '\a', 'g', '\x2', '\x2', 
		'\x91', '\x92', '\a', 'v', '\x2', '\x2', '\x92', '\x93', '\a', 'w', '\x2', 
		'\x2', '\x93', '\x94', '\a', 't', '\x2', '\x2', '\x94', '\x95', '\a', 
		'p', '\x2', '\x2', '\x95', '\x1A', '\x3', '\x2', '\x2', '\x2', '\x96', 
		'\x97', '\a', 'x', '\x2', '\x2', '\x97', '\x98', '\a', 'q', '\x2', '\x2', 
		'\x98', '\x99', '\a', 'k', '\x2', '\x2', '\x99', '\x9A', '\a', '\x66', 
		'\x2', '\x2', '\x9A', '\x1C', '\x3', '\x2', '\x2', '\x2', '\x9B', '\x9C', 
		'\a', 'h', '\x2', '\x2', '\x9C', '\x9D', '\a', 'n', '\x2', '\x2', '\x9D', 
		'\x9E', '\a', 'q', '\x2', '\x2', '\x9E', '\x9F', '\a', '\x63', '\x2', 
		'\x2', '\x9F', '\xA0', '\a', 'v', '\x2', '\x2', '\xA0', '\x1E', '\x3', 
		'\x2', '\x2', '\x2', '\xA1', '\xA2', '\a', 'k', '\x2', '\x2', '\xA2', 
		'\xA3', '\a', 'p', '\x2', '\x2', '\xA3', '\xA4', '\a', 'v', '\x2', '\x2', 
		'\xA4', ' ', '\x3', '\x2', '\x2', '\x2', '\xA5', '\xA6', '\a', 'u', '\x2', 
		'\x2', '\xA6', '\xA7', '\a', 'v', '\x2', '\x2', '\xA7', '\xA8', '\a', 
		't', '\x2', '\x2', '\xA8', '\xA9', '\a', 'k', '\x2', '\x2', '\xA9', '\xAA', 
		'\a', 'p', '\x2', '\x2', '\xAA', '\xAB', '\a', 'i', '\x2', '\x2', '\xAB', 
		'\"', '\x3', '\x2', '\x2', '\x2', '\xAC', '\xAD', '\a', 'q', '\x2', '\x2', 
		'\xAD', '\xAE', '\a', '\x64', '\x2', '\x2', '\xAE', '\xAF', '\a', 'l', 
		'\x2', '\x2', '\xAF', '\xB0', '\a', 'g', '\x2', '\x2', '\xB0', '\xB1', 
		'\a', '\x65', '\x2', '\x2', '\xB1', '\xB2', '\a', 'v', '\x2', '\x2', '\xB2', 
		'$', '\x3', '\x2', '\x2', '\x2', '\xB3', '\xB4', '\a', 'U', '\x2', '\x2', 
		'\xB4', '\xB5', '\a', 'g', '\x2', '\x2', '\xB5', '\xB6', '\a', 's', '\x2', 
		'\x2', '\xB6', '\xB7', '\a', 'w', '\x2', '\x2', '\xB7', '\xB8', '\a', 
		'g', '\x2', '\x2', '\xB8', '\xB9', '\a', 'p', '\x2', '\x2', '\xB9', '\xBA', 
		'\a', '\x65', '\x2', '\x2', '\xBA', '\xBB', '\a', 'g', '\x2', '\x2', '\xBB', 
		'&', '\x3', '\x2', '\x2', '\x2', '\xBC', '\xBD', '\t', '\x2', '\x2', '\x2', 
		'\xBD', '(', '\x3', '\x2', '\x2', '\x2', '\xBE', '\xBF', '\a', '<', '\x2', 
		'\x2', '\xBF', '*', '\x3', '\x2', '\x2', '\x2', '\xC0', '\xC1', '\a', 
		'=', '\x2', '\x2', '\xC1', ',', '\x3', '\x2', '\x2', '\x2', '\xC2', '\xC3', 
		'\a', '.', '\x2', '\x2', '\xC3', '.', '\x3', '\x2', '\x2', '\x2', '\xC4', 
		'\xC5', '\a', '-', '\x2', '\x2', '\xC5', '\x30', '\x3', '\x2', '\x2', 
		'\x2', '\xC6', '\xC7', '\a', '/', '\x2', '\x2', '\xC7', '\x32', '\x3', 
		'\x2', '\x2', '\x2', '\xC8', '\xC9', '\a', ',', '\x2', '\x2', '\xC9', 
		'\x34', '\x3', '\x2', '\x2', '\x2', '\xCA', '\xCB', '\a', '?', '\x2', 
		'\x2', '\xCB', '\xCC', '\a', '?', '\x2', '\x2', '\xCC', '\x36', '\x3', 
		'\x2', '\x2', '\x2', '\xCD', '\xCE', '\a', '#', '\x2', '\x2', '\xCE', 
		'\xCF', '\a', '?', '\x2', '\x2', '\xCF', '\x38', '\x3', '\x2', '\x2', 
		'\x2', '\xD0', '\xD1', '\a', '@', '\x2', '\x2', '\xD1', '\xD2', '\a', 
		'?', '\x2', '\x2', '\xD2', ':', '\x3', '\x2', '\x2', '\x2', '\xD3', '\xD4', 
		'\a', '>', '\x2', '\x2', '\xD4', '<', '\x3', '\x2', '\x2', '\x2', '\xD5', 
		'\xD6', '\a', '>', '\x2', '\x2', '\xD6', '\xD7', '\a', '?', '\x2', '\x2', 
		'\xD7', '>', '\x3', '\x2', '\x2', '\x2', '\xD8', '\xD9', '\a', '*', '\x2', 
		'\x2', '\xD9', '@', '\x3', '\x2', '\x2', '\x2', '\xDA', '\xDB', '\a', 
		'+', '\x2', '\x2', '\xDB', '\x42', '\x3', '\x2', '\x2', '\x2', '\xDC', 
		'\xDD', '\a', ']', '\x2', '\x2', '\xDD', '\x44', '\x3', '\x2', '\x2', 
		'\x2', '\xDE', '\xDF', '\a', '_', '\x2', '\x2', '\xDF', '\x46', '\x3', 
		'\x2', '\x2', '\x2', '\xE0', '\xE1', '\a', '}', '\x2', '\x2', '\xE1', 
		'H', '\x3', '\x2', '\x2', '\x2', '\xE2', '\xE3', '\a', '\x7F', '\x2', 
		'\x2', '\xE3', 'J', '\x3', '\x2', '\x2', '\x2', '\xE4', '\xE5', '\a', 
		'#', '\x2', '\x2', '\xE5', 'L', '\x3', '\x2', '\x2', '\x2', '\xE6', '\xE7', 
		'\a', '\x30', '\x2', '\x2', '\xE7', 'N', '\x3', '\x2', '\x2', '\x2', '\xE8', 
		'\xEA', '\a', '\x31', '\x2', '\x2', '\xE9', '\xEB', '\n', '\x3', '\x2', 
		'\x2', '\xEA', '\xE9', '\x3', '\x2', '\x2', '\x2', '\xEB', '\xEC', '\x3', 
		'\x2', '\x2', '\x2', '\xEC', '\xEA', '\x3', '\x2', '\x2', '\x2', '\xEC', 
		'\xED', '\x3', '\x2', '\x2', '\x2', '\xED', '\xEE', '\x3', '\x2', '\x2', 
		'\x2', '\xEE', '\xF2', '\a', '\x31', '\x2', '\x2', '\xEF', '\xF1', '\v', 
		'\x2', '\x2', '\x2', '\xF0', '\xEF', '\x3', '\x2', '\x2', '\x2', '\xF1', 
		'\xF4', '\x3', '\x2', '\x2', '\x2', '\xF2', '\xF3', '\x3', '\x2', '\x2', 
		'\x2', '\xF2', '\xF0', '\x3', '\x2', '\x2', '\x2', '\xF3', '\xF5', '\x3', 
		'\x2', '\x2', '\x2', '\xF4', '\xF2', '\x3', '\x2', '\x2', '\x2', '\xF5', 
		'\xF6', '\a', '\x31', '\x2', '\x2', '\xF6', 'P', '\x3', '\x2', '\x2', 
		'\x2', '\xF7', '\xFD', '\a', '$', '\x2', '\x2', '\xF8', '\xF9', '\a', 
		'^', '\x2', '\x2', '\xF9', '\xFC', '\a', '$', '\x2', '\x2', '\xFA', '\xFC', 
		'\v', '\x2', '\x2', '\x2', '\xFB', '\xF8', '\x3', '\x2', '\x2', '\x2', 
		'\xFB', '\xFA', '\x3', '\x2', '\x2', '\x2', '\xFC', '\xFF', '\x3', '\x2', 
		'\x2', '\x2', '\xFD', '\xFE', '\x3', '\x2', '\x2', '\x2', '\xFD', '\xFB', 
		'\x3', '\x2', '\x2', '\x2', '\xFE', '\x100', '\x3', '\x2', '\x2', '\x2', 
		'\xFF', '\xFD', '\x3', '\x2', '\x2', '\x2', '\x100', '\x101', '\a', '$', 
		'\x2', '\x2', '\x101', 'R', '\x3', '\x2', '\x2', '\x2', '\x102', '\x103', 
		'\a', '\x31', '\x2', '\x2', '\x103', '\x104', '\a', '\x31', '\x2', '\x2', 
		'\x104', '\x108', '\x3', '\x2', '\x2', '\x2', '\x105', '\x107', '\v', 
		'\x2', '\x2', '\x2', '\x106', '\x105', '\x3', '\x2', '\x2', '\x2', '\x107', 
		'\x10A', '\x3', '\x2', '\x2', '\x2', '\x108', '\x109', '\x3', '\x2', '\x2', 
		'\x2', '\x108', '\x106', '\x3', '\x2', '\x2', '\x2', '\x109', '\x10C', 
		'\x3', '\x2', '\x2', '\x2', '\x10A', '\x108', '\x3', '\x2', '\x2', '\x2', 
		'\x10B', '\x10D', '\a', '\xF', '\x2', '\x2', '\x10C', '\x10B', '\x3', 
		'\x2', '\x2', '\x2', '\x10C', '\x10D', '\x3', '\x2', '\x2', '\x2', '\x10D', 
		'\x10E', '\x3', '\x2', '\x2', '\x2', '\x10E', '\x10F', '\a', '\f', '\x2', 
		'\x2', '\x10F', '\x110', '\x3', '\x2', '\x2', '\x2', '\x110', '\x111', 
		'\b', '*', '\x2', '\x2', '\x111', 'T', '\x3', '\x2', '\x2', '\x2', '\x112', 
		'\x114', '\x5', ']', '/', '\x2', '\x113', '\x112', '\x3', '\x2', '\x2', 
		'\x2', '\x114', '\x115', '\x3', '\x2', '\x2', '\x2', '\x115', '\x113', 
		'\x3', '\x2', '\x2', '\x2', '\x115', '\x116', '\x3', '\x2', '\x2', '\x2', 
		'\x116', 'V', '\x3', '\x2', '\x2', '\x2', '\x117', '\x11C', '\x5', '[', 
		'.', '\x2', '\x118', '\x11B', '\x5', '[', '.', '\x2', '\x119', '\x11B', 
		'\x5', ']', '/', '\x2', '\x11A', '\x118', '\x3', '\x2', '\x2', '\x2', 
		'\x11A', '\x119', '\x3', '\x2', '\x2', '\x2', '\x11B', '\x11E', '\x3', 
		'\x2', '\x2', '\x2', '\x11C', '\x11A', '\x3', '\x2', '\x2', '\x2', '\x11C', 
		'\x11D', '\x3', '\x2', '\x2', '\x2', '\x11D', 'X', '\x3', '\x2', '\x2', 
		'\x2', '\x11E', '\x11C', '\x3', '\x2', '\x2', '\x2', '\x11F', '\x120', 
		'\t', '\x4', '\x2', '\x2', '\x120', '\x121', '\x3', '\x2', '\x2', '\x2', 
		'\x121', '\x122', '\b', '-', '\x2', '\x2', '\x122', 'Z', '\x3', '\x2', 
		'\x2', '\x2', '\x123', '\x124', '\t', '\x5', '\x2', '\x2', '\x124', '\\', 
		'\x3', '\x2', '\x2', '\x2', '\x125', '\x126', '\t', '\x6', '\x2', '\x2', 
		'\x126', '^', '\x3', '\x2', '\x2', '\x2', '\f', '\x2', '\xEC', '\xF2', 
		'\xFB', '\xFD', '\x108', '\x10C', '\x115', '\x11A', '\x11C', '\x3', '\b', 
		'\x2', '\x2',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
