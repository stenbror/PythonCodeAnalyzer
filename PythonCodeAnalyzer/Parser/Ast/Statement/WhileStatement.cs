namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class WhileStatement : StatementNode
    {
        public Token Operator1 { get; set; } // 'while'
        public ExpressionNode Left { get; set; }    // NamedExpr
        public Token Operator2 { get; set; }    // ':'
        public StatementNode Right { get; set; }    // suite
        public StatementNode ElseElement { get; set; }    // else part
        
        public WhileStatement(uint start, uint end, Token op1, ExpressionNode left, Token op2, StatementNode right, StatementNode elseElement) : base(start, end)
        {
            Operator1 = op1;
            Left = left;
            Operator2 = op2;
            Right = right;
            ElseElement = elseElement;
        }
    }
}