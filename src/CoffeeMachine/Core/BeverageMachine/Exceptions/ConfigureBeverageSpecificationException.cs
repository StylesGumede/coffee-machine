using System;

namespace CoffeeMachine.Core.BeverageMachine.Exceptions
{
    public class ConfigureBeverageSpecificationException : Exception
    {
        public ConfigureBeverageSpecificationException(string message): base(message)
        {
        }
    }
}