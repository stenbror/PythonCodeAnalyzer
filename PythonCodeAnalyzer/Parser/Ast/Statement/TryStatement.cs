namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class TryStatement : StatementNode
    {
        public Token Operator1 { get; set; }                    // 'try'
        public Token Operator2 { get; set; }                    // ':'
        public StatementNode Left { get; set; }                 // suite
        public StatementNode[] ExceptElements { get; set; }     // excepts
        public StatementNode ElseElement { get; set; }          // elsePart
        public StatementNode FinallyElements { get; set; }      // finallyPart
        
        public TryStatement(uint start, uint end, Token op1, Token op2, StatementNode left, StatementNode[] excepts, StatementNode elsePart, StatementNode finallyPart) : base(start, end)
        {
            Operator1 = op1;
            Operator2 = op2;
            Left = left;
            ExceptElements = excepts;
            ElseElement = elsePart;
            FinallyElements = finallyPart;
        }   
    }
}