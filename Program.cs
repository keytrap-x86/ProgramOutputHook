using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ProgramOutputHook
{
    internal class Program
    {
        private static void Main()
        {
            Console.Write("Enter the program's name that you wish to listen to : ");
            var programToListenTo = Console.ReadLine();

            while(!File.Exists(programToListenTo))
            {
                Console.WriteLine($"Cannot find '{programToListenTo}' in the current directory.");
                Console.Write("Enter the program's name that you wish to listen to : ");
                programToListenTo = Console.ReadLine();
            }

            Console.Write("Enter some arguments (press ENTER for none) : ");
            var programArgument = Console.ReadLine();

            StartProcess(programToListenTo, programArgument);

            Console.WriteLine("Done. [ENTER] Restart [Other] Exit...");
            var key = Console.ReadKey();

            while(key.Key == ConsoleKey.Enter)
            {
                StartProcess(programToListenTo, programArgument);

                Console.WriteLine("Done. [ENTER] Restart [Other] Exit...");
                key = Console.ReadKey();
            }
        }

        private static void StartProcess(string programToListenTo, string programArgument = null)
        {
            var process = new Process();
            var procStartInfo = new ProcessStartInfo(programToListenTo, programArgument)
            {
                UseShellExecute = false, // has to be false for redirecting output
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8
            };

            process.StartInfo = procStartInfo;
            process.EnableRaisingEvents = true;
            process.OutputDataReceived += (s, e) => Console.WriteLine(e.Data);
            process.ErrorDataReceived += (s, e) => Console.WriteLine(e.Data);

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            Console.WriteLine($"Program {programToListenTo} started" +
                              $"{(string.IsNullOrEmpty(programArgument) ? "." : $"with arguments : '{programArgument}'")}");

            process.WaitForExit();
        }
    }
}
