using System;
using System.Collections.Generic;

namespace PythonCodeAnalyzer.Parser
{
    public interface IPythonTokenizer
    {
        public Token CurSymbol { get; set; }
        public uint Position { get; set; }
        public void Advance();
        public Token[] Symbols { get; set; }
    }
    
    public class PythonTokenizer : IPythonTokenizer
    {
        public Token CurSymbol { get; set; }
        public uint Position { get; set; }
        public Token[] Symbols { get; set; }
        
        private Dictionary<string, Token.TokenKind> ReservedKeywords = new Dictionary<string, Token.TokenKind>()
        {
            { "False", Token.TokenKind.PyFalse },
            { "None", Token.TokenKind.PyNone },
            { "True", Token.TokenKind.PyTrue },
            { "and", Token.TokenKind.PyAnd },
            { "as", Token.TokenKind.PyAs },
            { "assert", Token.TokenKind.PyAssert },
            { "async", Token.TokenKind.PyAsync },
            { "await", Token.TokenKind.PyAwait },
            { "break", Token.TokenKind.PyBreak },
            { "class", Token.TokenKind.PyClass },
            { "continue", Token.TokenKind.PyContinue },
            { "def", Token.TokenKind.PyDef },
            { "del", Token.TokenKind.PyDel },
            { "elif", Token.TokenKind.PyElif },
            { "else", Token.TokenKind.PyElse },
            { "except", Token.TokenKind.PyExcept },
            { "finally", Token.TokenKind.PyFinally },
            { "for", Token.TokenKind.PyFor },
            { "from", Token.TokenKind.PyFrom },
            { "global", Token.TokenKind.PyGlobal },
            { "if", Token.TokenKind.PyIf },
            { "import", Token.TokenKind.PyImport },
            { "in", Token.TokenKind.PyIn },
            { "is", Token.TokenKind.PyIs },
            { "lambda", Token.TokenKind.PyLambda },
            { "nonlocal", Token.TokenKind.PyNonlocal },
            { "not", Token.TokenKind.PyNot },
            { "or", Token.TokenKind.PyOr },
            { "pass", Token.TokenKind.PyPass },
            { "raise", Token.TokenKind.PyRaise },
            { "return", Token.TokenKind.PyReturn },
            { "try", Token.TokenKind.PyTry },
            { "while", Token.TokenKind.PyWhile },
            { "with", Token.TokenKind.PyWith },
            { "yield", Token.TokenKind.PyYield }
        };
        
        private uint[] _indentStack = new uint[100];
        private uint _indentLevel = 0;
        private int _pending = 0;

        public Token.TokenKind IsReservedKeywordOrLiteralName(string key)
        {
            if (ReservedKeywords.ContainsKey(key))
            {
                return ReservedKeywords[key];
            }

            return Token.TokenKind.Name;
        }

        public static bool IsLiteralNameStartCharacter(char ch)
        {
            return ch == '_' || char.IsLetter(ch);
        }

        public static bool IsLiteralCharacterOrDigit(char ch)
        {
            return ch == '_' || char.IsLetterOrDigit(ch);
        }

        public void Advance()
        {
            
        }
    }
}