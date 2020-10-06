namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class DecoratedStatement : StatementNode
    {
        public StatementNode Decorators { get; set; }
        public StatementNode Right { get; set; }
        
        public DecoratedStatement(uint start, uint end, StatementNode decorator, StatementNode right) : base(start, end)
        {
            Decorators = decorator;
            Right = right;
        }
    }
}