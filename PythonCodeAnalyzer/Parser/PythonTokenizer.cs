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
        private uint _index = 0;
        private uint _TokenStartPos = 0;
        private uint _TokenEndPos = 0;
        private bool _atBOL = false;
        private Stack<char> _LevelStack;
        
        private char[] SourceCode { get; set; }
        public uint TabSize { get; set; }
        private bool IsInteractiveMode { get; set; }

        public PythonTokenizer(char[] sourceCode, bool isInteractive, uint tabSize)
        {
            SourceCode = sourceCode;
            TabSize = tabSize;
            IsInteractiveMode = isInteractive;
            _indentLevel = 0;
            _pending = 0;
            _index = 0;
            _TokenStartPos = 0;
            _TokenEndPos = 0;
            for (var i = 0; i < 100; i++) _indentStack[i] = 0;
            _atBOL = true;
            _LevelStack = new Stack<char>();
        }

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
            CurSymbol = GetSymbol();
        }

        public Token GetSymbol()
        {
            _TokenStartPos = _index;
            
            /* Handle whitespace and other Trivia below */
            
            /* Handle End Of File */
            
            /* Handle Name literal or Reserved keywords */
            
            /* Handle Newline - Token or Trivia */
            
            /* Period or start of Number */
            
            /* Handle Numbers */
            
            /* Handle String */
            
            /* Handle Line continuation */
            
            /* Handle Operators or Delimiters */
            switch (SourceCode[_index])
            {
                case '(':
                    _LevelStack.Push(')');
                    _index++;
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyLeftParen);
                    break;
                case '[':
                    _LevelStack.Push(']');
                    _index++;
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyLeftBracket);
                    break;
                case '{':
                    _LevelStack.Push('}');
                    _index++;
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyLeftCurly);
                    break;
                case ')':
                    if (SourceCode[_index] != _LevelStack.Peek())
                    {
                        throw new NotImplementedException(); // Needs new LexicalException
                    }
                    _LevelStack.Pop();
                    _index++;
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyRightParen);
                    break;
                case ']':
                    if (SourceCode[_index] != _LevelStack.Peek())
                    {
                        throw new NotImplementedException(); // Needs new LexicalException
                    }
                    _LevelStack.Pop();
                    _index++;
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyRightBracket);
                    break;
                case '}':
                    if (SourceCode[_index] != _LevelStack.Peek())
                    {
                        throw new NotImplementedException(); // Needs new LexicalException
                    }
                    _LevelStack.Pop();
                    _index++;
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyRightCurly);
                    break;
                
            }
            
            

            throw new NotImplementedException(); // Needs new LexicalException
        }
    }
}