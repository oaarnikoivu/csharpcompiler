namespace Compiler.Tokenization
{
    public enum TokenType
    {
        // non-terminals
        IntLiteral, Identifier, Operator, CharLiteral,

        // special symbols - terminals
        Underscore, Dot, QuestionMark, Quote, Zero,
        
        // special tokens
        EndOfText, Error
    }
}