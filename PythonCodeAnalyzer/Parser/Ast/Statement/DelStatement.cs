namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class DelStatement : StatementNode
    {
        public Token Operator { get; set; }
        public ExpressionNode Right { get; set; }
        
        public DelStatement(uint start, uint end, Token op, ExpressionNode right) : base(start, end)
        {
            Operator = op;
            Right = right;
        }
    }
}