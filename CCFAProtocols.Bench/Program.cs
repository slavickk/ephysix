using BenchmarkDotNet.Running;

namespace CCFAProtocols.Bench;

class Program
{
    static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
}