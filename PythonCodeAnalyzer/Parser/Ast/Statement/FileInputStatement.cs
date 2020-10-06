namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class FileInputStatement : StatementNode
    {
        public Token[] Newlines { get; set; }
        public StatementNode[] Statements { get; set; }
        public Token EOF { get; set; }
        
        public FileInputStatement(uint start, uint end, Token[] newlines, StatementNode[] statements, Token eof) : base(start, end)
        {
            Newlines = newlines;
            Statements = statements;
            EOF = eof;
        }
    }
}