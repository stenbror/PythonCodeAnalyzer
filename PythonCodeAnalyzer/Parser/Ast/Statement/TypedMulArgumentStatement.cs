namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class TypedMulArgumentStatement : StatementNode
    {
        public Token Operator { get; set; }
        public StatementNode Right { get; set; }
        
        public TypedMulArgumentStatement(uint start, uint end, Token op, StatementNode right) : base(start, end)
        {
            Operator = op;
            Right = right;
        }
    }
}