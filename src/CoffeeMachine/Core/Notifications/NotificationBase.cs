namespace CoffeeMachine.Core.Notifications
{
    public abstract class NotificationBase
    {
        protected NotificationBase()
        {
        }

        public string Message { get; protected set; }
    }
}