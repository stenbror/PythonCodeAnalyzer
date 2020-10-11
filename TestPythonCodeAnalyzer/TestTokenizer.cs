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
        public void TestReservedKeywords()
        {
            var lex = new PythonTokenizer("".ToCharArray(), false, 8);
            Assert.Equal(Token.TokenKind.PyFalse, lex.IsReservedKeywordOrLiteralName("False"));
            Assert.Equal(Token.TokenKind.PyNone, lex.IsReservedKeywordOrLiteralName("None"));
            Assert.Equal(Token.TokenKind.PyTrue, lex.IsReservedKeywordOrLiteralName("True"));
            Assert.Equal(Token.TokenKind.PyAnd, lex.IsReservedKeywordOrLiteralName("and"));
            Assert.Equal(Token.TokenKind.PyAs, lex.IsReservedKeywordOrLiteralName("as"));
            Assert.Equal(Token.TokenKind.PyAssert, lex.IsReservedKeywordOrLiteralName("assert"));
            Assert.Equal(Token.TokenKind.PyAsync, lex.IsReservedKeywordOrLiteralName("async"));
            Assert.Equal(Token.TokenKind.PyAwait, lex.IsReservedKeywordOrLiteralName("await"));
            Assert.Equal(Token.TokenKind.PyBreak, lex.IsReservedKeywordOrLiteralName("break"));
            Assert.Equal(Token.TokenKind.PyClass, lex.IsReservedKeywordOrLiteralName("class"));
            Assert.Equal(Token.TokenKind.PyContinue, lex.IsReservedKeywordOrLiteralName("continue"));
            Assert.Equal(Token.TokenKind.PyDef, lex.IsReservedKeywordOrLiteralName("def"));
            Assert.Equal(Token.TokenKind.PyDel, lex.IsReservedKeywordOrLiteralName("del"));
            Assert.Equal(Token.TokenKind.PyElif, lex.IsReservedKeywordOrLiteralName("elif"));
            Assert.Equal(Token.TokenKind.PyElse, lex.IsReservedKeywordOrLiteralName("else"));
            Assert.Equal(Token.TokenKind.PyExcept, lex.IsReservedKeywordOrLiteralName("except"));
            Assert.Equal(Token.TokenKind.PyFinally, lex.IsReservedKeywordOrLiteralName("finally"));
            Assert.Equal(Token.TokenKind.PyFor, lex.IsReservedKeywordOrLiteralName("for"));
            Assert.Equal(Token.TokenKind.PyFrom, lex.IsReservedKeywordOrLiteralName("from"));
            Assert.Equal(Token.TokenKind.PyGlobal, lex.IsReservedKeywordOrLiteralName("global"));
            Assert.Equal(Token.TokenKind.PyIf, lex.IsReservedKeywordOrLiteralName("if"));
            Assert.Equal(Token.TokenKind.PyImport, lex.IsReservedKeywordOrLiteralName("import"));
            Assert.Equal(Token.TokenKind.PyIn, lex.IsReservedKeywordOrLiteralName("in"));
            Assert.Equal(Token.TokenKind.PyIs, lex.IsReservedKeywordOrLiteralName("is"));
            Assert.Equal(Token.TokenKind.PyLambda, lex.IsReservedKeywordOrLiteralName("lambda"));
            Assert.Equal(Token.TokenKind.PyNonlocal, lex.IsReservedKeywordOrLiteralName("nonlocal"));
            Assert.Equal(Token.TokenKind.PyNot, lex.IsReservedKeywordOrLiteralName("not"));
            Assert.Equal(Token.TokenKind.PyOr, lex.IsReservedKeywordOrLiteralName("or"));
            Assert.Equal(Token.TokenKind.PyPass, lex.IsReservedKeywordOrLiteralName("pass"));
            Assert.Equal(Token.TokenKind.PyRaise, lex.IsReservedKeywordOrLiteralName("raise"));
            Assert.Equal(Token.TokenKind.PyReturn, lex.IsReservedKeywordOrLiteralName("return"));
            Assert.Equal(Token.TokenKind.PyTry, lex.IsReservedKeywordOrLiteralName("try"));
            Assert.Equal(Token.TokenKind.PyWhile, lex.IsReservedKeywordOrLiteralName("while"));
            Assert.Equal(Token.TokenKind.PyWith, lex.IsReservedKeywordOrLiteralName("with"));
            Assert.Equal(Token.TokenKind.PyYield, lex.IsReservedKeywordOrLiteralName("yield"));
        }

        [Fact]
        public void TestReservedKeywordsFoundNameLiteral()
        {
            var lex = new PythonTokenizer("".ToCharArray(), false, 8);
            Assert.Equal(Token.TokenKind.Name, lex.IsReservedKeywordOrLiteralName("_yield_"));
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
        
        
        
        
        
    }
}