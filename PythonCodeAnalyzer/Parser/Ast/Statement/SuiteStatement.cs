namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class SuiteStatement : StatementNode
    {
        public Token Operator1 { get; set; }    // Newline
        public Token Operator2 { get; set; }    // Indent
        public StatementNode[] Statements { get; set; }    // Stmt+
        public Token Operator3 { get; set; }    // Dedent
        
        
        public SuiteStatement(uint start, uint end, Token op1, Token op2, StatementNode[] nodes, Token op3) : base(start, end)
        {
            Operator1 = op1;
            Operator2 = op2;
            Operator3 = op3;
            Statements = nodes;
        }
    }
}