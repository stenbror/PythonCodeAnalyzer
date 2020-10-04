using Microsoft.VisualBasic.CompilerServices;

namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class ElseStatement : StatementNode
    {
        public Token Operator1 { get; set; }    // 'else'
        public Token Operator2 { get; set; }    // ':'
        public StatementNode Right { get; set; }    // suite
        
        public ElseStatement(uint start, uint end, Token op1, Token op2, StatementNode right) : base(start, end)
        {
            Operator1 = op1;
            Operator2 = op2;
            Right = right;
        }
    }
}