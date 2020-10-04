namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class ElifStatement : StatementNode
    {
        public Token Operator1 { get; set; }    // 'elif'
        public ExpressionNode Left { get; set; }    // test
        public Token Operator2 { get; set; }    //    ':'
        public StatementNode Right { get; set; }    // suite
        
        public ElifStatement(uint start, uint end, Token op1, ExpressionNode left, Token op2, StatementNode right) : base(start, end)
        {
            Operator1 = op1;
            Left = left;
            Operator2 = op2;
            Right = right;
        }
    }
}