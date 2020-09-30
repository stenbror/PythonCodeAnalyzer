namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class TermExpression : ExpressionNode
    {
        public enum OperatorKind
        {
            Unknown,
            Mul,
            Modulo,
            Matrice,
            Div,
            FloorDiv
        }
        
        public OperatorKind TermOperator { get; set; }
        public ExpressionNode Left { get; set; }
        public Token Operator { get; set; }
        public ExpressionNode Right { get; set; }

        public TermExpression(uint start, uint end, OperatorKind kind, ExpressionNode left, Token op, ExpressionNode right) : base(start, end)
        {
            TermOperator = kind;
            Left = left;
            Operator = op;
            Right = right;
        }
    }
}