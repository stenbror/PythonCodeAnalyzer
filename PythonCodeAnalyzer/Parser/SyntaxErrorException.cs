using System;

namespace PythonCodeAnalyzer.Parser
{
    public class SyntaxErrorException : Exception
    {
        public Token ErrorSymbol { get; set; }
        public uint Position { get; set; }
        
        public SyntaxErrorException(uint pos, Token op, string message) : base(message)
        {
            ErrorSymbol = op;
            Position = pos;
        }
    }
}