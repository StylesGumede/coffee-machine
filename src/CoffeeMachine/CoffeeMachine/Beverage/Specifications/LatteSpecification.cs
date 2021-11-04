using CoffeeMachine.Core.BeverageMachine.Beverage.Specification;

namespace CoffeeMachine.CoffeeMachine.Beverage.Specifications
{
    public class LatteSpecification : BeverageSpecification
    {
        public LatteSpecification(string displayName) : base(displayName)
        {
        }
        
        public override T Clone<T>() => this.MemberwiseClone() as T;
    }
}