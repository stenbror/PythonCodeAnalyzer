namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class AtomExpession : ExpressionNode
    {
        public bool IsAsync { get; set; }
        public Token Operator { get; set; }
        public ExpressionNode Right { get; set; }
        public ExpressionNode[] Trailers { get; set; }
        
        public AtomExpession(uint start, uint end, bool isAsync, Token op, ExpressionNode right, ExpressionNode[] trailers) : base(start, end)
        {
            IsAsync = isAsync;
            Right = right;
            Trailers = trailers;
        }
    }
}