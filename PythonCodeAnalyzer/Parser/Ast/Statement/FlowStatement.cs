namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class FlowStatement : StatementNode
    {
        public enum OperatorKind
        {
            Unknown,
            Continue,
            Break,
            Return,
            Raise
        }

        public OperatorKind Kind { get; set; }
        public Token Operator { get; set; }
        public Token Operator2 { get; set; }
        public ExpressionNode Left { get; set; }
        public ExpressionNode Right { get; set; }
        
        // Constructor for break, continue statement
        public FlowStatement(uint start, uint end, OperatorKind kind, Token op) : base(start, end)
        {
            Kind = kind;
            Operator = op;
        }
        
        // Constructor for return statement
        public FlowStatement(uint start, uint end, OperatorKind kind, Token op, ExpressionNode right) : base(start, end)
        {
            Kind = kind;
            Operator = op;
            Right = right;
        }
        
        // Constructor for raise statement
        public FlowStatement(uint start, uint end, OperatorKind kind, Token op, ExpressionNode left, Token op2, ExpressionNode right) : base(start, end)
        {
            Kind = kind;
            Operator = op;
            Left = left;
            Operator2 = op2;
            Right = right;
        }
    }
}