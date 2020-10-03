namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class SetContainerExpression : ExpressionNode
    {
        public ExpressionNode[] Keys { get; set; }
        public Token[] Separators { get; set; }
        
        public SetContainerExpression(uint start, uint end, ExpressionNode[] keys, Token[] separators) : base(start, end)
        {
            Keys = keys;
            Separators = separators;
        }
    }
}