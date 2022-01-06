using System;
using System.Collections.Generic;
using System.Linq;

namespace Uninstructed.Game.Player.Commands.Models
{
    public class ProcessingResult
    {
        public readonly ProcessingStatus Status;
        public readonly string Output;

        protected ProcessingResult(ProcessingStatus status, string output = null)
        {
            Status = status;
            Output = output;
        }


        public static ProcessingResult Error(string description)
        {
            return new(ProcessingStatus.Error, description);
        }

        public static ProcessingResult Ok(string output = null)
        {
            return new(ProcessingStatus.Ok, output);
        }

        public static ProcessingResult Dead()
        {
            return new(ProcessingStatus.Dead);
        }
    }
}
