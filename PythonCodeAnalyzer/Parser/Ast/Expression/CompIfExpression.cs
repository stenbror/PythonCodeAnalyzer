namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class CompIfExpression : ExpressionNode
    {
        public Token Operator { get; set; }
        public ExpressionNode Right { get; set; }
        public ExpressionNode Next { get; set; }
        
        public CompIfExpression(uint start, uint end, Token op, ExpressionNode right, ExpressionNode next) : base(start, end)
        {
            Operator = op;
            Right = right;
            Next = next;
        }
    }
}