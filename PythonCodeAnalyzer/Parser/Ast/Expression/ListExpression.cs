namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class ListExpression : ExpressionNode
    {
        public enum ListType
        {
            Unknown,
            ExprList,
            TestList,
            SubscriptList,
            ArgumentList,
            SetEntries,
            DictionaryEntries
        }

        public ListType ContainerType { get; set; }
        public ExpressionNode[] Elements { get; set; }
        public Token[] Separators { get; set; }
        
        public ListExpression(uint start, uint end, ListType type, ExpressionNode[] elements, Token[] separators) : base(start, end)
        {
            ContainerType = type;
            Elements = elements;
            Separators = separators;
        }
    }
}