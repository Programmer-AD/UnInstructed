using System;
using Uninstructed.Game.Player.Commands.Models;
using Uninstructed.Game.Player.Config;

namespace Uninstructed.Game.Player.Commands.Processing
{
    internal class ConfigCommandProcessor : ICommandProcessor
    {
        private readonly ConfigContainer container;

        public ConfigCommandProcessor(ConfigContainer container)
        {
            this.container = container;
        }

        public ProcessingResult Process(Command command)
        {
            if (command.Args.Length != 2)
            {
                return ProcessingResult.Error("Wrong argument count for config");
            }
            if (Enum.TryParse(command.Args[0], out ConfigKey key))
            {
                if (byte.TryParse(command.Args[1], out var value))
                {
                    container[key] = value != 0;
                    return ProcessingResult.Ok();
                }
                return ProcessingResult.Error("Wrong value for config");
            }
            return ProcessingResult.Error("Wrong config key");
        }
    }
}
