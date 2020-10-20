
using Xunit;
using PythonCodeAnalyzer.Parser;
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
    }
}