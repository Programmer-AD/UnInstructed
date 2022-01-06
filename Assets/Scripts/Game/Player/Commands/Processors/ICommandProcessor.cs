using Uninstructed.Game.Player.Commands.Models;

namespace Uninstructed.Game.Player.Commands.Processors
{
    internal interface ICommandProcessor
    {
        public ProcessingResult Process(Command command);
    }
}
