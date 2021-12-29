using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Uninstructed.Game.Player.IO
{
    public class PlayerProgram
    {
        private readonly Process process;

        public bool Working { get; private set; }

        public PlayerProgram()
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
                }
            };
            process.StandardInput.AutoFlush = true;
            process.Exited += Exited;
        }

        public void SetStartInfo(string fileName, string args)
        {
            if (!Working)
            {
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = args;
            }
        }

        public void Start()
        {
            if (!Working)
            {
                try
                {
                    process.Start();
                    Working = true;
                }
                catch (Exception) { }
            }
        }

        public void Stop()
        {
            if (Working)
            {
                process.Close();
                Working = false;
            }
        }

        public async Task<string> ReadLineAsync()
        {
            string result = await process.StandardOutput.ReadLineAsync();
            ReadInput?.Invoke(result);
            return result;
        }

        public async Task WriteLineAsync(string data)
        {
            await process.StandardInput.WriteLineAsync(data);
            WriteOutput?.Invoke(data);
        }

        public event Action<string> ReadInput;
        public event Action<string> WriteOutput;

        private void Exited(object sender, EventArgs e)
        {
            Working = false;
        }
    }
}
