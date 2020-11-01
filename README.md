# PythonCodeAnalyzer

This will be a little library written in C# for walking Python 3.9 abstract syntax tree produced by a parser. This will be a "Roslyn" like tool for analyzing your Python code and possible refactor it by using it in your own C# programs.

It is in early stage. It will be TDD and contains unittests for the part implemented.

It is designed to be very light weight and easy to use for Syntax transformation and check of your Python 3.9 code. It might end up as front end for a new dotnet based Python interpreter / compiler. It is currently based on a RC of .Net 5.0

UnitTests are implemented and passed for Expression rules. For statement nodes we have most UnitTests for happy path inplace. Missing Typecomment, Block controll and Typed and Var ArgsList. Missing implementing in Tokenizer stops testing parser for this. Should be ready for testing for most rule from starting rules.
