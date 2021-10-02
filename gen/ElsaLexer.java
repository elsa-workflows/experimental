// Generated from C:/Projects/Elsa/experimental/src/dsl/Elsa.Dsl/Dsl\ElsaLexer.g4 by ANTLR 4.9.1
import org.antlr.v4.runtime.Lexer;
import org.antlr.v4.runtime.CharStream;
import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.TokenStream;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.misc.*;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class ElsaLexer extends Lexer {
	static { RuntimeMetaData.checkVersion("4.9.1", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		EQ=1, GREATER=2, TRIGGER=3, VARIABLE=4, LET=5, IF=6, THEN=7, ELSE=8, FOR=9, 
		RETURN=10, VOID=11, FLOAT=12, INT=13, STRING=14, OBJECT=15, SEQUENCE=16, 
		ID=17, SYMBOL=18, INTEGER_VAL=19, COLON=20, SEMICOLON=21, COMMA=22, PLUS=23, 
		MINUS=24, STAR=25, EQUALS=26, NOT_EQUALS=27, GREATER_EQUALS=28, LESS=29, 
		LESS_EQUALS=30, DECREMENT=31, INCREMENT=32, PARENTHESES_OPEN=33, PARENTHESES_CLOSE=34, 
		BRACKET_OPEN=35, BRACKET_CLOSE=36, CURLYBRACE_OPEN=37, CURLYBRACE_CLOSE=38, 
		EXCLAMATION=39, LANGSPEC_BEGIN=40, STRING_VAL=41, LINE_COMMENT=42, TEXT=43, 
		CODE_VAL=44, WS=45;
	public static String[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static String[] modeNames = {
		"DEFAULT_MODE"
	};

	private static String[] makeRuleNames() {
		return new String[] {
			"EQ", "GREATER", "TRIGGER", "VARIABLE", "LET", "IF", "THEN", "ELSE", 
			"FOR", "RETURN", "VOID", "FLOAT", "INT", "STRING", "OBJECT", "SEQUENCE", 
			"ID", "SYMBOL", "INTEGER_VAL", "COLON", "SEMICOLON", "COMMA", "PLUS", 
			"MINUS", "STAR", "EQUALS", "NOT_EQUALS", "GREATER_EQUALS", "LESS", "LESS_EQUALS", 
			"DECREMENT", "INCREMENT", "PARENTHESES_OPEN", "PARENTHESES_CLOSE", "BRACKET_OPEN", 
			"BRACKET_CLOSE", "CURLYBRACE_OPEN", "CURLYBRACE_CLOSE", "EXCLAMATION", 
			"LANGSPEC_BEGIN", "STRING_VAL", "LINE_COMMENT", "TEXT", "CODE_VAL", "WS", 
			"LETTER", "DIGIT"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, "'='", "'>'", "'trigger'", "'variable'", "'let'", "'if'", "'then'", 
			"'else'", "'for'", "'return'", "'void'", "'float'", "'int'", "'string'", 
			"'object'", "'Sequence'", null, null, null, "':'", "';'", "','", "'+'", 
			"'-'", "'*'", "'=='", "'!='", "'>='", "'<'", "'<='", "'--'", "'++'", 
			"'('", "')'", "'['", "']'", "'{'", "'}'", "'!'"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, "EQ", "GREATER", "TRIGGER", "VARIABLE", "LET", "IF", "THEN", "ELSE", 
			"FOR", "RETURN", "VOID", "FLOAT", "INT", "STRING", "OBJECT", "SEQUENCE", 
			"ID", "SYMBOL", "INTEGER_VAL", "COLON", "SEMICOLON", "COMMA", "PLUS", 
			"MINUS", "STAR", "EQUALS", "NOT_EQUALS", "GREATER_EQUALS", "LESS", "LESS_EQUALS", 
			"DECREMENT", "INCREMENT", "PARENTHESES_OPEN", "PARENTHESES_CLOSE", "BRACKET_OPEN", 
			"BRACKET_CLOSE", "CURLYBRACE_OPEN", "CURLYBRACE_CLOSE", "EXCLAMATION", 
			"LANGSPEC_BEGIN", "STRING_VAL", "LINE_COMMENT", "TEXT", "CODE_VAL", "WS"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}


	public ElsaLexer(CharStream input) {
		super(input);
		_interp = new LexerATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@Override
	public String getGrammarFileName() { return "ElsaLexer.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public String[] getChannelNames() { return channelNames; }

	@Override
	public String[] getModeNames() { return modeNames; }

	@Override
	public ATN getATN() { return _ATN; }

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\2/\u0139\b\1\4\2\t"+
		"\2\4\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4\13"+
		"\t\13\4\f\t\f\4\r\t\r\4\16\t\16\4\17\t\17\4\20\t\20\4\21\t\21\4\22\t\22"+
		"\4\23\t\23\4\24\t\24\4\25\t\25\4\26\t\26\4\27\t\27\4\30\t\30\4\31\t\31"+
		"\4\32\t\32\4\33\t\33\4\34\t\34\4\35\t\35\4\36\t\36\4\37\t\37\4 \t \4!"+
		"\t!\4\"\t\"\4#\t#\4$\t$\4%\t%\4&\t&\4\'\t\'\4(\t(\4)\t)\4*\t*\4+\t+\4"+
		",\t,\4-\t-\4.\t.\4/\t/\4\60\t\60\3\2\3\2\3\3\3\3\3\4\3\4\3\4\3\4\3\4\3"+
		"\4\3\4\3\4\3\5\3\5\3\5\3\5\3\5\3\5\3\5\3\5\3\5\3\6\3\6\3\6\3\6\3\7\3\7"+
		"\3\7\3\b\3\b\3\b\3\b\3\b\3\t\3\t\3\t\3\t\3\t\3\n\3\n\3\n\3\n\3\13\3\13"+
		"\3\13\3\13\3\13\3\13\3\13\3\f\3\f\3\f\3\f\3\f\3\r\3\r\3\r\3\r\3\r\3\r"+
		"\3\16\3\16\3\16\3\16\3\17\3\17\3\17\3\17\3\17\3\17\3\17\3\20\3\20\3\20"+
		"\3\20\3\20\3\20\3\20\3\21\3\21\3\21\3\21\3\21\3\21\3\21\3\21\3\21\3\22"+
		"\3\22\3\22\7\22\u00bc\n\22\f\22\16\22\u00bf\13\22\3\23\3\23\3\24\6\24"+
		"\u00c4\n\24\r\24\16\24\u00c5\3\25\3\25\3\26\3\26\3\27\3\27\3\30\3\30\3"+
		"\31\3\31\3\32\3\32\3\33\3\33\3\33\3\34\3\34\3\34\3\35\3\35\3\35\3\36\3"+
		"\36\3\37\3\37\3\37\3 \3 \3 \3!\3!\3!\3\"\3\"\3#\3#\3$\3$\3%\3%\3&\3&\3"+
		"\'\3\'\3(\3(\3)\3)\6)\u00f8\n)\r)\16)\u00f9\3)\3)\7)\u00fe\n)\f)\16)\u0101"+
		"\13)\3)\3)\3*\3*\3*\3*\7*\u0109\n*\f*\16*\u010c\13*\3*\3*\3+\3+\3+\3+"+
		"\7+\u0114\n+\f+\16+\u0117\13+\3+\5+\u011a\n+\3+\3+\3+\3+\3,\3,\6,\u0122"+
		"\n,\r,\16,\u0123\3-\3-\3-\7-\u0129\n-\f-\16-\u012c\13-\3-\3-\3-\3-\3."+
		"\3.\3.\3.\3/\3/\3\60\3\60\6\u00ff\u010a\u0115\u012a\2\61\3\3\5\4\7\5\t"+
		"\6\13\7\r\b\17\t\21\n\23\13\25\f\27\r\31\16\33\17\35\20\37\21!\22#\23"+
		"%\24\'\25)\26+\27-\30/\31\61\32\63\33\65\34\67\359\36;\37= ?!A\"C#E$G"+
		"%I&K\'M(O)Q*S+U,W-Y.[/]\2_\2\3\2\7\6\2&&--?@bb\3\2\61\61\5\2\13\f\17\17"+
		"\"\"\5\2C\\aac|\3\2\62;\2\u0143\2\3\3\2\2\2\2\5\3\2\2\2\2\7\3\2\2\2\2"+
		"\t\3\2\2\2\2\13\3\2\2\2\2\r\3\2\2\2\2\17\3\2\2\2\2\21\3\2\2\2\2\23\3\2"+
		"\2\2\2\25\3\2\2\2\2\27\3\2\2\2\2\31\3\2\2\2\2\33\3\2\2\2\2\35\3\2\2\2"+
		"\2\37\3\2\2\2\2!\3\2\2\2\2#\3\2\2\2\2%\3\2\2\2\2\'\3\2\2\2\2)\3\2\2\2"+
		"\2+\3\2\2\2\2-\3\2\2\2\2/\3\2\2\2\2\61\3\2\2\2\2\63\3\2\2\2\2\65\3\2\2"+
		"\2\2\67\3\2\2\2\29\3\2\2\2\2;\3\2\2\2\2=\3\2\2\2\2?\3\2\2\2\2A\3\2\2\2"+
		"\2C\3\2\2\2\2E\3\2\2\2\2G\3\2\2\2\2I\3\2\2\2\2K\3\2\2\2\2M\3\2\2\2\2O"+
		"\3\2\2\2\2Q\3\2\2\2\2S\3\2\2\2\2U\3\2\2\2\2W\3\2\2\2\2Y\3\2\2\2\2[\3\2"+
		"\2\2\3a\3\2\2\2\5c\3\2\2\2\7e\3\2\2\2\tm\3\2\2\2\13v\3\2\2\2\rz\3\2\2"+
		"\2\17}\3\2\2\2\21\u0082\3\2\2\2\23\u0087\3\2\2\2\25\u008b\3\2\2\2\27\u0092"+
		"\3\2\2\2\31\u0097\3\2\2\2\33\u009d\3\2\2\2\35\u00a1\3\2\2\2\37\u00a8\3"+
		"\2\2\2!\u00af\3\2\2\2#\u00b8\3\2\2\2%\u00c0\3\2\2\2\'\u00c3\3\2\2\2)\u00c7"+
		"\3\2\2\2+\u00c9\3\2\2\2-\u00cb\3\2\2\2/\u00cd\3\2\2\2\61\u00cf\3\2\2\2"+
		"\63\u00d1\3\2\2\2\65\u00d3\3\2\2\2\67\u00d6\3\2\2\29\u00d9\3\2\2\2;\u00dc"+
		"\3\2\2\2=\u00de\3\2\2\2?\u00e1\3\2\2\2A\u00e4\3\2\2\2C\u00e7\3\2\2\2E"+
		"\u00e9\3\2\2\2G\u00eb\3\2\2\2I\u00ed\3\2\2\2K\u00ef\3\2\2\2M\u00f1\3\2"+
		"\2\2O\u00f3\3\2\2\2Q\u00f5\3\2\2\2S\u0104\3\2\2\2U\u010f\3\2\2\2W\u0121"+
		"\3\2\2\2Y\u0125\3\2\2\2[\u0131\3\2\2\2]\u0135\3\2\2\2_\u0137\3\2\2\2a"+
		"b\7?\2\2b\4\3\2\2\2cd\7@\2\2d\6\3\2\2\2ef\7v\2\2fg\7t\2\2gh\7k\2\2hi\7"+
		"i\2\2ij\7i\2\2jk\7g\2\2kl\7t\2\2l\b\3\2\2\2mn\7x\2\2no\7c\2\2op\7t\2\2"+
		"pq\7k\2\2qr\7c\2\2rs\7d\2\2st\7n\2\2tu\7g\2\2u\n\3\2\2\2vw\7n\2\2wx\7"+
		"g\2\2xy\7v\2\2y\f\3\2\2\2z{\7k\2\2{|\7h\2\2|\16\3\2\2\2}~\7v\2\2~\177"+
		"\7j\2\2\177\u0080\7g\2\2\u0080\u0081\7p\2\2\u0081\20\3\2\2\2\u0082\u0083"+
		"\7g\2\2\u0083\u0084\7n\2\2\u0084\u0085\7u\2\2\u0085\u0086\7g\2\2\u0086"+
		"\22\3\2\2\2\u0087\u0088\7h\2\2\u0088\u0089\7q\2\2\u0089\u008a\7t\2\2\u008a"+
		"\24\3\2\2\2\u008b\u008c\7t\2\2\u008c\u008d\7g\2\2\u008d\u008e\7v\2\2\u008e"+
		"\u008f\7w\2\2\u008f\u0090\7t\2\2\u0090\u0091\7p\2\2\u0091\26\3\2\2\2\u0092"+
		"\u0093\7x\2\2\u0093\u0094\7q\2\2\u0094\u0095\7k\2\2\u0095\u0096\7f\2\2"+
		"\u0096\30\3\2\2\2\u0097\u0098\7h\2\2\u0098\u0099\7n\2\2\u0099\u009a\7"+
		"q\2\2\u009a\u009b\7c\2\2\u009b\u009c\7v\2\2\u009c\32\3\2\2\2\u009d\u009e"+
		"\7k\2\2\u009e\u009f\7p\2\2\u009f\u00a0\7v\2\2\u00a0\34\3\2\2\2\u00a1\u00a2"+
		"\7u\2\2\u00a2\u00a3\7v\2\2\u00a3\u00a4\7t\2\2\u00a4\u00a5\7k\2\2\u00a5"+
		"\u00a6\7p\2\2\u00a6\u00a7\7i\2\2\u00a7\36\3\2\2\2\u00a8\u00a9\7q\2\2\u00a9"+
		"\u00aa\7d\2\2\u00aa\u00ab\7l\2\2\u00ab\u00ac\7g\2\2\u00ac\u00ad\7e\2\2"+
		"\u00ad\u00ae\7v\2\2\u00ae \3\2\2\2\u00af\u00b0\7U\2\2\u00b0\u00b1\7g\2"+
		"\2\u00b1\u00b2\7s\2\2\u00b2\u00b3\7w\2\2\u00b3\u00b4\7g\2\2\u00b4\u00b5"+
		"\7p\2\2\u00b5\u00b6\7e\2\2\u00b6\u00b7\7g\2\2\u00b7\"\3\2\2\2\u00b8\u00bd"+
		"\5]/\2\u00b9\u00bc\5]/\2\u00ba\u00bc\5_\60\2\u00bb\u00b9\3\2\2\2\u00bb"+
		"\u00ba\3\2\2\2\u00bc\u00bf\3\2\2\2\u00bd\u00bb\3\2\2\2\u00bd\u00be\3\2"+
		"\2\2\u00be$\3\2\2\2\u00bf\u00bd\3\2\2\2\u00c0\u00c1\t\2\2\2\u00c1&\3\2"+
		"\2\2\u00c2\u00c4\5_\60\2\u00c3\u00c2\3\2\2\2\u00c4\u00c5\3\2\2\2\u00c5"+
		"\u00c3\3\2\2\2\u00c5\u00c6\3\2\2\2\u00c6(\3\2\2\2\u00c7\u00c8\7<\2\2\u00c8"+
		"*\3\2\2\2\u00c9\u00ca\7=\2\2\u00ca,\3\2\2\2\u00cb\u00cc\7.\2\2\u00cc."+
		"\3\2\2\2\u00cd\u00ce\7-\2\2\u00ce\60\3\2\2\2\u00cf\u00d0\7/\2\2\u00d0"+
		"\62\3\2\2\2\u00d1\u00d2\7,\2\2\u00d2\64\3\2\2\2\u00d3\u00d4\7?\2\2\u00d4"+
		"\u00d5\7?\2\2\u00d5\66\3\2\2\2\u00d6\u00d7\7#\2\2\u00d7\u00d8\7?\2\2\u00d8"+
		"8\3\2\2\2\u00d9\u00da\7@\2\2\u00da\u00db\7?\2\2\u00db:\3\2\2\2\u00dc\u00dd"+
		"\7>\2\2\u00dd<\3\2\2\2\u00de\u00df\7>\2\2\u00df\u00e0\7?\2\2\u00e0>\3"+
		"\2\2\2\u00e1\u00e2\7/\2\2\u00e2\u00e3\7/\2\2\u00e3@\3\2\2\2\u00e4\u00e5"+
		"\7-\2\2\u00e5\u00e6\7-\2\2\u00e6B\3\2\2\2\u00e7\u00e8\7*\2\2\u00e8D\3"+
		"\2\2\2\u00e9\u00ea\7+\2\2\u00eaF\3\2\2\2\u00eb\u00ec\7]\2\2\u00ecH\3\2"+
		"\2\2\u00ed\u00ee\7_\2\2\u00eeJ\3\2\2\2\u00ef\u00f0\7}\2\2\u00f0L\3\2\2"+
		"\2\u00f1\u00f2\7\177\2\2\u00f2N\3\2\2\2\u00f3\u00f4\7#\2\2\u00f4P\3\2"+
		"\2\2\u00f5\u00f7\7\61\2\2\u00f6\u00f8\n\3\2\2\u00f7\u00f6\3\2\2\2\u00f8"+
		"\u00f9\3\2\2\2\u00f9\u00f7\3\2\2\2\u00f9\u00fa\3\2\2\2\u00fa\u00fb\3\2"+
		"\2\2\u00fb\u00ff\7\61\2\2\u00fc\u00fe\13\2\2\2\u00fd\u00fc\3\2\2\2\u00fe"+
		"\u0101\3\2\2\2\u00ff\u0100\3\2\2\2\u00ff\u00fd\3\2\2\2\u0100\u0102\3\2"+
		"\2\2\u0101\u00ff\3\2\2\2\u0102\u0103\7\61\2\2\u0103R\3\2\2\2\u0104\u010a"+
		"\7$\2\2\u0105\u0106\7^\2\2\u0106\u0109\7$\2\2\u0107\u0109\13\2\2\2\u0108"+
		"\u0105\3\2\2\2\u0108\u0107\3\2\2\2\u0109\u010c\3\2\2\2\u010a\u010b\3\2"+
		"\2\2\u010a\u0108\3\2\2\2\u010b\u010d\3\2\2\2\u010c\u010a\3\2\2\2\u010d"+
		"\u010e\7$\2\2\u010eT\3\2\2\2\u010f\u0110\7\61\2\2\u0110\u0111\7\61\2\2"+
		"\u0111\u0115\3\2\2\2\u0112\u0114\13\2\2\2\u0113\u0112\3\2\2\2\u0114\u0117"+
		"\3\2\2\2\u0115\u0116\3\2\2\2\u0115\u0113\3\2\2\2\u0116\u0119\3\2\2\2\u0117"+
		"\u0115\3\2\2\2\u0118\u011a\7\17\2\2\u0119\u0118\3\2\2\2\u0119\u011a\3"+
		"\2\2\2\u011a\u011b\3\2\2\2\u011b\u011c\7\f\2\2\u011c\u011d\3\2\2\2\u011d"+
		"\u011e\b+\2\2\u011eV\3\2\2\2\u011f\u0122\5]/\2\u0120\u0122\5%\23\2\u0121"+
		"\u011f\3\2\2\2\u0121\u0120\3\2\2\2\u0122\u0123\3\2\2\2\u0123\u0121\3\2"+
		"\2\2\u0123\u0124\3\2\2\2\u0124X\3\2\2\2\u0125\u012a\7B\2\2\u0126\u0129"+
		"\7B\2\2\u0127\u0129\13\2\2\2\u0128\u0126\3\2\2\2\u0128\u0127\3\2\2\2\u0129"+
		"\u012c\3\2\2\2\u012a\u012b\3\2\2\2\u012a\u0128\3\2\2\2\u012b\u012d\3\2"+
		"\2\2\u012c\u012a\3\2\2\2\u012d\u012e\7B\2\2\u012e\u012f\3\2\2\2\u012f"+
		"\u0130\b-\2\2\u0130Z\3\2\2\2\u0131\u0132\t\4\2\2\u0132\u0133\3\2\2\2\u0133"+
		"\u0134\b.\2\2\u0134\\\3\2\2\2\u0135\u0136\t\5\2\2\u0136^\3\2\2\2\u0137"+
		"\u0138\t\6\2\2\u0138`\3\2\2\2\20\2\u00bb\u00bd\u00c5\u00f9\u00ff\u0108"+
		"\u010a\u0115\u0119\u0121\u0123\u0128\u012a\3\b\2\2";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}