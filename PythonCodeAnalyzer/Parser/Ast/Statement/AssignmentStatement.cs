namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class AssignmentStatement : StatementNode
    {
        public Node Left { get; set; }
        public Token Assignment { get; set; }
        public ExpressionNode Right { get; set; }
        public Token TypeComment { get; set; }
        
        public AssignmentStatement(uint start, uint end, Node left, Token op, ExpressionNode rigt) : base(start, end)
        {
            Left = Left;
            Assignment = op;
            Right = rigt;
            TypeComment = null;
        }
    }
}