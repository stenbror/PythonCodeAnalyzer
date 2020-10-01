namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class FactorExpression : ExpressionNode
    {
        public enum FactorOperatorKind
        {
            Unknown,
            UnaryPlus,
            UnaryMinus,
            UnaryInvert
        }

        public FactorOperatorKind FactorOperator { get; set; }
        public Token Operator { get; set; }
        public ExpressionNode Right { get; set; }

        public FactorExpression(uint start, uint end, FactorOperatorKind kind, Token op, ExpressionNode right) : base(start,
            end)
        {
            FactorOperator = kind;
            Operator = op;
            Right = right;
        }
    }
}