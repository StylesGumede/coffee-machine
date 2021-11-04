using System.Collections.Generic;
using CoffeeMachine.Core.BeverageMachine.Beverage.Specification;
using CoffeeMachine.Core.BeverageMachine.DispenserUnit;
using CoffeeMachine.Core.IngredientUnit;
using CoffeeMachine.Core.InputProcessing;

namespace CoffeeMachine.Core.BeverageMachine
{
    public interface IBeverageMachine
    {
        IEnumerable<BeverageSpecification> SupportedBeverageSpecifications { get; }
        IIngredientUnit IngredientUnit { get; }
        IInputCommandProcessor InputCommandProcessor { get; }
        IDispenserUnit DispenserUnit { get; }
        void Run();
        void ConfigureBeverageSpecifications(params BeverageSpecification[] specifications);
        void ClearBeverageSpecifications();
    }
}