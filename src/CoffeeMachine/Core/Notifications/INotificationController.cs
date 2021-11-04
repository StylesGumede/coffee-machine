using System;

namespace CoffeeMachine.Core.Notifications
{
    public interface INotificationController
    {
        event Action<string> SpecialKeywordEntered; 
        void Notify(string message);
        void Notify<TNotification>() where TNotification : NotificationBase, new();
        void Prompt(string message, string[] options, Action<string> onOptionSelected);
        void Prompt<T>(string message, T[] options, Action<T> onOptionSelected) where T : IIdentifiable;
        void Notify(NotificationBase notification);
    }
}