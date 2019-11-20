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
        public VarDeclarationNode VarDeclaration { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public AssignCommandNode AssignCommand { get; }
        
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
        /// <param name="assignCommand"></param>
        /// <param name="toExpression"></param>
        /// <param name="command"></param>
        /// <param name="position"></param>
        /// <param name="becomesExpression"></param>
        public ForCommandNode(VarDeclarationNode varDeclaration, AssignCommandNode assignCommand, IExpressionNode toExpression, ICommandNode command, Position position)
        {
            VarDeclaration = varDeclaration;
            AssignCommand = assignCommand;
            ToExpression = toExpression;
            Command = command;
            Position = position;
        }
    }
}