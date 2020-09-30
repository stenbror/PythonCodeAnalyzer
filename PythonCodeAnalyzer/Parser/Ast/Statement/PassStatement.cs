namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class PassStatement : StatementNode
    {
        public Token Operator { get; set; }
        
        public PassStatement(uint start, uint end, Token op)  : base(start, end)
        {
            Operator = op;
        }
    }
}