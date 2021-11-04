using System.Collections.Generic;
using CoffeeMachine.Core.BeverageMachine.Beverage.Specification;
using CoffeeMachine.Core.IngredientUnit;

namespace CoffeeMachine.Core.InputProcessing
{
    public class ProcessContext
    {
        public ProcessContext(IEnumerable<BeverageSpecification> beverageSpecifications, IIngredientUnit ingredientUnit)
        {
            this.BeverageSpecifications = beverageSpecifications;
            IngredientUnit = ingredientUnit;
            this.SelectedOption = new NoSpecification();
        }

        public IEnumerable<BeverageSpecification> BeverageSpecifications { get; }
        public BeverageSpecification SelectedOption { get; set; }
        public IIngredientUnit IngredientUnit { get; }
    }
}