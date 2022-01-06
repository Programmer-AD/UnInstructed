using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Uninstructed.Game.Main;
using Uninstructed.Game.Player.Commands;
using Uninstructed.Game.Player.Commands.Models;
using Uninstructed.Game.Player.IO;

namespace Uninstructed.Game.Player
{
    public class PlayerController
    {
        public Entity Player { get; private set; }

        private PlayerProgram program;
        private readonly ConcurrentQueue<Command> commandQueue;
        private readonly PrimaryCommandProcessor commandProcessor;

        private CancellationTokenSource tokenSource;
        private Task prereadTask, executeTask;
        private volatile bool working;
        public bool Working => working;

        public PlayerController(Entity player)
        {
            Player = player;

            commandProcessor = new (player, OnWorkStart, OnWorkEnd);
            commandQueue = new ();

            working = false;
        }

        public bool TryStart(string fileName, string arguments)
        {
            if (working) return false;
            try
            {
                program = new(fileName, arguments);
                program.Start();

                commandQueue.Clear();
                working = true;
                tokenSource = new();

                prereadTask = Task.Run(PrereadTaskMain, tokenSource.Token);
                executeTask = Task.Run(ExecuteTaskMain, tokenSource.Token);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public event Action ProgramStopped;
        public void Stop()
        {
            if (working)
            {
                working = false;

                tokenSource.Cancel();
                tokenSource.Dispose();
                tokenSource = null;

                prereadTask.Dispose();
                prereadTask = null;

                executeTask.Dispose();
                executeTask = null;

                program.Stop();
                program = null;

                ProgramStopped?.Invoke();
            }
        }

        public event Action WorkStart;
        private void OnWorkStart()
        {
            WorkStart?.Invoke();
        }

        private void OnWorkEnd()
        {
            Stop();
        }

        private async Task PrereadTaskMain()
        {
            while (working && program.Working)
            {
                var input = await program.ReadLineAsync();
                if (input != null)
                {
                    var command = new Command(input);
                    commandQueue.Enqueue(command);
                }
            }
            Stop();
        }

        private async Task ExecuteTaskMain()
        {
            while (working && program.Working)
            {
                if (!Player.Busy)
                {
                    if (commandQueue.TryDequeue(out var command))
                    {
                        var result = commandProcessor.Process(command);
                        await WaitUntilPlayerBusy();
                        await PrintResult(result);
                    }
                }
            }
            Stop();
        }

        private async Task WaitUntilPlayerBusy()
        {
            while (Player.Busy)
            {
                await Task.Yield();
            }
        }

        private async Task PrintResult(ProcessingResult result)
        {
            var status = result.Status.ToString().ToLower();
            await program.WriteLineAsync(status);

            if (!string.IsNullOrEmpty(result.Output))
            {
                await program.WriteLineAsync(result.Output);
            }
        }
    }
}
