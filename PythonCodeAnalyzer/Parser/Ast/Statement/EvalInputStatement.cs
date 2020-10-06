namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class EvalInputStatement : StatementNode
    {
        public ExpressionNode Right { get; set; }
        public Token[] Newlines { get; set; }
        public Token EOF { get; set; }
        
        public EvalInputStatement(uint start, uint end, ExpressionNode right, Token[] newlines, Token eof) : base(start, end)
        {
            Right = right;
            Newlines = newlines;
            EOF = eof;
        }
    }
}