namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class ArithExpression : ExpressionNode
    {
        public enum ArithOperatorKind
        {
            Unknown,
            Plus,
            Minus
        }
        
        public ArithOperatorKind ArithOperator { get; set; }
        public ExpressionNode Left { get; set; }
        public Token Operator { get; set; }
        public ExpressionNode Right { get; set; }

        public ArithExpression(uint start, uint end, ArithOperatorKind kind,  ExpressionNode left, Token op, ExpressionNode right) : base(start, end)
        {
            ArithOperator = kind;
            Left = left;
            Operator = op;
            Right = right;
        }
    }
}