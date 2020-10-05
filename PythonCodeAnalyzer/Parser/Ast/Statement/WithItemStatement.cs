namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class WithItemStatement : StatementNode
    {
        public ExpressionNode Left { get; set; }
        public Token Operator1 { get; set; }
        public ExpressionNode Right { get; set; }
        
        public WithItemStatement(uint start, uint end, ExpressionNode left, Token op, ExpressionNode right) : base(start, end)
        {
            Left = left;
            Operator1 = op;
            Right = right;
        }
    }
}