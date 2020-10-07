namespace PythonCodeAnalyzer.Parser.Ast.Statement
{
    public partial class ImportFromStatement : StatementNode
    {
        public Token  Operator1 { get; set; }        // 'from'
        public Token[]  Dots { get; set; }            // '.' '...'
        public StatementNode Left { get; set; }       // dotted_name
        public Token  Operator2 { get; set; }        // 'import'
        public Token  Operator3 { get; set; }        // '(' or '*'
        public StatementNode Right { get; set; }    // import_as_names        
        public Token  Operator4 { get; set; }        // ')'
        
        public ImportFromStatement(
            uint start, 
            uint end, 
            Token op1, 
            Token[] dots, 
            StatementNode left, 
            Token op2, 
            Token op3, 
            StatementNode right, 
            Token op4) : base(start, end)
        {
            Operator1 = op1;
            Operator2 = op2;
            Operator3 = op3;
            Operator4 = op4;
            Left = left;
            Right = right;
            Dots = dots;
        }
    }
}