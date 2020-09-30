using System.Dynamic;

namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class ConditionalExpression : ExpressionNode
    {
        public ExpressionNode Left { get; set; }
        public Token Operator1 { get; set; }
        public ExpressionNode Right { get; set; }
        public Token Operator2 { get; set; }
        public ExpressionNode Next { get; set; }

        public ConditionalExpression(uint start, uint end, ExpressionNode left, Token op1, ExpressionNode right, Token op2, ExpressionNode next) : base(start, end)
        {
            Left = left;
            Operator1 = op1;
            Right = right;
            Operator2 = op2;
            Next = next;
        }
    }
}