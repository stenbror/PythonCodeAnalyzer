namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class WithStatement : StatementNode
    {
        public Token Operator1 { get; set; }
        public StatementNode[] WithItems { get; set; }
        public Token[] Separators { get; set; }
        public Token Operator2 { get; set; }
        public Token TypeComment { get; set; }
        public StatementNode Suite { get; set; }
        
        public WithStatement(uint start, uint end, Token op1, StatementNode[] withItems, Token[] separators, Token colon, Token typeComment, StatementNode suite) : base(start, end)
        {
            Operator1 = op1;
            WithItems = withItems;
            Separators = separators;
            Operator2 = colon;
            TypeComment = typeComment;
            Suite = suite;
        }
    }
}