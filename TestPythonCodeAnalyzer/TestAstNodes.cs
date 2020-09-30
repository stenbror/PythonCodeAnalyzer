using PythonCodeAnalyzer.Parser;
using PythonCodeAnalyzer.Parser.Ast.Expression;
using Xunit;

namespace TestPythonCodeAnalyzer
{
    public class AstNodes
    {
        [Fact]
        public void TestNamedExpr()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.NamedExpression(0, 12,
                new NoneExpression(0, 4, new Token(0, 4, Token.TokenKind.PyNone)), new Token(5, 7, Token.TokenKind.PyColonAssign), new NoneExpression(8, 12, new Token(8, 12, Token.TokenKind.PyNone)));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(12UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyColonAssign);
            Assert.True(test.Right is NoneExpression);
            Assert.True(test is NamedExpression);
        }
        
        [Fact]
        public void TestConditionalExpr()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.ConditionalExpression(
                0, 
                23,
                new NoneExpression(0, 4, new Token(0, 4, Token.TokenKind.PyNone)),
                new Token(5, 7, Token.TokenKind.PyIf),
                new NoneExpression(8, 12, new Token(8, 12, Token.TokenKind.PyNone)),
                new Token(14, 18, Token.TokenKind.PyElse),
                new NoneExpression(19, 23, new Token(19, 23, Token.TokenKind.PyNone)));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(23UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator1.Kind == Token.TokenKind.PyIf);
            Assert.True(test.Right is NoneExpression);
            Assert.True(test.Operator2.Kind == Token.TokenKind.PyElse);
            Assert.True(test.Next is NoneExpression);
            Assert.True(test is ConditionalExpression);
        }
        
        [Fact]
        public void TestLambdaExpr()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.LambdaExpression(
                0, 
                23,
                true,
                new Token(5, 7, Token.TokenKind.PyLambda),
                new NoneExpression(8, 12, new Token(8, 12, Token.TokenKind.PyNone)),
                new Token(14, 18, Token.TokenKind.PyColon),
                new NoneExpression(19, 23, new Token(19, 23, Token.TokenKind.PyNone)));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(23UL, test.End);
            Assert.True(test.IsConditional);
            Assert.True(test.Operator1.Kind == Token.TokenKind.PyLambda);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator2.Kind == Token.TokenKind.PyColon);
            Assert.True(test.Right is NoneExpression);
            Assert.True(test is LambdaExpression);
        }
        
        
        
        
        [Fact]
        public void TestRelationLess()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.RelationExpression(0, 11,
                RelationExpression.Relation.Less, new NoneExpression(0, 4, new Token(0, 4, Token.TokenKind.PyNone)), new Token(5, 6, Token.TokenKind.PyLess), new NoneExpression(7, 11, new Token(7, 11, Token.TokenKind.PyNone)));
            
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
                RelationExpression.Relation.LessEqual, new NoneExpression(0, 4, new Token(0, 4, Token.TokenKind.PyNone)), new Token(5, 7, Token.TokenKind.PyLessEqual), new NoneExpression(8, 12, new Token(8, 12, Token.TokenKind.PyNone)));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(12UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyLessEqual);
            Assert.True(test.Right is NoneExpression);
            Assert.Equal(RelationExpression.Relation.LessEqual, test.RelationKind);
        }
        
        [Fact]
        public void TestRelationGreater()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.RelationExpression(0, 11,
                RelationExpression.Relation.Greater, new NoneExpression(0, 4, new Token(0, 4, Token.TokenKind.PyNone)), new Token(5, 6, Token.TokenKind.PyGreater), new NoneExpression(7, 11, new Token(7, 11, Token.TokenKind.PyNone)));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(11UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyGreater);
            Assert.True(test.Right is NoneExpression);
            Assert.Equal(RelationExpression.Relation.Greater, test.RelationKind);
        }
        
        [Fact]
        public void TestRelationGreaterEqual()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.RelationExpression(0, 12,
                RelationExpression.Relation.GreaterEqual, new NoneExpression(0, 4, new Token(0, 4, Token.TokenKind.PyNone)), new Token(5, 7, Token.TokenKind.PyGreaterEqual), new NoneExpression(8, 12, new Token(8, 12, Token.TokenKind.PyNone)));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(12UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyGreaterEqual);
            Assert.True(test.Right is NoneExpression);
            Assert.Equal(RelationExpression.Relation.GreaterEqual, test.RelationKind);
        }
        
        [Fact]
        public void TestRelationEqual()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.RelationExpression(0, 12,
                RelationExpression.Relation.Equal, new NoneExpression(0, 4, new Token(0, 4, Token.TokenKind.PyNone)), new Token(5, 7, Token.TokenKind.PyEqual), new NoneExpression(8, 12, new Token(8, 12, Token.TokenKind.PyNone)));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(12UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyEqual);
            Assert.True(test.Right is NoneExpression);
            Assert.Equal(RelationExpression.Relation.Equal, test.RelationKind);
        }
        
        [Fact]
        public void TestRelationNotEqual()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.RelationExpression(0, 12,
                RelationExpression.Relation.NotEqual, new NoneExpression(0, 4, new Token(0, 4, Token.TokenKind.PyNone)), new Token(5, 7, Token.TokenKind.PyNotEqual), new NoneExpression(8, 12, new Token(8, 12, Token.TokenKind.PyNone)));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(12UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyNotEqual);
            Assert.True(test.Right is NoneExpression);
            Assert.Equal(RelationExpression.Relation.NotEqual, test.RelationKind);
        }
        
        [Fact]
        public void TestRelationIs()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.RelationExpression(0, 12,
                RelationExpression.Relation.Is, new NoneExpression(0, 4, new Token(0, 4, Token.TokenKind.PyNone)), new Token(5, 7, Token.TokenKind.PyIs), new NoneExpression(8, 12, new Token(8, 12, Token.TokenKind.PyNone)));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(12UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyIs);
            Assert.True(test.Right is NoneExpression);
            Assert.Equal(RelationExpression.Relation.Is, test.RelationKind);
        }
        
        [Fact]
        public void TestRelationIn()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.RelationExpression(0, 12,
                RelationExpression.Relation.In, new NoneExpression(0, 4, new Token(0, 4, Token.TokenKind.PyNone)), new Token(5, 7, Token.TokenKind.PyIn), new NoneExpression(8, 12, new Token(8, 12, Token.TokenKind.PyNone)));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(12UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyIn);
            Assert.True(test.Right is NoneExpression);
            Assert.Equal(RelationExpression.Relation.In, test.RelationKind);
        }
        
        [Fact]
        public void TestRelationIsNot()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.RelationExpression(0, 17,
                RelationExpression.Relation.IsNot, new NoneExpression(0, 4, new Token(0, 4, Token.TokenKind.PyNone)), new Token(5, 7, Token.TokenKind.PyIs), new Token(8, 12, Token.TokenKind.PyNot), new NoneExpression(13, 17, new Token(13, 17, Token.TokenKind.PyNone)));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(17UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyIs);
            Assert.True(test.Operator2.Kind == Token.TokenKind.PyNot);
            Assert.True(test.Right is NoneExpression);
            Assert.Equal(RelationExpression.Relation.IsNot, test.RelationKind);
        }
        
        [Fact]
        public void TestRelationNotIn()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.RelationExpression(0, 17,
                RelationExpression.Relation.NotIn, new NoneExpression(0, 4, new Token(0, 4, Token.TokenKind.PyNone)), new Token(5, 8, Token.TokenKind.PyNot), new Token(9, 11, Token.TokenKind.PyIn), new NoneExpression(13, 17, new Token(13, 17, Token.TokenKind.PyNone)));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(17UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyNot);
            Assert.True(test.Operator2.Kind == Token.TokenKind.PyIn);
            Assert.True(test.Right is NoneExpression);
            Assert.Equal(RelationExpression.Relation.NotIn, test.RelationKind);
        }
        
        
        [Fact]
        public void TestArithPlus()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.ArithExpression(0, 11,
                ArithExpression.ArithOperatorKind.Plus, new NoneExpression(0, 4, new Token(0, 4, Token.TokenKind.PyPlus)), new Token(5, 6, Token.TokenKind.PyPlus),new NoneExpression(7, 11, new Token(7, 11, Token.TokenKind.PyNone)));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(11UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyPlus);
            Assert.True(test.Right is NoneExpression);
            Assert.Equal(ArithExpression.ArithOperatorKind.Plus, test.ArithOperator);
        }
        
        [Fact]
        public void TestArithMinus()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.ArithExpression(0, 11,
                ArithExpression.ArithOperatorKind.Minus, new NoneExpression(0, 4, new Token(0, 4, Token.TokenKind.PyMinus)), new Token(5, 6, Token.TokenKind.PyMinus), new NoneExpression(7, 11, new Token(7, 11, Token.TokenKind.PyNone)));
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(11UL, test.End);
            Assert.True(test.Left is NoneExpression);
            Assert.True(test.Operator.Kind == Token.TokenKind.PyMinus);
            Assert.True(test.Right is NoneExpression);
            Assert.Equal(ArithExpression.ArithOperatorKind.Minus, test.ArithOperator);
        }
    }
}