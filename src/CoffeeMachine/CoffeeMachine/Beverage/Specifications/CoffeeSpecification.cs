using CoffeeMachine.Core.BeverageMachine.Beverage.Specification;

namespace CoffeeMachine.CoffeeMachine.Beverage.Specifications
{
    public class CoffeeSpecification : BeverageSpecification
    {
        public CoffeeSpecification(string displayName) : base(displayName)
        {
        }

        public override T Clone<T>()
        {
            return this.MemberwiseClone() as T;
        }
    }
}