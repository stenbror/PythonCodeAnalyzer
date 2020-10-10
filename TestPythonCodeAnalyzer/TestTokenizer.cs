﻿using System;
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
            var lex = new PythonTokenizer();
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
            var lex = new PythonTokenizer();
            Assert.Equal(Token.TokenKind.Name, lex.IsReservedKeywordOrLiteralName("_yield_"));
        }
        
        
    }
}