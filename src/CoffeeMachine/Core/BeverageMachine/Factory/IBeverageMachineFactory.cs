namespace CoffeeMachine.Core.BeverageMachine.Factory
{
    public interface IBeverageMachineFactory
    {
        T Create<T>() where T : class, IBeverageMachine;
    }
}