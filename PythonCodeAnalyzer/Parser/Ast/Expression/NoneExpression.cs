namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class NoneExpression : ExpressionNode
    {
        public Token Operator { get; set; }
        
        public NoneExpression(uint start, uint end, Token op) : base(start, end)
        {
            Operator = op;
        }
    }
}