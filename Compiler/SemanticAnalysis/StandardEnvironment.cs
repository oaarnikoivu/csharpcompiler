using System.Collections.Generic;
using System.Collections.Immutable;
using Compiler.Nodes.Interfaces;
using Compiler.Nodes.StandardEnvironmentNodes;
using static Compiler.CodeGeneration.TriangleAbstractMachine.Type;
using static Compiler.CodeGeneration.TriangleAbstractMachine.Primitive;

namespace Compiler.SemanticAnalysis
{
    /// <summary>
    /// Items found in the standard environment
    /// </summary>
    public static class StandardEnvironment
    {

        /// <summary>
        /// Everything in the standard environment that needs adding to the symbol table
        /// </summary>
        /// <returns></returns>
        public static ImmutableDictionary<string, IDeclarationNode> GetItems()
        {
            return new Dictionary<string, IDeclarationNode>()
            {
                {IntegerType.Name, IntegerType},
                {CharType.Name, CharType},
                {BooleanType.Name, BooleanType},
                {AnyType.Name, AnyType},
                {VoidType.Name, VoidType},
                
                {True.Name, BooleanType},
                {False.Name, BooleanType},
                
                {Plus.Name, Plus},
                {Minus.Name, Minus},
                {Multiply.Name, Multiply},
                {Divide.Name, Divide},
                {LessThan.Name, LessThan},
                {GreaterThan.Name, GreaterThan},
                {Equal.Name, Equal},
                {Not.Name, Not},

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
        
        public static SimpleTypeDeclarationNode AnyType { get; } = new SimpleTypeDeclarationNode("Any");
        
        public static SimpleTypeDeclarationNode VoidType { get; } = new SimpleTypeDeclarationNode("Void");
        
        // Operators
        public static BinaryOperationDeclarationNode GreaterThan { get; } = new BinaryOperationDeclarationNode(">", 
            GT, IntegerType, IntegerType, BooleanType);
        
        public static BinaryOperationDeclarationNode LessThan { get; } = new BinaryOperationDeclarationNode("<",
            LT, IntegerType, IntegerType, BooleanType);
        
        public static BinaryOperationDeclarationNode Equal { get; } = new BinaryOperationDeclarationNode("=", 
            EQ,  AnyType, AnyType, BooleanType);
        
        public static BinaryOperationDeclarationNode Plus { get; } = new BinaryOperationDeclarationNode("+", 
            ADD, IntegerType, IntegerType, IntegerType);
        
        public static BinaryOperationDeclarationNode Minus { get; } = new BinaryOperationDeclarationNode("-", 
            SUB, IntegerType, IntegerType, IntegerType);
        
        public static BinaryOperationDeclarationNode Multiply { get; } = new BinaryOperationDeclarationNode("*", 
            MULT, IntegerType, IntegerType, IntegerType);
        
        public static BinaryOperationDeclarationNode Divide { get; } = new BinaryOperationDeclarationNode("/", 
            DIV, IntegerType, IntegerType, IntegerType);
        
        public static UnaryOperationDeclarationNode Not { get; } = new UnaryOperationDeclarationNode("!", 
            NOT, BooleanType, BooleanType);
        
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