namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class AugAssignStatement : StatementNode
    {
        public enum OperatorKind
        {
            Unknown,
            PlusAssign,
            MinusAssign,
            MulAssign,
            PowerAssign,
            DivAssign,
            FloorDivAssign,
            ModuloAssign,
            MatriceAssign,
            AndAssign,
            OrAssign,
            XorAssign,
            ShiftLeftAssign,
            ShiftRightAssign
        }

        public OperatorKind Kind { get; set; }
        public ExpressionNode Left { get; set; }
        public Token Operator { get; set; }
        public ExpressionNode Right { get; set; }
        
        public AugAssignStatement(uint start, uint end, OperatorKind kind, ExpressionNode left, Token op, ExpressionNode right) : base(start, end)
        {
            Kind = kind;
            Left = left;
            Operator = op;
            Right = right;
        }
    }
}