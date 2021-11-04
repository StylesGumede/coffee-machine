using System.Linq;
using CoffeeMachine.Core.InputProcessing;
using CoffeeMachine.Core.Notifications;

namespace CoffeeMachine.Core.BeverageMachine.Processing
{
    public class MenuOptionsProcess : Process
    {
        public MenuOptionsProcess(INotificationController notificationController) : base(notificationController)
        {
        }

        public override void Execute(ProcessContext processContext)
        {
            this.NotificationController.Notify("Hello there, let's make you a hot beverage!!");
            this.NotificationController.Prompt(
                "What type of Coffee would you like to have?",
                processContext.BeverageSpecifications.Select(x => x.DisplayName).ToArray(),
                userSelectedOption =>
                {
                    processContext.SelectedOption =
                        processContext.BeverageSpecifications.First(x => x.DisplayName == userSelectedOption);
                });
        }
    }
}