using System;
using CoffeeMachine.CoffeeMachine.Beverage.Specifications;
using CoffeeMachine.CoffeeMachine.Ingredients;
using CoffeeMachine.Core.BeverageMachine.Makers;
using CoffeeMachine.Core.IngredientUnit;
using CoffeeMachine.Core.Notifications;

namespace CoffeeMachine.CoffeeMachine.Beverage.Makers
{
    public class CappuccinoMaker : BeverageMakerBase<Cappuccino, CappuccinoSpecification>
    {
        public CappuccinoMaker(IIngredientUnit ingredientUnit) : base(ingredientUnit)
        {
        }

        protected override void OnMake(
            CappuccinoSpecification beverageSpecification, 
            Action<Cappuccino> onSuccess,
            Action<NotificationBase> onFail)
        {
            this.IngredientUnit.Use<BeansIngredient>(beverageSpecification.BeanUnitsRequired);
            this.IngredientUnit.Use<MilkIngredient>(beverageSpecification.MilkUnitsRequired);
            this.IngredientUnit.Use<SugarIngredient>(beverageSpecification.SugarUnitsRequired);

            onSuccess(new Cappuccino(beverageSpecification.Clone<CappuccinoSpecification>()));
        }
    }
}