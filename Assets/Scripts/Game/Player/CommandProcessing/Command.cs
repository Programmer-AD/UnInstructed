using System;
using System.Linq;

namespace Uninstructed.Game.Player
{
    class Command
    {
        public readonly CommandType Type;
        public readonly string[] Args;

        public Command(string command)
        {
            var parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
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
