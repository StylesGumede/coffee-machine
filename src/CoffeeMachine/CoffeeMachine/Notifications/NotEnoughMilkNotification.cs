using CoffeeMachine.Core.Notifications;

namespace CoffeeMachine.CoffeeMachine.Notifications
{
    public class NotEnoughMilkNotification : NotificationBase
    {
        public NotEnoughMilkNotification()
        {
            this.Message = "There's not enough milk available. Sorry :(";
        }
    }
}