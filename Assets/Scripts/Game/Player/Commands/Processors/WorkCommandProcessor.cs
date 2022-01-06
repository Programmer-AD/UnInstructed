using System;
using Uninstructed.Game.Player.Commands.Models;

namespace Uninstructed.Game.Player.Commands.Processors
{
    internal class WorkCommandProcessor : ICommandProcessor
    {
        private readonly Action startCallback;
        private readonly Action stopCallback;

        public WorkCommandProcessor(Action startCallback, Action stopCallback)
        {
            this.startCallback = startCallback;
            this.stopCallback = stopCallback;
        }

        public ProcessingResult Process(Command command)
        {
            if (command.Args.Length == 0)
            {
                return ProcessingResult.Error("No command at call work");
            }
            switch (command.Args[0])
            {
                case "start":
                    startCallback();
                    return ProcessingResult.Ok();
                case "stop":
                    stopCallback();
                    return ProcessingResult.Ok();
            }
            return ProcessingResult.Error("Wrong command at call work");
        }
    }
}
