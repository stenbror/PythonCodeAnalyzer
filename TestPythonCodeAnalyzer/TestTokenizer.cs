using System;
using Xunit;
using TestPythonCodeAnalyzer;
using PythonCodeAnalyzer.Parser;
using PythonCodeAnalyzer.Parser.Ast.Expression;

namespace TestPythonCodeAnalyzer
{
    public class TestTokenizer
    {
        [Fact]
        public void TestReservedKeywords_LiteralName()
        {
            var lex = new PythonTokenizer("_False ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.Name, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(6u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_LiteralNameWithDigit()
        {
            var lex = new PythonTokenizer("N3ame_ ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.Name, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(6u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_False()
        {
            var lex = new PythonTokenizer("False ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyFalse, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(5u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_None()
        {
            var lex = new PythonTokenizer("None ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyNone, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(4u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_True()
        {
            var lex = new PythonTokenizer("True ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyTrue, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(4u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_and()
        {
            var lex = new PythonTokenizer("and ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyAnd, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(3u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_as()
        {
            var lex = new PythonTokenizer("as ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyAs, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(2u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_assert()
        {
            var lex = new PythonTokenizer("assert ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyAssert, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(6u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_async()
        {
            var lex = new PythonTokenizer("async ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyAsync, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(5u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_await()
        {
            var lex = new PythonTokenizer("await ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyAwait, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(5u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_break()
        {
            var lex = new PythonTokenizer("break ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyBreak, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(5u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_class()
        {
            var lex = new PythonTokenizer("class ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyClass, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(5u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_continue()
        {
            var lex = new PythonTokenizer("continue ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyContinue, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(8u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_def()
        {
            var lex = new PythonTokenizer("def ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyDef, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(3u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_del()
        {
            var lex = new PythonTokenizer("del ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyDel, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(3u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_elif()
        {
            var lex = new PythonTokenizer("elif ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyElif, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(4u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_else()
        {
            var lex = new PythonTokenizer("else ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyElse, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(4u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_except()
        {
            var lex = new PythonTokenizer("except ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyExcept, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(6u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_finally()
        {
            var lex = new PythonTokenizer("finally ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyFinally, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(7u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_for()
        {
            var lex = new PythonTokenizer("for ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyFor, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(3u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_from()
        {
            var lex = new PythonTokenizer("from ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyFrom, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(4u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_global()
        {
            var lex = new PythonTokenizer("global ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyGlobal, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(6u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_if()
        {
            var lex = new PythonTokenizer("if ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyIf, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(2u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_import()
        {
            var lex = new PythonTokenizer("import ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyImport, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(6u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_in()
        {
            var lex = new PythonTokenizer("in ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyIn, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(2u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_is()
        {
            var lex = new PythonTokenizer("is ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyIs, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(2u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_lambda()
        {
            var lex = new PythonTokenizer("lambda ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyLambda, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(6u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_nonlocal()
        {
            var lex = new PythonTokenizer("nonlocal ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyNonlocal, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(8u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_not()
        {
            var lex = new PythonTokenizer("not ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyNot, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(3u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_or()
        {
            var lex = new PythonTokenizer("or ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyOr, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(2u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_pass()
        {
            var lex = new PythonTokenizer("pass ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyPass, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(4u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_raise()
        {
            var lex = new PythonTokenizer("raise ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyRaise, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(5u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_return()
        {
            var lex = new PythonTokenizer("return ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyReturn, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(6u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_try()
        {
            var lex = new PythonTokenizer("try ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyTry, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(3u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_while()
        {
            var lex = new PythonTokenizer("while ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyWhile, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(5u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_with()
        {
            var lex = new PythonTokenizer("with ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyWith, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(4u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestReservedKeywords_yield()
        {
            var lex = new PythonTokenizer("yield ".ToCharArray(), false, 8);
            lex.Advance();
            Assert.Equal(Token.TokenKind.PyYield, lex.CurSymbol.Kind);
            Assert.Equal(0u, lex.CurSymbol.Start);
            Assert.Equal(5u, lex.CurSymbol.End);
        }
        
        [Fact]
        public void TestLevelParenthezis()
        {
            var lex = new PythonTokenizer("()".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyLeftParen, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(1u, tok1.End);
            var tok2 = lex.GetSymbol();
            Assert.Equal(1u, tok2.Start);
            Assert.Equal(2u, tok2.End);
            Assert.Equal(Token.TokenKind.PyRightParen, tok2.Kind);
        }
        
        [Fact]
        public void TestLevelBrackets()
        {
            var lex = new PythonTokenizer("[]".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyLeftBracket, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(1u, tok1.End);
            var tok2 = lex.GetSymbol();
            Assert.Equal(1u, tok2.Start);
            Assert.Equal(2u, tok2.End);
            Assert.Equal(Token.TokenKind.PyRightBracket, tok2.Kind);
        }
        
        [Fact]
        public void TestLevelCurlys()
        {
            var lex = new PythonTokenizer("{}".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyLeftCurly, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(1u, tok1.End);
            var tok2 = lex.GetSymbol();
            Assert.Equal(1u, tok2.Start);
            Assert.Equal(2u, tok2.End);
            Assert.Equal(Token.TokenKind.PyRightCurly, tok2.Kind);
        }
        
        [Fact]
        public void TestLevelParenthisMissingStart()
        {
            var lex = new PythonTokenizer(")".ToCharArray(), false, 8);
            try
            {
                var tok1 = lex.GetSymbol();
                Assert.True(false);
            }
            catch (Exception e)
            {
                Assert.True(true);
            }
        }
        
        [Fact]
        public void TestLevelBracketMissingStart()
        {
            var lex = new PythonTokenizer("]".ToCharArray(), false, 8);
            try
            {
                var tok1 = lex.GetSymbol();
                Assert.True(false);
            }
            catch (Exception e)
            {
                Assert.True(true);
            }
        }
        
        [Fact]
        public void TestLevelCurlyMissingStart()
        {
            var lex = new PythonTokenizer("}".ToCharArray(), false, 8);
            try
            {
                var tok1 = lex.GetSymbol();
                Assert.True(false);
            }
            catch (Exception e)
            {
                Assert.True(true);
            }
        }
        
        [Fact]
        public void TestOperatorOrDelimiterPlus()
        {
            var lex = new PythonTokenizer("+ ".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyPlus, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(1u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterPlusAssign()
        {
            var lex = new PythonTokenizer("+=".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyPlusAssign, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterMinus()
        {
            var lex = new PythonTokenizer("- ".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyMinus, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(1u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterMinusAssign()
        {
            var lex = new PythonTokenizer("-=".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyMinusAssign, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterArrow()
        {
            var lex = new PythonTokenizer("->".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyArrow, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterMul()
        {
            var lex = new PythonTokenizer("* ".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyMul, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(1u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterMulAssign()
        {
            var lex = new PythonTokenizer("*=".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyMulAssign, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterPower()
        {
            var lex = new PythonTokenizer("** ".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyPower, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterPowerAssign()
        {
            var lex = new PythonTokenizer("**=".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyPowerAssign, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(3u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterDiv()
        {
            var lex = new PythonTokenizer("/ ".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyDiv, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(1u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterDivAssign()
        {
            var lex = new PythonTokenizer("/=".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyDivAssign, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterFloorDiv()
        {
            var lex = new PythonTokenizer("// ".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyFloorDiv, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterFloorDivAssign()
        {
            var lex = new PythonTokenizer("//=".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyFloorDivAssign, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(3u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterModulo()
        {
            var lex = new PythonTokenizer("% ".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyModulo, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(1u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterModuloAssign()
        {
            var lex = new PythonTokenizer("%=".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyModuloAssign, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterMatrice()
        {
            var lex = new PythonTokenizer("@ ".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyMatrice, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(1u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterMatriceAssign()
        {
            var lex = new PythonTokenizer("@=".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyMatriceAssign, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterAnd()
        {
            var lex = new PythonTokenizer("& ".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyAnd, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(1u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterAndAssign()
        {
            var lex = new PythonTokenizer("&=".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyAndAssign, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterOr()
        {
            var lex = new PythonTokenizer("| ".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyOr, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(1u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterOrAssign()
        {
            var lex = new PythonTokenizer("|=".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyOrAssign, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterXor()
        {
            var lex = new PythonTokenizer("^ ".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyXor, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(1u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterXorAssign()
        {
            var lex = new PythonTokenizer("^=".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyXorAssign, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterInvert()
        {
            var lex = new PythonTokenizer("~".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyInvert, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(1u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterLess()
        {
            var lex = new PythonTokenizer("< ".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyLess, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(1u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterLessEqual()
        {
            var lex = new PythonTokenizer("<=".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyLessEqual, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterNotEqual()
        {
            var lex = new PythonTokenizer("<>".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyNotEqual, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterShiftLeft()
        {
            var lex = new PythonTokenizer("<< ".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyShiftLeft, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterShiftLeftAssign()
        {
            var lex = new PythonTokenizer("<<=".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyShiftLeftAssign, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(3u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterShiftRight()
        {
            var lex = new PythonTokenizer(">> ".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyShiftRight, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterShiftRightAssign()
        {
            var lex = new PythonTokenizer(">>=".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyShiftRightAssign, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(3u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterGreater()
        {
            var lex = new PythonTokenizer("> ".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyGreater, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(1u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterGreaterEqual()
        {
            var lex = new PythonTokenizer(">=".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyGreaterEqual, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterEqual()
        {
            var lex = new PythonTokenizer("==".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyEqual, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterAssign()
        {
            var lex = new PythonTokenizer("= ".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyAssign, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(1u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterNotEqual2()
        {
            var lex = new PythonTokenizer("!=".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyNotEqual, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterColon()
        {
            var lex = new PythonTokenizer(": ".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyColon, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(1u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterColonAssign()
        {
            var lex = new PythonTokenizer(":=".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyColonAssign, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterComma()
        {
            var lex = new PythonTokenizer(",".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyComma, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(1u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterDot()
        {
            var lex = new PythonTokenizer(". ".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyDot, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(1u, tok1.End);
        }
        
        [Fact]
        public void TestOperatorOrDelimiterElipsis()
        {
            var lex = new PythonTokenizer("...".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.PyElipsis, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(3u, tok1.End);
        }
        
        [Fact]
        public void TestNumberZero()
        {
            var lex = new PythonTokenizer("0 ".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.Number, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(1u, tok1.End);
        }
        
        [Fact]
        public void TestNumberZeroImaginary_j()
        {
            var lex = new PythonTokenizer("0j".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.Number, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        [Fact]
        public void TestNumberZeroImaginary_J()
        {
            var lex = new PythonTokenizer("0J".ToCharArray(), false, 8);
            var tok1 = lex.GetSymbol();
            Assert.Equal(Token.TokenKind.Number, tok1.Kind);
            Assert.Equal(0u, tok1.Start);
            Assert.Equal(2u, tok1.End);
        }
        
        
        
        
        
        
        
    }
}