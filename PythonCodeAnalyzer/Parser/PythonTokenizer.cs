namespace PythonCodeAnalyzer.Parser
{
    public interface IPythonTokenizer
    {
        public Token CurSymbol { get; set; }
        public uint Position { get; set; }
        public void Advance();
    }
    
    public class PythonTokenizer : IPythonTokenizer
    {
        public Token CurSymbol { get; set; }
        public uint Position { get; set; }

        public void Advance()
        {
            
        }
    }
}