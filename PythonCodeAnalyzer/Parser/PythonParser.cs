using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using PythonCodeAnalyzer.Parser.Ast;
using PythonCodeAnalyzer.Parser.Ast.Expression;
using PythonCodeAnalyzer.Parser.Ast.Statement;

namespace PythonCodeAnalyzer.Parser
{
    public class PythonParser
    {
        public PythonTokenizer Tokenizer { get; set; }


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
                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting ')' in tuple declaration!");
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
                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                        "Expecting ']' in tuple declaration!");
                }
                case Token.TokenKind.PyLeftCurly:
                {
                    var op1 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    ExpressionNode right = null;
                    bool isDictionary = true;
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyRightCurly)
                    {
                        var op2 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        return new DictionaryExpression(startPos, Tokenizer.Position, op1, right, op2);
                    }
                    (right, isDictionary) = ParseDictorSetMaker();
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyRightCurly)
                    {
                        var op2 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        return isDictionary ? (ExpressionNode) new DictionaryExpression(startPos, Tokenizer.Position, op1, right, op2) : new SetExpression(startPos, Tokenizer.Position, op1, right, op2);
                    }
                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
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
            var nodes = new List<ExpressionNode>();
            var startPos = Tokenizer.Position;
            while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyLeftParen ||
                   Tokenizer.CurSymbol.Kind == Token.TokenKind.PyLeftBracket ||
                   Tokenizer.CurSymbol.Kind == Token.TokenKind.PyDot)
            {
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyLeftParen)
                {
                    var op1 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    ExpressionNode right = null;
                    if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyRightParen)
                    {
                        right = ParseArgList();
                    }
                    if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyRightParen) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting ')'");
                    var op2 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    nodes.Add(new PythonCodeAnalyzer.Parser.Ast.Expression.CallExpression(startPos, Tokenizer.Position, op1, right, op2));
                }
                else if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyLeftBracket)
                {
                    var op1 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    ExpressionNode right = null;
                    //if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyRightBracket)
                    //{
                        right = ParseSubscriptList();
                    //}
                    if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyRightBracket) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting ']'");
                    var op2 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    nodes.Add( new PythonCodeAnalyzer.Parser.Ast.Expression.IndexExpression(startPos, Tokenizer.Position, op1, right, op2) );
                }
                else
                {
                    var op1 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind != Token.TokenKind.Name) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting name literal after '.'");
                    var op2 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    nodes.Add(new DotNameExpression(startPos, Tokenizer.Position, op1, op2));
                }
            }
            return nodes.ToArray();
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
                        res = new ShiftExpression(startPos, Tokenizer.Position, ShiftExpression.OperatorKind.Left, res, op, right);
                    }
                        break;
                    case Token.TokenKind.PyShiftRight:
                    {
                        var op = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        var right = ParseArithExpr();
                        res = new ShiftExpression(startPos, Tokenizer.Position, ShiftExpression.OperatorKind.Right, res, op, right);
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
                                res = new RelationExpression(startPos, Tokenizer.Position, RelationExpression.Relation.Is , res, op, null, right);
                            }
                        }
                        break;
                    case Token.TokenKind.PyNot:
                        {
                            var op = Tokenizer.CurSymbol;
                            Tokenizer.Advance();
                            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyIn)
                            {
                                var op2 = Tokenizer.CurSymbol;
                                Tokenizer.Advance();
                                var right = ParseOrExpr();
                                res = new RelationExpression(startPos, Tokenizer.Position, RelationExpression.Relation.NotIn , res, op, op2, right);
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

            return ParseComparison();
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
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyLambda)
            {
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                ExpressionNode left = null;
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyColon) left = ParseVarArgsList();
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyColon) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting ':' in lambda expression!");
                var op2 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                ExpressionNode right = (isConditional) ? ParseTest() : ParseNoCond();
                return new LambdaExpression(startPos, Tokenizer.Position, isConditional, op1, left, op2, right);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'lambda' in lambda expression!");
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
                return new ListExpression(startPos, Tokenizer.Position, ListExpression.ListType.TestListStarExpr , nodes.ToArray(), null);
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
                return new ListExpression(startPos, Tokenizer.Position, ListExpression.ListType.TestListStarExpr , nodes.ToArray(), separators.ToArray());
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
                return new ListExpression(startPos, Tokenizer.Position, ListExpression.ListType.ExprList, nodes.ToArray(), separators.ToArray());
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
            var startPos = Tokenizer.Position;
            var Keys = new List<ExpressionNode>();
            var Colons = new List<Token>();
            var Values = new List<ExpressionNode>();
            var Separators = new List<Token>();
            bool isDictionary = true;
            
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMul)
            {
                isDictionary = false;
                Keys.Add(ParseStarExpr());
            }
            else if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPower)
            {
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseOrExpr();
                Keys.Add(new PowerKeyExpression(startPos, Tokenizer.Position, op1, right));
                Colons.Add(null);
                Values.Add(null);
            }
            else
            {
                Keys.Add(ParseTest());
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyColon)
                {
                    Colons.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                    Values.Add(ParseTest());
                }
                else
                {
                    isDictionary = false;
                }
            }

            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyFor ||
                Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAsync)
            {
                Keys.Add(ParseCompFor());
            }
            else
            {
                while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                {
                    Separators.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyRightCurly) break;

                    if (isDictionary)
                    {
                        if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPower)
                        {
                            var op1 = Tokenizer.CurSymbol;
                            Tokenizer.Advance();
                            var right = ParseOrExpr();
                            Keys.Add(new PowerKeyExpression(startPos, Tokenizer.Position, op1, right));
                            Colons.Add(null);
                            Values.Add(null);
                        }
                        else
                        {
                            Keys.Add(ParseTest());
                            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyColon)
                            {
                                Colons.Add(Tokenizer.CurSymbol);
                                Tokenizer.Advance();
                                Values.Add(ParseTest());
                            }
                            else
                            {
                                throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting ':' in dictionary entry!");
                            }
                        }
                    }
                    else
                    {
                        if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMul)
                        {
                            Keys.Add(ParseStarExpr());
                        }
                        else
                        {
                            Keys.Add(ParseTest());
                        }
                    }
                    
                }
            }
            
            return (isDictionary) 
                ? new Tuple<ExpressionNode, bool>(new DictionaryContainerExpression(startPos, Tokenizer.Position, Keys.ToArray(), Colons.ToArray(), Values.ToArray(), Separators.ToArray()), true) 
                : new Tuple<ExpressionNode, bool>(new SetContainerExpression(startPos, Tokenizer.Position, Keys.ToArray(), Separators.ToArray()), false);
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

            return ParseSyncCompFor();
        }
        
        public ExpressionNode ParseCompIf()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyIf)
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

        public ExpressionNode ParseArgumentExpression()
        {
            var startPos = Tokenizer.Position;
            var left = ParseVFPDef();
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAssign)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseTest();
                return new VarArgumentExpression(startPos, Tokenizer.Position, left, op, right);
            }

            return left;
        }
        
        public ExpressionNode ParseVarArgsList()
        {
            var startPos = Tokenizer.Position;
            var elements = new List<ExpressionNode>();
            var separators = new List<Token>();
            Token div = null;
            
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMul)
            {
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var rigthMul = ParseArgumentExpression();
                Token op2 = null;
                ExpressionNode rightPower = null;
                
                elements.Add(new StarExpression(startPos, Tokenizer.Position, op1, rigthMul));
                
                while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                {
                    separators.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                        throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                            "Unexpected ',' in argument list!");
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyColon) continue;
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPower)
                    {
                        op2 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        rightPower = ParseArgumentExpression();
                        if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                        {
                            separators.Add(Tokenizer.CurSymbol);
                            Tokenizer.Advance();
                        }
                        if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                                "Unexpected ',' in argument list!");
                        elements.Add(new VarArgumentKWExpression(startPos, Tokenizer.Position, op2, rightPower));
                        continue;
                    }
                    elements.Add(ParseArgumentExpression());
                }
                
                return new VarArgsListExpression(startPos, Tokenizer.Position, elements.ToArray(), separators.ToArray(), div);
            }
            else if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPower)
            {
                var op2 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var rightPower = ParseArgumentExpression();
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                {
                    separators.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                }
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                        "Unexpected ',' in argument list!");
                elements.Add(new VarArgumentKWExpression(startPos, Tokenizer.Position, op2, rightPower));
                return new VarArgsListExpression(startPos, Tokenizer.Position, elements.ToArray(), separators.ToArray(), null);
            }
            else
            {
                elements.Add(ParseArgumentExpression());
                while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                {
                    separators.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                        throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                            "Unexpected ',' in argument list!");
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyDiv ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyColon ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMul ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPower) continue;
                    elements.Add(ParseArgumentExpression());
                }

                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyDiv)
                {
                    div = Tokenizer.CurSymbol;
                    Tokenizer.Advance();

                    Token lastSymbol = div;
                    
                    while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                    {
                        separators.Add(Tokenizer.CurSymbol);
                        lastSymbol = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                                "Unexpected ',' in argument list!");
                        if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMul ||
                            Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPower) continue;
                        elements.Add(ParseArgumentExpression());
                        if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyComma)
                        {
                            return new VarArgsListExpression(startPos, Tokenizer.Position, elements.ToArray(), separators.ToArray(), div);
                        }
                    }
                    
                    if (lastSymbol.Kind != Token.TokenKind.PyComma) 
                        throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                        "Expected ',' in argument list!");
                    
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMul)
                    {
                        var op1 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        var rigthMul = ParseArgumentExpression();
                        Token op2 = null;
                        ExpressionNode rightPower = null;
                        
                        elements.Add(new StarExpression(startPos, Tokenizer.Position, op1, rigthMul));
                        
                        while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                        {
                            separators.Add(Tokenizer.CurSymbol);
                            Tokenizer.Advance();
                            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                                throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                                    "Unexpected ',' in argument list!");
                            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyColon) continue;
                            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPower)
                            {
                                op2 = Tokenizer.CurSymbol;
                                Tokenizer.Advance();
                                rightPower = ParseArgumentExpression();
                                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                                {
                                    separators.Add(Tokenizer.CurSymbol);
                                    Tokenizer.Advance();
                                }
                                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                                        "Unexpected ',' in argument list!");
                                elements.Add(new VarArgumentKWExpression(startPos, Tokenizer.Position, op2, rightPower));
                                continue;
                            }
                            elements.Add(ParseArgumentExpression());
                        }
                        
                        return new VarArgsListExpression(startPos, Tokenizer.Position, elements.ToArray(), separators.ToArray(), div);
                    }
                    else if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPower)
                    {
                        var op2 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        var rightPower = ParseArgumentExpression();
                        if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                        {
                            separators.Add(Tokenizer.CurSymbol);
                            Tokenizer.Advance();
                        }
                        if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                                "Unexpected ',' in argument list!");
                        elements.Add(new VarArgumentKWExpression(startPos, Tokenizer.Position, op2, rightPower));
                        return new VarArgsListExpression(startPos, Tokenizer.Position, elements.ToArray(), separators.ToArray(), null);
                    }
                }
                return new VarArgsListExpression(startPos, Tokenizer.Position, elements.ToArray(), separators.ToArray(), null);
            }
        }

        public ExpressionNode ParseVFPDef()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.Name)
            {
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                return new VFPDefExpression(startPos, Tokenizer.Position, op1);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting name literal in argument!");
        }

        public StatementNode ParseCompoundStmt()
        {
            switch (Tokenizer.CurSymbol.Kind)
            {
                case Token.TokenKind.PyIf:
                    return ParseIfStmt();
                case Token.TokenKind.PyFor:
                    return ParseForStmt();
                case Token.TokenKind.PyWhile:
                    return ParseWhileStmt();
                case Token.TokenKind.PyTry:
                    return ParseTryStmt();
                case Token.TokenKind.PyWith:
                    return ParseWithStmt();
                case Token.TokenKind.PyDef:
                    return ParseAsyncFuncDef();
                case Token.TokenKind.PyClass:
                    return ParseClassDeclaration();
                case Token.TokenKind.PyMatrice:
                    return ParseDecorated();
                case Token.TokenKind.PyAsync:
                    return ParseAsyncFuncDef();
                default:
                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Illegal statement!");
            }
        }
        
        public StatementNode ParseAsyncStmt()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAsync)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                StatementNode right = null;
                switch (Tokenizer.CurSymbol.Kind)
                {
                    case Token.TokenKind.PyDef:
                        right = ParseFuncDefDeclaration();
                        return new AsyncStatement(startPos, Tokenizer.Position, op, right);
                    case Token.TokenKind.PyWith:
                        right = ParseWithStmt();
                        return new AsyncStatement(startPos, Tokenizer.Position, op, right);
                    case Token.TokenKind.PyFor:
                        right = ParseForStmt();
                        return new AsyncStatement(startPos, Tokenizer.Position, op, right);
                    default:
                        throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'def', 'with' or 'for' in async statement!");
                }
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'async' in async statement!");
        }
        
        public StatementNode ParseIfStmt()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyIf)
            {
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var left = ParseNamedExpr();
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyColon) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting ':' in if statement!");
                var op2 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseSuite();
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyElif &&
                    Tokenizer.CurSymbol.Kind != Token.TokenKind.PyElse)
                {
                    return new IfStatement(startPos, Tokenizer.Position, op1, left, op2, right, null, null);
                }
                
                var elifElements = new List<StatementNode>();
                StatementNode elseElement = null;
                while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyElif)
                {
                    var op3 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var first = ParseNamedExpr();
                    if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyColon) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting ':' in elif statement!");
                    var op4 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var second = ParseSuite();
                    elifElements.Add(new ElifStatement(startPos, Tokenizer.Position, op3, first, op4, second));
                }

                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyElse) elseElement = ParseElseStmt();
                return new IfStatement(startPos, Tokenizer.Position, op1, left, op2, right, elifElements.ToArray(), elseElement);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'if' in if statement!");
        }
        
        public StatementNode ParseElseStmt()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyElse)
            {
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyColon) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting ':' in else statement!");
                var op2 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseSuite();
                return new ElseStatement(startPos, Tokenizer.Position, op1, op2, right);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'else' in else statement!");
        }
        
        public StatementNode ParseWhileStmt()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyWhile)
            {
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var left = ParseNamedExpr();
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyColon) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting ':' in while statement!");
                var op2 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseSuite();
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyElse)
                {
                    var elseElement = ParseElseStmt();
                    return new WhileStatement(startPos, Tokenizer.Position, op1, left, op2, right, elseElement);
                }

                return new WhileStatement(startPos, Tokenizer.Position, op1, left, op2, right, null);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'while' in while statement!");
        }
        
        public StatementNode ParseForStmt()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyFor)
            {
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var left = ParseExprList();
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyIn) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'in' in for statement!");
                var op2 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseTestList();
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyColon) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting ':' in for statement!");
                var op3 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                Token typeComment = null;
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.TypeComment)
                {
                    typeComment = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                }

                var next = ParseSuite();
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyElse)
                {
                    var elseElement = ParseElseStmt();
                    return new ForStatement(startPos, Tokenizer.Position, op1, left, op2, right, op3, typeComment, next, elseElement);
                }
                
                return new ForStatement(startPos, Tokenizer.Position, op1, left, op2, right, op3, typeComment, next, null);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'for' in for statement!");
        }
        
        public StatementNode ParseTryStmt()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyTry)
            {
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyColon) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting ':' in try statement!");
                var op2 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var left = ParseSuite();
                bool needFinally = true;
                var excepts = new List<StatementNode>();
                StatementNode elsePart = null;
                StatementNode finallyPart = null;
                
                while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyExcept)
                {
                    excepts.Add(ParseExceptStmt());
                    needFinally = false;
                }

                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyElse)
                {
                    elsePart = ParseElseStmt();
                }
                
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyFinally && needFinally) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'finally' in try statement missing except statements!");
                
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyFinally)
                {
                    var op3 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyColon) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting ':' in finally part of try statement!");
                    var op4 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var next = ParseSuite();
                    finallyPart = new FinallyStatement(startPos, Tokenizer.Position, op3, op4, next);
                }
                return new TryStatement(startPos, Tokenizer.Position, op1, op2, left, excepts.ToArray(), elsePart, finallyPart);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'try' in try statement!");
        }
        
        public StatementNode ParseWithStmt()
        {
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyWith)
            {
                var startPos = Tokenizer.Position;
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var nodes = new List<StatementNode>();
                var separators = new List<Token>();
                Token typeComment = null;
                nodes.Add(ParseWithItem());
                while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                {
                    separators.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                    nodes.Add(ParseWithItem());
                }

                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyColon)
                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                        "Expecting ':' in with statement!");
                var colon = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.TypeComment)
                {
                    typeComment = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                }

                var right = ParseSuite();
                return new WithStatement(startPos, Tokenizer.Position, op1, nodes.ToArray(), separators.ToArray(),
                    colon, typeComment, right);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                "Expecting 'with' in with statement!");
        }
        
        public StatementNode ParseWithItem()
        {
            var startPos = Tokenizer.Position;
            var left = ParseTest();
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAs)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseOrExpr();
                return new WithItemStatement(startPos, Tokenizer.Position, left, op, right);
            }
            return new WithItemStatement(startPos, Tokenizer.Position, left, null, null);
        }
        
        public StatementNode ParseExceptStmt()
        {
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyExcept)
            {
                var startPos = Tokenizer.Position;
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                ExpressionNode left = null;
                Token op2 = null;
                Token op3 = null;
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyColon)
                {
                    left = ParseTest();
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAs)
                    {
                        op2 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        if (Tokenizer.CurSymbol.Kind != Token.TokenKind.Name)
                            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                                "Expecting name literal in except statement after 'as'!");
                        op3 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                    }
                }

                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyColon)
                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                        "Expecting ':' in except statement!");
                var op4 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseSuite();
                return new ExceptStatement(startPos, Tokenizer.Position, op1, left, op2, op3, op4, right);
            }

            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                "Expecting 'except' in except statement!");
        }
        
        public StatementNode ParseSuite()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.Newline)
            {
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.Indent)
                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                        "Expecting indent in block level of statement block!");
                var op2 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var nodes = new List<StatementNode>();
                nodes.Add(ParseStmt());
                while (Tokenizer.CurSymbol.Kind != Token.TokenKind.Dedent)
                {
                    nodes.Add(ParseStmt());
                }
                var op3 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                return new SuiteStatement(startPos, Tokenizer.Position, op1, op2, nodes.ToArray(), op3);
            }

            return ParseSimpleStmt();
        }
        
        public StatementNode ParseClassDeclaration()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyClass)
            {
                var op1 = Tokenizer.CurSymbol; // 'class'
                Tokenizer.Advance();
                
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.Name)
                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                        "Expecting name of class declaration!");
                var name = Tokenizer.CurSymbol;
                Tokenizer.Advance();

                Token op2 = null, op3 = null;
                ExpressionNode left = null;
                
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyLeftParen)
                {
                    op2 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyRightParen) left = ParseArgList();
                    if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyRightParen)
                        throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                            "Expecting ')' in class declaration!");
                    op3 = Tokenizer.CurSymbol; 
                    Tokenizer.Advance();
                }
                
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyColon)
                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                        "Expecting ':' in class declaration!");
                var op4 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseSuite();
                return new ClassDeclarationStatement(startPos, Tokenizer.Position, op1, name, op2, left, op3, op4, right);
            }

            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                "Expecting 'class' in class declaration!");
        }
        
        public StatementNode ParseStmt()
        {
            switch (Tokenizer.CurSymbol.Kind)
            {
                case Token.TokenKind.PyIf:
                case Token.TokenKind.PyFor:
                case Token.TokenKind.PyWhile:
                case Token.TokenKind.PyTry:
                case Token.TokenKind.PyWith:
                case Token.TokenKind.PyDef:
                case Token.TokenKind.PyClass:
                case Token.TokenKind.PyMatrice:
                case Token.TokenKind.PyAsync:
                    return ParseCompoundStmt();
                default:
                    return ParseSimpleStmt();
            }
        }
        
        public StatementNode ParseSimpleStmt()
        {
            var startPos = Tokenizer.Position;
            var nodes = new List<StatementNode>();
            var separators = new List<Token>();
            Token newline = null;
            nodes.Add(ParseSmallStmt());
            while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PySemiColon)
            {
                separators.Add(Tokenizer.CurSymbol);
                Tokenizer.Advance();
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.Newline) break;
                nodes.Add(ParseSmallStmt());
            }
            if (Tokenizer.CurSymbol.Kind != Token.TokenKind.Newline) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting Newline after statement!");
            newline = Tokenizer.CurSymbol;
            Tokenizer.Advance();
            return new ListStatement(startPos, Tokenizer.Position, ListStatement.ListKind.SimpleStatementList, nodes.ToArray(), separators.ToArray(), newline);
        }
        
        public StatementNode ParseSmallStmt()
        {
            switch (Tokenizer.CurSymbol.Kind)
            {
                case Token.TokenKind.PyDel:
                    return ParseDelStmt();
                case Token.TokenKind.PyPass:
                    return ParsePassStmt();
                case Token.TokenKind.PyBreak:
                case Token.TokenKind.PyContinue:
                case Token.TokenKind.PyReturn:
                case Token.TokenKind.PyRaise:
                case Token.TokenKind.PyYield:
                    return ParseFlowStmt();
                case Token.TokenKind.PyImport:
                case Token.TokenKind.PyFrom:
                    return ParseImportStmt();
                case Token.TokenKind.PyGlobal:
                    return ParseGlobalStmt();
                case Token.TokenKind.PyNonlocal:
                    return ParseNonLocalStmt();
                case Token.TokenKind.PyAssert:
                    return ParseAssertStmt();
                default:
                    return ParseExprStmt();
            }
        }
        
        public StatementNode ParseExprStmt()
        {
            var startPos = Tokenizer.Position;
            var left = ParseTestListStarExpr();
            switch (Tokenizer.CurSymbol.Kind)
            {
                case Token.TokenKind.PyPlusAssign:
                {
                    var op = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var right = (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyYield)
                        ? ParseYieldExpr()
                        : ParseTestList();
                    return new AugAssignStatement(startPos, Tokenizer.Position, AugAssignStatement.OperatorKind.PlusAssign, left, op, right);
                }
                case Token.TokenKind.PyMinusAssign:
                {
                    var op = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var right = (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyYield)
                        ? ParseYieldExpr()
                        : ParseTestList();
                    return new AugAssignStatement(startPos, Tokenizer.Position, AugAssignStatement.OperatorKind.MinusAssign, left, op, right);
                }
                case Token.TokenKind.PyMulAssign:
                {
                    var op = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var right = (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyYield)
                        ? ParseYieldExpr()
                        : ParseTestList();
                    return new AugAssignStatement(startPos, Tokenizer.Position, AugAssignStatement.OperatorKind.MulAssign, left, op, right);
                }
                case Token.TokenKind.PyPowerAssign:
                {
                    var op = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var right = (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyYield)
                        ? ParseYieldExpr()
                        : ParseTestList();
                    return new AugAssignStatement(startPos, Tokenizer.Position, AugAssignStatement.OperatorKind.PowerAssign, left, op, right);
                }
                case Token.TokenKind.PyDivAssign:
                {
                    var op = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var right = (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyYield)
                        ? ParseYieldExpr()
                        : ParseTestList();
                    return new AugAssignStatement(startPos, Tokenizer.Position, AugAssignStatement.OperatorKind.DivAssign, left, op, right);
                }
                case Token.TokenKind.PyFloorDivAssign:
                {
                    var op = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var right = (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyYield)
                        ? ParseYieldExpr()
                        : ParseTestList();
                    return new AugAssignStatement(startPos, Tokenizer.Position, AugAssignStatement.OperatorKind.FloorDivAssign, left, op, right);
                }
                case Token.TokenKind.PyModuloAssign:
                {
                    var op = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var right = (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyYield)
                        ? ParseYieldExpr()
                        : ParseTestList();
                    return new AugAssignStatement(startPos, Tokenizer.Position, AugAssignStatement.OperatorKind.ModuloAssign, left, op, right);
                }
                case Token.TokenKind.PyMatriceAssign:
                {
                    var op = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var right = (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyYield)
                        ? ParseYieldExpr()
                        : ParseTestList();
                    return new AugAssignStatement(startPos, Tokenizer.Position, AugAssignStatement.OperatorKind.MatriceAssign, left, op, right);
                }
                case Token.TokenKind.PyAndAssign:
                {
                    var op = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var right = (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyYield)
                        ? ParseYieldExpr()
                        : ParseTestList();
                    return new AugAssignStatement(startPos, Tokenizer.Position, AugAssignStatement.OperatorKind.AndAssign, left, op, right);
                }
                case Token.TokenKind.PyOrAssign:
                {
                    var op = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var right = (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyYield)
                        ? ParseYieldExpr()
                        : ParseTestList();
                    return new AugAssignStatement(startPos, Tokenizer.Position, AugAssignStatement.OperatorKind.OrAssign, left, op, right);
                }
                case Token.TokenKind.PyXorAssign:
                {
                    var op = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var right = (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyYield)
                        ? ParseYieldExpr()
                        : ParseTestList();
                    return new AugAssignStatement(startPos, Tokenizer.Position, AugAssignStatement.OperatorKind.XorAssign, left, op, right);
                }
                case Token.TokenKind.PyShiftLeftAssign:
                {
                    var op = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var right = (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyYield)
                        ? ParseYieldExpr()
                        : ParseTestList();
                    return new AugAssignStatement(startPos, Tokenizer.Position, AugAssignStatement.OperatorKind.ShiftLeftAssign, left, op, right);
                }
                case Token.TokenKind.PyShiftRightAssign:
                {
                    var op = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var right = (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyYield)
                        ? ParseYieldExpr()
                        : ParseTestList();
                    return new AugAssignStatement(startPos, Tokenizer.Position, AugAssignStatement.OperatorKind.ShiftRightAssign, left, op, right);
                }
                case Token.TokenKind.PyColon:
                    return ParseAnnAssign(startPos, left);
                case Token.TokenKind.PyAssign:
                {
                    var rightNodes = new List<ExpressionNode>();
                    var operators = new List<Token>();
                    
                    while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAssign)
                    {
                        operators.Add(Tokenizer.CurSymbol);
                        Tokenizer.Advance();
                        var right = (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyYield)
                            ? ParseYieldExpr()
                            : ParseTestListStarExpr();
                        rightNodes.Add(right);
                    }
                    
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.TypeComment)
                    {
                        var op2 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        return new AssignmentStatement(startPos, Tokenizer.Position, left, operators, rightNodes, op2);
                    }
                    return new AssignmentStatement(startPos, Tokenizer.Position, left, operators, rightNodes, null);
                }
                default:
                    return new PlainExpressionStatement(startPos, Tokenizer.Position, left);
            }
        }
        
        public StatementNode ParseAnnAssign(uint start, ExpressionNode left)
        {
            if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyColon) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting ':' in AnnAssign Statement!");
            var colon = Tokenizer.CurSymbol;
            Tokenizer.Advance();
            var type = ParseTest();
            Token assign = null;
            ExpressionNode right = null;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAssign)
            {
                assign = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                right = (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyYield)
                    ? ParseYieldExpr()
                    : ParseTestListStarExpr();
            }
            return new AnnAssignStatement(start, Tokenizer.Position, left, colon, type, assign, right);
        }
        
        public StatementNode ParseDelStmt()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyDel)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseExprList();
                return new DelStatement(startPos, Tokenizer.Position, op, right);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'del' in del statement!");
        }
        
        public StatementNode ParsePassStmt()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPass)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                return new PassStatement(startPos, Tokenizer.Position, op);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'pass' in pass statement!");
        }
        
        public StatementNode ParseFlowStmt()
        {
            var startPos = Tokenizer.Position;
            StatementNode res = null;
            // Add check for valid place to do flow statements TODO!
            switch (Tokenizer.CurSymbol.Kind)
            {
                case Token.TokenKind.PyBreak:
                    res = ParseBreakStmt();
                    break;
                case Token.TokenKind.PyContinue:
                    res = ParseContinueStmt();
                    break;
                case Token.TokenKind.PyReturn:
                    res = ParseReturnStmt();
                    break;
                case Token.TokenKind.PyRaise:
                    res = ParseRaiseStmt();
                    break;
                case Token.TokenKind.PyYield:
                {
                    var node = ParseYieldExpr();
                    res = new PlainExpressionStatement(startPos, Tokenizer.Position, node);
                    break;
                }
                default:
                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting valid flow statement!");
            }
            return res;
        }
        
        public StatementNode ParseBreakStmt()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyBreak)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                return new FlowStatement(startPos, Tokenizer.Position, FlowStatement.OperatorKind.Break, op);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'break' in flow statement!");
        }
        
        public StatementNode ParseContinueStmt()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyContinue)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                return new FlowStatement(startPos, Tokenizer.Position, FlowStatement.OperatorKind.Continue, op);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'continue' in flow statement!");
        }
        
        public StatementNode ParseReturnStmt()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyReturn)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PySemiColon &&
                    Tokenizer.CurSymbol.Kind != Token.TokenKind.Newline)
                {
                    var right = ParseTestListStarExpr();
                    return new FlowStatement(startPos, Tokenizer.Position, FlowStatement.OperatorKind.Return, op, right);
                }
                return new FlowStatement(startPos, Tokenizer.Position, FlowStatement.OperatorKind.Return, op);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'return' in flow statement!");
        }
        
        public StatementNode ParseRaiseStmt()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyRaise)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PySemiColon &&
                    Tokenizer.CurSymbol.Kind != Token.TokenKind.Newline)
                {
                    var left = ParseTest();
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyFrom)
                    {
                        var op2 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        var right = ParseTest();
                        return new FlowStatement(startPos, Tokenizer.Position, FlowStatement.OperatorKind.Raise, op, left, op2, right);
                    }
                    return new FlowStatement(startPos, Tokenizer.Position, FlowStatement.OperatorKind.Raise, op, left, null, null);
                }
                return new FlowStatement(startPos, Tokenizer.Position, FlowStatement.OperatorKind.Raise, op, null, null, null);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'raise' in flow statement!");
        }
        
        public StatementNode ParseImportStmt()
        {
            switch (Tokenizer.CurSymbol.Kind)
            {
                case Token.TokenKind.PyImport:
                    return ParseImportName();
                case Token.TokenKind.PyFrom:
                    return ParseImportFrom();
                default:
                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'import' or 'from' in import statement!");
            }
        }
        
        public StatementNode ParseImportName()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyImport)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseDottedAsNames();
                return new ImportStatement(startPos, Tokenizer.Position, op, right);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'import' in import statement!");
        }
        
        public StatementNode ParseImportFrom()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyFrom)
            {
                var op1 = Tokenizer.CurSymbol; // 'from'
                Tokenizer.Advance();
                var dots = new List<Token>();
                while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyDot || Tokenizer.CurSymbol.Kind == Token.TokenKind.PyElipsis)
                {
                    dots.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                }
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyImport && dots.Count == 0)
                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting '.' or dotted name before 'import'!");
                var left = (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyImport) ? ParseDottedName() : null;
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyImport) 
                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'import' in from import statement!");
                var op2 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                Token op3 = null, op4 = null;
                StatementNode right = null;
                switch (Tokenizer.CurSymbol.Kind)
                {
                    case Token.TokenKind.PyMul:
                        op3 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        break;
                    case Token.TokenKind.PyLeftParen:
                        op3 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        right = ParseImportAsNames();
                        if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyRightParen) 
                            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting ')' in from import statement!");
                        op4 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        break;
                    default:
                        right = ParseImportAsNames();
                        break;
                }
                
                return new ImportFromStatement(startPos, Tokenizer.Position, op1, dots.ToArray(), left, op2, op3, right, op4);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'from' in import statement!");
        }
        
        public StatementNode ParseImportAsName()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.Name)
            {
                var left = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAs)
                {
                    var op = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind != Token.TokenKind.Name) 
                        throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting Name literal!");
                    var right = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    return new ImportAsNameStatement(startPos, Tokenizer.Position, left, op, right);
                }
                return new ImportAsNameStatement(startPos, Tokenizer.Position, left, null, null);
            } 
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting Name literal!");
        }
        
        public StatementNode ParseImportAsNames()
        {
            var startPos = Tokenizer.Position;
            var elements = new List<StatementNode>();
            var commas = new List<Token>();
            elements.Add(ParseImportAsName());
            while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
            {
                commas.Add(Tokenizer.CurSymbol);
                Tokenizer.Advance();
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyRightParen &&
                    Tokenizer.CurSymbol.Kind != Token.TokenKind.Newline &&
                    Tokenizer.CurSymbol.Kind != Token.TokenKind.PySemiColon)
                {
                    elements.Add(ParseImportAsName());
                }
            }
            return new ImportListStatement(startPos, Tokenizer.Position, ImportListStatement.ListKind.ImportAsNames, elements.ToArray(), commas.ToArray());
        }
        
        public StatementNode ParseDottedAsName()
        {
            var startPos = Tokenizer.Position;
            var left = ParseDottedName();
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAs)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.Name) 
                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting Name literal!");
                var right = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                return new DottedAsNameStatement(startPos, Tokenizer.Position, left, op, right);
            }

            return left;
        }
        
        public StatementNode ParseDottedAsNames()
        {
            var startPos = Tokenizer.Position;
            var elements = new List<StatementNode>();
            var commas = new List<Token>();
            elements.Add(ParseDottedAsName());
            while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
            {
                commas.Add(Tokenizer.CurSymbol);
                Tokenizer.Advance();
                elements.Add(ParseDottedAsName());
            }
            return new ImportListStatement(startPos, Tokenizer.Position, ImportListStatement.ListKind.DottedAsNames, elements.ToArray(), commas.ToArray());
        }
        
        public StatementNode ParseDottedName()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.Name)
            {
                var names = new List<Token>();
                var dots = new List<Token>();
                names.Add(Tokenizer.CurSymbol);
                Tokenizer.Advance();
                while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyDot)
                {
                    dots.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind != Token.TokenKind.Name) 
                        throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting Name literal!");
                    names.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                }
                return new DottedNameStatement(startPos, Tokenizer.Position, names.ToArray(), dots.ToArray());
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting Name literal!");
        }
        
        public StatementNode ParseGlobalStmt()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyGlobal)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var nodes = new List<Token>();
                var separators = new List<Token>();
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.Name) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting name literal in global statement!");
                nodes.Add(Tokenizer.CurSymbol);
                Tokenizer.Advance();
                while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                {
                    separators.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind != Token.TokenKind.Name) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting name literal after ',' in global statement!");
                    nodes.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                }
                return new ScopeStatement(startPos, Tokenizer.Position, ScopeStatement.ScopeKind.Global, op, nodes.ToArray(), separators.ToArray());
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'global' in global statement!");
        }
        
        public StatementNode ParseNonLocalStmt()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyNonlocal)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var nodes = new List<Token>();
                var separators = new List<Token>();
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.Name) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting name literal in nonlocal statement!");
                nodes.Add(Tokenizer.CurSymbol);
                Tokenizer.Advance();
                while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                {
                    separators.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind != Token.TokenKind.Name) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting name literal after ',' in nonlocal statement!");
                    nodes.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                }
                return new ScopeStatement(startPos, Tokenizer.Position, ScopeStatement.ScopeKind.Nonlocal, op, nodes.ToArray(), separators.ToArray());
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'nonlocal' in global statement!");
        }
        
        public StatementNode ParseAssertStmt()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAssert)
            {
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var left = ParseTest();
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                {
                    var op2 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var right = ParseTest();
                    return new AssertStatement(startPos, Tokenizer.Position, op1, left, op2, right);
                }
                return new AssertStatement(startPos, Tokenizer.Position, op1, left, null, null);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'assert' in assert statement!");
        }
        
        public StatementNode ParseDecorator()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMatrice)
            {
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var left = ParseDottedName();
                Token op2 = null, op3 = null;
                ExpressionNode right = null;
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyLeftParen)
                {
                    op2 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyRightParen) right = ParseArgList();
                    if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyRightParen) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting ')' in decorator statement!");
                    op3 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                }
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.Newline) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting newline in decorator statement!");
                var op4 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                return new DecoratorStatement(startPos, Tokenizer.Position, op1, left, op2, right, op3, op4);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting '@' in decorator statement!");
        }
        
        public StatementNode ParseDecorators()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMatrice)
            {
                var nodes = new List<StatementNode>();
                while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMatrice)
                {
                    nodes.Add(ParseDecorator());
                }
                return new ListStatement(startPos, Tokenizer.Position, ListStatement.ListKind.DecoratorList, nodes.ToArray(), null, null);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting '@' in decorator statement!");
        }
        
        public StatementNode ParseDecorated()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMatrice)
            {
                var left = ParseDecorators();
                StatementNode right = null;
                switch (Tokenizer.CurSymbol.Kind)
                {
                    case Token.TokenKind.PyClass:
                        right = ParseClassDeclaration();
                        break;
                    case Token.TokenKind.PyDef:
                        right = ParseFuncDefDeclaration();
                        break;
                    case Token.TokenKind.PyAsync:
                        right = ParseAsyncFuncDef();
                        break;
                    default:
                        throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'class', 'def' or 'async' in decorated statement!");
                }
                return new DecoratedStatement(startPos, Tokenizer.Position, left, right);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting '@' in decorator statement!");
        }
        
        public StatementNode ParseAsyncFuncDef()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAsync)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyDef) throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'def' in async statement!");
                var right = ParseFuncDefDeclaration();
                return new AsyncStatement(startPos, Tokenizer.Position, op, right);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'async' in async statement!");
        }
        
        public StatementNode ParseFuncDefDeclaration()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyDef)
            {
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.Name) 
                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting name literal in func statement!");
                var op2 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var left = ParseParameters();
                Token op3 = null, op4 = null, op5 = null;
                ExpressionNode right = null;
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyArrow)
                {
                    op3 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    right = ParseTest();
                }
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyColon) 
                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting ':' in func statement!");
                op4 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.TypeComment)
                {
                    op5= Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                }
                var next = ParseFuncBodySuite();
                return new FuncDeclarationStatement(startPos, Tokenizer.Position, op1, op2, left, op3, right, op4, op5, next);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting 'def' in func statement!");
        }
        
        public StatementNode ParseParameters()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyLeftParen)
            {
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                StatementNode right = null;
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyRightParen) right = ParseTypedArgsList();
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyDef) 
                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting ')' in parameter of func statement!");
                var op2 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                return new ParameterStatement(startPos, Tokenizer.Position, op1, right, op2);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting '(' in parameter of func statement!");
        }
        
        public StatementNode ParseArgumentStatement()
        {
            var startPos = Tokenizer.Position;
            var left = ParseTFPDef();
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyAssign)
            {
                var op = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseTest();
                return new TypedArgumentStatement(startPos, Tokenizer.Position, left, op, right);
            }

            return left;
        }
        
        public StatementNode ParseTypedArgsList() // Add TypeComments and check syntax!
        {
            var startPos = Tokenizer.Position;
            var elements = new List<StatementNode>();
            var separators = new List<Token>();
            var typeComments = new List<Token>();
            Token div = null;
            
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMul)
            {
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var rigthMul = ParseArgumentStatement();
                Token op2 = null; 
                StatementNode rightPower = null;
                
                elements.Add(new TypedMulArgumentStatement(startPos, Tokenizer.Position, op1, rigthMul));
                
                while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                {
                    separators.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                        throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                            "Unexpected ',' in argument list!");
                    else if (Tokenizer.CurSymbol.Kind == Token.TokenKind.TypeComment)
                    {
                        typeComments.Add(Tokenizer.CurSymbol);
                        Tokenizer.Advance();
                    }
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyColon) continue;
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPower)
                    {
                        op2 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        rightPower = ParseArgumentStatement();
                        if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                        {
                            separators.Add(Tokenizer.CurSymbol);
                            Tokenizer.Advance();
                        }
                        if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                        {
                            separators.Add(Tokenizer.CurSymbol);
                            Tokenizer.Advance();
                            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.TypeComment)
                            {
                                typeComments.Add(Tokenizer.CurSymbol);
                                Tokenizer.Advance();
                            }
                        }
                        if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                                "Unexpected ',' in argument list!");
                        elements.Add(new TypedPowerArgumentStatement(startPos, Tokenizer.Position, op2, rightPower));
                        continue;
                    }
                    elements.Add(ParseArgumentStatement());
                }
                
                return new TypedArgsStatement(startPos, Tokenizer.Position, elements.ToArray(), separators.ToArray(), null, div);
            }
            else if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPower)
            {
                var op2 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var rightPower = ParseArgumentStatement();
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                {
                    separators.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                }

                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                {
                    separators.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.TypeComment)
                    {
                        typeComments.Add(Tokenizer.CurSymbol);
                        Tokenizer.Advance();
                    }
                }
                elements.Add(new TypedPowerArgumentStatement(startPos, Tokenizer.Position, op2, rightPower));
                return new TypedArgsStatement(startPos, Tokenizer.Position, elements.ToArray(), separators.ToArray(), null, null);
            }
            else
            {
                elements.Add(ParseArgumentStatement());
                while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                {
                    separators.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                        throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                            "Unexpected ',' in argument list!");
                    else if (Tokenizer.CurSymbol.Kind == Token.TokenKind.TypeComment)
                    {
                        typeComments.Add(Tokenizer.CurSymbol);
                        Tokenizer.Advance();
                    }
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyDiv ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyColon ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMul ||
                        Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPower) continue;
                    elements.Add(ParseArgumentStatement());
                }

                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyDiv)
                {
                    div = Tokenizer.CurSymbol;
                    Tokenizer.Advance();

                    Token lastSymbol = div;
                    
                    while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                    {
                        separators.Add(Tokenizer.CurSymbol);
                        lastSymbol = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                                "Unexpected ',' in argument list!");
                        else if (Tokenizer.CurSymbol.Kind == Token.TokenKind.TypeComment)
                        {
                            typeComments.Add(Tokenizer.CurSymbol);
                            Tokenizer.Advance();
                        }
                        if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMul ||
                            Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPower ||
                            Tokenizer.CurSymbol.Kind == Token.TokenKind.TypeComment) continue;
                        elements.Add(ParseArgumentStatement());
                        if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyComma)
                        {
                            return new TypedArgsStatement(startPos, Tokenizer.Position, elements.ToArray(), separators.ToArray(), null, div);
                        }
                    }
                    
                    if (lastSymbol.Kind != Token.TokenKind.PyComma) 
                        throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                        "Expected ',' in argument list!");
                    
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.TypeComment)
                    {
                        typeComments.Add(Tokenizer.CurSymbol);
                        Tokenizer.Advance();
                    }
                    
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMul)
                    {
                        var op1 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        var rigthMul = ParseArgumentStatement();
                        Token op2 = null;
                        StatementNode rightPower = null;
                        
                        elements.Add(new TypedMulArgumentStatement(startPos, Tokenizer.Position, op1, rigthMul));
                        
                        while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                        {
                            separators.Add(Tokenizer.CurSymbol);
                            Tokenizer.Advance();
                            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                                throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                                    "Unexpected ',' in argument list!");
                            else if (Tokenizer.CurSymbol.Kind == Token.TokenKind.TypeComment)
                            {
                                typeComments.Add(Tokenizer.CurSymbol);
                                Tokenizer.Advance();
                            }
                            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyColon) continue;
                            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPower)
                            {
                                op2 = Tokenizer.CurSymbol;
                                Tokenizer.Advance();
                                rightPower = ParseArgumentStatement();
                                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                                {
                                    separators.Add(Tokenizer.CurSymbol);
                                    Tokenizer.Advance();
                                }

                                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                                {
                                    separators.Add(Tokenizer.CurSymbol);
                                    Tokenizer.Advance();
                                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.TypeComment)
                                    {
                                        typeComments.Add(Tokenizer.CurSymbol);
                                        Tokenizer.Advance();
                                    }

                                    break;
                                }
                                elements.Add(new TypedPowerArgumentStatement(startPos, Tokenizer.Position, op2, rightPower));
                                continue;
                            }
                            elements.Add(ParseArgumentStatement());
                        }
                        
                        return new TypedArgsStatement(startPos, Tokenizer.Position, elements.ToArray(), separators.ToArray(), null, div);
                    }
                    else if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPower)
                    {
                        var op2 = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        var rightPower = ParseArgumentStatement();
                        if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                        {
                            separators.Add(Tokenizer.CurSymbol);
                            Tokenizer.Advance();
                        }
                        if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                                "Unexpected ',' in argument list!");
                        elements.Add(new TypedPowerArgumentStatement(startPos, Tokenizer.Position, op2, rightPower));
                        return new TypedArgsStatement(startPos, Tokenizer.Position, elements.ToArray(), separators.ToArray(), null, null);
                    }
                }
                return new TypedArgsStatement(startPos, Tokenizer.Position, elements.ToArray(), separators.ToArray(), null, null);
            }
        }
        
        public StatementNode ParseTFPDef()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.Name)
            {
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyColon)
                {
                    var op2 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    var right = ParseTest();
                    return new TFPDefStatement(startPos, Tokenizer.Position, op1, op2, right);
                }
                return new TFPDefStatement(startPos, Tokenizer.Position, op1, null, null);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, "Expecting name literal in argument!");
        }
        
        public StatementNode ParseSingleInput()
        {
            Token op = null;
            StatementNode right = null;
            Tokenizer.Advance();
            var startPos = Tokenizer.Position;
            
            switch (Tokenizer.CurSymbol.Kind)
            {
                case Token.TokenKind.PyIf:
                case Token.TokenKind.PyFor:
                case Token.TokenKind.PyWhile:
                case Token.TokenKind.PyTry:
                case Token.TokenKind.PyWith:
                case Token.TokenKind.PyDef:
                case Token.TokenKind.PyClass:
                case Token.TokenKind.PyMatrice:
                case Token.TokenKind.PyAsync:
                    right = ParseCompoundStmt();
                    if (Tokenizer.CurSymbol.Kind != Token.TokenKind.Newline) 
                        throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol, 
                            "Expecting newline after block statement!");
                    op = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    break;
                case Token.TokenKind.Newline:
                    op = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    break;
                default:
                    right = ParseSimpleStmt();
                    break;
            }
            return new SingleInputStatement(startPos, Tokenizer.Position, right, op);
        }
        
        public StatementNode ParseFileInput()
        {
            Tokenizer.Advance();
            var startPos = Tokenizer.Position;
            var newlines = new List<Token>();
            var nodes = new List<StatementNode>();
            
            while (Tokenizer.CurSymbol.Kind != Token.TokenKind.EOF)
            {
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.Newline)
                {
                    newlines.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                }
                else
                {
                    nodes.Add(ParseStmt());
                }
            }

            var eof = Tokenizer.CurSymbol; 
            
            return new FileInputStatement(startPos, Tokenizer.Position, newlines.ToArray(), nodes.ToArray(), eof);
        }
        
        public StatementNode ParseEvalInput()
        {
            Tokenizer.Advance();
            var startPos = Tokenizer.Position;
            var right = ParseTestList();
            var newlines = new List<Token>();
            while (Tokenizer.CurSymbol.Kind == Token.TokenKind.Newline)
            {
                newlines.Add(Tokenizer.CurSymbol);
                Tokenizer.Advance();
            }
            if (Tokenizer.CurSymbol.Kind != Token.TokenKind.EOF) 
                throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                    "Expecting end of file!");
            return new EvalInputStatement(startPos, Tokenizer.Position, right, newlines.ToArray(), Tokenizer.CurSymbol);
        }

        public StatementNode ParseFuncBodySuite()
        {
            var startPos = Tokenizer.Position;
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.Newline)
            {
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                Token op2 = null, op3 = null;
                if (Tokenizer.CurSymbol.Kind == Token.TokenKind.TypeComment)
                {
                    op2 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind != Token.TokenKind.Newline)
                        throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                            "Expecting newline after type comment in block level of statement block!");
                    op3 = Tokenizer.CurSymbol;
                    Tokenizer.Advance();
                }
                
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.Indent)
                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                        "Expecting indent in block level of statement block!");
                var op4 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var nodes = new List<StatementNode>();
                nodes.Add(ParseStmt());
                while (Tokenizer.CurSymbol.Kind != Token.TokenKind.Dedent)
                {
                    nodes.Add(ParseStmt());
                }
                var op5 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                return new FuncSuiteStatement(startPos, Tokenizer.Position, op1, op2, op3, op4,nodes.ToArray(), op5);
            }

            return ParseSimpleStmt();
        }

        public StatementNode ParseFuncInput()
        {
            Tokenizer.Advance();
            var startPos = Tokenizer.Position;
            var right = ParseFuncType();
            var newlines = new List<Token>();
            while (Tokenizer.CurSymbol.Kind == Token.TokenKind.Newline)
            {
                newlines.Add(Tokenizer.CurSymbol);
                Tokenizer.Advance();
            }
            if (Tokenizer.CurSymbol.Kind != Token.TokenKind.EOF) 
                throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                    "Expecting end of file!");

            return new FuncInputStatement(startPos, Tokenizer.Position, right, newlines.ToArray(), Tokenizer.CurSymbol);
        }

        public ExpressionNode ParseFuncType()
        {
            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyLeftParen)
            {
                var startPos = Tokenizer.Position;
                var op1 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                ExpressionNode left = null;
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyRightParen)
                {
                    left = ParseTypeList();
                }
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyRightParen)
                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                    "Expecting ')' om FuncType expression!");
                var op2 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyArrow)
                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                        "Expecting '->' om FuncType expression!");
                var op3 = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                var right = ParseTest();

                return new FuncTypeExpression(startPos, Tokenizer.Position, op1, left, op2, op3, right);
            }
            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                "Expecting '(' om FuncType expression!");
        }

        public ExpressionNode ParseTypeList()
        {
            var startPos = Tokenizer.Position;
            Token mul = null, power = null;
            ExpressionNode right = null, left = null;
            var elements = new List<ExpressionNode>();
            var separators = new List<Token>();

            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMul)
            {
                mul = Tokenizer.CurSymbol;
                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyComma &&
                    Tokenizer.CurSymbol.Kind != Token.TokenKind.PyRightParen)
                {
                    Tokenizer.Advance();
                    left = ParseTest();
                }

                while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                {
                    separators.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPower)
                    {
                        power = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        right = ParseTest();
                        if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyRightParen)
                            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                                "Expecting ')' om FuncType expression!");
                    }
                    else
                    {
                        Tokenizer.Advance();
                        elements.Add(ParseTest());   
                    }
                }
            }
            else if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPower)
            {
                power = Tokenizer.CurSymbol;
                Tokenizer.Advance();
                right = ParseTest();
            }
            else
            {
                elements.Add(ParseTest());
                while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                {
                    separators.Add(Tokenizer.CurSymbol);
                    Tokenizer.Advance();
                    
                    if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyMul)
                    {
                        mul = Tokenizer.CurSymbol;
                        if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyComma &&
                            Tokenizer.CurSymbol.Kind != Token.TokenKind.PyRightParen)
                        {
                            Tokenizer.Advance();
                            left = ParseTest();
                        }

                        while (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyComma)
                        {
                            separators.Add(Tokenizer.CurSymbol);
                            Tokenizer.Advance();
                            if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPower)
                            {
                                power = Tokenizer.CurSymbol;
                                Tokenizer.Advance();
                                right = ParseTest();
                                if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyRightParen)
                                    throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                                        "Expecting ')' om FuncType expression!");
                            }
                            else
                            {
                                Tokenizer.Advance();
                                elements.Add(ParseTest());   
                            }
                        }
                    }
                    else if (Tokenizer.CurSymbol.Kind == Token.TokenKind.PyPower)
                    {
                        power = Tokenizer.CurSymbol;
                        Tokenizer.Advance();
                        right = ParseTest();
                        if (Tokenizer.CurSymbol.Kind != Token.TokenKind.PyRightParen)
                            throw new SyntaxErrorException(Tokenizer.Position, Tokenizer.CurSymbol,
                                "Expecting ')' om FuncType expression!");
                    }
                    else
                    {
                        elements.Add(ParseTest());
                    }
                }
            }
            
            return new TypeListExpression(startPos, Tokenizer.Position, mul, left, power, right, elements.ToArray(), separators.ToArray());
        }
    }
}
