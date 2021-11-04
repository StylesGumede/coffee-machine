namespace CoffeeMachine.Core.Presentation.Console
{
    public interface IConsoleWrapper
    {
        void WriteLine(string value);
        string ReadLine();
    }
}