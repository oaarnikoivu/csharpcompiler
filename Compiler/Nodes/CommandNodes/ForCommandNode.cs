using System.Linq.Expressions;
using Compiler.Nodes.Interfaces;
using Compiler.Nodes.TerminalNodes;

namespace Compiler.Nodes.CommandNodes
{
    public class ForCommandNode : ICommandNode
    {
        /// <summary>
        /// The identifier associated with the node
        /// </summary>
        public IdentifierNode Identifier { get; }
        
        /// <summary>
        /// The assignment expression condition associated with the node
        /// </summary>
        public IExpressionNode AssignExpression { get;  }
        
        /// <summary>
        /// The to expression condition associated with the node
        /// </summary>
        public IExpressionNode ToExpression { get; }
        
        /// <summary>
        /// The command associated with the node 
        /// </summary>
        public ICommandNode Command { get; }
        
        /// <summary>
        /// The position in the code where the content associated with the node begins
        /// </summary>
        public Position Position { get; }

        /// <summary>
        /// Creates a new for node
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="assignExpression"></param>
        /// <param name="toExpression"></param>
        /// <param name="command"></param>
        public ForCommandNode(IdentifierNode identifier, IExpressionNode assignExpression, IExpressionNode toExpression,
            ICommandNode command)
        {
            Identifier = identifier;
            AssignExpression = assignExpression;
            ToExpression = toExpression;
            Command = command;
        }
    }
}