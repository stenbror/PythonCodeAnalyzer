using PythonCodeAnalyzer.Parser;
using PythonCodeAnalyzer.Parser.Ast.Expression;
using Xunit;

namespace TestPythonCodeAnalyzer
{
    public class TestAstNodes
    {
        [Fact]
        public void TestRelationLess()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.RelationExpression(0, 11,
                RelationExpression.Relation.Less, new NoneExpression(0, 4), new Token(5, 6, Token.TokenKind.PyLess), new NoneExpression(7, 11));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(11UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyLess);
            Assert.True(test.Right is NoneExpression);
            Assert.Equal(RelationExpression.Relation.Less, test.RelationKind);
        }
        
        [Fact]
        public void TestRelationLessEqual()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.RelationExpression(0, 12,
                RelationExpression.Relation.Less, new NoneExpression(0, 4), new Token(5, 7, Token.TokenKind.PyLessEqual), new NoneExpression(8, 12));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(12UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyLessEqual);
            Assert.True(test.Right is NoneExpression);
            Assert.Equal(RelationExpression.Relation.Less, test.RelationKind);
        }
        
        [Fact]
        public void TestRelationGreater()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.RelationExpression(0, 11,
                RelationExpression.Relation.Less, new NoneExpression(0, 4), new Token(5, 6, Token.TokenKind.PyGreater), new NoneExpression(7, 11));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(11UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyGreater);
            Assert.True(test.Right is NoneExpression);
            Assert.Equal(RelationExpression.Relation.Less, test.RelationKind);
        }
        
        [Fact]
        public void TestRelationGreaterEqual()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.RelationExpression(0, 12,
                RelationExpression.Relation.Less, new NoneExpression(0, 4), new Token(5, 7, Token.TokenKind.PyGreaterEqual), new NoneExpression(8, 12));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(12UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyGreaterEqual);
            Assert.True(test.Right is NoneExpression);
            Assert.Equal(RelationExpression.Relation.Less, test.RelationKind);
        }
        
        [Fact]
        public void TestRelationEqual()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.RelationExpression(0, 12,
                RelationExpression.Relation.Less, new NoneExpression(0, 4), new Token(5, 7, Token.TokenKind.PyEqual), new NoneExpression(8, 12));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(12UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyEqual);
            Assert.True(test.Right is NoneExpression);
            Assert.Equal(RelationExpression.Relation.Less, test.RelationKind);
        }
        
        [Fact]
        public void TestRelationNotEqual()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.RelationExpression(0, 12,
                RelationExpression.Relation.Less, new NoneExpression(0, 4), new Token(5, 7, Token.TokenKind.PyNotEqual), new NoneExpression(8, 12));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(12UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyNotEqual);
            Assert.True(test.Right is NoneExpression);
            Assert.Equal(RelationExpression.Relation.Less, test.RelationKind);
        }
        
        [Fact]
        public void TestRelationIs()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.RelationExpression(0, 12,
                RelationExpression.Relation.Less, new NoneExpression(0, 4), new Token(5, 7, Token.TokenKind.PyIs), new NoneExpression(8, 12));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(12UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyIs);
            Assert.True(test.Right is NoneExpression);
            Assert.Equal(RelationExpression.Relation.Less, test.RelationKind);
        }
        
        [Fact]
        public void TestRelationIn()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.RelationExpression(0, 12,
                RelationExpression.Relation.Less, new NoneExpression(0, 4), new Token(5, 7, Token.TokenKind.PyIn), new NoneExpression(8, 12));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(12UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyIn);
            Assert.True(test.Right is NoneExpression);
            Assert.Equal(RelationExpression.Relation.Less, test.RelationKind);
        }
        
        [Fact]
        public void TestRelationIsNot()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.RelationExpression(0, 17,
                RelationExpression.Relation.Less, new NoneExpression(0, 4), new Token(5, 7, Token.TokenKind.PyIs), new Token(8, 12, Token.TokenKind.PyNot), new NoneExpression(13, 17));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(17UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyIs);
            Assert.True(test.Operator2.Kind == Token.TokenKind.PyNot);
            Assert.True(test.Right is NoneExpression);
            Assert.Equal(RelationExpression.Relation.Less, test.RelationKind);
        }
        
        [Fact]
        public void TestRelationNotIn()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.RelationExpression(0, 17,
                RelationExpression.Relation.Less, new NoneExpression(0, 4), new Token(5, 8, Token.TokenKind.PyNot), new Token(9, 11, Token.TokenKind.PyIn), new NoneExpression(13, 17));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(17UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyNot);
            Assert.True(test.Operator2.Kind == Token.TokenKind.PyIn);
            Assert.True(test.Right is NoneExpression);
            Assert.Equal(RelationExpression.Relation.Less, test.RelationKind);
        }
    }
}