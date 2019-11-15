using System.Collections.Generic;
using System.Text;
using Compiler.IO;

namespace Compiler.Tokenization
{
    public class Tokenizer
    {
        /// <summary>
        /// The error reporter
        /// </summary>
        public ErrorReporter Reporter { get; }

        /// <summary>
        /// The reader getting the characters from the file
        /// </summary>
        private IFileReader Reader { get; }

        /// <summary>
        /// The characters currently in the token
        /// </summary>
        private StringBuilder TokenSpelling { get; } = new StringBuilder();

        /// <summary>
        /// Creates a new tokenizer
        /// </summary>
        /// <param name="reporter"></param>
        /// <param name="reader"></param>
        public Tokenizer(IFileReader reader, ErrorReporter reporter)
        {
            Reader = reader;
            Reporter = reporter;
        }

        /// <summary>
        /// Gets all the tokens from the file
        /// </summary>
        /// <returns></returns>
        public List<Token> GetAllTokens()
        {
            List<Token> tokens = new List<Token>();
            Token token = GetNextToken();
            while (token.Type != TokenType.EndOfText)
            {
                tokens.Add(token);
                token = GetNextToken();
            }

            tokens.Add(token);
            Reader.Close();
            return tokens;
        }

        /// <summary>
        /// Scan the next token
        /// </summary>
        /// <returns></returns>
        private Token GetNextToken()
        {
            // skip separators
            SkipSeparators();

            Position tokenStartPosition = Reader.CurrentPosition;
            TokenType tokenType = ScanToken();

            Token token = new Token(tokenType, TokenSpelling.ToString(), tokenStartPosition);
            Debugger.Write($"Scanned {token}");

            if (tokenType == TokenType.Error)
            {
                Reporter.HasErrors = true;
            }

            return token;
        }

        /// <summary>
        /// Skip forward until the next character is not whitespace or a comment
        /// </summary>
        private void SkipSeparators()
        {
            while (Reader.Current == '#' || IsWhiteSpace(Reader.Current))
            {
                if (Reader.Current == '#')
                    Reader.SkipRestOfLine();
                else
                    Reader.MoveNext();
            }
        }

        /// <summary>
        /// Find the next token                              
        /// </summary>
        /// <returns></returns>
        private TokenType ScanToken()
        {

            if (IsOperator(Reader.Current))
            {
                TakeIt();
                return TokenType.Operator;
            }

            return TokenType.Test;
        }

        /// <summary>
        /// Appends the current character to the current token then moves to the next character
        /// </summary>
        private void TakeIt()
        {
            TokenSpelling.Append(Reader.Current);
            Reader.MoveNext();
        }

        /// <summary>
        /// Checks whether a character is white space
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static bool IsWhiteSpace(char c)
        {
            return c == ' ' || c == '\t' || c == '\n';
        }

        /// <summary>
        /// Checks whether a character is an operator
        /// </summary>
        /// <param name="c">The character to check</param>
        /// <returns>True if and only if the character is an operator in the language</returns>
        private static bool IsOperator(char c)
        {
            switch (c)
            {
                case '+':
                case '-':
                case '*':
                case '/':
                case '<':
                case '>':
                case '=':
                case '\\':
                    return true;
                default:
                    return false;
            }
        }
    }
}