using System;
using Compiler.Nodes.Interfaces;
using Compiler.CodeGeneration;

namespace Compiler.Nodes.StandardEnvironmentNodes
{
    /// <summary>
    /// A node corresponding to a type declaration
    /// </summary>
    public class SimpleTypeDeclarationNode : ITypeDeclarationNode
    {
        /// <summary>
        /// The name of the type
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The position in the code where the content associated with the node begins
        /// </summary>
        public Position Position { get { return Position.BuiltIn; } }

        /// <summary>
        /// The size of the type on the target machine
        /// </summary>
        public byte Size { get { return (TargetType != null && TriangleAbstractMachine.TypeSize.TryGetValue(TargetType.Value, out byte size)) ? size : (byte)0; } }

        /// <summary>
        /// The type on the target machine that this type should be represented as
        /// </summary>
        public TriangleAbstractMachine.Type? TargetType { get; }

        /// <summary>
        /// Creates a new type declaration node
        /// </summary>
        /// <param name="name">The name of the type</param>
        /// <param name="targetType">The type on the target machine that this type should be represented as</param>
        public SimpleTypeDeclarationNode(string name, TriangleAbstractMachine.Type? targetType = null)
        {
            Name = name;
            TargetType = targetType;
        }

        /// <inheritDoc />
        public override string ToString()
        {
            return Name;
        }
    }
}