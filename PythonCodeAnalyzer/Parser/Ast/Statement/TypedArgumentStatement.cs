namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class TypedArgumentStatement : StatementNode
    {
        public StatementNode Left { get; set; }
        public Token Operator { get; set; }
        public ExpressionNode Right { get; set; }
        
        public TypedArgumentStatement(uint start, uint end, StatementNode left, Token op, ExpressionNode right) : base(start, end)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
    }
}