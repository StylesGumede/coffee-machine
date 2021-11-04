namespace CoffeeMachine.Core.IngredientUnit
{
    public abstract class Ingredient
    {
        protected Ingredient(int capacity)
        {
            this.Capacity = capacity;
        }

        public int Capacity { get; private set; }

        public bool CanSatisfy(int units)
        {
            return (this.Capacity - units) > 0;
        }

        public void Use(int units)
        {
            this.Capacity -= units;
        }
    }
}