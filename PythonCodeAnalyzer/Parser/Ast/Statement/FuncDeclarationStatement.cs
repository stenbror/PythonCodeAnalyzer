namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class FuncDeclarationStatement : StatementNode
    {
        public Token Operator1 { get; set; }
        public Token Name { get; set; }
        public StatementNode Left { get; set; }
        public Token Operator2 { get; set; }
        public ExpressionNode Right { get; set; }
        public Token Operator3 { get; set; }
        public Token TypeComment { get; set; }
        public StatementNode Next { get; set; }
        
        public FuncDeclarationStatement(
            uint start, 
            uint end,
            Token op1,
            Token name,
            StatementNode left,
            Token op2,
            ExpressionNode right,
            Token op3,
            Token commenttype,
            StatementNode next) : base(start, end)
        {
            Operator1 = op1;
            Name = name;
            Operator2 = op2;
            Left = left;
            Operator2 = op2;
            Right = right;
            Operator3 = op3;
            TypeComment = TypeComment;
            Next = next;
        }
    }
}