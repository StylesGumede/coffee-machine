namespace CoffeeMachine.Core.Presentation.Console
{
    public class ConsoleWrapper : IConsoleWrapper
    {
        public void WriteLine(string value) => System.Console.WriteLine(value);
        public string ReadLine() => System.Console.ReadLine();
    }
}