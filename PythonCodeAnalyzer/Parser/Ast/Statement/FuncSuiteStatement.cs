namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class FuncSuiteStatement : StatementNode
    {
        public Token Operator1 { get; set; }    // Newline
        public Token Operator2 { get; set; }    // TypeComment
        public Token Operator3 { get; set; }    // Newline
        public Token Operator4 { get; set; }    // Indent
        public StatementNode[] Statements { get; set; }    // Stmt+
        public Token Operator5 { get; set; }    // Dedent
        
        
        public FuncSuiteStatement(uint start, uint end, Token op1, Token op2, Token op3, Token op4, StatementNode[] nodes, Token op5) : base(start, end)
        {
            Operator1 = op1;
            Operator2 = op2;
            Operator3 = op3;
            Operator4 = op4;
            Operator5 = op5;
            Statements = nodes;
        }
    }
}