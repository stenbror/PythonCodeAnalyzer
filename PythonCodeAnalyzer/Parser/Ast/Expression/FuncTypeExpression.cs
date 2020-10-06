using System.Buffers.Text;

namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class FuncTypeExpression : ExpressionNode
    {
        public Token Operator1 { get; set; }        // '('
        public ExpressionNode Left { get; set; }    // TypeList
        public Token Operator2 { get; set; }        // ')'
        public Token Operator3 { get; set; }        // '->'
        public ExpressionNode Right { get; set; }   // Test
        
        
        public FuncTypeExpression(uint start, uint end, Token op1, ExpressionNode left, Token op2, Token op3,
            ExpressionNode right) : base(start, end)
        {
            Operator1 = op1;
            Left = left;
            Operator2 = op2;
            Operator3 = op3;
            Right = right;
        }
    }
}