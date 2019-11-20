using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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

            // remember the starting position of the token
            Position tokenStartPosition = Reader.CurrentPosition;
            
            // scan the token and work out its type
            TokenType tokenType = ScanToken();

            // create the token
            Token token = new Token(tokenType, TokenSpelling.ToString(), tokenStartPosition);
            Debugger.Write($"Scanned {token}");

            // report error if necessary
            HandleTokenErrors(tokenType, tokenStartPosition, token);

            return token;
        }

        private void HandleTokenErrors(TokenType tokenType, Position tokenStartPosition, Token token)
        {
            switch (tokenType)
            {
                case TokenType.UnknownInputError:
                    Reporter.ReportError($"Unknown input character: {token.Spelling} " +
                                         $"at position: {tokenStartPosition}");
                    break;
                case TokenType.UnacceptableSeqError:
                    Reporter.ReportError($"Unacceptable sequences of input characters at: {tokenStartPosition}");
                    break;
            }
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
            TokenSpelling.Clear();
            if (char.IsLetter(Reader.Current) || Reader.Current == '_')
            {
                // Reading an identifier
                TakeIt();
                while (Reader.Current == '_' || char.IsLetter(Reader.Current)) TakeIt();
                while (char.IsLetterOrDigit(Reader.Current)) TakeIt();

                if (TokenTypes.IsKeyword(TokenSpelling)) 
                    return TokenTypes.GetTokenForKeyword(TokenSpelling);
                else
                    return TokenType.Identifier;
            }
            else if (char.IsDigit(Reader.Current))
            {
                // Reading an integer
                if (Reader.Current == '0')
                    TakeIt();
                else if (Reader.Current > '0') // non zero digit
                    while (char.IsDigit(Reader.Current)) // digit
                        TakeIt();
                return TokenType.IntLiteral;
            }
            else if (IsOperator(Reader.Current))
            {
                // Read an operator
                TakeIt();
                return TokenType.Operator;
            }
            else if (Reader.Current == ':')
            {
                // Read an :
                // Is it a : or a :=
                TakeIt();
                if (Reader.Current == '=')
                {
                    TakeIt();
                    return TokenType.Becomes;
                }
                else
                {
                    return TokenType.Colon;
                }
            }
            else if (Reader.Current == ';')
            {
                TakeIt();
                return TokenType.Semicolon;
            }
            else if (Reader.Current == '~')
            {
                TakeIt();
                return TokenType.Is;
            }
            else if (Reader.Current == '(')
            {
                TakeIt();
                return TokenType.LeftBracket;
            }
            else if (Reader.Current == ')')
            {
                TakeIt();
                return TokenType.RightBracket;
            }
            else if (Reader.Current == '\'')
            {
                // Read a '
                TakeIt();

                // Scan <graphic>
                if (char.IsLetter(Reader.Current))
                    TakeIt();
                else if (char.IsDigit(Reader.Current))
                    TakeIt();
                else if (Reader.Current == '.')
                    TakeIt();
                else if (Reader.Current == '?')
                    TakeIt();
                else if (char.IsWhiteSpace(Reader.Current))
                    TakeIt();

                // Try getting the closing ' 
                if (Reader.Current == '\'')
                {
                    TakeIt();
                    return TokenType.CharLiteral;
                }
                else
                {
                    return TokenType.UnacceptableSeqError;
                }
            }
            else if (IsOperator(Reader.Current))
            {
                TakeIt();
                return TokenType.Operator;
            }
            else if (Reader.Current == default(char))
            {
                TakeIt();
                return TokenType.EndOfText;
            }
            else
            {
                TakeIt();
                return TokenType.UnknownInputError;
            }
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
                case '!':
                    return true;
                default:
                    return false;
            }
        }
    }
}