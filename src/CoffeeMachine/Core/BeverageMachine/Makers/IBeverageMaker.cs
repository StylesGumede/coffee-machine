using System;
using CoffeeMachine.Core.BeverageMachine.Beverage;
using CoffeeMachine.Core.BeverageMachine.Beverage.Specification;
using CoffeeMachine.Core.Notifications;

namespace CoffeeMachine.Core.BeverageMachine.Makers
{
    public interface IBeverageMaker<TBeverage, TBeverageSpecification> where TBeverage : BeverageBase
        where TBeverageSpecification : BeverageSpecification
    {
        void Make(TBeverageSpecification beverageSpecification, Action<TBeverage> onSuccess,
            Action<NotificationBase> onFail);
    }
}