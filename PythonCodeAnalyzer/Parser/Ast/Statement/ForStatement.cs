namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class ForStatement : StatementNode
    {
        public Token Operator1 { get; set; }              // 'for'
        public ExpressionNode Left { get; set; }          // exprlist
        public Token Operator2 { get; set; }              // 'in'
        public ExpressionNode Right { get; set; }         // testlist
        public Token Operator3 { get; set; }              // ':'
        public Token TypeComment { get; set; }            // typecomment
        public StatementNode Next { get; set; }           // suite    
        public StatementNode ElseElement { get; set; }    // elsepart
        
        public ForStatement(uint start, uint end, Token op1, ExpressionNode left, Token op2, ExpressionNode right, Token op3, Token op4, StatementNode next, StatementNode elseElement) : base(start, end)
        {
            Operator1 = op1;
            Operator2 = op2;
            Operator3 = op3;
            TypeComment = op4;
            Left = left;
            Right = right;
            Next = next;
            ElseElement = elseElement;
        }
    }
}