using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;

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
            if (Char.IsLetter(SourceCode[_index]) || SourceCode[_index] == '_')
            {
                _index++;
                while (Char.IsLetterOrDigit(SourceCode[_index]) || SourceCode[_index] == '_') _index++;
                var key = new String( SourceCode[ (int) _TokenStartPos .. (int) _index ] );
                
                if (ReservedKeywords.ContainsKey(key)) return new Token(_TokenStartPos, _index, ReservedKeywords[key] );
                else return new Token(_TokenStartPos, _index, Token.TokenKind.Name);
            }
            
            /* Handle Newline - Token or Trivia */

            /* Period or start of Number */
            if (SourceCode[_index] == '.')
            {
                if (SourceCode[_index + 1] == '.' && SourceCode[_index + 2] == '.')
                {
                    _index += 3;
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyElipsis);
                }
                else if (!char.IsDigit(SourceCode[_index + 1]))
                {
                    _index++;
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyDot);
                }
            }
            
            /* Handle Numbers */
            if (char.IsDigit(SourceCode[_index]) || SourceCode[_index] == '.')
            {
                
            }
            
            /* Handle String */
            
            /* Handle Line continuation */
            
            /* Handle Operators or Delimiters */
            switch (SourceCode[_index])
            {
                case '(':
                    _LevelStack.Push(')');
                    _index++;
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyLeftParen);
                case '[':
                    _LevelStack.Push(']');
                    _index++;
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyLeftBracket);
                case '{':
                    _LevelStack.Push('}');
                    _index++;
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyLeftCurly);
                case ')':
                    if (_LevelStack.Count == 0 || SourceCode[_index] != _LevelStack.Peek())
                    {
                        throw new NotImplementedException(); // Needs new LexicalException
                    }
                    _LevelStack.Pop();
                    _index++;
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyRightParen);
                case ']':
                    if (_LevelStack.Count == 0 || SourceCode[_index] != _LevelStack.Peek())
                    {
                        throw new NotImplementedException(); // Needs new LexicalException
                    }
                    _LevelStack.Pop();
                    _index++;
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyRightBracket);
                case '}':
                    if (_LevelStack.Count == 0 || SourceCode[_index] != _LevelStack.Peek())
                    {
                        throw new NotImplementedException(); // Needs new LexicalException
                    }
                    _LevelStack.Pop();
                    _index++;
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyRightCurly);
                case ';':
                    _index++;
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PySemiColon);
                case ',':
                    _index++;
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyComma);
                case '~':
                    _index++;
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyInvert);
                case '+':
                    _index++;
                    if (SourceCode[_index] == '=')
                    {
                        _index++;
                        return new Token(_TokenStartPos, _index, Token.TokenKind.PyPlusAssign);
                    }
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyPlus);
                case '-':
                    _index++;
                    if (SourceCode[_index] == '=')
                    {
                        _index++;
                        return new Token(_TokenStartPos, _index, Token.TokenKind.PyMinusAssign);
                    }
                    else if (SourceCode[_index] == '>')
                    {
                        _index++;
                        return new Token(_TokenStartPos, _index, Token.TokenKind.PyArrow);
                    } 
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyMinus);
                case '*':
                    _index++;
                    if (SourceCode[_index] == '*')
                    {
                        _index++;
                        if (SourceCode[_index] == '=')
                        {
                            _index++;
                            return new Token(_TokenStartPos, _index, Token.TokenKind.PyPowerAssign);
                        }
                        return new Token(_TokenStartPos, _index, Token.TokenKind.PyPower);
                    }
                    else if (SourceCode[_index] == '=')
                    {
                        _index++;
                        return new Token(_TokenStartPos, _index, Token.TokenKind.PyMulAssign);
                    } 
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyMul);
                case '/':
                    _index++;
                    if (SourceCode[_index] == '/')
                    {
                        _index++;
                        if (SourceCode[_index] == '=')
                        {
                            _index++;
                            return new Token(_TokenStartPos, _index, Token.TokenKind.PyFloorDivAssign);
                        }
                        return new Token(_TokenStartPos, _index, Token.TokenKind.PyFloorDiv);
                    }
                    else if (SourceCode[_index] == '=')
                    {
                        _index++;
                        return new Token(_TokenStartPos, _index, Token.TokenKind.PyDivAssign);
                    } 
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyDiv);
                case '<':
                    _index++;
                    if (SourceCode[_index] == '<')
                    {
                        _index++;
                        if (SourceCode[_index] == '=')
                        {
                            _index++;
                            return new Token(_TokenStartPos, _index, Token.TokenKind.PyShiftLeftAssign);
                        }
                        return new Token(_TokenStartPos, _index, Token.TokenKind.PyShiftLeft);
                    }
                    else if (SourceCode[_index] == '=')
                    {
                        _index++;
                        return new Token(_TokenStartPos, _index, Token.TokenKind.PyLessEqual);
                    } 
                    else if (SourceCode[_index] == '>') // Yes, it is not standard anymore in CPython, but i want it.
                    {
                        _index++;
                        return new Token(_TokenStartPos, _index, Token.TokenKind.PyNotEqual);
                    } 
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyLess);
                case '>':
                    _index++;
                    if (SourceCode[_index] == '>')
                    {
                        _index++;
                        if (SourceCode[_index] == '=')
                        {
                            _index++;
                            return new Token(_TokenStartPos, _index, Token.TokenKind.PyShiftRightAssign);
                        }
                        return new Token(_TokenStartPos, _index, Token.TokenKind.PyShiftRight);
                    }
                    else if (SourceCode[_index] == '=')
                    {
                        _index++;
                        return new Token(_TokenStartPos, _index, Token.TokenKind.PyGreaterEqual);
                    }
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyGreater);
                case '%':
                    _index++;
                    if (SourceCode[_index] == '=')
                    {
                        _index++;
                        return new Token(_TokenStartPos, _index, Token.TokenKind.PyModuloAssign);
                    }
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyModulo);
                case '@':
                    _index++;
                    if (SourceCode[_index] == '=')
                    {
                        _index++;
                        return new Token(_TokenStartPos, _index, Token.TokenKind.PyMatriceAssign);
                    }
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyMatrice);
                case '&':
                    _index++;
                    if (SourceCode[_index] == '=')
                    {
                        _index++;
                        return new Token(_TokenStartPos, _index, Token.TokenKind.PyAndAssign);
                    }
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyAnd);
                case '|':
                    _index++;
                    if (SourceCode[_index] == '=')
                    {
                        _index++;
                        return new Token(_TokenStartPos, _index, Token.TokenKind.PyOrAssign);
                    }
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyOr);
                case '^':
                    _index++;
                    if (SourceCode[_index] == '=')
                    {
                        _index++;
                        return new Token(_TokenStartPos, _index, Token.TokenKind.PyXorAssign);
                    }
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyXor);
                case '=':
                    _index++;
                    if (SourceCode[_index] == '=')
                    {
                        _index++;
                        return new Token(_TokenStartPos, _index, Token.TokenKind.PyEqual);
                    }
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyAssign);
                case '!':
                    _index++;
                    if (SourceCode[_index] == '=')
                    {
                        _index++;
                        return new Token(_TokenStartPos, _index, Token.TokenKind.PyNotEqual);
                    }
                    throw new NotImplementedException();
                case ':':
                    _index++;
                    if (SourceCode[_index] == '=')
                    {
                        _index++;
                        return new Token(_TokenStartPos, _index, Token.TokenKind.PyColonAssign);
                    }
                    return new Token(_TokenStartPos, _index, Token.TokenKind.PyColon);
            }
            
            

            throw new NotImplementedException(); // Needs new LexicalException
        }
    }
}