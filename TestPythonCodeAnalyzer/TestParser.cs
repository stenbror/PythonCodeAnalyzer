
using System;
using Xunit;
using PythonCodeAnalyzer.Parser;
using PythonCodeAnalyzer.Parser.Ast;
using PythonCodeAnalyzer.Parser.Ast.Expression;
using PythonCodeAnalyzer.Parser.Ast.Statement;

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

            var node = (NameLiteralExpression) parser.ParseAtom();
            Assert.Equal(Token.TokenKind.Name, node.Name.Kind);
            Assert.Equal("__init__", node.Name.Text);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(8UL, node.End);
            Assert.Equal(0UL, node.Name.Start);
            Assert.Equal(8UL, node.Name.End);
        }

        [Fact]
        public void TestNumberAtom()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("12.34e-45J(".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (NumberLiteralExpression) parser.ParseAtom();
            Assert.Equal(Token.TokenKind.Number, node.Number.Kind);
            Assert.Equal("12.34e-45J", node.Number.Text);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(10UL, node.End);
            Assert.Equal(0UL, node.Number.Start);
            Assert.Equal(10UL, node.Number.End);
        }

        [Fact]
        public void TestStringAtom()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("\"Hello, World!\"(".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (StringLiteralExpression) parser.ParseAtom();
            Assert.Equal(Token.TokenKind.String, node.Strings[0].Kind);
            Assert.Equal("\"Hello, World!\"", node.Strings[0].Text);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(15UL, node.End);
            Assert.Equal(0UL, node.Strings[0].Start);
            Assert.Equal(15UL, node.Strings[0].End);
        }

        [Fact]
        public void TestMultipleStringAtom()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("\"Hello, World!\"'Norway'(".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (StringLiteralExpression) parser.ParseAtom();
            Assert.Equal(Token.TokenKind.String, node.Strings[0].Kind);
            Assert.Equal("\"Hello, World!\"", node.Strings[0].Text);
            Assert.Equal("'Norway'", node.Strings[1].Text);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(23UL, node.End);
            Assert.Equal(0UL, node.Strings[0].Start);
            Assert.Equal(15UL, node.Strings[0].End);
            Assert.Equal(15UL, node.Strings[1].Start);
            Assert.Equal(23UL, node.Strings[1].End);
        }

        [Fact]
        public void TestNoneAtom()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("None(".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (NoneExpression) parser.ParseAtom();
            Assert.Equal(Token.TokenKind.PyNone, node.Operator.Kind);
            Assert.Equal("None", node.Operator.Text);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(4UL, node.End);
            Assert.Equal(0UL, node.Operator.Start);
            Assert.Equal(4UL, node.Operator.End);
        }

        [Fact]
        public void TestFalseAtom()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("False(".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (FalseLiteralExpression) parser.ParseAtom();
            Assert.Equal(Token.TokenKind.PyFalse, node.Operator.Kind);
            Assert.Equal("False", node.Operator.Text);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(5UL, node.End);
            Assert.Equal(0UL, node.Operator.Start);
            Assert.Equal(5UL, node.Operator.End);
        }

        [Fact]
        public void TestTrueAtom()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("True(".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (TrueLiteralExpression) parser.ParseAtom();
            Assert.Equal(Token.TokenKind.PyTrue, node.Operator.Kind);
            Assert.Equal("True", node.Operator.Text);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(4UL, node.End);
            Assert.Equal(0UL, node.Operator.Start);
            Assert.Equal(4UL, node.Operator.End);
        }

        [Fact]
        public void TestElipsisAtom()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("...(".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (ElipsisLiteralExpression) parser.ParseAtom();
            Assert.Equal(Token.TokenKind.PyElipsis, node.Elipsis.Kind);
            Assert.Equal("...", node.Elipsis.Text);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(3UL, node.End);
            Assert.Equal(0UL, node.Elipsis.Start);
            Assert.Equal(3UL, node.Elipsis.End);
        }

        [Fact]
        public void TestEmptyTupleAtom()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("();".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (TupleExpression) parser.ParseAtom();
            Assert.Equal(Token.TokenKind.PyLeftParen, node.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyRightParen, node.Operator2.Kind);
            Assert.Equal(0UL, node.Operator1.Start);
            Assert.Equal(1UL, node.Operator1.End);
            Assert.Equal(1UL, node.Operator2.Start);
            Assert.Equal(2UL, node.Operator2.End);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(2UL, node.End);
        }

        [Fact]
        public void TestEmptyListAtom()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("[];".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (AtomListExpression) parser.ParseAtom();
            Assert.Equal(Token.TokenKind.PyLeftBracket, node.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyRightBracket, node.Operator2.Kind);
            Assert.Equal(0UL, node.Operator1.Start);
            Assert.Equal(1UL, node.Operator1.End);
            Assert.Equal(1UL, node.Operator2.Start);
            Assert.Equal(2UL, node.Operator2.End);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(2UL, node.End);
        }

        [Fact]
        public void TestEmptyDictionaryOrSetAtom()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{};".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (DictionaryExpression) parser.ParseAtom();
            Assert.Equal(Token.TokenKind.PyLeftCurly, node.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyRightCurly, node.Operator2.Kind);
            Assert.Equal(0UL, node.Operator1.Start);
            Assert.Equal(1UL, node.Operator1.End);
            Assert.Equal(1UL, node.Operator2.Start);
            Assert.Equal(2UL, node.Operator2.End);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(2UL, node.End);
        }

        [Fact]
        public void TestAtomExprAlone()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("__init__+=".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (NameLiteralExpression) parser.ParseAtomExpression();
            Assert.Equal(Token.TokenKind.Name, node.Name.Kind);
            Assert.Equal("__init__", node.Name.Text);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(8UL, node.End);
            Assert.Equal(0UL, node.Name.Start);
            Assert.Equal(8UL, node.Name.End);
        }

        [Fact]
        public void TestAtomExprAwait()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("await __init__+=".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (AtomExpression) parser.ParseAtomExpression();
            Assert.True(node.IsAwait);
            Assert.Equal(Token.TokenKind.PyAwait, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal(6U, ((NameLiteralExpression) node.Right).Name.Start);
            Assert.Equal(14U, ((NameLiteralExpression) node.Right).Name.End);
            Assert.True(node.TrailerCollection == null);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(14UL, node.End);
        }

        [Fact]
        public void TestAtomExprAwaitAndCall()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("await __init__();".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (AtomExpression) parser.ParseAtomExpression();
            Assert.True(node.IsAwait);
            Assert.Equal(Token.TokenKind.PyAwait, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal(6U, ((NameLiteralExpression) node.Right).Name.Start);
            Assert.Equal(14U, ((NameLiteralExpression) node.Right).Name.End);
            Assert.True(node.TrailerCollection != null);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(16UL, node.End);

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

            var node = (AtomExpression) parser.ParseAtomExpression();
            Assert.True(node.IsAwait);
            Assert.Equal(Token.TokenKind.PyAwait, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal(6U, ((NameLiteralExpression) node.Right).Name.Start);
            Assert.Equal(14U, ((NameLiteralExpression) node.Right).Name.End);
            Assert.True(node.TrailerCollection != null);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(17UL, node.End);
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

            var node = (AtomExpression) parser.ParseAtomExpression();
            Assert.True(node.IsAwait);
            Assert.Equal(Token.TokenKind.PyAwait, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal(6U, ((NameLiteralExpression) node.Right).Name.Start);
            Assert.Equal(14U, ((NameLiteralExpression) node.Right).Name.End);
            Assert.True(node.TrailerCollection != null);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(19UL, node.End);
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

            var node = (PowerExpression) parser.ParsePower();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(6UL, node.End);
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

            var node = (FactorExpression) parser.ParseFactor();
            Assert.Equal(FactorExpression.FactorOperatorKind.UnaryPlus, node.FactorOperator);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(2UL, node.End);
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

            var node = (FactorExpression) parser.ParseFactor();
            Assert.Equal(FactorExpression.FactorOperatorKind.UnaryMinus, node.FactorOperator);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(2UL, node.End);
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

            var node = (FactorExpression) parser.ParseFactor();
            Assert.Equal(FactorExpression.FactorOperatorKind.UnaryInvert, node.FactorOperator);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(2UL, node.End);
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

            var node = (TermExpression) parser.ParseTerm();
            Assert.Equal(TermExpression.OperatorKind.Mul, node.TermOperator);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(5UL, node.End);
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

            var node = (TermExpression) parser.ParseTerm();
            Assert.Equal(TermExpression.OperatorKind.Modulo, node.TermOperator);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(5UL, node.End);
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

            var node = (TermExpression) parser.ParseTerm();
            Assert.Equal(TermExpression.OperatorKind.Matrice, node.TermOperator);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(5UL, node.End);
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

            var node = (TermExpression) parser.ParseTerm();
            Assert.Equal(TermExpression.OperatorKind.Div, node.TermOperator);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(5UL, node.End);
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

            var node = (TermExpression) parser.ParseTerm();
            Assert.Equal(TermExpression.OperatorKind.FloorDiv, node.TermOperator);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(6UL, node.End);
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

            var node = (TermExpression) parser.ParseTerm();
            Assert.Equal(TermExpression.OperatorKind.FloorDiv, node.TermOperator);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(11UL, node.End);

            // Left part of the last operator
            var node2 = ((TermExpression) node.Left);
            Assert.Equal(TermExpression.OperatorKind.FloorDiv, node2.TermOperator);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(7UL, node2.End);
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

            var node = (ArithExpression) parser.ParseArithExpr();
            Assert.Equal(ArithExpression.ArithOperatorKind.Plus, node.ArithOperator);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(5UL, node.End);
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

            var node = (ArithExpression) parser.ParseArithExpr();
            Assert.Equal(ArithExpression.ArithOperatorKind.Minus, node.ArithOperator);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(5UL, node.End);
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

            var node = (ArithExpression) parser.ParseArithExpr();
            Assert.Equal(ArithExpression.ArithOperatorKind.Plus, node.ArithOperator);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(9UL, node.End);

            // Left part of the last operator
            var node2 = ((ArithExpression) node.Left);
            Assert.Equal(ArithExpression.ArithOperatorKind.Plus, node2.ArithOperator);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(6UL, node2.End);
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

            var node = (ShiftExpression) parser.ParseShiftExpr();
            Assert.Equal(ShiftExpression.OperatorKind.Left, node.ShiftOperator);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(6UL, node.End);
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

            var node = (ShiftExpression) parser.ParseShiftExpr();
            Assert.Equal(ShiftExpression.OperatorKind.Right, node.ShiftOperator);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(6UL, node.End);
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

            var node = (ShiftExpression) parser.ParseShiftExpr();
            Assert.Equal(ShiftExpression.OperatorKind.Left, node.ShiftOperator);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(11UL, node.End);

            // Left part of the last operator
            var node2 = ((ShiftExpression) node.Left);
            Assert.Equal(ShiftExpression.OperatorKind.Left, node2.ShiftOperator);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(7UL, node2.End);
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

            var node = (AndExpression) parser.ParseAndExpr();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(5UL, node.End);
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

            var node = (AndExpression) parser.ParseAndExpr();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(9UL, node.End);

            // Left part of the last operator
            var node2 = ((AndExpression) node.Left);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(6UL, node2.End);
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

            var node = (XorExpression) parser.ParseXorExpr();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(5UL, node.End);
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

            var node = (XorExpression) parser.ParseXorExpr();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(9UL, node.End);

            // Left part of the last operator
            var node2 = ((XorExpression) node.Left);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(6UL, node2.End);
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

            var node = (OrExpression) parser.ParseOrExpr();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(5UL, node.End);
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

            var node = (OrExpression) parser.ParseOrExpr();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(9UL, node.End);

            // Left part of the last operator
            var node2 = ((OrExpression) node.Left);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(6UL, node2.End);
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

            var node = (StarExpression) parser.ParseStarExpr();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(2UL, node.End);
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

            var node = (RelationExpression) parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.Less, node.RelationKind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(5UL, node.End);
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

            var node = (RelationExpression) parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.LessEqual, node.RelationKind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(6UL, node.End);
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

            var node = (RelationExpression) parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.Equal, node.RelationKind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(6UL, node.End);
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

            var node = (RelationExpression) parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.NotEqual, node.RelationKind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(6UL, node.End);
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

            var node = (RelationExpression) parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.NotEqual, node.RelationKind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(6UL, node.End);
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

            var node = (RelationExpression) parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.GreaterEqual, node.RelationKind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(6UL, node.End);
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

            var node = (RelationExpression) parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.Greater, node.RelationKind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(5UL, node.End);
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

            var node = (RelationExpression) parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.Is, node.RelationKind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(6UL, node.End);
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

            var node = (RelationExpression) parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.In, node.RelationKind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(6UL, node.End);
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

            var node = (RelationExpression) parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.NotIn, node.RelationKind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(10UL, node.End);
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

            var node = (RelationExpression) parser.ParseComparison();
            Assert.Equal(RelationExpression.Relation.IsNot, node.RelationKind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(10UL, node.End);
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

            var node = (RelationExpression) parser.ParseComparison();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(9UL, node.End);

            // Left part of the last operator
            var node2 = ((RelationExpression) node.Left);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(6UL, node2.End);
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

            var node = (NotTestExpression) parser.ParseNotTest();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(5UL, node.End);
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

            var node = (NotTestExpression) parser.ParseNotTest();
            Assert.Equal(Token.TokenKind.PyNot, node.Operator.Kind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(9UL, node.End);

            var node2 = (NotTestExpression) node.Right;
            Assert.Equal(Token.TokenKind.PyNot, node2.Operator.Kind);
            Assert.Equal(4UL, node2.Start);
            Assert.Equal(9UL, node2.End);
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

            var node = (AndTestExpression) parser.ParseAndTest();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(7UL, node.End);
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

            var node = (AndTestExpression) parser.ParseAndTest();
            Assert.Equal(Token.TokenKind.PyTestAnd, node.Operator.Kind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(13UL, node.End);

            var node2 = (AndTestExpression) node.Left;
            Assert.Equal(Token.TokenKind.PyTestAnd, node2.Operator.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(8UL, node2.End);
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

            var node = (OrTestExpression) parser.ParseOrTest();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(6UL, node.End);
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

            var node = (OrTestExpression) parser.ParseOrTest();
            Assert.Equal(Token.TokenKind.PyTestOr, node.Operator.Kind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(11UL, node.End);

            var node2 = (OrTestExpression) node.Left;
            Assert.Equal(Token.TokenKind.PyTestOr, node2.Operator.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(7UL, node2.End);
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

            var node = (NameLiteralExpression) parser.ParseNoCond();
            Assert.Equal(Token.TokenKind.Name, node.Name.Kind);
            Assert.Equal("__init__", node.Name.Text);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(8UL, node.End);
            Assert.Equal(0UL, node.Name.Start);
            Assert.Equal(8UL, node.Name.End);
        }

        [Fact]
        public void TestNoCondLambdaExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("lambda: a; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (LambdaExpression) parser.ParseNoCond();
            Assert.Equal(Token.TokenKind.PyLambda, node.Operator1.Kind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(9L, node.End);
            Assert.True(node.Left == null);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind);

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

            var node = (NameLiteralExpression) parser.ParseTest();
            Assert.Equal(Token.TokenKind.Name, node.Name.Kind);
            Assert.Equal("__init__", node.Name.Text);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(8UL, node.End);
            Assert.Equal(0UL, node.Name.Start);
            Assert.Equal(8UL, node.Name.End);
        }

        [Fact]
        public void TestuleTestLambdaExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("lambda: a; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (LambdaExpression) parser.ParseTest();
            Assert.Equal(Token.TokenKind.PyLambda, node.Operator1.Kind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(9L, node.End);
            Assert.True(node.Left == null);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind);

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

            var node = (ConditionalExpression) parser.ParseTest();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(20UL, node.End);

            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal("__init__", ((NameLiteralExpression) node.Left).Name.Text);

            Assert.Equal(Token.TokenKind.PyIf, node.Operator1.Kind);
            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Right).Name.Text);

            Assert.Equal(Token.TokenKind.PyElse, node.Operator2.Kind);
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

            var node = (NameLiteralExpression) parser.ParseNamedExpr();
            Assert.Equal(Token.TokenKind.Name, node.Name.Kind);
            Assert.Equal("__init__", node.Name.Text);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(8UL, node.End);
            Assert.Equal(0UL, node.Name.Start);
            Assert.Equal(8UL, node.Name.End);
        }

        [Fact]
        public void TestRuleNamedExprWithColonAssignExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("__init__ := a; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (NamedExpression) parser.ParseNamedExpr();
            Assert.Equal(Token.TokenKind.PyColonAssign, node.Operator.Kind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(13UL, node.End);

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

            var node = (YieldExpression) parser.ParseYieldExpr();
            Assert.Equal(Token.TokenKind.PyYield, node.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyFrom, node.Operator2.Kind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(19UL, node.End);

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

            var node = (YieldExpression) parser.ParseYieldExpr();
            Assert.Equal(Token.TokenKind.PyYield, node.Operator1.Kind);
            Assert.True(node.Operator2 == null);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(14UL, node.End);

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

            var node = (CompIfExpression) parser.ParseCompIter();
            Assert.Equal(Token.TokenKind.PyIf, node.Operator.Kind);
            Assert.True(node.Next == null);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(4UL, node.End);

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

            var node = (CompIfExpression) parser.ParseCompIter();
            Assert.Equal(Token.TokenKind.PyIf, node.Operator.Kind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(9UL, node.End);

            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Right).Name.Text);
            Assert.Equal(3U, ((NameLiteralExpression) node.Right).Name.Start);
            Assert.Equal(4U, ((NameLiteralExpression) node.Right).Name.End);

            var node2 = (CompIfExpression) node.Next;
            Assert.Equal(Token.TokenKind.PyIf, node2.Operator.Kind);
            Assert.Equal(5UL, node2.Start);
            Assert.Equal(9UL, node2.End);
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

            var node = (CompSyncForExpression) parser.ParseCompIter();
            Assert.Equal(Token.TokenKind.PyFor, node.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyIn, node.Operator2.Kind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(10UL, node.End);

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

            var node = (CompSyncForExpression) parser.ParseCompIter();
            Assert.Equal(Token.TokenKind.PyFor, node.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyIn, node.Operator2.Kind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(15UL, node.End);

            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Left).Name.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Left).Name.Text);
            Assert.Equal(4U, ((NameLiteralExpression) node.Left).Name.Start);
            Assert.Equal(5U, ((NameLiteralExpression) node.Left).Name.End);

            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node.Right).Name.Text);
            Assert.Equal(9U, ((NameLiteralExpression) node.Right).Name.Start);
            Assert.Equal(10U, ((NameLiteralExpression) node.Right).Name.End);

            var node2 = (CompIfExpression) node.Next;
            Assert.Equal(Token.TokenKind.PyIf, node2.Operator.Kind);
            Assert.Equal("c", ((NameLiteralExpression) node2.Right).Name.Text);
            Assert.Equal(14U, ((NameLiteralExpression) node2.Right).Name.Start);
            Assert.Equal(15U, ((NameLiteralExpression) node2.Right).Name.End);
        }

        [Fact]
        public void TestSingleCompAsyncForExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("async for a in b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node2 = (CompForExpression) parser.ParseCompFor();
            Assert.Equal(Token.TokenKind.PyAsync, node2.AsyncOperator.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(16UL, node2.End);

            var node = (CompSyncForExpression) node2.Right;
            Assert.Equal(Token.TokenKind.PyFor, node.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyIn, node.Operator2.Kind);
            Assert.Equal(6UL, node.Start);
            Assert.Equal(16UL, node.End);

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

            var node = (StarArgument) parser.ParseArgument();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(2UL, node.End);

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

            var node = (PowerArgument) parser.ParseArgument();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(3UL, node.End);

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

            var node = (ArgumentExpression) parser.ParseArgument();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(1UL, node.End);

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

            var node = (ArgumentExpression) parser.ParseArgument();
            Assert.Equal(Token.TokenKind.PyAssign, node.Operator.Kind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(5UL, node.End);

            Assert.Equal(Token.TokenKind.Name, node.Left.Kind);
            Assert.Equal("a", node.Left.Text);
            Assert.Equal(0U, node.Left.Start);
            Assert.Equal(1U, node.Left.End);

            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node.Right).Name.Text);
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

            var node = (ArgumentExpression) parser.ParseArgument();
            Assert.Equal(Token.TokenKind.PyColonAssign, node.Operator.Kind);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(6UL, node.End);

            Assert.Equal(Token.TokenKind.Name, node.Left.Kind);
            Assert.Equal("a", node.Left.Text);
            Assert.Equal(0U, node.Left.Start);
            Assert.Equal(1U, node.Left.End);

            Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression) node.Right).Name.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node.Right).Name.Text);
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

            var node = (ArgumentExpression) parser.ParseArgument();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(12UL, node.End);

            Assert.Equal(Token.TokenKind.Name, node.Left.Kind);
            Assert.Equal("a", node.Left.Text);
            Assert.Equal(0U, node.Left.Start);
            Assert.Equal(1U, node.Left.End);

            var node2 = (CompSyncForExpression) node.Right;
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

            var node = (ArgumentExpression) parser.ParseArgList();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(1UL, node.End);

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

            var node = (ListExpression) parser.ParseArgList();
            Assert.Equal(ListExpression.ListType.ArgumentList, node.ContainerType);
            Assert.Equal(1UL, node.Start);
            Assert.Equal(3UL, node.End);
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

            var node = (ListExpression) parser.ParseArgList();
            Assert.Equal(ListExpression.ListType.ArgumentList, node.ContainerType);
            Assert.Equal(1UL, node.Start);
            Assert.Equal(5UL, node.End);
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

            var node = (ListExpression) parser.ParseArgList();
            Assert.Equal(ListExpression.ListType.ArgumentList, node.ContainerType);
            Assert.Equal(1UL, node.Start);
            Assert.Equal(6UL, node.End);
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

            var node = (NameLiteralExpression) parser.ParseTestList();
            Assert.Equal("a", node.Name.Text);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(1UL, node.End);
        }

        [Fact]
        public void TestATestList2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a,; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (ListExpression) parser.ParseTestList();
            Assert.Equal(ListExpression.ListType.TestList, node.ContainerType);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(2UL, node.End);
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

            var node = (ListExpression) parser.ParseTestList();
            Assert.Equal(ListExpression.ListType.TestList, node.ContainerType);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(4UL, node.End);
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

            var node = (ListExpression) parser.ParseTestList();
            Assert.Equal(ListExpression.ListType.TestList, node.ContainerType);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(5UL, node.End);
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

            var node = (StarExpression) parser.ParseExprList();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(2UL, node.End);
        }

        [Fact]
        public void TestAExprList2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a: ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (NameLiteralExpression) parser.ParseExprList();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(1UL, node.End);
        }

        [Fact]
        public void TestAExprList3()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("*a,: ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (ListExpression) parser.ParseExprList();
            Assert.Equal(ListExpression.ListType.ExprList, node.ContainerType);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(3UL, node.End);

            Assert.True(node.Elements.Length == 1);
            var node2 = (StarExpression) node.Elements[0];
            Assert.Equal("a", ((NameLiteralExpression) node2.Right).Name.Text);

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

            var node = (ListExpression) parser.ParseExprList();
            Assert.Equal(ListExpression.ListType.ExprList, node.ContainerType);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(2UL, node.End);

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

            var node = (ListExpression) parser.ParseExprList();
            Assert.Equal(ListExpression.ListType.ExprList, node.ContainerType);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(5UL, node.End);

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

            var node = (ListExpression) parser.ParseExprList();
            Assert.Equal(ListExpression.ListType.ExprList, node.ContainerType);
            Assert.Equal(0UL, node.Start);
            Assert.Equal(5UL, node.End);

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

            var node = (SubscriptExpression) parser.ParseSubscript();
            Assert.Equal(1UL, node.Start);
            Assert.Equal(2UL, node.End);

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

            var node = (SubscriptExpression) parser.ParseSubscript();
            Assert.Equal(1UL, node.Start);
            Assert.Equal(2UL, node.End);

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

            var node = (SubscriptExpression) parser.ParseSubscript();
            Assert.Equal(1UL, node.Start);
            Assert.Equal(3UL, node.End);

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

            var node = (SubscriptExpression) parser.ParseSubscript();
            Assert.Equal(1UL, node.Start);
            Assert.Equal(3UL, node.End);

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

            var node = (SubscriptExpression) parser.ParseSubscript();
            Assert.Equal(1UL, node.Start);
            Assert.Equal(4UL, node.End);

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

            var node = (SubscriptExpression) parser.ParseSubscript();
            Assert.Equal(1UL, node.Start);
            Assert.Equal(6UL, node.End);

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

            var node = (SubscriptExpression) parser.ParseSubscript();
            Assert.Equal(1UL, node.Start);
            Assert.Equal(7UL, node.End);

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

            var node = (SubscriptExpression) parser.ParseSubscript();
            Assert.Equal(1UL, node.Start);
            Assert.Equal(5UL, node.End);

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

            var node = (SubscriptExpression) parser.ParseSubscriptList();
            Assert.Equal(1UL, node.Start);
            Assert.Equal(5UL, node.End);

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

            var node = (ListExpression) parser.ParseSubscriptList();
            Assert.Equal(ListExpression.ListType.SubscriptList, node.ContainerType);
            Assert.Equal(1UL, node.Start);
            Assert.Equal(6UL, node.End);

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

            var node = (ListExpression) parser.ParseSubscriptList();
            Assert.Equal(ListExpression.ListType.SubscriptList, node.ContainerType);
            Assert.Equal(1UL, node.Start);
            Assert.Equal(8UL, node.End);

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

            var node = (ListExpression) parser.ParseSubscriptList();
            Assert.Equal(ListExpression.ListType.SubscriptList, node.ContainerType);
            Assert.Equal(1UL, node.Start);
            Assert.Equal(9UL, node.End);

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

            var node = (TupleExpression) parser.ParseAtom();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(19UL, node.End);
            var node2 = (YieldExpression) node.Right;
            Assert.Equal(2U, node2.Start);
        }

        [Fact]
        public void TestTuple2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("( *a ); ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (TupleExpression) parser.ParseAtom();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(6UL, node.End);

            var node2 = (StarExpression) node.Right;
            Assert.Equal(2U, node2.Start);
        }

        [Fact]
        public void TestTuple3()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("( *a for b in c ); ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (TupleExpression) parser.ParseAtom();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(17UL, node.End);

            var node2 = (ListExpression) node.Right;
            Assert.True(node2.Elements.Length == 2);

            var node4 = (StarExpression) node2.Elements[0];
            Assert.Equal("a", ((NameLiteralExpression) node4.Right).Name.Text);

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

            var node = (TupleExpression) parser.ParseAtom();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(7UL, node.End);

            var node2 = (ListExpression) node.Right;
            Assert.True(node2.Elements.Length == 1);

            var node4 = (StarExpression) node2.Elements[0];
            Assert.Equal("a", ((NameLiteralExpression) node4.Right).Name.Text);

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

            var node = (TupleExpression) parser.ParseAtom();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(14UL, node.End);

            var node2 = (ListExpression) node.Right;
            Assert.True(node2.Elements.Length == 2);

            var node4 = (StarExpression) node2.Elements[0];
            Assert.Equal("a", ((NameLiteralExpression) node4.Right).Name.Text);

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

            var node = (TupleExpression) parser.ParseAtom();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(15UL, node.End);

            var node2 = (ListExpression) node.Right;
            Assert.True(node2.Elements.Length == 2);

            var node4 = (StarExpression) node2.Elements[0];
            Assert.Equal("a", ((NameLiteralExpression) node4.Right).Name.Text);

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

            var node = (TupleExpression) parser.ParseAtom();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(15UL, node.End);

            var node2 = (ListExpression) node.Right;
            Assert.Equal(ListExpression.ListType.TestListStarExpr, node2.ContainerType);

            var node3 = (NamedExpression) node2.Elements[0];
            Assert.Equal(2U, node3.Start);

            var node4 = (StarExpression) node2.Elements[1];
            Assert.Equal("b", ((NameLiteralExpression) node4.Right).Name.Text);

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

            var node = (AtomListExpression) parser.ParseAtom();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(15UL, node.End);

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

            var node = (SetExpression) parser.ParseAtom();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(5UL, node.End);

            var node2 = (SetContainerExpression) node.Right;
            Assert.True(node2.Keys.Length == 1);
            Assert.Equal("a", ((NameLiteralExpression) node2.Keys[0]).Name.Text);

            Assert.True(node2.Separators.Length == 0);
        }

        [Fact]
        public void TestAtomSet2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{ a for b in c }; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (SetExpression) parser.ParseAtom();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(16UL, node.End);

            var node2 = (SetContainerExpression) node.Right;
            Assert.True(node2.Keys.Length == 2);
            Assert.Equal("a", ((NameLiteralExpression) node2.Keys[0]).Name.Text);
            Assert.Equal(4U, ((CompSyncForExpression) node2.Keys[1]).Start);

            Assert.True(node2.Separators.Length == 0);
        }

        [Fact]
        public void TestAtomSet3()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{ a, }; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (SetExpression) parser.ParseAtom();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(6UL, node.End);

            var node2 = (SetContainerExpression) node.Right;
            Assert.True(node2.Keys.Length == 1);
            Assert.Equal("a", ((NameLiteralExpression) node2.Keys[0]).Name.Text);

            Assert.True(node2.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[0].Kind);
        }

        [Fact]
        public void TestAtomSet4()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{ a, b }; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (SetExpression) parser.ParseAtom();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(8UL, node.End);

            var node2 = (SetContainerExpression) node.Right;
            Assert.True(node2.Keys.Length == 2);
            Assert.Equal("a", ((NameLiteralExpression) node2.Keys[0]).Name.Text);
            Assert.Equal("b", ((NameLiteralExpression) node2.Keys[1]).Name.Text);

            Assert.True(node2.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[0].Kind);
        }

        [Fact]
        public void TestAtomSet5()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{ a, b, }; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (SetExpression) parser.ParseAtom();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(9UL, node.End);

            var node2 = (SetContainerExpression) node.Right;
            Assert.True(node2.Keys.Length == 2);
            Assert.Equal("a", ((NameLiteralExpression) node2.Keys[0]).Name.Text);
            Assert.Equal("b", ((NameLiteralExpression) node2.Keys[1]).Name.Text);

            Assert.True(node2.Separators.Length == 2);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[0].Kind);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[1].Kind);
        }

        [Fact]
        public void TestAtomSet6()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{ a, b, *c }; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (SetExpression) parser.ParseAtom();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(12UL, node.End);

            var node2 = (SetContainerExpression) node.Right;
            Assert.True(node2.Keys.Length == 3);
            Assert.Equal("a", ((NameLiteralExpression) node2.Keys[0]).Name.Text);
            Assert.Equal("b", ((NameLiteralExpression) node2.Keys[1]).Name.Text);

            var node3 = (StarExpression) node2.Keys[2];
            Assert.True(node3.Start == 8U);

            Assert.True(node2.Separators.Length == 2);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[0].Kind);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[1].Kind);
        }

        [Fact]
        public void TestAtomSet7()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{ *a, b, *c }; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (SetExpression) parser.ParseAtom();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(13UL, node.End);

            var node2 = (SetContainerExpression) node.Right;
            Assert.True(node2.Keys.Length == 3);
            Assert.Equal("b", ((NameLiteralExpression) node2.Keys[1]).Name.Text);

            var node3 = (StarExpression) node2.Keys[2];
            Assert.Equal(9U, node3.Start);

            var node4 = (StarExpression) node2.Keys[0];
            Assert.Equal(2U, node4.Start);

            Assert.True(node2.Separators.Length == 2);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[0].Kind);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[1].Kind);
        }

        [Fact]
        public void TestAtomDictionary1()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{ a : b }; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (DictionaryExpression) parser.ParseAtom();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(9UL, node.End);

            var node2 = (DictionaryContainerExpression) node.Right;
            Assert.True(node2.Keys.Length == 1);

            Assert.Equal("a", ((NameLiteralExpression) node2.Keys[0]).Name.Text);
            Assert.True(node2.Colons.Length == 1);
            Assert.Equal(Token.TokenKind.PyColon, node2.Colons[0].Kind);
            Assert.Equal("b", ((NameLiteralExpression) node2.Values[0]).Name.Text);

            Assert.True(node2.Separators.Length == 0);
        }
        
        [Fact]
        public void TestAtomDictionary2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{ a : b for c in d }; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (DictionaryExpression)parser.ParseAtom();
            Assert.Equal(0UL, node.Start );
            Assert.Equal(20UL, node.End );

            var node2 = (DictionaryContainerExpression)node.Right;
            Assert.True(node2.Keys.Length == 2);
            
            Assert.Equal("a", ((NameLiteralExpression)node2.Keys[0]).Name.Text);
            Assert.True(node2.Colons.Length == 1);
            Assert.Equal(Token.TokenKind.PyColon, node2.Colons[0].Kind);
            Assert.Equal("b", ((NameLiteralExpression)node2.Values[0]).Name.Text);

            var node3 = (CompSyncForExpression)node2.Keys[1];
            Assert.Equal(8U, node3.Start);
            
            Assert.True(node2.Separators.Length == 0);
        }
        
        [Fact]
        public void TestAtomDictionary3()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{ a : b, }; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (DictionaryExpression) parser.ParseAtom();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(10UL, node.End);

            var node2 = (DictionaryContainerExpression) node.Right;
            Assert.True(node2.Keys.Length == 1);

            Assert.Equal("a", ((NameLiteralExpression) node2.Keys[0]).Name.Text);
            Assert.True(node2.Colons.Length == 1);
            Assert.Equal(Token.TokenKind.PyColon, node2.Colons[0].Kind);
            Assert.Equal("b", ((NameLiteralExpression) node2.Values[0]).Name.Text);

            Assert.True(node2.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[0].Kind);
        }
        
        [Fact]
        public void TestAtomDictionary4()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{ a : b, c : d }; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (DictionaryExpression) parser.ParseAtom();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(16UL, node.End);

            var node2 = (DictionaryContainerExpression) node.Right;
            Assert.True(node2.Keys.Length == 2);

            Assert.Equal("a", ((NameLiteralExpression) node2.Keys[0]).Name.Text);
            Assert.True(node2.Colons.Length == 2);
            Assert.Equal(Token.TokenKind.PyColon, node2.Colons[0].Kind);
            Assert.Equal("b", ((NameLiteralExpression) node2.Values[0]).Name.Text);
            
            Assert.Equal("c", ((NameLiteralExpression) node2.Keys[1]).Name.Text);
            Assert.Equal(Token.TokenKind.PyColon, node2.Colons[1].Kind);
            Assert.Equal("d", ((NameLiteralExpression) node2.Values[1]).Name.Text);

            Assert.True(node2.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[0].Kind);
        }
        
        [Fact]
        public void TestAtomDictionary5()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{ a : b, c : d, }; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (DictionaryExpression) parser.ParseAtom();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(17UL, node.End);

            var node2 = (DictionaryContainerExpression) node.Right;
            Assert.True(node2.Keys.Length == 2);

            Assert.Equal("a", ((NameLiteralExpression) node2.Keys[0]).Name.Text);
            Assert.True(node2.Colons.Length == 2);
            Assert.Equal(Token.TokenKind.PyColon, node2.Colons[0].Kind);
            Assert.Equal("b", ((NameLiteralExpression) node2.Values[0]).Name.Text);
            
            Assert.Equal("c", ((NameLiteralExpression) node2.Keys[1]).Name.Text);
            Assert.Equal(Token.TokenKind.PyColon, node2.Colons[1].Kind);
            Assert.Equal("d", ((NameLiteralExpression) node2.Values[1]).Name.Text);

            Assert.True(node2.Separators.Length == 2);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[0].Kind);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[1].Kind);
        }
        
        [Fact]
        public void TestAtomDictionary6()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{ a : b, **c }; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (DictionaryExpression) parser.ParseAtom();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(14UL, node.End);

            var node2 = (DictionaryContainerExpression) node.Right;
            Assert.True(node2.Keys.Length == 2);

            Assert.Equal("a", ((NameLiteralExpression) node2.Keys[0]).Name.Text);
            Assert.True(node2.Colons.Length == 2);
            Assert.Equal(Token.TokenKind.PyColon, node2.Colons[0].Kind);
            Assert.Equal("b", ((NameLiteralExpression) node2.Values[0]).Name.Text);

            var node3 = (PowerKeyExpression) node2.Keys[1];
            Assert.Equal(Token.TokenKind.PyPower, node3.Operator.Kind);
            Assert.Equal("c", ((NameLiteralExpression)node3.Right).Name.Text );

            Assert.True(node2.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[0].Kind);
        }
        
        [Fact]
        public void TestAtomDictionary8()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{ **a, b : c, **d }; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (DictionaryExpression) parser.ParseAtom();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(19UL, node.End);

            var node2 = (DictionaryContainerExpression) node.Right;
            Assert.True(node2.Keys.Length == 3);
            
            var node4 = (PowerKeyExpression) node2.Keys[0];
            Assert.Equal(Token.TokenKind.PyPower, node4.Operator.Kind);
            Assert.Equal("a", ((NameLiteralExpression)node4.Right).Name.Text );

            Assert.Equal("b", ((NameLiteralExpression) node2.Keys[1]).Name.Text);
            Assert.Equal(3, node2.Colons.Length); // Needs to be align.
            Assert.Equal(Token.TokenKind.PyColon, node2.Colons[1].Kind);
            Assert.Equal("c", ((NameLiteralExpression) node2.Values[1]).Name.Text);

            var node3 = (PowerKeyExpression) node2.Keys[2];
            Assert.Equal(Token.TokenKind.PyPower, node3.Operator.Kind);
            Assert.Equal("d", ((NameLiteralExpression)node3.Right).Name.Text );

            Assert.True(node2.Separators.Length == 2);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[0].Kind);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[1].Kind);
        }
        
        [Fact]
        public void TestAtomIllegal()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("pass ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(0U, e.Position);
                Assert.Equal(Token.TokenKind.PyPass, e.ErrorSymbol.Kind);
                Assert.Equal("Illegal literal!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestTupleAtomIllegal()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("( a; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(3U, e.Position);
                Assert.Equal(Token.TokenKind.PySemiColon, e.ErrorSymbol.Kind);
                Assert.Equal("Expecting ')' in tuple declaration!", e.Message);
            }
            catch
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestListAtomIllegal()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("[ a; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(3U, e.Position);
                Assert.Equal(Token.TokenKind.PySemiColon, e.ErrorSymbol.Kind);
                Assert.Equal("Expecting ']' in tuple declaration!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestDictionaryAtomIllegal()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{ a : b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(7U, e.Position);
                Assert.Equal(Token.TokenKind.PySemiColon, e.ErrorSymbol.Kind);
                Assert.Equal("Expecting '}' in dictionary or set declaration!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestTrailerCallMissingEndParenthis()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a(b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(3U, e.Position);
                Assert.Equal(Token.TokenKind.PySemiColon, e.ErrorSymbol.Kind);
                Assert.Equal("Expecting ')'", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestTrailerIndexMissingEndBracket()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a[b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(3U, e.Position);
                Assert.Equal(Token.TokenKind.PySemiColon, e.ErrorSymbol.Kind);
                Assert.Equal("Expecting ']'", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestTrailerMissingDotName()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a.; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(2U, e.Position);
                Assert.Equal(Token.TokenKind.PySemiColon, e.ErrorSymbol.Kind);
                Assert.Equal("Expecting name literal after '.'", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestNotMissingIn()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a not b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(6U, e.Position);
                Assert.Equal(Token.TokenKind.Name, e.ErrorSymbol.Kind);
                Assert.Equal("Expecting 'not' 'in' , but missing 'in' in relation expression!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestLambadMissingColon()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("lambda a b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(9U, e.Position);
                Assert.Equal(Token.TokenKind.Name, e.ErrorSymbol.Kind);
                Assert.Equal("Expecting ':' in lambda expression!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestExpressionTestWithoutElse()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a if b c; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(7U, e.Position);
                Assert.Equal(Token.TokenKind.Name, e.ErrorSymbol.Kind);
                Assert.Equal("Missing 'else' in test expression!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestTupleWithDoubleCommas()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("(a,,b); ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(3U, e.Position);
                Assert.Equal(Token.TokenKind.PyComma, e.ErrorSymbol.Kind);
                Assert.Equal("Double ',' in expression list!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestSubscriptListWithDoubleCommas()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a[b,,c]; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(4U, e.Position);
                Assert.Equal(Token.TokenKind.PyComma, e.ErrorSymbol.Kind);
                Assert.Equal("Double ',' in expression list!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestExprListWithDoubleCommas()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a(b for c,,d in e); ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(10U, e.Position);
                Assert.Equal(Token.TokenKind.PyComma, e.ErrorSymbol.Kind);
                Assert.Equal("Double ',' in expression list!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestTestListWithDoubleCommas()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a,, b; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseTestList();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(2U, e.Position);
                Assert.Equal(Token.TokenKind.PyComma, e.ErrorSymbol.Kind);
                Assert.Equal("Double ',' in expression list!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestArgumentListWithDoubleCommas()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a(a,,b); ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(4U, e.Position);
                Assert.Equal(Token.TokenKind.PyComma, e.ErrorSymbol.Kind);
                Assert.Equal("Double ',' in expression list!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestArgumentWithMulMissingName()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a(*, b); ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(3U, e.Position);
                Assert.Equal(Token.TokenKind.PyComma, e.ErrorSymbol.Kind);
                Assert.Equal("Expecting name literal in '*' argument!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestArgumentWithPowerMissingName()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a(**, b); ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(4U, e.Position);
                Assert.Equal(Token.TokenKind.PyComma, e.ErrorSymbol.Kind);
                Assert.Equal("Expecting name literal in '**' argument!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestArgumentMissingName()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a(,b); ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(2U, e.Position);
                Assert.Equal(Token.TokenKind.PyComma, e.ErrorSymbol.Kind);
                Assert.Equal("Expecting valid argument!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestArgumentWithCompForMissingFor()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a(b async c in d); ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(10U, e.Position);
                Assert.Equal(Token.TokenKind.Name, e.ErrorSymbol.Kind);
                Assert.Equal("Expecting 'for' in for comprehension expression!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestArgumentWithCompForMissingIn()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a(b async for c d); ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(16U, e.Position);
                Assert.Equal(Token.TokenKind.Name, e.ErrorSymbol.Kind);
                Assert.Equal("Expecting 'in' in for comprehension expression!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestArgumentWithCompIfMissingExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a(b async for c in d if ); ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(24U, e.Position);
                Assert.Equal(Token.TokenKind.PyRightParen, e.ErrorSymbol.Kind);
                Assert.Equal("Illegal literal!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestSetMissingNameAfterMul()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{ * }; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(4U, e.Position);
                Assert.Equal(Token.TokenKind.PyRightCurly, e.ErrorSymbol.Kind);
                Assert.Equal("Illegal literal!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestDictionaryMissingNameAfterPower()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{ ** }; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(5U, e.Position);
                Assert.Equal(Token.TokenKind.PyRightCurly, e.ErrorSymbol.Kind);
                Assert.Equal("Illegal literal!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestDictionaryMissingValue()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{ a : , }; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(6U, e.Position);
                Assert.Equal(Token.TokenKind.PyComma, e.ErrorSymbol.Kind);
                Assert.Equal("Illegal literal!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestSetMissingKey()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("{ , b }; ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(2U, e.Position);
                Assert.Equal(Token.TokenKind.PyComma, e.ErrorSymbol.Kind);
                Assert.Equal("Illegal literal!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestYieldFromMissingExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("( yield from ); ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(13U, e.Position);
                Assert.Equal(Token.TokenKind.PyRightParen, e.ErrorSymbol.Kind);
                Assert.Equal("Illegal literal!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        [Fact]
        public void TestYieldMissingExpression()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("( yield ); ".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            try
            {
                parser.ParseNamedExpr();
                Assert.True(false);
            }
            catch (SyntaxErrorException e)
            {
                Assert.Equal(8U, e.Position);
                Assert.Equal(Token.TokenKind.PyRightParen, e.ErrorSymbol.Kind);
                Assert.Equal("Illegal literal!", e.Message);
            }
            catch 
            {
                Assert.True(false);
            }
        }
        
        
        
        // Statement rules is tested below!
        
        [Fact]
        public void TestPassStmt()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("pass; \r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            Assert.True(node.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PySemiColon, node.Separators[0].Kind);

            var node2 = (PassStatement) node.Elements[0];
            Assert.Equal(Token.TokenKind.PyPass, node2.Operator.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(4UL, node2.End);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestPassStmtMultiple()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("pass; pass\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 2);
            
            Assert.True(node.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PySemiColon, node.Separators[0].Kind);

            var node2 = (PassStatement) node.Elements[0];
            Assert.Equal(Token.TokenKind.PyPass, node2.Operator.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(4UL, node2.End);
            
            var node3 = (PassStatement) node.Elements[1];
            Assert.Equal(Token.TokenKind.PyPass, node3.Operator.Kind);
            Assert.Equal(6UL, node3.Start);
            Assert.Equal(10UL, node3.End);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestDelStmt()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("del a, b, c;\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            Assert.True(node.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PySemiColon, node.Separators[0].Kind);

            var node2 = (DelStatement) node.Elements[0];
            Assert.Equal(Token.TokenKind.PyDel, node2.Operator.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(11UL, node2.End);

            var node3 = (ListExpression)node2.Right;
            Assert.Equal(ListExpression.ListType.ExprList, node3.ContainerType);
            Assert.True(node3.Elements.Length == 3);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestBreakStmt()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("break\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (FlowStatement) node.Elements[0];
            Assert.Equal(FlowStatement.OperatorKind.Break, node2.Kind);
            Assert.Equal(Token.TokenKind.PyBreak, node2.Operator.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(5UL, node2.End);
            
            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestContinueStmt()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("continue\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (FlowStatement) node.Elements[0];
            Assert.Equal(FlowStatement.OperatorKind.Continue, node2.Kind);
            Assert.Equal(Token.TokenKind.PyContinue, node2.Operator.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(8UL, node2.End);
            
            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestEmptyReturnStmt()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("return\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (FlowStatement) node.Elements[0];
            Assert.Equal(FlowStatement.OperatorKind.Return, node2.Kind);
            Assert.Equal(Token.TokenKind.PyReturn, node2.Operator.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(6UL, node2.End);
            
            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestReturnStmt()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("return 0, b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (FlowStatement) node.Elements[0];
            Assert.Equal(FlowStatement.OperatorKind.Return, node2.Kind);
            Assert.Equal(Token.TokenKind.PyReturn, node2.Operator.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(11UL, node2.End);
            
            var node3 = (ListExpression)node2.Right;
            Assert.Equal(ListExpression.ListType.TestListStarExpr, node3.ContainerType);
            Assert.True(node3.Elements.Length == 2);
            
            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestYieldFromStmt()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("yield from a\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (PlainExpressionStatement) node.Elements[0];
            Assert.Equal(Token.TokenKind.PyYield, ((YieldExpression)node2.Node).Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyFrom, ((YieldExpression)node2.Node).Operator2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(12UL, node2.End);
            
            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestYieldStmt()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("yield a\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (PlainExpressionStatement) node.Elements[0];
            Assert.Equal(Token.TokenKind.PyYield, ((YieldExpression)node2.Node).Operator1.Kind);
            Assert.True(((YieldExpression)node2.Node).Operator2 == null);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(7UL, node2.End);
            
            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestRaiseZeroArgumentStmt()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("raise\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (FlowStatement) node.Elements[0];
            Assert.Equal(FlowStatement.OperatorKind.Raise, node2.Kind);
            Assert.Equal(Token.TokenKind.PyRaise, node2.Operator.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(5UL, node2.End);

            Assert.True(node2.Left == null);
            Assert.True(node2.Right == null);
            Assert.True(node2.Operator2 == null);
            
            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestRaiseOneArgumentStmt()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("raise a\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (FlowStatement) node.Elements[0];
            Assert.Equal(FlowStatement.OperatorKind.Raise, node2.Kind);
            Assert.Equal(Token.TokenKind.PyRaise, node2.Operator.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(7UL, node2.End);

            Assert.Equal("a" , ((NameLiteralExpression)node2.Left).Name.Text);
            Assert.True(node2.Right == null);
            Assert.True(node2.Operator2 == null);
            
            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestRaiseTwoArgumentStmt()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("raise a from b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();

            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (FlowStatement) node.Elements[0];
            Assert.Equal(FlowStatement.OperatorKind.Raise, node2.Kind);
            Assert.Equal(Token.TokenKind.PyRaise, node2.Operator.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(14UL, node2.End);

            Assert.Equal("a" , ((NameLiteralExpression)node2.Left).Name.Text);
            Assert.Equal("b" , ((NameLiteralExpression)node2.Right).Name.Text);
            Assert.Equal(Token.TokenKind.PyFrom, node2.Operator2.Kind);
            
            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestImportStmt1()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("import a\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (ImportStatement) node.Elements[0];
            Assert.Equal(Token.TokenKind.PyImport, node2.Operaor.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(8UL, node2.End);
            
            var node3 = (ImportListStatement)node2.Right;
            Assert.True(node3.Elements.Length == 1);

            var node4 = (DottedNameStatement)node3.Elements[0];
            Assert.True(node4.Names.Length == 1);
            Assert.True(node4.Dots.Length == 0);
            Assert.Equal("a", node4.Names[0].Text);
                            
            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestImportStmt2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("import a.b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (ImportStatement) node.Elements[0];
            Assert.Equal(Token.TokenKind.PyImport, node2.Operaor.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(10UL, node2.End);
            
            var node3 = (ImportListStatement)node2.Right;
            Assert.True(node3.Elements.Length == 1);

            var node4 = (DottedNameStatement)node3.Elements[0];
            Assert.True(node4.Names.Length == 2);
            Assert.True(node4.Dots.Length == 1);
            Assert.Equal("a", node4.Names[0].Text);
            Assert.Equal("b", node4.Names[1].Text);
                            
            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestImportStmt3()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("import a.b, c\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (ImportStatement) node.Elements[0];
            Assert.Equal(Token.TokenKind.PyImport, node2.Operaor.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(13UL, node2.End);
            
            var node3 = (ImportListStatement)node2.Right;
            Assert.True(node3.Elements.Length == 2);
            Assert.True(node3.Separators.Length == 1);

            var node4 = (DottedNameStatement)node3.Elements[0];
            Assert.True(node4.Names.Length == 2);
            Assert.True(node4.Dots.Length == 1);
            Assert.Equal("a", node4.Names[0].Text);
            Assert.Equal("b", node4.Names[1].Text);
                            
            var node5 = (DottedNameStatement)node3.Elements[1];
            Assert.True(node5.Names.Length == 1);
            Assert.True(node5.Dots.Length == 0);
            Assert.Equal("c", node5.Names[0].Text);
            
            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestImportStmt4()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("import a.b, c, d as e\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (ImportStatement) node.Elements[0];
            Assert.Equal(Token.TokenKind.PyImport, node2.Operaor.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(21UL, node2.End);
            
            var node3 = (ImportListStatement)node2.Right;
            Assert.True(node3.Elements.Length == 3);
            Assert.True(node3.Separators.Length == 2);

            var node4 = (DottedNameStatement)node3.Elements[0];
            Assert.True(node4.Names.Length == 2);
            Assert.True(node4.Dots.Length == 1);
            Assert.Equal("a", node4.Names[0].Text);
            Assert.Equal("b", node4.Names[1].Text);
                            
            var node5 = (DottedNameStatement)node3.Elements[1];
            Assert.True(node5.Names.Length == 1);
            Assert.True(node5.Dots.Length == 0);
            Assert.Equal("c", node5.Names[0].Text);
            
            var node6 = (DottedAsNameStatement)node3.Elements[2];
            Assert.Equal("e", node6.Name.Text);
            Assert.Equal(Token.TokenKind.PyAs, node6.Operator.Kind);
            var node7 = (DottedNameStatement) node6.Left;
            Assert.Equal("d", node7.Names[0].Text);
            
            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestImportStmt5()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("import a.b, c, d as e, f.g\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (ImportStatement) node.Elements[0];
            Assert.Equal(Token.TokenKind.PyImport, node2.Operaor.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(26UL, node2.End);
            
            var node3 = (ImportListStatement)node2.Right;
            Assert.True(node3.Elements.Length == 4);
            Assert.True(node3.Separators.Length == 3);

            var node4 = (DottedNameStatement)node3.Elements[0];
            Assert.True(node4.Names.Length == 2);
            Assert.True(node4.Dots.Length == 1);
            Assert.Equal("a", node4.Names[0].Text);
            Assert.Equal("b", node4.Names[1].Text);
                            
            var node5 = (DottedNameStatement)node3.Elements[1];
            Assert.True(node5.Names.Length == 1);
            Assert.True(node5.Dots.Length == 0);
            Assert.Equal("c", node5.Names[0].Text);
            
            var node6 = (DottedAsNameStatement)node3.Elements[2];
            Assert.Equal("e", node6.Name.Text);
            Assert.Equal(Token.TokenKind.PyAs, node6.Operator.Kind);
            var node7 = (DottedNameStatement) node6.Left;
            Assert.Equal("d", node7.Names[0].Text);
            
            var node8 = (DottedNameStatement)node3.Elements[3];
            Assert.True(node8.Names.Length == 2);
            Assert.True(node8.Dots.Length == 1);
            Assert.Equal("f", node8.Names[0].Text);
            Assert.Equal("g", node8.Names[1].Text);
            
            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestImportFromStmt1()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("from a import *\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (ImportFromStatement) node.Elements[0];
            Assert.Equal(Token.TokenKind.PyFrom, node2.Operator1.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(15UL, node2.End);
            
            var left = (DottedNameStatement)node2.Left;
            
            Assert.Equal(Token.TokenKind.PyMul, node2.Operator3.Kind);
            
                            
            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestImportFromStmt2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("from a import ( b, c as d )\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (ImportFromStatement) node.Elements[0];
            Assert.Equal(Token.TokenKind.PyFrom, node2.Operator1.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(27UL, node2.End);
            
            var left = (DottedNameStatement)node2.Left;
            Assert.Equal("a", left.Names[0].Text);
            
            var right = (ImportListStatement)node2.Right;
            Assert.Equal(Token.TokenKind.PyLeftParen, node2.Operator3.Kind);
            Assert.Equal(Token.TokenKind.PyRightParen, node2.Operator4.Kind);
            Assert.True(right.Elements.Length == 2);
            Assert.True(right.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PyComma, right.Separators[0].Kind);

            var elem1 = (ImportAsNameStatement)right.Elements[0];
            Assert.Equal("b", elem1.Left.Text);
            Assert.True(elem1.Operator == null);
            Assert.True(elem1.Right == null);
            
            var elem2 = (ImportAsNameStatement)right.Elements[1];
            Assert.Equal("c", elem2.Left.Text);
            Assert.Equal(Token.TokenKind.PyAs, elem2.Operator.Kind);
            Assert.Equal("d", elem2.Right.Text);


            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestImportFromStmt3()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("from .....a import ( b, c as d )\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (ImportFromStatement) node.Elements[0];
            Assert.Equal(Token.TokenKind.PyFrom, node2.Operator1.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(32UL, node2.End);
            
            var left = (DottedNameStatement)node2.Left;
            Assert.Equal("a", left.Names[0].Text);
            
            // Global in import from statement and not in dottedname statement....
            Assert.Equal(3, node2.Dots.Length);
            Assert.Equal(Token.TokenKind.PyElipsis, node2.Dots[0].Kind);
            Assert.Equal(Token.TokenKind.PyDot, node2.Dots[1].Kind);
            Assert.Equal(Token.TokenKind.PyDot, node2.Dots[2].Kind);
            
            var right = (ImportListStatement)node2.Right;
            Assert.Equal(Token.TokenKind.PyLeftParen, node2.Operator3.Kind);
            Assert.Equal(Token.TokenKind.PyRightParen, node2.Operator4.Kind);
            Assert.True(right.Elements.Length == 2);
            Assert.True(right.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PyComma, right.Separators[0].Kind);

            var elem1 = (ImportAsNameStatement)right.Elements[0];
            Assert.Equal("b", elem1.Left.Text);
            Assert.True(elem1.Operator == null);
            Assert.True(elem1.Right == null);
            
            var elem2 = (ImportAsNameStatement)right.Elements[1];
            Assert.Equal("c", elem2.Left.Text);
            Assert.Equal(Token.TokenKind.PyAs, elem2.Operator.Kind);
            Assert.Equal("d", elem2.Right.Text);


            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestImportFromStmt4()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("from ..... import ( b, c as d )\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (ImportFromStatement) node.Elements[0];
            Assert.Equal(Token.TokenKind.PyFrom, node2.Operator1.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(31UL, node2.End);

            Assert.True(node2.Left == null);
            
            // Global in import from statement and not in dottedname statement....
            Assert.Equal(3, node2.Dots.Length);
            Assert.Equal(Token.TokenKind.PyElipsis, node2.Dots[0].Kind);
            Assert.Equal(Token.TokenKind.PyDot, node2.Dots[1].Kind);
            Assert.Equal(Token.TokenKind.PyDot, node2.Dots[2].Kind);
            
            var right = (ImportListStatement)node2.Right;
            Assert.Equal(Token.TokenKind.PyLeftParen, node2.Operator3.Kind);
            Assert.Equal(Token.TokenKind.PyRightParen, node2.Operator4.Kind);
            Assert.True(right.Elements.Length == 2);
            Assert.True(right.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PyComma, right.Separators[0].Kind);

            var elem1 = (ImportAsNameStatement)right.Elements[0];
            Assert.Equal("b", elem1.Left.Text);
            Assert.True(elem1.Operator == null);
            Assert.True(elem1.Right == null);
            
            var elem2 = (ImportAsNameStatement)right.Elements[1];
            Assert.Equal("c", elem2.Left.Text);
            Assert.Equal(Token.TokenKind.PyAs, elem2.Operator.Kind);
            Assert.Equal("d", elem2.Right.Text);


            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestGlobalStmt1()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("global a\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (ScopeStatement) node.Elements[0];
            Assert.Equal(ScopeStatement.ScopeKind.Global, node2.Scope);
            Assert.Equal(Token.TokenKind.PyGlobal, node2.Operator.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(8UL, node2.End);

            Assert.True(node2.Names.Length == 1);
            Assert.True(node2.Separators.Length == 0);
            Assert.Equal("a", node2.Names[0].Text);
            
            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestNonLocalStmt1()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("nonlocal a\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (ScopeStatement) node.Elements[0];
            Assert.Equal(ScopeStatement.ScopeKind.Nonlocal, node2.Scope);
            Assert.Equal(Token.TokenKind.PyNonlocal, node2.Operator.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(10UL, node2.End);

            Assert.True(node2.Names.Length == 1);
            Assert.True(node2.Separators.Length == 0);
            Assert.Equal("a", node2.Names[0].Text);
            
            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestGlobalStmt2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("global a, b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (ScopeStatement) node.Elements[0];
            Assert.Equal(ScopeStatement.ScopeKind.Global, node2.Scope);
            Assert.Equal(Token.TokenKind.PyGlobal, node2.Operator.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(11UL, node2.End);

            Assert.True(node2.Names.Length == 2);
            Assert.True(node2.Separators.Length == 1);
            Assert.Equal("a", node2.Names[0].Text);
            Assert.Equal("b", node2.Names[1].Text);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[0].Kind);
            
            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestNonLocalStmt2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("nonlocal a, b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (ScopeStatement) node.Elements[0];
            Assert.Equal(ScopeStatement.ScopeKind.Nonlocal, node2.Scope);
            Assert.Equal(Token.TokenKind.PyNonlocal, node2.Operator.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(13UL, node2.End);

            Assert.True(node2.Names.Length == 2);
            Assert.True(node2.Separators.Length == 1);
            Assert.Equal("a", node2.Names[0].Text);
            Assert.Equal("b", node2.Names[1].Text);
            Assert.Equal(Token.TokenKind.PyComma, node2.Separators[0].Kind);
            
            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestAssertStmt1()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("assert a\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AssertStatement) node.Elements[0];
            Assert.Equal(Token.TokenKind.PyAssert, node2.Operator1.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(8UL, node2.End);

            var node3 = (NameLiteralExpression) node2.Left;
            Assert.Equal("a", node3.Name.Text);
            Assert.True(node2.Operator2 == null);
            Assert.True(node2.Right == null);
            
            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestAssertStmt2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("assert a, b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AssertStatement) node.Elements[0];
            Assert.Equal(Token.TokenKind.PyAssert, node2.Operator1.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(11UL, node2.End);

            var node3 = (NameLiteralExpression) node2.Left;
            Assert.Equal("a", node3.Name.Text);
            Assert.Equal(Token.TokenKind.PyComma, node2.Operator2.Kind);
            Assert.Equal("b", ((NameLiteralExpression)node2.Right).Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementSingle()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a + b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (PlainExpressionStatement) node.Elements[0];
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(5UL, node2.End);

            var node3 = (ArithExpression)node2.Node;
            Assert.Equal(ArithExpression.ArithOperatorKind.Plus, node3.ArithOperator);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementPlusAssign()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a += b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.PlusAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(6UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyPlusAssign, node2.Operator.Kind);
            var right = (NameLiteralExpression) node2.Right;
            Assert.Equal("b", right.Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementPlusAssignYieldExpr()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a += yield b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.PlusAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(12UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyPlusAssign, node2.Operator.Kind);
            var right = (YieldExpression) node2.Right;
            Assert.Equal(Token.TokenKind.PyYield, right.Operator1.Kind);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementMinusAssign()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a -= b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.MinusAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(6UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyMinusAssign, node2.Operator.Kind);
            var right = (NameLiteralExpression) node2.Right;
            Assert.Equal("b", right.Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementMinusAssignYieldExpr()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a -= yield b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.MinusAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(12UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyMinusAssign, node2.Operator.Kind);
            var right = (YieldExpression) node2.Right;
            Assert.Equal(Token.TokenKind.PyYield, right.Operator1.Kind);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementMulAssign()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a *= b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.MulAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(6UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyMulAssign, node2.Operator.Kind);
            var right = (NameLiteralExpression) node2.Right;
            Assert.Equal("b", right.Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementMulAssignYieldExpr()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a *= yield b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.MulAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(12UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyMulAssign, node2.Operator.Kind);
            var right = (YieldExpression) node2.Right;
            Assert.Equal(Token.TokenKind.PyYield, right.Operator1.Kind);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementMatriceAssign()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a @= b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.MatriceAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(6UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyMatriceAssign, node2.Operator.Kind);
            var right = (NameLiteralExpression) node2.Right;
            Assert.Equal("b", right.Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementMatriceAssignYieldExpr()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a @= yield b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.MatriceAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(12UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyMatriceAssign, node2.Operator.Kind);
            var right = (YieldExpression) node2.Right;
            Assert.Equal(Token.TokenKind.PyYield, right.Operator1.Kind);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementPlusDivAssign()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a /= b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.DivAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(6UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyDivAssign, node2.Operator.Kind);
            var right = (NameLiteralExpression) node2.Right;
            Assert.Equal("b", right.Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementDivAssignYieldExpr()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a /= yield b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.DivAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(12UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyDivAssign, node2.Operator.Kind);
            var right = (YieldExpression) node2.Right;
            Assert.Equal(Token.TokenKind.PyYield, right.Operator1.Kind);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementModuloAssign()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a %= b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.ModuloAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(6UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyModuloAssign, node2.Operator.Kind);
            var right = (NameLiteralExpression) node2.Right;
            Assert.Equal("b", right.Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementModuloAssignYieldExpr()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a %= yield b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.ModuloAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(12UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyModuloAssign, node2.Operator.Kind);
            var right = (YieldExpression) node2.Right;
            Assert.Equal(Token.TokenKind.PyYield, right.Operator1.Kind);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementAndAssign()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a &= b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.AndAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(6UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyAndAssign, node2.Operator.Kind);
            var right = (NameLiteralExpression) node2.Right;
            Assert.Equal("b", right.Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementAndAssignYieldExpr()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a &= yield b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.AndAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(12UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyAndAssign, node2.Operator.Kind);
            var right = (YieldExpression) node2.Right;
            Assert.Equal(Token.TokenKind.PyYield, right.Operator1.Kind);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementOrAssign()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a |= b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.OrAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(6UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyOrAssign, node2.Operator.Kind);
            var right = (NameLiteralExpression) node2.Right;
            Assert.Equal("b", right.Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementOrAssignYieldExpr()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a |= yield b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.OrAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(12UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyOrAssign, node2.Operator.Kind);
            var right = (YieldExpression) node2.Right;
            Assert.Equal(Token.TokenKind.PyYield, right.Operator1.Kind);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementXorAssign()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a ^= b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.XorAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(6UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyXorAssign, node2.Operator.Kind);
            var right = (NameLiteralExpression) node2.Right;
            Assert.Equal("b", right.Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementXorAssignYieldExpr()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a ^= yield b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.XorAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(12UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyXorAssign, node2.Operator.Kind);
            var right = (YieldExpression) node2.Right;
            Assert.Equal(Token.TokenKind.PyYield, right.Operator1.Kind);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementShiftLeftAssign()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a <<= b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.ShiftLeftAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(7UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyShiftLeftAssign, node2.Operator.Kind);
            var right = (NameLiteralExpression) node2.Right;
            Assert.Equal("b", right.Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementShiftLeftAssignYieldExpr()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a <<= yield b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.ShiftLeftAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(13UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyShiftLeftAssign, node2.Operator.Kind);
            var right = (YieldExpression) node2.Right;
            Assert.Equal(Token.TokenKind.PyYield, right.Operator1.Kind);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementShiftRightAssign()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a >>= b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.ShiftRightAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(7UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyShiftRightAssign, node2.Operator.Kind);
            var right = (NameLiteralExpression) node2.Right;
            Assert.Equal("b", right.Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementShiftRightAssignYieldExpr()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a >>= yield b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.ShiftRightAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(13UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyShiftRightAssign, node2.Operator.Kind);
            var right = (YieldExpression) node2.Right;
            Assert.Equal(Token.TokenKind.PyYield, right.Operator1.Kind);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementPowerAssign()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a **= b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.PowerAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(7UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyPowerAssign, node2.Operator.Kind);
            var right = (NameLiteralExpression) node2.Right;
            Assert.Equal("b", right.Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementPowerAssignYieldExpr()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a **= yield b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.PowerAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(13UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyPowerAssign, node2.Operator.Kind);
            var right = (YieldExpression) node2.Right;
            Assert.Equal(Token.TokenKind.PyYield, right.Operator1.Kind);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementFloorDivAssign()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a //= b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.FloorDivAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(7UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyFloorDivAssign, node2.Operator.Kind);
            var right = (NameLiteralExpression) node2.Right;
            Assert.Equal("b", right.Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementFloorDivAssignYieldExpr()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a //= yield b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.FloorDivAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(13UL, node2.End);

            var left = (NameLiteralExpression)node2.Left;
            Assert.Equal("a", left.Name.Text);
            Assert.Equal(Token.TokenKind.PyFloorDivAssign, node2.Operator.Kind);
            var right = (YieldExpression) node2.Right;
            Assert.Equal(Token.TokenKind.PyYield, right.Operator1.Kind);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionAssign()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a = b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AssignmentStatement) node.Elements[0];
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(5UL, node2.End);

            Assert.True(node2.Assignment.Count == 1);
            Assert.True(node2.Right.Count == 1);
            Assert.Equal("a", ((NameLiteralExpression)node2.Left).Name.Text);
            Assert.Equal("b", ((NameLiteralExpression)node2.Right[0]).Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementAssignYieldExpr()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a = yield b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AssignmentStatement) node.Elements[0];
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(11UL, node2.End);

            Assert.True(node2.Assignment.Count == 1);
            Assert.True(node2.Right.Count == 1);
            Assert.Equal("a", ((NameLiteralExpression)node2.Left).Name.Text);

            var node3 = (YieldExpression) node2.Right[0];
            Assert.Equal("b", ((NameLiteralExpression)node3.Right).Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionAssignMuliple()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a = b = c\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AssignmentStatement) node.Elements[0];
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(9UL, node2.End);

            Assert.True(node2.Assignment.Count == 2);
            Assert.True(node2.Right.Count == 2);
            Assert.Equal("a", ((NameLiteralExpression)node2.Left).Name.Text);
            Assert.Equal("b", ((NameLiteralExpression)node2.Right[0]).Name.Text);
            Assert.Equal("c", ((NameLiteralExpression)node2.Right[1]).Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementAssignYieldExprMuliple()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a = yield b = yield c\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AssignmentStatement) node.Elements[0];
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(21UL, node2.End);

            Assert.True(node2.Assignment.Count == 2);
            Assert.True(node2.Right.Count == 2);
            Assert.Equal("a", ((NameLiteralExpression)node2.Left).Name.Text);

            var node3 = (YieldExpression) node2.Right[0];
            Assert.Equal("b", ((NameLiteralExpression)node3.Right).Name.Text);
            
            var node4 = (YieldExpression) node2.Right[1];
            Assert.Equal("c", ((NameLiteralExpression)node4.Right).Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementSingleNoOperator()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (PlainExpressionStatement) node.Elements[0];
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(1UL, node2.End);

            var node3 = (NameLiteralExpression) node2.Node;
            Assert.Equal("a", node3.Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestAnnOperatorExpressionStatement()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a : b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AnnAssignStatement) node.Elements[0];
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(5UL, node2.End);

            var node3 = (NameLiteralExpression) node2.Left;
            Assert.Equal("a", node3.Name.Text);
            
            Assert.Equal(Token.TokenKind.PyColon, node2.Colon.Kind);
            
            var node4 = (NameLiteralExpression) node2.Type;
            Assert.Equal("b", node4.Name.Text);

            Assert.True(node2.Assignment == null);
            Assert.True(node2.Right == null);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestAnnAssignExpressionStatement()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a : b = c\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AnnAssignStatement) node.Elements[0];
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(9UL, node2.End);

            var node3 = (NameLiteralExpression) node2.Left;
            Assert.Equal("a", node3.Name.Text);
            
            Assert.Equal(Token.TokenKind.PyColon, node2.Colon.Kind);
            
            var node4 = (NameLiteralExpression) node2.Type;
            Assert.Equal("b", node4.Name.Text);

            Assert.Equal(Token.TokenKind.PyAssign, node2.Assignment.Kind);
            
            var node5 = (NameLiteralExpression) node2.Right;
            Assert.Equal("c", node5.Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionTestStarExprList1()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("*a\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (PlainExpressionStatement) node.Elements[0];
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(2UL, node2.End);

            var node3 = (StarExpression) node2.Node;
            
            var node4 = (NameLiteralExpression) node3.Right;
            Assert.Equal("a", node4.Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionTestStarExprList2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("*a, b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (PlainExpressionStatement) node.Elements[0];
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(5UL, node2.End);

            var node3 = (ListExpression) node2.Node;
            Assert.True(node3.Elements.Length == 2);
            Assert.True(node3.Separators.Length == 1);

            var node4 = (StarExpression) node3.Elements[0];
            Assert.Equal("a", ((NameLiteralExpression)node4.Right).Name.Text);
            
            Assert.Equal("b", ((NameLiteralExpression)node3.Elements[1]).Name.Text);

            Assert.Equal(Token.TokenKind.PyComma, node3.Separators[0].Kind);            
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionTestStarExprList3()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("*a, b, *c;\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (PlainExpressionStatement) node.Elements[0];
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(9UL, node2.End);

            var node3 = (ListExpression) node2.Node;
            Assert.True(node3.Elements.Length == 3);
            Assert.True(node3.Separators.Length == 2);

            var node4 = (StarExpression) node3.Elements[0];
            Assert.Equal("a", ((NameLiteralExpression)node4.Right).Name.Text);
            
            Assert.Equal("b", ((NameLiteralExpression)node3.Elements[1]).Name.Text);
            
            var node5 = (StarExpression) node3.Elements[2];
            Assert.Equal("c", ((NameLiteralExpression)node5.Right).Name.Text);

            Assert.Equal(Token.TokenKind.PyComma, node3.Separators[0].Kind);  
            Assert.Equal(Token.TokenKind.PyComma, node3.Separators[1].Kind);  
            
            Assert.True(node.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestExpressionStatementPlusAssignWithtrailingComma()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a, += b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ListStatement) parser.ParseStmt();
            Assert.True(node.Elements.Length == 1);
            
            var node2 = (AugAssignStatement) node.Elements[0];
            Assert.Equal(AugAssignStatement.OperatorKind.PlusAssign, node2.Kind);
            Assert.Equal(0UL, node2.Start);
            Assert.Equal(7UL, node2.End);

            var left = (ListExpression) node2.Left;
            Assert.True(left.Elements.Length == 1);
            Assert.True(left.Separators.Length == 1);
            var node3 = (NameLiteralExpression) left.Elements[0];
            Assert.Equal("a", node3.Name.Text);
            
            Assert.Equal(Token.TokenKind.PyPlusAssign, node2.Operator.Kind);
            var right = (NameLiteralExpression) node2.Right;
            Assert.Equal("b", right.Name.Text);
            
            Assert.True(node.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.Newline, node.NewLine.Kind);
        }
        
        [Fact]
        public void TestCompoundStatementIfSingle()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("if a: b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (IfStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(9UL, node.End);

            Assert.Equal(Token.TokenKind.PyIf, node.Operator1.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind);

            var node2 = (ListStatement) node.Right;
            Assert.True(node2.Elements.Length == 1);
            Assert.True(node2.Separators.Length == 0);

            var node3 = (PlainExpressionStatement) node2.Elements[0];
            Assert.Equal("b", ((NameLiteralExpression) node3.Node).Name.Text);

            Assert.True(node.ElifElements == null);
            Assert.True(node.ElseElement == null);
        }
        
        [Fact]
        public void TestCompoundStatementIfElseSingle()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("if a: b\r\nelse: c\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (IfStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(18UL, node.End);

            Assert.Equal(Token.TokenKind.PyIf, node.Operator1.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind);

            var node2 = (ListStatement) node.Right;
            Assert.True(node2.Elements.Length == 1);
            Assert.True(node2.Separators.Length == 0);

            var node3 = (PlainExpressionStatement) node2.Elements[0];
            Assert.Equal("b", ((NameLiteralExpression) node3.Node).Name.Text);

            Assert.True(node.ElifElements.Length == 0);

            var node4 = (ElseStatement) node.ElseElement;
            Assert.Equal(Token.TokenKind.PyElse, node4.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyColon, node4.Operator2.Kind);

            var node5 = (ListStatement) node4.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node5.Kind);
            Assert.True(node5.Elements.Length == 1);
            Assert.True(node5.Separators.Length == 0);

            var node6 = (PlainExpressionStatement) node5.Elements[0];
            Assert.Equal("c", ((NameLiteralExpression) node6.Node).Name.Text);
        }
        
        [Fact]
        public void TestCompoundStatementIfElifElseSingle()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("if a: b\r\nelif c: d\r\nelse: d\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (IfStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(29UL, node.End);

            Assert.Equal(Token.TokenKind.PyIf, node.Operator1.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind);

            var node2 = (ListStatement) node.Right;
            Assert.True(node2.Elements.Length == 1);
            Assert.True(node2.Separators.Length == 0);

            var node3 = (PlainExpressionStatement) node2.Elements[0];
            Assert.Equal("b", ((NameLiteralExpression) node3.Node).Name.Text);

            Assert.True(node.ElifElements.Length == 1);

            var node10 = (ElifStatement) node.ElifElements[0];
            Assert.Equal(Token.TokenKind.PyElif, node10.Operator1.Kind);
            Assert.Equal("c", ((NameLiteralExpression) node10.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyColon, node10.Operator2.Kind);
            
            var node11 = (ListStatement) node10.Right;
            Assert.True(node11.Elements.Length == 1);
            Assert.True(node11.Separators.Length == 0);

            var node12 = (PlainExpressionStatement) node11.Elements[0];
            Assert.Equal("d", ((NameLiteralExpression) node12.Node).Name.Text);
            
            var node4 = (ElseStatement) node.ElseElement;
            Assert.Equal(Token.TokenKind.PyElse, node4.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyColon, node4.Operator2.Kind);

            var node5 = (ListStatement) node4.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node5.Kind);
            Assert.True(node5.Elements.Length == 1);
            Assert.True(node5.Separators.Length == 0);

            var node6 = (PlainExpressionStatement) node5.Elements[0];
            Assert.Equal("d", ((NameLiteralExpression) node6.Node).Name.Text);
        }
        
        [Fact]
        public void TestCompoundStatementIfMultipleElifElseSingle()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("if a: b\r\nelif c: d\r\nelif e: f\r\nelse: g\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (IfStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(40UL, node.End);

            Assert.Equal(Token.TokenKind.PyIf, node.Operator1.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind);

            var node2 = (ListStatement) node.Right;
            Assert.True(node2.Elements.Length == 1);
            Assert.True(node2.Separators.Length == 0);

            var node3 = (PlainExpressionStatement) node2.Elements[0];
            Assert.Equal("b", ((NameLiteralExpression) node3.Node).Name.Text);

            Assert.True(node.ElifElements.Length == 2);

            var node10 = (ElifStatement) node.ElifElements[0];
            Assert.Equal(Token.TokenKind.PyElif, node10.Operator1.Kind);
            Assert.Equal("c", ((NameLiteralExpression) node10.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyColon, node10.Operator2.Kind);
            
            var node11 = (ListStatement) node10.Right;
            Assert.True(node11.Elements.Length == 1);
            Assert.True(node11.Separators.Length == 0);

            var node12 = (PlainExpressionStatement) node11.Elements[0];
            Assert.Equal("d", ((NameLiteralExpression) node12.Node).Name.Text);
            
            var node20 = (ElifStatement) node.ElifElements[1];
            Assert.Equal(Token.TokenKind.PyElif, node20.Operator1.Kind);
            Assert.Equal("e", ((NameLiteralExpression) node20.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyColon, node20.Operator2.Kind);
            
            var node21 = (ListStatement) node20.Right;
            Assert.True(node21.Elements.Length == 1);
            Assert.True(node21.Separators.Length == 0);

            var node22 = (PlainExpressionStatement) node21.Elements[0];
            Assert.Equal("f", ((NameLiteralExpression) node22.Node).Name.Text);
            
            var node4 = (ElseStatement) node.ElseElement;
            Assert.Equal(Token.TokenKind.PyElse, node4.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyColon, node4.Operator2.Kind);

            var node5 = (ListStatement) node4.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node5.Kind);
            Assert.True(node5.Elements.Length == 1);
            Assert.True(node5.Separators.Length == 0);

            var node6 = (PlainExpressionStatement) node5.Elements[0];
            Assert.Equal("g", ((NameLiteralExpression) node6.Node).Name.Text);
        }
        
        [Fact]
        public void TestCompoundStatementIfMultipleElifNoElseSingle()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("if a: b\r\nelif c: d\r\nelif e: f\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (IfStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(31UL, node.End);

            Assert.Equal(Token.TokenKind.PyIf, node.Operator1.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind);

            var node2 = (ListStatement) node.Right;
            Assert.True(node2.Elements.Length == 1);
            Assert.True(node2.Separators.Length == 0);

            var node3 = (PlainExpressionStatement) node2.Elements[0];
            Assert.Equal("b", ((NameLiteralExpression) node3.Node).Name.Text);

            Assert.True(node.ElifElements.Length == 2);

            var node10 = (ElifStatement) node.ElifElements[0];
            Assert.Equal(Token.TokenKind.PyElif, node10.Operator1.Kind);
            Assert.Equal("c", ((NameLiteralExpression) node10.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyColon, node10.Operator2.Kind);
            
            var node11 = (ListStatement) node10.Right;
            Assert.True(node11.Elements.Length == 1);
            Assert.True(node11.Separators.Length == 0);

            var node12 = (PlainExpressionStatement) node11.Elements[0];
            Assert.Equal("d", ((NameLiteralExpression) node12.Node).Name.Text);
            
            var node20 = (ElifStatement) node.ElifElements[1];
            Assert.Equal(Token.TokenKind.PyElif, node20.Operator1.Kind);
            Assert.Equal("e", ((NameLiteralExpression) node20.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyColon, node20.Operator2.Kind);
            
            var node21 = (ListStatement) node20.Right;
            Assert.True(node21.Elements.Length == 1);
            Assert.True(node21.Separators.Length == 0);

            var node22 = (PlainExpressionStatement) node21.Elements[0];
            Assert.Equal("f", ((NameLiteralExpression) node22.Node).Name.Text);

            Assert.True(node.ElseElement == null);
        }
        
        [Fact]
        public void TestCompoundStatementWhileNoElseSingle()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("while a: b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (WhileStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(12UL, node.End);

            Assert.Equal(Token.TokenKind.PyWhile, node.Operator1.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind);

            var node2 = (ListStatement) node.Right;
            Assert.True(node2.Elements.Length == 1);
            Assert.True(node2.Separators.Length == 0);

            var node3 = (PlainExpressionStatement) node2.Elements[0];
            Assert.Equal("b", ((NameLiteralExpression) node3.Node).Name.Text);
            
            Assert.True(node.ElseElement == null);
        }
        
        [Fact]
        public void TestCompoundStatementWhileElseSingle()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("while a: b\r\nelse: c\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (WhileStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(21UL, node.End);

            Assert.Equal(Token.TokenKind.PyWhile, node.Operator1.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind);

            var node2 = (ListStatement) node.Right;
            Assert.True(node2.Elements.Length == 1);
            Assert.True(node2.Separators.Length == 0);

            var node3 = (PlainExpressionStatement) node2.Elements[0];
            Assert.Equal("b", ((NameLiteralExpression) node3.Node).Name.Text);
            
            var node4 = (ElseStatement) node.ElseElement;
            Assert.Equal(Token.TokenKind.PyElse, node4.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyColon, node4.Operator2.Kind);

            var node5 = (ListStatement) node4.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node5.Kind);
            Assert.True(node5.Elements.Length == 1);
            Assert.True(node5.Separators.Length == 0);

            var node6 = (PlainExpressionStatement) node5.Elements[0];
            Assert.Equal("c", ((NameLiteralExpression) node6.Node).Name.Text);
        }
        
        [Fact]
        public void TestCompoundStatementForNoElseSingle()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("for a in b: c\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ForStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(15UL, node.End);

            Assert.Equal(Token.TokenKind.PyFor, node.Operator1.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyIn, node.Operator2.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node.Right).Name.Text);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator3.Kind);

            var node2 = (ListStatement) node.Next;
            Assert.True(node2.Elements.Length == 1);
            Assert.True(node2.Separators.Length == 0);

            var node3 = (PlainExpressionStatement) node2.Elements[0];
            Assert.Equal("c", ((NameLiteralExpression) node3.Node).Name.Text);
            
            Assert.True(node.ElseElement == null);
        }
        
        [Fact]
        public void TestCompoundStatementForElseSingle()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("for a in b: c\r\nelse: d\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ForStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(24UL, node.End);

            Assert.Equal(Token.TokenKind.PyFor, node.Operator1.Kind);
            Assert.Equal("a", ((NameLiteralExpression) node.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyIn, node.Operator2.Kind);
            Assert.Equal("b", ((NameLiteralExpression) node.Right).Name.Text);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator3.Kind);

            var node2 = (ListStatement) node.Next;
            Assert.True(node2.Elements.Length == 1);
            Assert.True(node2.Separators.Length == 0);

            var node3 = (PlainExpressionStatement) node2.Elements[0];
            Assert.Equal("c", ((NameLiteralExpression) node3.Node).Name.Text);
            
            var node4 = (ElseStatement) node.ElseElement;
            Assert.Equal(Token.TokenKind.PyElse, node4.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyColon, node4.Operator2.Kind);

            var node5 = (ListStatement) node4.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node5.Kind);
            Assert.True(node5.Elements.Length == 1);
            Assert.True(node5.Separators.Length == 0);

            var node6 = (PlainExpressionStatement) node5.Elements[0];
            Assert.Equal("d", ((NameLiteralExpression) node6.Node).Name.Text);
        }
        
        [Fact]
        public void TestCompoundStatementWithSingle()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("with a: b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (WithStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(11UL, node.End);

            Assert.Equal(Token.TokenKind.PyWith, node.Operator1.Kind);

            Assert.True(node.WithItems.Length == 1);
            
            var node4 = (WithItemStatement)node.WithItems[0];
            Assert.Equal("a", ((NameLiteralExpression)node4.Left).Name.Text);
            Assert.True(node4.Operator1 == null);
            Assert.True(node4.Right == null);
            
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind);
            
            var node2 = (ListStatement) node.Suite;
            Assert.True(node2.Elements.Length == 1);
            Assert.True(node2.Separators.Length == 0);

            var node3 = (PlainExpressionStatement) node2.Elements[0];
            Assert.Equal("b", ((NameLiteralExpression) node3.Node).Name.Text);
        }
        
        [Fact]
        public void TestCompoundStatementWithMultiple()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("with a, b as c: d\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (WithStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(19UL, node.End);

            Assert.Equal(Token.TokenKind.PyWith, node.Operator1.Kind);

            Assert.True(node.WithItems.Length == 2);
            
            var node4 = (WithItemStatement)node.WithItems[0];
            Assert.Equal("a", ((NameLiteralExpression)node4.Left).Name.Text);
            Assert.True(node4.Operator1 == null);
            Assert.True(node4.Right == null);
            
            var node5 = (WithItemStatement)node.WithItems[1];
            Assert.Equal("b", ((NameLiteralExpression)node5.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyAs, node5.Operator1.Kind);
            Assert.Equal("c", ((NameLiteralExpression)node5.Right).Name.Text);
            
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind);
            
            var node2 = (ListStatement) node.Suite;
            Assert.True(node2.Elements.Length == 1);
            Assert.True(node2.Separators.Length == 0);

            var node3 = (PlainExpressionStatement) node2.Elements[0];
            Assert.Equal("d", ((NameLiteralExpression) node3.Node).Name.Text);
        }
        
        [Fact]
        public void TestCompoundStatementTryFinallySingle()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("try: a\r\nfinally: b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (TryStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(20UL, node.End);

            Assert.Equal(Token.TokenKind.PyTry, node.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind);

            var node5 = (ListStatement) node.Left;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node5.Kind);

            var node6 = (PlainExpressionStatement) node5.Elements[0];
            Assert.Equal("a", ((NameLiteralExpression) node6.Node).Name.Text);
            
            var node2 = (FinallyStatement) node.FinallyElements;
            Assert.Equal(Token.TokenKind.PyFinally, node2.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyColon, node2.Operator2.Kind);

            var node3 = (ListStatement) node2.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node3.Kind);

            var node4 = (PlainExpressionStatement) node3.Elements[0];
            Assert.Equal("b", ((NameLiteralExpression) node4.Node).Name.Text);
        }
        
        [Fact]
        public void TestCompoundStatementTrySingleExceptFinallySingle()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("try: a\r\nexcept: c\r\nfinally: b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (TryStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(31UL, node.End);

            Assert.Equal(Token.TokenKind.PyTry, node.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind);

            var node5 = (ListStatement) node.Left;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node5.Kind);

            var node6 = (PlainExpressionStatement) node5.Elements[0];
            Assert.Equal("a", ((NameLiteralExpression) node6.Node).Name.Text);

            Assert.True(node.ExceptElements.Length == 1);
            Assert.True(node.ElseElement == null);
            var node7 = (ExceptStatement)node.ExceptElements[0];
            Assert.Equal(Token.TokenKind.PyExcept, node7.Operator1.Kind);
            Assert.True(node7.Left == null);
            Assert.Equal(Token.TokenKind.PyColon, node7.Operator4.Kind);

            var node8 = (ListStatement) node7.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node8.Kind);

            var node9 = (PlainExpressionStatement) node8.Elements[0];
            Assert.Equal("c", ((NameLiteralExpression) node9.Node).Name.Text);
            
            var node2 = (FinallyStatement) node.FinallyElements;
            Assert.Equal(Token.TokenKind.PyFinally, node2.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyColon, node2.Operator2.Kind);

            var node3 = (ListStatement) node2.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node3.Kind);

            var node4 = (PlainExpressionStatement) node3.Elements[0];
            Assert.Equal("b", ((NameLiteralExpression) node4.Node).Name.Text);
        }
        
        [Fact]
        public void TestCompoundStatementTryMultipleExceptFinallySingle()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("try: a\r\nexcept: c\r\nexcept g as h: t\r\nfinally: b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (TryStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(49UL, node.End);

            Assert.Equal(Token.TokenKind.PyTry, node.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind);

            var node5 = (ListStatement) node.Left;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node5.Kind);

            var node6 = (PlainExpressionStatement) node5.Elements[0];
            Assert.Equal("a", ((NameLiteralExpression) node6.Node).Name.Text);

            Assert.True(node.ExceptElements.Length == 2);
            Assert.True(node.ElseElement == null);
            
            var node7 = (ExceptStatement)node.ExceptElements[0];
            Assert.Equal(Token.TokenKind.PyExcept, node7.Operator1.Kind);
            
            Assert.True(node7.Left == null);
            Assert.Equal(Token.TokenKind.PyColon, node7.Operator4.Kind);

            var node8 = (ListStatement) node7.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node8.Kind);

            var node9 = (PlainExpressionStatement) node8.Elements[0];
            Assert.Equal("c", ((NameLiteralExpression) node9.Node).Name.Text);
            
            var node17 = (ExceptStatement)node.ExceptElements[1];
            Assert.Equal(Token.TokenKind.PyExcept, node17.Operator1.Kind);
            Assert.Equal("g", ((NameLiteralExpression) node17.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyAs, node17.Operator2.Kind);
            Assert.Equal(Token.TokenKind.Name, node17.Operator3.Kind);
            Assert.Equal("h", node17.Operator3.Text);
            
            Assert.Equal(Token.TokenKind.PyColon, node17.Operator4.Kind);

            var node18 = (ListStatement) node17.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node18.Kind);

            var node19 = (PlainExpressionStatement) node18.Elements[0];
            Assert.Equal("t", ((NameLiteralExpression) node19.Node).Name.Text);
            
            var node2 = (FinallyStatement) node.FinallyElements;
            Assert.Equal(Token.TokenKind.PyFinally, node2.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyColon, node2.Operator2.Kind);

            var node3 = (ListStatement) node2.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node3.Kind);

            var node4 = (PlainExpressionStatement) node3.Elements[0];
            Assert.Equal("b", ((NameLiteralExpression) node4.Node).Name.Text);
        }
        
        [Fact]
        public void TestCompoundStatementTryTrippleExceptFinallySingle()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("try: a\r\nexcept: c\r\nexcept g as h: t\r\nexcept r: v\r\nfinally: b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (TryStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(62UL, node.End);

            Assert.Equal(Token.TokenKind.PyTry, node.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind);

            var node5 = (ListStatement) node.Left;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node5.Kind);

            var node6 = (PlainExpressionStatement) node5.Elements[0];
            Assert.Equal("a", ((NameLiteralExpression) node6.Node).Name.Text);

            Assert.True(node.ExceptElements.Length == 3);
            Assert.True(node.ElseElement == null);
            
            var node7 = (ExceptStatement)node.ExceptElements[0];
            Assert.Equal(Token.TokenKind.PyExcept, node7.Operator1.Kind);
            
            Assert.True(node7.Left == null);
            Assert.Equal(Token.TokenKind.PyColon, node7.Operator4.Kind);

            var node8 = (ListStatement) node7.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node8.Kind);

            var node9 = (PlainExpressionStatement) node8.Elements[0];
            Assert.Equal("c", ((NameLiteralExpression) node9.Node).Name.Text);
            
            var node17 = (ExceptStatement)node.ExceptElements[1];
            Assert.Equal(Token.TokenKind.PyExcept, node17.Operator1.Kind);
            Assert.Equal("g", ((NameLiteralExpression) node17.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyAs, node17.Operator2.Kind);
            Assert.Equal(Token.TokenKind.Name, node17.Operator3.Kind);
            Assert.Equal("h", node17.Operator3.Text);
            
            Assert.Equal(Token.TokenKind.PyColon, node17.Operator4.Kind);

            var node18 = (ListStatement) node17.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node18.Kind);

            var node19 = (PlainExpressionStatement) node18.Elements[0];
            Assert.Equal("t", ((NameLiteralExpression) node19.Node).Name.Text);
            
            var node27 = (ExceptStatement)node.ExceptElements[2];
            Assert.Equal(Token.TokenKind.PyExcept, node27.Operator1.Kind);
            Assert.Equal("r", ((NameLiteralExpression) node27.Left).Name.Text);
            Assert.True(node27.Operator2 == null);
            Assert.True(node27.Operator3 == null);
            
            Assert.Equal(Token.TokenKind.PyColon, node27.Operator4.Kind);

            var node28 = (ListStatement) node27.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node28.Kind);

            var node29 = (PlainExpressionStatement) node28.Elements[0];
            Assert.Equal("v", ((NameLiteralExpression) node29.Node).Name.Text);
            
            var node2 = (FinallyStatement) node.FinallyElements;
            Assert.Equal(Token.TokenKind.PyFinally, node2.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyColon, node2.Operator2.Kind);

            var node3 = (ListStatement) node2.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node3.Kind);

            var node4 = (PlainExpressionStatement) node3.Elements[0];
            Assert.Equal("b", ((NameLiteralExpression) node4.Node).Name.Text);
        }
        
        [Fact]
        public void TestCompoundStatementTryTrippleExceptAndElseFinallySingle()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("try: a\r\nexcept: c\r\nexcept g as h: t\r\nexcept r: v\r\nelse: s\r\nfinally: b\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (TryStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(71UL, node.End);

            Assert.Equal(Token.TokenKind.PyTry, node.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind);

            var node5 = (ListStatement) node.Left;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node5.Kind);

            var node6 = (PlainExpressionStatement) node5.Elements[0];
            Assert.Equal("a", ((NameLiteralExpression) node6.Node).Name.Text);

            Assert.True(node.ExceptElements.Length == 3);
            
            var node7 = (ExceptStatement)node.ExceptElements[0];
            Assert.Equal(Token.TokenKind.PyExcept, node7.Operator1.Kind);
            
            Assert.True(node7.Left == null);
            Assert.Equal(Token.TokenKind.PyColon, node7.Operator4.Kind);

            var node8 = (ListStatement) node7.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node8.Kind);

            var node9 = (PlainExpressionStatement) node8.Elements[0];
            Assert.Equal("c", ((NameLiteralExpression) node9.Node).Name.Text);
            
            var node17 = (ExceptStatement)node.ExceptElements[1];
            Assert.Equal(Token.TokenKind.PyExcept, node17.Operator1.Kind);
            Assert.Equal("g", ((NameLiteralExpression) node17.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyAs, node17.Operator2.Kind);
            Assert.Equal(Token.TokenKind.Name, node17.Operator3.Kind);
            Assert.Equal("h", node17.Operator3.Text);
            
            Assert.Equal(Token.TokenKind.PyColon, node17.Operator4.Kind);

            var node18 = (ListStatement) node17.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node18.Kind);

            var node19 = (PlainExpressionStatement) node18.Elements[0];
            Assert.Equal("t", ((NameLiteralExpression) node19.Node).Name.Text);
            
            var node27 = (ExceptStatement)node.ExceptElements[2];
            Assert.Equal(Token.TokenKind.PyExcept, node27.Operator1.Kind);
            Assert.Equal("r", ((NameLiteralExpression) node27.Left).Name.Text);
            Assert.True(node27.Operator2 == null);
            Assert.True(node27.Operator3 == null);
            
            Assert.Equal(Token.TokenKind.PyColon, node27.Operator4.Kind);

            var node28 = (ListStatement) node27.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node28.Kind);

            var node29 = (PlainExpressionStatement) node28.Elements[0];
            Assert.Equal("v", ((NameLiteralExpression) node29.Node).Name.Text);
            
            var node2 = (FinallyStatement) node.FinallyElements;
            Assert.Equal(Token.TokenKind.PyFinally, node2.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyColon, node2.Operator2.Kind);

            var node3 = (ListStatement) node2.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node3.Kind);

            var node4 = (PlainExpressionStatement) node3.Elements[0];
            Assert.Equal("b", ((NameLiteralExpression) node4.Node).Name.Text);
            
            var node40 = (ElseStatement) node.ElseElement;
            Assert.Equal(Token.TokenKind.PyElse, node40.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyColon, node40.Operator2.Kind);

            var node50 = (ListStatement) node40.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node50.Kind);
            Assert.True(node50.Elements.Length == 1);
            Assert.True(node50.Separators.Length == 0);

            var node60 = (PlainExpressionStatement) node50.Elements[0];
            Assert.Equal("s", ((NameLiteralExpression) node60.Node).Name.Text);
        }
        
        [Fact]
        public void TestCompoundStatementTryTrippleExceptAndElseSingle()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("try: a\r\nexcept: c\r\nexcept g as h: t\r\nexcept r: v\r\nelse: s\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (TryStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(59UL, node.End);

            Assert.Equal(Token.TokenKind.PyTry, node.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator2.Kind);

            var node5 = (ListStatement) node.Left;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node5.Kind);

            var node6 = (PlainExpressionStatement) node5.Elements[0];
            Assert.Equal("a", ((NameLiteralExpression) node6.Node).Name.Text);

            Assert.True(node.ExceptElements.Length == 3);
            
            var node7 = (ExceptStatement)node.ExceptElements[0];
            Assert.Equal(Token.TokenKind.PyExcept, node7.Operator1.Kind);
            
            Assert.True(node7.Left == null);
            Assert.Equal(Token.TokenKind.PyColon, node7.Operator4.Kind);

            var node8 = (ListStatement) node7.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node8.Kind);

            var node9 = (PlainExpressionStatement) node8.Elements[0];
            Assert.Equal("c", ((NameLiteralExpression) node9.Node).Name.Text);
            
            var node17 = (ExceptStatement)node.ExceptElements[1];
            Assert.Equal(Token.TokenKind.PyExcept, node17.Operator1.Kind);
            Assert.Equal("g", ((NameLiteralExpression) node17.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyAs, node17.Operator2.Kind);
            Assert.Equal(Token.TokenKind.Name, node17.Operator3.Kind);
            Assert.Equal("h", node17.Operator3.Text);
            
            Assert.Equal(Token.TokenKind.PyColon, node17.Operator4.Kind);

            var node18 = (ListStatement) node17.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node18.Kind);

            var node19 = (PlainExpressionStatement) node18.Elements[0];
            Assert.Equal("t", ((NameLiteralExpression) node19.Node).Name.Text);
            
            var node27 = (ExceptStatement)node.ExceptElements[2];
            Assert.Equal(Token.TokenKind.PyExcept, node27.Operator1.Kind);
            Assert.Equal("r", ((NameLiteralExpression) node27.Left).Name.Text);
            Assert.True(node27.Operator2 == null);
            Assert.True(node27.Operator3 == null);
            
            Assert.Equal(Token.TokenKind.PyColon, node27.Operator4.Kind);

            var node28 = (ListStatement) node27.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node28.Kind);

            var node29 = (PlainExpressionStatement) node28.Elements[0];
            Assert.Equal("v", ((NameLiteralExpression) node29.Node).Name.Text);

            Assert.True(node.FinallyElements == null);
            
            var node40 = (ElseStatement) node.ElseElement;
            Assert.Equal(Token.TokenKind.PyElse, node40.Operator1.Kind);
            Assert.Equal(Token.TokenKind.PyColon, node40.Operator2.Kind);

            var node50 = (ListStatement) node40.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node50.Kind);
            Assert.True(node50.Elements.Length == 1);
            Assert.True(node50.Separators.Length == 0);

            var node60 = (PlainExpressionStatement) node50.Elements[0];
            Assert.Equal("s", ((NameLiteralExpression) node60.Node).Name.Text);
        }
        
        [Fact]
        public void TestCompoundStatementClassDeclaration()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("class Test: pass\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ClassDeclarationStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(18UL, node.End);

            Assert.Equal(Token.TokenKind.PyClass, node.Operator1.Kind);
            Assert.Equal("Test", node.ClassName.Text);
            Assert.True(node.Operator2 == null);
            Assert.True(node.Left == null);
            Assert.True(node.Operator3 == null);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator4.Kind);

            var node2 = (ListStatement) node.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList,node2.Kind);
            Assert.True(node2.Elements.Length == 1);
            var node3 = (PassStatement)node2.Elements[0];
            Assert.Equal(Token.TokenKind.PyPass, node3.Operator.Kind);
        }
        
        [Fact]
        public void TestCompoundStatementClassDeclarationEmptyInheritance()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("class Test(): pass\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ClassDeclarationStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(20UL, node.End);

            Assert.Equal(Token.TokenKind.PyClass, node.Operator1.Kind);
            Assert.Equal("Test", node.ClassName.Text);
            Assert.Equal(Token.TokenKind.PyLeftParen, node.Operator2.Kind);
            Assert.True(node.Left == null);
            Assert.Equal(Token.TokenKind.PyRightParen, node.Operator3.Kind);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator4.Kind);

            var node2 = (ListStatement) node.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList,node2.Kind);
            Assert.True(node2.Elements.Length == 1);
            var node3 = (PassStatement)node2.Elements[0];
            Assert.Equal(Token.TokenKind.PyPass, node3.Operator.Kind);
        }
        
        [Fact]
        public void TestCompoundStatementClassDeclarationWithInheritance()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("class Test(a): pass\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (ClassDeclarationStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(21UL, node.End);

            Assert.Equal(Token.TokenKind.PyClass, node.Operator1.Kind);
            Assert.Equal("Test", node.ClassName.Text);
            Assert.Equal(Token.TokenKind.PyLeftParen, node.Operator2.Kind);
            //Assert.True(node.Left == null);
            var node10 = (ArgumentExpression) node.Left;
            Assert.Equal("a", node10.Left.Text);
            
            Assert.Equal(Token.TokenKind.PyRightParen, node.Operator3.Kind);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator4.Kind);

            var node2 = (ListStatement) node.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList,node2.Kind);
            Assert.True(node2.Elements.Length == 1);
            var node3 = (PassStatement)node2.Elements[0];
            Assert.Equal(Token.TokenKind.PyPass, node3.Operator.Kind);
        }
        
        [Fact]
        public void TestdecoratorWithClassDeclaration1()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("@dummy\r\nclass Test: pass\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var nodeFront = (DecoratedStatement) parser.ParseStmt();
            Assert.Equal(0UL, nodeFront.Start);
            Assert.Equal(26UL, nodeFront.End);
            
            // Decorator handling below:
            var node4 = (ListStatement)nodeFront.Decorators;
            Assert.Equal(ListStatement.ListKind.DecoratorList, node4.Kind);
            Assert.True(node4.Elements.Length == 1);

            // Decorator one
            var node5 = (DecoratorStatement) node4.Elements[0];
            Assert.Equal(Token.TokenKind.PyMatrice, node5.Operator1.Kind);
            var node7 = (DottedNameStatement) node5.Left;
            Assert.True(node7.Names.Length == 1);
            Assert.Equal("dummy", node7.Names[0].Text);
            
            Assert.True(node5.Operator2 == null);
            Assert.True(node5.Operator3 == null);
            Assert.Equal(Token.TokenKind.Newline, node5.Operator4.Kind);
            
            // Class part below:
            var node = (ClassDeclarationStatement)nodeFront.Right;
            Assert.Equal(Token.TokenKind.PyClass, node.Operator1.Kind);
            Assert.Equal("Test", node.ClassName.Text);
            Assert.True(node.Operator2 == null);
            Assert.True(node.Left == null);
            Assert.True(node.Operator3 == null);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator4.Kind);
            
            var node2 = (ListStatement) node.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList,node2.Kind);
            Assert.True(node2.Elements.Length == 1);
            var node3 = (PassStatement)node2.Elements[0];
            Assert.Equal(Token.TokenKind.PyPass, node3.Operator.Kind);
        }
        
        [Fact]
        public void TestdecoratorWithClassDeclaration2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("@dummy\r\n@test()\r\nclass Test: pass\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var nodeFront = (DecoratedStatement) parser.ParseStmt();
            Assert.Equal(0UL, nodeFront.Start);
            Assert.Equal(35UL, nodeFront.End);
            
            // Decorator handling below:
            var node4 = (ListStatement)nodeFront.Decorators;
            Assert.Equal(ListStatement.ListKind.DecoratorList, node4.Kind);
            Assert.True(node4.Elements.Length == 2);

            // Decorator one
            var node5 = (DecoratorStatement) node4.Elements[0];
            Assert.Equal(Token.TokenKind.PyMatrice, node5.Operator1.Kind);
            var node7 = (DottedNameStatement) node5.Left;
            Assert.True(node7.Names.Length == 1);
            Assert.Equal("dummy", node7.Names[0].Text);
            
            Assert.True(node5.Operator2 == null);
            Assert.True(node5.Operator3 == null);
            Assert.Equal(Token.TokenKind.Newline, node5.Operator4.Kind);
            
            // Decorator two
            var node15 = (DecoratorStatement) node4.Elements[1];
            Assert.Equal(Token.TokenKind.PyMatrice, node15.Operator1.Kind);
            var node17 = (DottedNameStatement) node15.Left;
            Assert.True(node7.Names.Length == 1);
            Assert.Equal("test", node17.Names[0].Text);
            
            Assert.Equal(Token.TokenKind.PyLeftParen, node15.Operator2.Kind);
            Assert.Equal(Token.TokenKind.PyRightParen, node15.Operator3.Kind);
            Assert.Equal(Token.TokenKind.Newline, node15.Operator4.Kind);
            
            // Class part below:
            var node = (ClassDeclarationStatement)nodeFront.Right;
            Assert.Equal(Token.TokenKind.PyClass, node.Operator1.Kind);
            Assert.Equal("Test", node.ClassName.Text);
            Assert.True(node.Operator2 == null);
            Assert.True(node.Left == null);
            Assert.True(node.Operator3 == null);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator4.Kind);
            
            var node2 = (ListStatement) node.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList,node2.Kind);
            Assert.True(node2.Elements.Length == 1);
            var node3 = (PassStatement)node2.Elements[0];
            Assert.Equal(Token.TokenKind.PyPass, node3.Operator.Kind);
        }
        
        [Fact]
        public void TestdecoratorWithClassDeclaration3()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("@dummy\r\n@test(a)\r\nclass Test: pass\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var nodeFront = (DecoratedStatement) parser.ParseStmt();
            Assert.Equal(0UL, nodeFront.Start);
            Assert.Equal(36UL, nodeFront.End);
            
            // Decorator handling below:
            var node4 = (ListStatement)nodeFront.Decorators;
            Assert.Equal(ListStatement.ListKind.DecoratorList, node4.Kind);
            Assert.True(node4.Elements.Length == 2);

            // Decorator one
            var node5 = (DecoratorStatement) node4.Elements[0];
            Assert.Equal(Token.TokenKind.PyMatrice, node5.Operator1.Kind);
            var node7 = (DottedNameStatement) node5.Left;
            Assert.True(node7.Names.Length == 1);
            Assert.Equal("dummy", node7.Names[0].Text);
            
            Assert.True(node5.Operator2 == null);
            Assert.True(node5.Operator3 == null);
            Assert.Equal(Token.TokenKind.Newline, node5.Operator4.Kind);
            
            // Decorator two
            var node15 = (DecoratorStatement) node4.Elements[1];
            Assert.Equal(Token.TokenKind.PyMatrice, node15.Operator1.Kind);
            var node17 = (DottedNameStatement) node15.Left;
            Assert.True(node17.Names.Length == 1);
            Assert.Equal("test", node17.Names[0].Text);
            
            Assert.Equal(Token.TokenKind.PyLeftParen, node15.Operator2.Kind);
            Assert.Equal(Token.TokenKind.PyRightParen, node15.Operator3.Kind);
            Assert.Equal(Token.TokenKind.Newline, node15.Operator4.Kind);

            var node18 = (ArgumentExpression) node15.Right;
            Assert.Equal("a", node18.Left.Text);
            
            // Class part below:
            var node = (ClassDeclarationStatement)nodeFront.Right;
            Assert.Equal(Token.TokenKind.PyClass, node.Operator1.Kind);
            Assert.Equal("Test", node.ClassName.Text);
            Assert.True(node.Operator2 == null);
            Assert.True(node.Left == null);
            Assert.True(node.Operator3 == null);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator4.Kind);
            
            var node2 = (ListStatement) node.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList,node2.Kind);
            Assert.True(node2.Elements.Length == 1);
            var node3 = (PassStatement)node2.Elements[0];
            Assert.Equal(Token.TokenKind.PyPass, node3.Operator.Kind);
        }
        
        [Fact]
        public void TestdecoratorWithClassDeclaration4()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("@dummy\r\n@test(a, b)\r\nclass Test: pass\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var nodeFront = (DecoratedStatement) parser.ParseStmt();
            Assert.Equal(0UL, nodeFront.Start);
            Assert.Equal(39UL, nodeFront.End);
            
            // Decorator handling below:
            var node4 = (ListStatement)nodeFront.Decorators;
            Assert.Equal(ListStatement.ListKind.DecoratorList, node4.Kind);
            Assert.True(node4.Elements.Length == 2);

            // Decorator one
            var node5 = (DecoratorStatement) node4.Elements[0];
            Assert.Equal(Token.TokenKind.PyMatrice, node5.Operator1.Kind);
            var node7 = (DottedNameStatement) node5.Left;
            Assert.True(node7.Names.Length == 1);
            Assert.Equal("dummy", node7.Names[0].Text);
            
            Assert.True(node5.Operator2 == null);
            Assert.True(node5.Operator3 == null);
            Assert.Equal(Token.TokenKind.Newline, node5.Operator4.Kind);
            
            // Decorator two
            var node15 = (DecoratorStatement) node4.Elements[1];
            Assert.Equal(Token.TokenKind.PyMatrice, node15.Operator1.Kind);
            var node17 = (DottedNameStatement) node15.Left;
            Assert.True(node17.Names.Length == 1);
            Assert.Equal("test", node17.Names[0].Text);
            
            Assert.Equal(Token.TokenKind.PyLeftParen, node15.Operator2.Kind);
            Assert.Equal(Token.TokenKind.PyRightParen, node15.Operator3.Kind);
            Assert.Equal(Token.TokenKind.Newline, node15.Operator4.Kind);

            var node18 = (ListExpression) node15.Right;
            Assert.Equal(ListExpression.ListType.ArgumentList, node18.ContainerType);
            Assert.True(node18.Elements.Length == 2);
            Assert.Equal("a", ((ArgumentExpression)node18.Elements[0]).Left.Text);
            Assert.Equal(Token.TokenKind.PyComma, node18.Separators[0].Kind);
            Assert.Equal("b", ((ArgumentExpression)node18.Elements[1]).Left.Text);
            
            // Class part below:
            var node = (ClassDeclarationStatement)nodeFront.Right;
            Assert.Equal(Token.TokenKind.PyClass, node.Operator1.Kind);
            Assert.Equal("Test", node.ClassName.Text);
            Assert.True(node.Operator2 == null);
            Assert.True(node.Left == null);
            Assert.True(node.Operator3 == null);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator4.Kind);
            
            var node2 = (ListStatement) node.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList,node2.Kind);
            Assert.True(node2.Elements.Length == 1);
            var node3 = (PassStatement)node2.Elements[0];
            Assert.Equal(Token.TokenKind.PyPass, node3.Operator.Kind);
        }
        
        [Fact]
        public void TestdecoratorWithAsyncFuncDef()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("@dummy\r\nasync def Test(): pass\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var nodeFront = (DecoratedStatement) parser.ParseStmt();
            Assert.Equal(0UL, nodeFront.Start);
            Assert.Equal(32UL, nodeFront.End);
            
            // Decorator handling below:
            var node4 = (ListStatement)nodeFront.Decorators;
            Assert.Equal(ListStatement.ListKind.DecoratorList, node4.Kind);
            Assert.True(node4.Elements.Length == 1);

            // Decorator one
            var node5 = (DecoratorStatement) node4.Elements[0];
            Assert.Equal(Token.TokenKind.PyMatrice, node5.Operator1.Kind);
            var node7 = (DottedNameStatement) node5.Left;
            Assert.True(node7.Names.Length == 1);
            Assert.Equal("dummy", node7.Names[0].Text);
            
            Assert.True(node5.Operator2 == null);
            Assert.True(node5.Operator3 == null);
            Assert.Equal(Token.TokenKind.Newline, node5.Operator4.Kind);
            
            // async def part below:
            var node = (AsyncStatement)nodeFront.Right;
            Assert.Equal(Token.TokenKind.PyAsync, node.Operator.Kind);

            var node10 = (FuncDeclarationStatement)node.Right;
            Assert.Equal(Token.TokenKind.PyDef, node10.Operator1.Kind);
        }
        
        [Fact]
        public void TestdecoratorWithFuncDef()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("@dummy\r\ndef Test(): pass\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var nodeFront = (DecoratedStatement) parser.ParseStmt();
            Assert.Equal(0UL, nodeFront.Start);
            Assert.Equal(26UL, nodeFront.End);
            
            // Decorator handling below:
            var node4 = (ListStatement)nodeFront.Decorators;
            Assert.Equal(ListStatement.ListKind.DecoratorList, node4.Kind);
            Assert.True(node4.Elements.Length == 1);

            // Decorator one
            var node5 = (DecoratorStatement) node4.Elements[0];
            Assert.Equal(Token.TokenKind.PyMatrice, node5.Operator1.Kind);
            var node7 = (DottedNameStatement) node5.Left;
            Assert.True(node7.Names.Length == 1);
            Assert.Equal("dummy", node7.Names[0].Text);
            
            Assert.True(node5.Operator2 == null);
            Assert.True(node5.Operator3 == null);
            Assert.Equal(Token.TokenKind.Newline, node5.Operator4.Kind);
            
            // async def part below:
            var node = (FuncDeclarationStatement)nodeFront.Right;
            Assert.Equal(Token.TokenKind.PyDef, node.Operator1.Kind);
        }
        
        [Fact]
        public void TestAsyncFuncDef()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("async def Test(): pass\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (AsyncStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(24UL, node.End);

            Assert.Equal(Token.TokenKind.PyAsync, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.PyDef, ((FuncDeclarationStatement)node.Right).Operator1.Kind);
        }
        
        [Fact]
        public void TestAsyncWithStmt()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("async with a: pass\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (AsyncStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(20UL, node.End);

            Assert.Equal(Token.TokenKind.PyAsync, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.PyWith, ((WithStatement)node.Right).Operator1.Kind);
        }
        
        [Fact]
        public void TestAsyncForStmt()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("async for a in b: pass\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (AsyncStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(24UL, node.End);

            Assert.Equal(Token.TokenKind.PyAsync, node.Operator.Kind);
            Assert.Equal(Token.TokenKind.PyFor, ((ForStatement)node.Right).Operator1.Kind);
        }
        
        [Fact]
        public void TestDefDeclarationStmt1()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("def a(): pass\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (FuncDeclarationStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(15UL, node.End);

            var node3 = (ParameterStatement) node.Left;
            Assert.Equal(Token.TokenKind.PyLeftParen, node3.Operator1.Kind);
            Assert.True(node3.Right == null);
            Assert.Equal(Token.TokenKind.PyRightParen, node3.Operator2.Kind);

            Assert.Equal(Token.TokenKind.PyDef, node.Operator1.Kind);
            Assert.Equal("a", node.Name.Text);
            Assert.True(node.Operator2 == null);
            Assert.True(node.Right == null);
            Assert.Equal(Token.TokenKind.PyColon, node.Operator3.Kind);

            var node2 = (ListStatement) node.Next;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node2.Kind);
        }
        
        [Fact]
        public void TestDefDeclarationStmt2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("def a() -> b: pass\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (FuncDeclarationStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(20UL, node.End);

            var node3 = (ParameterStatement) node.Left;
            Assert.Equal(Token.TokenKind.PyLeftParen, node3.Operator1.Kind);
            Assert.True(node3.Right == null);
            Assert.Equal(Token.TokenKind.PyRightParen, node3.Operator2.Kind);

            Assert.Equal(Token.TokenKind.PyDef, node.Operator1.Kind);
            Assert.Equal("a", node.Name.Text);
            Assert.Equal(Token.TokenKind.PyArrow, node.Operator2.Kind);
            
            var node4 = (NameLiteralExpression)node.Right;
            Assert.Equal("b", node4.Name.Text);
            
            Assert.Equal(Token.TokenKind.PyColon, node.Operator3.Kind);

            var node2 = (ListStatement) node.Next;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node2.Kind);
        }
        
        [Fact]
        public void TestDefDeclarationStmt3()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("def a(c, d) -> b: pass\r\n\0".ToCharArray(), false, 8);
            parser.Tokenizer.Advance();
            
            var node = (FuncDeclarationStatement) parser.ParseStmt();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(24UL, node.End);

            var node3 = (ParameterStatement) node.Left;
            Assert.Equal(Token.TokenKind.PyLeftParen, node3.Operator1.Kind);
            
            var node10 = (TypedArgsStatement)node3.Right;
            Assert.True(node10.Elements.Length == 2);
            Assert.True(node10.Separators.Length == 1);
            
            Assert.Equal(Token.TokenKind.PyRightParen, node3.Operator2.Kind);

            Assert.Equal(Token.TokenKind.PyDef, node.Operator1.Kind);
            Assert.Equal("a", node.Name.Text);
            Assert.Equal(Token.TokenKind.PyArrow, node.Operator2.Kind);
            
            var node4 = (NameLiteralExpression)node.Right;
            Assert.Equal("b", node4.Name.Text);
            
            Assert.Equal(Token.TokenKind.PyColon, node.Operator3.Kind);

            var node2 = (ListStatement) node.Next;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node2.Kind);
        }
        
        [Fact]
        public void TestTopLevelStartEvalInput()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("a, b, c\r\n\0".ToCharArray(), false, 8);
            
            var node = (EvalInputStatement) parser.ParseEvalInput();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(9UL, node.End);
            Assert.True(node.Newlines.Length == 1);
            Assert.Equal(Token.TokenKind.Newline, node.Newlines[0].Kind);
            Assert.Equal(Token.TokenKind.EOF, node.EOF.Kind);

            var node2 = (ListExpression) node.Right;
            Assert.Equal(ListExpression.ListType.TestList, node2.ContainerType);
            Assert.True(node2.Elements.Length == 3);
            Assert.True(node2.Separators.Length == 2);
        }
        
        [Fact]
        public void TestTopLevelStartFileInput()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("pass; pass\r\n\0".ToCharArray(), false, 8);
            
            var node = (FileInputStatement) parser.ParseFileInput();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(12UL, node.End);

            Assert.True(node.Newlines.Length == 0);
            
            Assert.True(node.Statements.Length == 1);
            var node2 = (ListStatement)node.Statements[0];
            Assert.Equal(ListStatement.ListKind.SimpleStatementList ,node2.Kind);
            Assert.True(node2.Elements.Length == 2);
            Assert.True(node2.Separators.Length == 1);
            
            Assert.Equal(Token.TokenKind.EOF, node.EOF.Kind);
        }
        
        [Fact]
        public void TestTopLevelStartFileInputMultiple()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("pass\r\npass\r\n\r\n\0".ToCharArray(), false, 8);
            
            var node = (FileInputStatement) parser.ParseFileInput();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(14UL, node.End);

            Assert.True(node.Newlines.Length == 1);
            
            Assert.True(node.Statements.Length == 2);
            var node2 = (ListStatement)node.Statements[0];
            Assert.Equal(ListStatement.ListKind.SimpleStatementList ,node2.Kind);
            Assert.True(node2.Elements.Length == 1);
            Assert.True(node2.Separators.Length == 0);
            
            var node3 = (ListStatement)node.Statements[1];
            Assert.Equal(ListStatement.ListKind.SimpleStatementList ,node3.Kind);
            Assert.True(node3.Elements.Length == 1);
            Assert.True(node3.Separators.Length == 0);
            
            Assert.Equal(Token.TokenKind.EOF, node.EOF.Kind);
        }
        
        [Fact]
        public void TestTopLevelStartSingleInputJustNewline()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("\r\n\0".ToCharArray(), false, 8);
            
            var node = (SingleInputStatement) parser.ParseSingleInput();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(2UL, node.End);

            Assert.Equal(Token.TokenKind.Newline, node.Newline.Kind);
            Assert.True(node.Right == null);
        }
        
        [Fact]
        public void TestTopLevelStartSingleInputSimpleStmt()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("pass\r\n\0".ToCharArray(), false, 8);
            
            var node = (SingleInputStatement) parser.ParseSingleInput();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(6UL, node.End);
            
            Assert.True(node.Newline == null);

            var node2 = (ListStatement) node.Right;
            Assert.Equal(ListStatement.ListKind.SimpleStatementList, node2.Kind);
            Assert.True(node2.Elements.Length == 1);
            Assert.Equal(Token.TokenKind.PyPass, ((PassStatement)node2.Elements[0]).Operator.Kind );
            Assert.True(node2.Separators.Length == 0);
        }
        
        [Fact]
        public void TestTopLevelStartSingleInputCompoundStmt()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("if a: pass\r\n\r\n\0".ToCharArray(), false, 8);
            
            var node = (SingleInputStatement) parser.ParseSingleInput();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(14UL, node.End);
            
            Assert.Equal(Token.TokenKind.Newline, node.Newline.Kind);

            var node2 = (IfStatement) node.Right;
            Assert.Equal(Token.TokenKind.PyIf, node2.Operator1.Kind);
        }
        
        [Fact]
        public void TestTopLevelStartFuncTypeInputStmt()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("() -> a + b\0".ToCharArray(), false, 8);
            
            var node = (FuncInputStatement) parser.ParseFuncInput();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(11UL, node.End);

            Assert.True(node.Newlines.Length == 0);
            var node2 = (FuncTypeExpression)node.Right;
            Assert.Equal(Token.TokenKind.PyLeftParen, node2.Operator1.Kind);
            Assert.True(node2.Left == null);
            Assert.Equal(Token.TokenKind.PyRightParen, node2.Operator2.Kind);
            Assert.Equal(Token.TokenKind.PyArrow, node2.Operator3.Kind);

            var node3 = (ArithExpression) node2.Right;
            Assert.Equal(ArithExpression.ArithOperatorKind.Plus, node3.ArithOperator);
            
            Assert.Equal(Token.TokenKind.EOF, node.EOF.Kind);
        }
        
        [Fact]
        public void TestTopLevelStartFuncTypeInputStmtWithTrailingNewlines()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("() -> a + b\r\n\r\n\0".ToCharArray(), false, 8);
            
            var node = (FuncInputStatement) parser.ParseFuncInput();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(15UL, node.End);

            Assert.True(node.Newlines.Length == 2);
            Assert.Equal(Token.TokenKind.Newline, node.Newlines[0].Kind);
            Assert.Equal(Token.TokenKind.Newline, node.Newlines[1].Kind);
            
            var node2 = (FuncTypeExpression)node.Right;
            Assert.Equal(Token.TokenKind.PyLeftParen, node2.Operator1.Kind);
            Assert.True(node2.Left == null);
            Assert.Equal(Token.TokenKind.PyRightParen, node2.Operator2.Kind);
            Assert.Equal(Token.TokenKind.PyArrow, node2.Operator3.Kind);

            var node3 = (ArithExpression) node2.Right;
            Assert.Equal(ArithExpression.ArithOperatorKind.Plus, node3.ArithOperator);
            
            Assert.Equal(Token.TokenKind.EOF, node.EOF.Kind);
        }
        
        [Fact]
        public void TestTopLevelStartFuncTypeInputStmtWithArguments1()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("(**a) -> a + b\r\n\r\n\0".ToCharArray(), false, 8);
            
            var node = (FuncInputStatement) parser.ParseFuncInput();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(18UL, node.End);

            Assert.True(node.Newlines.Length == 2);
            Assert.Equal(Token.TokenKind.Newline, node.Newlines[0].Kind);
            Assert.Equal(Token.TokenKind.Newline, node.Newlines[1].Kind);
            
            var node2 = (FuncTypeExpression)node.Right;
            Assert.Equal(Token.TokenKind.PyLeftParen, node2.Operator1.Kind);
            
            var node4 = (TypeListExpression) node2.Left;
            Assert.True(node4.Elements.Length == 0);
            Assert.True(node4.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.PyPower, node4.Power.Kind);
            Assert.Equal("a", ((NameLiteralExpression)node4.Right).Name.Text);
            
            Assert.Equal(Token.TokenKind.PyRightParen, node2.Operator2.Kind);
            Assert.Equal(Token.TokenKind.PyArrow, node2.Operator3.Kind);

            var node3 = (ArithExpression) node2.Right;
            Assert.Equal(ArithExpression.ArithOperatorKind.Plus, node3.ArithOperator);
            
            Assert.Equal(Token.TokenKind.EOF, node.EOF.Kind);
        }
        
        [Fact]
        public void TestTopLevelStartFuncTypeInputStmtWithArguments2()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("(*a) -> a + b\r\n\r\n\0".ToCharArray(), false, 8);
            
            var node = (FuncInputStatement) parser.ParseFuncInput();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(17UL, node.End);

            Assert.True(node.Newlines.Length == 2);
            Assert.Equal(Token.TokenKind.Newline, node.Newlines[0].Kind);
            Assert.Equal(Token.TokenKind.Newline, node.Newlines[1].Kind);
            
            var node2 = (FuncTypeExpression)node.Right;
            Assert.Equal(Token.TokenKind.PyLeftParen, node2.Operator1.Kind);
            
            var node4 = (TypeListExpression) node2.Left;
            Assert.True(node4.Elements.Length == 0);
            Assert.True(node4.Separators.Length == 0);
            Assert.Equal(Token.TokenKind.PyMul, node4.Mul.Kind);
            Assert.Equal("a", ((NameLiteralExpression)node4.Left).Name.Text);
            
            Assert.Equal(Token.TokenKind.PyRightParen, node2.Operator2.Kind);
            Assert.Equal(Token.TokenKind.PyArrow, node2.Operator3.Kind);

            var node3 = (ArithExpression) node2.Right;
            Assert.Equal(ArithExpression.ArithOperatorKind.Plus, node3.ArithOperator);
            
            Assert.Equal(Token.TokenKind.EOF, node.EOF.Kind);
        }
        
        [Fact]
        public void TestTopLevelStartFuncTypeInputStmtWithArguments3()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("(*a, b) -> a + b\r\n\r\n\0".ToCharArray(), false, 8);
            
            var node = (FuncInputStatement) parser.ParseFuncInput();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(20UL, node.End);

            Assert.True(node.Newlines.Length == 2);
            Assert.Equal(Token.TokenKind.Newline, node.Newlines[0].Kind);
            Assert.Equal(Token.TokenKind.Newline, node.Newlines[1].Kind);
            
            var node2 = (FuncTypeExpression)node.Right;
            Assert.Equal(Token.TokenKind.PyLeftParen, node2.Operator1.Kind);
            
            var node4 = (TypeListExpression) node2.Left;
            Assert.True(node4.Elements.Length == 1);
            Assert.True(node4.Separators.Length == 1);
            Assert.Equal(Token.TokenKind.PyMul, node4.Mul.Kind);
            Assert.Equal("a", ((NameLiteralExpression)node4.Left).Name.Text);
            
            
            Assert.Equal("b", ((NameLiteralExpression)node4.Elements[0]).Name.Text);
            Assert.Equal(Token.TokenKind.PyComma, node4.Separators[0].Kind);
            
            Assert.Equal(Token.TokenKind.PyRightParen, node2.Operator2.Kind);
            Assert.Equal(Token.TokenKind.PyArrow, node2.Operator3.Kind);

            var node3 = (ArithExpression) node2.Right;
            Assert.Equal(ArithExpression.ArithOperatorKind.Plus, node3.ArithOperator);
            
            Assert.Equal(Token.TokenKind.EOF, node.EOF.Kind);
        }
        
        [Fact]
        public void TestTopLevelStartFuncTypeInputStmtWithArguments4()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("(*a, b,) -> a + b\r\n\r\n\0".ToCharArray(), false, 8);
            
            var node = (FuncInputStatement) parser.ParseFuncInput();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(21UL, node.End);

            Assert.True(node.Newlines.Length == 2);
            Assert.Equal(Token.TokenKind.Newline, node.Newlines[0].Kind);
            Assert.Equal(Token.TokenKind.Newline, node.Newlines[1].Kind);
            
            var node2 = (FuncTypeExpression)node.Right;
            Assert.Equal(Token.TokenKind.PyLeftParen, node2.Operator1.Kind);
            
            var node4 = (TypeListExpression) node2.Left;
            Assert.True(node4.Elements.Length == 1);
            Assert.True(node4.Separators.Length == 2);
            Assert.Equal(Token.TokenKind.PyMul, node4.Mul.Kind);
            Assert.Equal("a", ((NameLiteralExpression)node4.Left).Name.Text);
            
            
            Assert.Equal("b", ((NameLiteralExpression)node4.Elements[0]).Name.Text);
            Assert.Equal(Token.TokenKind.PyComma, node4.Separators[0].Kind);
            
            Assert.Equal(Token.TokenKind.PyRightParen, node2.Operator2.Kind);
            Assert.Equal(Token.TokenKind.PyArrow, node2.Operator3.Kind);

            var node3 = (ArithExpression) node2.Right;
            Assert.Equal(ArithExpression.ArithOperatorKind.Plus, node3.ArithOperator);
            
            Assert.Equal(Token.TokenKind.EOF, node.EOF.Kind);
        }
        
        [Fact]
        public void TestTopLevelStartFuncTypeInputStmtWithArguments5()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("(a, b, *c, d, e, **f) -> a + b\r\n\r\n\0".ToCharArray(), false, 8);
            
            var node = (FuncInputStatement) parser.ParseFuncInput();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(34UL, node.End);

            Assert.True(node.Newlines.Length == 2);
            Assert.Equal(Token.TokenKind.Newline, node.Newlines[0].Kind);
            Assert.Equal(Token.TokenKind.Newline, node.Newlines[1].Kind);
            
            var node2 = (FuncTypeExpression)node.Right;
            Assert.Equal(Token.TokenKind.PyLeftParen, node2.Operator1.Kind);
            
            var node4 = (TypeListExpression) node2.Left;
            Assert.True(node4.Elements.Length == 4);
            Assert.True(node4.Separators.Length == 5);
            Assert.Equal(Token.TokenKind.PyMul, node4.Mul.Kind);
            Assert.Equal("c", ((NameLiteralExpression)node4.Left).Name.Text);
            Assert.Equal(Token.TokenKind.PyPower, node4.Power.Kind);
            Assert.Equal("f", ((NameLiteralExpression)node4.Right).Name.Text);

            
            Assert.Equal("a", ((NameLiteralExpression)node4.Elements[0]).Name.Text);
            Assert.Equal(Token.TokenKind.PyComma, node4.Separators[0].Kind);
            
            Assert.Equal(Token.TokenKind.PyRightParen, node2.Operator2.Kind);
            Assert.Equal(Token.TokenKind.PyArrow, node2.Operator3.Kind);

            var node3 = (ArithExpression) node2.Right;
            Assert.Equal(ArithExpression.ArithOperatorKind.Plus, node3.ArithOperator);
            
            Assert.Equal(Token.TokenKind.EOF, node.EOF.Kind);
        }
        
        [Fact]
        public void TestTopLevelStartFuncTypeInputStmtWithArguments6()
        {
            var parser = new PythonParser();
            Assert.True(parser != null);
            parser.Tokenizer = new PythonTokenizer("(a, b, *c, d, e,) -> a + b\r\n\r\n\0".ToCharArray(), false, 8);
            
            var node = (FuncInputStatement) parser.ParseFuncInput();
            Assert.Equal(0UL, node.Start);
            Assert.Equal(30UL, node.End);

            Assert.True(node.Newlines.Length == 2);
            Assert.Equal(Token.TokenKind.Newline, node.Newlines[0].Kind);
            Assert.Equal(Token.TokenKind.Newline, node.Newlines[1].Kind);
            
            var node2 = (FuncTypeExpression)node.Right;
            Assert.Equal(Token.TokenKind.PyLeftParen, node2.Operator1.Kind);
            
            var node4 = (TypeListExpression) node2.Left;
            Assert.True(node4.Elements.Length == 4);
            Assert.True(node4.Separators.Length == 5);
            Assert.Equal(Token.TokenKind.PyMul, node4.Mul.Kind);
            Assert.Equal("c", ((NameLiteralExpression)node4.Left).Name.Text);
            Assert.True(node4.Power == null);
            
            Assert.Equal("a", ((NameLiteralExpression)node4.Elements[0]).Name.Text);
            Assert.Equal(Token.TokenKind.PyComma, node4.Separators[0].Kind);
            
            Assert.Equal(Token.TokenKind.PyRightParen, node2.Operator2.Kind);
            Assert.Equal(Token.TokenKind.PyArrow, node2.Operator3.Kind);

            var node3 = (ArithExpression) node2.Right;
            Assert.Equal(ArithExpression.ArithOperatorKind.Plus, node3.ArithOperator);
            
            Assert.Equal(Token.TokenKind.EOF, node.EOF.Kind);
        }
    }
}