using ClassApiExecutor;

namespace ConsoleFormStreamDefinition
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] fields =new string[] { "Kind", "LifePhase", "IsReversal", "UndoState", "CustCard", "Result" };

            TranParserHelper.getDefine(fields);
            Console.WriteLine("Hello, World!");
        }
    }
}