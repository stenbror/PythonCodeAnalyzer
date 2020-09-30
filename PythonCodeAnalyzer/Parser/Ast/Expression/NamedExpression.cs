namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class NamedExpression : ExpressionNode
    {
        public ExpressionNode Left { get; set; }
        public Token Operator { get; set; }
        public ExpressionNode Right { get; set; }

        public NamedExpression(uint start, uint end, ExpressionNode left, Token op, ExpressionNode right) : base(start, end)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
    }
}