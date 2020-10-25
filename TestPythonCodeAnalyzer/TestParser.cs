
using Xunit;
using PythonCodeAnalyzer.Parser;
using PythonCodeAnalyzer.Parser.Ast;
using PythonCodeAnalyzer.Parser.Ast.Expression;

namespace TestPythonCodeAnalyzer
{
    public class TestParser
    {
        [Fact]
        public void TestNameAtom()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("__init__(".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (NameLiteralExpression)parser.ParseAtom();
            Assert.Equal(Token.TokenKind.Name, node.Name.Kind );
            Assert.Equal("__init__", node.Name.Text);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(8UL, node.End );
            Assert.Equal(0UL, node.Name.Start );
            Assert.Equal(8UL, node.Name.End );
        }
        
        [Fact]
        public void TestNumberAtom()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("12.34e-45J(".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (NumberLiteralExpression)parser.ParseAtom();
            Assert.Equal(Token.TokenKind.Number, node.Number.Kind );
            Assert.Equal("12.34e-45J", node.Number.Text);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(10UL, node.End );
            Assert.Equal(0UL, node.Number.Start );
            Assert.Equal(10UL, node.Number.End );
        }
        
        [Fact]
        public void TestStringAtom()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("\"Hello, World!\"(".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (StringLiteralExpression)parser.ParseAtom();
            Assert.Equal(Token.TokenKind.String, node.Strings[0].Kind );
            Assert.Equal("\"Hello, World!\"", node.Strings[0].Text);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(15UL, node.End );
            Assert.Equal(0UL, node.Strings[0].Start );
            Assert.Equal(15UL, node.Strings[0].End );
        }
        
        [Fact]
        public void TestMultipleStringAtom()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("\"Hello, World!\"'Norway'(".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (StringLiteralExpression)parser.ParseAtom();
            Assert.Equal(Token.TokenKind.String, node.Strings[0].Kind );
            Assert.Equal("\"Hello, World!\"", node.Strings[0].Text);
            Assert.Equal("'Norway'", node.Strings[1].Text);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(23UL, node.End );
            Assert.Equal(0UL, node.Strings[0].Start );
            Assert.Equal(15UL, node.Strings[0].End );
            Assert.Equal(15UL, node.Strings[1].Start );
            Assert.Equal(23UL, node.Strings[1].End );
        }
        
        [Fact]
        public void TestNoneAtom()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("None(".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (NoneExpression)parser.ParseAtom();
            Assert.Equal(Token.TokenKind.PyNone, node.Operator.Kind );
            Assert.Equal("None", node.Operator.Text);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(4UL, node.End );
            Assert.Equal(0UL, node.Operator.Start );
            Assert.Equal(4UL, node.Operator.End );
        }
        
        [Fact]
        public void TestFalseAtom()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("False(".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (FalseLiteralExpression)parser.ParseAtom();
            Assert.Equal(Token.TokenKind.PyFalse, node.Operator.Kind );
            Assert.Equal("False", node.Operator.Text);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.Equal(0UL, node.Operator.Start );
            Assert.Equal(5UL, node.Operator.End );
        }
        
        [Fact]
        public void TestTrueAtom()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("True(".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (TrueLiteralExpression)parser.ParseAtom();
            Assert.Equal(Token.TokenKind.PyTrue, node.Operator.Kind );
            Assert.Equal("True", node.Operator.Text);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(4UL, node.End );
            Assert.Equal(0UL, node.Operator.Start );
            Assert.Equal(4UL, node.Operator.End );
        }
        
        [Fact]
        public void TestElipsisAtom()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("...(".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ElipsisLiteralExpression)parser.ParseAtom();
            Assert.Equal(Token.TokenKind.PyElipsis, node.Elipsis.Kind );
            Assert.Equal("...", node.Elipsis.Text);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(3UL, node.End );
            Assert.Equal(0UL, node.Elipsis.Start );
            Assert.Equal(3UL, node.Elipsis.End );
        }
        
        [Fact]
        public void TestEmptyTupleAtom()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("();".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (TupleExpression)parser.ParseAtom();
            Assert.Equal(Token.TokenKind.PyLeftParen, node.Operator1.Kind );
            Assert.Equal(Token.TokenKind.PyRightParen, node.Operator2.Kind );
            Assert.Equal(0UL, node.Operator1.Start );
            Assert.Equal(1UL, node.Operator1.End );
            Assert.Equal(1UL, node.Operator2.Start );
            Assert.Equal(2UL, node.Operator2.End );
            Assert.Equal(0UL, node.Start );
            Assert.Equal(2UL, node.End );
        }
        
        [Fact]
        public void TestEmptyListAtom()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("[];".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (AtomListExpression)parser.ParseAtom();
            Assert.Equal(Token.TokenKind.PyLeftBracket, node.Operator1.Kind );
            Assert.Equal(Token.TokenKind.PyRightBracket, node.Operator2.Kind );
            Assert.Equal(0UL, node.Operator1.Start );
            Assert.Equal(1UL, node.Operator1.End );
            Assert.Equal(1UL, node.Operator2.Start );
            Assert.Equal(2UL, node.Operator2.End );
            Assert.Equal(0UL, node.Start );
            Assert.Equal(2UL, node.End );
        }
        
        [Fact]
        public void TestEmptyDictionaryOrSetAtom()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{};".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (DictionaryExpression)parser.ParseAtom();
            Assert.Equal(Token.TokenKind.PyLeftCurly, node.Operator1.Kind );
            Assert.Equal(Token.TokenKind.PyRightCurly, node.Operator2.Kind );
            Assert.Equal(0UL, node.Operator1.Start );
            Assert.Equal(1UL, node.Operator1.End );
            Assert.Equal(1UL, node.Operator2.Start );
            Assert.Equal(2UL, node.Operator2.End );
            Assert.Equal(0UL, node.Start );
            Assert.Equal(2UL, node.End );
        }
        
        [Fact]
        public void TestAtomExprAlone()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("__init__+=".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (NameLiteralExpression)parser.ParseAtomExpression();
            Assert.Equal(Token.TokenKind.Name, node.Name.Kind );
            Assert.Equal("__init__", node.Name.Text);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(8UL, node.End );
            Assert.Equal(0UL, node.Name.Start );
            Assert.Equal(8UL, node.Name.End );
        }
        
        [Fact]
        public void TestAtomExprAwait()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("await __init__+=".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (AtomExpression)parser.ParseAtomExpression();
            Assert.True(node.IsAwait);
            Assert.Equal(Token.TokenKind.PyAwait, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression)node.Right).Name.Kind);
            Assert.Equal(6U, ((NameLiteralExpression)node.Right).Name.Start);
            Assert.Equal(14U, ((NameLiteralExpression)node.Right).Name.End);
            Assert.True(node.TrailerCollection == null);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(14UL, node.End );
        }
        
        [Fact]
        public void TestAtomExprAwaitAndCall()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("await __init__();".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (AtomExpression)parser.ParseAtomExpression();
            Assert.True(node.IsAwait);
            Assert.Equal(Token.TokenKind.PyAwait, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression)node.Right).Name.Kind);
            Assert.Equal(6U, ((NameLiteralExpression)node.Right).Name.Start);
            Assert.Equal(14U, ((NameLiteralExpression)node.Right).Name.End);
            Assert.True(node.TrailerCollection != null);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(16UL, node.End );

            var trailerNode = node.TrailerCollection;
            Assert.True(trailerNode.Length == 1);
            Assert.Equal(Token.TokenKind.PyLeftParen, ((CallExpression) trailerNode[0]).Operator1.Kind);
            Assert.True(((CallExpression) trailerNode[0]).Right == null);
            Assert.Equal(Token.TokenKind.PyRightParen, ((CallExpression) trailerNode[0]).Operator2.Kind);
        }
        
        [Fact]
        public void TestAtomExprAwaitAndIndex()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("await __init__[1];".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (AtomExpression)parser.ParseAtomExpression();
            Assert.True(node.IsAwait);
            Assert.Equal(Token.TokenKind.PyAwait, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression)node.Right).Name.Kind);
            Assert.Equal(6U, ((NameLiteralExpression)node.Right).Name.Start);
            Assert.Equal(14U, ((NameLiteralExpression)node.Right).Name.End);
            Assert.True(node.TrailerCollection != null);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(17UL, node.End );
            var trailerNode = node.TrailerCollection;
            Assert.True(trailerNode.Length == 1);
            Assert.Equal(Token.TokenKind.PyLeftBracket, ((IndexExpression) trailerNode[0]).Operator1.Kind);
            Assert.True(((IndexExpression) trailerNode[0]).Right != null);
            Assert.Equal(Token.TokenKind.PyRightBracket, ((IndexExpression) trailerNode[0]).Operator2.Kind);
        }
        
        [Fact]
        public void TestAtomExprAwaitAndDotName()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("await __init__.exec;".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (AtomExpression)parser.ParseAtomExpression();
            Assert.True(node.IsAwait);
            Assert.Equal(Token.TokenKind.PyAwait, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression)node.Right).Name.Kind);
            Assert.Equal(6U, ((NameLiteralExpression)node.Right).Name.Start);
            Assert.Equal(14U, ((NameLiteralExpression)node.Right).Name.End);
            Assert.True(node.TrailerCollection != null);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(19UL, node.End );
            var trailerNode = node.TrailerCollection;
            Assert.True(trailerNode.Length == 1);
            Assert.Equal(Token.TokenKind.PyDot, ((DotNameExpression) trailerNode[0]).Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((DotNameExpression) trailerNode[0]).Name.Kind);
            Assert.Equal("exec", ((DotNameExpression) trailerNode[0]).Name.Text);
        }
        
        [Fact]
        public void TestPowerExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a ** b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (PowerExpression)parser.ParsePower();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(6UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyPower, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestUnaryPlusExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("+a; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (FactorExpression)parser.ParseFactor();
            Assert.Equal(FactorExpression.FactorOperatorKind.UnaryPlus, node.FactorOperator);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(2UL, node.End );
            Assert.Equal(Token.TokenKind.PyPlus, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestUnaryMinusExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("-a; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (FactorExpression)parser.ParseFactor();
            Assert.Equal(FactorExpression.FactorOperatorKind.UnaryMinus, node.FactorOperator);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(2UL, node.End );
            Assert.Equal(Token.TokenKind.PyMinus, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestUnaryInvertExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("~a; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (FactorExpression)parser.ParseFactor();
            Assert.Equal(FactorExpression.FactorOperatorKind.UnaryInvert, node.FactorOperator);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(2UL, node.End );
            Assert.Equal(Token.TokenKind.PyInvert, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestTermSingleMulExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a * b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (TermExpression)parser.ParseTerm();
            Assert.Equal(TermExpression.OperatorKind.Mul, node.TermOperator);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyMul, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestTermSingleModuloExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a % b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (TermExpression)parser.ParseTerm();
            Assert.Equal(TermExpression.OperatorKind.Modulo, node.TermOperator);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyModulo, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestTermSingleMatriceExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a @ b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (TermExpression)parser.ParseTerm();
            Assert.Equal(TermExpression.OperatorKind.Matrice, node.TermOperator);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyMatrice, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestTermSingleDivExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a / b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (TermExpression)parser.ParseTerm();
            Assert.Equal(TermExpression.OperatorKind.Div, node.TermOperator);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyDiv, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestTermSingleFloorDivExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a // b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (TermExpression)parser.ParseTerm();
            Assert.Equal(TermExpression.OperatorKind.FloorDiv, node.TermOperator);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(6UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyFloorDiv, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestTermMultipleFloorDivExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a // b // c; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (TermExpression)parser.ParseTerm();
            Assert.Equal(TermExpression.OperatorKind.FloorDiv, node.TermOperator);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(11UL, node.End );
            
            // Left part of the last operator
            var node2 = ((TermExpression) node.Left);
            Assert.Equal(TermExpression.OperatorKind.FloorDiv, node2.TermOperator);
            Assert.Equal(0UL, node2.Start );
            Assert.Equal(7UL, node2.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node2.Left).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node2.Left).Name.Text);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node2.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node2.Right).Name.Text);
            
            // Right part of the last operator
            Assert.Equal(Token.TokenKind.PyFloorDiv, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("c", ((NameLiteralExpression) node.Right).Name.Text);
        }
        
        [Fact]
        public void TestArithSinglePlusExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a + b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ArithExpression)parser.ParseArithExpr();
            Assert.Equal(ArithExpression.ArithOperatorKind.Plus, node.ArithOperator);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyPlus, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestArithSingleMinusExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a - b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ArithExpression)parser.ParseArithExpr();
            Assert.Equal(ArithExpression.ArithOperatorKind.Minus, node.ArithOperator);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyMinus, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestArithMultiplePlusExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a + b + c; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ArithExpression)parser.ParseArithExpr();
            Assert.Equal(ArithExpression.ArithOperatorKind.Plus, node.ArithOperator);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(9UL, node.End );
            
            // Left part of the last operator
            var node2 = ((ArithExpression) node.Left);
            Assert.Equal(ArithExpression.ArithOperatorKind.Plus, node2.ArithOperator);
            Assert.Equal(0UL, node2.Start );
            Assert.Equal(6UL, node2.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node2.Left).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node2.Left).Name.Text);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node2.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node2.Right).Name.Text);
            
            // Right part of the last operator
            Assert.Equal(Token.TokenKind.PyPlus, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("c", ((NameLiteralExpression) node.Right).Name.Text);
        }
        
        [Fact]
        public void TestSingleShiftLeftExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a << b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ShiftExpression)parser.ParseShiftExpr();
            Assert.Equal(ShiftExpression.OperatorKind.Left, node.ShiftOperator);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(6UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyShiftLeft, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestSingleShiftRightExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a >> b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ShiftExpression)parser.ParseShiftExpr();
            Assert.Equal(ShiftExpression.OperatorKind.Right, node.ShiftOperator);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(6UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyShiftRight, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestMultipleShiftLeftExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a << b << c; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ShiftExpression)parser.ParseShiftExpr();
            Assert.Equal(ShiftExpression.OperatorKind.Left, node.ShiftOperator);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(11UL, node.End );
            
            // Left part of the last operator
            var node2 = ((ShiftExpression) node.Left);
            Assert.Equal(ShiftExpression.OperatorKind.Left, node2.ShiftOperator);
            Assert.Equal(0UL, node2.Start );
            Assert.Equal(7UL, node2.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node2.Left).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node2.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyShiftLeft, node2.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node2.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node2.Right).Name.Text);
            
            // Right part of the last operator
            Assert.Equal(Token.TokenKind.PyShiftLeft, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("c", ((NameLiteralExpression) node.Right).Name.Text);
        }
        
        [Fact]
        public void TestSingleAndExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a & b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (AndExpression)parser.ParseAndExpr();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyAnd, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestMultipleAndExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a & b & c; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (AndExpression)parser.ParseAndExpr();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(9UL, node.End );
            
            // Left part of the last operator
            var node2 = ((AndExpression) node.Left);
            Assert.Equal(0UL, node2.Start );
            Assert.Equal(6UL, node2.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node2.Left).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node2.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyAnd, node2.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node2.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node2.Right).Name.Text);
            
            // Right part of the last operator
            Assert.Equal(Token.TokenKind.PyAnd, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("c", ((NameLiteralExpression) node.Right).Name.Text);
        }
        
        [Fact]
        public void TestSingleXorExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a ^ b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (XorExpression)parser.ParseXorExpr();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyXor, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestMultipleXorExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a ^ b ^ c; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (XorExpression)parser.ParseXorExpr();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(9UL, node.End );
            
            // Left part of the last operator
            var node2 = ((XorExpression) node.Left);
            Assert.Equal(0UL, node2.Start );
            Assert.Equal(6UL, node2.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node2.Left).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node2.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyXor, node2.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node2.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node2.Right).Name.Text);
            
            // Right part of the last operator
            Assert.Equal(Token.TokenKind.PyXor, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("c", ((NameLiteralExpression) node.Right).Name.Text);
        }
        
        [Fact]
        public void TestSingleOrExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a | b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (OrExpression)parser.ParseOrExpr();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyOr, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestMultipleOrExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a | b | c; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (OrExpression)parser.ParseOrExpr();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(9UL, node.End );
            
            // Left part of the last operator
            var node2 = ((OrExpression) node.Left);
            Assert.Equal(0UL, node2.Start );
            Assert.Equal(6UL, node2.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node2.Left).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node2.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyOr, node2.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node2.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node2.Right).Name.Text);
            
            // Right part of the last operator
            Assert.Equal(Token.TokenKind.PyOr, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("c", ((NameLiteralExpression) node.Right).Name.Text);
        }
        
        [Fact]
        public void TestStarExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("*a; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (StarExpression)parser.ParseStarExpr();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(2UL, node.End );
            Assert.Equal(Token.TokenKind.PyMul, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Right).Name.Text);
        }
        
        [Fact]
        public void TestSingleRelationLessExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a < b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (RelationExpression)parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.Less, node.RelationKind);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyLess, node.Operator.Kind);
            Assert.True(node.Operator2 == null);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestSingleRelationLessEqualExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a <= b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (RelationExpression)parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.LessEqual, node.RelationKind);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(6UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyLessEqual, node.Operator.Kind);
            Assert.True(node.Operator2 == null);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestSingleRelationEqualExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a == b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (RelationExpression)parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.Equal, node.RelationKind);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(6UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyEqual, node.Operator.Kind);
            Assert.True(node.Operator2 == null);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestSingleRelationNotEqualExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a != b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (RelationExpression)parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.NotEqual, node.RelationKind);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(6UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyNotEqual, node.Operator.Kind);
            Assert.True(node.Operator2 == null);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestSingleRelationNotEqual2Expression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a <> b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (RelationExpression)parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.NotEqual, node.RelationKind);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(6UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyNotEqual, node.Operator.Kind);
            Assert.True(node.Operator2 == null);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestSingleRelationGreaterEqualExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a >= b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (RelationExpression)parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.GreaterEqual, node.RelationKind);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(6UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyGreaterEqual, node.Operator.Kind);
            Assert.True(node.Operator2 == null);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestSingleRelationGreaterExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a > b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (RelationExpression)parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.Greater, node.RelationKind);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyGreater, node.Operator.Kind);
            Assert.True(node.Operator2 == null);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestSingleRelationIsExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a is b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (RelationExpression)parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.Is, node.RelationKind);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(6UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyIs, node.Operator.Kind);
            Assert.True(node.Operator2 == null);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestSingleRelationInExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a in b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (RelationExpression)parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.In, node.RelationKind);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(6UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyIn, node.Operator.Kind);
            Assert.True(node.Operator2 == null);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestSingleRelationNotInExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a not in b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (RelationExpression)parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.NotIn, node.RelationKind);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(10UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyNot, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.PyIn, node.Operator2.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestSingleRelationIsNotExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a is not b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (RelationExpression)parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.IsNot, node.RelationKind);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(10UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal(Token.TokenKind.PyIs, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.PyNot, node.Operator2.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
        }
        
        [Fact]
        public void TestMultipleRelationLessExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a < b < c; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (RelationExpression)parser.ParseComparison();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(9UL, node.End );
            
            // Left part of the last operator
            var node2 = ((RelationExpression) node.Left);
            Assert.Equal(0UL, node2.Start );
            Assert.Equal(6UL, node2.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node2.Left).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node2.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyLess, node2.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node2.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node2.Right).Name.Text);
            
            // Right part of the last operator
            Assert.Equal(Token.TokenKind.PyLess, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("c", ((NameLiteralExpression) node.Right).Name.Text);
        }
        
        [Fact]
        public void TestSingleNotTestExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("not b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (NotTestExpression)parser.ParseNotTest();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.Equal(Token.TokenKind.PyNot, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node.Right).Name.Text);
        }
        
        [Fact]
        public void TestMultipleNotTestExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("not not b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (NotTestExpression)parser.ParseNotTest();
            Assert.Equal(Token.TokenKind.PyNot, node.Operator.Kind);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(9UL, node.End );

            var node2 = (NotTestExpression)node.Right;
            Assert.Equal(Token.TokenKind.PyNot, node2.Operator.Kind);
            Assert.Equal(4UL, node2.Start );
            Assert.Equal(9UL, node2.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node2.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node2.Right).Name.Text);
        }
        
        [Fact]
        public void TestSingleAndTestExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a and b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (AndTestExpression)parser.ParseAndTest();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(7UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyTestAnd, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node.Right).Name.Text);
        }
        
        [Fact]
        public void TestMultipleAndTestExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a and b and c; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (AndTestExpression)parser.ParseAndTest();
            Assert.Equal(Token.TokenKind.PyTestAnd, node.Operator.Kind);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(13UL, node.End );
            
            var node2 = (AndTestExpression) node.Left;
            Assert.Equal(Token.TokenKind.PyTestAnd, node2.Operator.Kind);
            Assert.Equal(0UL, node2.Start );
            Assert.Equal(8UL, node2.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node2.Left).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node2.Left).Name.Text);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node2.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node2.Right).Name.Text);
            
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("c", ((NameLiteralExpression) node.Right).Name.Text);
        }
        
        [Fact]
        public void TestSingleOrTestExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a or b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (OrTestExpression)parser.ParseOrTest();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(6UL, node.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyTestOr, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node.Right).Name.Text);
        }
        
        [Fact]
        public void TestMultipleOrTestExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a or b or c; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (OrTestExpression)parser.ParseOrTest();
            Assert.Equal(Token.TokenKind.PyTestOr, node.Operator.Kind);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(11UL, node.End );
            
            var node2 = (OrTestExpression) node.Left;
            Assert.Equal(Token.TokenKind.PyTestOr, node2.Operator.Kind);
            Assert.Equal(0UL, node2.Start );
            Assert.Equal(7UL, node2.End );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node2.Left).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node2.Left).Name.Text);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node2.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node2.Right).Name.Text);
            
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("c", ((NameLiteralExpression) node.Right).Name.Text);
        }
        
        [Fact]
        public void TestNoCond()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("__init__; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (NameLiteralExpression)parser.ParseNoCond();
            Assert.Equal(Token.TokenKind.Name, node.Name.Kind );
            Assert.Equal("__init__", node.Name.Text);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(8UL, node.End );
            Assert.Equal(0UL, node.Name.Start );
            Assert.Equal(8UL, node.Name.End );
        }
        
        [Fact]
        public void TestNoCondLambdaExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("lambda: a; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (LambdaExpression)parser.ParseNoCond();
            Assert.Equal(Token.TokenKind.PyLambda, node.Operator1.Kind );
            Assert.Equal(0UL, node.Start );
            Assert.Equal(9L, node.End );
            Assert.True(node.Left == null);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind );
            
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Right).Name.Text);
        }
        
        [Fact]
        public void TestRuleTestExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("__init__; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (NameLiteralExpression)parser.ParseTest();
            Assert.Equal(Token.TokenKind.Name, node.Name.Kind );
            Assert.Equal("__init__", node.Name.Text);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(8UL, node.End );
            Assert.Equal(0UL, node.Name.Start );
            Assert.Equal(8UL, node.Name.End );
        }
        
        [Fact]
        public void TestuleTestLambdaExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("lambda: a; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (LambdaExpression)parser.ParseTest();
            Assert.Equal(Token.TokenKind.PyLambda, node.Operator1.Kind );
            Assert.Equal(0UL, node.Start );
            Assert.Equal(9L, node.End );
            Assert.True(node.Left == null);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind );
            
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Right).Name.Text);
        }
        
        [Fact]
        public void TestRuleTestWithConditionalExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("__init__ if a else b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ConditionalExpression)parser.ParseTest();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(20UL, node.End );
            
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal("__init__", ((NameLiteralExpression) node.Left).Name.Text);
            
            Assert.Equal(Token.TokenKind.PyIf, node.Operator1.Kind );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Right).Name.Text);
            
            Assert.Equal(Token.TokenKind.PyElse, node.Operator2.Kind );
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Next).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node.Next).Name.Text);
        }
        
        [Fact]
        public void TestRuleNamedTestExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("__init__; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (NameLiteralExpression)parser.ParseNamedExpr();
            Assert.Equal(Token.TokenKind.Name, node.Name.Kind );
            Assert.Equal("__init__", node.Name.Text);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(8UL, node.End );
            Assert.Equal(0UL, node.Name.Start );
            Assert.Equal(8UL, node.Name.End );
        }
        
        [Fact]
        public void TestRuleNamedExprWithColonAssignExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("__init__ := a; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (NamedExpression)parser.ParseNamedExpr();
            Assert.Equal(Token.TokenKind.PyColonAssign, node.Operator.Kind );
            Assert.Equal(0UL, node.Start );
            Assert.Equal(13UL, node.End );
            
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal("__init__", ((NameLiteralExpression) node.Left).Name.Text);
            Assert.Equal(0U, ((NameLiteralExpression) node.Left).Name.Start);
            Assert.Equal(8U, ((NameLiteralExpression) node.Left).Name.End);
            
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Right).Name.Text);
            Assert.Equal(12U, ((NameLiteralExpression) node.Right).Name.Start);
            Assert.Equal(13U, ((NameLiteralExpression) node.Right).Name.End);
        }
        
        [Fact]
        public void TestYieldFromExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("yield from __init__; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (YieldExpression)parser.ParseYieldExpr();
            Assert.Equal(Token.TokenKind.PyYield, node.Operator1.Kind );
            Assert.Equal(Token.TokenKind.PyFrom, node.Operator2.Kind );
            Assert.Equal(0UL, node.Start );
            Assert.Equal(19UL, node.End );
            
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("__init__", ((NameLiteralExpression) node.Right).Name.Text);
            Assert.Equal(11U, ((NameLiteralExpression) node.Right).Name.Start);
            Assert.Equal(19U, ((NameLiteralExpression) node.Right).Name.End);
        }
        
        [Fact]
        public void TestYieldExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("yield __init__; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (YieldExpression)parser.ParseYieldExpr();
            Assert.Equal(Token.TokenKind.PyYield, node.Operator1.Kind );
            Assert.True(node.Operator2 == null);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(14UL, node.End );
            
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("__init__", ((NameLiteralExpression) node.Right).Name.Text);
            Assert.Equal(6U, ((NameLiteralExpression) node.Right).Name.Start);
            Assert.Equal(14U, ((NameLiteralExpression) node.Right).Name.End);
        }
        
        [Fact]
        public void TestSingleCompIfExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("if a; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (CompIfExpression)parser.ParseCompIter();
            Assert.Equal(Token.TokenKind.PyIf, node.Operator.Kind );
            Assert.True(node.Next == null);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(4UL, node.End );
            
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Right).Name.Text);
            Assert.Equal(3U, ((NameLiteralExpression) node.Right).Name.Start);
            Assert.Equal(4U, ((NameLiteralExpression) node.Right).Name.End);
        }
        
        [Fact]
        public void TestMulipleCompIfExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("if a if b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (CompIfExpression)parser.ParseCompIter();
            Assert.Equal(Token.TokenKind.PyIf, node.Operator.Kind );
            Assert.Equal(0UL, node.Start );
            Assert.Equal(9UL, node.End );
            
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Right).Name.Text);
            Assert.Equal(3U, ((NameLiteralExpression) node.Right).Name.Start);
            Assert.Equal(4U, ((NameLiteralExpression) node.Right).Name.End);
            
            var node2 = (CompIfExpression)node.Next;
            Assert.Equal(Token.TokenKind.PyIf, node2.Operator.Kind );
            Assert.Equal(5UL, node2.Start );
            Assert.Equal(9UL, node2.End );
            Assert.True(node2.Next == null);
            
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node2.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node2.Right).Name.Text);
            Assert.Equal(8U, ((NameLiteralExpression) node2.Right).Name.Start);
            Assert.Equal(9U, ((NameLiteralExpression) node2.Right).Name.End);
        }
        
        [Fact]
        public void TestSingleCompForExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("for a in b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (CompSyncForExpression)parser.ParseCompIter();
            Assert.Equal(Token.TokenKind.PyFor, node.Operator1.Kind );
            Assert.Equal(Token.TokenKind.PyIn, node.Operator2.Kind );
            Assert.Equal(0UL, node.Start );
            Assert.Equal(10UL, node.End );
            
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Left).Name.Text);
            Assert.Equal(4U, ((NameLiteralExpression) node.Left).Name.Start);
            Assert.Equal(5U, ((NameLiteralExpression) node.Left).Name.End);
            
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node.Right).Name.Text);
            Assert.Equal(9U, ((NameLiteralExpression) node.Right).Name.Start);
            Assert.Equal(10U, ((NameLiteralExpression) node.Right).Name.End);
            
            Assert.True(node.Next == null);
        }
        
        [Fact]
        public void TestMultipleCompForExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("for a in b if c; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (CompSyncForExpression)parser.ParseCompIter();
            Assert.Equal(Token.TokenKind.PyFor, node.Operator1.Kind );
            Assert.Equal(Token.TokenKind.PyIn, node.Operator2.Kind );
            Assert.Equal(0UL, node.Start );
            Assert.Equal(15UL, node.End );
            
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Left).Name.Text);
            Assert.Equal(4U, ((NameLiteralExpression) node.Left).Name.Start);
            Assert.Equal(5U, ((NameLiteralExpression) node.Left).Name.End);
            
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node.Right).Name.Text);
            Assert.Equal(9U, ((NameLiteralExpression) node.Right).Name.Start);
            Assert.Equal(10U, ((NameLiteralExpression) node.Right).Name.End);

            var node2 = (CompIfExpression) node.Next;
            Assert.Equal(Token.TokenKind.PyIf, node2.Operator.Kind );
            Assert.Equal("c", ((NameLiteralExpression)node2.Right).Name.Text );
            Assert.Equal(14U, ((NameLiteralExpression)node2.Right).Name.Start );
            Assert.Equal(15U, ((NameLiteralExpression)node2.Right).Name.End );
        }
        
        [Fact]
        public void TestSingleCompAsyncForExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("async for a in b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node2 = (CompForExpression) parser.ParseCompFor();
            Assert.Equal(Token.TokenKind.PyAsync, node2.AsyncOperator.Kind );
            Assert.Equal(0UL, node2.Start );
            Assert.Equal(16UL, node2.End );
            
            var node = (CompSyncForExpression)node2.Right;
            Assert.Equal(Token.TokenKind.PyFor, node.Operator1.Kind );
            Assert.Equal(Token.TokenKind.PyIn, node.Operator2.Kind );
            Assert.Equal(6UL, node.Start );
            Assert.Equal(16UL, node.End );
            
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Left).Name.Text);
            Assert.Equal(10U, ((NameLiteralExpression) node.Left).Name.Start);
            Assert.Equal(11U, ((NameLiteralExpression) node.Left).Name.End);
            
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node.Right).Name.Text);
            Assert.Equal(15U, ((NameLiteralExpression) node.Right).Name.Start);
            Assert.Equal(16U, ((NameLiteralExpression) node.Right).Name.End);
            
            Assert.True(node.Next == null);
        }
        
        [Fact]
        public void TestArgument1()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("*a, ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (StarArgument)parser.ParseArgument();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(2UL, node.End );
            
            Assert.Equal(Token.TokenKind.PyMul, node.MulOperator.Kind);
            Assert.Equal("a", node.Name.Text);
            Assert.Equal(1U, node.Name.Start);
            Assert.Equal(2U, node.Name.End);
        }
        
        [Fact]
        public void TestArgument2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("**a, ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (PowerArgument)parser.ParseArgument();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(3UL, node.End );
            
            Assert.Equal(Token.TokenKind.PyPower, node.PowerOperator.Kind);
            Assert.Equal("a", node.Name.Text);
            Assert.Equal(2U, node.Name.Start);
            Assert.Equal(3U, node.Name.End);
        }
        
        [Fact]
        public void TestArgument3()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a, ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ArgumentExpression)parser.ParseArgument();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(1UL, node.End );
            
            Assert.Equal(Token.TokenKind.Name, node.Left.Kind);
            Assert.Equal("a", node.Left.Text);
            Assert.Equal(0U, node.Left.Start);
            Assert.Equal(1U, node.Left.End);
        }
        
        [Fact]
        public void TestArgument4()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a = b, ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ArgumentExpression)parser.ParseArgument();
            Assert.Equal(Token.TokenKind.PyAssign, node.Operator.Kind);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            
            Assert.Equal(Token.TokenKind.Name, node.Left.Kind);
            Assert.Equal("a", node.Left.Text);
            Assert.Equal(0U, node.Left.Start);
            Assert.Equal(1U, node.Left.End);
            
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression)node.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression)node.Right).Name.Text);
            Assert.Equal(4U, node.Right.Start);
            Assert.Equal(5U, node.Right.End);
        }
        
        [Fact]
        public void TestArgument5()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a := b, ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ArgumentExpression)parser.ParseArgument();
            Assert.Equal(Token.TokenKind.PyColonAssign, node.Operator.Kind);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(6UL, node.End );
            
            Assert.Equal(Token.TokenKind.Name, node.Left.Kind);
            Assert.Equal("a", node.Left.Text);
            Assert.Equal(0U, node.Left.Start);
            Assert.Equal(1U, node.Left.End);
            
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression)node.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression)node.Right).Name.Text);
            Assert.Equal(5U, node.Right.Start);
            Assert.Equal(6U, node.Right.End);
        }
        
        [Fact]
        public void TestArgument6()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a for b in c, ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ArgumentExpression)parser.ParseArgument();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(12UL, node.End );
            
            Assert.Equal(Token.TokenKind.Name, node.Left.Kind);
            Assert.Equal("a", node.Left.Text);
            Assert.Equal(0U, node.Left.Start);
            Assert.Equal(1U, node.Left.End);

            var node2 = (CompSyncForExpression)node.Right;
            Assert.Equal(2u, node2.Start);
            Assert.Equal(12u, node2.End);
        }
        
        [Fact]
        public void TestArgList1()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ArgumentExpression)parser.ParseArgList();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(1UL, node.End );
            
            Assert.Equal(Token.TokenKind.Name, node.Left.Kind);
            Assert.Equal("a", node.Left.Text);
            Assert.Equal(0U, node.Left.Start);
            Assert.Equal(1U, node.Left.End);
        }
        
        [Fact]
        public void TestArgList2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("(a,) ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            parser.Tokenizer.Advance();
            
            var node = (ListExpression)parser.ParseArgList();
            Assert.Equal(ListExpression.ListType.ArgumentList, node.ContainerType);
            Assert.Equal(1UL, node.Start );
            Assert.Equal(3UL, node.End );
            Assert.True(node.Elements.Length == 1);
            Assert.Equal("a", ((ArgumentExpression) node.Elements[0]).Left.Text);
            
            Assert.True(node.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PyComma, node.Separators[0].Kind);
        }
        
        [Fact]
        public void TestArgList3()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("(a, b) ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            parser.Tokenizer.Advance();
            
            var node = (ListExpression)parser.ParseArgList();
            Assert.Equal(ListExpression.ListType.ArgumentList, node.ContainerType);
            Assert.Equal(1UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.True(node.Elements.Length == 2);
            Assert.Equal("a", ((ArgumentExpression) node.Elements[0]).Left.Text);
            Assert.Equal("b", ((ArgumentExpression) node.Elements[1]).Left.Text);
            
            Assert.True(node.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PyComma, node.Separators[0].Kind);
        }
        
        [Fact]
        public void TestArgList4()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("(a, b,) ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            parser.Tokenizer.Advance();
            
            var node = (ListExpression)parser.ParseArgList();
            Assert.Equal(ListExpression.ListType.ArgumentList, node.ContainerType);
            Assert.Equal(1UL, node.Start );
            Assert.Equal(6UL, node.End );
            Assert.True(node.Elements.Length == 2);
            Assert.Equal("a", ((ArgumentExpression) node.Elements[0]).Left.Text);
            Assert.Equal("b", ((ArgumentExpression) node.Elements[1]).Left.Text);
            
            Assert.True(node.Separators.Length == 2);
            Assert.Equal(Token.TokenKind.PyComma, node.Separators[0].Kind);
            Assert.Equal(Token.TokenKind.PyComma, node.Separators[1].Kind);
        }
        
        [Fact]
        public void TestATestList1()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (NameLiteralExpression)parser.ParseTestList();
            Assert.Equal("a", node.Name.Text );
            Assert.Equal(0UL, node.Start );
            Assert.Equal(1UL, node.End );
        }
        
        [Fact]
        public void TestATestList2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a,; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListExpression)parser.ParseTestList();
            Assert.Equal(ListExpression.ListType.TestList, node.ContainerType);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(2UL, node.End );
            Assert.True(node.Elements.Length == 1);
            Assert.Equal("a", ((NameLiteralExpression) node.Elements[0]).Name.Text);
            
            Assert.True(node.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PyComma, node.Separators[0].Kind);
        }
        
        [Fact]
        public void TestATestList3()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a, b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListExpression)parser.ParseTestList();
            Assert.Equal(ListExpression.ListType.TestList, node.ContainerType);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(4UL, node.End );
            Assert.True(node.Elements.Length == 2);
            Assert.Equal("a", ((NameLiteralExpression) node.Elements[0]).Name.Text);
            Assert.Equal("b", ((NameLiteralExpression) node.Elements[1]).Name.Text);
            
            Assert.True(node.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PyComma, node.Separators[0].Kind);
        }
        
        [Fact]
        public void TestATestList4()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a, b,; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListExpression)parser.ParseTestList();
            Assert.Equal(ListExpression.ListType.TestList, node.ContainerType);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.True(node.Elements.Length == 2);
            Assert.Equal("a", ((NameLiteralExpression) node.Elements[0]).Name.Text);
            Assert.Equal("b", ((NameLiteralExpression) node.Elements[1]).Name.Text);
            
            Assert.True(node.Separators.Length == 2);
            Assert.Equal(Token.TokenKind.PyComma, node.Separators[0].Kind);
            Assert.Equal(Token.TokenKind.PyComma, node.Separators[1].Kind);
        }
        
        [Fact]
        public void TestAExprList1()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("*a: ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (StarExpression)parser.ParseExprList();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(2UL, node.End );
        }
        
        [Fact]
        public void TestAExprList2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a: ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (NameLiteralExpression)parser.ParseExprList();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(1UL, node.End );
        }
        
        [Fact]
        public void TestAExprList3()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("*a,: ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListExpression)parser.ParseExprList();
            Assert.Equal(ListExpression.ListType.ExprList, node.ContainerType);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(3UL, node.End );
            
            Assert.True(node.Elements.Length == 1);
            var node2 = (StarExpression) node.Elements[0];
            Assert.Equal("a", ((NameLiteralExpression)node2.Right).Name.Text);
            
            Assert.True(node.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PyComma, node.Separators[0].Kind);
        }
        
        [Fact]
        public void TestAExprList4()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a,: ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListExpression)parser.ParseExprList();
            Assert.Equal(ListExpression.ListType.ExprList, node.ContainerType);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(2UL, node.End );
            
            Assert.True(node.Elements.Length == 1);
            var node2 = (NameLiteralExpression) node.Elements[0];
            Assert.Equal("a", node2.Name.Text);
            
            Assert.True(node.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PyComma, node.Separators[0].Kind);
        }
        
        [Fact]
        public void TestAExprList5()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a, *b: ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListExpression)parser.ParseExprList();
            Assert.Equal(ListExpression.ListType.ExprList, node.ContainerType);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            
            Assert.True(node.Elements.Length == 2);
            var node2 = (NameLiteralExpression) node.Elements[0];
            Assert.Equal("a", node2.Name.Text);
            var node3 = (StarExpression) node.Elements[1];
            
            Assert.True(node.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PyComma, node.Separators[0].Kind);
        }
        
        [Fact]
        public void TestAExprList6()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a, b,: ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListExpression)parser.ParseExprList();
            Assert.Equal(ListExpression.ListType.ExprList, node.ContainerType);
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            
            Assert.True(node.Elements.Length == 2);
            var node2 = (NameLiteralExpression) node.Elements[0];
            Assert.Equal("a", node2.Name.Text);
            
            var node3 = (NameLiteralExpression) node.Elements[1];
            Assert.Equal("b", node3.Name.Text);
            
            Assert.True(node.Separators.Length == 2);
            Assert.Equal(Token.TokenKind.PyComma, node.Separators[0].Kind);
            Assert.Equal(Token.TokenKind.PyComma, node.Separators[1].Kind);
        }
        
        [Fact]
        public void TestSubscript1()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("[1]; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            parser.Tokenizer.Advance();
            
            var node = (SubscriptExpression)parser.ParseSubscript();
            Assert.Equal(1UL, node.Start );
            Assert.Equal(2UL, node.End );

            var node2 = (NumberLiteralExpression) node.StartPos;
            Assert.Equal("1", node2.Number.Text);
            Assert.True(node.Operator1 == null);
            Assert.True(node.EndPos == null);
            Assert.True(node.Operator2 == null);
            Assert.True(node.Step == null);
        }
        
        [Fact]
        public void TestSubscript2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("[:]; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            parser.Tokenizer.Advance();
            
            var node = (SubscriptExpression)parser.ParseSubscript();
            Assert.Equal(1UL, node.Start );
            Assert.Equal(2UL, node.End );

            Assert.True(node.StartPos == null);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator1.Kind);
            Assert.True(node.EndPos == null);
            Assert.True(node.Operator2 == null);
            Assert.True(node.Step == null);
        }
        
        [Fact]
        public void TestSubscript3()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("[::]; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            parser.Tokenizer.Advance();
            
            var node = (SubscriptExpression)parser.ParseSubscript();
            Assert.Equal(1UL, node.Start );
            Assert.Equal(3UL, node.End );

            Assert.True(node.StartPos == null);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator1.Kind);
            Assert.True(node.EndPos == null);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind);
            Assert.True(node.Step == null);
        }
        
        [Fact]
        public void TestSubscript4()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("[1:]; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            parser.Tokenizer.Advance();
            
            var node = (SubscriptExpression)parser.ParseSubscript();
            Assert.Equal(1UL, node.Start );
            Assert.Equal(3UL, node.End );

            var node2 = (NumberLiteralExpression) node.StartPos;
            Assert.Equal("1", node2.Number.Text);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator1.Kind);
            Assert.True(node.EndPos == null);
            Assert.True(node.Operator2 == null);
            Assert.True(node.Step == null);
        }
        
        [Fact]
        public void TestSubscript5()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("[1::]; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            parser.Tokenizer.Advance();
            
            var node = (SubscriptExpression)parser.ParseSubscript();
            Assert.Equal(1UL, node.Start );
            Assert.Equal(4UL, node.End );

            var node2 = (NumberLiteralExpression) node.StartPos;
            Assert.Equal("1", node2.Number.Text);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator1.Kind);
            Assert.True(node.EndPos == null);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind);
            Assert.True(node.Step == null);
        }
        
        [Fact]
        public void TestSubscript6()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("[1:10:]; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            parser.Tokenizer.Advance();
            
            var node = (SubscriptExpression)parser.ParseSubscript();
            Assert.Equal(1UL, node.Start );
            Assert.Equal(6UL, node.End );

            var node2 = (NumberLiteralExpression) node.StartPos;
            Assert.Equal("1", node2.Number.Text);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator1.Kind);
            var node3 = (NumberLiteralExpression) node.EndPos;
            Assert.Equal("10", node3.Number.Text);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind);
            Assert.True(node.Step == null);
        }
        
        [Fact]
        public void TestSubscript7()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("[1:10:2]; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            parser.Tokenizer.Advance();
            
            var node = (SubscriptExpression)parser.ParseSubscript();
            Assert.Equal(1UL, node.Start );
            Assert.Equal(7UL, node.End );

            var node2 = (NumberLiteralExpression) node.StartPos;
            Assert.Equal("1", node2.Number.Text);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator1.Kind);
            var node3 = (NumberLiteralExpression) node.EndPos;
            Assert.Equal("10", node3.Number.Text);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind);
            var node4 = (NumberLiteralExpression) node.Step;
            Assert.Equal("2", node4.Number.Text);
        }
        
        [Fact]
        public void TestSubscript8()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("[1:10]; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            parser.Tokenizer.Advance();
            
            var node = (SubscriptExpression)parser.ParseSubscript();
            Assert.Equal(1UL, node.Start );
            Assert.Equal(5UL, node.End );

            var node2 = (NumberLiteralExpression) node.StartPos;
            Assert.Equal("1", node2.Number.Text);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator1.Kind);
            var node3 = (NumberLiteralExpression) node.EndPos;
            Assert.Equal("10", node3.Number.Text);
            
            Assert.True(node.Operator2 == null);
            Assert.True(node.Step == null);
        }
        
        [Fact]
        public void TestSubscriptList1()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("[1:10]; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            parser.Tokenizer.Advance();
            
            var node = (SubscriptExpression)parser.ParseSubscriptList();
            Assert.Equal(1UL, node.Start );
            Assert.Equal(5UL, node.End );

            var node2 = (NumberLiteralExpression) node.StartPos;
            Assert.Equal("1", node2.Number.Text);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator1.Kind);
            var node3 = (NumberLiteralExpression) node.EndPos;
            Assert.Equal("10", node3.Number.Text);
            
            Assert.True(node.Operator2 == null);
            Assert.True(node.Step == null);
        }
        
        [Fact]
        public void TestSubscriptList2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("[1:10,]; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            parser.Tokenizer.Advance();
            
            var node = (ListExpression)parser.ParseSubscriptList();
            Assert.Equal(ListExpression.ListType.SubscriptList, node.ContainerType);
            Assert.Equal(1UL, node.Start );
            Assert.Equal(6UL, node.End );
            
            Assert.True(node.Elements.Length == 1);
            var node2 = (SubscriptExpression) node.Elements[0];
            var node3 = (NumberLiteralExpression) node2.StartPos;
            Assert.Equal("1", node3.Number.Text);
            
            Assert.True(node.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PyComma, node.Separators[0].Kind);
        }
        
        [Fact]
        public void TestSubscriptList3()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("[1:10, b]; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            parser.Tokenizer.Advance();
            
            var node = (ListExpression)parser.ParseSubscriptList();
            Assert.Equal(ListExpression.ListType.SubscriptList, node.ContainerType);
            Assert.Equal(1UL, node.Start );
            Assert.Equal(8UL, node.End );
            
            Assert.True(node.Elements.Length == 2);
            var node2 = (SubscriptExpression) node.Elements[0];
            var node3 = (NumberLiteralExpression) node2.StartPos;
            Assert.Equal("1", node3.Number.Text);
            
            var node4 = (SubscriptExpression) node.Elements[1];
            var node5 = (NameLiteralExpression) node4.StartPos;
            Assert.Equal("b", node5.Name.Text);
            
            Assert.True(node.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PyComma, node.Separators[0].Kind);
        }
        
        [Fact]
        public void TestSubscriptList4()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("[1:10, b,]; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            parser.Tokenizer.Advance();
            
            var node = (ListExpression)parser.ParseSubscriptList();
            Assert.Equal(ListExpression.ListType.SubscriptList, node.ContainerType);
            Assert.Equal(1UL, node.Start );
            Assert.Equal(9UL, node.End );
            
            Assert.True(node.Elements.Length == 2);
            var node2 = (SubscriptExpression) node.Elements[0];
            var node3 = (NumberLiteralExpression) node2.StartPos;
            Assert.Equal("1", node3.Number.Text);
            
            var node4 = (SubscriptExpression) node.Elements[1];
            var node5 = (NameLiteralExpression) node4.StartPos;
            Assert.Equal("b", node5.Name.Text);
            
            Assert.True(node.Separators.Length == 2);
            Assert.Equal(Token.TokenKind.PyComma, node.Separators[0].Kind);
            Assert.Equal(Token.TokenKind.PyComma, node.Separators[1].Kind);
        }
        
        [Fact]
        public void TestTuple1()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("( yield from test ); ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (TupleExpression)parser.ParseAtom();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(19UL, node.End );
            var node2 = (YieldExpression)node.Right;
            Assert.Equal(2U, node2.Start);
        }
        
        [Fact]
        public void TestTuple2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("( *a ); ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (TupleExpression)parser.ParseAtom();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(6UL, node.End );

            var node2 = (StarExpression)node.Right;
            Assert.Equal(2U, node2.Start);
        }
        
        [Fact]
        public void TestTuple3()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("( *a for b in c ); ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (TupleExpression)parser.ParseAtom();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(17UL, node.End );

            var node2 = (ListExpression)node.Right;
            Assert.True(node2.Elements.Length == 2);

            var node4 = (StarExpression) node2.Elements[0];
            Assert.Equal("a", ((NameLiteralExpression)node4.Right).Name.Text);

            var node3 = (CompSyncForExpression) node2.Elements[1];
            Assert.Equal(5U, node3.Start);
        }
        
        [Fact]
        public void TestTuple4()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("( *a, ); ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (TupleExpression)parser.ParseAtom();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(7UL, node.End );

            var node2 = (ListExpression)node.Right;
            Assert.True(node2.Elements.Length == 1);
            
            var node4 = (StarExpression) node2.Elements[0];
            Assert.Equal("a", ((NameLiteralExpression)node4.Right).Name.Text);
            
            Assert.True(node2.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[0].Kind);
        }
        
        [Fact]
        public void TestTuple5()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("( *a, b := c ); ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (TupleExpression)parser.ParseAtom();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(14UL, node.End );

            var node2 = (ListExpression)node.Right;
            Assert.True(node2.Elements.Length == 2);
            
            var node4 = (StarExpression) node2.Elements[0];
            Assert.Equal("a", ((NameLiteralExpression)node4.Right).Name.Text);

            var node3 = (NamedExpression) node2.Elements[1];
            Assert.Equal(6U, node3.Start);
            
            Assert.True(node2.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[0].Kind);
        }
        
        [Fact]
        public void TestTuple6()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("( *a, b := c, ); ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (TupleExpression)parser.ParseAtom();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(15UL, node.End );

            var node2 = (ListExpression)node.Right;
            Assert.True(node2.Elements.Length == 2);
            
            var node4 = (StarExpression) node2.Elements[0];
            Assert.Equal("a", ((NameLiteralExpression)node4.Right).Name.Text);

            var node3 = (NamedExpression) node2.Elements[1];
            Assert.Equal(6U, node3.Start);
            
            Assert.True(node2.Separators.Length == 2);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[0].Kind);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[1].Kind);
        }
        
        [Fact]
        public void TestTuple7()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("( a := b, *b, ); ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (TupleExpression)parser.ParseAtom();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(15UL, node.End );

            var node2 = (ListExpression) node.Right;
            Assert.Equal(ListExpression.ListType.TestListStarExpr, node2.ContainerType);

            var node3 = (NamedExpression) node2.Elements[0];
            Assert.Equal(2U, node3.Start);

            var node4 = (StarExpression) node2.Elements[1];
            Assert.Equal("b", ((NameLiteralExpression)node4.Right).Name.Text);
            
            Assert.True(node2.Separators.Length == 2);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[0].Kind);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[1].Kind);
        }
        
        [Fact]
        public void TestAtomList1()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("[ a := b, *b, ]; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (AtomListExpression)parser.ParseAtom();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(15UL, node.End );

            var node2 = (ListExpression) node.Right;
            Assert.Equal(ListExpression.ListType.TestListStarExpr, node2.ContainerType);

            var node3 = (NamedExpression) node2.Elements[0];
            Assert.Equal(2U, node3.Start);

            var node4 = (StarExpression) node2.Elements[1];
            Assert.Equal(10U, node4.Start);
            
            Assert.True(node2.Separators.Length == 2);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[0].Kind);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[1].Kind);
        }
        
        [Fact]
        public void TestAtomSet1()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{ a }; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (SetExpression)parser.ParseAtom();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );

            var node2 = (SetContainerExpression)node.Right;
            Assert.True(node2.Keys.Length == 1);
            Assert.Equal("a", ((NameLiteralExpression)node2.Keys[0]).Name.Text);
            
            Assert.True(node2.Separators.Length == 0);
        }
        
        [Fact]
        public void TestAtomSet2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{ a for b in c }; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (SetExpression)parser.ParseAtom();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(16UL, node.End );

            var node2 = (SetContainerExpression)node.Right;
            Assert.True(node2.Keys.Length == 2);
            Assert.Equal("a", ((NameLiteralExpression)node2.Keys[0]).Name.Text);
            Assert.Equal(4U, ((CompSyncForExpression)node2.Keys[1]).Start);
            
            Assert.True(node2.Separators.Length == 0);
        }
    }
}