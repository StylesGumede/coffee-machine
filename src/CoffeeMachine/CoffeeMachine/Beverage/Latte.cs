using CoffeeMachine.Core.BeverageMachine.Beverage;
using CoffeeMachine.Core.BeverageMachine.Beverage.Specification;

namespace CoffeeMachine.CoffeeMachine.Beverage
{
    public class Latte : BeverageBase
    {
        public Latte(BeverageSpecification beverageSpecification) : base(beverageSpecification)
        {
        }
    }
}