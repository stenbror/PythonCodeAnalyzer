namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class RelationExpression : ExpressionNode
    {
        public enum Relation
        {
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

        public Relation Kind { get; private set; }

        public RelationExpression(uint start, uint end, Relation relation, ExpressionNode left, Token op, ExpressionNode right) 
            : base(start, end)
        {
            Kind = relation;
        }
    }

}