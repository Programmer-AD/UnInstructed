using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Uninstructed.Game.Player.IO
{
    internal class PlayerProgram
    {
        private readonly ConcurrentQueue<string> queue;
        private readonly Process process;

        private bool started = false, stopped = false;

        private bool working = false;
        public bool Working => working && !process.HasExited;
        public bool HasData => queue.Count > 0;

        public PlayerProgram(string command, string args)
        {
            queue = new ConcurrentQueue<string>();
            process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    ErrorDialog = true,

                    FileName = command,
                    Arguments = args
                },
                EnableRaisingEvents = true
            };
            process.OutputDataReceived += OnInput;
        }

        public void Start()
        {
            if (!started)
            {
                process.Start();
                process.BeginOutputReadLine();
                working = true;
                started = true;
            }
        }

        public void Stop()
        {
            if (!stopped)
            {
                stopped = true;
                working = false;
                process.CancelOutputRead();
                process.Kill();
                process.WaitForExit();
                process.Dispose();
            }
        }

        public string ReadLine()
        {
            if (queue.TryDequeue(out var result))
            {
                return result;
            }
            return null;
        }

        public void WriteLine(string data)
        {
            if (Working)
            {
                process.StandardInput.WriteLine(data);
                process.StandardInput.Flush();
            }
        }

        private void OnInput(object _, DataReceivedEventArgs args)
        {
            queue.Enqueue(args.Data);
        }
    }
}
