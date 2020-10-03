using System.IO.MemoryMappedFiles;

namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class ArgumentExpression : ExpressionNode
    {
        public Token Left { get; set; }
        public Token Operator { get; set; }
        public ExpressionNode Right { get; set; }
        
        public ArgumentExpression(uint start, uint end, Token left, Token op, ExpressionNode right) : base(start, end)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
    }
}