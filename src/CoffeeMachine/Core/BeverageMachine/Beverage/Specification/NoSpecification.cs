namespace CoffeeMachine.Core.BeverageMachine.Beverage.Specification
{
    public class NoSpecification : BeverageSpecification
    {
        public NoSpecification() : base("Cancel, I would not like coffee for now")
        {
        }
        
        public override T Clone<T>() => this.MemberwiseClone() as T;
    }
}