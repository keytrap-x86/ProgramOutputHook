using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ProgramOutputHook
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Veuillez entrer le nom du programme à écouter : ");
            var programToListenTo = Console.ReadLine();

            while(!File.Exists(programToListenTo))
            {
                Console.WriteLine($"Impossible de trouver le fichier '{programToListenTo}' dans le répertoire actuel.");
                Console.Write("Veuillez entrer le nom du programme à écouter : ");
                programToListenTo = Console.ReadLine();
            }

            Console.Write("Veuillez entrer un argument à donner au programme (facultatif) : ");
            var programArgument = Console.ReadLine();

            StartProcess(programToListenTo, programArgument);

            Console.WriteLine("Terminé. [ENTREE] Recommencer [Autre] Quitter...");
            var key = Console.ReadKey();

            while(key.Key == ConsoleKey.Enter)
            {
                StartProcess(programToListenTo, programArgument);

                Console.WriteLine("Terminé. [ENTREE] Recommencer [Autre] Quitter...");
                key = Console.ReadKey();
            }
        }

        private static void StartProcess(string programToListenTo, string programArgument = null)
        {
            var process = new Process();
            var procStartInfo = new ProcessStartInfo(programToListenTo, programArgument)
            {
                UseShellExecute = false,
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

            Console.WriteLine($"Programme {programToListenTo} démarré avec l'argument '{programArgument}'");

            process.WaitForExit();
        }
    }
}
