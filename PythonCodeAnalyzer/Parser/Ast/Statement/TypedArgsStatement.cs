namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class TypedArgsStatement : StatementNode
    {
        public StatementNode[] Elements { get; set; }
        public Token[] Separators { get; set; }
        public Token[] TypeComments { get; set; }
        public Token Div { get; set; }
        
        public TypedArgsStatement(uint start, uint end, StatementNode[] elements, Token[] separators, Token[] typeComments, Token div) : base(start, end)
        {
            Elements = elements;
            Separators = separators;
            TypeComments = typeComments;
            Div = div;
        }
    }
}