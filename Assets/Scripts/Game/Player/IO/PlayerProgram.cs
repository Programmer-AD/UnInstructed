using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Uninstructed.Game.Player.IO
{
    internal class PlayerProgram
    {
        private readonly Process process;

        public bool Working { get; private set; }

        public PlayerProgram(string command, string args)
        {
            Working = false;

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

            process.Exited += ExitedHandler;
        }

        public void Start()
        {
            if (!Working)
            {
                process.Start();
                Working = true;
            }
        }

        public void Stop()
        {
            if (Working)
            {
                process.Dispose();
                Working = false;
            }
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

        private void ExitedHandler(object sender, EventArgs e)
        {
            Working = false;
        }
    }
}
