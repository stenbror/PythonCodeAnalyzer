namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class ImportListStatement : StatementNode
    {
        public enum ListKind
        {
            Unknown,
            DottedAsNames,
            ImportAsNames
        };

        public ListKind Kind { get; set; }
        public StatementNode[] Elements { get; set; }
        public Token[] Separators { get; set; }
        
        public ImportListStatement(uint start, uint end, ListKind kind, StatementNode[] elements, Token[] commas) : base(start, end)
        {
            Kind = kind;
            Elements = elements;
            Separators = commas;
        }
    }
}