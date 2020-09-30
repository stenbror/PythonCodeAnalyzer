namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class LambdaExpression : ExpressionNode
    {
        public Token Operator1 { get; set; }
        public ExpressionNode Left { get; set; }
        public Token Operator2 { get; set; }
        public ExpressionNode Right { get; set; }
        public bool IsConditional { get; set; }

        public LambdaExpression(uint start, uint end, bool isCond, Token op1, ExpressionNode left, Token op2, ExpressionNode right) : base(start, end)
        {
            IsConditional = isCond;
            Operator1 = op1;
            Left = left;
            Operator2 = op2;
            Right = right;
        }
    }
}