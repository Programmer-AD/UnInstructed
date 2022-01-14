using System;
using System.Collections;
using Uninstructed.Game.Main;
using Uninstructed.Game.Player.Commands;
using Uninstructed.Game.Player.Commands.Models;
using Uninstructed.Game.Player.IO;
using UnityEngine;

namespace Uninstructed.Game.Player
{
    public class PlayerController
    {
        public Entity Player { get; private set; }
        public string OuterResult;

        private PlayerProgram program;
        private readonly PrimaryCommandProcessor commandProcessor;

        private Coroutine executingRoutine;

        private bool inited;
        public bool Inited => inited;

        private bool working;
        public bool Working => working;

        private bool started;
        public bool Started => started;

        public PlayerController(Entity player)
        {
            Player = player;

            commandProcessor = new(player, OnWorkInit, OnWorkStart, OnWorkStop);

            working = false;
            started = false;
            inited = false;
        }

        public bool TryStart(string command, string arguments)
        {
            if (working)
            {
                return false;
            }
            try
            {
                working = false;
                started = false;
                inited = false;

                program = new(command, arguments);
                program.Start();

                working = true;

                executingRoutine = Player.StartCoroutine(ExecuteTaskMain());

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
            inited = false;

            if (executingRoutine != null)
            {
                Player.StopCoroutine(executingRoutine);
                executingRoutine = null;
            }

            if (program != null)
            {
                ProgramStopped?.Invoke();
                program.Stop();
            }

            Player.Stop();
        }

        private void OnWorkInit()
        {
            inited = true;
        }

        public event Action WorkStart;
        private void OnWorkStart()
        {
            inited = true;
            started = true;
            WorkStart?.Invoke();
        }

        private void OnWorkStop()
        {
            working = false;
        }

        private IEnumerator ExecuteTaskMain()
        {
            while (working && (program.Working || program.HasData))
            {
                var input = program.ReadLine();
                if (input != null)
                {
                    OuterResult = null;
                    var command = new Command(input);
                    if (!Player.Busy && (
                        inited && (started || command.Type != CommandType.Player)
                        || command.Type == CommandType.Work))
                    {
                        var result = commandProcessor.Process(command);
                        yield return Player.StartCoroutine(WaitUntilPlayerBusy());
                        if (result.Status == ProcessingStatus.Ok
                            && string.IsNullOrEmpty(result.Output)
                            && !string.IsNullOrEmpty(OuterResult))
                        {
                            result = ProcessingResult.Ok(OuterResult);
                        }
                        PrintResult(result);
                    }
                }
                else
                {
                    yield return null;
                }
            }
            Stop();
        }

        private IEnumerator WaitUntilPlayerBusy()
        {
            while (Player.Busy && working)
            {
                yield return null;
            }
        }

        private void PrintResult(ProcessingResult result)
        {
            var status = result.Status.ToString().ToLower();
            program.WriteLine(status);

            if (!string.IsNullOrEmpty(result.Output))
            {
                program.WriteLine(result.Output);
            }
        }
    }
}
