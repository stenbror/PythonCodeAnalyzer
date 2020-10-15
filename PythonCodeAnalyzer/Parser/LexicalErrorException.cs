using System;

namespace PythonCodeAnalyzer.Parser
{
    public class LexicalErrorException : Exception
    {
        public uint Position { get; set; }
        
        public LexicalErrorException(uint position, string message) : base(message)
        {
            Position = position;
        }
    }
}