using System.Collections.Generic;
using Compiler.IO;
using Compiler.Tokenization;

namespace Compiler.SyntaticAnalysis
{
    /// <summary>
    /// A recursive descent parser
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// The error reporter
        /// </summary>
        public ErrorReporter Reporter { get; }

        /// <summary>
        /// The tokens to be parsed
        /// </summary>
        private List<Token> tokens;

        /// <summary>
        /// The index of the current token in tokens
        /// </summary>
        private int currentIndex;
        
        /// <summary>
        /// The current token
        /// </summary>
        private Token CurrentToken => tokens[currentIndex];

        /// <summary>
        /// Advances the current token to the next one to be parsed
        /// </summary>
        private void MoveNext()
        {
            if (currentIndex < tokens.Count - 1)
                currentIndex += 1;
        }

        /// <summary>
        /// Creates a new parser 
        /// </summary>
        /// <param name="reporter"></param>
        public Parser(ErrorReporter reporter)
        {
            Reporter = reporter;
        }

        /// <summary>
        /// Checks the current token is the expected kind and moves to the next token
        /// </summary>
        /// <param name="expectedType"></param>
        private void Accept(TokenType expectedType)
        {
            if (CurrentToken.Type == expectedType)
            {
                Debugger.Write($"Accepted {CurrentToken}");
                MoveNext();
            }
        }
        
        /// <summary>
        /// Parses a program
        /// </summary>
        /// <param name="tokens">The tokens to parse</param>
        public void Parse(List<Token> tokens)
        {
            this.tokens = tokens;
            ParseProgram();
        }

        /// <summary>
        /// Parses a program
        /// </summary>
        private void ParseProgram()
        {
            Debugger.Write($"Parsing program");
            ParseCommand();
        }

        /// <summary>
        /// Parses a command
        /// </summary>
        private void ParseCommand()
        {
            Debugger.Write($"Parsing command");
            ParseSingleCommand();
            while (CurrentToken.Type == TokenType.Semicolon)
            {
                Accept(TokenType.Semicolon);
                ParseSingleCommand();
            }
        }

        /// <summary>
        /// Parses a single command
        /// </summary>
        private void ParseSingleCommand()
        {
            Debugger.Write("Parsing single command");
            switch (CurrentToken.Type)
            {
                case TokenType.Identifier:
                    ParseAssignmentOrCallCommand();
                    break;
                case TokenType.If:
                    ParseIfCommand();
                    break;
                case TokenType.While:
                    ParseWhileCommand();
                    break;
                case TokenType.Let:
                    ParseLetCommand();
                    break;
                case TokenType.Begin:
                    ParseBeginCommand();
                    break;
                case TokenType.For:
                    ParseForCommand();
                    break;
            }
        }

        /// <summary>
        /// Parses an assignment or call command
        /// </summary>
        private void ParseAssignmentOrCallCommand()
        {
            Debugger.Write("Parsing Assignment Command or Call Command");
            ParseIdentifier();
            if (CurrentToken.Type == TokenType.LeftBracket)
            {
                Debugger.Write("Parsing Call Command");
                Accept(TokenType.LeftBracket);
                ParseParameter();
                Accept(TokenType.RightBracket);
            }
            else if (CurrentToken.Type == TokenType.Becomes)
            {
                Debugger.Write("Parsing Assignment Command");
                Accept(TokenType.Becomes);
                ParseExpression();
            }
        }

        /// <summary>
        /// Parses an identifier
        /// </summary>
        private void ParseIdentifier()
        {
            Debugger.Write("Parsing Identifier");
            Accept(TokenType.Identifier);
        }

        /// <summary>
        /// Parses an if command
        /// </summary>
        private void ParseIfCommand()
        {
            Debugger.Write("Parsing If Command");
            Accept(TokenType.If);
            Accept(TokenType.LeftBracket);
            ParseExpression();
            Accept(TokenType.RightBracket);
            Accept(TokenType.Then);
            ParseSingleCommand();
            Accept(TokenType.Else);
            ParseSingleCommand();
            Accept(TokenType.EndIf);
        }

        /// <summary>
        /// Parses a while command
        /// </summary>
        private void ParseWhileCommand()
        {
            Debugger.Write("Parsing While Command");
            Accept(TokenType.While);
            Accept(TokenType.LeftBracket);
            ParseExpression();
            Accept(TokenType.RightBracket);
            Accept(TokenType.Do);
            ParseSingleCommand();
        }

        /// <summary>
        /// Parses a let command
        /// </summary>
        private void ParseLetCommand()
        {
            Debugger.Write("Parsing Let Command");
            Accept(TokenType.Let);
            ParseDeclaration();
            Accept(TokenType.In);
            ParseSingleCommand();
        }

        /// <summary>
        /// Parses a begin command
        /// </summary>
        private void ParseBeginCommand()
        {
            Debugger.Write("Parsing Begin Command");
            Accept(TokenType.Begin);
            ParseCommand();
            Accept(TokenType.End);
        }

        /// <summary>
        /// Parses a for command
        /// </summary>
        private void ParseForCommand()
        {
            Debugger.Write("Parsing For Command");
            Accept(TokenType.For);
            ParseIdentifier();
            Accept(TokenType.Becomes);
            ParseExpression();
            Accept(TokenType.To);
            ParseExpression();
            Accept(TokenType.Do);
            ParseSingleCommand();
            Accept(TokenType.Next);
        }

        /// <summary>
        /// Parses a declaration
        /// </summary>
        private void ParseDeclaration()
        {
            Debugger.Write("Parsing Declaration");
            ParseSingleDeclaration();
            while (CurrentToken.Type == TokenType.Semicolon)
            {
                Accept(TokenType.Semicolon);
                ParseSingleDeclaration();
            }
        }

        /// <summary>
        /// Parses a single declaration
        /// </summary>
        private void ParseSingleDeclaration()
        {
            Debugger.Write("Parsing Single Declaration");
            switch (CurrentToken.Type)
            {
                case TokenType.Const:
                    ParseConstDeclaration();
                    break;
                case TokenType.Var:
                    ParseVarDeclaration();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Parses a const declaration
        /// </summary>
        private void ParseConstDeclaration()
        {
            Debugger.Write("Parsing Const Declaration");
            Accept(TokenType.Const);
            ParseIdentifier();
            Accept(TokenType.Is);
            ParseExpression();
        }

        /// <summary>
        /// Parses a var declaration
        /// </summary>
        private void ParseVarDeclaration()
        {
            Debugger.Write("Parsing Var Declaration");
            Accept(TokenType.Var);
            ParseIdentifier();
            Accept(TokenType.Colon);
            ParseTypeDenoter();
        }

        /// <summary>
        /// Parses a type denoter
        /// </summary>
        private void ParseTypeDenoter()
        {
            Debugger.Write("Parsing Type Denoter");
            ParseIdentifier();
        }

        /// <summary>
        /// Parses a parameter
        /// </summary>
        private void ParseParameter()
        {
            Debugger.Write("Parsing Parameter");
            switch (CurrentToken.Type)
            {
                case TokenType.RightBracket:
                    // Empty parameter list
                    break;
                case TokenType.Var:
                    ParseVarParameter();
                    break;
                default:
                    ParseValParameter();
                    break;
            }
        }

        /// <summary>
        /// Parses a variable parameter
        /// </summary>
        private void ParseVarParameter()
        {
            Debugger.Write("Parsing Variable Parameter");
            Accept(TokenType.Var);
            ParseIdentifier();
        }

        /// <summary>
        /// Parses a value parameter
        /// </summary>
        private void ParseValParameter()
        {
            Debugger.Write("Parsing Value Parameter");
            ParseExpression();
        }

        /// <summary>
        /// Parses an expression
        /// </summary>
        private void ParseExpression()
        {
            Debugger.Write("Parsing Expression");
            ParsePrimaryExpression();
            while (CurrentToken.Type == TokenType.Operator)
            {
                ParseOperator();
                ParsePrimaryExpression();
            }
        }

        /// <summary>
        /// Parses a primary expression
        /// </summary>
        private void ParsePrimaryExpression()
        {
            Debugger.Write("Parsing Primary Expression");
            switch (CurrentToken.Type)
            {
                case TokenType.IntLiteral:
                    ParseIntExpression();
                    break;
                case TokenType.CharLiteral:
                    ParseCharExpression();
                    break;
                case TokenType.Identifier:
                    ParseIdExpression();
                    break;
                case TokenType.Operator:
                    ParseUnaryExpression();
                    break;
                case TokenType.LeftBracket:
                    ParseBracketExpression();
                    break;
            }
        }

        /// <summary>
        /// Parses an integer expression
        /// </summary>
        private void ParseIntExpression()
        {
            Debugger.Write("Parsing Int Expression");
            ParseIntegerLiteral();
        }

        /// <summary>
        /// Parses a character expression
        /// </summary>
        private void ParseCharExpression()
        {
            Debugger.Write("Parsing Char Expression");
            ParseCharacterLiteral();
        }

        /// <summary>
        /// Parses ID Expression
        /// </summary>
        private void ParseIdExpression()
        {
            Debugger.Write("Parsing Call Expression or Identifier Expression");
            ParseIdentifier();
        }

        /// <summary>
        /// Parses a unary expresion
        /// </summary>
        private void ParseUnaryExpression()
        {
            Debugger.Write("Parsing Unary Expression");
            ParseOperator();
            ParsePrimaryExpression();
        }
        
        /// <summary>
        /// Parses a bracket expression
        /// </summary>
        private void ParseBracketExpression()
        {
            Debugger.Write("Parsing Bracket Expression");
            Accept(TokenType.LeftBracket);
            ParseExpression();
            Accept(TokenType.RightBracket);
        }

        /// <summary>
        /// Parses an integer literal
        /// </summary>
        private void ParseIntegerLiteral()
        {
            Debugger.Write("Parsing integer literal");
            Accept(TokenType.IntLiteral);
        }
        
        /// <summary>
        /// Parses a character literal
        /// </summary>
        private void ParseCharacterLiteral()
        {
            Debugger.Write("Parsing character literal");
            Accept(TokenType.CharLiteral);
        }

        /// <summary>
        /// Parses an operator
        /// </summary>
        private void ParseOperator()
        {
            Debugger.Write("Parsing operator");
            Accept(TokenType.Operator);
        }
    }
}