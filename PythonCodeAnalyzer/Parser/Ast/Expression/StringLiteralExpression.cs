namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class StringLiteralExpression : ExpressionNode
    {
        public Token[] Strings { get; set; }
        
        public StringLiteralExpression(uint start, uint end, Token[] units) : base(start, end)
        {
            Strings = units;
        }
    }
}