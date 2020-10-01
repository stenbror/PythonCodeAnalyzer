namespace PythonCodeAnalyzer.Parser
{
    public interface IPythonTokenizer
    {
        public Token CurSymbol { get; set; }
        public uint Position { get; set; }
        public void Advance();
        public Token[] Symbols { get; set; }
    }
    
    public class PythonTokenizer : IPythonTokenizer
    {
        public Token CurSymbol { get; set; }
        public uint Position { get; set; }
        public Token[] Symbols { get; set; }

        public void Advance()
        {
            
        }
    }
}