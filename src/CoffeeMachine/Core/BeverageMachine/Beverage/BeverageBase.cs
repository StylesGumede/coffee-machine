using CoffeeMachine.Core.BeverageMachine.Beverage.Specification;

namespace CoffeeMachine.Core.BeverageMachine.Beverage
{
    public abstract class BeverageBase: IIdentifiable
    {
        protected BeverageBase(BeverageSpecification beverageSpecification)
        {
            this.BeverageSpecification = beverageSpecification;
        }
        
        public string Name => this.GetType().Name;
        public BeverageSpecification BeverageSpecification { get; }
    }
}