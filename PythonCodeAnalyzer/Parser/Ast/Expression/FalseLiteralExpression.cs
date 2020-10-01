namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public class FalseLiteralExpression : ExpressionNode
    {
        public Token Operator { get; set; }

        public FalseLiteralExpression(uint start, uint end, Token op) : base(start, end)
        {
            Operator = op;
        }
    }
}