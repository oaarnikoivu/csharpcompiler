using Compiler.Nodes.Interfaces;
using Compiler.Nodes.StandardEnvironmentNodes;
using Compiler.Nodes.TerminalNodes;

namespace Compiler.Nodes.ExpressionNodes
{
    public class CallExpressionNode : IExpressionNode
    {
        public IdentifierNode Identifier { get; }
        
        public IParameterNode Parameter { get; }
        
        public Position Position { get; }
        
        public SimpleTypeDeclarationNode Type { get; set; }

        public CallExpressionNode(IdentifierNode identifier, IParameterNode parameter)
        {
            Identifier = identifier;
            Parameter = parameter;
        }
    }
}