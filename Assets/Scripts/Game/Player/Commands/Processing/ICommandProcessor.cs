using Uninstructed.Game.Player.Commands.Models;

namespace Uninstructed.Game.Player.Commands.Processing
{
    internal interface ICommandProcessor
    {
        public ProcessingResult Process(Command command);
    }
}
