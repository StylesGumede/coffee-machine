using CoffeeMachine.CoffeeMachine.Beverage.Specifications;
using CoffeeMachine.Core.BeverageMachine.DispenserUnit;
using CoffeeMachine.Core.BeverageMachine.Makers;
using CoffeeMachine.Core.InputProcessing;
using CoffeeMachine.Core.Notifications;

namespace CoffeeMachine.CoffeeMachine.Processing.Cappuccino
{
    public class CreateCappuccinoProcess : Process
    {
        private readonly IBeverageMaker<Beverage.Cappuccino, CappuccinoSpecification> _beverageMaker;
        private readonly IDispenserUnit _dispenserUnit;

        public CreateCappuccinoProcess(
            INotificationController notificationController,
            IBeverageMaker<Beverage.Cappuccino, CappuccinoSpecification> beverageMaker,
            IDispenserUnit dispenserUnit) : base(notificationController)
        {
            _beverageMaker = beverageMaker;
            _dispenserUnit = dispenserUnit;
        }

        public override void Execute(ProcessContext processContext)
        {
            if (!processContext.SelectedOption.Is<CappuccinoSpecification>())
                return;

            this.NotificationController.Notify<CreatingBeverageNotification>();

            this._beverageMaker.Make(
                processContext.SelectedOption.To<CappuccinoSpecification>(),
                onSuccess: beverage => this._dispenserUnit.Add(beverage),
                onFail: notification => this.NotificationController.Notify(notification));

            this.NotificationController.Notify<EnjoyYourBeverageNotification>();
        }
    }
}