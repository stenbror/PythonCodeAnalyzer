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
            var startPos = Tokenizer.Position;
            var res = ParseFactor();
            while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMul ||
                   Tokenizer.CurSymbol.Kind == Token.TokenKind.PyDiv ||
                   Tokenizer.CurSymbol.Kind == Token.TokenKind.PyFloorDiv ||
                   Tokenizer.CurSymbol.Kind == Token.TokenKind.PyModulo ||
                   Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMatrice)
            {
                switch (Tokenizer.CurSymbol.Kind)
                {
                    case Token.TokenKind.PyMul:
                        {
                            var op = Tokenizer.CurSymbol;
                            Tokenizer.Advance();
                            var right = ParseFactor();
                            res = new TermExpression(startPos, Tokenizer.Position, TermExpression.OperatorKind.Mul, res, op, right);
                        }
                        break;
                    case Token.TokenKind.PyDiv:
                        {
                            var op = Tokenizer.CurSymbol;
                            Tokenizer.Advance();
                            var right = ParseFactor();
                            res = new TermExpression(startPos, Tokenizer.Position, TermExpression.OperatorKind.Div, res, op, right);
                        }
                        break;
                    case Token.TokenKind.PyFloorDiv:
                        {
                            var op = Tokenizer.CurSymbol;
                            Tokenizer.Advance();
                            var right = ParseFactor();
                            res = new TermExpression(startPos, Tokenizer.Position, TermExpression.OperatorKind.FloorDiv, res, op, right);
                        }
                        break;
                    case Token.TokenKind.PyModulo:
                        {
                            var op = Tokenizer.CurSymbol;
                            Tokenizer.Advance();
                            var right = ParseFactor();
                            res = new TermExpression(startPos, Tokenizer.Position, TermExpression.OperatorKind.Modulo, res, op, right);
                        }
                        break;
                    case Token.TokenKind.PyMatrice:
                        {
                            var op = Tokenizer.CurSymbol;
                            Tokenizer.Advance();
                            var right = ParseFactor();
                            res = new TermExpression(startPos, Tokenizer.Position, TermExpression.OperatorKind.Matrice, res, op, right);
                        }
                        break;
                }
            }
            return res;
        }
        
        public ExpressionNode ParseArithExpr()
        {
            var startPos = Tokenizer.Position;
            var res = ParseTerm();
            while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPlus ||
                   Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMinus )
            {
                switch (Tokenizer.CurSymbol.Kind)
                {
                    case Token.TokenKind.PyPlus:
                        {
                            var op = Tokenizer.CurSymbol;
                            Tokenizer.Advance();
                            var right = ParseTerm();
                            res = new ArithExpression(startPos, Tokenizer.Position, ArithExpression.ArithOperatorKind.Plus, res, op, right);
                        }
                        break;
                    case Token.TokenKind.PyMinus:
                        {
                            var op = Tokenizer.CurSymbol;
                            Tokenizer.Advance();
                            var right = ParseTerm();
                            res = new ArithExpression(startPos, Tokenizer.Position, ArithExpression.ArithOperatorKind.Minus, res, op, right);
                        }
                        break;
                }
            }
            return res;
        }
        
        public ExpressionNode ParseShiftExpr()
        {
            var startPos = Tokenizer.Position;
            var res = ParseArithExpr();
            while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyShiftLeft ||
                   Tokenizer.CurSymbol.Kind == Token.TokenKind.PyShiftRight )
            {
                switch (Tokenizer.CurSymbol.Kind)
                {
                    case Token.TokenKind.PyShiftLeft:
                    {
                        var op = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        var right = ParseArithExpr();
                        res = new ArithExpression(startPos, Tokenizer.Position, ArithExpression.ArithOperatorKind.Plus, res, op, right);
                    }
                        break;
                    case Token.TokenKind.PyShiftRight:
                    {
                        var op = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        var right = ParseArithExpr();
                        res = new ArithExpression(startPos, Tokenizer.Position, ArithExpression.ArithOperatorKind.Minus, res, op, right);
                    }
                        break;
                }
            }
            return res;
        }
        
        public ExpressionNode ParseAndExpr()
        {
            var startPos = Tokenizer.Position;
            var res = ParseShiftExpr();
            while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAnd) // '&'
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseShiftExpr();
                res = new AndExpression(startPos, Tokenizer.Position, res, op, right);
            }
            return res;
        }
        
        public ExpressionNode ParseXorExpr()
        {
            var startPos = Tokenizer.Position;
            var res = ParseAndExpr();
            while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyXor) // '^'
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseAndExpr();
                res = new XorExpression(startPos, Tokenizer.Position, res, op, right);
            }
            return res;
        }
        
        public ExpressionNode ParseOrExpr()
        {
            var startPos = Tokenizer.Position;
            var res = ParseXorExpr();
            while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyOr) // '|'
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseXorExpr();
                res = new OrExpression(startPos, Tokenizer.Position, res, op, right);
            }
            return res;
        }
        
        public ExpressionNode ParseStarExpr()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMul)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseOrExpr();
                return new StarExpression(startPos, Tokenizer.Position, op, right);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting '*' in star expression!");
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