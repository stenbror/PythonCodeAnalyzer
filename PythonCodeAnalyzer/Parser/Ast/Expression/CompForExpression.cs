namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class CompForExpression : ExpressionNode
    {
        public Token AsyncOperator { get; set; }
        public ExpressionNode Right { get; set; }
        
        public CompForExpression(uint start, uint end, Token op, ExpressionNode right) : base(start, end)
        {
            AsyncOperator = op;
            Right = right;
        }
    }
}