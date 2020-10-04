namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class AssertStatement : StatementNode
    {
        public Token Operator1 { get; set; }
        public ExpressionNode Left { get; set; }
        public Token Operator2 { get; set; }
        public ExpressionNode Right { get; set; }
            
        
        public AssertStatement(uint start, uint end, Token op1, ExpressionNode left, Token op2, ExpressionNode right) : base(start, end)
        {
            Operator1 = op1;
            Left = left;
            Operator2 = op2;
            Right = right;
        }
    }
}