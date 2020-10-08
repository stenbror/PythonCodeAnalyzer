namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class VarArgsListExpression : ExpressionNode
    {
        public ExpressionNode[] Elements { get; set; }
        public Token[] Separators { get; set; }
        public Token Div { get; set; }
        
        public VarArgsListExpression(uint start, uint end, ExpressionNode[] elements, Token[] separators, Token div) : base(start, end)
        {
            Elements = elements;
            Separators = separators;
            Div = div;
        }
    }
}