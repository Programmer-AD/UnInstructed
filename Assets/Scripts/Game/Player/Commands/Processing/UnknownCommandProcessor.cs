using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game.Player.Commands.Models;

namespace Uninstructed.Game.Player.Commands.Processing
{
    internal class UnknownCommandProcessor : ICommandProcessor
    {
        public ProcessingResult Process(Command command)
            => ProcessingResult.Error("Unknown command type!");
    }
}
