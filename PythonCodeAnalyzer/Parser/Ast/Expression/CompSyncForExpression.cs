﻿namespace PythonCodeAnalyzer.Parser.Ast.Expression
{
    public partial class CompSyncForExpression : ExpressionNode
    {
        public Token Operator1 { get; set; }
        public ExpressionNode Left { get; set; }
        public Token Operator2 { get; set; }
        public ExpressionNode Right { get; set; }
        public ExpressionNode Next { get; set; }
        
        public CompSyncForExpression(uint start, uint end, Token op1, ExpressionNode left, Token op2, ExpressionNode right, ExpressionNode next) : base(start, end)
        {
            Operator1 = op1;
            Left = left;
            Operator2 = op2;
            Right = right;
            Next = next;
        }
    }
}