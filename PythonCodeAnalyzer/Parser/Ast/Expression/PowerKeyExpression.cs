namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class PowerKeyExpression : ExpressionNode
    {
        public Token Operator { get; set; }
        public ExpressionNode Right { get; set; }
        
        public PowerKeyExpression(uint start, uint end, Token op, ExpressionNode right) : base(start, end)
        {
            Operator = op;
            Right = right;
        }
    }
}