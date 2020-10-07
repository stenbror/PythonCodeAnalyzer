namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class ImportAsNameStatement : StatementNode
    {
        public Token Left { get; set; }
        public Token Operator { get; set; }
        public Token Right { get; set; }
        
        public ImportAsNameStatement(uint start, uint end, Token left, Token op, Token right) : base(start, end)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
    }
}