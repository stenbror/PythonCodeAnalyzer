namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class AtomListExpression : ExpressionNode
    {
        public Token Operator1 { get; set; }
        public ExpressionNode Right { get; set; }
        public Token Operator2 { get; set; }
        
        public AtomListExpression(uint start, uint end, Token op1, ExpressionNode right, Token op2) : base(start, end)
        {
            Operator1 = op1;
            Right = right;
            Operator2 = op2;
        }
    }
}