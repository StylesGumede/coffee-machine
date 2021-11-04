namespace CoffeeMachine.Core.Notifications
{
    public class CreatingBeverageNotification: NotificationBase
    {
        public CreatingBeverageNotification()
        {
            this.Message = "Hold on...busy creating your delicious beverage...";
        }
    }
}