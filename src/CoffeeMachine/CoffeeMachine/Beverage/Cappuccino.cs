using CoffeeMachine.Core.BeverageMachine.Beverage;
using CoffeeMachine.Core.BeverageMachine.Beverage.Specification;

namespace CoffeeMachine.CoffeeMachine.Beverage
{
    public class Cappuccino : BeverageBase
    {
        public Cappuccino(BeverageSpecification beverageSpecification) : base(beverageSpecification)
        {
        }
    }
}