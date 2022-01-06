using System.Collections.Generic;
using Uninstructed.Game.Main;
using Uninstructed.Game.Player.Commands;
using Uninstructed.Game.Player.Commands.Models;
using Uninstructed.Game.Player.Commands.Processing;
using Uninstructed.Game.Player.Config;
using Uninstructed.Game.Player.IO;

namespace Uninstructed.Game.Player
{
    public class PlayerController
    {
        public Entity PlayerEntity { get; private set; }

        private readonly Dictionary<CommandType, ICommandProcessor> commandProcessor;
        private PlayerProgram program;
        private CommandQueue commandQueue;
        private ConfigContainer config;

        public PlayerController(Entity playerEntity)
        {
            PlayerEntity = playerEntity;
            commandProcessor = MakeCommandProcessor();
        }


        private Dictionary<CommandType, ICommandProcessor> MakeCommandProcessor()
        {
            var result = new Dictionary<CommandType, ICommandProcessor>
            {
                [CommandType.Unknown] = new UnknownCommandProcessor(),
                [CommandType.Config] = new ConfigCommandProcessor(config),
                [CommandType.Get] = new GetCommandProcessor(PlayerEntity),
                [CommandType.Player] = new PlayerCommandProcessor(PlayerEntity),
                [CommandType.Work] = new WorkCommandProcessor(OnWorkStart, OnWorkEnd),
            };

            return result;
        }

        private void OnWorkStart()
        {
            //TODO
        }
        private void OnWorkEnd()
        {
            //TODO
        }
    }
}
