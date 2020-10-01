namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class ElipsisLiteralExpression : ExpressionNode
    {
        public Token Elipsis { get; set; }
        
        public ElipsisLiteralExpression(uint start, uint end, Token op) : base(start, end)
        {
            Elipsis = op;
        }
    }
}