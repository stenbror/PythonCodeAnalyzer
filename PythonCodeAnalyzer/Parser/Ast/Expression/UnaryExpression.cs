namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class UnaryExpression : ExpressionNode
    {
        public enum UnaryOperator
        {
            Unknown,
            Plus,
            Minus,
            Invert
        }

        public UnaryOperator OperatorKind { get; set; }
        public Token Operator { get; set; }
        public ExpressionNode Right { get; set; }

        public UnaryExpression(uint start, uint end, UnaryOperator kind, Token op, ExpressionNode right) : base(start, end)
        {
            OperatorKind = kind;
            Operator = op;
            Right = right;
        }
    }
}