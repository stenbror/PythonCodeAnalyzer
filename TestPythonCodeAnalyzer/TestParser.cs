using System;
using Xunit;
using TestPythonCodeAnalyzer;
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
        
        // [Fact]
        // public void TestAtomExprAwaitAndIndex()
        // {
        //     var parser = new PythonParser();
        //     Assert.True(parser != null);
        //     parser.Tokenizer = new PythonTokenizer("await __init__[1];".ToCharArray(), false, 8);
        //     parser.Tokenizer.Advance();
        //     
        //     var node = (AtomExpression)parser.ParseAtomExpression();
        //     Assert.True(node.IsAwait);
        //     Assert.Equal(Token.TokenKind.PyAwait, node.Operator.Kind);
        //     Assert.Equal(Token.TokenKind.Name, ((NameLiteralExpression)node.Right).Name.Kind);
        //     Assert.Equal(6U, ((NameLiteralExpression)node.Right).Name.Start);
        //     Assert.Equal(14U, ((NameLiteralExpression)node.Right).Name.End);
        //     Assert.True(node.TrailerCollection != null);
        //     Assert.Equal(0UL, node.Start );
        //     Assert.Equal(17UL, node.End );
        //     var trailerNode = node.TrailerCollection;
        //     Assert.True(trailerNode.Length == 1);
        //     Assert.Equal(Token.TokenKind.PyLeftBracket, ((IndexExpression) trailerNode[0]).Operator1.Kind);
        //     Assert.True(((IndexExpression) trailerNode[0]).Right != null);
        //     Assert.Equal(Token.TokenKind.PyRightBracket, ((IndexExpression) trailerNode[0]).Operator2.Kind);
        // }
        
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
    }
}