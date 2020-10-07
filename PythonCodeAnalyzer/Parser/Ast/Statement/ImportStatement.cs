namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class ImportStatement : StatementNode
    {
        public Token Operaor { get; set; }
        public StatementNode Right { get; set; }
        
        public ImportStatement(uint start, uint end, Token op, StatementNode right) : base(start, end)
        {
            Operaor = op;
            Right = right;
        }
    }
}