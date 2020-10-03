namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class YieldExpression : ExpressionNode
    {
        public Token Operator1 { get; set; }
        public Token Operator2 { get; set; }
        public ExpressionNode Right { get; set; }
        
        public YieldExpression(uint start, uint end, Token op1, Token op2, ExpressionNode right) : base(start, end)
        {
            Operator1 = op1;
            Operator2 = op2;
            Right = right;
        }
    }
}