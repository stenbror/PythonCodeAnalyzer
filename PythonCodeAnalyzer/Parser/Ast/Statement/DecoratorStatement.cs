namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class DecoratorStatement : StatementNode
    {
        public Token Operator1 { get; set; }
        public StatementNode Left { get; set; }
        public Token Operator2 { get; set; }
        public Token Operator3 { get; set; }
        public ExpressionNode Right { get; set; }
        public Token Operator4 { get; set; }
        
        public DecoratorStatement(uint start, uint end, Token op1, StatementNode left, Token op2, ExpressionNode right, Token op3, Token op4) : base(start, end)
        {
            Operator1 = op1;
            Operator2 = op2;
            Operator3 = op3;
            Operator4 = op4;
            Left = left;
            Right = right;
        }
    }
}