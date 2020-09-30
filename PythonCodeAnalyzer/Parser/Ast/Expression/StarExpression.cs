namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class StarExpression : ExpressionNode
    {
        public Token Operator { get; set; }
        public ExpressionNode Right { get; set; }

        public StarExpression(uint start, uint end, Token op, ExpressionNode right) : base(start, end)
        {
            Operator = op;
            Right = right;
        }
    }
}