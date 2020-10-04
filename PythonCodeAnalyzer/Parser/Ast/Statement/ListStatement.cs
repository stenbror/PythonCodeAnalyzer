namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class ListStatement : StatementNode
    {
        public enum ListKind
        {
            Unknown,
            StatementList,
            SimpleStatementList
        }

        public ListKind Kind { get; set; }
        public StatementNode[] Elements { get; set; }
        public Token[] Separators { get; set; }
        public Token NewLine { get; set; }
        
        public ListStatement(uint start, uint end, ListKind kind, StatementNode[] elements, Token[] separators, Token newline) : base(start, end)
        {
            Kind = kind;
            Elements = elements;
            Separators = separators;
            NewLine = newline;
        }
    }
}