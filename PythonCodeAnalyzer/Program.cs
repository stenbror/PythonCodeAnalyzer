using System;
using System.IO.Compression;
using PythonCodeAnalyzer.Parser.Ast.Expression;

namespace PythonCodeAnalyzer
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var test = new PythonCodeAnalyzer.Parser.Ast.Expression.RelationExpression(0, 5,
                RelationExpression.Relation.Equal, null, null, null);

            Console.WriteLine("{0}", test.GetType());
            
        }
    }
}
