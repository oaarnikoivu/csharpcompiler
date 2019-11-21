using Compiler.Nodes.DeclarationNodes;
using Compiler.Nodes.Interfaces;

namespace Compiler.Nodes.CommandNodes
{
    public class ForCommandNode : ICommandNode
    {
        /// <summary>
        /// The variable declaration associated with the node
        /// </summary>
        public VarDeclarationNode VarDeclaration { get; }
        
        /// <summary>
        /// The assignment command associated with the node
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
        /// <param name="varDeclaration"></param>
        /// <param name="assignCommand"></param>
        /// <param name="toExpression"></param>
        /// <param name="command"></param>
        /// <param name="position"></param>
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