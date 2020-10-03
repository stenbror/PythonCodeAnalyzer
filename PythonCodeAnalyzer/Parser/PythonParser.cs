using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
                {
                    var op1 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    ExpressionNode right = null;
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyRightParen)
                    {
                        var op2 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        return new TupleExpression(startPos, Tokenizer.Position, op1, right, op2);
                    }
                    right = (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyYield)
                        ? ParseYieldExpr()
                        : ParseTestListComp();
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyRightParen)
                    {
                        var op2 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        return new TupleExpression(startPos, Tokenizer.Position, op1, right, op2);
                    }
                    throw new SyntaxErrorException(startPos, Tokenizer.CurSymbol, "Expecting ')' in tuple declaration!");
                }
                case Token.TokenKind.PyLeftBracket:
                {
                    var op1 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    ExpressionNode right = null;
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyRightBracket)
                    {
                        var op2 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        return new AtomListExpression(startPos, Tokenizer.Position, op1, right, op2);
                    }
                    right = ParseTestListComp();
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyRightBracket)
                    {
                        var op2 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        return new AtomListExpression(startPos, Tokenizer.Position, op1, right, op2);
                    }
                    throw new SyntaxErrorException(startPos, Tokenizer.CurSymbol,
                        "Expecting ']' in tuple declaration!");
                }
                case Token.TokenKind.PyLeftCurly:
                {
                    var op1 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    ExpressionNode right = null;
                    bool isDictionary = true;
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyRightBracket)
                    {
                        var op2 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        return new DictionaryExpression(startPos, Tokenizer.Position, op1, right, op2);
                    }
                    (right, isDictionary) = ParseDictorSetMaker();
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyRightBracket)
                    {
                        var op2 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        return isDictionary ? (ExpressionNode) new DictionaryExpression(startPos, Tokenizer.Position, op1, right, op2) : new SetExpression(startPos, Tokenizer.Position, op1, right, op2);
                    }
                    throw new SyntaxErrorException(startPos, Tokenizer.CurSymbol,
                        "Expecting '}' in dictionary or set declaration!");
                }
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
            var startPos = Tokenizer.Position;
            var res = ParseOrExpr();
            while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyLess ||
                   Tokenizer.CurSymbol.Kind == Token.TokenKind.PyLessEqual ||
                   Tokenizer.CurSymbol.Kind == Token.TokenKind.PyEqual ||
                   Tokenizer.CurSymbol.Kind == Token.TokenKind.PyGreaterEqual ||
                   Tokenizer.CurSymbol.Kind == Token.TokenKind.PyGreater ||
                   Tokenizer.CurSymbol.Kind == Token.TokenKind.PyNotEqual ||
                   Tokenizer.CurSymbol.Kind == Token.TokenKind.PyNot ||
                   Tokenizer.CurSymbol.Kind == Token.TokenKind.PyIn ||
                   Tokenizer.CurSymbol.Kind == Token.TokenKind.PyIs)
            {
                switch (Tokenizer.CurSymbol.Kind)
                {
                    case Token.TokenKind.PyLess:
                        {
                            var op = Tokenizer.CurSymbol;
                            Tokenizer.Advance();
                            var right = ParseOrExpr();
                            res = new RelationExpression(startPos, Tokenizer.Position, RelationExpression.Relation.Less , res, op, null, right);
                        }
                        break;
                    case Token.TokenKind.PyLessEqual:
                        {
                            var op = Tokenizer.CurSymbol;
                            Tokenizer.Advance();
                            var right = ParseOrExpr();
                            res = new RelationExpression(startPos, Tokenizer.Position, RelationExpression.Relation.LessEqual , res, op, null, right);
                        }
                        break;
                    case Token.TokenKind.PyEqual:
                        {
                            var op = Tokenizer.CurSymbol;
                            Tokenizer.Advance();
                            var right = ParseOrExpr();
                            res = new RelationExpression(startPos, Tokenizer.Position, RelationExpression.Relation.Equal , res, op, null, right);
                        }
                        break;
                    case Token.TokenKind.PyGreaterEqual:
                        {
                            var op = Tokenizer.CurSymbol;
                            Tokenizer.Advance();
                            var right = ParseOrExpr();
                            res = new RelationExpression(startPos, Tokenizer.Position, RelationExpression.Relation.GreaterEqual , res, op, null, right);
                        }
                        break;
                    case Token.TokenKind.PyGreater:
                        {
                            var op = Tokenizer.CurSymbol;
                            Tokenizer.Advance();
                            var right = ParseOrExpr();
                            res = new RelationExpression(startPos, Tokenizer.Position, RelationExpression.Relation.Greater , res, op, null, right);
                        }
                        break;
                    case Token.TokenKind.PyNotEqual:
                        {
                            var op = Tokenizer.CurSymbol;
                            Tokenizer.Advance();
                            var right = ParseOrExpr();
                            res = new RelationExpression(startPos, Tokenizer.Position, RelationExpression.Relation.NotEqual , res, op, null, right);
                        }
                        break;
                    case Token.TokenKind.PyIn:
                        {
                            var op = Tokenizer.CurSymbol;
                            Tokenizer.Advance();
                            var right = ParseOrExpr();
                            res = new RelationExpression(startPos, Tokenizer.Position, RelationExpression.Relation.In , res, op, null, right);
                        }
                        break;
                    case Token.TokenKind.PyIs:
                        {
                            var op = Tokenizer.CurSymbol;
                            Tokenizer.Advance();
                            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyNot)
                            {
                                var op2 = Tokenizer.CurSymbol;
                                Tokenizer.Advance();
                                var right = ParseOrExpr();
                                res = new RelationExpression(startPos, Tokenizer.Position, RelationExpression.Relation.IsNot , res, op, op2, right);
                            }
                            else
                            {
                                var right = ParseOrExpr();
                                res = new RelationExpression(startPos, Tokenizer.Position, RelationExpression.Relation.Less , res, op, null, right);
                            }
                        }
                        break;
                    case Token.TokenKind.PyNot:
                        {
                            var op = Tokenizer.CurSymbol;
                            Tokenizer.Advance();
                            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyIn)
                            {
                                var right = ParseOrExpr();
                                res = new RelationExpression(startPos, Tokenizer.Position, RelationExpression.Relation.NotIn , res, op, null, right);
                            }
                            else
                            {
                                throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'not' 'in' , but missing 'in' in relation expression!");
                            }
                            
                        }
                        break;
                }
            }
            return res;
        }
        
        public ExpressionNode ParseNotTest()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyNot)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseNotTest();
                return new NotTestExpression(startPos, Tokenizer.Position, op, right);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'not' in not test expression!");
        }
        
        public ExpressionNode ParseAndTest()
        {
            var startPos = Tokenizer.Position;
            var res = ParseNotTest();
            while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyTestAnd) // 'and'
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseNotTest();
                res = new AndTestExpression(startPos, Tokenizer.Position, res, op, right);
            }
            return res;
        }
        
        public ExpressionNode ParseOrTest()
        {
            var startPos = Tokenizer.Position;
            var res = ParseNotTest();
            while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyTestOr) // 'or'
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseNotTest();
                res = new OrTestExpression(startPos, Tokenizer.Position, res, op, right);
            }
            return res;
        }
        
        public ExpressionNode ParseLambda(bool isConditional)
        {
            throw new NotImplementedException();
        }
        
        public ExpressionNode ParseNoCond()
        {
            return (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyLambda) ? ParseLambda(false) : ParseOrTest();
        }
        
        public ExpressionNode ParseTest()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyLambda) return ParseLambda(true);
            var left = ParseOrTest();
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyIf)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseOrTest();
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyElse)
                {
                    var op2 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var next = ParseTest();
                    return new ConditionalExpression(startPos, Tokenizer.Position, left, op, right, op2, next);
                }
                throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Missing 'else' in test expression!");
            }
            return left;
        }
        
        public ExpressionNode ParseNamedExpr()
        {
            var startPos = Tokenizer.Position;
            var left = ParseTest();
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyColonAssign)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseTest();
                return new NamedExpression(startPos, Tokenizer.Position, left, op, right);
            }
            return left;
        }

        public ExpressionNode ParseTestListComp()
        {
            var startPos = Tokenizer.Position;
            var node = (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMul) ? ParseStarExpr() : ParseNamedExpr();
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyFor ||
                Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAsync)
            {
                var nodes = new List<ExpressionNode>();
                nodes.Add(node);
                nodes.Add(ParseCompFor());
                return new ListExpression(startPos, Tokenizer.Position, ListExpression.ListType.TestList , nodes.ToArray(), null);
            }
            else if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
            {
                var nodes = new List<ExpressionNode>();
                var separators = new List<Token>();
                nodes.Add(node);
                while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                {
                    separators.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Double ',' in expression list!");
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyRightBracket || Tokenizer.CurSymbol.Kind == Token.TokenKind.PyRightParen) continue;
                    nodes.Add((Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMul) ? ParseStarExpr() : ParseNamedExpr());
                }
                return new ListExpression(startPos, Tokenizer.Position, ListExpression.ListType.TestList , nodes.ToArray(), separators.ToArray());
            }
            return node;
        }
        
        public ExpressionNode ParseSubscriptList()
        {
            var startPos = Tokenizer.Position;
            var node = ParseSubscript();
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
            {
                var nodes = new List<ExpressionNode>();
                var separators = new List<Token>();
                nodes.Add(node);
                while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                {
                    separators.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Double ',' in expression list!");
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyRightBracket) continue;
                    nodes.Add(ParseSubscript());
                }
                return new ListExpression(startPos, Tokenizer.Position, ListExpression.ListType.SubscriptList , nodes.ToArray(), separators.ToArray());
            }
            return node;
        }
        
        public ExpressionNode ParseSubscript()
        {
            var startPos = Tokenizer.Position;
            ExpressionNode one = null, two = null, three = null;
            Token op1 = null, op2 = null;
            if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyColon) one = ParseTest();
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyColon)
            {
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyColon)
                {
                    op1 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                }

                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyRightBracket &&
                    Tokenizer.CurSymbol.Kind != Token.TokenKind.PyColon &&
                    Tokenizer.CurSymbol.Kind != Token.TokenKind.PyComma)
                {
                    two = ParseTest();
                }

                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyColon)
                {
                    op2 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyComma &&
                        Tokenizer.CurSymbol.Kind != Token.TokenKind.PyRightBracket)
                    {
                        three = ParseTest();
                    }
                }
            }
            return new SubscriptExpression(startPos, Tokenizer.Position, one, op1, two, op2, three);
        }
        
        public ExpressionNode ParseExprList()
        {
            var startPos = Tokenizer.Position;
            var node = (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMul) ? ParseStarExpr() : ParseOrExpr();
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
            {
                var nodes = new List<ExpressionNode>();
                var separators = new List<Token>();
                nodes.Add(node);
                while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                {
                    separators.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Double ',' in expression list!");
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyColon || Tokenizer.CurSymbol.Kind == Token.TokenKind.PyIn) continue;
                    nodes.Add((Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMul) ? ParseStarExpr() : ParseOrExpr());
                }
                return new ListExpression(startPos, Tokenizer.Position, ListExpression.ListType.TestList, nodes.ToArray(), separators.ToArray());
            }
            return node;
        }
        
        public ExpressionNode ParseTestList()
        {
            var startPos = Tokenizer.Position;
            var node = ParseTest();
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
            {
                var nodes = new List<ExpressionNode>();
                var separators = new List<Token>();
                nodes.Add(node);
                while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                {
                    separators.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Double ',' in expression list!");
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyColon || Tokenizer.CurSymbol.Kind == Token.TokenKind.PySemiColon || Tokenizer.CurSymbol.Kind == Token.TokenKind.Newline) continue;
                    nodes.Add(ParseTest());
                }
                return new ListExpression(startPos, Tokenizer.Position, ListExpression.ListType.TestList, nodes.ToArray(), separators.ToArray());
            }
            return node;
        }
        
        public Tuple<ExpressionNode, bool> ParseDictorSetMaker()
        {
            throw new NotImplementedException();
        }
        
        public ExpressionNode ParseArgList()
        {
            var startPos = Tokenizer.Position;
            var node = ParseArgument();
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
            {
                var nodes = new List<ExpressionNode>();
                var separators = new List<Token>();
                nodes.Add(node);
                while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                {
                    separators.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Double ',' in expression list!");
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyRightParen) continue;
                    nodes.Add(ParseArgument());
                }
                return new ListExpression(startPos, Tokenizer.Position, ListExpression.ListType.ArgumentList , nodes.ToArray(), separators.ToArray());
            }
            return node;
        }
        
        public ExpressionNode ParseArgument()
        { 
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMul)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.Name)
                {
                    var op2 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    return new StarArgument(startPos, Tokenizer.Position, op, op2);
                }
                throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting name literal in '*' argument!");
            }
            else if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPower)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.Name)
                {
                    var op2 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    return new PowerArgument(startPos, Tokenizer.Position, op, op2);
                }
                throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting name literal in '**' argument!");
            }
            else if (Tokenizer.CurSymbol.Kind == Token.TokenKind.Name)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                switch (Tokenizer.CurSymbol.Kind)
                {
                    case Token.TokenKind.PyAsync:
                    case Token.TokenKind.PyFor:
                    {
                        var right = ParseCompFor();
                        return new ArgumentExpression(startPos, Tokenizer.Position, op, null, right);
                    }
                    case Token.TokenKind.PyColonAssign:
                    case Token.TokenKind.PyAssign:
                    {
                        var op2 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        var right = ParseTest();
                        return new ArgumentExpression(startPos, Tokenizer.Position, op, op2, right);
                    }
                    default:
                        return new ArgumentExpression(startPos, Tokenizer.Position, op, null, null);
                }
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting valid argument!");
        }
        
        public ExpressionNode ParseCompIter()
        {
            return (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAsync ||
                    Tokenizer.CurSymbol.Kind == Token.TokenKind.PyFor)
                ? ParseCompFor()
                : ParseCompIf();
        }
        
        public ExpressionNode ParseSyncCompFor()
        {
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyFor)
            {
                var startPos = Tokenizer.Position;
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var left = ParseExprList();
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyIn)
                {
                    var op2 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var right = ParseOrTest();
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAsync ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyFor ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyIf)
                    {
                        var next = ParseCompIter();
                        return new CompSyncForExpression(startPos, Tokenizer.Position, op1, left, op2, right, next);
                    }
                    return new CompSyncForExpression(startPos, Tokenizer.Position, op1, left, op2, right, null);
                }
                throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'in' in for comprehension expression!");
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'for' in for comprehension expression!");
        }
        
        public ExpressionNode ParseCompFor()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAsync)
            {
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseSyncCompFor();
                return new CompForExpression(startPos, Tokenizer.Position, op1, right);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'async' in for comprehension expression!");
        }
        
        public ExpressionNode ParseCompIf()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAsync)
            {
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseNoCond();
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAsync ||
                   Tokenizer.CurSymbol.Kind == Token.TokenKind.PyFor ||
                   Tokenizer.CurSymbol.Kind == Token.TokenKind.PyIf)
                {
                    var next = ParseCompIter();
                    return new CompIfExpression(startPos, Tokenizer.Position, op1, right, next);
                }
                return new CompIfExpression(startPos, Tokenizer.Position, op1, right, null);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'if' in if comprehension expression!");
        }
        
        public ExpressionNode ParseYieldExpr()
        {
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyYield)
            {
                var startPos = Tokenizer.Position;
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                ExpressionNode right = null;
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyFrom)
                {
                    var op2 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    right = ParseTest();
                    return new YieldExpression(startPos, Tokenizer.Position, op1, op2, right);
                }
                right = ParseTestListStarExpr();
                return new YieldExpression(startPos, Tokenizer.Position, op1, null, right);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'yield' in yield expression!");;
        }
        
        public ExpressionNode ParseTestListStarExpr()
        {
            var startPos = Tokenizer.Position;
            var node = (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMul) ? ParseStarExpr() : ParseTest();
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
            {
                var nodes = new List<ExpressionNode>();
                var separators = new List<Token>();
                nodes.Add(node);
                while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                {
                    separators.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPlusAssign ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAssign ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyColon ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMinusAssign ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMulAssign ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPowerAssign ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyDivAssign ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyFloorDivAssign ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyModuloAssign ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMatriceAssign ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAndAssign ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyOrAssign ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyXorAssign ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyShiftLeftAssign ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyShiftRightAssign
                    )
                        continue;
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Double ',' in expression list!");
                    nodes.Add((Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMul) ? ParseStarExpr() : ParseTest());
                }
                return new ListExpression(startPos, Tokenizer.Position, ListExpression.ListType.TestListStarExpr , nodes.ToArray(), separators.ToArray());
            }
            return node;
        }
        
        
        
        
    }
}