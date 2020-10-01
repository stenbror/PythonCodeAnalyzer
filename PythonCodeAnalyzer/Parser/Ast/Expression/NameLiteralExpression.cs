namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class NameLiteralExpression : ExpressionNode
    {
        public Token Name { get; set; }
        
        public NameLiteralExpression(uint start, uint end, Token op) : base(start, end)
        {
            Name = op;
        }
    }
}