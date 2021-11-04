using CoffeeMachine.Core.Notifications;

namespace CoffeeMachine.CoffeeMachine.Notifications
{
    public class NotEnoughSugarNotification : NotificationBase
    {
        public NotEnoughSugarNotification()
        {
            this.Message = "There's not enough sugar available. Sorry :(";
        }
    }
}