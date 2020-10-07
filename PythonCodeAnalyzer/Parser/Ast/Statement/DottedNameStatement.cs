namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class DottedNameStatement : StatementNode
    {
        public Token[] Names { get; set; }
        public Token[] Dots { get; set; }
        
        public DottedNameStatement(uint start, uint end, Token[] names, Token[] dots) : base(start, end)
        {
            Names = names;
            Dots = dots;
        }
    }
}