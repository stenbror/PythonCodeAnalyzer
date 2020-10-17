namespace PythonCodeAnalyzer.Parser
{
    public class Token
    {
        public enum TokenKind
        {
            Unused,
            PyLess,
            PyLessEqual,
            PyEqual,
            PyGreaterEqual,
            PyGreater,
            PyNotEqual,
            PyIn,
            PyNot,
            PyIs,
            PyNone,
            PyPlus,
            PyMinus,
            PyColonAssign,
            PyIf,
            PyElse,
            PyLambda, 
            PyColon,
            PyOr,
            PyXor,
            PyAnd,
            PyMul,
            PyShiftLeft,
            PyShiftRight,
            PyModulo,
            PyMatrice,
            PyDiv,
            PyFloorDiv,
            PyInvert,
            PyPower,
            PyAwait,
            Name,
            Number,
            String,
            PyElipsis,
            PyTrue,
            PyFalse,
            PyLeftParen,
            PyRightParen,
            PyLeftBracket,
            PyRightBracket,
            PyLeftCurly,
            PyRightCurly,
            PyDot,
            PyTestAnd,
            PyTestOr,
            PyComma,
            PySemiColon,
            Newline,
            PyAsync,
            PyFor,
            PyAssign,
            PyPlusAssign,
            PyMinusAssign,
            PyMulAssign,
            PyPowerAssign,
            PyDivAssign,
            PyFloorDivAssign,
            PyModuloAssign,
            PyMatriceAssign,
            PyAndAssign,
            PyOrAssign,
            PyXorAssign,
            PyShiftLeftAssign,
            PyShiftRightAssign,
            PyYield,
            PyFrom,
            PyWhile,
            PyTry,
            PyWith,
            PyClass,
            PyDef,
            PyDel,
            PyPass,
            PyContinue,
            PyBreak,
            PyReturn,
            PyRaise,
            PyImport,
            PyGlobal,
            PyNonlocal,
            PyAssert,
            TypeComment,
            PyElif,
            PyExcept,
            PyFinally,
            PyAs,
            Indent,
            Dedent,
            PyArrow,
            EOF

        }  
        
        public TokenKind Kind { get; set; }
        public uint Start { get; set; }
        public uint End { get; set; }
        public string Text { get; set; }

        public Token(uint start, uint end, TokenKind kind)
        {
            Start = start;
            End = end;
            Kind = kind;
        }
    }
}