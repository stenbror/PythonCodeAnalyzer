namespace PythonCodeAnalyzer.Parser.Ast
{
    public class Node
    {
        public uint Start { get; set; }
        public uint End { get; set; }

        protected Node(uint start, uint end)
        {
            
        }
    }
}