using CoffeeMachine.Core.Notifications;

namespace CoffeeMachine.CoffeeMachine.Notifications
{
    public class NotEnoughBeansNotification : NotificationBase
    {
        public NotEnoughBeansNotification()
        {
            this.Message = "There's not enough coffee beans available. Sorry :(";
        }
    }
}