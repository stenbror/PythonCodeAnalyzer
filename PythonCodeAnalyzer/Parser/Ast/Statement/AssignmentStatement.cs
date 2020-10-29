using System.Collections.Generic;

namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class AssignmentStatement : StatementNode
    {
        public ExpressionNode Left { get; set; }
        public List<Token> Assignment { get; set; }
        public List<ExpressionNode> Right { get; set; }
        public Token TypeComment { get; set; }

        public AssignmentStatement(uint start, uint end, ExpressionNode left, List<Token> op, List<ExpressionNode> right, Token typeComment) :
            base(start, end)
        {
            Left = left;
            Assignment = op;
            Right = right;
            TypeComment = typeComment;
        }
    }
}