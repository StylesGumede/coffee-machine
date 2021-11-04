using System.Linq;
using CoffeeMachine.Core.BeverageMachine.DispenserUnit;
using CoffeeMachine.Core.BeverageMachine.DispenserUnit.States;
using CoffeeMachine.Core.InputProcessing;
using CoffeeMachine.Core.Notifications;

namespace CoffeeMachine.Core.BeverageMachine.Processing
{
    public class CheckDispenseUnitProcess : Process
    {
        private readonly IDispenserUnit _dispenserUnit;

        public CheckDispenseUnitProcess(
            INotificationController notificationController,
            IDispenserUnit dispenserUnit) : base(notificationController)
        {
            _dispenserUnit = dispenserUnit;
        }

        public override void Execute(ProcessContext context)
        {
            this.NotificationController.Notify("Checking dispense unit...");

            if (this._dispenserUnit.State is TrayFull)
                this.NotificationController.Prompt(
                    this._dispenserUnit.State.Message,
                    this._dispenserUnit.Tray.ToArray(), beverage =>
                    {
                        this._dispenserUnit.Remove(beverage);
                    });
        }
    }
}