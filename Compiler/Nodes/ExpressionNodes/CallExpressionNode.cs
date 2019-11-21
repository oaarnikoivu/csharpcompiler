using Compiler.Nodes.Interfaces;
using Compiler.Nodes.StandardEnvironmentNodes;
using Compiler.Nodes.TerminalNodes;

namespace Compiler.Nodes.ExpressionNodes
{
    /// <summary>
    /// 
    /// </summary>
    public class CallExpressionNode : IExpressionNode
    {
        /// <summary>
        /// 
        /// </summary>
        public IdentifierNode Identifier { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public IParameterNode Parameter { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public Position Position { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public SimpleTypeDeclarationNode Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="parameter"></param>
        public CallExpressionNode(IdentifierNode identifier, IParameterNode parameter)
        {
            Identifier = identifier;
            Parameter = parameter;
        }
    }
}