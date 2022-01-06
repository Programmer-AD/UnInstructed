using System;
using System.Linq;

namespace Uninstructed.Game.Player.Commands.Models
{
    public class Command
    {
        public readonly CommandType Type;
        public readonly string[] Args;

        public Command(string command)
        {
            var parts = command.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0 && Enum.TryParse(parts[0], out Type))
            {
                Args = parts.Skip(1).ToArray();
            }
            else
            {
                Args = parts;
                Type = CommandType.Unknown;
            }
        }
    }
}
