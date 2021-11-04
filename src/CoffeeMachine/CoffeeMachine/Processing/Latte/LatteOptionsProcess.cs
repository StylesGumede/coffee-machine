using CoffeeMachine.CoffeeMachine.Beverage.Specifications;
using CoffeeMachine.CoffeeMachine.Ingredients;
using CoffeeMachine.CoffeeMachine.Notifications;
using CoffeeMachine.Core.InputProcessing;
using CoffeeMachine.Core.Notifications;

namespace CoffeeMachine.CoffeeMachine.Processing.Latte
{
    public class LatteOptionsProcess : Process
    {
        public LatteOptionsProcess(INotificationController notificationController) : base(notificationController)
        {
        }

        public override void Execute(ProcessContext processContext)
        {
            if (!processContext.SelectedOption.Is<LatteSpecification>())
                return;

            int beanUnits = 3;

            if (!processContext.IngredientUnit.HasEnough<BeansIngredient>(beanUnits))
            {
                this.NotificationController.Notify<NotEnoughBeansNotification>();
                this.TerminateProcess();
            }

            processContext.SelectedOption.To<LatteSpecification>().BeanUnitsRequired = beanUnits;

            var yesNoOptions = new[] { "Yes", "No" };

            int milkUnits = 2;

            if (!processContext.IngredientUnit.HasEnough<MilkIngredient>(milkUnits))
            {
                this.NotificationController.Notify<NotEnoughMilkNotification>();
                this.TerminateProcess();
            }

            processContext.SelectedOption.To<LatteSpecification>().MilkUnitsRequired = milkUnits;

            this.NotificationController.Prompt(
                "Would you like sugar with your Latte?", yesNoOptions,
                option =>
                {
                    if (option == "No")
                    {
                        processContext.SelectedOption.To<LatteSpecification>().SugarUnitsRequired = 0;
                        return;
                    }

                    this.NotificationController.Prompt("How many units of sugar would you like with your Latte?",
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

                            processContext.SelectedOption.To<LatteSpecification>().SugarUnitsRequired = sugarUnits;
                        });
                });
        }
    }
}