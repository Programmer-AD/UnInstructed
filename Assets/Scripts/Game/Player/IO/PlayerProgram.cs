using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Uninstructed.Game.Player.IO
{
    internal class PlayerProgram
    {
        private readonly Process process;

        private bool working =false;
        public bool Working => working && !process.HasExited;

        public PlayerProgram(string command, string args)
        {
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
            };
        }

        public void Start()
        {
            if (!working)
            {
                process.Start();
                working = true;
            }
        }

        public void Stop()
        {
            working = false;
            process.Dispose();
        }

        public async Task<string> ReadLineAsync()
        {
            var result = await process.StandardOutput.ReadLineAsync();
            return result;
        }

        public async Task WriteLineAsync(string data)
        {
            if (Working)
            {
                await process.StandardInput.WriteLineAsync(data);
                await process.StandardInput.FlushAsync();
            }
        }
    }
}
