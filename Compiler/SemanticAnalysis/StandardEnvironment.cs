using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Compiler.Nodes.Interfaces;
using Compiler.Nodes.StandardEnvironmentNodes;
using static Compiler.CodeGeneration.TriangleAbstractMachine.Type;
using static Compiler.CodeGeneration.TriangleAbstractMachine.Primitive;

namespace Compiler.SemanticAnalysis
{
    public static class StandardEnvironment
    {

        public static ImmutableDictionary<string, IDeclarationNode> GetItems()
        {
            return new Dictionary<string, IDeclarationNode>()
            {
                {IntegerType.Name, IntegerType},
                {CharType.Name, CharType},
                {BooleanType.Name, BooleanType},
                {VoidType.Name, VoidType},
                
                {True.Name, BooleanType},
                {False.Name, BooleanType},
                
                {Chr.Name, Chr},
                {Ord.Name, Ord},
                {Eof.Name, Eof},
                {Eol.Name, Eol},
                
                {Get.Name, Get},
                {GetInt.Name, GetInt},
                {Put.Name, Put},
                {PutInt.Name, PutInt},
                {PutEol.Name, PutEol}
            }.ToImmutableDictionary();
        }
        
        // Types
        public static SimpleTypeDeclarationNode IntegerType { get; } = new SimpleTypeDeclarationNode("Integer", 
            INTEGER);
        
        public static SimpleTypeDeclarationNode CharType { get; } = new SimpleTypeDeclarationNode("Char",
            CHARACTER);
        
        public static SimpleTypeDeclarationNode BooleanType { get; } = new SimpleTypeDeclarationNode("Boolean",
            BOOLEAN);
        
        public static SimpleTypeDeclarationNode VoidType { get; } = new SimpleTypeDeclarationNode("Void");
        
        // Constants
        public static BuiltInConstDeclarationNode True { get; } = new BuiltInConstDeclarationNode("true", 
            BooleanType);
        
        public static BuiltInConstDeclarationNode False { get; } = new BuiltInConstDeclarationNode("false", 
            BooleanType);
        
        // Functions
        public static FunctionDeclarationNode Chr { get; } = new FunctionDeclarationNode("chr", ID, 
            CharType, (IntegerType, false));
        
        public static FunctionDeclarationNode Ord { get; } = new FunctionDeclarationNode("ord", ID, 
            IntegerType, (CharType, false));
        
        public static FunctionDeclarationNode Eof { get; } = new FunctionDeclarationNode("eof", EOF,
            BooleanType);
        
        public static FunctionDeclarationNode Eol { get; } = new FunctionDeclarationNode("eol", EOL,
            BooleanType);
        
        // Procedures
        public static FunctionDeclarationNode Get { get; } = new FunctionDeclarationNode("get", GET,
            VoidType, (CharType, true));
        
        public static FunctionDeclarationNode GetInt { get; }  = new FunctionDeclarationNode("getint", GETINT,
            VoidType, (IntegerType, true));
        
        public static FunctionDeclarationNode Put { get; } = new FunctionDeclarationNode("put", PUT,
            VoidType, (CharType, false));
        
        public static FunctionDeclarationNode PutInt { get; } = new FunctionDeclarationNode("putint", PUTINT,
            VoidType, (IntegerType, false));
        
        public static FunctionDeclarationNode PutEol { get; } = new FunctionDeclarationNode("puteol", PUTEOL,
            VoidType);
        
    }
}