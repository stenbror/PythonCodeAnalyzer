using System.Threading;

namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class ParameterStatement : StatementNode
    {
        public Token Operator1 { get; set; }
        public StatementNode Right { get; set; }
        public Token Operator2 { get; set; }
        
        public ParameterStatement(uint start, uint end, Token op1, StatementNode right, Token op2) : base(start, end)
        {
            Operator1 = op1;
            Right = right;
            Operator2 = op2;
        }
    }
}