namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class AtomExpression : ExpressionNode
    {
        public bool IsAwait { get; set; }
        public Token Operator { get; set; }
        public ExpressionNode Right { get; set; }
        public ExpressionNode TrailerCollection { get; set; }
        
        public AtomExpression(uint start, uint end, bool isAwait, Token op, ExpressionNode right, ExpressionNode trailerCollection) : base(start, end)
        {
            IsAwait = isAwait;
            Operator = op;
            Right = right;
            TrailerCollection = trailerCollection;
        }
    }
}