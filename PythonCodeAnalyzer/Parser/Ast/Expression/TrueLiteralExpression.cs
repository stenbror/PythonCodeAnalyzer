namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class TrueLiteralExpression : ExpressionNode
    {
        public Token Operator { get; set; }

        public TrueLiteralExpression(uint start, uint end, Token op) : base(start, end)
        {
            Operator = op;
        }
    }
}