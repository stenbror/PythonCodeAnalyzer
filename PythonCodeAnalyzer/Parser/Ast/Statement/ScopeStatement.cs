namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class ScopeStatement : StatementNode
    {
        public enum ScopeKind
        {
            Unknown,
            Global,
            Nonlocal
        }

        public ScopeKind Scope { get; set; }
        public Token Operator { get; set; }
        public Token[] Names { get; set; }
        public Token[] Separators { get; set; }
        
        public ScopeStatement(uint start, uint end, ScopeKind kind, Token op, Token[] names, Token[] commas) : base(start, end)
        {
            Scope = kind;
            Operator = op;
            Names = names;
            Separators = commas;
        }
    }
}