namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class PowerArgument : ExpressionNode
    {
        public Token PowerOperator { get; set; }
        public Token Name { get; set; }
        
        public PowerArgument(uint start, uint end, Token op, Token name) : base(start, end)
        {
            PowerOperator = op;
            Name = name;
        }
    }
}