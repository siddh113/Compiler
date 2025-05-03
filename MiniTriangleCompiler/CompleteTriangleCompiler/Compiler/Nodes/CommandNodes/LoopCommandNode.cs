namespace Compiler.Nodes
{
    /// <summary>
    /// A node corresponding to a loop command
    /// </summary>
    public class LoopCommandNode : ICommandNode
    {
        /// <summary>
        /// The initial command executed before the loop condition is checked
        /// </summary>
        public ICommandNode InitialCommand { get; }

        /// <summary>
        /// The condition associated with the loop
        /// </summary>
        public IExpressionNode Expression { get; }

        /// <summary>
        /// The command executed while the condition holds true
        /// </summary>
        public ICommandNode LoopCommand { get; }

        /// <summary>
        /// The position in the code where the content associated with the node begins
        /// </summary>
        public Position Position { get; }

        /// <summary>
        /// Creates a new loop node
        /// </summary>
        /// <param name="initialCommand">The initial command executed before checking the condition</param>
        /// <param name="expression">The condition associated with the loop</param>
        /// <param name="loopCommand">The command executed while the condition holds true</param>
        /// <param name="position">The position in the code where the content associated with the node begins</param>
        public LoopCommandNode(ICommandNode initialCommand, IExpressionNode expression, ICommandNode loopCommand, Position position)
        {
            InitialCommand = initialCommand;
            Expression = expression;
            LoopCommand = loopCommand;
            Position = position;
        }
    }
}
