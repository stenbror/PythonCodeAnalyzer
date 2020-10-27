# PythonCodeAnalyzer

This will be a little library written in C# for walking Python 3.9 abstract syntax tree produced by a parser. This will be a "Roslyn" like tool for analyzing your Python code and possible refactor it by using it in your own C# programs.

It is in early stage. It will be TDD and contains unittests for the part implemented.

It is designed to be very light weight and easy to use for Syntax transformation and check of your Python 3.9 code. It might end up as front end for a new dotnet based Python interpreter / compiler. It is currently based on a RC of .Net 5.0

Expression part of parser including tokenizer is now implemented and covered by UnitTests. That should make it possibly to use the framework for parsing expression part of Python from source code to final binary tree of nodes. AST.

Next, is implementing all UnitTests for Statement rules that does not including block controll. That is Indent / Dedent handling.

Simple use for now is:


using PythonCodeAnalyzer;
using PythonCodeAnalyzer.Parser;
using PythonCodeAnalyzer.Parser.Ast;
using PythonCodeAnalyzer.Parser.Ast.Expression;

var parser = new PythonParser();
parser.Tokenizer = new PythonTokenizer("a + b * c / d; ".ToCharArray(), false, 8);

parser.Tokenizer.Advance(); // Advance first token needed when not starting at top level rule in parser.

var node = parser.ParseNamedExpr();

node contain root node for ASTTree generated.

Just remember, this framework is not ready for any purpose until the whole parser is tested and implemented with correct Tokenizer handling block of code.
