using CoffeeMachine.CoffeeMachine.Beverage.Specifications;
using CoffeeMachine.Core.BeverageMachine.DispenserUnit;
using CoffeeMachine.Core.BeverageMachine.Makers;
using CoffeeMachine.Core.InputProcessing;
using CoffeeMachine.Core.Notifications;

namespace CoffeeMachine.CoffeeMachine.Processing.Coffee
{
    public class CreateCoffeeProcess : Process
    {
        private readonly IBeverageMaker<Beverage.Coffee, CoffeeSpecification> _beverageMaker;
        private readonly IDispenserUnit _dispenserUnit;

        public CreateCoffeeProcess(
            INotificationController notificationController,
            IBeverageMaker<Beverage.Coffee, CoffeeSpecification> beverageMaker,
            IDispenserUnit dispenserUnit) : base(notificationController)
        {
            _beverageMaker = beverageMaker;
            _dispenserUnit = dispenserUnit;
        }

        public override void Execute(ProcessContext processContext)
        {
            if (!processContext.SelectedOption.Is<CoffeeSpecification>())
                return;

            this.NotificationController.Notify<CreatingBeverageNotification>();

            this._beverageMaker.Make(
                processContext.SelectedOption.To<CoffeeSpecification>(),
                onSuccess: beverage => this._dispenserUnit.Add(beverage),
                onFail: notification => this.NotificationController.Notify(notification));
            
            this.NotificationController.Notify<EnjoyYourBeverageNotification>();
        }
    }
}