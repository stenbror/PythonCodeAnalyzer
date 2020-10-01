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
                CurSymbol = Symbols[++index];
                Position = CurSymbol.End;
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
            Assert.Equal(Token.TokenKind.Name, node.Name.Kind );
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
            Assert.Equal(Token.TokenKind.Number, node.Number.Kind );
            Assert.Equal(0UL, node.Number.Start );
            Assert.Equal(5UL, node.Number.End );
        }
    }
}