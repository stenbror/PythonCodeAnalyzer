namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class TypeListExpression : ExpressionNode
    {
        public Token Mul { get; set; }
        public ExpressionNode Left { get; set; }
        public Token Power { get; set; }
        public ExpressionNode Right { get; set; }
        public ExpressionNode[] Elements { get; set; }
        public Token[] Separators { get; set; }
        
        public TypeListExpression(
            uint start, 
            uint end, 
            Token mul, 
            ExpressionNode left, 
            Token power, 
            ExpressionNode right, 
            ExpressionNode[] elements, 
            Token[] separators) : base(start, end)
        {
            Mul = mul;
            Left = left;
            Power = power;
            Right = right;
            Elements = elements;
            Separators = separators;
        }
    }
}