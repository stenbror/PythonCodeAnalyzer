namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class IfStatement : StatementNode
    {
        public Token Operator1 { get; set; }    // 'if'
        public ExpressionNode Left { get; set; } // test
        public Token Operator2 { get; set; } // ':'
        public StatementNode Right { get; set; } // suite
        public StatementNode[] ElifElements { get; set; }
        public StatementNode ElseElement { get; set; }
        
        public IfStatement(uint start, uint end, Token op1, ExpressionNode left, Token op2, StatementNode right, StatementNode[] elifElements, StatementNode elseElement ) : base(start, end)
        {
            Operator1 = op1;
            Left = left;
            Operator2 = op2;
            Right = right;
            ElifElements = elifElements;
            ElseElement = elseElement;
        }
    }
}