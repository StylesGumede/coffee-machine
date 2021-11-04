using System;

namespace CoffeeMachine.Core.BeverageMachine.LifeCycle
{
    public class ConsoleApplicationLifeTime : ILifeTime
    {
        public void ShutDown()
        {
            Environment.Exit(0);
        }
    }
}