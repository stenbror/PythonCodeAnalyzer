namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class DictionaryContainerExpression :  ExpressionNode
    {
        public ExpressionNode[] Keys { get; set; }
        public Token[] Colons { get; set; }
        public ExpressionNode[] Values { get; set; }
        public Token[] Separators { get; set; }
        
        public DictionaryContainerExpression(uint start, uint end, ExpressionNode[] keys, Token[] colons, ExpressionNode[] values, Token[] separators) : base(start, end)
        {
            Keys = keys;
            Colons = colons;
            Values = values;
            Separators = separators;
        }
    }
}