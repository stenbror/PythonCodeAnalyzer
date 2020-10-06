namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class FuncInputStatement : StatementNode
    {
        public ExpressionNode Right { get; set; }
        public Token[] Newlines { get; set; }
        public Token EOF { get; set; }
        
        public FuncInputStatement(uint start, uint end, ExpressionNode right, Token[] newlines, Token eof) : base(start, end)
        {
            Right = right;
            Newlines = newlines;
            EOF = eof;
        }
    }
}