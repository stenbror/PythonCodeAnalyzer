using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
                var trailers = ParseTrailer();
                return new AtomExpression(startPos, Tokenizer.Position, awaitOp != null, awaitOp, res, trailers);
            }
            if (awaitOp != null)
            {
                return new AtomExpression(startPos, Tokenizer.Position, true, awaitOp, res, null);
            }
            return res;
        }

        public ExpressionNode[] ParseTrailer()
        {
            return new ExpressionNode[] { };
        }

        public ExpressionNode ParsePower()
        {
            var startPos = Tokenizer.Position;
            var left = ParseAtomExpression();
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPower)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseFactor();
                return new PowerExpression(startPos, Tokenizer.Position, left, op, right);
            }
            return left;
        }
        
        public ExpressionNode ParseFactor()
        {
            var startPos = Tokenizer.Position;
            switch (Tokenizer.CurSymbol.Kind)
            {
                case Token.TokenKind.PyPlus:
                {
                    var op = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var right = ParseFactor();
                    return new FactorExpression(startPos, Tokenizer.Position, FactorExpression.FactorOperatorKind.UnaryPlus, op, right);
                }
                case Token.TokenKind.PyMinus:
                {
                    var op = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var right = ParseFactor();
                    return new FactorExpression(startPos, Tokenizer.Position, FactorExpression.FactorOperatorKind.UnaryMinus, op, right);
                }
                case Token.TokenKind.PyInvert:
                {
                    var op = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var right = ParseFactor();
                    return new FactorExpression(startPos, Tokenizer.Position, FactorExpression.FactorOperatorKind.UnaryInvert, op, right);
                }
                default:
                    return ParsePower();
            }
        }
        
        public ExpressionNode ParseTerm()
        {
            throw new NotImplementedException();
        }
        
        public ExpressionNode ParseArithExpr()
        {
            throw new NotImplementedException();
        }
        
        public ExpressionNode ParseShiftExpr()
        {
            throw new NotImplementedException();
        }
        
        public ExpressionNode ParseAndExpr()
        {
            throw new NotImplementedException();
        }
        
        public ExpressionNode ParseXorExpr()
        {
            throw new NotImplementedException();
        }
        
        public ExpressionNode ParseOrExpr()
        {
            throw new NotImplementedException();
        }
        
        public ExpressionNode ParseStarExpr()
        {
            throw new NotImplementedException();
        }
        
        public ExpressionNode ParseComparison()
        {
            throw new NotImplementedException();
        }
        
        public ExpressionNode ParseNotTest()
        {
            throw new NotImplementedException();
        }
        
        public ExpressionNode ParseAndTest()
        {
            throw new NotImplementedException();
        }
        
        public ExpressionNode ParseLambda(bool isConditional)
        {
            throw new NotImplementedException();
        }
        
        public ExpressionNode ParseNoCond()
        {
            throw new NotImplementedException();
        }
        
        public ExpressionNode ParseTest()
        {
            throw new NotImplementedException();
        }
        
        public ExpressionNode ParseNamedExpr()
        {
            throw new NotImplementedException();
        }
        
        
    }
}