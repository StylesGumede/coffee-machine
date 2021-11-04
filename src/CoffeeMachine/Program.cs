using CoffeeMachine.CoffeeMachine;
using CoffeeMachine.CoffeeMachine.Factory;

namespace CoffeeMachine
{
    class Program
    {
        static void Main(string[] args) => new BeverageMachineFactory()
            .Create<HotBeverageMachine>()
            .Run();
    }
}