using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Compiler.Tokenization
{
    public enum TokenType
    {
        // non-terminals
        IntLiteral, Identifier, Operator, CharLiteral,
        
        // reserved words - terminals
        If, Then, Else, EndIf, While, Let, In, Begin, End, For, To, Do, Const, Var, Next,
        
        // punctuation - terminals
        Colon, Semicolon, Becomes, Is, LeftBracket, RightBracket, 

        // special tokens
        EndOfText, Error, 
        
        // special error tokens
        UnknownInputError, UnacceptabelSeqError
        
    }

    /// <summary>
    /// Utility functions for working with the tokens
    /// </summary>
    public static class TokenTypes
    {
        /// <summary>
        /// A mapping from keyword to the token type for that keyword
        /// </summary>
        public static ImmutableDictionary<string, TokenType> Keywords { get; } = new Dictionary<string, TokenType>()
        {
            {"if", TokenType.If},
            {"then", TokenType.Then},
            {"else", TokenType.Else},
            {"endif", TokenType.EndIf},
            {"while", TokenType.While},
            {"let", TokenType.Let},
            {"begin", TokenType.Begin},
            {"end", TokenType.End},
            {"for", TokenType.For},
            {"to", TokenType.To},
            {"do", TokenType.Do},
            {"const", TokenType.Const},
            {"var", TokenType.Var},
            {"in", TokenType.In},
            {"next", TokenType.Next}
            
        }.ToImmutableDictionary();

        /// <summary>
        /// Checks whether a word is a keyword
        /// </summary>
        /// <param name="word"></param>
        /// <returns>True if and only if the word is a keyword</returns>
        public static bool IsKeyword(StringBuilder word)
        {
            return Keywords.ContainsKey(word.ToString());
        }

        /// <summary>
        /// Gets the token for a keyword
        /// </summary>
        /// <param name="word"></param>
        /// <returns>The token associated with the given keyword</returns>
        /// <exception cref="ArgumentException"></exception>
        public static TokenType GetTokenForKeyword(StringBuilder word)
        {
            if (!IsKeyword(word)) throw new ArgumentException("Word is not a keyword");
            return Keywords[word.ToString()];
        }
    }
}