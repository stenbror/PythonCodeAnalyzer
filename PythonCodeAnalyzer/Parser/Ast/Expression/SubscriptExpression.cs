namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class SubscriptExpression : ExpressionNode
    {
        public ExpressionNode StartPos { get; set; }
        public Token Operator1 { get; set; }
        public ExpressionNode EndPos { get; set; }
        public Token Operator2 { get; set; }
        public ExpressionNode Step { get; set; }
        
        public SubscriptExpression(uint start, uint end, ExpressionNode one, Token op1, ExpressionNode two, Token op2, ExpressionNode three) : base(start, end)
        {
            StartPos = one;
            Operator1 = op1;
            EndPos = two;
            Operator2 = op2;
            Step = three;
        }   
    }
}