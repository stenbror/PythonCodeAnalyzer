namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class StarArgument : ExpressionNode
    {
        public Token MulOperator { get; set; }
        public Token Name { get; set; }
        
        public StarArgument(uint start, uint end, Token op, Token name) : base(start, end)
        {
            MulOperator = op;
            Name = name;
        }
    }
}