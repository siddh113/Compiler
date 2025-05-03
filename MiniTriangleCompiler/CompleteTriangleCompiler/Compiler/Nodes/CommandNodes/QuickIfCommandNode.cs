namespace Compiler.Nodes
{
    /// <summary>
    /// A node corresponding to a quick if command ( ? <expression> => <command> )
    /// </summary>
    public class QuickIfCommandNode : ICommandNode
    {
        /// <summary>
        /// The condition to be evaluated
        /// </summary>
        public IExpressionNode Expression { get; }

        /// <summary>
        /// The command executed if the condition is true
        /// </summary>
        public ICommandNode Command { get; }

        /// <summary>
        /// The position in the code where the content associated with the node begins
        /// </summary>
        public Position Position { get; }

        /// <summary>
        /// Creates a new quick if command node
        /// </summary>
        /// <param name="expression">The condition to be evaluated</param>
        /// <param name="command">The command to execute if the condition is true</param>
        /// <param name="position">The position in the code where the content associated with the node begins</param>
        public QuickIfCommandNode(IExpressionNode expression, ICommandNode command)
        {
            Expression = expression;
            Command = command;
        }
    }
}
