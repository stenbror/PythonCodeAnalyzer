using System;
using PythonCodeAnalyzer.Parser.Ast.Expression;
using Xunit;
using TestPythonCodeAnalyzer;


namespace TestPythonCodeAnalyzer
{
    public class TestAstNodes
    {
        [Fact]
        public void TestRelationLess()
        {
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.RelationExpression(0, 5,
                RelationExpression.Relation.Less, null, null, null);
            
            Assert.Equal(0UL, test.Start);
            Assert.Equal(5UL, test.End);
            Assert.True(test.Left == null);
            Assert.True(test.Operator == null);
            Assert.True(test.Right == null);
            Assert.Equal(RelationExpression.Relation.Less, test.RelationKind);
        }
    }
}