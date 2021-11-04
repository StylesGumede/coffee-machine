using System.Collections.Generic;

namespace CoffeeMachine.Core.IngredientUnit
{
    public interface IIngredientUnit
    {
        IEnumerable<Ingredient> Ingredients { get; }
        void Add(Ingredient ingredient);
        bool HasEnough<T>(int requiredUnits) where T : Ingredient;
        void Use<T>(int units) where T : Ingredient;
    }
}