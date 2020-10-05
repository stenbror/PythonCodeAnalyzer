namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class ClassDeclarationStatement : StatementNode
    {
        public Token  Operator1 { get; set; }
        public Token  ClassName { get; set; }
        public Token  Operator2 { get; set; }
        public ExpressionNode Left { get; set; }
        public Token  Operator3 { get; set; }
        public Token  Operator4 { get; set; }
        public StatementNode Right { get; set; }
        
        public ClassDeclarationStatement
            (uint start, uint end, Token op1, Token name, 
                Token op2, ExpressionNode left, Token op3, Token op4, StatementNode right) : base(start, end)
        {
            Operator1 = op1;
            Operator2 = op2;
            Operator3 = op3;
            Operator4 = op4;
            ClassName = name;
            Left = left;
            Right = right;
        }
    }
}