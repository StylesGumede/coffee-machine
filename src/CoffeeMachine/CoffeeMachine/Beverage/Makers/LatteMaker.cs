using System;
using CoffeeMachine.CoffeeMachine.Beverage.Specifications;
using CoffeeMachine.CoffeeMachine.Ingredients;
using CoffeeMachine.Core.BeverageMachine.Makers;
using CoffeeMachine.Core.IngredientUnit;
using CoffeeMachine.Core.Notifications;

namespace CoffeeMachine.CoffeeMachine.Beverage.Makers
{
    public class LatteMaker : BeverageMakerBase<Latte, LatteSpecification>
    {
        public LatteMaker(IIngredientUnit ingredientUnit) : base(ingredientUnit)
        {
        }

        protected override void OnMake(
            LatteSpecification beverageSpecification, 
            Action<Latte> onSuccess,
            Action<NotificationBase> onFail)
        {
            this.IngredientUnit.Use<BeansIngredient>(beverageSpecification.BeanUnitsRequired);
            this.IngredientUnit.Use<MilkIngredient>(beverageSpecification.MilkUnitsRequired);
            this.IngredientUnit.Use<SugarIngredient>(beverageSpecification.SugarUnitsRequired);

            onSuccess(new Latte(beverageSpecification.Clone<LatteSpecification>()));
        }
    }
    
    //Cappuccino
}