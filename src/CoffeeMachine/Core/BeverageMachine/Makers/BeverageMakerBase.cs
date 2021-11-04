using System;
using CoffeeMachine.CoffeeMachine.Ingredients;
using CoffeeMachine.CoffeeMachine.Notifications;
using CoffeeMachine.Core.BeverageMachine.Beverage;
using CoffeeMachine.Core.BeverageMachine.Beverage.Specification;
using CoffeeMachine.Core.IngredientUnit;
using CoffeeMachine.Core.Notifications;

namespace CoffeeMachine.Core.BeverageMachine.Makers
{
    public abstract class
        BeverageMakerBase<TBeverage, TBeverageSpecification> : IBeverageMaker<TBeverage, TBeverageSpecification>
        where TBeverage : BeverageBase
        where TBeverageSpecification : BeverageSpecification
    {
        protected IIngredientUnit IngredientUnit;

        protected BeverageMakerBase(IIngredientUnit ingredientUnit)
        {
            IngredientUnit = ingredientUnit;
        }

        public void Make(TBeverageSpecification beverageSpecification, Action<TBeverage> onSuccess,
            Action<NotificationBase> onFail)
        {
            if (beverageSpecification == null) throw new ArgumentNullException(nameof(beverageSpecification));
            if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
            if (onFail == null) throw new ArgumentNullException(nameof(onFail));

            if (!this.IngredientUnit.HasEnough<BeansIngredient>(beverageSpecification.BeanUnitsRequired))
            {
                onFail(new NotEnoughBeansNotification());
                return;
            }

            if (!this.IngredientUnit.HasEnough<SugarIngredient>(beverageSpecification.SugarUnitsRequired))
            {
                onFail(new NotEnoughSugarNotification());
                return;
            }

            if (!this.IngredientUnit.HasEnough<MilkIngredient>(beverageSpecification.MilkUnitsRequired))
            {
                onFail(new NotEnoughMilkNotification());
                return;
            }
            
            this.OnMake(beverageSpecification, onSuccess, onFail);
        }

        protected abstract void OnMake(TBeverageSpecification beverageSpecification, Action<TBeverage> onSuccess,
            Action<NotificationBase> onFail);
    }
}