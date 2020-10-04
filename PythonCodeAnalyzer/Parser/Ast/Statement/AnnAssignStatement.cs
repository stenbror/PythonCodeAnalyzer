namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class AnnAssignStatement : StatementNode
    {
        public ExpressionNode Left { get; set; }
        public Token Colon { get; set; }
        public ExpressionNode Type { get; set; }
        public Token Assignment { get; set; }
        public ExpressionNode Right { get; set; }
        
        public AnnAssignStatement(uint start, uint end, ExpressionNode left, Token colon, ExpressionNode type, Token assign, ExpressionNode right) : base(start, end)
        {
            Left = left;
            Colon = colon;
            Type = type;
            Assignment = assign;
            Right = right;
        }
    }
}