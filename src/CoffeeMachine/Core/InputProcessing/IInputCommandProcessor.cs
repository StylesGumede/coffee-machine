using System.Collections.Generic;

namespace CoffeeMachine.Core.InputProcessing
{
    public interface IInputCommandProcessor
    {
        IEnumerable<Process> Processes { get; }
        void Run(ProcessContext processContext);
        void Register(Process process);
    }
}