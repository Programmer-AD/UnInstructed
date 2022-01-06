using Uninstructed.Game.Player.Commands.Models;

namespace Uninstructed.Game.Player.Commands.Processing
{
    internal class UnknownCommandProcessor : ICommandProcessor
    {
        public ProcessingResult Process(Command command)
        {
            return ProcessingResult.Error("Unknown command type!");
        }
    }
}
