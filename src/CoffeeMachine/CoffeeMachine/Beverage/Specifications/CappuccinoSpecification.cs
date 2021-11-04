using CoffeeMachine.Core.BeverageMachine.Beverage.Specification;

namespace CoffeeMachine.CoffeeMachine.Beverage.Specifications
{
    public class CappuccinoSpecification : BeverageSpecification
    {
        public CappuccinoSpecification(string displayName) : base(displayName)
        {
        }

        public override T Clone<T>() => this.MemberwiseClone() as T;
    }
}