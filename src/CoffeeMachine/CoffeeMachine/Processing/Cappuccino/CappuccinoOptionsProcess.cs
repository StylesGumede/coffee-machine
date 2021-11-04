using CoffeeMachine.CoffeeMachine.Beverage.Specifications;
using CoffeeMachine.CoffeeMachine.Ingredients;
using CoffeeMachine.CoffeeMachine.Notifications;
using CoffeeMachine.Core.InputProcessing;
using CoffeeMachine.Core.Notifications;

namespace CoffeeMachine.CoffeeMachine.Processing.Cappuccino
{
    public class CappuccinoOptionsProcess : Process
    {
        public CappuccinoOptionsProcess(INotificationController notificationController) : base(notificationController)
        {
        }

        public override void Execute(ProcessContext processContext)
        {
            if (!processContext.SelectedOption.Is<CappuccinoSpecification>())
                return;

            int beanUnits = 5;

            if (!processContext.IngredientUnit.HasEnough<BeansIngredient>(beanUnits))
            {
                this.NotificationController.Notify<NotEnoughBeansNotification>();
                this.TerminateProcess();
            }

            processContext.SelectedOption.To<CappuccinoSpecification>().BeanUnitsRequired = beanUnits;

            int milkUnits = 3;

            if (!processContext.IngredientUnit.HasEnough<MilkIngredient>(milkUnits))
            {
                this.NotificationController.Notify<NotEnoughMilkNotification>();
                this.TerminateProcess();
            }

            processContext.SelectedOption.To<CappuccinoSpecification>().MilkUnitsRequired = milkUnits;

            this.NotificationController.Prompt("How many units of sugar would you like with your Cappuccino?",
                new[] { "1", "2", "3" },
                choice =>
                {
                    int sugarUnits = int.Parse(choice);

                    if (!processContext.IngredientUnit.HasEnough<SugarIngredient>(sugarUnits))
                    {
                        this.NotificationController.Notify<NotEnoughSugarNotification>();
                        this.TerminateProcess();
                        return;
                    }

                    processContext.SelectedOption.To<CappuccinoSpecification>().SugarUnitsRequired = sugarUnits;
                });
        }
    }
}