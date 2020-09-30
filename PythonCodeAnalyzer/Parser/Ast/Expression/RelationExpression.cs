namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class RelationExpression : ExpressionNode
    {
        public enum Relation
        {
            Unused,
            Less,
            LessEqual,
            Equal,
            GreaterEqual,
            Greater,
            NotEqual,
            In,
            NotIn,
            Is,
            IsNot
        };

        public Relation RelationKind { get; set; }
        public ExpressionNode Left { get; set; }
        public Token Operator { get; set; }
        public Token Operator2 { get; set; }
        public ExpressionNode Right { get; set; }

        public RelationExpression(uint start, uint end, Relation relation, ExpressionNode left, Token op, ExpressionNode right) 
            : base(start, end)
        {
            RelationKind = relation;
            Left = left;
            Operator = op;
            Operator2 = null;
            Right = right;
        }
        
        public RelationExpression(uint start, uint end, Relation relation, ExpressionNode left, Token op1, Token op2, ExpressionNode right) 
            : base(start, end)
        {
            RelationKind = relation;
            Left = left;
            Operator = op1;
            Operator2 = op2;
            Right = right;
        }
    }

}