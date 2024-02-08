using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Xml.Linq;
using CSScriptLib;
using ParserLibrary;
using TestJsonRazbor;




public interface ICalc
{
    int Sum(int a, int b);
}

public interface ICalc2
{
    int Sum(int a, int b);

    int Div(int a, int b);
}

public class Samples
{
    public static void LoadCode()
    {
        dynamic calc = CSScript.Evaluator
                               .LoadCode(
                                   @"using System;
                                     public class Script : ICalc
                                     {
                                         public int Sum(int a, int b)
                                         {
                                             return a+b;
                                         }
                                     }");
        int result = calc.Sum(1, 2);
    }

    public static void LoadFile()
    {
        var tt=GetExpireFromTrack2("22200*****232=2512");
        var script = Path.GetTempFileName();
        try
        {
/*            File.WriteAllText(script, @"using System;
                                        public class Script : ICalc
                                        {
                                            public int Sum(int a, int b)
                                            {
                                                return a+b;
                                            }
                                            public int Div(int a, int b)
                                            {
                                                return a-b;
                                            }
                                        }");*/
            File.WriteAllText(script, @"using System;
                                        public class Script 
                                        {
                                            public string ConvWithDec(string input)
                                            {
                                                return input.Substring(0,input.Length-2)+"".""+input.Substring(input.Length-2) ;
                                            }
    public string GetExpireFromTrack2(string Track2)
    {
        int pos = Track2.IndexOf(""="");
        string sExpDate = Track2.Substring(pos + 1);
        return new DateTime(2000+Convert.ToInt32(sExpDate.Substring(0,2)), Convert.ToInt32(sExpDate.Substring(2,2)),1).ToString(""o"");
    }
                                        }");



            dynamic calc = CSScript.Evaluator.LoadFile(script);
            var ans=calc.GetExpireFromTrack2("22200*****232=2512");
            int result = calc.Sum(1, 2);
            result= calc.Div(3, 1);
        }
        finally
        {
            File.Delete(script);
        }
    }

    public static string GetExpireFromTrack2(string Track2)
    {
        int pos = Track2.IndexOf("=");
        string sExpDate = Track2.Substring(pos + 1);
        return new DateTime(2000 + Convert.ToInt32(sExpDate.Substring(0, 2)), Convert.ToInt32(sExpDate.Substring(2, 2)), 1).ToString("o");
    }
    public static void LoadAndUnload()
    {
        // Based on https://github.com/dotnet/samples/blob/master/core/tutorials/Unloading/Host/Program.cs
        //
        // Limitations: the "accuracy" of the unloading is determined by the quality of the
        // .NET Core's `AssemblyLoadContext` implementation. For example using variable of `dynamic` type to keep the
        // reference to the script object is problematic. Thus the loaded assembly file stays locked
        // even though the runtime successfully reports unloading the assembly and collecting the weak reference
        // of `AssemblyLoadContext` object.
        //
        // However using interfaces (e.g. `ICalc`) or raw Reflection does not exhibit this problem.
        //
        // Having a debugger attached to the process can also affect the outcome of the unloading.

        WeakReference assemblyHost;

        var asmFile = Path.GetFullPath("Script.dll");

        Samples.LoadAndUnloadImpl(asmFile, out assemblyHost);

        // Poll and run GC until the AssemblyLoadContext is unloaded.
        // You don't need to do that unless you want to know when the context
        // got unloaded. You can just leave it to the regular GC.
        for (int i = 0; assemblyHost.IsAlive && (i < 10); i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        Console.WriteLine($"Unload success: {!assemblyHost.IsAlive}");

        File.Delete(asmFile); // prove that the assembly is unloaded
    }

    public static void LoadAndUnloadImpl(string asmFile, out WeakReference alcWeakRef)
    {
        CSScript.Evaluator
                .ReferenceAssemblyOf<ICalc>()
                .CompileAssemblyFromCode(
                    @"using System;
                      public class Script : ICalc
                      {
                          public int Sum(int a, int b)
                          {
                              return a+b;
                          }
                      }", asmFile);

        var asm = new UnloadableAssembly();

        alcWeakRef = new WeakReference(asm);

/*        ICalc script = (ICalc)asm.LoadFromAssemblyPath(asmFile)
                                 .CreateObject("*"); // or `CreateInstance("css_root+Script")`

        int result = script.Sum(1, 3);
*/
        asm.Unload();
    }

    class UnloadableAssembly : AssemblyLoadContext
    {
        public UnloadableAssembly(string name = null) : base(name ?? Guid.NewGuid().ToString(), isCollectible: true)
            => this.Unloading += x => Console.WriteLine("Unloading " + this.Name);
    }

    public static void LoadMethod()
    {
        dynamic script = CSScript.RoslynEvaluator
                                 .LoadMethod(
                                     @"int Product(int a, int b)
                                       {
                                           return a * b;
                                       }");

        int result = script.Product(3, 2);
    }

    public static void LoadMethodWithInterface()
    {
        ICalc2 script = CSScript.RoslynEvaluator
                                .LoadMethod<ICalc2>(
                                    @"public int Sum(int a, int b)
                                      {
                                          return a + b;
                                      }
                                      public int Div(int a, int b)
                                      {
                                          return a/b;
                                      }");
        int result = script.Sum(15, 3);
    }

    public static void CreateDelegate()
    {
        var log = CSScript.RoslynEvaluator
                          .CreateDelegate(@"
                                            using System;
                                            using ParserLibrary;

                                            void Log(string message)
                                            {
                                                Console.WriteLine(AbstrParser.RetVal());
                                            }");
        log = CSScript.RoslynEvaluator
                          .CreateDelegate(@"
                                            using System;
                                            using ParserLibrary;

                                            int  Log1(int[] a)
                                            {
                                                Console.WriteLine(AbstrParser.RetVal());
                                                return a[0]+a[1]+a[2];
                                            }");
        int[] args = new int[] { 5, 6, 7 };
        var res = log(args);

//        var res =log("Test message");
    }

    public static void LoadCodeWithInterface()
    {
        string code = @"
                using System;
                public class Script : ICalc
                {
                    public int Sum(int a, int b)
                    {
                        return a + b;
                    }
                }";

        var script = CSScript.Evaluator.LoadCode<ICalc>(code);

        int result = script.Sum(13, 2);
    }

    public static void CompileCode()
    {
        var info = new CompileInfo { RootClass = "printer_script", AssemblyFile = "script.dll" };

        var printer_asm = CSScript.Evaluator
                                  .CompileCode(
                                      @"using System;
                                        public class Printer
                                        {
                                            public static void Print() =>
                                                Console.WriteLine(""Printing..."");
                                        }", info);

        printer_asm
            .GetType("printer_script+Printer")
            .GetMethod("Print")
            .Invoke(null, null);
    }

    public static void ScriptReferencingScript()
    {
        var info = new CompileInfo { RootClass = "printer_script" };

        var printer_asm = CSScript.Evaluator
                                  .CompileCode(
                                      @"using System;
                                        public class Printer
                                        {
                                            public void Print() =>
                                                Console.WriteLine(""Printing..."");
                                        }", info);

        dynamic script = CSScript.Evaluator
                                 .ReferenceAssembly(printer_asm)
                                 .LoadMethod(@"using static printer_script;
                                               void Test()
                                               {
                                                   new Printer().Print();
                                               }");
        script.Test();
    }

    public static void LoadDelegate()
    {
        try
        {
/*            var Product = CSScript.Evaluator
                                  .LoadDelegate<Func<int, int, int>>(
                                      @"int Product(int a, int b)
                                            {
                                                return a * b;
                                            }");

            int result = Product(3, 2);*/
        }
        catch (NotImplementedException ex)
        {
            // This method is only implemented for CS-Script .NET but not .NET Core
            // You may want to consider using interfaces with LoadCode/LoadMethod or use CreateDelegate instead.
        }
    }

    public static void Referencing()
    {
        string code = @"
                using System;
                using System.Xml;
                public class Script : ICalc
                {
                    public int Sum(int a, int b)
                    {
                        return a + b;
                    }
                }";

        var script = CSScript.Evaluator
                             .ReferenceAssembliesFromCode(code)
                             .ReferenceAssembly(Assembly.GetExecutingAssembly())
                             .ReferenceAssembly(Assembly.GetExecutingAssembly().Location)
                             .ReferenceAssemblyByName("System")
                             .ReferenceAssemblyByNamespace("System.Xml")
                             .TryReferenceAssemblyByNamespace("Fake.Namespace", out var resolved)
                             .ReferenceAssemblyOf(new Samples())
                             .ReferenceAssemblyOf<XDocument>()
                             .ReferenceDomainAssemblies()
                             .LoadCode<ICalc>(code);

        int result = script.Sum(13, 2);
    }
}