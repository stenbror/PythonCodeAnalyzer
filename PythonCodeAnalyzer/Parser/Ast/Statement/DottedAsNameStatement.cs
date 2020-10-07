namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class DottedAsNameStatement : StatementNode
    {
        public StatementNode Left { get; set; }
        public Token  Operator { get; set; }
        public Token Name { get; set; }
        
        public DottedAsNameStatement(uint start, uint end, StatementNode left, Token op, Token right) : base(start, end)
        {
            Left = left;
            Operator = op;
            Name = right;
        }
    }
}