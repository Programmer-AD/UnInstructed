using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uninstructed.Game.Player.Commands.Models;
using Uninstructed.Game.Player.IO;

namespace Uninstructed.Game.Player.Commands
{
    internal class CommandQueue
    {
        private readonly PlayerProgram playerProgram;
        private readonly Queue<Command> commands;

        public CommandQueue(PlayerProgram playerProgram)
        {
            this.playerProgram = playerProgram;
            commands = new();
        }

        public async Task ReadCommand()
        {
            var input = await playerProgram.ReadLineAsync();
            var command = new Command(input);
            commands.Enqueue(command);
        }

        public Command DequeueCommand()
        {
            if (commands.Count > 0)
            {
                return commands.Dequeue();
            }
            return null;
        }
    }
}
