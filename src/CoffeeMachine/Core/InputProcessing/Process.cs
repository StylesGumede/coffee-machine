using CoffeeMachine.Core.InputProcessing.Exceptions;
using CoffeeMachine.Core.Notifications;

namespace CoffeeMachine.Core.InputProcessing
{
    public abstract class Process
    {
        protected Process(
            INotificationController notificationController)
        {
            this.NotificationController = notificationController;
        }

        protected INotificationController NotificationController { get; }
        public string Name => this.GetType().Name;

        public void TerminateProcess() => throw new TerminateProcessException();
        public abstract void Execute(ProcessContext context);
    }
}