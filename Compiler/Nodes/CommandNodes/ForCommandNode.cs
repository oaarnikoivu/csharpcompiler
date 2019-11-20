using System.Linq.Expressions;
using Compiler.Nodes.DeclarationNodes;
using Compiler.Nodes.Interfaces;
using Compiler.Nodes.TerminalNodes;

namespace Compiler.Nodes.CommandNodes
{
    public class ForCommandNode : ICommandNode
    {
        /// <summary>
        /// 
        /// </summary>
        public IDeclarationNode VarDeclaration { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public IExpressionNode BecomesExpression { get; }
        
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
        /// <param name="toExpression"></param>
        /// <param name="command"></param>
        /// <param name="position"></param>
        public ForCommandNode(IDeclarationNode varDeclaration, IExpressionNode becomesExpression, 
            IExpressionNode toExpression, ICommandNode command, Position position)
        {
            VarDeclaration = varDeclaration;
            BecomesExpression = becomesExpression;
            ToExpression = toExpression;
            Command = command;
            Position = position;
        }
    }
}