using System;
using System.Collections.Generic;
using System.Linq;

namespace CoffeeMachine.Core.IngredientUnit
{
    public class IngredientUnit : IIngredientUnit
    {
        private readonly List<Ingredient> _ingredients = new List<Ingredient>();

        public IEnumerable<Ingredient> Ingredients => this._ingredients;

        public void Add(Ingredient ingredient)
        {
            if (ingredient == null)
                throw new ArgumentNullException(nameof(ingredient));

            if (this._ingredients.Any(x => x.GetType() == ingredient.GetType()))
                throw new Exception($"Ingredient {ingredient.GetType().Name} already configured.");

            this._ingredients.Add(ingredient);
        }

        public bool HasEnough<T>(int requiredUnits) where T : Ingredient
        {
            var ingredient = this._ingredients.FirstOrDefault(x => x.GetType() == typeof(T)) ??
                             throw new InvalidOperationException(
                                 $"Ingredient with name '{typeof(T).Name}' not configured");

            return ingredient.CanSatisfy(requiredUnits);
        }

        public void Use<T>(int units) where T : Ingredient
        {
            var ingredient = this._ingredients.FirstOrDefault(x => x.GetType() == typeof(T)) ??
                             throw new InvalidOperationException(
                                 $"Ingredient with name '{typeof(T).Name}' not configured");

            ingredient.Use(units);
        }
    }
}