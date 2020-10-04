namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class ExceptStatement : StatementNode
    {
        public Token Operator1 { get; set; }        // 'except'
        public ExpressionNode Left { get; set; }    // test
        public Token Operator2 { get; set; }        // 'as'
        public Token Operator3 { get; set; }        // name
        public Token Operator4 { get; set; }        // ':'
        public StatementNode Right { get; set; }    // suite
        
        public ExceptStatement(uint start, uint end, Token op1, ExpressionNode left, Token op2, Token op3, Token op4, StatementNode right) : base(start, end)
        {
            Operator1 = op1;
            Operator2 = op2;
            Operator3 = op3;
            Operator4 = op4;
            Left = left;
            Right = right;
        }
    }
}