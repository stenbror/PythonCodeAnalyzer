namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class VFPDefExpression : ExpressionNode
    {
        public Token Name { get; set; }
        
        public VFPDefExpression(uint start, uint end, Token op1) : base(start, end)
        {
            Name = op1;
        }
    }
}