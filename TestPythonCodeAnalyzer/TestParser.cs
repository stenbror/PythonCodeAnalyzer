using System;
using Xunit;
using TestPythonCodeAnalyzer;
using PythonCodeAnalyzer.Parser;
using PythonCodeAnalyzer.Parser.Ast.Expression;

namespace TestPythonCodeAnalyzer
{
    internal class Tokenizer : IPythonTokenizer
    {
        public Token CurSymbol { get; set; }
        public uint Position { get; set; }
        public Token[] Symbols { get; set; }

        private uint index = 0;
        private bool isFirst = true;
        
        public void Advance()
        {
            if (isFirst)
            {
                CurSymbol = Symbols[0];
                isFirst = false;
            }
            else if (index < Symbols.Length)
            {
                Position = CurSymbol.End;
                CurSymbol = Symbols[++index];
                
            }
            //throw new NotImplementedException();
            
        }
    }
    
    public class TestParser
    {
        internal PythonParser Setup(Token[] symbols)
        {
            var parser = new PythonParser();
            parser.Tokenizer = new Tokenizer();
            parser.Tokenizer.Symbols = symbols;
            parser.Tokenizer.Advance();
            return parser;
        }
        
        [Fact]
        public void TestNameAtom()
        {
            var parser = Setup( new Token[] { new Token(0, 5, Token.TokenKind.Name ), new Token(5, 5, Token.TokenKind.EOF) } );
            
            Assert.True(parser != null);
            NameLiteralExpression node = (NameLiteralExpression)parser.ParseAtom();
            Assert.Equal(Token.TokenKind.Name, node.Name.Kind );
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.Equal(0UL, node.Name.Start );
            Assert.Equal(5UL, node.Name.End );
        }
        
        [Fact]
        public void TestNumberAtom()
        {
            var parser = Setup( new Token[] { new Token(0, 5, Token.TokenKind.Number ), new Token(5, 5, Token.TokenKind.EOF) } );
            
            Assert.True(parser != null);
            NumberLiteralExpression node = (NumberLiteralExpression)parser.ParseAtom();
            Assert.Equal(Token.TokenKind.Number, node.Number.Kind );
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.Equal(0UL, node.Number.Start );
            Assert.Equal(5UL, node.Number.End );
        }
        
        [Fact]
        public void TestElipsisAtom()
        {
            var parser = Setup( new Token[] { new Token(0, 5, Token.TokenKind.PyElipsis ), new Token(5, 5, Token.TokenKind.EOF) } );
            
            Assert.True(parser != null);
            ElipsisLiteralExpression node = (ElipsisLiteralExpression)parser.ParseAtom();
            Assert.Equal(Token.TokenKind.PyElipsis, node.Elipsis.Kind );
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.Equal(0UL, node.Elipsis.Start );
            Assert.Equal(5UL, node.Elipsis.End );
        }
        
        [Fact]
        public void TestNoneAtom()
        {
            var parser = Setup( new Token[] { new Token(0, 5, Token.TokenKind.PyNone ), new Token(5, 5, Token.TokenKind.EOF) } );
            
            Assert.True(parser != null);
            NoneExpression node = (NoneExpression)parser.ParseAtom();
            Assert.Equal(Token.TokenKind.PyNone, node.Operator.Kind );
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.Equal(0UL, node.Operator.Start );
            Assert.Equal(5UL, node.Operator.End );
        }
        
        [Fact]
        public void TestTrueAtom()
        {
            var parser = Setup( new Token[] { new Token(0, 5, Token.TokenKind.PyTrue ), new Token(5, 5, Token.TokenKind.EOF) } );
            
            Assert.True(parser != null);
            TrueLiteralExpression node = (TrueLiteralExpression)parser.ParseAtom();
            Assert.Equal(Token.TokenKind.PyTrue, node.Operator.Kind );
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.Equal(0UL, node.Operator.Start );
            Assert.Equal(5UL, node.Operator.End );
        }
        
        [Fact]
        public void TestFalseAtom()
        {
            var parser = Setup( new Token[] { new Token(0, 5, Token.TokenKind.PyFalse ), new Token(5, 5, Token.TokenKind.EOF) } );
            
            Assert.True(parser != null);
            FalseLiteralExpression node = (FalseLiteralExpression)parser.ParseAtom();
            Assert.Equal(Token.TokenKind.PyFalse, node.Operator.Kind );
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.Equal(0UL, node.Operator.Start );
            Assert.Equal(5UL, node.Operator.End );
        }
        
        [Fact]
        public void TestStringSingleAtom()
        {
            var parser = Setup( new Token[] { new Token(0, 5, Token.TokenKind.String ), new Token(5, 5, Token.TokenKind.EOF) } );
            
            Assert.True(parser != null);
            StringLiteralExpression node = (StringLiteralExpression)parser.ParseAtom();
            Assert.Equal(Token.TokenKind.String, node.Strings[0].Kind );
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.Equal(0UL, node.Strings[0].Start );
            Assert.Equal(5UL, node.Strings[0].End );
        }
        
        [Fact]
        public void TestStringMultipleAtom()
        {
            var parser = Setup( new Token[]
            {
                new Token(0, 5, Token.TokenKind.String ), 
                new Token(10, 15, Token.TokenKind.String ), 
                new Token(20, 26, Token.TokenKind.String ), 
                new Token(26, 26, Token.TokenKind.EOF)
            } );
            
            Assert.True(parser != null);
            StringLiteralExpression node = (StringLiteralExpression)parser.ParseAtom();
            Assert.Equal(3, node.Strings.Length);
            Assert.Equal(Token.TokenKind.String, node.Strings[0].Kind );
            Assert.Equal(0UL, node.Start );
            Assert.Equal(26UL, node.End );
            Assert.Equal(0UL, node.Strings[0].Start );
            Assert.Equal(5UL, node.Strings[0].End );
            Assert.Equal(Token.TokenKind.String, node.Strings[1].Kind );
            Assert.Equal(10UL, node.Strings[1].Start );
            Assert.Equal(15UL, node.Strings[1].End );
            Assert.Equal(Token.TokenKind.String, node.Strings[2].Kind );
            Assert.Equal(20UL, node.Strings[2].Start );
            Assert.Equal(26UL, node.Strings[2].End );
        }
        
        [Fact]
        public void TestPowerNoOperator()
        {
            var parser = Setup( new Token[] { new Token(0, 5, Token.TokenKind.Name ), new Token(5, 5, Token.TokenKind.EOF) } );
            
            Assert.True(parser != null);
            NameLiteralExpression node = (NameLiteralExpression)parser.ParsePower();
            Assert.Equal(Token.TokenKind.Name, node.Name.Kind );
            Assert.Equal(0UL, node.Start );
            Assert.Equal(5UL, node.End );
            Assert.Equal(0UL, node.Name.Start );
            Assert.Equal(5UL, node.Name.End );
        }
        
        [Fact]
        public void TestPowerOperator()
        {
            var parser = Setup( new Token[]
            {
                new Token(0, 5, Token.TokenKind.Name ), 
                new Token(5, 6, Token.TokenKind.PyPower),
                new Token(6, 12, Token.TokenKind.Name ),
                new Token(12, 13, Token.TokenKind.EOF)
            } );
            
            Assert.True(parser != null);
            PowerExpression node = (PowerExpression)parser.ParsePower();
            Assert.Equal(Token.TokenKind.PyPower, node.Operator.Kind );
            Assert.Equal(0UL, node.Start );
            Assert.Equal(12UL, node.End );
            
            Assert.True(node.Left is NameLiteralExpression);
            Assert.Equal(0UL, node.Left.Start );
            Assert.Equal(5UL, node.Left.End );
            
            Assert.True(node.Right is NameLiteralExpression);
            Assert.Equal(6UL, node.Right.Start );
            Assert.Equal(12UL, node.Right.End );
        }
        
    }
}