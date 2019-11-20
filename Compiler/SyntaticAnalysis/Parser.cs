using System;
using System.Collections.Generic;
using Compiler.CodeGeneration;
using Compiler.IO;
using Compiler.Tokenization;
using Compiler.Nodes;
using Compiler.Nodes.CommandNodes;
using Compiler.Nodes.DeclarationNodes;
using Compiler.Nodes.ExpressionNodes;
using Compiler.Nodes.Interfaces;
using Compiler.Nodes.ParameterNodes;
using Compiler.Nodes.TerminalNodes;
using Compiler.Nodes.TypeDenoterNodes;
using Debugger = Compiler.IO.Debugger;

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
            else
            {
                Reporter.ReportError($"Incorrect use of token type: {CurrentToken.Type} " +
                                     $"at position: {CurrentToken.Position}");
            }
        }
        
        /// <summary>
        /// Parses a program
        /// </summary>
        /// <param name="tokens">The tokens to parse</param>
        public ProgramNode Parse(List<Token> tokens)
        {
            this.tokens = tokens;
            ProgramNode program = ParseProgram();
            return program;
        }

        /// <summary>
        /// Parses a program
        /// </summary>
        private ProgramNode ParseProgram()
        {
            Debugger.Write($"Parsing program");
            ICommandNode command = ParseCommand();
            ProgramNode program = new ProgramNode(command);
            return program;
        }

        /// <summary>
        /// Parses a command
        /// </summary>
        private ICommandNode ParseCommand()
        {
            Debugger.Write("Parsing command");
            List<ICommandNode> commands = new List<ICommandNode>();
            commands.Add(ParseSingleCommand());
            while (CurrentToken.Type == TokenType.Semicolon)
            {
                Accept(TokenType.Semicolon);
                commands.Add(ParseSingleCommand());
            }

            if (commands.Count == 1)
                return commands[0];
            else 
                return new SequentialCommandNode(commands);
        }

        /// <summary>
        /// Parses a single command
        /// </summary>
        private ICommandNode ParseSingleCommand()
        {
            Debugger.Write("Parsing single command");
            switch (CurrentToken.Type)
            {
                case TokenType.Identifier:
                    return ParseAssignmentOrCallCommand();
                case TokenType.If:
                    return ParseIfCommand();
                case TokenType.While:
                    return ParseWhileCommand();
                case TokenType.Let:
                    return ParseLetCommand();
                case TokenType.Begin:
                    return ParseBeginCommand();
                case TokenType.For:
                    return ParseForCommand();
                default:
                    return ParseSkipCommand();
            }
        }

        /// <summary>
        /// Parses a skip command
        /// </summary>
        /// <returns>An abstract syntax tree representing the skip command</returns>
        private ICommandNode ParseSkipCommand()
        {
            Debugger.Write("Parsing Skip Command");
            Position startPosition = CurrentToken.Position;
            return new BlankCommandNode(startPosition);
        }

        /// <summary>
        /// Parses an assignment or call command
        /// </summary>
        private ICommandNode ParseAssignmentOrCallCommand()
        {
            Debugger.Write("Parsing Assignment Command or Call Command");
            Position startPosition = CurrentToken.Position;
            IdentifierNode identifier = ParseIdentifier();
            if (CurrentToken.Type == TokenType.LeftBracket)
            {
                Debugger.Write("Parsing Call Command");
                Accept(TokenType.LeftBracket);
                IParameterNode parameter = ParseParameter();
                Accept(TokenType.RightBracket);
                return new CallCommandNode(identifier, parameter);
            }
            else if (CurrentToken.Type == TokenType.Becomes)
            {
                Debugger.Write("Parsing Assignment Command");
                Accept(TokenType.Becomes);
                IExpressionNode expression = ParseExpression();
                return new AssignCommandNode(identifier, expression);
            }
            else
            {
                return new ErrorNode(startPosition);
            }
        }
        
        /// <summary>
        /// Parses an identifier
        /// </summary>
        private IdentifierNode ParseIdentifier()
        {
            Debugger.Write("Parsing Identifier");
            Token identifierToken = CurrentToken;
            Accept(TokenType.Identifier);
            return new IdentifierNode(identifierToken);
        }

        /// <summary>
        /// Parses an if command
        /// </summary>
        private ICommandNode ParseIfCommand()
        {
            Debugger.Write("Parsing If Command");
            Position startPosition = CurrentToken.Position;
            Accept(TokenType.If);
            IExpressionNode expression = ParseBracketExpression();
            Accept(TokenType.Then);
            ICommandNode thenCommand = ParseSingleCommand();
            if (CurrentToken.Type == TokenType.Else)
            {
                Accept(TokenType.Else);
                ICommandNode elseCommand = ParseSingleCommand();
                Accept(TokenType.EndIf);
                return new IfElseCommandNode(expression, thenCommand, elseCommand, startPosition);
            }
            Accept(TokenType.EndIf);
            return new IfCommandNode(expression, thenCommand, startPosition);
        }

        /// <summary>
        /// Parses a while command
        /// </summary>
        private WhileCommandNode ParseWhileCommand()
        {
            Debugger.Write("Parsing While Command");
            Position startPosition = CurrentToken.Position;
            Accept(TokenType.While);
            IExpressionNode expression = ParseBracketExpression();
            Accept(TokenType.Do);
            ICommandNode command = ParseSingleCommand();
            return new WhileCommandNode(expression, command, startPosition);
        }

        /// <summary>
        /// Parses a let command
        /// </summary>
        private ICommandNode ParseLetCommand()
        {
            Debugger.Write("Parsing Let Command");
            Position startPosition = CurrentToken.Position;
            Accept(TokenType.Let);
            IDeclarationNode declaration = ParseDeclaration();
            Accept(TokenType.In);
            ICommandNode command = ParseSingleCommand();
            return new LetCommandNode(declaration, command, startPosition);
        }

        /// <summary>
        /// Parses a begin command
        /// </summary>
        private ICommandNode ParseBeginCommand()
        {
            Debugger.Write("Parsing Begin Command");
            Accept(TokenType.Begin);
            ICommandNode command = ParseCommand();
            Accept(TokenType.End);
            return command;
        }

        /// <summary>
        /// Parses a for command
        /// </summary>
        private ICommandNode ParseForCommand()
        {
            Debugger.Write("Parsing For Command");
            Position startPosition = CurrentToken.Position;
            IDeclarationNode declaration = ParseVarDeclaration();
            Accept(TokenType.Becomes);
            IExpressionNode becomesExpression = ParseExpression();
            Accept(TokenType.To);
            IExpressionNode toExpression = ParseExpression();
            Accept(TokenType.Do);
            ICommandNode command = ParseSingleCommand();
            Accept(TokenType.Next);
            return new ForCommandNode(declaration, becomesExpression, toExpression, command, startPosition);
        }

        /// <summary>
        /// Parses a declaration
        /// </summary>
        private IDeclarationNode ParseDeclaration()
        {
            Debugger.Write("Parsing Declaration");
            List<IDeclarationNode> declarations = new List<IDeclarationNode>();
            declarations.Add(ParseSingleDeclaration());
            while (CurrentToken.Type == TokenType.Semicolon)
            {
                Accept(TokenType.Semicolon);
                declarations.Add(ParseSingleDeclaration());
            }

            if (declarations.Count == 1)
                return declarations[0];
            else 
                return new SequentialDeclarationNode(declarations);
        }

        /// <summary>
        /// Parses a single declaration
        /// </summary>
        private IDeclarationNode ParseSingleDeclaration()
        {
            Debugger.Write("Parsing Single Declaration");
            switch (CurrentToken.Type)
            {
                case TokenType.Const:
                    return ParseConstDeclaration();
                case TokenType.Var:
                    return ParseVarDeclaration();
                default:
                    Reporter.ReportError($"Could not parse single declaration at: {CurrentToken.Position}");
                    return new ErrorNode(CurrentToken.Position);
                    
            }
        }

        /// <summary>
        /// Parses a const declaration
        /// </summary>
        private IDeclarationNode ParseConstDeclaration()
        {
            Debugger.Write("Parsing Const Declaration");
            Position startPosition = CurrentToken.Position;
            Accept(TokenType.Const);
            IdentifierNode identifier = ParseIdentifier();
            Accept(TokenType.Is);
            IExpressionNode expression = ParseExpression();
            return new ConstDeclarationNode(identifier, expression, startPosition);
        }

        /// <summary>
        /// Parses a var declaration
        /// </summary>
        private IDeclarationNode ParseVarDeclaration()
        {
            Debugger.Write("Parsing Var Declaration");
            Position startPosition = CurrentToken.Position;
            if (CurrentToken.Type == TokenType.Var)
            {
                Accept(TokenType.Var);
                IdentifierNode identifier = ParseIdentifier();
                Accept(TokenType.Colon);
                TypeDenoterNode typeDenoter = ParseTypeDenoter();
                return new VarDeclarationNode(identifier, typeDenoter, startPosition);
            }
            else if (CurrentToken.Type == TokenType.For) // Declare I as a new variable of type integer with undefined value
            {
                Accept(TokenType.For);
                IdentifierNode identifier = ParseIdentifier();
                return new VarDeclarationNode(identifier, 
                    new TypeDenoterNode(
                        new IdentifierNode(
                            new Token(TokenType.Identifier, "Integer", startPosition))), startPosition);
            }
            else
            {
                return new ErrorNode(startPosition);
            }

        }

        /// <summary>
        /// Parses a type denoter
        /// </summary>
        private TypeDenoterNode ParseTypeDenoter()
        {
            Debugger.Write("Parsing Type Denoter");
            IdentifierNode identifier = ParseIdentifier();
            return new TypeDenoterNode(identifier);
        }

        /// <summary>
        /// Parses a parameter
        /// </summary>
        private IParameterNode ParseParameter()
        {
            Debugger.Write("Parsing Parameter");
            switch (CurrentToken.Type)
            {
                case TokenType.RightBracket:
                    return new BlankParameterNode(CurrentToken.Position);
                case TokenType.Var:
                    return ParseVarParameter();
                default:
                    return ParseValParameter();
            }
        }

        /// <summary>
        /// Parses a variable parameter
        /// </summary>
        private IParameterNode ParseVarParameter()
        {
            Debugger.Write("Parsing Variable Parameter");
            Position startPosition = CurrentToken.Position;
            Accept(TokenType.Var);
            IdentifierNode identifier = ParseIdentifier();
            return new VarParameterNode(identifier, startPosition);
        }

        /// <summary>
        /// Parses a value parameter
        /// </summary>
        private IParameterNode ParseValParameter()
        {
            Debugger.Write("Parsing Value Parameter");
            IExpressionNode expression = ParseExpression();
            return new ExpressionParameterNode(expression);
        }

        /// <summary>
        /// Parses an expression
        /// </summary>
        private IExpressionNode ParseExpression()
        {
            Debugger.Write("Parsing Expression");
            IExpressionNode leftExpression = ParsePrimaryExpression();
            while (CurrentToken.Type == TokenType.Operator)
            {
                OperatorNode operation = ParseOperator();
                IExpressionNode rightExpression = ParsePrimaryExpression();
                leftExpression = new BinaryExpressionNode(leftExpression, operation, rightExpression);
            }

            return leftExpression;
        }

        /// <summary>
        /// Parses a primary expression
        /// </summary>
        private IExpressionNode ParsePrimaryExpression()
        {
            Debugger.Write("Parsing Primary Expression");
            switch (CurrentToken.Type)
            {
                case TokenType.IntLiteral:
                    return ParseIntExpression();
                case TokenType.CharLiteral:
                    return ParseCharExpression();
                case TokenType.Identifier:
                    return ParseIdExpression();
                case TokenType.Operator:
                    return ParseUnaryExpression();
                case TokenType.LeftBracket:
                    return ParseBracketExpression();
                default:
                    Reporter.ReportError($"Could not parse primary expression at: {CurrentToken.Position}");
                    return new ErrorNode(CurrentToken.Position);
            }
        }

        /// <summary>
        /// Parses an integer expression
        /// </summary>
        private IExpressionNode ParseIntExpression()
        {
            Debugger.Write("Parsing Int Expression");
            IntegerLiteralNode intLit = ParseIntegerLiteral();
            return new IntegerExpressionNode(intLit);
        }

        /// <summary>
        /// Parses a character expression
        /// </summary>
        private IExpressionNode ParseCharExpression()
        {
            Debugger.Write("Parsing Char Expression");
            CharacterLiteralNode charLit = ParseCharacterLiteral();
            return new CharacterExpressionNode(charLit);
        }

        /// <summary>
        /// Parses ID Expression
        /// </summary>
        private IExpressionNode ParseIdExpression()
        {
            Debugger.Write("Parsing Call Expression or Identifier Expression");
            Position startPosition = CurrentToken.Position;
            IdentifierNode identifier = ParseIdentifier();
            if (CurrentToken.Type == TokenType.LeftBracket)
            {
                // Parse call expression
                return ParseCallExpression(identifier);

            }
            else
            {
                Debugger.Write("Parsing Identifier Expression");
                return new IdExpressionNode(identifier);
            }
        }

        /// <summary>
        /// Parses a call expression
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        private IExpressionNode ParseCallExpression(IdentifierNode identifier)
        {
            Debugger.Write("Parsing Call Expression");
            Accept(TokenType.LeftBracket);
            IParameterNode parameter = ParseParameter();
            Accept(TokenType.RightBracket);
            return new CallExpressionNode(identifier, parameter);
        }
        
        /// <summary>
        /// Parses a unary expresion
        /// </summary>
        private IExpressionNode ParseUnaryExpression()
        {
            Debugger.Write("Parsing Unary Expression");
            OperatorNode operation = ParseOperator();
            IExpressionNode expression = ParsePrimaryExpression();
            return new UnaryExpressionNode(operation, expression);
        }
        
        /// <summary>
        /// Parses a bracket expression
        /// </summary>
        private IExpressionNode ParseBracketExpression()
        {
            Debugger.Write("Parsing Bracket Expression");
            Accept(TokenType.LeftBracket);
            IExpressionNode expression = ParseExpression();
            Accept(TokenType.RightBracket);
            return expression;
        }

        /// <summary>
        /// Parses an integer literal
        /// </summary>
        private IntegerLiteralNode ParseIntegerLiteral()
        {
            Debugger.Write("Parsing integer literal");
            Token integerLiteralToken = CurrentToken;
            Accept(TokenType.IntLiteral);
            return new IntegerLiteralNode(integerLiteralToken);
        }
        
        /// <summary>
        /// Parses a character literal
        /// </summary>
        private CharacterLiteralNode ParseCharacterLiteral()
        {
            Debugger.Write("Parsing character literal");
            Token characterLiteralToken = CurrentToken;
            Accept(TokenType.CharLiteral);
            return new CharacterLiteralNode(characterLiteralToken);
        }

        /// <summary>
        /// Parses an operator
        /// </summary>
        private OperatorNode ParseOperator()
        {
            Debugger.Write("Parsing operator");
            Token OperatorToken = CurrentToken;
            Accept(TokenType.Operator);
            return new OperatorNode(OperatorToken);
        }
    }
}