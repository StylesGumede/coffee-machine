using CoffeeMachine.CoffeeMachine.Beverage.Specifications;
using CoffeeMachine.Core.BeverageMachine.DispenserUnit;
using CoffeeMachine.Core.BeverageMachine.Makers;
using CoffeeMachine.Core.InputProcessing;
using CoffeeMachine.Core.Notifications;

namespace CoffeeMachine.CoffeeMachine.Processing.Latte
{
    public class CreateLatteProcess : Process
    {
        private readonly IBeverageMaker<Beverage.Latte, LatteSpecification> _beverageMaker;
        private readonly IDispenserUnit _dispenserUnit;

        public CreateLatteProcess(
            INotificationController notificationController,
            IBeverageMaker<Beverage.Latte, LatteSpecification> beverageMaker,
            IDispenserUnit dispenserUnit) : base(notificationController)
        {
            _beverageMaker = beverageMaker;
            _dispenserUnit = dispenserUnit;
        }

        public override void Execute(ProcessContext processContext)
        {
            if (!processContext.SelectedOption.Is<LatteSpecification>())
                return;

            this.NotificationController.Notify<CreatingBeverageNotification>();

            this._beverageMaker.Make(
                processContext.SelectedOption.To<LatteSpecification>(),
                onSuccess: beverage => this._dispenserUnit.Add(beverage),
                onFail: notification => this.NotificationController.Notify(notification));

            this.NotificationController.Notify<EnjoyYourBeverageNotification>();
        }
    }
}