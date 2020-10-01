using System;
using System.Collections.Generic;
using System.Security.Principal;
using PythonCodeAnalyzer.Parser.Ast;
using PythonCodeAnalyzer.Parser.Ast.Expression;

namespace PythonCodeAnalyzer.Parser
{
    public class PythonParser
    {
        public IPythonTokenizer Tokenizer { get; set; }


        public ExpressionNode ParseAtom()
        {
            var startPos = Tokenizer.Position;
            switch (Tokenizer.CurSymbol.Kind)
            {
                case Token.TokenKind.Name:
                {
                    var unit = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    return new NameLiteralExpression(startPos, Tokenizer.Position, unit);
                }
                case Token.TokenKind.Number:
                {
                    var unit = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    return new NumberLiteralExpression(startPos, Tokenizer.Position, unit);
                }
                case Token.TokenKind.String:
                {
                    var units = new List<Token>();
                    while (Tokenizer.CurSymbol.Kind == Token.TokenKind.String)
                    {
                        units.Add(Tokenizer.CurSymbol);
                        Tokenizer.Advance();
                    }
                    return new StringLiteralExpression(startPos, Tokenizer.Position, units.ToArray());
                }
                case Token.TokenKind.PyNone:
                {
                    var unit = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    return new NoneExpression(startPos, Tokenizer.Position, unit);   
                }
                case Token.TokenKind.PyElipsis:
                {
                    var unit = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    return new ElipsisLiteralExpression(startPos, Tokenizer.Position, unit); 
                }
                case Token.TokenKind.PyTrue:
                {
                    var unit = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    return new TrueLiteralExpression(startPos, Tokenizer.Position, unit);
                }
                case Token.TokenKind.PyFalse:
                {
                    var unit = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    return new FalseLiteralExpression(startPos, Tokenizer.Position, unit); 
                }
                case Token.TokenKind.PyLeftParen:
                case Token.TokenKind.PyLeftBracket:
                case Token.TokenKind.PyLeftCurly:
                    throw new NotImplementedException();            
                default:
                    throw new SyntaxErrorException(startPos, Tokenizer.CurSymbol, "Illegal literal!");
            }
        }

        public ExpressionNode ParseAtomExpression()
        {
            var startPos = Tokenizer.Position;
            Token awaitOp = null;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAwait)
            {
                awaitOp = Tokenizer.CurSymbol;
                Tokenizer.Advance();
            }
            var res = ParseAtom();
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyLeftParen ||
                Tokenizer.CurSymbol.Kind == Token.TokenKind.PyLeftBracket || Tokenizer.CurSymbol.Kind == Token.TokenKind.PyDot)
            {
                // Handle Trailer later!
                return new AtomExpression(startPos, Tokenizer.Position, awaitOp != null, awaitOp, res, null);
            }
            if (awaitOp != null)
            {
                return new AtomExpression(startPos, Tokenizer.Position, true, awaitOp, res, null);
            }
            return res;
        }
        
        
    }
}