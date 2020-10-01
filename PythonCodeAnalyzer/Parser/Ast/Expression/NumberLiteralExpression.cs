namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class NumberLiteralExpression : ExpressionNode
    {
        public Token Number { get; set; }
        
        public NumberLiteralExpression(uint start, uint end, Token op) : base(start, end)
        {
            Number = op;
        }
    }
}