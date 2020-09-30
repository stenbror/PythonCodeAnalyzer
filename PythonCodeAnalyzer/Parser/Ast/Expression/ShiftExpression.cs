namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class ShiftExpression : ExpressionNode
    {
        public enum OperatorKind
        {
            Unknown,
            Left,
            Right
        }
        
        public OperatorKind ShiftOperator { get; set; }
        public ExpressionNode Left { get; set; }
        public Token Operator { get; set; }
        public ExpressionNode Right { get; set; }

        public ShiftExpression(uint start, uint end, OperatorKind kind, ExpressionNode left, Token op, ExpressionNode right) : base(start, end)
        {
            ShiftOperator = kind;
            Left = left;
            Operator = op;
            Right = right;
        }
    }
}