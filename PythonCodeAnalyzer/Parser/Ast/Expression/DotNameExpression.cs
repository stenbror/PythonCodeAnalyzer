namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public class DotNameExpression : ExpressionNode
    {
        public Token Operator { get; set; }
        public Token Name { get; set; }
        
        public DotNameExpression(uint start, uint end, Token op1, Token op2) : base(start, end)
        {
            Operator = op1;
            Name = op2;
        }
    }
}