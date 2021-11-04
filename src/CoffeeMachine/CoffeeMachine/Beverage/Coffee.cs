using CoffeeMachine.Core.BeverageMachine.Beverage;
using CoffeeMachine.Core.BeverageMachine.Beverage.Specification;

namespace CoffeeMachine.CoffeeMachine.Beverage
{
    public class Coffee : BeverageBase
    {
        public Coffee(BeverageSpecification beverageSpecification) : base(beverageSpecification)
        {
        }
    }
    

}