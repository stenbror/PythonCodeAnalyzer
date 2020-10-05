namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class AsyncStatement : StatementNode
    {
        public Token Operator { get; set; }
        public StatementNode Right { get; set; }
        
        public AsyncStatement(uint start, uint end, Token op, StatementNode right) : base(start, end)
        {
            Operator = op;
            Right = right;
        }
    }
}