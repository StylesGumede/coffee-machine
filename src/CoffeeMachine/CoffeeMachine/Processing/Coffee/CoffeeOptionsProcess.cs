using CoffeeMachine.CoffeeMachine.Beverage.Specifications;
using CoffeeMachine.CoffeeMachine.Ingredients;
using CoffeeMachine.CoffeeMachine.Notifications;
using CoffeeMachine.Core.InputProcessing;
using CoffeeMachine.Core.Notifications;

namespace CoffeeMachine.CoffeeMachine.Processing.Coffee
{
    public class CoffeeOptionsProcess : Process
    {
        public CoffeeOptionsProcess(INotificationController notificationController) : base(notificationController)
        {
        }

        public override void Execute(ProcessContext processContext)
        {
            if (!processContext.SelectedOption.Is<CoffeeSpecification>())
                return;

            int beanUnits = 2;

            if (!processContext.IngredientUnit.HasEnough<BeansIngredient>(beanUnits))
            {
                this.NotificationController.Notify<NotEnoughBeansNotification>();
                this.TerminateProcess();
            }

            processContext.SelectedOption.To<CoffeeSpecification>().BeanUnitsRequired = beanUnits;

            var yesNoOptions = new[] { "Yes", "No" };

            this.NotificationController.Prompt(
                "Would you like milk with your Coffee?", yesNoOptions,
                option =>
                {
                    if (option == "No")
                    {
                        processContext.SelectedOption.To<CoffeeSpecification>().MilkUnitsRequired = 0;
                        return;
                    }

                    int milkUnits = 1;

                    if (!processContext.IngredientUnit.HasEnough<MilkIngredient>(milkUnits))
                    {
                        this.NotificationController.Notify<NotEnoughMilkNotification>();
                        this.TerminateProcess();
                    }

                    processContext.SelectedOption.To<CoffeeSpecification>().MilkUnitsRequired = milkUnits;
                });


            this.NotificationController.Prompt(
                "Would you like sugar with your Coffee?", yesNoOptions,
                option =>
                {
                    if (option == "No")
                    {
                        processContext.SelectedOption.To<CoffeeSpecification>().SugarUnitsRequired = 0;
                        return;
                    }

                    this.NotificationController.Prompt("How many units of sugar would you like with your Coffee?",
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

                            processContext.SelectedOption.To<CoffeeSpecification>().SugarUnitsRequired = sugarUnits;
                        });
                });
        }
    }
}