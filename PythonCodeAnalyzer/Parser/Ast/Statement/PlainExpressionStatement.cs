namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class PlainExpressionStatement : StatementNode
    {
        public ExpressionNode Node { get; set; }
        
        public PlainExpressionStatement(uint start, uint end, ExpressionNode node) : base(start, end)
        {
            Node = node;
        }
    }
}