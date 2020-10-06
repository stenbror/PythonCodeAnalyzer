namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class SingleInputStatement : StatementNode
    {
        public StatementNode Right { get; set; }
        public Token Newline { get; set; }
        
        public SingleInputStatement(uint start, uint end, StatementNode right, Token op) : base(start, end)
        {
            Right = right;
            Newline = op;
        }
        
    }
}