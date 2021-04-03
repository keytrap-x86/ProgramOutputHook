## ProgramOutputHook

Demonstrates how one can easily hook to a program's output and display it in a console (or GUI app)

The most interresting par :

```csharp
private static void StartProcess(string programToListenTo, string programArgument = null) {
	var process = new Process();
	var procStartInfo = new ProcessStartInfo(programToListenTo, programArgument) {
		UseShellExecute = false,
		// has to be false for redirecting output
		RedirectStandardOutput = true,
		RedirectStandardError = true,
		StandardOutputEncoding = Encoding.UTF8
	};

	process.StartInfo = procStartInfo;
	process.EnableRaisingEvents = true;
	process.OutputDataReceived += (s, e) = >Console.WriteLine(e.Data);
	process.ErrorDataReceived += (s, e) = >Console.WriteLine(e.Data);

	process.Start();

	process.BeginOutputReadLine();
	process.BeginErrorReadLine();

	Console.WriteLine($ "Program {programToListenTo} started{(string.IsNullOrEmpty(programArgument) ? "." : $"
	with arguments: '{programArgument}'")}");

	process.WaitForExit();
}
```