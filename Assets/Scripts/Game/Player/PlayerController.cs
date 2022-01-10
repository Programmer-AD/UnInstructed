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

        private volatile bool started;
        public bool Started => started;

        public PlayerController(Entity player)
        {
            Player = player;

            commandProcessor = new(player, OnWorkStart, OnWorkStop);
            commandQueue = new();

            working = false;
            started = false;
        }

        public bool TryStart(string command, string arguments)
        {
            if (working)
            {
                return false;
            }

            try
            {
                program = new(command, arguments);
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
            working = false;
            started = false;

            tokenSource?.Cancel();

            program?.Stop();
            prereadTask = null;
            executeTask = null;
            program = null;

            tokenSource?.Dispose();
            tokenSource = null;

            ProgramStopped?.Invoke();
        }

        public event Action WorkStart;
        private void OnWorkStart()
        {
            started = true;
            WorkStart?.Invoke();
        }

        private void OnWorkStop()
        {
            working = false;
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
        }

        private async Task ExecuteTaskMain()
        {
            while (working && (program.Working || commandQueue.Count > 0))
            {
                try
                {
                    if (!Player.Busy && commandQueue.TryDequeue(out var command) && (started || command.Type != CommandType.Player))
                    {
                        var result = commandProcessor.Process(command);
                        await WaitUntilPlayerBusy();
                        await PrintResult(result);
                    }
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogException(ex);
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
