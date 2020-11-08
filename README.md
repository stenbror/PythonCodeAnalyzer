# PythonCodeAnalyzer

This will be a little library written in C# for walking Python 3.9 abstract syntax tree produced by a parser. This will be a "Roslyn" like tool for analyzing your Python code and possible refactor it by using it in your own C# programs.

We now have a mostly complete parser with UnitTests for all Happy flow rules and most Expression rules with also failure tests. This includes block controls with Indent and Dedent handling. Trivia is not collected yet and non type comments will fail for now.
