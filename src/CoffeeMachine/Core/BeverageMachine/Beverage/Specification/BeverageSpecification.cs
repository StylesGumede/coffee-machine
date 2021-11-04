namespace CoffeeMachine.Core.BeverageMachine.Beverage.Specification
{
    public abstract class BeverageSpecification
    {
        protected BeverageSpecification(string displayName)
        {
            DisplayName = displayName;
        }

        public string DisplayName { get; }
        public int MilkUnitsRequired { get; set; }
        public int BeanUnitsRequired { get; set; }
        public int SugarUnitsRequired { get; set; }

        public bool Is<T>() => this.GetType() == typeof(T);

        public T To<T>() where T : BeverageSpecification => this as T;

        public abstract T Clone<T>() where T: BeverageSpecification;
    }
}